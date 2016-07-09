Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class DataList
        Inherits ControlBase
        Implements IInstanceRequires

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControlTypes.DataList, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            If Not String.IsNullOrEmpty(Me.BoundControlID) Then
                If Me.IsRendered Then Exit Sub

                If Not Me.BoundControlRenderWaiting Then
                    Dim Controller As ControllerBase = Me

                    Do Until Controller.Parent Is Nothing
                        If TypeOf Controller.Parent Is ControllerBase AndAlso
                            TypeOf Controller.Parent Is INamable Then

                            If String.Compare(
                                CType(Controller.Parent, INamable).ControlID, Me.BoundControlID, True) = 0 Then

                                Throw New Exception.InternalParentException(Exception.InternalParentException.ChildDirectiveTypes.Control)
                            End If
                        End If

                        Controller = Controller.Parent
                    Loop

                    Me.RegisterToRenderCompletedOf(Me.BoundControlID)
                End If

                If TypeOf SenderController Is ControlBase AndAlso
                    TypeOf SenderController Is INamable Then

                    If String.Compare(
                        CType(SenderController, INamable).ControlID, Me.BoundControlID, True) <> 0 Then

                        Exit Sub
                    Else
                        Me.RenderInternal()
                    End If
                End If
            Else
                Me.RenderInternal()
            End If
        End Sub

        Private Sub RenderInternal()
            ' Parse Block Content
            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)
            Dim BlockContent As String = String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1)

            ' Check This Control has a Content
            Dim idxCon As Integer = BlockContent.IndexOf(":"c)

            ' Get ControlID Accourding to idxCon Value -1 = no content, else has content
            If idxCon = -1 Then
                ' No Content

                Throw New Exception.GrammerException()
            End If

            ' ControlIDWithIndex Like ControlID~INDEX
            Dim ControlIDWithIndex As String = BlockContent.Substring(0, idxCon)

            Dim CoreContent As String = Nothing
            Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

            Dim OpeningTag As String = String.Format("{0}:{{", ControlIDWithIndex)
            Dim ClosingTag As String = String.Format("}}:{0}", ControlIDWithIndex)

            idxCoreContStart = BlockContent.IndexOf(OpeningTag) + OpeningTag.Length
            idxCoreContEnd = BlockContent.LastIndexOf(ClosingTag, BlockContent.Length)

            If idxCoreContStart = OpeningTag.Length AndAlso
                idxCoreContEnd = (BlockContent.Length - OpeningTag.Length) Then

                CoreContent = BlockContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                Dim ContentCollection As String() = Me.SplitContentByControlIDWithIndex(CoreContent, ControlIDWithIndex)

                If ContentCollection.Length > 0 Then
                    ' Catch Error Template Block (Is Exists)
                    Dim ReContentCollection As New Generic.List(Of String)
                    Dim MessageTemplate As String = String.Empty, TemplatePointerText As String = "!MESSAGETEMPLATE"

                    For cC As Integer = 0 To ContentCollection.Length - 1
                        If ContentCollection(cC).IndexOf(TemplatePointerText) = 0 Then
                            If String.IsNullOrEmpty(MessageTemplate) Then
                                MessageTemplate = ContentCollection(cC).Substring(TemplatePointerText.Length)
                            Else
                                Throw New Exception.MultipleBlockException("Only One Message Template Block Allowed for a DataList Control!")
                            End If
                        Else
                            ReContentCollection.Add(ContentCollection(cC))
                        End If
                    Next
                    ContentCollection = ReContentCollection.ToArray()
                    ' ---- 

                    ' Reset Variables
                    Helpers.VariablePool.Set(Me.ControlID, Nothing)

                    ' Call Related Function and Exam It
                    Dim ControllerLevel As ControllerBase = Me
                    Dim Leveling As Integer = Me.Level

                    Do
                        If Leveling > 0 Then
                            ControllerLevel = ControllerLevel.Parent

                            If TypeOf ControllerLevel Is RenderlessController Then _
                                ControllerLevel = ControllerLevel.Parent

                            Leveling -= 1
                        End If
                    Loop Until ControllerLevel Is Nothing OrElse Leveling = 0

                    ' Execution preparation should be done at the same level with it's parent. Because of that, send parent as parameters
                    Me.BindInfo.PrepareProcedureParameters(
                        New [Shared].Execution.BindInfo.ProcedureParser(
                            Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                ProcedureParameter.Value = PropertyController.ParseProperty(
                                                               ProcedureParameter.Query,
                                                               ControllerLevel.Parent,
                                                               CType(IIf(ControllerLevel.Parent Is Nothing, Nothing, ControllerLevel.Parent.ContentArguments), [Global].ArgumentInfoCollection),
                                                               New IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As IDomain)
                                                                                                                       RaiseEvent InstanceRequested(Instance)
                                                                                                                   End Sub)
                                                           )
                            End Sub)
                    )

                    Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                        Manager.Assembly.InvokeBind(Me.BindInfo, Manager.Assembly.ExecuterTypes.Control)

                    If BindInvokeResult.ReloadRequired Then
                        Throw New Exception.ReloadRequiredException(BindInvokeResult.ApplicationPath)
                    Else
                        If Not BindInvokeResult.InvokeResult Is Nothing AndAlso
                            TypeOf BindInvokeResult.InvokeResult Is System.Exception Then

                            Throw New Exception.ExecutionException(
                                CType(BindInvokeResult.InvokeResult, System.Exception).Message,
                                CType(BindInvokeResult.InvokeResult, System.Exception).InnerException
                            )
                        Else
                            If BindInvokeResult.InvokeResult Is Nothing OrElse
                                (
                                    Not TypeOf BindInvokeResult.InvokeResult Is ControlResult.PartialDataTable AndAlso
                                    Not TypeOf BindInvokeResult.InvokeResult Is ControlResult.DirectDataAccess
                                ) Then

                                Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(0, 0))
                            Else
                                Dim CompareCulture As New Globalization.CultureInfo("en-US")

                                If TypeOf BindInvokeResult.InvokeResult Is ControlResult.PartialDataTable Then
                                    Dim RepeaterList As ControlResult.PartialDataTable =
                                        CType(BindInvokeResult.InvokeResult, ControlResult.PartialDataTable)

                                    Dim DataListArgs As New [Global].ArgumentInfoCollection

                                    If Not RepeaterList.Message Is Nothing Then
                                        Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(0, 0))

                                        If String.IsNullOrEmpty(MessageTemplate) Then
                                            Me.DefineRenderedValue(RepeaterList.Message.Message)
                                        Else
                                            DataListArgs.AppendKeyWithValue("MessageType", RepeaterList.Message.Type)
                                            DataListArgs.AppendKeyWithValue("Message", RepeaterList.Message.Message)

                                            Dim DummyControllerContainer As ControllerBase =
                                                ControllerBase.ProvideDummyController(Me, DataListArgs)
                                            Me.RequestParse(MessageTemplate, DummyControllerContainer)
                                            DummyControllerContainer.Render(Me)

                                            Me.DefineRenderedValue(DummyControllerContainer.RenderedValue)
                                        End If
                                    Else
                                        Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(RepeaterList.Rows.Count, RepeaterList.Total))

                                        Dim RenderedContent As New Text.StringBuilder
                                        Dim ContentIndex As Integer = 0, rC As Integer = 0
                                        Dim IsItemIndexColumnExists As Boolean = False

                                        For Each dC As DataColumn In RepeaterList.Columns
                                            If CompareCulture.CompareInfo.Compare(dC.ColumnName, "ItemIndex", Globalization.CompareOptions.IgnoreCase) = 0 Then IsItemIndexColumnExists = True

                                            DataListArgs.AppendKey(dC.ColumnName)
                                        Next
                                        DataListArgs.AppendKey("_sys_ItemIndex") : RepeaterList.Columns.Add("_sys_ItemIndex", GetType(Integer))
                                        ' this is for user interaction
                                        If Not IsItemIndexColumnExists Then DataListArgs.AppendKey("ItemIndex") : RepeaterList.Columns.Add("ItemIndex", GetType(Integer))

                                        For Each dR As DataRow In RepeaterList.Rows
                                            Dim dRValues As Object() = dR.ItemArray

                                            If Not IsItemIndexColumnExists Then
                                                dRValues(dRValues.Length - 2) = rC
                                                dRValues(dRValues.Length - 1) = rC
                                            Else
                                                dRValues(dRValues.Length - 1) = rC
                                            End If

                                            DataListArgs.Reset(dRValues)

                                            ContentIndex = rC Mod ContentCollection.Length

                                            Dim DummyControllerContainer As ControllerBase =
                                                ControllerBase.ProvideDummyController(Me, DataListArgs)
                                            Me.RequestParse(ContentCollection(ContentIndex), DummyControllerContainer)
                                            DummyControllerContainer.Render(Me)

                                            RenderedContent.Append(DummyControllerContainer.RenderedValue)

                                            rC += 1
                                        Next

                                        Me.DefineRenderedValue(RenderedContent.ToString())
                                    End If
                                ElseIf TypeOf BindInvokeResult.InvokeResult Is ControlResult.DirectDataAccess Then
                                    Dim DataReaderInfo As ControlResult.DirectDataAccess =
                                        CType(BindInvokeResult.InvokeResult, ControlResult.DirectDataAccess)

                                    Dim DataListArgs As New [Global].ArgumentInfoCollection

                                    If DataReaderInfo.DatabaseCommand Is Nothing Then
                                        If Not DataReaderInfo.Message Is Nothing Then
                                            Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(0, 0))

                                            If String.IsNullOrEmpty(MessageTemplate) Then
                                                Me.DefineRenderedValue(DataReaderInfo.Message.Message)
                                            Else
                                                DataListArgs.AppendKeyWithValue("MessageType", DataReaderInfo.Message.Type)
                                                DataListArgs.AppendKeyWithValue("Message", DataReaderInfo.Message.Message)

                                                Dim DummyControllerContainer As ControllerBase =
                                                    ControllerBase.ProvideDummyController(Me, DataListArgs)
                                                Me.RequestParse(MessageTemplate, DummyControllerContainer)
                                                DummyControllerContainer.Render(Me)

                                                Me.DefineRenderedValue(DummyControllerContainer.RenderedValue)
                                            End If

                                            Helper.EventLogging.WriteToLog(
                                                String.Format("DirectDataAccess [{0}] failed! DatabaseCommand must not be null!", Me.ControlID))
                                        Else
                                            Throw New NullReferenceException("DirectDataAccess failed! DatabaseCommand must not be null!")
                                        End If
                                    Else
                                        Dim DBConnection As IDbConnection =
                                            DataReaderInfo.DatabaseCommand.Connection
                                        Dim DBCommand As IDbCommand =
                                            DataReaderInfo.DatabaseCommand
                                        Dim DBReader As IDataReader

                                        Try
                                            DBConnection.Open()
                                            DBReader = DBCommand.ExecuteReader()

                                            Dim RenderedContent As New Text.StringBuilder
                                            Dim ContentIndex As Integer = 0, rC As Integer = 0
                                            Dim IsItemIndexColumnExists As Boolean = False

                                            If DBReader.Read() Then
                                                Do
                                                    DataListArgs.Reset()

                                                    For cC As Integer = 0 To DBReader.FieldCount - 1
                                                        If CompareCulture.CompareInfo.Compare(DBReader.GetName(cC), "ItemIndex", Globalization.CompareOptions.IgnoreCase) = 0 Then IsItemIndexColumnExists = True

                                                        DataListArgs.AppendKeyWithValue(DBReader.GetName(cC), DBReader.GetValue(cC))
                                                    Next
                                                    DataListArgs.AppendKeyWithValue("_sys_ItemIndex", rC)
                                                    ' this is for user interaction
                                                    If Not IsItemIndexColumnExists Then DataListArgs.AppendKeyWithValue("ItemIndex", rC)

                                                    ContentIndex = rC Mod ContentCollection.Length

                                                    Dim DummyControllerContainer As ControllerBase =
                                                        ControllerBase.ProvideDummyController(Me, DataListArgs)
                                                    Me.RequestParse(ContentCollection(ContentIndex), DummyControllerContainer)
                                                    DummyControllerContainer.Render(Me)

                                                    RenderedContent.Append(DummyControllerContainer.RenderedValue)

                                                    rC += 1
                                                Loop While DBReader.Read()

                                                Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(rC, rC))
                                                Me.DefineRenderedValue(RenderedContent.ToString())
                                            Else
                                                Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(0, 0))

                                                If Not DataReaderInfo.Message Is Nothing Then
                                                    If String.IsNullOrEmpty(MessageTemplate) Then
                                                        Me.DefineRenderedValue(DataReaderInfo.Message.Message)
                                                    Else
                                                        DataListArgs.AppendKeyWithValue("MessageType", DataReaderInfo.Message.Type)
                                                        DataListArgs.AppendKeyWithValue("Message", DataReaderInfo.Message.Message)

                                                        Dim DummyControllerContainer As ControllerBase =
                                                            ControllerBase.ProvideDummyController(Me, DataListArgs)
                                                        Me.RequestParse(MessageTemplate, DummyControllerContainer)
                                                        DummyControllerContainer.Render(Me)

                                                        Me.DefineRenderedValue(DummyControllerContainer.RenderedValue)
                                                    End If
                                                Else
                                                    Me.DefineRenderedValue(String.Empty)
                                                End If
                                            End If

                                            ' Close and Dispose Database Reader
                                            DBReader.Close()
                                            DBReader.Dispose()
                                            GC.SuppressFinalize(DBReader)
                                            ' ----

                                            ' Close and Dispose Database Command
                                            DBCommand.Dispose()
                                            GC.SuppressFinalize(DBCommand)
                                            ' ----

                                            ' Close and Dispose Database Connection
                                            DBConnection.Close()
                                            DBConnection.Dispose()
                                            GC.SuppressFinalize(DBConnection)
                                            ' ----
                                        Catch ex As System.Exception
                                            If Not DataReaderInfo.Message Is Nothing Then
                                                Helpers.VariablePool.Set(Me.ControlID, New [Global].DataListOutputInfo(0, 0))

                                                If String.IsNullOrEmpty(MessageTemplate) Then
                                                    Me.DefineRenderedValue(DataReaderInfo.Message.Message)
                                                Else
                                                    DataListArgs.AppendKeyWithValue("MessageType", DataReaderInfo.Message.Type)
                                                    DataListArgs.AppendKeyWithValue("Message", DataReaderInfo.Message.Message)

                                                    Dim DummyControllerContainer As ControllerBase =
                                                        ControllerBase.ProvideDummyController(Me, DataListArgs)
                                                    Me.RequestParse(MessageTemplate, DummyControllerContainer)
                                                    DummyControllerContainer.Render(Me)

                                                    Me.DefineRenderedValue(DummyControllerContainer.RenderedValue)
                                                End If

                                                Helper.EventLogging.WriteToLog(ex)
                                            Else
                                                Throw New Exception.DirectDataAccessException(ex)
                                            End If
                                        End Try
                                    End If
                                End If
                            End If
                        End If
                    End If
                    ' ----
                Else
                    Throw New Exception.ParseException()
                End If
            Else
                Throw New Exception.ParseException()
            End If

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class
End Namespace