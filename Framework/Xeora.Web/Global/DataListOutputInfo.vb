Option Strict On

Namespace Xeora.Web.Global
    <Serializable()>
    Public Class DataListOutputInfo
        Private _PartialRecords As Long
        Private _TotalRecords As Long
        Private _Message As [Shared].ControlResult.Message

        Public Sub New(ByVal PartialRecords As Long, ByVal TotalRecords As Long)
            Me._Message = Nothing

            Me._PartialRecords = PartialRecords
            Me._TotalRecords = TotalRecords
        End Sub

        Public ReadOnly Property Count() As Long
            Get
                Return Me._PartialRecords
            End Get
        End Property

        Public ReadOnly Property Total() As Long
            Get
                Return Me._TotalRecords
            End Get
        End Property

        Public Property Message() As [Shared].ControlResult.Message
            Get
                Return Me._Message
            End Get
            Set(ByVal value As [Shared].ControlResult.Message)
                Me._Message = value
            End Set
        End Property
    End Class
End Namespace