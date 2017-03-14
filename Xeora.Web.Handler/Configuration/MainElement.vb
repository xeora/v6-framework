Imports System.Configuration

Namespace Xeora.Web.Configuration
    Public Class MainElement
        Inherits ConfigurationElement

        <ComponentModel.TypeConverter(GetType(StringToStringArray))>
        <ConfigurationProperty("defaultDomain", IsRequired:=True)>
        Public ReadOnly Property DefaultDomain As String()
            Get
                Return CType(MyBase.Item("defaultDomain"), String())
            End Get
        End Property

        Private NotInheritable Class StringToStringArray
            Inherits ConfigurationConverterBase

            Public Overrides Function CanConvertTo(ByVal ctx As ComponentModel.ITypeDescriptorContext, ByVal type As Type) As Boolean
                Return (type.ToString() = GetType(String()).ToString())
            End Function

            Public Overrides Function CanConvertFrom(ByVal ctx As ComponentModel.ITypeDescriptorContext, ByVal type As Type) As Boolean
                Return (type.ToString() = GetType(String).ToString())
            End Function

            Public Overrides Function ConvertTo(ByVal ctx As ComponentModel.ITypeDescriptorContext, ByVal ci As Globalization.CultureInfo, ByVal value As Object, ByVal type As Type) As Object
                Return String.Join("\"c, CType(value, String))
            End Function

            Public Overrides Function ConvertFrom(ByVal ctx As ComponentModel.ITypeDescriptorContext, ByVal ci As Globalization.CultureInfo, ByVal data As Object) As Object
                Return CType(data, String).Split("\"c)
            End Function
        End Class

        <ConfigurationProperty("physicalRoot", IsRequired:=True)>
        Public ReadOnly Property PhysicalRoot As String
            Get
                Return CType(MyBase.Item("physicalRoot"), String)
            End Get
        End Property

        <ConfigurationProperty("virtualRoot", DefaultValue:="/")>
        Public ReadOnly Property VirtualRoot As String
            Get
                Return CType(MyBase.Item("virtualRoot"), String)
            End Get
        End Property

        <ConfigurationProperty("applicationRoot", DefaultValue:=".\")>
        Public ReadOnly Property ApplicationRoot As String
            Get
                Return CType(MyBase.Item("applicationRoot"), String)
            End Get
        End Property

        <ConfigurationProperty("debugging", DefaultValue:=False)>
        Public ReadOnly Property Debugging As Boolean
            Get
                Return CType(MyBase.Item("debugging"), Boolean)
            End Get
        End Property

        <ConfigurationProperty("logHTTPExceptions", DefaultValue:=True)>
        Public ReadOnly Property LogHTTPExceptions As Boolean
            Get
                Return CType(MyBase.Item("logHTTPExceptions"), Boolean)
            End Get
        End Property

        <ConfigurationProperty("useHTML5Header", DefaultValue:=False)>
        Public ReadOnly Property UseHTML5Header As Boolean
            Get
                Return CType(MyBase.Item("useHTML5Header"), Boolean)
            End Get
        End Property

        <ConfigurationProperty("bandwidth", DefaultValue:=CType(0, UInt64))>
        Public ReadOnly Property Bandwidth As UInt64
            Get
                Return CType(MyBase.Item("bandwidth"), UInt64)
            End Get
        End Property

        <ConfigurationProperty("loggingPath")>
        Public ReadOnly Property LoggingPath As String
            Get
                Dim ReturnValue As String = CType(MyBase.Item("loggingPath"), String)

                If String.IsNullOrEmpty(ReturnValue) Then
                    Return IO.Path.Combine(PhysicalRoot, "XeoraLogs")
                Else
                    Return ReturnValue
                End If
            End Get
        End Property
    End Class
End Namespace