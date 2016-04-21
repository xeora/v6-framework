Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Language.Intellisense
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Operations
Imports Microsoft.VisualStudio.Utilities

Namespace Xeora.VSAddIn.IDE.Editor.Completion
    <Export(GetType(ICompletionSourceProvider))>
    <Name("XeoraTemplate")>
    <Order(Before:="HTML Completion Source Provider")>
    <ContentType("xeora")>
    Public Class TemplateSourceProvider
        Implements ICompletionSourceProvider

        Public Function TryCreateCompletionSource(ByVal textBuffer As ITextBuffer) As ICompletionSource Implements ICompletionSourceProvider.TryCreateCompletionSource
            Return New TemplateSource(Me, textBuffer)
        End Function

        <Import()>
        Public Property NavigatorService() As ITextStructureNavigatorSelectorService
    End Class
End Namespace