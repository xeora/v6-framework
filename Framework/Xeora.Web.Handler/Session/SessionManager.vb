Option Strict On

Namespace Xeora.Web.Handler.Session
    Friend Class SessionManager
        Private _SessionIDManager As System.Web.SessionState.ISessionIDManager
        Private _SessionItems As Hashtable

        Public Sub New()
            Me._SessionIDManager = New System.Web.SessionState.SessionIDManager()
            Me._SessionIDManager.Initialize()

            Me._SessionItems = Hashtable.Synchronized(New Hashtable())
            Me.SessionStateMode = System.Web.SessionState.SessionStateMode.Off
            Me.CookieMode = System.Web.HttpCookieMode.AutoDetect
            Me.Timeout = 20
        End Sub

        Private Shared _Current As SessionManager = Nothing
        Public Shared ReadOnly Property Current As SessionManager
            Get
                If SessionManager._Current Is Nothing Then _
                    SessionManager._Current = New SessionManager()

                Return SessionManager._Current
            End Get
        End Property

        Public Property SessionStateMode As System.Web.SessionState.SessionStateMode
        Public Property CookieMode As System.Web.HttpCookieMode
        Public Property Timeout As Integer

        Public Sub InitializeRequest(ByRef Context As System.Web.HttpContext)
            Me._SessionIDManager.InitializeRequest(Context, False, Nothing)
        End Sub

        Public Sub Acquire(ByRef Context As System.Web.HttpContext)
            Dim isNew As Boolean = False
            Dim sessionID As String, sessionData As SessionItem = Nothing

            Dim redirected As Boolean = False

            Threading.Monitor.Enter(Me._SessionItems.SyncRoot)
            Try
                Me.InitializeRequest(Context)

                sessionID = Me._SessionIDManager.GetSessionID(Context)

                If Not sessionID Is Nothing Then
                    sessionData = CType(Me._SessionItems(sessionID), SessionItem)

                    If Not sessionData Is Nothing Then
                        If Date.Compare(Date.Now, sessionData.Expires) <= 0 Then
                            sessionData.Expires = Date.Now.AddMinutes(Me.Timeout)

                            Me._SessionItems(sessionID) = sessionData
                        Else
                            ' Remove Items From Session
                            sessionData.Items.Clear()
                            sessionData.Expires = Date.Now.AddMinutes(Me._Timeout)

                            Me._SessionItems(sessionID) = sessionData
                        End If
                    End If
                Else
                    sessionID = Me._SessionIDManager.CreateSessionID(Context)

                    Me._SessionIDManager.SaveSessionID(Context, sessionID, redirected, Nothing)
                End If

                If redirected Then Exit Try

                If sessionData Is Nothing Then
                    ' Identify the session as a new session state instance. Create a new SessionItem
                    ' and add it to the local Hashtable.
                    isNew = True

                    sessionData = New SessionItem(
                                        New System.Web.SessionState.SessionStateItemCollection(),
                                        System.Web.SessionState.SessionStateUtility.GetSessionStaticObjects(Context),
                                        Date.Now.AddMinutes(Me.Timeout)
                                    )

                    Me._SessionItems(sessionID) = sessionData
                End If

                ' Add the session data to the current HttpContext.
                System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(Context,
                    New System.Web.SessionState.HttpSessionStateContainer(
                        sessionID,
                        sessionData.Items,
                        sessionData.StaticObjects,
                        Me.Timeout,
                        isNew,
                        Me.CookieMode,
                        System.Web.SessionState.SessionStateMode.Custom,
                        False)
                )
            Finally
                Threading.Monitor.Exit(Me._SessionItems.SyncRoot)
            End Try
        End Sub

        Public Sub Release(ByRef Context As System.Web.HttpContext)
            ' Read the session state from the context
            Dim stateProvider As System.Web.SessionState.HttpSessionStateContainer =
                CType(System.Web.SessionState.SessionStateUtility.GetHttpSessionStateFromContext(Context), System.Web.SessionState.HttpSessionStateContainer)

            If stateProvider.IsAbandoned Then
                Threading.Monitor.Enter(Me._SessionItems.SyncRoot)
                Try
                    If Me._SessionItems.ContainsKey(stateProvider.SessionID) Then _
                        Me._SessionItems.Remove(stateProvider.SessionID)
                Finally
                    Threading.Monitor.Exit(Me._SessionItems.SyncRoot)
                End Try

                System.Web.SessionState.SessionStateUtility.RemoveHttpSessionStateFromContext(Context)
            End If
        End Sub
    End Class
End Namespace