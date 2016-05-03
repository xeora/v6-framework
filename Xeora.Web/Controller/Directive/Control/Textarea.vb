Option Strict On

Namespace Xeora.Web.Controller.Directive.Control
    Public Class Textarea
        Inherits ControlBase
        Implements IHasContent

        Private _Content As String

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfo.ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControlTypes.Textarea, ContentArguments)

            Me._Content = String.Empty
        End Sub

        Public Property Content() As String Implements IHasContent.Content
            Get
                Return Me._Content
            End Get
            Set(ByVal Value As String)
                Me._Content = Value
                If String.IsNullOrEmpty(Me._Content) Then Me._Content = String.Empty
            End Set
        End Property

        Public Overrides Sub Clone(ByRef Control As IControl)
            Control = New Textarea(Me.DraftStartIndex, Me.DraftValue, Me.ContentArguments)
            MyBase.Clone(Control)

            With CType(Control, Textarea)
                ._Content = Me._Content
            End With
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
            ' Render Text Content
            Dim DummyControllerContainer As ControllerBase =
                ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
            Me.RequestParse(Me.Content, DummyControllerContainer)
            DummyControllerContainer.Render(Me)
            Me.Content = DummyControllerContainer.RenderedValue
            ' !--

            ' NO BIND REQUIRED FOR TEXTAREA CONTROL

            ' Render Attributes
            For aC As Integer = Me.Attributes.Count - 1 To 0 Step -1
                Dim Item As AttributeInfo = Me.Attributes.Item(aC)

                DummyControllerContainer = ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
                Me.RequestParse(Item.Value, DummyControllerContainer)
                DummyControllerContainer.Render(Me)

                Me.Attributes.Item(aC) = New AttributeInfo(Item.Key, DummyControllerContainer.RenderedValue)
            Next
            ' !--

            If Me.SecurityInfo.Disabled.IsSet AndAlso
                Me.SecurityInfo.Disabled.Type = SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                Me.DefineRenderedValue(Me.SecurityInfo.Disabled.Value)
            Else
                Me.DefineRenderedValue(
                    String.Format(
                        "<textarea name=""{0}"" id=""{0}""{1}>{2}</textarea>",
                        Me.ControlID,
                        Attributes.ToString(),
                        Me.Content
                    )
                )
            End If

            Me.UnRegisterFromRenderCompletedOf(Me.BoundControlID)
        End Sub
    End Class
End Namespace