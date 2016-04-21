Imports Microsoft.VisualStudio.Language

Namespace Xeora.VSAddIn.IDE.Editor.Completion.SourceBuilder
    Public Class Special
        Inherits BuilderBase

        Public Property RequestFromControl As Boolean

        Public Overrides Function Build() As Intellisense.Completion()
            Dim CompList As New Generic.List(Of Intellisense.Completion)()

            CompList.Add(New Intellisense.Completion("Domain Content Marker", "DomainContents$", String.Empty, Me.ProvideImageSource(IconResource.singlestatement), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Control", "C:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            CompList.Add(New Intellisense.Completion("Server-Side Executable Execution", "F:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Client-Side Executable Bind", "XF:", String.Empty, Me.ProvideImageSource(IconResource.blockstatement), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Update Block", "H:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            CompList.Add(New Intellisense.Completion("Translation", "L:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Template", "T:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            CompList.Add(New Intellisense.Completion("TemplateID w/ Same VariablePool", "P:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Message Block", "MB:", String.Empty, Me.ProvideImageSource(IconResource.blockstatement), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("InLine Statement", "S:", String.Empty, Me.ProvideImageSource(IconResource.control), Nothing))
            If Not Me.RequestFromControl Then _
                CompList.Add(New Intellisense.Completion("Page Render Duration", "PageRenderDuration$", String.Empty, Me.ProvideImageSource(IconResource.singlestatement), Nothing))

            Return CompList.ToArray()
        End Function

        Public Overrides Function Builders() As Intellisense.Completion()
            Return Nothing
        End Function
    End Class
End Namespace