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
                    If Me._HttpContextTable.ContainsKey(RequestID) Then
                        If Not CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Marked Then
                            Me._HttpContextTable.Remove(RequestID)
                        Else
                            CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Removable = True
                        End If
                    End If
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        Public Sub Mark(ByVal RequestID As String)
            If Not Me._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
                Try
                    If Me._HttpContextTable.ContainsKey(RequestID) Then _
                        CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Marked = True
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        Public Sub UnMark(ByVal RequestID As String)
            If Not Me._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(Me._HttpContextTable.SyncRoot)
                Try
                    If Me._HttpContextTable.ContainsKey(RequestID) Then
                        Dim ContextItem As ContextContainer =
                            CType(Me._HttpContextTable.Item(RequestID), ContextContainer)

                        If ContextItem.Removable Then
                            Me._HttpContextTable.Remove(RequestID)
                        Else
                            CType(Me._HttpContextTable.Item(RequestID), ContextContainer).Marked = False
                        End If
                    End If
                Finally
                    Threading.Monitor.Exit(Me._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub
    End Class
End Namespace