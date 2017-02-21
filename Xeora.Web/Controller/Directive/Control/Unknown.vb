Option Strict On

Namespace Xeora.Web.Controller.Directive.Control
    Public Class Unknown
        Inherits ControlBase

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            Throw New NotSupportedException("UnKnown Custom Control Type!")
        End Sub
    End Class
End Namespace