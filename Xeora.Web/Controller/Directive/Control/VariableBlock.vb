Option Strict On
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive.Control
    Public Class VariableBlock
        Inherits ControlBase
        Implements IInstanceRequires

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            ' UpdateBlock can be located under a variable block because of this
            ' Variable block should be rendered with its current settings all the time

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
            Dim ContentDescription As [Global].ContentDescription =
                New [Global].ContentDescription(Me.InsideValue)

            Dim BlockContent As String =
                ContentDescription.Parts.Item(0)

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
                                                        New IInstanceRequires.InstanceRequestedEventHandler(
                                                            Sub(ByRef Instance As IDomain)
                                                                RaiseEvent InstanceRequested(Instance)
                                                            End Sub)
                                                    )
                    End Sub)
            )

            Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                Manager.Assembly.InvokeBind(Me.BindInfo, Manager.Assembly.ExecuterTypes.Control)

            Dim VariableBlockResult As ControlResult.VariableBlock = Nothing

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
                    VariableBlockResult = CType(BindInvokeResult.InvokeResult, ControlResult.VariableBlock)
                End If
            End If
            ' ----

            If Not VariableBlockResult Is Nothing Then
                For Each Key As String In VariableBlockResult.Keys
                    Me.ContentArguments.AppendKeyWithValue(Key, VariableBlockResult.Item(Key))
                Next
            End If

            Me.RequestParse(BlockContent, Me)

            Me.DefineRenderedValue(Me.Create())

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class
End Namespace