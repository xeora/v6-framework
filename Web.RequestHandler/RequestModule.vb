Namespace SolidDevelopment.Web
    Public NotInheritable Class RequestModule
        Implements System.Web.IHttpModule

        Private Shared _pInitialized As Boolean = False
        Private Shared _pApplicationID As String = String.Empty
        Private Shared _pApplicationLocation As String = String.Empty

        Private Shared _HttpContextTable As Hashtable

        Private Shared _pSessionIDManager As System.Web.SessionState.ISessionIDManager
        Private Shared _pSessionItems As Hashtable
        Private Shared _pTimeout As Integer
        Private Shared _pCookieMode As System.Web.HttpCookieMode = _
            System.Web.HttpCookieMode.AutoDetect
        Private Shared _pSessionStateMode As System.Web.SessionState.SessionStateMode = _
            System.Web.SessionState.SessionStateMode.Off

        Private Shared _VPService As SolidDevelopment.Web.Managers.VariablePoolOperationClass
        Private Const SESSIONKEYID As String = "000000000000000000000000_00000000"

        Private Class SessionItem
            Private _Items As System.Web.SessionState.SessionStateItemCollection
            Private _StaticObjects As System.Web.HttpStaticObjectsCollection
            Private _Expires As DateTime

            Public Sub New(ByVal Items As System.Web.SessionState.SessionStateItemCollection, ByVal StaticObjects As System.Web.HttpStaticObjectsCollection, ByVal Expires As DateTime)
                Me._Items = Items
                Me._StaticObjects = StaticObjects
                Me._Expires = Expires
            End Sub

            Public ReadOnly Property Items As System.Web.SessionState.SessionStateItemCollection
                Get
                    Return Me._Items
                End Get
            End Property

            Public ReadOnly Property StaticObjects As System.Web.HttpStaticObjectsCollection
                Get
                    Return Me._StaticObjects
                End Get
            End Property

            Public Property Expires As DateTime
                Get
                    Return Me._Expires
                End Get
                Set(ByVal value As DateTime)
                    Me._Expires = value
                End Set
            End Property
        End Class

        Private Class ContextContainer
            Private _IsThreadContext As Boolean
            Private _Context As System.Web.HttpContext

            Public Sub New(ByVal IsThreadContext As Boolean, ByVal Context As System.Web.HttpContext)
                Me._IsThreadContext = IsThreadContext
                Me._Context = Context
            End Sub

            Public ReadOnly Property IsThreadContext As Boolean
                Get
                    Return Me._IsThreadContext
                End Get
            End Property

            Public ReadOnly Property Context As System.Web.HttpContext
                Get
                    Return Me._Context
                End Get
            End Property
        End Class

        Public Sub Init(ByVal app As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            ' Application Domain UnHandled Exception Event Handling Defination
            Try
                If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")
            Catch ex As Exception
                ' Just Handle Exceptions
            End Try

            AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf Me.OnUnhandledExceptions)
            ' !---

            ' Add event handlers.
            AddHandler app.BeginRequest, New EventHandler(AddressOf Me.OnBeginRequest)
            AddHandler app.AcquireRequestState, New EventHandler(AddressOf Me.OnAcquireRequestState)
            AddHandler app.PreRequestHandlerExecute, New EventHandler(AddressOf Me.OnPreRequestHandlerExecute)
            AddHandler app.PostRequestHandlerExecute, New EventHandler(AddressOf Me.OnPostRequestHandlerExecute)
            AddHandler app.ReleaseRequestState, New EventHandler(AddressOf Me.OnReleaseRequestState)
            AddHandler app.EndRequest, New EventHandler(AddressOf Me.OnEndRequest)
            ' !---

            RequestModule._VPService = New SolidDevelopment.Web.Managers.VariablePoolOperationClass()

            Me.LoadApplication(False)

            ' If not already initialized, initialize timer and configuration.
            System.Threading.Monitor.Enter(Me)
            Try
                If Not RequestModule._pInitialized Then
                    If RequestModule._HttpContextTable Is Nothing Then _
                        RequestModule._HttpContextTable = Hashtable.Synchronized(New Hashtable())

                    ' Get the configuration section and set timeout and CookieMode values.
                    Dim cfg As System.Configuration.Configuration = _
                        System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration( _
                                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                    Dim wConfig As System.Web.Configuration.SessionStateSection = _
                        CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                    RequestModule._pSessionStateMode = wConfig.Mode
                    RequestModule._pTimeout = CInt(wConfig.Timeout.TotalMinutes)

                    If RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                        RequestModule._pCookieMode = wConfig.Cookieless

                        RequestModule._pSessionItems = Hashtable.Synchronized(New Hashtable)
                    End If

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
                RootPath = RootPath.Remove(0, RootPath.IndexOf("~/") + 2)
                RootPath = RootPath.Insert(0, Configurations.ApplicationRoot.BrowserSystemImplementation)

                app.Context.RewritePath(RootPath)
            ElseIf RootPath.IndexOf("¨/") > -1 Then
                ' It search something outside of XeoraCube Handler
                RootPath = RootPath.Remove(0, RootPath.IndexOf("¨/") + 2)
                RootPath = RootPath.Insert(0, Configurations.VirtualRoot)

                app.Context.Response.Clear()
                app.Context.Response.Redirect(RootPath)
                app.Context.Response.End()

                Exit Sub
            End If
            ' !--

            ' Check, this worker has the same ApplicationID with the most active one.
            Dim ApplicationID As Byte() = _
                RequestModule.VariablePool.GetVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")

            If Not ApplicationID Is Nothing AndAlso _
                String.Compare(RequestModule._pApplicationID, System.Text.Encoding.UTF8.GetString(ApplicationID)) <> 0 Then
                RequestModule._pApplicationID = System.Text.Encoding.UTF8.GetString(ApplicationID)
                RequestModule._pApplicationLocation = _
                    IO.Path.Combine( _
                        SolidDevelopment.Web.Configurations.TemporaryRoot, _
                        String.Format("{0}{2}{1}", _
                            SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID, _
                            RequestModule._pApplicationID, _
                            IO.Path.DirectorySeparatorChar _
                        ) _
                    )
            End If

            ' Define a RequestID and ApplicationID for XeoraCube
            app.Context.Items.Add("RequestID", Guid.NewGuid().ToString())
            app.Context.Items.Add("ApplicationID", RequestModule._pApplicationID)
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
                    RequestModule.SessionIDManager.InitializeRequest(context, False, Nothing)

                    sessionID = RequestModule.SessionIDManager.GetSessionID(context)

                    If Not sessionID Is Nothing Then
                        sessionData = CType(RequestModule._pSessionItems(sessionID), SessionItem)

                        If Not sessionData Is Nothing Then
                            If Date.Compare(Date.Now, sessionData.Expires) <= 0 Then
                                sessionData.Expires = Date.Now.AddMinutes(RequestModule._pTimeout)
                            Else
                                ' Remove Session Data From HashTable
                                RequestModule._pSessionItems.Remove(sessionID)

                                sessionData = Nothing
                            End If
                        End If
                    Else
                        sessionID = RequestModule.SessionIDManager.CreateSessionID(context)

                        RequestModule.SessionIDManager.SaveSessionID(context, sessionID, redirected, Nothing)
                    End If

                    If Not redirected Then
                        If sessionData Is Nothing Then
                            ' Identify the session as a new session state instance. Create a new SessionItem
                            ' and add it to the local Hashtable.
                            isNew = True

                            sessionData = New SessionItem( _
                                                New System.Web.SessionState.SessionStateItemCollection(), _
                                                System.Web.SessionState.SessionStateUtility.GetSessionStaticObjects(context), _
                                                Date.Now.AddMinutes(RequestModule._pTimeout) _
                                            )

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
            End If
        End Sub


        '
        ' Event handler for HttpApplication.PreRequestHandlerExecute
        '
        Private Sub OnPreRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            ' Prepare Context Variables
            Dim RequestID As String = _
                CType(context.Items.Item("RequestID"), String)

            System.Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
            Try
                If RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                    RequestModule._HttpContextTable.Remove(RequestID)

                If Not context Is Nothing Then _
                    RequestModule._HttpContextTable.Add(RequestID, New ContextContainer(False, context))
            Finally
                System.Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
            End Try
            ' !--
        End Sub

        '
        ' Event handler for HttpApplication.PostRequestHandlerExecute
        '
        Private Sub OnPostRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            Dim RequestID As String = _
                CType(context.Items.Item("RequestID"), String)
            Dim IsTemplateRequest As Boolean = _
                CType(context.Items.Item("_sys_TemplateRequest"), Boolean)

            ' WAIT UNTIL CONFIRMATION FINISHES!
            If IsTemplateRequest Then SolidDevelopment.Web.General.ConfirmVariables()

            If Not RequestModule._HttpContextTable Is Nothing AndAlso _
                Not String.IsNullOrEmpty(RequestID) Then

                System.Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                        RequestModule._HttpContextTable.Remove(RequestID)
                Finally
                    System.Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnReleaseRequestState(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            If RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                ' Read the session state from the context
                Dim stateProvider As System.Web.SessionState.HttpSessionStateContainer = _
                    CType(System.Web.SessionState.SessionStateUtility.GetHttpSessionStateFromContext(context), System.Web.SessionState.HttpSessionStateContainer)

                ' If Session.Abandon() was called, remove the session data from the local Hashtable
                ' and execute the Session_OnEnd event from the Global.asax file.
                If stateProvider.IsAbandoned Then
                    System.Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
                    Try
                        If RequestModule._pSessionItems.ContainsKey(stateProvider.SessionID) Then _
                            RequestModule._pSessionItems.Remove(stateProvider.SessionID)
                    Finally
                        System.Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
                    End Try

                    System.Web.SessionState.SessionStateUtility.RaiseSessionEnd(stateProvider, Me, EventArgs.Empty)
                    System.Web.SessionState.SessionStateUtility.RemoveHttpSessionStateFromContext(context)
                End If
            End If
        End Sub

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnEndRequest(ByVal source As Object, ByVal args As EventArgs)
            CType(source, System.Web.HttpApplication).CompleteRequest()
        End Sub

        Public Shared ReadOnly Property VariablePool() As SolidDevelopment.Web.Managers.VariablePoolOperationClass
            Get
                Return RequestModule._VPService
            End Get
        End Property

        Public Shared ReadOnly Property Context(ByVal RequestID As String) As System.Web.HttpContext
            Get
                If String.IsNullOrEmpty(RequestID) OrElse _
                    Not RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                    Return Nothing

                Return CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer).Context
            End Get
        End Property

        Public Shared Function CreateThreadContext(ByVal RequestID As String) As String
            Dim rNewRequestID As String = String.Empty

            If Not RequestModule._HttpContextTable Is Nothing AndAlso _
                Not String.IsNullOrEmpty(RequestID) Then

                System.Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then
                        rNewRequestID = Guid.NewGuid().ToString()

                        Dim tContext As System.Web.HttpContext = _
                            CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer).Context

                        Dim NewContext As System.Web.HttpContext = _
                            New System.Web.HttpContext(tContext.Request, tContext.Response)

                        For Each Key As Object In tContext.Items.Keys
                            NewContext.Items.Add(Key, tContext.Items.Item(Key))
                        Next
                        NewContext.Items.Item("RequestID") = rNewRequestID

                        RequestModule.SessionIDManager.InitializeRequest(NewContext, False, Nothing)

                        RequestModule._HttpContextTable.Add(rNewRequestID, New ContextContainer(True, NewContext))
                    End If
                Finally
                    System.Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
                End Try
            End If

            Return rNewRequestID
        End Function

        Public Shared Sub DestroyThreadContext(ByVal RequestID As String)
            If Not RequestModule._HttpContextTable Is Nothing AndAlso _
                Not String.IsNullOrEmpty(RequestID) Then

                System.Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then
                        Dim ContextItem As ContextContainer = _
                            CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer)

                        If ContextItem.IsThreadContext Then _
                            RequestModule._HttpContextTable.Remove(RequestID)
                    End If
                Finally
                    System.Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        Private Sub LoadApplication(ByVal ForceReload As Boolean)
            If Not ForceReload AndAlso Not String.IsNullOrEmpty(RequestModule._pApplicationID) Then Exit Sub

            If ForceReload Then
                RequestModule.VariablePool.UnRegisterVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")
                RequestModule.VariablePool.ConfirmRegistrations(RequestModule.SESSIONKEYID)
            End If

            Dim ApplicationID As Byte() = _
                RequestModule.VariablePool.GetVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")

            If Not ApplicationID Is Nothing Then
                RequestModule._pApplicationID = System.Text.Encoding.UTF8.GetString(ApplicationID)
                RequestModule._pApplicationLocation = _
                    IO.Path.Combine( _
                        SolidDevelopment.Web.Configurations.TemporaryRoot, _
                        String.Format("{0}{2}{1}", _
                            SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID, _
                            RequestModule._pApplicationID, _
                            IO.Path.DirectorySeparatorChar _
                        ) _
                    )

                If Me.IsReloadRequired() Then Me.LoadApplication(True)
            Else
                Try
                    RequestModule._pApplicationID = Guid.NewGuid().ToString()
                    RequestModule._pApplicationLocation = _
                        IO.Path.Combine( _
                            SolidDevelopment.Web.Configurations.TemporaryRoot, _
                            String.Format("{0}{2}{1}", _
                                SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID, _
                                RequestModule._pApplicationID, _
                                IO.Path.DirectorySeparatorChar _
                            ) _
                        )

                    RequestModule.VariablePool.RegisterVariableToPool( _
                        RequestModule.SESSIONKEYID, "ApplicationID", _
                        System.Text.Encoding.UTF8.GetBytes(RequestModule._pApplicationID) _
                    )
                    RequestModule.VariablePool.ConfirmRegistrations(RequestModule.SESSIONKEYID)

                    If Not IO.Directory.Exists(RequestModule._pApplicationLocation) Then _
                        IO.Directory.CreateDirectory(RequestModule._pApplicationLocation)

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
                Catch ex As Exception
                    Throw New Exception(String.Format("{0}!", SolidDevelopment.Web.Globals.SystemMessages.SYSTEM_APPLICATIONLOADINGERROR), ex)
                End Try
            End If
        End Sub

        Private Function IsReloadRequired() As Boolean
            Dim rBoolean As Boolean = False

            If IO.Directory.Exists(RequestModule._pApplicationLocation) Then
                Dim ThemeDllsLocation As String = _
                        IO.Path.Combine( _
                            Configurations.PyhsicalRoot, _
                            Configurations.ApplicationRoot.FileSystemImplementation _
                        )
                ThemeDllsLocation = IO.Path.Combine(ThemeDllsLocation, String.Format("Themes{0}Dlls", IO.Path.DirectorySeparatorChar))

                Dim DI As New IO.DirectoryInfo(ThemeDllsLocation)
                Dim MD5 As System.Security.Cryptography.MD5 = _
                    System.Security.Cryptography.MD5.Create()

                For Each fI As IO.FileInfo In DI.GetFiles()
                    If Not IO.File.Exists( _
                        IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name)) Then

                        rBoolean = True

                        Exit For
                    Else
                        Dim RealStream As IO.Stream = Nothing
                        Dim CacheStream As IO.Stream = Nothing

                        Try
                            RealStream = fI.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                            CacheStream = New IO.FileStream(IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                            Dim RealHash As Byte() = MD5.ComputeHash(RealStream)
                            Dim CacheHash As Byte() = MD5.ComputeHash(CacheStream)

                            If RealHash.Length <> CacheHash.Length Then
                                rBoolean = True
                            Else
                                For bC As Integer = 0 To RealHash.Length - 1
                                    If RealHash(bC) <> CacheHash(bC) Then
                                        rBoolean = True

                                        Exit For
                                    End If
                                Next
                            End If
                        Catch ex As Exception
                            rBoolean = True
                        Finally
                            If Not RealStream Is Nothing Then RealStream.Close()
                            If Not CacheStream Is Nothing Then CacheStream.Close()
                        End Try

                        If rBoolean Then Exit For
                    End If
                Next
            Else
                rBoolean = True
            End If

            Return rBoolean
        End Function

        Private Sub UnLoadApplication()
            Dim ApplicationsRoot As String = _
                IO.Path.Combine( _
                    SolidDevelopment.Web.Configurations.TemporaryRoot, _
                    SolidDevelopment.Web.Configurations.WorkingPath.WorkingPathID _
                )

            For Each Path As String In IO.Directory.GetDirectories(ApplicationsRoot)
                If Path.EndsWith("PoolSessions") OrElse _
                    Path.Contains(RequestModule._pApplicationID) Then Continue For

                ' Check if all files are in use
                Dim IsRemovable As Boolean = True

                For Each FilePath As String In IO.Directory.GetFiles(Path)
                    Dim CheckFS As IO.FileStream = Nothing

                    Try
                        CheckFS = New IO.FileStream(FilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
                    Catch ex As Exception
                        IsRemovable = False

                        Exit For
                    Finally
                        If Not CheckFS Is Nothing Then _
                            CheckFS.Close()
                    End Try
                Next

                If IsRemovable Then
                    Try
                        IO.Directory.Delete(Path, True)
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next
        End Sub

        Private Shared ReadOnly Property SessionIDManager As System.Web.SessionState.ISessionIDManager
            Get
                If RequestModule._pSessionIDManager Is Nothing Then
                    RequestModule._pSessionIDManager = New System.Web.SessionState.SessionIDManager()
                    RequestModule._pSessionIDManager.Initialize()
                End If

                Return RequestModule._pSessionIDManager
            End Get
        End Property

        Public Sub Dispose() Implements System.Web.IHttpModule.Dispose
            SolidDevelopment.Web.Managers.Assembly.ClearCache()

            Me.UnLoadApplication()
        End Sub
    End Class
End Namespace