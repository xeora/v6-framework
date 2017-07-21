Imports System.Configuration

Namespace Xeora.Web.Configuration
    <ConfigurationCollection(GetType(BannedElement))>
    Public Class BannedElements
        Inherits ConfigurationElementCollection

        Protected Overrides Function CreateNewElement() As ConfigurationElement
            Return New BannedElement()
        End Function

        Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As Object
            Return CType(element, BannedElement).Value
        End Function
    End Class
End Namespace