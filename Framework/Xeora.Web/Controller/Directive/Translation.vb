Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class Translation
        Inherits DirectiveControllerBase
        Implements IInstanceRequires

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.Translation, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            Dim TranslationID As String =
                Me.InsideValue.Split(":"c)(1)

            Dim Instance As IDomain = Nothing
            RaiseEvent InstanceRequested(Instance)

            Dim ExamingInstance As IDomain = Instance
            Dim TranslationText As String = String.Empty

            Do
                TranslationText = ExamingInstance.Language.Get(TranslationID)

                If String.IsNullOrEmpty(TranslationText) Then ExamingInstance = ExamingInstance.Parent
            Loop Until ExamingInstance Is Nothing OrElse Not String.IsNullOrEmpty(TranslationText)

            Me.DefineRenderedValue(TranslationText)
        End Sub
    End Class
End Namespace
