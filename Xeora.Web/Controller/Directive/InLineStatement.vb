Option Strict On

Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class InLineStatement
        Inherits DirectiveControllerBase
        Implements IParsingRequires
        Implements IInstanceRequires
        Implements INamable
        Implements IBoundable

        Private _ControlID As String
        Private _BoundControlID As String

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.InLineStatement, ContentArguments)

            Me._ControlID = Me.CaptureControlID()
            Me._BoundControlID = Me.CaptureBoundControlID()
        End Sub

        Public ReadOnly Property ControlID As String Implements INamable.ControlID
            Get
                Return Me._ControlID
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
            Dim ContentDescription As [Global].ContentDescription =
                New [Global].ContentDescription(Me.InsideValue)

            Dim BlockContent As String =
                ContentDescription.Parts.Item(0)

            Dim NoCacheMarker As String = "!NOCACHE", NoCache As Boolean = False

            If BlockContent.IndexOf(NoCacheMarker) = 0 Then _
                NoCache = True : BlockContent = BlockContent.Substring(NoCacheMarker.Length)

            BlockContent = BlockContent.Trim()

            If String.IsNullOrEmpty(BlockContent) Then _
                Throw New Exception.EmptyBlockException()

            ' InLineStatement does not have any ContentArguments, That's why it copies it's parent Arguments
            If Not Me.Parent Is Nothing Then _
                Me.ContentArguments.Replace(Me.Parent.ContentArguments)

            RaiseEvent ParseRequested(BlockContent, Me)

            BlockContent = Me.Create()

            Dim Instance As IDomain = Nothing
            RaiseEvent InstanceRequested(Instance)

            Dim MethodResultInfo As Object =
                Manager.Assembly.ExecuteStatement(Instance.IDAccessTree, Me.ControlID, BlockContent.Trim(), NoCache)

            If Not MethodResultInfo Is Nothing AndAlso TypeOf MethodResultInfo Is System.Exception Then
                Throw New Exception.ExecutionException(
                        CType(MethodResultInfo, System.Exception).Message,
                        CType(MethodResultInfo, System.Exception).InnerException
                    )
            End If

            If Not MethodResultInfo Is Nothing Then
                Dim RenderResult As String = String.Empty

                If TypeOf MethodResultInfo Is ControlResult.RedirectOrder Then
                    Helpers.Context.Content.Item("RedirectLocation") =
                        CType(MethodResultInfo, ControlResult.RedirectOrder).Location
                Else
                    RenderResult = [Shared].Execution.GetPrimitiveValue(MethodResultInfo)
                End If

                Me.DefineRenderedValue(RenderResult)
            Else
                Me.DefineRenderedValue(String.Empty)
            End If
        End Sub
    End Class
End Namespace