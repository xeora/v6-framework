Namespace SolidDevelopment.Web
    Public Class RequestHandler
        Implements System.Web.IHttpAsyncHandler
        Implements System.Web.SessionState.IRequiresSessionState

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Function BeginProcessRequest(ByVal context As System.Web.HttpContext, ByVal CallBack As System.AsyncCallback, ByVal State As Object) As System.IAsyncResult Implements System.Web.IHttpAsyncHandler.BeginProcessRequest
            ' Prepare Context
            Dim RequestID As String = _
                CType(context.Items.Item("RequestID"), String)

            Dim XeoraHandler As DoXeoraDelegate = _
                New DoXeoraDelegate(AddressOf Me.DoXeora)

            Return XeoraHandler.BeginInvoke(RequestID, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                             Try
                                                                                 XeoraHandler.EndInvoke(aR)
                                                                             Catch ex As Exception
                                                                                 ' Just Handle Exceptions
                                                                                 ' I catch this error when i was testing the videostream of Mayadroom
                                                                                 ' w3wp.exe create an exception which is Remote Host Close the Connection
                                                                                 ' Which was causing to stop the Website...
                                                                             End Try

                                                                             CallBack.Invoke(aR)
                                                                         End Sub), State)
        End Function

        Private Delegate Sub DoXeoraDelegate(ByVal RequestID As String)
        Private Sub DoXeora(ByVal RequestID As String)
            XeoraHandler.Start(RequestID)
        End Sub

        Public Sub EndProcessRequest(ByVal result As System.IAsyncResult) Implements System.Web.IHttpAsyncHandler.EndProcessRequest
            ' Do Nothing
        End Sub

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            Throw New InvalidOperationException()
        End Sub

        Private Class XeoraHandler
            Private _RequestID As String

            Private _BeginRequestTime As Date
            Private _SupportCompression As Boolean
            Private _ThemeWebControl As Managers.ThemeWebControl

            Private Sub New(ByVal RequestID As String)
                Me._RequestID = RequestID
            End Sub

            Public Shared Sub Start(ByVal RequestID As String)
                Dim Context As System.Web.HttpContext = _
                    RequestModule.Context(RequestID)

                If Context Is Nothing Then Exit Sub

                Dim XeoraHandler As New XeoraHandler(RequestID)
                XeoraHandler.HandleRequest(Context)
            End Sub

            Private Sub HandleRequest(ByVal context As System.Web.HttpContext)
                If context Is Nothing Then Exit Sub

                ' Prepare RequestDateTime and Compression Support
                Me._BeginRequestTime = Date.Now
                Me._SupportCompression = False

                ' Make Available Context for Current Async Call
                SolidDevelopment.Web.General.AssignRequestID( _
                    CType(context.Items.Item("RequestID"), String))
                ' !--

                General.Context.Items.Add("_sys_TemplateRequest", False)

                Dim IsEO As Boolean = False

                Try
                    ' These values will change according to users settings but if it is first visit, it will be default
                    Dim ThemeID As String = General.CurrentThemeID, TranslationID As String = General.CurrentThemeTranslationID
                    ' Create with default ones
                    Me._ThemeWebControl = New Managers.ThemeWebControl(ThemeID, TranslationID)

                    Dim URLMI As PGlobals.URLMappingInfos = _
                        Me._ThemeWebControl.URLMappingInfo

                    ' Resolve If Request is Mapped (and if urlmapping is active)
                    If URLMI.URLMapping Then
                        Dim ResolvedMapped As PGlobals.URLMappingInfos.ResolvedMapped = _
                            URLMI.ResolveMappedURL(General.Context.Request.RawUrl)

                        If Not ResolvedMapped Is Nothing AndAlso _
                            ResolvedMapped.IsResolved Then ' this is a mapped request

                            Dim QueryString As New System.Text.StringBuilder

                            For qS As Integer = 0 To ResolvedMapped.QueryStrings.Count - 1
                                QueryString.AppendFormat("{0}={1}", _
                                    ResolvedMapped.QueryStrings.Item(qS).ID, _
                                    ResolvedMapped.QueryStrings.Item(qS).Value _
                                )

                                If ResolvedMapped.QueryStrings.Count > (qS + 1) Then _
                                    QueryString.Append("&")
                            Next

                            Dim RequestURL As String = _
                                String.Format("{0}{1}", _
                                    Configurations.ApplicationRoot.BrowserSystemImplementation, _
                                    ResolvedMapped.TemplateID _
                                )
                            If QueryString.Length > 0 Then _
                                RequestURL = String.Concat(RequestURL, "?", QueryString.ToString())

                            General.Context.RewritePath(RequestURL)
                        End If
                    End If
                    ' !---

                    If General.Context.Response.ContentEncoding Is Nothing Then General.Context.Response.ContentEncoding = System.Text.Encoding.UTF8

                    If Not General.Context.Request.QueryString.Item("nocache") Is Nothing Then
                        General.Context.Response.CacheControl = "no-cache"
                        General.Context.Response.AddHeader("Pragma", "no-cache")
                        General.Context.Response.Expires = -1
                        General.Context.Response.ExpiresAbsolute = Date.Today.AddMilliseconds(-1)

                        Select Case General.Context.Request.QueryString.Item("nocache")
                            Case "L2", "L2XC"
                                System.Web.HttpResponse.RemoveOutputCacheItem(General.Context.Request.FilePath)
                        End Select

                        ' Session Cookie Option
                        Dim CookilessSessionString As String = _
                            String.Format("{0}_Cookieless", Configurations.VirtualRoot.Replace("/"c, "_"c))
                        Select Case General.Context.Request.QueryString.Item("nocache")
                            Case "L0XC", "L1XC", "L2XC"
                                General.Context.Session.Contents.Item(CookilessSessionString) = True
                            Case Else
                                General.Context.Session.Contents.Item(CookilessSessionString) = False
                        End Select
                        ' !--
                    Else
                        General.Context.Response.ExpiresAbsolute = Date.Today.AddYears(1)
                    End If

                    Dim AcceptEncodings As String = _
                        General.Context.Request.ServerVariables.Item("HTTP_ACCEPT_ENCODING")
                    If Not AcceptEncodings Is Nothing Then _
                        Me._SupportCompression = (AcceptEncodings.IndexOf("gzip") > -1)

                    ' Check Requested Path For Template
                    Dim RequestedTemplate As String = _
                        SolidDevelopment.Web.General.ResolveTemplateFromURL(General.Context.Request.RawUrl)

                    If RequestedTemplate Is Nothing Then
                        ' Requested File is not a WebService Or Template

                        Dim ThemePublicContentsPath As String = _
                            General.GetThemePublicContentsPath(ThemeID, TranslationID)
                        Dim RequestedFileVirtualPath As String = _
                            General.Context.Request.Path

                        If RequestedFileVirtualPath.IndexOf(ThemePublicContentsPath) = -1 Then
                            ' This is also not a request for default ThemePublicContents

                            ' Exam Match Values
                            Dim ControlThemeID As String = ThemeID, ControlTranslationID As String = TranslationID
                            Me.FindRequestPathThemeAndTranslation(RequestedFileVirtualPath, ControlThemeID, ControlTranslationID)

                            ' If matched one is not the same with the default one
                            ' Create the matched ThemeWebControl and Update ThemePublicContentsPath
                            If String.Compare(ThemeID, ControlThemeID) <> 0 OrElse _
                                String.Compare(TranslationID, ControlTranslationID) <> 0 Then

                                Me._ThemeWebControl = New Managers.ThemeWebControl(ControlThemeID, ControlTranslationID)

                                ' Update ThemeWebPath variable with the new ones
                                ThemePublicContentsPath = General.GetThemePublicContentsPath(ControlThemeID, ControlTranslationID)
                            End If
                        End If

                        ' Provide Requested File Stream
                        If RequestedFileVirtualPath.IndexOf(ThemePublicContentsPath) > -1 Then
                            Me.PostPublicContentFileToClient(RequestedFileVirtualPath)
                        Else
                            Me.PostRequestedStaticFileToClient()
                        End If
                    Else
                        Dim IsScriptRequesting As Boolean = _
                            String.Compare( _
                                RequestedTemplate, _
                                String.Format("_bi_sps_v{0}.js", Me._ThemeWebControl.BuiltInScriptVersion) _
                            ) = 0

                        If IsScriptRequesting Then
                            Me.PostBuildInJavaScriptToClient()
                        Else
                            ' Mark this request is for TemplateRequest
                            General.Context.Items.Item("_sys_TemplateRequest") = True

                            ' Set Title Globals
                            General.SiteTitle = Me._ThemeWebControl.Theme.Translation.GetTranslation("SITETITLE")

                            ' This is a WebService or Template request
                            Me._ThemeWebControl.ServiceID = RequestedTemplate
                            Me._ThemeWebControl.BlockRenderingID = General.Context.Request.Headers.Item("X-BlockRenderingID")

                            ' If Theme is not found in the theme calling path, then try to pass the file stream
                            If Me._ThemeWebControl.ServiceID Is Nothing Then
                                Me.PostRequestedStaticFileToClient()

                                GoTo QUICKFINISH
                            End If

                            If String.Compare(General.Context.Request.HttpMethod, "GET", True) = 0 Then
                                ' Check is hashcode is assign to the requested template
                                Dim ParentURL As String = _
                                    General.Context.Request.FilePath

                                ParentURL = ParentURL.Remove(0, ParentURL.IndexOf(Configurations.ApplicationRoot.BrowserSystemImplementation) + Configurations.ApplicationRoot.BrowserSystemImplementation.Length)

                                Dim mR_Parent As System.Text.RegularExpressions.Match = _
                                    System.Text.RegularExpressions.Regex.Match(ParentURL, String.Format("\d+/{0}", RequestedTemplate))

                                If Not mR_Parent.Success Then
                                    Dim RewrittenPath As String = _
                                        String.Format("{0}{1}/{2}", Configurations.ApplicationRoot.BrowserSystemImplementation, General.HashCode, RequestedTemplate)
                                    Dim QueryString As String = _
                                        General.Context.Request.ServerVariables.Item("QUERY_STRING")

                                    If Not String.IsNullOrEmpty(QueryString) Then _
                                        RewrittenPath = String.Format("{0}?{1}", RewrittenPath, QueryString)

                                    General.Context.RewritePath(RewrittenPath)
                                End If
                            End If

                            If Me._ThemeWebControl.IsAuthenticationRequired Then
                                Me.RedirectToAuthenticationPage(RequestedTemplate)
                            Else
                                Dim MethodResultContent As String = Nothing

                                If Me._ThemeWebControl.ServiceType = Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template AndAlso _
                                    String.Compare(General.Context.Request.HttpMethod, "POST", True) = 0 AndAlso _
                                    Not String.IsNullOrEmpty(General.Context.Request.Form.Item("PostBackInformation")) Then

                                    Dim IsAddonsCalled As Boolean = False
                                    Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo

                                    ' Decode Encoded Call Function to Readable
                                    Dim AssembleInfo As String = _
                                        Managers.Assembly.DecodeCallFunction( _
                                            General.Context.Request.Form.Item("PostBackInformation"))

                                    If Me._ThemeWebControl.Theme.Addons.CurrentInstance Is Nothing Then
PARENTCALL:
                                        tAssembleResultInfo = _
                                            Managers.Assembly.AssemblePostBackInformation(AssembleInfo)

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException AndAlso _
                                            Not IsAddonsCalled Then

                                            Dim CallingDllName As String = tAssembleResultInfo.DllName
                                            For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._ThemeWebControl.Theme.Addons.AddonInfos
                                                If String.Compare(Addon.AddonID, CallingDllName, True) = 0 Then
                                                    Me._ThemeWebControl.Theme.Addons.CreateInstance(Addon)

                                                    tAssembleResultInfo = _
                                                        Managers.Assembly.AssemblePostBackInformation( _
                                                                        Me._ThemeWebControl.Theme.CurrentID, _
                                                                        Me._ThemeWebControl.Theme.Addons.CurrentInstance.CurrentID, _
                                                                        AssembleInfo, Nothing)

                                                    Me._ThemeWebControl.Theme.Addons.DisposeInstance()
                                                End If
                                            Next
                                        End If
                                    Else
                                        IsAddonsCalled = True

                                        tAssembleResultInfo = _
                                            Managers.Assembly.AssemblePostBackInformation( _
                                                            Me._ThemeWebControl.Theme.CurrentID, _
                                                            Me._ThemeWebControl.Theme.Addons.CurrentInstance.CurrentID, _
                                                            AssembleInfo, Nothing)

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                                            GoTo PARENTCALL
                                        End If
                                    End If

                                    If Not tAssembleResultInfo.FunctionParams Is Nothing Then
                                        Dim ArgumentValueList As Object() = New Object() {}
                                        Managers.Assembly.PrepareArguments(Nothing, tAssembleResultInfo.FunctionParams, ArgumentValueList)

                                        For pC As Integer = 0 To tAssembleResultInfo.FunctionParams.Length - 1
                                            Me._ThemeWebControl.ArgumentList.Add( _
                                                tAssembleResultInfo.FunctionParams(pC), _
                                                ArgumentValueList(pC) _
                                            )
                                        Next
                                    End If

                                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                        TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                                        Me._ThemeWebControl.MessageResult = New PGlobals.MapControls.MessageResult(CType(tAssembleResultInfo.MethodResult, Exception).ToString())

                                        Me.WritePage(String.Empty)
                                    ElseIf Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.MessageResult Then

                                        Me._ThemeWebControl.MessageResult = CType(tAssembleResultInfo.MethodResult, SolidDevelopment.Web.PGlobals.MapControls.MessageResult)

                                        Me.WritePage(String.Empty)
                                    ElseIf Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder Then

                                        SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                                        SolidDevelopment.Web.General.Context.Items.Add( _
                                            "RedirectLocation", _
                                            CType(tAssembleResultInfo.MethodResult, SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder).Location _
                                        )
                                    Else
                                        MethodResultContent = SolidDevelopment.Web.PGlobals.Execution.GetPrimitiveValue(tAssembleResultInfo.MethodResult)

                                        Select Case Me._ThemeWebControl.ServiceType
                                            Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template
                                                Me.WritePage(MethodResultContent)

                                            Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice
                                                Me.WritePage()

                                        End Select
                                    End If
                                Else
                                    Select Case Me._ThemeWebControl.ServiceType
                                        Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template
                                            Me.WritePage(MethodResultContent)

                                        Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice
                                            Me.WritePage()

                                    End Select
                                End If
                            End If
                        End If
                    End If
                    ' !--
QUICKFINISH:
                Catch ex As Exception
                    If TypeOf ex Is System.Web.HttpException AndAlso Not Configurations.LogHTTPExceptions Then Exit Try

                    Dim CurrentContextCatch As System.Web.HttpContext = _
                        SolidDevelopment.Web.General.Context

                    If CurrentContextCatch Is Nothing Then CurrentContextCatch = context

                    ' Prepare For Exception List
                    Dim CompiledExceptions As New Text.StringBuilder

                    CompiledExceptions.AppendLine("-- APPLICATION EXCEPTION --")
                    CompiledExceptions.Append(ex.ToString())
                    CompiledExceptions.AppendLine()
                    ' ----

                    Dim LogResult As New Text.StringBuilder()

                    LogResult.AppendLine("-- Session Variables --")
                    ' -- Session Log Text
                    Dim SessionENumerator As IEnumerator = CurrentContextCatch.Session.GetEnumerator()

                    If Not SessionENumerator Is Nothing Then
                        Dim KeyItem As String

                        Try
                            Do While SessionENumerator.MoveNext()
                                KeyItem = CType(SessionENumerator.Current, String)

                                LogResult.AppendLine(String.Format(" {0} -> {1}", KeyItem, CurrentContextCatch.Session.Contents.Item(KeyItem)))
                            Loop
                        Catch exSession As Exception
                            ' The collection was modified after the enumerator was created.

                            LogResult.AppendLine(String.Format(" Exception Occured -> {0}", exSession.Message))
                        End Try
                    End If
                    ' !--
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Request POST Variables --")
                    For Each KeyItem As String In CurrentContextCatch.Request.Form
                        LogResult.AppendLine( _
                            String.Format(" {0} -> {1}", KeyItem, CurrentContextCatch.Request.Form.Item(KeyItem)) _
                        )
                    Next
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Request URL & Query String --")
                    LogResult.AppendLine( _
                        String.Format("{0}?{1}", _
                            CurrentContextCatch.Request.ServerVariables.Item("URL"), _
                            CurrentContextCatch.Request.ServerVariables.Item("QUERY_STRING") _
                        ) _
                    )
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Error Content --")
                    LogResult.Append(ex.ToString())

                    Try
                        SolidDevelopment.Web.Helpers.EventLogging.WriteToLog(LogResult.ToString())
                    Catch exLogging As Exception
                        CompiledExceptions.AppendLine("-- TXT LOGGING EXCEPTION --")
                        CompiledExceptions.Append(exLogging.ToString())
                        CompiledExceptions.AppendLine()

                        Try
                            If Not System.Diagnostics.EventLog.SourceExists("XeoraCube") Then System.Diagnostics.EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                            System.Diagnostics.EventLog.WriteEntry("XeoraCube", exLogging.ToString(), EventLogEntryType.Error)
                        Catch exLogging02 As Exception
                            CompiledExceptions.AppendLine("-- SYSTEM EVENT LOGGING EXCEPTION --")
                            CompiledExceptions.Append(exLogging02.ToString())
                            CompiledExceptions.AppendLine()
                        End Try
                    End Try

                    If SolidDevelopment.Web.Configurations.Debugging Then
                        CurrentContextCatch.Response.Clear()
                        CurrentContextCatch.Response.Write("<h2 align=""center"" style=""color:#CC0000"">" & Globals.SystemMessages.SYSTEM_ERROROCCURED & "!</h2>")
                        CurrentContextCatch.Response.Write("<hr size=""1px"">")
                        CurrentContextCatch.Response.Write("<pre>" & CompiledExceptions.ToString() & "</pre>")
                        CurrentContextCatch.Response.End()

                        IsEO = True
                    Else
                        If Not Me._ThemeWebControl Is Nothing Then
                            SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                            SolidDevelopment.Web.General.Context.Items.Add( _
                                "RedirectLocation", _
                                String.Format("http://{0}{1}", _
                                    CurrentContextCatch.Request.ServerVariables("HTTP_HOST"), _
                                    General.GetPageToRedirect( _
                                        False, _
                                        Me._ThemeWebControl.Theme.Settings.Configurations.DefaultPage, _
                                        Me._ThemeWebControl.Theme.Settings.Configurations.DefaultCaching _
                                    ) _
                                ) _
                            )
                        Else
                            CurrentContextCatch.Response.Clear()
                            CurrentContextCatch.Response.Write("<h2 align=""center"" style=""color:#CC0000"">" & Globals.SystemMessages.SYSTEM_ERROROCCURED & "!</h2>")
                            CurrentContextCatch.Response.Write("<h4 align=""center"">" & ex.Message & "</h4>")
                            CurrentContextCatch.Response.End()

                            IsEO = True
                        End If
                    End If
                Finally
                    If Not Me._ThemeWebControl Is Nothing Then Me._ThemeWebControl.Dispose()

                    If SolidDevelopment.Web.General.Context.Items.Contains("RedirectLocation") Then

                        If CType(SolidDevelopment.Web.General.Context.Items.Item("RedirectLocation"), String).IndexOf("://") = -1 Then
                            Dim RedirectLocation As String = _
                                String.Format("http://{0}{1}", _
                                        SolidDevelopment.Web.General.Context.Request.ServerVariables("HTTP_HOST"), _
                                        SolidDevelopment.Web.General.Context.Items.Item("RedirectLocation") _
                                    )

                            SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                            SolidDevelopment.Web.General.Context.Items.Add( _
                                "RedirectLocation", _
                                RedirectLocation _
                            )
                        End If

                        If General.Context.Request.Headers.Item("X-BlockRenderingID") Is Nothing Then
                            Try
                                SolidDevelopment.Web.General.Context.Response.Redirect(CType(SolidDevelopment.Web.General.Context.Items.Item("RedirectLocation"), String), True)
                            Catch ex As System.Web.HttpException
                                ' Just Handle Exceptions (Remote host closed the connection )
                            Catch ex As Exception
                                Throw ex
                            End Try
                        Else
                            SolidDevelopment.Web.General.Context.Response.Clear()
                            SolidDevelopment.Web.General.Context.Response.Buffer = True
                            SolidDevelopment.Web.General.Context.Response.Write( _
                                String.Format("rl:{0}", CType(SolidDevelopment.Web.General.Context.Items.Item("RedirectLocation"), String)) _
                            )
                            SolidDevelopment.Web.General.Context.Response.End()
                        End If
                    End If
                End Try
            End Sub

            Private Sub FindRequestPathThemeAndTranslation(ByVal SearchPath As String, ByRef ThemeID As String, ByRef TranslationID As String)
                If Not String.IsNullOrEmpty(SearchPath) Then
                    Dim IsDone As Boolean = False
                    Dim themes As PGlobals.ThemeInfoCollection = _
                        Managers.ThemeWebControl.GetAvailableThemes()

                    Dim tempPath As String
                    For Each themeInfo As PGlobals.ThemeInfo In themes
                        For Each transInfo As PGlobals.ThemeInfo.ThemeTranslationInfo In themeInfo.ThemeTranslations
                            tempPath = General.GetThemePublicContentsPath(themeInfo.ThemeID, transInfo.Code)

                            If SearchPath.IndexOf(tempPath) > -1 Then
                                ThemeID = themeInfo.ThemeID
                                TranslationID = transInfo.Code

                                IsDone = True : Exit For
                            End If
                        Next

                        If IsDone Then Exit For
                    Next
                End If
            End Sub

            Private Sub PostRequestedStaticFileToClient()
                ' This is a common file located somewhere in the webserver
                Dim RequestFilePath As String = _
                    General.Context.Request.PhysicalPath

                If Not IO.File.Exists(RequestFilePath) Then
                    SolidDevelopment.Web.General.Context.Response.StatusCode = 404
                Else
                    Dim RequestFileStream As IO.Stream = _
                        New System.IO.FileStream(RequestFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                    Me.WriteOutput( _
                            General.GetMimeType( _
                                IO.Path.GetExtension(RequestFilePath) _
                            ), _
                            RequestFileStream, _
                            Me._SupportCompression _
                        )

                    RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                    ' Remove RedirectLocation if it exists
                    SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                End If
            End Sub

            Private Sub PostPublicContentFileToClient(ByVal RequestedFileVirtualPath As String)
                ' This is a well known Public Content file
                Dim ThemePublicContentsPathName As String = _
                    IO.Path.GetDirectoryName(RequestedFileVirtualPath)

                Dim RequestedFilePath As String = _
                    IO.Path.GetFileName(RequestedFileVirtualPath)

                Dim RequestFileStream As IO.Stream = Nothing

                ' Write File Content To Theme File Stream
                Me._ThemeWebControl.ProvideFileStream(RequestFileStream, ThemePublicContentsPathName, RequestedFilePath)

                If RequestFileStream Is Nothing Then
                    SolidDevelopment.Web.General.Context.Response.StatusCode = 404
                Else
                    Me.WriteOutput( _
                            General.GetMimeType( _
                                IO.Path.GetExtension(RequestedFileVirtualPath) _
                            ), _
                            RequestFileStream, _
                            Me._SupportCompression _
                        )

                    RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                    SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                End If
            End Sub

            Private Sub PostBuildInJavaScriptToClient()
                ' This is a Script file request
                Dim RequestFileStream As IO.Stream = Nothing

                ' Write File Content To BuiltIn Script Stream
                Me._ThemeWebControl.ProvideBuiltInScriptStream(RequestFileStream)

                Me.WriteOutput( _
                            General.GetMimeType(".js"), _
                            RequestFileStream, _
                            Me._SupportCompression _
                        )

                RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                ' Remove RedirectLocation if it exists
                SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
            End Sub

            Private Sub RedirectToAuthenticationPage(Optional ByVal CurrentRequestedTemplate As String = Nothing)
                Select Case Me._ThemeWebControl.ServiceType
                    Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template
                        ' Get AuthenticationPage 
                        Dim AuthenticationPage As String = String.Empty
                        If Me._ThemeWebControl.Theme.Addons.CurrentInstance Is Nothing Then
                            AuthenticationPage = Me._ThemeWebControl.Theme.Settings.Configurations.AuthenticationPage
                        Else
                            AuthenticationPage = Me._ThemeWebControl.Theme.Addons.CurrentInstance.Settings.Configurations.AuthenticationPage
                        End If

                        If Not String.IsNullOrEmpty(CurrentRequestedTemplate) AndAlso _
                            String.Compare(AuthenticationPage, CurrentRequestedTemplate, True) <> 0 Then

                            General.Context.Session.Contents.Item("_sys_Referer") = _
                                General.Context.Request.RawUrl
                        End If

                        ' Remove Redirect Location IF Exists
                        SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                        ' Reset Redirect Location to AuthenticationPage
                        SolidDevelopment.Web.General.Context.Items.Add( _
                            "RedirectLocation", _
                            SolidDevelopment.Web.General.GetPageToRedirect( _
                                True, _
                                AuthenticationPage, _
                                PGlobals.PageCachingTypes.TextsOnly _
                            ) _
                        )

                    Case Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice
                        Me.WritePage()

                End Select
            End Sub

            Private Overloads Sub WritePage()
                Dim sW As New IO.StringWriter

                sW.Write("<?xml version=""1.0"" encoding=""utf-8"" ?>")

                Me._ThemeWebControl.RenderService(General.CurrentRequestID)
                sW.Write(Me._ThemeWebControl.RenderedService)

                sW.Flush()
                sW.Close()

                Me.WriteOutput(Me._ThemeWebControl.MimeType, sW.ToString(), Me._SupportCompression)
            End Sub

            Private Overloads Sub WritePage(ByVal MethodResultContent As String)
                ' Render Page
                Me._ThemeWebControl.RenderService(General.CurrentRequestID)

                Dim CurrentContextCatch As System.Web.HttpContext = _
                        SolidDevelopment.Web.General.Context

                Dim sW As New IO.StringWriter

                If Not Me._ThemeWebControl.IsWorkingAsStandAlone AndAlso _
                    Me._ThemeWebControl.BlockRenderingID Is Nothing Then

                    If Configurations.UseHTML5Header Then
                        sW.WriteLine("<!doctype html>")
                    Else
                        sW.WriteLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">")
                    End If

                    sW.WriteLine("<html>")
                    sW.WriteLine("<head>")

                    Dim IsContentTypeAdded As Boolean = False, IsPragmaAdded As Boolean = False, IsCacheControlAdded As Boolean = False, IsExpiresAdded As Boolean = False

                    For Each kVP As Generic.KeyValuePair(Of General.MetaRecords.MetaTags, String) In General.MetaRecords.RegisteredMetaRecords
                        Select Case General.MetaRecords.QueryMetaTagSpace(kVP.Key)
                            Case General.MetaRecords.MetaTagSpace.name
                                sW.WriteLine( _
                                        String.Format( _
                                            "<meta name=""{0}"" content=""{1}"" />", _
                                            General.MetaRecords.GetMetaTagHtmlName(kVP.Key), _
                                            kVP.Value _
                                        ) _
                                    )
                            Case General.MetaRecords.MetaTagSpace.httpequiv
                                sW.WriteLine( _
                                    String.Format( _
                                        "<meta http-equiv=""{0}"" content=""{1}"" />", _
                                        General.MetaRecords.GetMetaTagHtmlName(kVP.Key), _
                                        kVP.Value _
                                    ) _
                                )
                        End Select

                        Select Case kVP.Key
                            Case General.MetaRecords.MetaTags.contenttype
                                IsContentTypeAdded = True
                            Case General.MetaRecords.MetaTags.pragma
                                IsPragmaAdded = True
                            Case General.MetaRecords.MetaTags.cachecontrol
                                IsCacheControlAdded = True
                            Case General.MetaRecords.MetaTags.expires
                                IsExpiresAdded = True
                        End Select
                    Next

                    Dim KeyName As String = String.Empty

                    For Each kVP As Generic.KeyValuePair(Of String, String) In General.MetaRecords.RegisteredCustomMetaRecords
                        KeyName = kVP.Key

                        Select Case General.MetaRecords.QueryMetaTagSpace(KeyName)
                            Case General.MetaRecords.MetaTagSpace.name
                                sW.WriteLine( _
                                        String.Format( _
                                            "<meta name=""{0}"" content=""{1}"" />", _
                                            KeyName, _
                                            kVP.Value _
                                        ) _
                                    )
                            Case General.MetaRecords.MetaTagSpace.httpequiv
                                sW.WriteLine( _
                                    String.Format( _
                                        "<meta http-equiv=""{0}"" content=""{1}"" />", _
                                        KeyName, _
                                        kVP.Value _
                                    ) _
                                )
                        End Select
                    Next

                    If Not IsContentTypeAdded Then
                        sW.WriteLine( _
                            String.Format( _
                                "<meta http-equiv=""Content-Type"" content=""{0}; charset={1}"" />", _
                                Me._ThemeWebControl.MimeType, _
                                CurrentContextCatch.Response.ContentEncoding.WebName _
                            ) _
                        )
                    End If

                    If Not CurrentContextCatch.Request.QueryString.Item("nocache") Is Nothing AndAlso _
                        ( _
                            String.Compare(CurrentContextCatch.Request.QueryString.Item("nocache"), "L2", True) = 0 OrElse _
                            String.Compare(CurrentContextCatch.Request.QueryString.Item("nocache"), "L2XC", True) = 0 _
                        ) Then

                        If Not IsPragmaAdded Then sW.WriteLine("<meta http-equiv=""Pragma"" content=""no-cache"" />")
                        If Not IsCacheControlAdded Then sW.WriteLine("<meta http-equiv=""Cache-Control"" content=""no-cache"" />")
                        If Not IsExpiresAdded Then sW.WriteLine("<meta http-equiv=""Expires"" content=""0"" />")
                    End If

                    sW.WriteLine( _
                        String.Format( _
                            "<title>{0}</title>", _
                            General.SiteTitle _
                        ) _
                    )

                    If Not String.IsNullOrEmpty(General.SiteIconURL) Then
                        sW.WriteLine( _
                            String.Format( _
                                "<link href=""{0}"" rel=""shortcut icon"">", _
                                General.SiteIconURL _
                            ) _
                        )
                    End If

                    Dim AddonID As String = Nothing

                    If Not Me._ThemeWebControl.Theme.Addons.CurrentInstance Is Nothing Then _
                        AddonID = Me._ThemeWebControl.Theme.Addons.CurrentInstance.CurrentID

                    sW.WriteLine( _
                        String.Format( _
                            "<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />", _
                            IO.Path.Combine( _
                                General.GetThemePublicContentsPath( _
                                    Me._ThemeWebControl.Theme.CurrentID, _
                                    Me._ThemeWebControl.Theme.Translation.CurrentTranslationID, _
                                    AddonID _
                                ), "styles.css").Replace("\"c, "/"c) _
                        ) _
                    )

                    sW.WriteLine( _
                        String.Format( _
                            "<script language=""javascript"" src=""{0}_bi_sps_v{1}.js"" type=""text/javascript""></script>", _
                            Configurations.ApplicationRoot.BrowserSystemImplementation, _
                            Me._ThemeWebControl.BuiltInScriptVersion _
                        ) _
                    )

                    sW.WriteLine("</head>")
                    sW.WriteLine("<body>")
                    sW.WriteLine( _
                        String.Format( _
                            "<form method=""post"" action=""{0}?{1}"" enctype=""multipart/form-data"" style=""margin: 0px; padding: 0px;"">", _
                            CurrentContextCatch.Request.Path, _
                            CurrentContextCatch.Request.ServerVariables.Item("QUERY_STRING") _
                        ) _
                    )
                    sW.WriteLine("<input type=""hidden"" name=""PostBackInformation"" id=""PostBackInformation"" />")
                End If

                sW.Write("{0}")

                If Not Me._ThemeWebControl.IsWorkingAsStandAlone AndAlso _
                    Me._ThemeWebControl.BlockRenderingID Is Nothing Then sW.WriteLine("</form>")

                sW.Write(MethodResultContent)

                If Not Me._ThemeWebControl.IsWorkingAsStandAlone AndAlso _
                    Me._ThemeWebControl.BlockRenderingID Is Nothing Then

                    sW.WriteLine("</body>")
                    sW.WriteLine("</html>")
                End If

                sW.Flush()
                sW.Close()

                Dim RenderedService As String = String.Empty

                RenderedService = sW.ToString()
                RenderedService = String.Format(RenderedService, Me._ThemeWebControl.RenderedService)

                Dim sys_RenderDurationMark As String = "<!--_sys_PAGERENDERDURATION-->"
                Dim idxRenderDurationMark As Integer = _
                    RenderedService.IndexOf(sys_RenderDurationMark)

                If idxRenderDurationMark > -1 Then
                    Dim EndRequestTimeSpan As TimeSpan = Date.Now.Subtract(Me._BeginRequestTime)

                    RenderedService = RenderedService.Remove(idxRenderDurationMark, sys_RenderDurationMark.Length)
                    RenderedService = RenderedService.Insert(idxRenderDurationMark, EndRequestTimeSpan.TotalMilliseconds.ToString())
                End If

                Me.WriteOutput(Me._ThemeWebControl.MimeType, RenderedService, Me._SupportCompression)
            End Sub

            Private Overloads Sub WriteOutput(ByVal ContentType As String, ByVal OutputContent As String, ByVal SendAsCompressed As Boolean)
                ' This header for examing Response.End and also sent to the browser about the version of Solid Web Content
                Dim ResponseEndRaised As Boolean = False
                Try
                    Dim vI As System.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version

                    SolidDevelopment.Web.General.Context.Response.AppendHeader("X-FrameworkVersion", String.Format("{0}.{1}.{2}", vI.Major, vI.Minor, vI.Build))
                Catch ex As System.Web.HttpException
                    ResponseEndRaised = True
                Catch ex As Exception
                    Throw ex
                End Try
                ' !--

                If Not ResponseEndRaised AndAlso _
                    Not SolidDevelopment.Web.General.Context.Items.Contains("RedirectLocation") Then

                    Dim OutputStream As IO.Stream = _
                        New IO.MemoryStream( _
                            System.Text.Encoding.UTF8.GetBytes(OutputContent) _
                        )

                    ' Send Output put To Client
                    Me.WriteOutput(ContentType, OutputStream, SendAsCompressed)

                    OutputStream.Close() : GC.SuppressFinalize(OutputStream)
                End If
            End Sub

            Private Overloads Sub WriteOutput(ByVal ContentType As String, ByRef OutputStream As IO.Stream, ByVal SendAsCompressed As Boolean)
                Dim ContentBuffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 4096), Byte()), bC As Integer

                If SendAsCompressed Then
                    Dim ExceptionText As String = Nothing

                    Dim gzippedStream As New IO.MemoryStream()
                    Dim gzipCompression As System.IO.Compression.GZipStream = Nothing

                    Try
                        gzipCompression = New System.IO.Compression.GZipStream(gzippedStream, IO.Compression.CompressionMode.Compress, True)

                        Do
                            bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                            gzipCompression.Write(ContentBuffer, 0, bC)
                        Loop While bC > 0
                    Catch ex As Exception
                        ExceptionText = ex.ToString()
                    Finally
                        If Not gzipCompression Is Nothing Then gzipCompression.Close() : GC.SuppressFinalize(gzipCompression)
                    End Try

                    If Not ExceptionText Is Nothing Then
                        If Not gzippedStream Is Nothing Then gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)

                        Throw New Exception(ExceptionText)
                    End If

                    If gzippedStream.Length < OutputStream.Length Then
                        SolidDevelopment.Web.General.Context.Response.AppendHeader("Content-Type", ContentType)
                        SolidDevelopment.Web.General.Context.Response.AppendHeader("Content-Encoding", "gzip")

                        Me.WriteOutput(ContentType, CType(gzippedStream, IO.Stream), False)

                        gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)
                    Else
                        Me.WriteOutput(ContentType, OutputStream, False)
                    End If
                Else
                    SolidDevelopment.Web.General.Context.Response.AppendHeader("Content-Type", ContentType)

                    If Me.Bandwidth > 0 Then ContentBuffer = CType(Array.CreateInstance(GetType(Byte), Me.Bandwidth), Byte())

                    OutputStream.Seek(0, IO.SeekOrigin.Begin)

                    Do
                        bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                        If bC > 0 Then
                            SolidDevelopment.Web.General.Context.Response.OutputStream.Write(ContentBuffer, 0, bC)

                            If Me.Bandwidth > 0 AndAlso bC = Me.Bandwidth Then System.Threading.Thread.Sleep(1000)
                        End If
                    Loop Until bC = 0
                End If
            End Sub

            Private Shared _Bandwidth As Long = -1
            Private ReadOnly Property Bandwidth() As Long
                Get
                    If XeoraHandler._Bandwidth = -1 Then
                        Dim _Bandwidth As String = System.Configuration.ConfigurationManager.AppSettings.Item("Bandwidth")

                        If Not String.IsNullOrEmpty(_Bandwidth) Then
                            Try
                                XeoraHandler._Bandwidth = Long.Parse(_Bandwidth)
                            Catch ex As Exception
                                XeoraHandler._Bandwidth = 0
                            End Try
                        End If
                    End If

                    Return XeoraHandler._Bandwidth
                End Get
            End Property
        End Class
    End Class
End Namespace