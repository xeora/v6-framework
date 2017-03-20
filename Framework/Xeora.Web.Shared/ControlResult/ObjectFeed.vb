Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class ObjectFeed
        Private _Message As Message
        Private _Objects As Object()

        Private _Count As Integer
        Private _Total As Integer

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal Objects As Object())
            Me._Message = Nothing
            Me.Objects = Objects
        End Sub

        Public Property Message() As Message
            Get
                Return Me._Message
            End Get
            Set(ByVal Value As Message)
                Me._Message = Value
            End Set
        End Property

        Public Property Objects() As Object()
            Get
                Return Me._Objects
            End Get
            Set(ByVal Value As Object())
                Me._Objects = Value

                If Me._Objects Is Nothing Then Me._Objects = New Object() {}
            End Set
        End Property

        Public Property Count() As Integer
            Get
                If Me._Count = 0 Then Me._Count = Me.Objects.Length

                Return Me._Count
            End Get
            Set(ByVal Value As Integer)
                Me._Count = Value
            End Set
        End Property

        Public Property Total() As Integer
            Get
                If Me._Total = 0 Then Me._Total = Me.Objects.Length

                Return Me._Total
            End Get
            Set(ByVal Value As Integer)
                Me._Total = Value
            End Set
        End Property
    End Class
End Namespace