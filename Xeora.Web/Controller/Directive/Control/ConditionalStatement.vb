Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class ConditionalStatement
        Inherits ControlBase
        Implements IInstanceRequires

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            ' UpdateBlock can be located under a conditional statement because of this
            ' Conditional statement should be rendered with its current settings all the time

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

            ' ConditionalStatment does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

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

                Dim ContentTrue As String = Nothing, ContentFalse As String = Nothing

                CoreContent = BlockContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                Dim ContentDescription As [Global].ContentDescription =
                    New [Global].ContentDescription(CoreContent, ControlIDWithIndex)

                If ContentDescription.HasParts Then
                    ContentTrue = ContentDescription.Parts.Item(0)

                    If ContentDescription.Parts.Count > 1 Then _
                        ContentFalse = ContentDescription.Parts.Item(1)

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

                    Dim ConditionResult As ControlResult.Conditional = Nothing

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
                            ConditionResult = CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.Conditional)
                        End If
                    End If
                    ' ----

                    ' if ConditionResult is not nothing, Render Results
                    If Not ConditionResult Is Nothing Then
                        Select Case ConditionResult.Result
                            Case ControlResult.Conditional.Conditions.True
                                If Not String.IsNullOrEmpty(ContentTrue) Then _
                                    Me.RequestParse(ContentTrue, Me)

                            Case ControlResult.Conditional.Conditions.False
                                If Not String.IsNullOrEmpty(ContentFalse) Then _
                                    Me.RequestParse(ContentFalse, Me)

                            Case ControlResult.Conditional.Conditions.UnKnown
                                ' Reserved For Future Uses
                        End Select

                        Me.DefineRenderedValue(Me.Create())
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