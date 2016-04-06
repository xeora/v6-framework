Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class PartialDataTable
        Inherits DataTable

        Private _Message As Message
        Private _Total As Integer

        Public Sub New()
            Me._Message = Nothing
        End Sub

        Public Property Message() As Message
            Get
                Return Me._Message
            End Get
            Set(ByVal Value As Message)
                Me._Message = Value

                Me.Clear()
                Me._Total = 0
            End Set
        End Property

        Public Sub Replace(ByVal Source As DataTable)
            Me.Clear()

            If Not Source Is Nothing Then _
                Me.Merge(Source, False, MissingSchemaAction.Add)
        End Sub

        Public Property Total() As Integer
            Get
                If Me._Total = 0 Then Me._Total = Me.Rows.Count

                Return Me._Total
            End Get
            Set(ByVal Value As Integer)
                Me._Total = Value
            End Set
        End Property
    End Class
End Namespace