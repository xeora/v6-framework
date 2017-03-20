Option Strict On

Namespace Xeora.Web.Context
    Public Class ContextManager
        Private _HttpContextTable As Hashtable

        Public Sub New()
            Me._HttpContextTable = Hashtable.Synchronized(New Hashtable())
        End Sub

        Private Shared _Current As ContextManager = Nothing
        Public Shared ReadOnly Property Current As ContextManager
            Get
                If ContextManager._Current Is Nothing Then _
                    ContextManager._Current = New ContextManager()

                Return ContextManager._Current
            End Get
        End Property

        Public ReadOnly Property Context(ByVal RequestID As String) As [Shared].IHttpContext
            Get
                If String.IsNullOrEmpty(RequestID) OrElse
                    Not Me._HttpContextTable.ContainsKey(RequestID) Then _
                    Return Nothing

                Return CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Context
            End Get
        End Property

        Friend Sub Add(ByVal RequestID As String, ByVal ContextContainer As ContextContainer)
            Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
            Try
                If Me._HttpContextTable.ContainsKey(RequestID) Then _
                    Me._HttpContextTable.Remove(RequestID)

                If Not ContextContainer Is Nothing AndAlso Not ContextContainer.Context Is Nothing Then _
                    Me._HttpContextTable.Add(RequestID, ContextContainer)
            Finally
                Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
            End Try
        End Sub

        Friend Sub Remove(ByVal RequestID As String)
            If Not Me._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
                Try
                    If Me._HttpContextTable.ContainsKey(RequestID) Then _
                        Me._HttpContextTable.Remove(RequestID)
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        Public Function DuplicateContext(ByVal RequestID As String) As String
            Dim rNewRequestID As String = String.Empty

            If Not Me._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
                Try
                    If Me._HttpContextTable.ContainsKey(RequestID) Then
                        Dim tContext As [Shared].IHttpContext =
                            CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Context

                        Dim NewContext As System.Web.HttpContext =
                            New System.Web.HttpContext(tContext.UnderlyingContext.Request, tContext.UnderlyingContext.Response)

                        For Each Key As Object In tContext.UnderlyingContext.Items.Keys
                            NewContext.Items.Add(Key, tContext.UnderlyingContext.Items.Item(Key))
                        Next
                        NewContext.Items.Item("RequestID") = Guid.NewGuid().ToString()

                        Handler.Session.SessionManager.Current.InitializeRequest(NewContext)

                        Dim NewBuildInContext As [Shared].IHttpContext =
                            New BuildInContext(NewContext)

                        rNewRequestID = NewBuildInContext.XeoraRequestID

                        Me._HttpContextTable.Add(rNewRequestID, New ContextContainer(True, NewBuildInContext))
                    End If
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If

            Return rNewRequestID
        End Function

        Public Function DisposeContext(ByVal RequestID As String) As Boolean
            If Not Me._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
                Try
                    If Me._HttpContextTable.ContainsKey(RequestID) Then
                        Dim ContextItem As ContextContainer =
                            CType(Me._HttpContextTable.Item(RequestID), ContextContainer)

                        If ContextItem.IsThreadContext Then
                            Me._HttpContextTable.Remove(RequestID)

                            Return True
                        End If
                    End If
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If

            Return False
        End Function
    End Class
End Namespace