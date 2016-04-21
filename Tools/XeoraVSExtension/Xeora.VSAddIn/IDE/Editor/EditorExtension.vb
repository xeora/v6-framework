Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Utilities

Namespace Xeora.VSAddIn.IDE.Editor
    Public Class EditorExtension
        <Export>
        <Name("xeora")>
        <BaseDefinition("htmlx")>
        Public XeoraTemplateContentTypeDefinition As ContentTypeDefinition

        <Export>
        <FileExtension(".xchtml")>
        <ContentType("xeora")>
        Public XeoraTemplateFileExtensionDefinition As FileExtensionToContentTypeDefinition

        <Export>
        <Name("xccontrols")>
        <BaseDefinition("xml")>
        Public XeoraControlsContentTypeDefinition As ContentTypeDefinition

        <Export>
        <FileExtension(".xml")>
        <ContentType("xccontrols")>
        Public XeoraControlsFileExtensionDefinition As FileExtensionToContentTypeDefinition
    End Class
End Namespace