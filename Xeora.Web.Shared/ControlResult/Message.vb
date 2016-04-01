Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class Message
        Public Enum Types
            [Error]
            Warning
            Success
        End Enum

        Private _Message As String
        Private _Type As Types

        Public Sub New(ByVal Message As String, Optional ByVal Type As Types = Types.Error)
            Me._Message = Message
            Me._Type = Type
        End Sub

        Public ReadOnly Property Message() As String
            Get
                Return Me._Message
            End Get
        End Property

        Public ReadOnly Property Type() As Types
            Get
                Return Me._Type
            End Get
        End Property
    End Class
End Namespace