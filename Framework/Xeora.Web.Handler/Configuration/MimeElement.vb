Imports System.Configuration

Namespace Xeora.Web.Configuration
    Public Class MimeElement
        Inherits System.Configuration.ConfigurationElement

        <ConfigurationProperty("type", IsRequired:=True)>
        Public ReadOnly Property Type As String
            Get
                Return CType(MyBase.Item("type"), String)
            End Get
        End Property

        <ConfigurationProperty("extension", IsRequired:=True)>
        Public ReadOnly Property Extension As String
            Get
                Return CType(MyBase.Item("extension"), String)
            End Get
        End Property
    End Class
End Namespace