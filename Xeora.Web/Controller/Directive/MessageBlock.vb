Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Class MessageBlock
        Inherits DirectiveControllerBase
        Implements IParsingRequires

        Public Event ParseRequested(ByVal DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.MessageBlock, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            If Me.MessageResult Is Nothing Then
                Me.DefineRenderedValue(String.Empty)
            Else
                Dim ContentDescription As [Global].ContentDescription =
                    New [Global].ContentDescription(Me.InsideValue)

                Dim BlockContent As String =
                    ContentDescription.Parts.Item(0)

                Me.ContentArguments.AppendKeyWithValue("MessageType", Me.MessageResult.Type)
                Me.ContentArguments.AppendKeyWithValue("Message", Me.MessageResult.Message)

                RaiseEvent ParseRequested(BlockContent, Me)

                Me.DefineRenderedValue(Me.Create())
            End If
        End Sub
    End Class
End Namespace
