Option Strict On

Namespace Xeora.Web.Context
    Friend Class ContextContainer
        Private _Context As [Shared].IHttpContext

        Public Sub New(ByRef Context As [Shared].IHttpContext)
            Me._Context = Context
            Me.Marked = False
            Me.Removable = False
        End Sub

        Public Property Marked As Boolean
        Public Property Removable As Boolean

        Public ReadOnly Property Context As [Shared].IHttpContext
            Get
                Return Me._Context
            End Get
        End Property
    End Class
End Namespace