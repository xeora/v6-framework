Imports System.Configuration

Namespace Xeora.Web.Configuration
    Public Class BannedElement
        Inherits ConfigurationElement

        <ConfigurationProperty("value", IsRequired:=True)>
        Public ReadOnly Property Value As String
            Get
                Return CType(MyBase.Item("value"), String)
            End Get
        End Property
    End Class
End Namespace