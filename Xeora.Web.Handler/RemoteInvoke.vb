Option Strict On

Namespace Xeora.Web.Handler
    Public Class RemoteInvoke
        Private Shared _XeoraSettings As Configuration.XeoraSection
        Private Shared _VariablePoolService As Site.Service.VariablePool

        Public Shared Sub ReloadApplication(ByVal RequestID As String)
            Dim Context As [Shared].IHttpContext =
                Web.Context.ContextManager.Current.Context(RequestID)

            RequestModule.DisposeAll()

            If Not Context Is Nothing Then _
                Context.Response.Redirect(Context.Request.URL.Raw)
        End Sub

        Public Shared ReadOnly Property Context(ByVal RequestID As String) As [Shared].IHttpContext
            Get
                Return Web.Context.ContextManager.Current.Context(RequestID)
            End Get
        End Property

        Public Shared ReadOnly Property VariablePool() As Site.Service.VariablePool
            Get
                If RemoteInvoke._VariablePoolService Is Nothing Then _
                    RemoteInvoke._VariablePoolService = New Site.Service.VariablePool()

                Return RemoteInvoke._VariablePoolService
            End Get
        End Property

        Public Shared Property XeoraSettings() As Configuration.XeoraSection
            Get
                Return RemoteInvoke._XeoraSettings
            End Get
            Friend Set(ByVal value As Configuration.XeoraSection)
                RemoteInvoke._XeoraSettings = value
            End Set
        End Property
    End Class
End Namespace