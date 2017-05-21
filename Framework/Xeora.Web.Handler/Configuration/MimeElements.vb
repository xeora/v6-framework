Imports System.Configuration

Namespace Xeora.Web.Configuration
    <ConfigurationCollection(GetType(MimeElement))>
    Public Class MimeElements
        Inherits ConfigurationElementCollection

        Protected Overrides Function CreateNewElement() As ConfigurationElement
            Return New MimeElement()
        End Function

        Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As Object
            Return CType(element, MimeElement).Extension
        End Function
    End Class
End Namespace