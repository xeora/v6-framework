Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class HashCodePointedTemplate
        Inherits DirectiveControllerBase

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.HashCodePointedTemplate, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)

            Me.DefineRenderedValue(
                String.Format("{0}/{1}", Helpers.HashCode, controlValueSplitted(1))
            )
        End Sub
    End Class
End Namespace