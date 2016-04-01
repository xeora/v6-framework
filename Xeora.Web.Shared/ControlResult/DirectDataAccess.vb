Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class DirectDataAccess
        Private _Message As Message
        Private _DBCommand As IDbCommand

        Public Sub New()
            Me._Message = Nothing
            Me._DBCommand = Nothing
        End Sub

        Public Property Message() As Message
            Get
                Return Me._Message
            End Get
            Set(ByVal Value As Message)
                Me._Message = Value
            End Set
        End Property

        Public Property DatabaseCommand() As IDbCommand
            Get
                Return Me._DBCommand
            End Get
            Set(ByVal Value As IDbCommand)
                Me._DBCommand = Value

                If Not Me._DBCommand Is Nothing Then
                    If Me._DBCommand.Connection Is Nothing Then _
                        Throw New NullReferenceException("Connection Parameter of Database Command must be available and valid!")

                    If String.IsNullOrEmpty(Me._DBCommand.CommandText) Then _
                        Throw New NullReferenceException("CommandText Parameter of Database Command must be available and valid!")
                End If
            End Set
        End Property
    End Class
End Namespace