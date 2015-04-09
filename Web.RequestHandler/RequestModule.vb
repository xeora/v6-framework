Namespace SolidDevelopment.Web
    Public NotInheritable Class RequestModule
        Implements System.Web.IHttpModule

        Private Shared _pInitialized As Boolean = False
        Private Shared _pApplicationID As String = String.Empty
        Private Shared _pApplicationLocation As String = String.Empty

        Private Shared _pSessionIDManager As System.Web.SessionState.ISessionIDManager
        Private Shared _pSessionItems As Hashtable
        Private Shared _pTimeout As Integer
        Private Shared _pCookieMode As System.Web.HttpCookieMode = _
            System.Web.HttpCookieMode.AutoDetect
        Private Shared _pSessionStateMode As System.Web.SessionState.SessionStateMode = _
            System.Web.SessionState.SessionStateMode.Off

        'Private Shared _NextPruning As Date
        'Private Shared _SessionPruning As Boolean = False

        '
        ' Recursivly remove expired session data from session collection.
        '
        'Private Sub RemoveExpiredSessionData()
        '    RequestModule._SessionPruning = True

        '    Dim sessionID As String, IsPruningDone As Boolean = True

        '    System.Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
        '    Try
        '        ' Create Static VariablePool Service...
        '        Dim staticVPService As New SolidDevelopment.Web.Managers.VariablePoolOperationClass

        '        Dim Enumerator As System.Collections.IDictionaryEnumerator = _
        '            RequestModule._pSessionItems.GetEnumerator()

        '        Do While Enumerator.MoveNext()
        '            Dim item As SessionItem = CType(Enumerator.Value, SessionItem)

        '            If Date.Compare( _
        '                    item.Expires, _
        '                    Date.Now.AddMinutes( _
        '                        RequestModule._pTimeout * -1) _
        '                ) <= 0 Then

        '                sessionID = Enumerator.Key.ToString()

        '                ' Remove From Hash Table
        '                RequestModule._pSessionItems.Remove(sessionID)

        '                ' Destroy Session Data using Statick Variable Pool Service Instance
        '                Dim SessionHashCodeList As String() = _
        '                    SolidDevelopment.Web.General.UnRegisterSessionHashCodeList(sessionID)

        '                For Each sHC As String In SessionHashCodeList
        '                    staticVPService.DestroySessionData( _
        '                        String.Format("{0}_{1}", sessionID, sHC) _
        '                    )
        '                Next
        '                staticVPService.DestroySessionData(sessionID)
        '                ' !---

        '                Dim stateProvider As System.Web.SessionState.HttpSessionStateContainer = _
        '                          New System.Web.SessionState.HttpSessionStateContainer(sessionID, _
        '                                                                                   item.Items, _
        '                                                                                   item.StaticObjects, _
        '                                                                                   RequestModule._pTimeout, _
        '                                                                                   False, _
        '                                                                                   RequestModule._pCookieMode, _
        '                                                                                   RequestModule._pSessionStateMode, _
        '                                                                                   False)

        '                System.Web.SessionState.SessionStateUtility.RaiseSessionEnd(stateProvider, Me, EventArgs.Empty)

        '                IsPruningDone = False

        '                Exit Do
        '            End If
        '        Loop
        '    Finally
        '        System.Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
        '    End Try

        '    If Not IsPruningDone Then Me.RemoveExpiredSessionData()

        '    RequestModule._SessionPruning = False
        '    RequestModule._NextPruning = Date.Now.AddDays(1)
        'End Sub

        ' The SessionItem class is used to store data for a particular session along with
        ' an expiration date and time. SessionItem objects are added to the local Hashtable
        ' in the OnReleaseRequestState event handler and retrieved from the local Hashtable
        ' in the OnAcquireRequestState event handler. The ExpireCallback method is called
        ' periodically by the local Timer to check for all expired SessionItem objects in the
        ' local Hashtable and remove them. 
        Private Class SessionItem
            Friend Items As System.Web.SessionState.SessionStateItemCollection
            Friend StaticObjects As System.Web.HttpStaticObjectsCollection
            Friend Expires As DateTime
        End Class

        Private Shared _ApplicationLoaded As Boolean = False

        Public Sub Init(ByVal app As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            ' Application Domain UnHandled Exception Event Handling Defination
            If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")

            AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf Me.OnUnhandledExceptions)
            ' !---

            ' Add event handlers.
            AddHandler app.BeginRequest, New EventHandler(AddressOf Me.OnBeginRequest)
            AddHandler app.AcquireRequestState, New EventHandler(AddressOf Me.OnAcquireRequestState)
            AddHandler app.PreRequestHandlerExecute, New EventHandler(AddressOf Me.OnPreRequestHandlerExecute)
            AddHandler app.PostRequestHandlerExecute, New EventHandler(AddressOf Me.OnPostRequestHandlerExecute)
            AddHandler app.ReleaseRequestState, New EventHandler(AddressOf Me.OnReleaseRequestState)
            AddHandler app.EndRequest, New EventHandler(AddressOf Me.OnEndRequest)
            'AddHandler app.Disposed, New EventHandler(AddressOf Me.OnApplicationDisposed)
            ' !---

            If Not RequestModule._ApplicationLoaded Then RequestModule._pApplicationID = Guid.NewGuid().ToString()
            RequestModule._pApplicationLocation = _
                IO.Path.Combine( _
                    SolidDevelopment.Web.Configurations.TemporaryRoot, _
                    String.Format("{0}{2}{1}", _
                        SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID, _
                        RequestModule._pApplicationID, _
                        IO.Path.DirectorySeparatorChar _
                    ) _
                )
            If Not RequestModule._ApplicationLoaded Then Me.LoadApplication()

            ' If not already initialized, initialize timer and configuration.
            System.Threading.Monitor.Enter(Me)
            Try
                If Not RequestModule._pInitialized Then
                    ' Get the configuration section and set timeout and CookieMode values.
                    Dim cfg As System.Configuration.Configuration = _
                        System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration( _
                                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                    Dim wConfig As System.Web.Configuration.SessionStateSection = _
                        CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                    RequestModule._pSessionStateMode = wConfig.Mode

                    If RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                        RequestModule._pTimeout = CInt(wConfig.Timeout.TotalMinutes)
                        RequestModule._pCookieMode = wConfig.Cookieless

                        RequestModule._pSessionItems = Hashtable.Synchronized(New Hashtable)

                        ' Create a SessionIDManager.
                        RequestModule._pSessionIDManager = New System.Web.SessionState.SessionIDManager()
                        RequestModule._pSessionIDManager.Initialize()
                    End If

                    ' Set NextPruning Date For Session Clearance.
                    'RequestModule._NextPruning = Date.Now.AddDays(1)

                    RequestModule._pInitialized = True
                End If
            Finally
                System.Threading.Monitor.Exit(Me)
            End Try
        End Sub

        '
        ' Unhandled Exception Logging for AppDomain
        '
        Private Sub OnUnhandledExceptions(ByVal source As Object, ByVal args As UnhandledExceptionEventArgs)
            If Not args.ExceptionObject Is Nothing AndAlso _
                TypeOf args.ExceptionObject Is Exception Then

                Try
                    System.Diagnostics.EventLog.WriteEntry("XeoraCube", _
                        " --- RequestModule Exception --- " & Environment.NewLine & Environment.NewLine & _
                        CType( _
                            args.ExceptionObject, Exception).ToString(), _
                            EventLogEntryType.Error _
                    )
                Catch ex As Exception
                    ' Just Handle Exception
                End Try
            End If
        End Sub

        '
        ' Event handler for HttpApplication.PostAcquireRequestState
        '
        Private Sub OnBeginRequest(ByVal source As Object, ByVal args As EventArgs)
            Dim app As System.Web.HttpApplication = _
                CType(source, System.Web.HttpApplication)

            ' Check URL contains RootPath (~) modifier
            Dim RootPath As String = _
                app.Context.Request.RawUrl

            If RootPath.IndexOf("~/") > -1 Then
                app.Context.Items.Add("_sys_SkipSessionConfirmation", True)

                RootPath = RootPath.Remove(0, RootPath.IndexOf("~/") + 2)
                RootPath = RootPath.Insert(0, Configurations.ApplicationRoot.BrowserSystemImplementation)

                app.Context.Response.Clear()
                app.Context.Response.Redirect(RootPath)
                app.Context.Response.Close()

                Exit Sub
                ' BACKUP FOR FUTURE USE (RequestOnFly!)
                'SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                'SolidDevelopment.Web.General.Context.Items.Add( _
                '        "RedirectLocation", _
                '        RootPath _
                '    )
            End If
            ' !--

            ' Define a RequestID and ApplicationID for XeoraCube
            app.Context.Items.Add("RequestID", Guid.NewGuid().ToString())
            app.Context.Items.Add("ApplicationID", RequestModule._pApplicationID)

            'If RequestModule._pInitialized AndAlso _
            '    RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off AndAlso _
            '    Date.Compare(RequestModule._NextPruning, Date.Now) < 0 Then

            '    Me.RemoveExpiredSessionData()
            'End If
        End Sub

        '
        ' Event handler for HttpApplication.AcquireRequestState
        '
        Private Sub OnAcquireRequestState(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            If RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                Dim isNew As Boolean = False
                Dim sessionID As String, sessionData As SessionItem = Nothing

                Dim redirected As Boolean = False

                System.Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
                Try
                    RequestModule._pSessionIDManager.InitializeRequest(context, False, Nothing)

                    sessionID = RequestModule._pSessionIDManager.GetSessionID(context)

                    If Not sessionID Is Nothing Then
                        sessionData = CType(RequestModule._pSessionItems(sessionID), SessionItem)

                        If Not sessionData Is Nothing Then
                            If Date.Compare(sessionData.Expires, Date.Now) > 0 Then
                                sessionData.Expires = Date.Now.AddMinutes(RequestModule._pTimeout)
                            Else
                                ' Remove Session Data From HashTable
                                RequestModule._pSessionItems.Remove(sessionID)

                                sessionData = Nothing
                            End If
                        End If
                    Else
                        sessionID = RequestModule._pSessionIDManager.CreateSessionID(context)

                        RequestModule._pSessionIDManager.SaveSessionID(context, sessionID, redirected, Nothing)
                    End If

                    If Not redirected Then
                        If sessionData Is Nothing Then
                            ' Identify the session as a new session state instance. Create a new SessionItem
                            ' and add it to the local Hashtable.
                            isNew = True

                            sessionData = New SessionItem()

                            sessionData.Items = New System.Web.SessionState.SessionStateItemCollection()
                            sessionData.StaticObjects = System.Web.SessionState.SessionStateUtility.GetSessionStaticObjects(context)
                            sessionData.Expires = Date.Now.AddMinutes(RequestModule._pTimeout)

                            RequestModule._pSessionItems(sessionID) = sessionData
                        End If

                        ' Add the session data to the current HttpContext.
                        System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(context, _
                                         New System.Web.SessionState.HttpSessionStateContainer(sessionID, _
                                                                                                  sessionData.Items, _
                                                                                                  sessionData.StaticObjects, _
                                                                                                  RequestModule._pTimeout, _
                                                                                                  isNew, _
                                                                                                  RequestModule._pCookieMode, _
                                                                                                  System.Web.SessionState.SessionStateMode.Custom, _
                                                                                                  False))
                    End If
                Finally
                    System.Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
                End Try

                ' Execute the Session_OnStart event for a new session.
                If isNew Then RaiseEvent Start(Me, EventArgs.Empty)
            End If
        End Sub

        '
        ' Event handler for HttpApplication.PreRequestHandlerExecute
        '
        Private Sub OnPreRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            ' Prepare Context Variables
            SolidDevelopment.Web.General.SetRequestHttpContext( _
                CType(context.Items.Item("RequestID"), String), context)
            ' !--
        End Sub

        '
        ' Event handler for HttpApplication.PostRequestHandlerExecute
        '
        Private Sub OnPostRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            ' Confirm Variable Pool Registrations If It's a Template Request
            Dim IsTemplateRequest As Boolean
            Boolean.TryParse( _
                CType(context.Items.Item("_sys_TemplateRequest"), String), _
                IsTemplateRequest _
            )
            If IsTemplateRequest Then SolidDevelopment.Web.General.ConfirmVariables()
        End Sub

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnReleaseRequestState(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            Dim IsSkipSessionConfirmation As Boolean
            Boolean.TryParse( _
                CType(context.Items.Item("_sys_SkipSessionConfirmation"), String), _
                IsSkipSessionConfirmation _
            )
            If Not IsSkipSessionConfirmation AndAlso _
                RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                ' Read the session state from the context
                Dim stateProvider As System.Web.SessionState.HttpSessionStateContainer = _
                    CType(System.Web.SessionState.SessionStateUtility.GetHttpSessionStateFromContext(context), System.Web.SessionState.HttpSessionStateContainer)

                ' If Session.Abandon() was called, remove the session data from the local Hashtable
                ' and execute the Session_OnEnd event from the Global.asax file.
                If stateProvider.IsAbandoned Then
                    System.Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
                    Try
                        RequestModule._pSessionItems.Remove(stateProvider.SessionID)
                    Finally
                        System.Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
                    End Try

                    System.Web.SessionState.SessionStateUtility.RaiseSessionEnd(stateProvider, Me, EventArgs.Empty)
                    System.Web.SessionState.SessionStateUtility.RemoveHttpSessionStateFromContext(context)

                    ' Create Static VariablePool Service...
                    Dim staticVPService As New SolidDevelopment.Web.Managers.VariablePoolOperationClass

                    ' Destroy Session Data using Static Variable Pool Service Instance
                    Dim SessionHashCodeList As String() = _
                        SolidDevelopment.Web.General.UnRegisterSessionHashCodeList(stateProvider.SessionID)

                    For Each sHC As String In SessionHashCodeList
                        staticVPService.DestroySessionData( _
                            String.Format("{0}_{1}", stateProvider.SessionID, sHC) _
                        )
                    Next
                    staticVPService.DestroySessionData(stateProvider.SessionID)
                    ' !---
                End If
            End If
        End Sub

        '
        ' Event for Session_OnStart event in the Global.asax file.
        '
        Public Event Start As EventHandler

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnEndRequest(ByVal source As Object, ByVal args As EventArgs)
            CType(source, System.Web.HttpApplication).CompleteRequest()
        End Sub

        Private Sub LoadApplication()
            Try
                If Not IO.Directory.Exists(RequestModule._pApplicationLocation) Then IO.Directory.CreateDirectory(RequestModule._pApplicationLocation)

                Dim ThemeDllsLocation As String = _
                        IO.Path.Combine( _
                            Configurations.PyhsicalRoot, _
                            Configurations.ApplicationRoot.FileSystemImplementation _
                        )
                ThemeDllsLocation = IO.Path.Combine(ThemeDllsLocation, String.Format("Themes{0}Dlls", IO.Path.DirectorySeparatorChar))

                Dim DI As New IO.DirectoryInfo(ThemeDllsLocation)

                For Each fI As IO.FileInfo In DI.GetFiles()
                    If Not IO.File.Exists( _
                        IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name)) Then

                        Try
                            fI.CopyTo( _
                                IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name), True)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        End Try
                    End If
                Next

                RequestModule._ApplicationLoaded = True
            Catch ex As Exception
                Throw New Exception(String.Format("{0}!", SolidDevelopment.Web.Globals.SystemMessages.SYSTEM_APPLICATIONLOADINGERROR), ex)
            End Try
        End Sub

        Public Sub Dispose() Implements System.Web.IHttpModule.Dispose
            SolidDevelopment.Web.Managers.Assembly.ClearCache()
        End Sub
    End Class
End Namespace