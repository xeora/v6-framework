Imports System.Configuration

Namespace Xeora.Web.Configuration
    Public Class XeoraSection
        Inherits ConfigurationSection

        <ConfigurationProperty("main", IsRequired:=True)>
        Public ReadOnly Property Main As MainElement
            Get
                Return CType(MyBase.Item("main"), MainElement)
            End Get
        End Property

        <ConfigurationProperty("requestTagFilter")>
        Public ReadOnly Property RequestTagFilter As RequestTagFilterElement
            Get
                Return CType(MyBase.Item("requestTagFilter"), RequestTagFilterElement)
            End Get
        End Property

        <ConfigurationProperty("servicePort")>
        Public ReadOnly Property ServicePort As ServicePortElement
            Get
                Return CType(MyBase.Item("servicePort"), ServicePortElement)
            End Get
        End Property

        <ConfigurationProperty("mime", IsDefaultCollection:=True)>
        <ConfigurationCollection(GetType(MimeElement), AddItemName:="item")>
        Public ReadOnly Property CustomMimes As MimeElements
            Get
                Return CType(MyBase.Item("mime"), MimeElements)
            End Get
        End Property
    End Class
End Namespace