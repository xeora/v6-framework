Option Strict On

Namespace Xeora.Web.Handler
    Public NotInheritable Class RequestModule
        Implements System.Web.IHttpModule

        Private Shared _QuickAccess As RequestModule = Nothing

        Private Shared _pInitialized As Boolean = False
        Private Shared _pApplicationID As String = String.Empty
        Private Shared _pApplicationLocation As String = String.Empty

        Private Shared _xeoraSettings As Configuration.XeoraSection

        Private Shared _HttpContextTable As Hashtable

        Private Shared _pSessionIDManager As System.Web.SessionState.ISessionIDManager
        Private Shared _pSessionItems As Hashtable
        Private Shared _pTimeout As Integer
        Private Shared _pCookieMode As System.Web.HttpCookieMode =
            System.Web.HttpCookieMode.AutoDetect
        Private Shared _pSessionStateMode As System.Web.SessionState.SessionStateMode =
            System.Web.SessionState.SessionStateMode.Off

        Private Shared _VPService As Site.Service.VariablePool
        Private Const SESSIONKEYID As String = "000000000000000000000000_00000000"

        Private Class SessionItem
            Private _Items As System.Web.SessionState.SessionStateItemCollection
            Private _StaticObjects As System.Web.HttpStaticObjectsCollection
            Private _Expires As Date

            Public Sub New(ByVal Items As System.Web.SessionState.SessionStateItemCollection, ByVal StaticObjects As System.Web.HttpStaticObjectsCollection, ByVal Expires As Date)
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

            Public Property Expires As Date
                Get
                    Return Me._Expires
                End Get
                Set(ByVal value As Date)
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

        Public Sub New()
            RequestModule._QuickAccess = Me
        End Sub

        Public Sub Init(ByVal app As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            ' Application Domain UnHandled Exception Event Handling Defination
            Try
                If Not EventLog.SourceExists("XeoraCube") Then EventLog.CreateEventSource("XeoraCube", "XeoraCube")
            Catch ex As System.Exception
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

            ' If not already initialized, initialize timer and configuration.
            Threading.Monitor.Enter(Me)
            Try
                If Not RequestModule._pInitialized Then
                    If RequestModule._HttpContextTable Is Nothing Then _
                        RequestModule._HttpContextTable = Hashtable.Synchronized(New Hashtable())

                    ' Get the configuration section and set timeout and CookieMode values.
                    Dim cfg As System.Configuration.Configuration =
                        System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)

                    ' Take care framework defaults
                    Dim IsModified As Boolean = False

                    ' Session CookieID
                    Dim sConfig As System.Web.Configuration.SessionStateSection =
                        CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                    If String.Compare(sConfig.CookieName, "xcsid") <> 0 Then
                        sConfig.CookieName = "xcsid"

                        IsModified = True
                    End If

                    ' disable ASP related headers 
                    Dim hConfig As System.Web.Configuration.HttpRuntimeSection =
                        CType(cfg.GetSection("system.web/httpRuntime"), System.Web.Configuration.HttpRuntimeSection)

                    If hConfig.EnableVersionHeader Then
                        hConfig.EnableVersionHeader = False

                        IsModified = True
                    End If

                    If IsModified Then _
                        cfg.Save(System.Configuration.ConfigurationSaveMode.Modified)

                    IsModified = False

                    Dim confDocStream As IO.Stream = Nothing
                    Dim xmlDocument As System.Xml.XmlDocument
                    Try
                        confDocStream = New IO.FileStream(cfg.FilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)

                        xmlDocument = New System.Xml.XmlDocument()
                        xmlDocument.PreserveWhitespace = True
                        xmlDocument.Load(confDocStream)
                    Catch ex As System.Exception
                        Throw
                    Finally
                        If Not confDocStream Is Nothing Then confDocStream.Close()
                    End Try

                    Dim xmlNodes As System.Xml.XmlNodeList = Nothing
                    ' take care xeora configuration section
                    xmlNodes = xmlDocument.SelectNodes("/configuration/configSections")

                    If xmlNodes.Count = 0 Then
                        xmlNodes = xmlDocument.SelectNodes("/configuration")

                        If xmlNodes.Count = 0 Then
                            Throw New System.Exception("Application Configuration File Error!")
                        Else
                            Dim sectionNode As Xml.XmlNode =
                                xmlDocument.CreateElement("section")
                            Dim nameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            nameAttribute.Value = "xeora"
                            Dim typeAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("type")
                            typeAttribute.Value = String.Format("Xeora.Web.Configuration.XeoraSection, {0}", System.Reflection.Assembly.GetExecutingAssembly().FullName)

                            sectionNode.Attributes.Append(nameAttribute)
                            sectionNode.Attributes.Append(typeAttribute)

                            Dim configSectionsNode As Xml.XmlNode =
                                xmlDocument.CreateElement("configSections")

                            configSectionsNode.AppendChild(sectionNode)

                            xmlNodes.Item(0).InsertBefore(configSectionsNode, xmlNodes.Item(0).FirstChild)

                            IsModified = True
                        End If
                    Else
                        xmlNodes = xmlDocument.SelectNodes("/configuration/configSections/section[@name='xeora']")

                        If xmlNodes.Count <> 1 Then
                            If xmlNodes.Count > 1 Then
                                For Each xmlNode As Xml.XmlNode In xmlNodes
                                    xmlNode.ParentNode.RemoveChild(xmlNode)
                                Next
                            End If

                            xmlNodes = xmlDocument.SelectNodes("/configuration/configSections")

                            Dim sectionNode As Xml.XmlNode =
                            xmlDocument.CreateElement("section")
                            Dim nameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            nameAttribute.Value = "xeora"
                            Dim typeAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("type")
                            typeAttribute.Value = String.Format("Xeora.Web.Configuration.XeoraSection, {0}", System.Reflection.Assembly.GetCallingAssembly().FullName)

                            sectionNode.Attributes.Append(nameAttribute)
                            sectionNode.Attributes.Append(typeAttribute)

                            xmlNodes.Item(0).AppendChild(sectionNode)

                            IsModified = True
                        End If
                    End If

                    ' take care xeora configuration section
                    xmlNodes = xmlDocument.SelectNodes("/configuration/xeora")

                    If xmlNodes.Count <> 1 Then
                        If xmlNodes.Count > 1 Then
                            For Each xmlNode As Xml.XmlNode In xmlNodes
                                xmlNode.ParentNode.RemoveChild(xmlNode)
                            Next
                        End If
                        xmlNodes = xmlDocument.SelectNodes("/configuration")

                        Dim xeoraNode As Xml.XmlNode =
                            xmlDocument.CreateElement("xeora")
                        Dim nameAttribute As Xml.XmlAttribute =
                            xmlDocument.CreateAttribute("configSource")
                        nameAttribute.Value = "xeora.config"

                        xeoraNode.Attributes.Append(nameAttribute)

                        xmlNodes.Item(0).AppendChild(xeoraNode)

                        IsModified = True
                    End If

                    ' take care framework headers
                    xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol")

                    If xmlNodes.Count = 0 Then
                        xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer")

                        If xmlNodes.Count = 0 Then
                            Throw New System.Exception("Application Configuration File Error!")
                        Else
                            Dim removeNode As Xml.XmlNode =
                                xmlDocument.CreateElement("remove")
                            Dim removeNameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            removeNameAttribute.Value = "X-Powered-By"
                            removeNode.Attributes.Append(removeNameAttribute)

                            Dim addNode As Xml.XmlNode =
                                xmlDocument.CreateElement("add")
                            Dim addNameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            addNameAttribute.Value = "X-Powered-By"
                            Dim addValueAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("value")
                            addValueAttribute.Value = "XeoraCube"

                            addNode.Attributes.Append(addNameAttribute)
                            addNode.Attributes.Append(addValueAttribute)

                            Dim customHeadersNode As Xml.XmlNode =
                                xmlDocument.CreateElement("customHeaders")

                            Dim protocolNode As Xml.XmlNode =
                                xmlDocument.CreateElement("httpProtocol")

                            customHeadersNode.AppendChild(removeNode)
                            customHeadersNode.AppendChild(addNode)
                            protocolNode.AppendChild(customHeadersNode)

                            xmlNodes.Item(0).AppendChild(protocolNode)

                            IsModified = True
                        End If
                    Else
                        xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders")

                        If xmlNodes.Count = 0 Then
                            xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol")

                            Dim removeNode As Xml.XmlNode =
                                xmlDocument.CreateElement("remove")
                            Dim removeNameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            removeNameAttribute.Value = "X-Powered-By"
                            removeNode.Attributes.Append(removeNameAttribute)

                            Dim addNode As Xml.XmlNode =
                                xmlDocument.CreateElement("add")
                            Dim addNameAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("name")
                            addNameAttribute.Value = "X-Powered-By"
                            Dim addValueAttribute As Xml.XmlAttribute =
                                xmlDocument.CreateAttribute("value")
                            addValueAttribute.Value = "XeoraCube"

                            addNode.Attributes.Append(addNameAttribute)
                            addNode.Attributes.Append(addValueAttribute)

                            Dim customHeadersNode As Xml.XmlNode =
                                xmlDocument.CreateElement("customHeaders")

                            customHeadersNode.AppendChild(removeNode)
                            customHeadersNode.AppendChild(addNode)

                            xmlNodes.Item(0).AppendChild(customHeadersNode)

                            IsModified = True
                        Else
                            Dim xpbModified As Boolean = False
                            xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders/remove[@name='X-Powered-By']")

                            If xmlNodes.Count <> 1 Then
                                If xmlNodes.Count > 1 Then
                                    For Each xmlNode As Xml.XmlNode In xmlNodes
                                        xmlNode.ParentNode.RemoveChild(xmlNode)
                                    Next
                                End If

                                xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders")

                                Dim removeNode As Xml.XmlNode =
                                    xmlDocument.CreateElement("remove")
                                Dim removeNameAttribute As Xml.XmlAttribute =
                                    xmlDocument.CreateAttribute("name")
                                removeNameAttribute.Value = "X-Powered-By"
                                removeNode.Attributes.Append(removeNameAttribute)

                                xmlNodes.Item(0).AppendChild(removeNode)

                                IsModified = True
                                xpbModified = True
                            End If

                            xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders/add[@name='X-Powered-By']")

                            If xmlNodes.Count <> 1 OrElse xpbModified Then
                                If xmlNodes.Count > 1 OrElse xpbModified Then
                                    For Each xmlNode As Xml.XmlNode In xmlNodes
                                        xmlNode.ParentNode.RemoveChild(xmlNode)
                                    Next
                                End If

                                xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders")

                                Dim addNode As Xml.XmlNode =
                                    xmlDocument.CreateElement("add")
                                Dim addNameAttribute As Xml.XmlAttribute =
                                    xmlDocument.CreateAttribute("name")
                                addNameAttribute.Value = "X-Powered-By"
                                Dim addValueAttribute As Xml.XmlAttribute =
                                    xmlDocument.CreateAttribute("value")
                                addValueAttribute.Value = "XeoraCube"

                                addNode.Attributes.Append(addNameAttribute)
                                addNode.Attributes.Append(addValueAttribute)

                                xmlNodes.Item(0).AppendChild(addNode)

                                IsModified = True
                            Else
                                If String.Compare(xmlNodes.Item(0).Attributes.GetNamedItem("value").Value, "XeoraCube") <> 0 Then
                                    xmlNodes.Item(0).Attributes.GetNamedItem("value").Value = "XeoraCube"
                                    IsModified = True
                                End If
                            End If

                            xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders/add[@name='X-FrameworkVersion']")

                            Dim vI As Version = Reflection.Assembly.GetExecutingAssembly().GetName().Version
                            Dim vIS As String = String.Format("{0}.{1}.{2}", vI.Major, vI.Minor, vI.Build)

                            If xmlNodes.Count <> 1 Then
                                If xmlNodes.Count > 1 Then
                                    For Each xmlNode As Xml.XmlNode In xmlNodes
                                        xmlNode.ParentNode.RemoveChild(xmlNode)
                                    Next
                                End If

                                xmlNodes = xmlDocument.SelectNodes("/configuration/system.webServer/httpProtocol/customHeaders")

                                Dim addNode As Xml.XmlNode =
                                    xmlDocument.CreateElement("add")
                                Dim addNameAttribute As Xml.XmlAttribute =
                                    xmlDocument.CreateAttribute("name")
                                addNameAttribute.Value = "X-FrameworkVersion"
                                Dim addValueAttribute As Xml.XmlAttribute =
                                    xmlDocument.CreateAttribute("value")
                                addValueAttribute.Value = vIS

                                addNode.Attributes.Append(addNameAttribute)
                                addNode.Attributes.Append(addValueAttribute)

                                xmlNodes.Item(0).AppendChild(addNode)

                                IsModified = True
                            Else
                                If String.Compare(xmlNodes.Item(0).Attributes.GetNamedItem("value").Value, vIS) <> 0 Then
                                    xmlNodes.Item(0).Attributes.GetNamedItem("value").Value = vIS
                                    IsModified = True
                                End If
                            End If
                        End If
                    End If

                    If IsModified Then
                        Dim cfgBackupFilePath As String =
                            String.Format("{0}.backup", cfg.FilePath)

                        confDocStream = Nothing
                        Try
                            IO.File.Move(cfg.FilePath, cfgBackupFilePath)

                            confDocStream =
                                New IO.FileStream(cfg.FilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                            xmlDocument.Save(confDocStream)

                            IO.File.Delete(cfgBackupFilePath)
                        Catch ex As System.Exception
                            If IO.File.Exists(cfgBackupFilePath) Then
                                If IO.File.Exists(cfg.FilePath) Then _
                                    IO.File.Delete(cfg.FilePath)

                                IO.File.Move(cfgBackupFilePath, cfg.FilePath)
                            End If

                            Throw
                        Finally
                            If Not confDocStream Is Nothing Then confDocStream.Close()
                        End Try
                    End If
                    ' !---

                    If IsModified Then
                        cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                    End If

                    RequestModule._xeoraSettings = CType(cfg.GetSection("xeora"), Configuration.XeoraSection)

                    RequestModule._pSessionStateMode = sConfig.Mode
                    RequestModule._pTimeout = CInt(sConfig.Timeout.TotalMinutes)

                    If RequestModule._pSessionStateMode = System.Web.SessionState.SessionStateMode.Off Then
                        RequestModule._pCookieMode = sConfig.Cookieless

                        RequestModule._pSessionItems = Hashtable.Synchronized(New Hashtable)
                    End If

                    RequestModule._pInitialized = True
                End If
            Finally
                Threading.Monitor.Exit(Me)
            End Try

            Me.LoadApplication(False)
        End Sub

        '
        ' Unhandled Exception Logging for AppDomain
        '
        Private Sub OnUnhandledExceptions(ByVal source As Object, ByVal args As UnhandledExceptionEventArgs)
            If Not args.ExceptionObject Is Nothing AndAlso
                TypeOf args.ExceptionObject Is System.Exception Then

                Try
                    EventLog.WriteEntry("XeoraCube",
                        " --- RequestModule Exception --- " & Environment.NewLine & Environment.NewLine &
                        CType(
                            args.ExceptionObject, System.Exception).ToString(),
                            EventLogEntryType.Error
                    )
                Catch ex As System.Exception
                    ' Just Handle Exception
                End Try
            End If
        End Sub

        '
        ' Event handler for HttpApplication.PostAcquireRequestState
        '
        Private Sub OnBeginRequest(ByVal source As Object, ByVal args As EventArgs)
            Dim app As System.Web.HttpApplication =
                CType(source, System.Web.HttpApplication)

            ' Check URL contains RootPath (~) modifier
            Dim RootPath As String =
                app.Context.Request.RawUrl

            If RootPath.IndexOf("~/") > -1 Then
                Dim tildeIdx As Integer = RootPath.IndexOf("~/")

                RootPath = RootPath.Remove(0, tildeIdx + 2)
                RootPath = RootPath.Insert(0, [Shared].Configurations.ApplicationRoot.BrowserImplementation)

                app.Context.RewritePath(RootPath)
            ElseIf RootPath.IndexOf("¨/") > -1 Then
                ' It search something outside of XeoraCube Handler
                Dim helfIdx As Integer = RootPath.IndexOf("¨/")

                RootPath = RootPath.Remove(0, helfIdx + 2)
                RootPath = RootPath.Insert(0, [Shared].Configurations.VirtualRoot)

                app.Context.Response.Clear()
                app.Context.Response.Redirect(RootPath, True)

                Exit Sub
            End If
            ' !--

            ' Check, this worker has the same ApplicationID with the most active one.
            Dim ApplicationID As Byte() =
                RequestModule.VariablePool.GetVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")

            If Not ApplicationID Is Nothing AndAlso
                String.Compare(RequestModule._pApplicationID, Text.Encoding.UTF8.GetString(ApplicationID)) <> 0 Then
                RequestModule._pApplicationID = Text.Encoding.UTF8.GetString(ApplicationID)
                RequestModule._pApplicationLocation =
                    IO.Path.Combine(
                        [Shared].Configurations.TemporaryRoot,
                        String.Format("{0}{2}{1}",
                            [Shared].Configurations.WorkingPath.WorkingPathID,
                            RequestModule._pApplicationID,
                            IO.Path.DirectorySeparatorChar
                        )
                    )
            End If

            If Me.IsReloadRequired() Then Me.LoadApplication(True)

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

                Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
                Try
                    RequestModule.SessionIDManager.InitializeRequest(context, False, Nothing)

                    sessionID = RequestModule.SessionIDManager.GetSessionID(context)

                    If Not sessionID Is Nothing Then
                        sessionData = CType(RequestModule._pSessionItems(sessionID), SessionItem)

                        If Not sessionData Is Nothing Then
                            If Date.Compare(Date.Now, sessionData.Expires) <= 0 Then
                                sessionData.Expires = Date.Now.AddMinutes(RequestModule._pTimeout)

                                RequestModule._pSessionItems(sessionID) = sessionData
                            Else
                                ' Remove Items From Session
                                sessionData.Items.Clear()
                                sessionData.Expires = Date.Now.AddMinutes(RequestModule._pTimeout)

                                RequestModule._pSessionItems(sessionID) = sessionData
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

                            sessionData = New SessionItem(
                                                New System.Web.SessionState.SessionStateItemCollection(),
                                                System.Web.SessionState.SessionStateUtility.GetSessionStaticObjects(context),
                                                Date.Now.AddMinutes(RequestModule._pTimeout)
                                            )

                            RequestModule._pSessionItems(sessionID) = sessionData
                        End If

                        ' Add the session data to the current HttpContext.
                        System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(context,
                                         New System.Web.SessionState.HttpSessionStateContainer(sessionID,
                                                                                                  sessionData.Items,
                                                                                                  sessionData.StaticObjects,
                                                                                                  RequestModule._pTimeout,
                                                                                                  isNew,
                                                                                                  RequestModule._pCookieMode,
                                                                                                  System.Web.SessionState.SessionStateMode.Custom,
                                                                                                  False))
                    End If
                Finally
                    Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
                End Try
            End If
        End Sub

        '
        ' Event handler for HttpApplication.PreRequestHandlerExecute
        '
        Private Sub OnPreRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            ' Prepare Context Variables
            Dim RequestID As String =
                CType(context.Items.Item("RequestID"), String)

            Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
            Try
                If RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                    RequestModule._HttpContextTable.Remove(RequestID)

                If Not context Is Nothing Then _
                    RequestModule._HttpContextTable.Add(RequestID, New ContextContainer(False, context))
            Finally
                Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
            End Try
            ' !--
        End Sub

        '
        ' Event handler for HttpApplication.PostRequestHandlerExecute
        '
        Private Sub OnPostRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            Dim RequestID As String =
                CType(context.Items.Item("RequestID"), String)
            Dim IsTemplateRequest As Boolean =
                CType(context.Items.Item("_sys_TemplateRequest"), Boolean)

            ' WAIT UNTIL CONFIRMATION FINISHES!
            'If IsTemplateRequest Then SolidDevelopment.Web.General.ConfirmVariables()

            If Not RequestModule._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                        RequestModule._HttpContextTable.Remove(RequestID)
                Finally
                    Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
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
                Dim stateProvider As System.Web.SessionState.HttpSessionStateContainer =
                    CType(System.Web.SessionState.SessionStateUtility.GetHttpSessionStateFromContext(context), System.Web.SessionState.HttpSessionStateContainer)

                ' If Session.Abandon() was called, remove the session data from the local Hashtable
                ' and execute the Session_OnEnd event from the Global.asax file.
                If stateProvider.IsAbandoned Then
                    Threading.Monitor.Enter(RequestModule._pSessionItems.SyncRoot)
                    Try
                        If RequestModule._pSessionItems.ContainsKey(stateProvider.SessionID) Then _
                            RequestModule._pSessionItems.Remove(stateProvider.SessionID)
                    Finally
                        Threading.Monitor.Exit(RequestModule._pSessionItems.SyncRoot)
                    End Try

                    ' This event is here for just dummy purpose 
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

        Public Shared Sub ReloadApplication(ByVal RequestID As String)
            Dim Context As System.Web.HttpContext =
                RequestModule.Context(RequestID)

            RequestModule._QuickAccess.Dispose()

            If Not Context Is Nothing Then _
                Context.Response.Redirect(Context.Request.RawUrl, True)
        End Sub

        Public Shared Sub ClearStaticCache()
            Controller.Directive.PartialCache.ClearCache()
        End Sub

        Public Shared ReadOnly Property VariablePool() As Site.Service.VariablePool
            Get
                Return RequestModule._VPService
            End Get
        End Property

        Public Shared ReadOnly Property XeoraSettings() As Object
            Get
                Return RequestModule._xeoraSettings
            End Get
        End Property

        Public Shared ReadOnly Property Context(ByVal RequestID As String) As System.Web.HttpContext
            Get
                If String.IsNullOrEmpty(RequestID) OrElse
                    Not RequestModule._HttpContextTable.ContainsKey(RequestID) Then _
                    Return Nothing

                Return CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer).Context
            End Get
        End Property

        Public Shared Function CreateThreadContext(ByVal RequestID As String) As String
            Dim rNewRequestID As String = String.Empty

            If Not RequestModule._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then
                        rNewRequestID = Guid.NewGuid().ToString()

                        Dim tContext As System.Web.HttpContext =
                            CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer).Context

                        Dim NewContext As System.Web.HttpContext =
                            New System.Web.HttpContext(tContext.Request, tContext.Response)

                        For Each Key As Object In tContext.Items.Keys
                            NewContext.Items.Add(Key, tContext.Items.Item(Key))
                        Next
                        NewContext.Items.Item("RequestID") = rNewRequestID

                        RequestModule.SessionIDManager.InitializeRequest(NewContext, False, Nothing)

                        RequestModule._HttpContextTable.Add(rNewRequestID, New ContextContainer(True, NewContext))
                    End If
                Finally
                    Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
                End Try
            End If

            Return rNewRequestID
        End Function

        Public Shared Sub DestroyThreadContext(ByVal RequestID As String)
            If Not RequestModule._HttpContextTable Is Nothing AndAlso
                Not String.IsNullOrEmpty(RequestID) Then

                Threading.Monitor.Enter(RequestModule._HttpContextTable.SyncRoot)
                Try
                    If RequestModule._HttpContextTable.ContainsKey(RequestID) Then
                        Dim ContextItem As ContextContainer =
                            CType(RequestModule._HttpContextTable.Item(RequestID), ContextContainer)

                        If ContextItem.IsThreadContext Then _
                            RequestModule._HttpContextTable.Remove(RequestID)
                    End If
                Finally
                    Threading.Monitor.Exit(RequestModule._HttpContextTable.SyncRoot)
                End Try
            End If
        End Sub

        Private Sub LoadApplication(ByVal ForceReload As Boolean)
            If Not ForceReload AndAlso Not String.IsNullOrEmpty(RequestModule._pApplicationID) Then Exit Sub

            Dim CacheRootLocation As String =
                IO.Path.Combine(
                    [Shared].Configurations.TemporaryRoot,
                    [Shared].Configurations.WorkingPath.WorkingPathID
                )
            If Not IO.Directory.Exists(CacheRootLocation) Then _
                IO.Directory.CreateDirectory(CacheRootLocation)

            If RequestModule._VPService Is Nothing Then _
                RequestModule._VPService = New Site.Service.VariablePool()

            If ForceReload Then
                ' Clear CacheFiles' MD5 Hash Cache
                Me._CacheFileHashCache = Nothing

                ' Clear Assembly Cache On Memory
                Manager.Assembly.ClearCache()

                RequestModule.VariablePool.UnRegisterVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")
                'RequestModule.VariablePool.ConfirmRegistrations(RequestModule.SESSIONKEYID)
            End If

            Dim ApplicationID As Byte() =
                RequestModule.VariablePool.GetVariableFromPool(RequestModule.SESSIONKEYID, "ApplicationID")

            If Not ApplicationID Is Nothing Then
                RequestModule._pApplicationID = Text.Encoding.UTF8.GetString(ApplicationID)
                RequestModule._pApplicationLocation =
                    IO.Path.Combine(
                        [Shared].Configurations.TemporaryRoot,
                        String.Format("{0}{2}{1}",
                            [Shared].Configurations.WorkingPath.WorkingPathID,
                            RequestModule._pApplicationID,
                            IO.Path.DirectorySeparatorChar
                        )
                    )

                If Me.IsReloadRequired() Then Me.LoadApplication(True)
            Else
                Try
                    RequestModule._pApplicationID = Guid.NewGuid().ToString()
                    RequestModule._pApplicationLocation =
                        IO.Path.Combine(
                            [Shared].Configurations.TemporaryRoot,
                            String.Format("{0}{2}{1}",
                                [Shared].Configurations.WorkingPath.WorkingPathID,
                                RequestModule._pApplicationID,
                                IO.Path.DirectorySeparatorChar
                            )
                        )

                    RequestModule.VariablePool.RegisterVariableToPool(
                        RequestModule.SESSIONKEYID, "ApplicationID",
                        System.Text.Encoding.UTF8.GetBytes(RequestModule._pApplicationID)
                    )
                    'RequestModule.VariablePool.ConfirmRegistrations(RequestModule.SESSIONKEYID)

                    If Not IO.Directory.Exists(RequestModule._pApplicationLocation) Then _
                        IO.Directory.CreateDirectory(RequestModule._pApplicationLocation)

                    Dim DefaultDomainRootLocation As String =
                        IO.Path.Combine(
                            [Shared].Configurations.PyhsicalRoot,
                            [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                            "Domains"
                        )
                    For dC As Integer = 0 To [Shared].Configurations.DefaultDomain.Length - 1
                        If dC > 0 Then
                            DefaultDomainRootLocation =
                                IO.Path.Combine(DefaultDomainRootLocation, "Addons")
                        End If

                        DefaultDomainRootLocation =
                            IO.Path.Combine(DefaultDomainRootLocation, [Shared].Configurations.DefaultDomain(dC))
                    Next

                    Me.LoadDomainExecutables(DefaultDomainRootLocation)

                    RequestModule.VariablePool.RegisterVariableToPool(
                        RequestModule.SESSIONKEYID, String.Format("{0}_Command", RequestModule._pApplicationID), Text.Encoding.UTF32.GetBytes("ClearDomainCache"))
                Catch ex As System.Exception
                    Throw New System.Exception(String.Format("{0}!", [Global].SystemMessages.SYSTEM_APPLICATIONLOADINGERROR), ex)
                End Try
            End If
        End Sub

        Private Sub LoadDomainExecutables(ByVal DomainRootPath As String)
            If Not IO.Directory.Exists(RequestModule._pApplicationLocation) Then _
                IO.Directory.CreateDirectory(RequestModule._pApplicationLocation)

            Dim DomainExecutablesLocation As String =
                IO.Path.Combine(DomainRootPath, "Executables")

            Dim DI As New IO.DirectoryInfo(DomainExecutablesLocation)

            For Each fI As IO.FileInfo In DI.GetFiles()
                If Not IO.File.Exists(
                    IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name)) Then

                    Try
                        fI.CopyTo(
                            IO.Path.Combine(RequestModule._pApplicationLocation, fI.Name), True)
                    Catch ex As System.Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next

            Dim DomainChildrenDI As IO.DirectoryInfo =
                New IO.DirectoryInfo(IO.Path.Combine(DomainRootPath, "Addons"))

            If DomainChildrenDI.Exists Then
                For Each ChildDomainDI As IO.DirectoryInfo In DomainChildrenDI.GetDirectories()
                    Me.LoadDomainExecutables(ChildDomainDI.FullName)
                Next
            End If
        End Sub

        Private Function IsReloadRequired() As Boolean
            Dim rBoolean As Boolean = False

            If IO.Directory.Exists(RequestModule._pApplicationLocation) Then
                Dim DefaultDomainRootLocation As String =
                    IO.Path.Combine(
                        CType(RequestModule.XeoraSettings, Configuration.XeoraSection).Main.PyhsicalRoot,
                        [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                        "Domains"
                    )
                For dC As Integer = 0 To CType(RequestModule.XeoraSettings, Configuration.XeoraSection).Main.DefaultDomain.Length - 1
                    If dC > 0 Then
                        DefaultDomainRootLocation =
                            IO.Path.Combine(DefaultDomainRootLocation, "Addons")
                    End If

                    DefaultDomainRootLocation =
                        IO.Path.Combine(DefaultDomainRootLocation, CType(RequestModule.XeoraSettings, Configuration.XeoraSection).Main.DefaultDomain(dC))
                Next

                rBoolean = Me.ExamReloadRequirement(DefaultDomainRootLocation)
            Else
                rBoolean = True
            End If

            Return rBoolean
        End Function

        Private _CacheFileHashCache As Dictionary(Of String, Byte()) = Nothing
        Private Function ExamReloadRequirement(ByVal DomainRootPath As String) As Boolean
            Dim rBoolean As Boolean = False

            If Me._CacheFileHashCache Is Nothing Then _
                Me._CacheFileHashCache = New Dictionary(Of String, Byte())

            Dim DI As IO.DirectoryInfo =
                New IO.DirectoryInfo(
                    IO.Path.Combine(DomainRootPath, "Executables"))
            Dim MD5 As Security.Cryptography.MD5 = Nothing

            For Each RealFI As IO.FileInfo In DI.GetFiles("*.dll")
                Dim CacheFI As IO.FileInfo =
                    New IO.FileInfo(
                        IO.Path.Combine(RequestModule._pApplicationLocation, RealFI.Name))

                If Not CacheFI.Exists OrElse RealFI.Length <> CacheFI.Length Then
                    rBoolean = True

                    Exit For
                Else
                    If RealFI.LastWriteTime.CompareTo(CacheFI.LastWriteTime) = 0 Then Continue For

                    Dim RealStream As IO.Stream = Nothing, RealHash As Byte()
                    Dim CacheStream As IO.Stream = Nothing, CacheHash As Byte()

                    If MD5 Is Nothing Then MD5 = Security.Cryptography.MD5.Create()

                    Try
                        RealStream = RealFI.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        RealHash = MD5.ComputeHash(RealStream)

                        If Me._CacheFileHashCache.ContainsKey(CacheFI.FullName) Then
                            CacheHash = Me._CacheFileHashCache.Item(CacheFI.FullName)
                        Else
                            CacheStream = CacheFI.Open(IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                            CacheHash = MD5.ComputeHash(CacheStream)

                            Me._CacheFileHashCache.Add(CacheFI.FullName, CacheHash)
                        End If

                        rBoolean = Not RealHash.SequenceEqual(CacheHash)
                    Catch ex As System.Exception
                        rBoolean = True
                    Finally
                        If Not RealStream Is Nothing Then RealStream.Close()
                        If Not CacheStream Is Nothing Then CacheStream.Close()
                    End Try

                    If rBoolean Then Exit For
                End If
            Next

            If Not rBoolean Then
                Dim DomainChildrenDI As IO.DirectoryInfo =
                    New IO.DirectoryInfo(IO.Path.Combine(DomainRootPath, "Addons"))

                If DomainChildrenDI.Exists Then
                    For Each ChildDomainDI As IO.DirectoryInfo In DomainChildrenDI.GetDirectories()
                        rBoolean = Me.ExamReloadRequirement(ChildDomainDI.FullName)

                        If rBoolean Then Exit For
                    Next
                End If
            End If

            Return rBoolean
        End Function

        Private Sub UnLoadApplication()
            Dim ApplicationsRoot As String =
                IO.Path.Combine(
                    [Shared].Configurations.TemporaryRoot,
                    [Shared].Configurations.WorkingPath.WorkingPathID
                )

            For Each Path As String In IO.Directory.GetDirectories(ApplicationsRoot)
                If Path.EndsWith("PoolSessions") OrElse
                    Path.Contains(RequestModule._pApplicationID) Then Continue For

                ' Check if all files are in use
                Dim IsRemovable As Boolean = True

                For Each FilePath As String In IO.Directory.GetFiles(Path)
                    Dim CheckFS As IO.FileStream = Nothing

                    Try
                        CheckFS = New IO.FileStream(FilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
                    Catch ex As System.Exception
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
                    Catch ex As System.Exception
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
            Manager.Assembly.ClearCache()

            Me.UnLoadApplication()
        End Sub
    End Class
End Namespace