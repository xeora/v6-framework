Option Strict On

Namespace Xeora.Web.Context
    Friend Class ContextContainer
        Private _IsThreadContext As Boolean
        Private _Context As [Shared].IHttpContext

        Public Sub New(ByVal IsThreadContext As Boolean, ByRef Context As [Shared].IHttpContext)
            Me._IsThreadContext = IsThreadContext
            Me._Context = Context
        End Sub

        Public ReadOnly Property IsThreadContext As Boolean
            Get
                Return Me._IsThreadContext
            End Get
        End Property

        Public ReadOnly Property Context As [Shared].IHttpContext
            Get
                Return Me._Context
            End Get
        End Property
    End Class
End Namespace