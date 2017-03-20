Option Strict On

Namespace Xeora.Web.Handler
    Public NotInheritable Class RequestModule
        Implements System.Web.IHttpModule

        Private Shared _Instance As New Concurrent.ConcurrentDictionary(Of Integer, RequestModule)
        Private Shared _Initialized As Boolean = False

        Private Sub New()
            RequestModule._Instance.TryAdd(Me.GetHashCode(), Me)

            If RequestModule._Initialized Then Exit Sub
            RequestModule._Initialized = True

            ' Application Domain UnHandled Exception Event Handling Defination
            AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf Me.OnUnhandledExceptions)
            ' !---

            ' If not already initialized, initialize timer and configuration.
            Threading.Monitor.Enter(Me)
            Try
                ' Get the configuration section and set timeout and CookieMode values.
                Dim cfg As System.Configuration.Configuration =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)

                ' Take care framework defaults
                Dim IsModified As Boolean = Me.Configure(cfg)

                If IsModified Then
                    cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                            System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                End If

                Dim sConfig As System.Web.Configuration.SessionStateSection =
                    CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)

                RemoteInvoke.XeoraSettings = CType(cfg.GetSection("xeora"), Configuration.XeoraSection)

                Session.SessionManager.Current.SessionStateMode = sConfig.Mode
                Session.SessionManager.Current.Timeout = CInt(sConfig.Timeout.TotalMinutes)

                If Session.SessionManager.Current.SessionStateMode = System.Web.SessionState.SessionStateMode.Off Then _
                    Session.SessionManager.Current.CookieMode = sConfig.Cookieless
            Finally
                Threading.Monitor.Exit(Me)
            End Try

            ApplicationLoader.Current.Initialize()
        End Sub

        Private Sub Init(ByVal app As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            ' Add event handlers.
            AddHandler app.BeginRequest, New EventHandler(AddressOf Me.OnBeginRequest)
            AddHandler app.AcquireRequestState, New EventHandler(AddressOf Me.OnAcquireRequestState)
            AddHandler app.PreRequestHandlerExecute, New EventHandler(AddressOf Me.OnPreRequestHandlerExecute)
            AddHandler app.PostRequestHandlerExecute, New EventHandler(AddressOf Me.OnPostRequestHandlerExecute)
            AddHandler app.ReleaseRequestState, New EventHandler(AddressOf Me.OnReleaseRequestState)
            AddHandler app.EndRequest, New EventHandler(AddressOf Me.OnEndRequest)
            ' !---
        End Sub

        Private Function Configure(ByRef cfg As System.Configuration.Configuration) As Boolean
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

            Return IsModified
        End Function

        '
        ' Unhandled Exception Logging for AppDomain
        '
        Private Sub OnUnhandledExceptions(ByVal source As Object, ByVal args As UnhandledExceptionEventArgs)
            If Not args.ExceptionObject Is Nothing AndAlso
                TypeOf args.ExceptionObject Is System.Exception Then

                Helper.EventLogger.LogToSystemEvent(
                    String.Format(" --- RequestModule Exception --- {0}{0}{1}", Environment.NewLine, CType(args.ExceptionObject, System.Exception).ToString()),
                    EventLogEntryType.Error
                )
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

            ' Define a RequestID and ApplicationID for XeoraCube
            app.Context.Items.Add("RequestID", Guid.NewGuid().ToString())
            app.Context.Items.Add("ApplicationID", ApplicationLoader.Current.ApplicationID)
        End Sub

        '
        ' Event handler for HttpApplication.AcquireRequestState
        '
        Private Sub OnAcquireRequestState(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            If Session.SessionManager.Current.SessionStateMode = System.Web.SessionState.SessionStateMode.Off Then _
                Session.SessionManager.Current.Acquire(context)
        End Sub

        '
        ' Event handler for HttpApplication.PreRequestHandlerExecute
        '
        Private Sub OnPreRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            Web.Context.ContextManager.Current.Add(
                CType(context.Items.Item("RequestID"), String),
                New Context.ContextContainer(False, New Context.BuildInContext(context))
            )
        End Sub

        '
        ' Event handler for HttpApplication.PostRequestHandlerExecute
        '
        Private Sub OnPostRequestHandlerExecute(ByVal source As Object, ByVal args As EventArgs)
            ' Do Nothing
        End Sub

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnReleaseRequestState(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            If Session.SessionManager.Current.SessionStateMode = System.Web.SessionState.SessionStateMode.Off Then _
                Session.SessionManager.Current.Release(context)
        End Sub

        '
        ' Event handler for HttpApplication.ReleaseRequestState
        '
        Private Sub OnEndRequest(ByVal source As Object, ByVal args As EventArgs)
            Dim context As System.Web.HttpContext = CType(source, System.Web.HttpApplication).Context

            CType(source, System.Web.HttpApplication).CompleteRequest()

            Web.Context.ContextManager.Current.Remove(
                CType(context.Items.Item("RequestID"), String))
        End Sub

        Friend Shared Sub DisposeAll()
            Dim Keys As Integer() = CType(Array.CreateInstance(GetType(Object), RequestModule._Instance.Keys.Count), Integer())
            RequestModule._Instance.Keys.CopyTo(Keys, 0)

            For Each Key As Integer In Keys
                Dim RequestModule As RequestModule = Nothing

                RequestModule._Instance.TryGetValue(Key, RequestModule)

                If Not RequestModule Is Nothing Then RequestModule.Dispose()
            Next

            ApplicationLoader.Current.CleanUp()
            RequestModule._Initialized = False
        End Sub

        Public Sub Dispose() Implements System.Web.IHttpModule.Dispose
            RequestModule._Instance.TryRemove(Me.GetHashCode(), Nothing)
        End Sub
    End Class
End Namespace