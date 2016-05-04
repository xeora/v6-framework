Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class Execution
        Inherits DirectiveControllerBase
        Implements ILevelable
        Implements IBoundable
        Implements IInstanceRequires

        Private _Leveling As Integer
        Private _BoundControlID As String

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.Execution, ContentArguments)

            Me._Leveling = Me.CaptureLeveling()
            Me._BoundControlID = Me.CaptureBoundControlID()
        End Sub

        Public ReadOnly Property Level As Integer Implements ILevelable.Level
            Get
                Return Me._Leveling
            End Get
        End Property

        Public ReadOnly Property BoundControlID As String Implements IBoundable.BoundControlID
            Get
                Return Me._BoundControlID
            End Get
        End Property

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
            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)

            Dim ControllerLevel As ControllerBase = Me
            Dim Leveling As Integer = Me._Leveling

            Do
                If Leveling > 0 Then
                    ControllerLevel = ControllerLevel.Parent

                    If TypeOf ControllerLevel Is RenderlessController Then _
                        ControllerLevel = ControllerLevel.Parent

                    Leveling -= 1
                End If
            Loop Until ControllerLevel Is Nothing OrElse Leveling = 0

            Dim BindInfo As [Shared].Execution.BindInfo =
                [Shared].Execution.BindInfo.Make(
                    String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1))

            BindInfo.PrepareProcedureParameters(
                        New [Shared].Execution.BindInfo.ProcedureParser(
                            Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                ProcedureParameter.Value = PropertyController.ParseProperty(
                                                               ProcedureParameter.Query,
                                                               Me,
                                                               CType(IIf(ControllerLevel.Parent Is Nothing, Nothing, ControllerLevel.Parent.ContentArguments), [Global].ArgumentInfoCollection),
                                                               New IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As IDomain)
                                                                                                                       RaiseEvent InstanceRequested(Instance)
                                                                                                                   End Sub)
                                                           )
                            End Sub)
                    )

            Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                Manager.Assembly.InvokeBind(BindInfo, Manager.Assembly.ExecuterTypes.Other)

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
                    If Not BindInvokeResult.InvokeResult Is Nothing AndAlso
                        TypeOf BindInvokeResult.InvokeResult Is ControlResult.RedirectOrder Then

                        If Helpers.Context.Items.Contains("RedirectLocation") Then _
                            Helpers.Context.Items.Remove("RedirectLocation")

                        Helpers.Context.Items.Add(
                            "RedirectLocation",
                            CType(BindInvokeResult.InvokeResult, ControlResult.RedirectOrder).Location
                        )

                        Me.DefineRenderedValue(String.Empty)
                    Else
                        Me.DefineRenderedValue(
                            [Shared].Execution.GetPrimitiveValue(BindInvokeResult.InvokeResult)
                        )
                    End If
                End If
            End If

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class

End Namespace
