Imports System.Configuration

Namespace Xeora.Web.Configuration
    Public Class ServicePortElement
        Inherits System.Configuration.ConfigurationElement

        <ConfigurationProperty("variablePool", DefaultValue:=CType(12005, UInt16))>
        Public ReadOnly Property VariablePool As UInt16
            Get
                Return CType(MyBase.Item("variablePool"), UInt16)
            End Get
        End Property

        <ConfigurationProperty("scheduledTasks", DefaultValue:=CType(0, UInt16))>
        Public ReadOnly Property ScheduledTasks As UInt16
            Get
                Return CType(MyBase.Item("scheduledTasks"), UInt16)
            End Get
        End Property
    End Class
End Namespace