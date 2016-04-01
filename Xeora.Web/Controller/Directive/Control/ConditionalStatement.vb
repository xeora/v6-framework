Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class ConditionalStatement
        Inherits ControlBase
        Implements IInstanceRequires

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfo.ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControlTypes.ConditionalStatement, ContentArguments)
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

                    Me.RegisterToRenderCompleted()
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

                Dim ContentTrue As String = Nothing, ContentFalse As String = Nothing

                CoreContent = BlockContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                Dim ContentCollection As String() = Me.SplitContentByControlIDWithIndex(CoreContent, ControlIDWithIndex)

                If ContentCollection.Length > 0 Then
                    For mC As Integer = 0 To ContentCollection.Length - 1
                        Select Case mC
                            Case 0
                                ContentTrue = ContentCollection(mC)
                            Case 1
                                ContentFalse = ContentCollection(mC)
                        End Select
                    Next

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

                    Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                        Manager.Assembly.InvokeBind(
                            Me.BindInfo,
                            PropertyController.ParseProperties(Me, ControllerLevel.ContentArguments, Me.BindInfo.ProcedureParams, New IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As IDomain)
                                                                                                                                                                                          RaiseEvent InstanceRequested(Instance)
                                                                                                                                                                                      End Sub)),
                            Manager.Assembly.ExecuterTypes.Control
                        )

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

            Me.UnRegisterFromRenderCompleted()
        End Sub
    End Class
End Namespace