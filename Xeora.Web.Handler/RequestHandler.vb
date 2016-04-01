Option Strict On

Namespace Xeora.Web.Handler
    Public Class RequestHandler
        Implements System.Web.IHttpAsyncHandler
        Implements System.Web.SessionState.IRequiresSessionState

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Function BeginProcessRequest(ByVal context As System.Web.HttpContext, ByVal CallBack As AsyncCallback, ByVal State As Object) As IAsyncResult Implements System.Web.IHttpAsyncHandler.BeginProcessRequest
            ' Prepare Context
            Dim RequestID As String =
                CType(context.Items.Item("RequestID"), String)

            Dim XeoraHandler As DoXeoraDelegate =
                New DoXeoraDelegate(AddressOf Me.DoXeora)

            Return XeoraHandler.BeginInvoke(RequestID, New AsyncCallback(Sub(aR As IAsyncResult)
                                                                             Try
                                                                                 XeoraHandler.EndInvoke(aR)
                                                                             Catch ex As System.Exception
                                                                                 ' Just Handle Exceptions
                                                                                 ' I catch this error when i was testing the videostream of Mayadroom
                                                                                 ' w3wp.exe create an exception which is "Remote Host Close the Connection"
                                                                                 ' that was causing to stop the Website...
                                                                             End Try

                                                                             CallBack.Invoke(aR)
                                                                         End Sub), State)
        End Function

        Private Delegate Sub DoXeoraDelegate(ByVal RequestID As String)
        Private Sub DoXeora(ByVal RequestID As String)
            Dim XeoraHandler As New XeoraHandler(RequestID)
            XeoraHandler.Start()
        End Sub

        Public Sub EndProcessRequest(ByVal result As IAsyncResult) Implements System.Web.IHttpAsyncHandler.EndProcessRequest
            ' Do Nothing
        End Sub

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            Throw New InvalidOperationException()
        End Sub

        Private Class XeoraHandler
            Private _RequestID As String

            Private _BeginRequestTime As Date
            Private _SupportCompression As Boolean
            Private _DomainControl As Site.DomainControl

            Private _MessageResult As [Shared].ControlResult.Message

            Public Sub New(ByVal RequestID As String)
                Me._RequestID = RequestID
                Me._MessageResult = Nothing
            End Sub

            Public Sub Start()
                Dim Context As System.Web.HttpContext =
                    RequestModule.Context(Me._RequestID)

                If Context Is Nothing Then Exit Sub

                ' Prepare RequestDate and Compression Support
                Me._BeginRequestTime = Date.Now
                Me._SupportCompression = False

                ' Make Available Context for Current Async Call
                [Shared].Helpers.AssignRequestID(
                    CType(Context.Items.Item("RequestID"), String))
                ' !--

                ' DO NOT USE General.Context for this line
                Context.Items.Add("_sys_TemplateRequest", False)

                Dim IsEO As Boolean = False

                Try
                    ' DomainIDAccessTree should be always the DefaultDomain according to the request it will change in the following code lines
                    Dim DomainIDAccessTree As String() = New String() {[Shared].Configurations.DefaultDomain}
                    ' LanguageID should be the language that visiter uses, It can be null which is not important because it will be created using
                    ' default domain default language id
                    Dim LanguageID As String = [Shared].Helpers.CurrentDomainLanguageID
                    ' Create with default ones
                    Me._DomainControl = New Site.DomainControl(DomainIDAccessTree, LanguageID)

                    ' Reset the languageID because it may changed on false language id
                    LanguageID = Site.DomainControl.Domain.Language.ID

                    ' Resolve If Request is Mapped (and if urlmapping is active)
                    If Me._DomainControl.URLMapping.IsActive Then
                        Dim ResolvedMapped As [Shared].URLMapping.ResolvedMapped =
                            Me._DomainControl.URLMapping.ResolveMappedURL([Shared].Helpers.Context.Request.RawUrl)

                        If Not ResolvedMapped Is Nothing AndAlso
                            ResolvedMapped.IsResolved Then
                            ' This is a mapped request

                            Dim RequestURL As String =
                                String.Format("{0}{1}",
                                    [Shared].Configurations.ApplicationRoot.BrowserImplementation,
                                    ResolvedMapped.TemplateID
                                )
                            If ResolvedMapped.URLQueryDictionary.Count > 0 Then _
                                RequestURL = String.Concat(RequestURL, "?", ResolvedMapped.URLQueryDictionary.ToString())

                            ' Let the server understand what this URL is about...
                            [Shared].Helpers.Context.RewritePath(RequestURL)
                        End If
                    End If
                    ' !---

                    If [Shared].Helpers.Context.Response.ContentEncoding Is Nothing Then _
                        [Shared].Helpers.Context.Response.ContentEncoding = Text.Encoding.UTF8

                    ' Let define server and user side page caching
                    If Not [Shared].Helpers.Context.Request.QueryString.Item("nocache") Is Nothing Then
                        [Shared].Helpers.Context.Response.CacheControl = "no-cache"
                        [Shared].Helpers.Context.Response.AddHeader("Pragma", "no-cache")
                        [Shared].Helpers.Context.Response.Expires = -1
                        [Shared].Helpers.Context.Response.ExpiresAbsolute = Date.Today.AddMilliseconds(-1)

                        Select Case [Shared].Helpers.Context.Request.QueryString.Item("nocache")
                            Case "L2", "L2XC"
                                System.Web.HttpResponse.RemoveOutputCacheItem([Shared].Helpers.Context.Request.FilePath)
                        End Select

                        ' Session Cookie Option
                        Dim CookilessSessionString As String =
                            String.Format("{0}_Cookieless", [Shared].Configurations.VirtualRoot.Replace("/"c, "_"c))
                        Select Case [Shared].Helpers.Context.Request.QueryString.Item("nocache")
                            Case "L0XC", "L1XC", "L2XC"
                                [Shared].Helpers.Context.Session.Contents.Item(CookilessSessionString) = True
                            Case Else
                                [Shared].Helpers.Context.Session.Contents.Item(CookilessSessionString) = False
                        End Select
                        ' !--
                    Else
                        ' 1 year cache! my god!
                        [Shared].Helpers.Context.Response.ExpiresAbsolute = Date.Today.AddYears(1)
                    End If

                    ' Let's check if the browser accept compressed output
                    Dim AcceptEncodings As String =
                        [Shared].Helpers.Context.Request.ServerVariables.Item("HTTP_ACCEPT_ENCODING")
                    If Not AcceptEncodings Is Nothing Then _
                        Me._SupportCompression = (AcceptEncodings.IndexOf("gzip") > -1)

                    ' This is a WebService or Template request
                    Dim UseDefaultTemplate As Boolean
                    Dim CapturedTemplateID As String =
                        [Shared].Helpers.ResolveTemplateFromURL([Shared].Helpers.Context.Request.RawUrl, UseDefaultTemplate)

                    If Not String.IsNullOrEmpty(CapturedTemplateID) OrElse UseDefaultTemplate Then _
                        Me._DomainControl.ServiceID = CapturedTemplateID

                    If String.IsNullOrEmpty(Me._DomainControl.ServiceID) Then
                        ' Requested File is not a WebService Or Template

                        Dim DomainContentsPath As String =
                            [Shared].Helpers.GetDomainContentsPath(DomainIDAccessTree, LanguageID)
                        Dim RequestedFileVirtualPath As String =
                            [Shared].Helpers.Context.Request.Path

                        If RequestedFileVirtualPath.IndexOf(DomainContentsPath) = -1 Then
                            ' This is also not a request for default DomainContents

                            ' Extract the ChildDomainIDAccessTree and LanguageID using RequestPath
                            Dim RequestedDomainWebPath As String = RequestedFileVirtualPath
                            Dim BrowserImplementation As String =
                                [Shared].Configurations.ApplicationRoot.BrowserImplementation
                            RequestedDomainWebPath = RequestedDomainWebPath.Remove(RequestedDomainWebPath.IndexOf(BrowserImplementation), BrowserImplementation.Length)

                            If RequestedDomainWebPath.IndexOf([Shared].Helpers.HashCode) = 0 Then _
                                RequestedDomainWebPath = RequestedDomainWebPath.Substring([Shared].Helpers.HashCode.Length + 1)

                            If RequestedDomainWebPath.IndexOf("/"c) > -1 Then
                                RequestedDomainWebPath = RequestedDomainWebPath.Split("/"c)(0)

                                If Not String.IsNullOrEmpty(RequestedDomainWebPath) Then
                                    Dim SplittedRequestedDomainWebPath As String() =
                                        RequestedDomainWebPath.Split("_"c)

                                    If SplittedRequestedDomainWebPath.Length = 2 Then
                                        Dim ChildDomainIDAccessTree As String(), ChildDomainLanguageID As String = String.Empty

                                        ChildDomainIDAccessTree = SplittedRequestedDomainWebPath(0).Split("-"c)
                                        ChildDomainLanguageID = SplittedRequestedDomainWebPath(1)

                                        Me._DomainControl = New Site.DomainControl(ChildDomainIDAccessTree, ChildDomainLanguageID)

                                        DomainContentsPath = [Shared].Helpers.GetDomainContentsPath(ChildDomainIDAccessTree, ChildDomainLanguageID)
                                    End If
                                End If
                            End If
                        End If

                        ' Provide Requested File Stream
                        If RequestedFileVirtualPath.IndexOf(DomainContentsPath) > -1 Then
                            ' This is a well known Domain Content file
                            ' Clean Domain Contents pointer
                            RequestedFileVirtualPath = RequestedFileVirtualPath.Remove(0, RequestedFileVirtualPath.IndexOf(DomainContentsPath) + DomainContentsPath.Length + 1)

                            Me.PostDomainContentFileToClient(RequestedFileVirtualPath)
                        Else
                            Dim ScriptFileName As String =
                                String.Format("_bi_sps_v{0}.js", Me._DomainControl.XeoraJSVersion)
                            Dim ScriptFileNameIndex As Integer =
                                RequestedFileVirtualPath.IndexOf(ScriptFileName)
                            Dim IsScriptRequesting As Boolean =
                                ScriptFileNameIndex > -1 AndAlso (RequestedFileVirtualPath.Length - ScriptFileName.Length) = ScriptFileNameIndex

                            If IsScriptRequesting Then
                                Me.PostBuildInJavaScriptToClient()
                            Else
                                Me.PostRequestedStaticFileToClient()
                            End If
                        End If
                    Else
                        ' Mark this request is for TemplateRequest, DO NOT USE General.Context for this line
                        Context.Items.Item("_sys_TemplateRequest") = True

                        ' Set Title Globals
                        [Shared].Helpers.SiteTitle = Site.DomainControl.Domain.Language.Get("SITETITLE")

                        If String.Compare([Shared].Helpers.Context.Request.HttpMethod, "GET", True) = 0 Then
                            ' Check if hashcode is assign to the requested template
                            Dim ParentURL As String =
                                [Shared].Helpers.Context.Request.FilePath

                            ParentURL = ParentURL.Remove(0, ParentURL.IndexOf([Shared].Configurations.ApplicationRoot.BrowserImplementation) + [Shared].Configurations.ApplicationRoot.BrowserImplementation.Length)

                            Dim mR_Parent As Text.RegularExpressions.Match =
                                Text.RegularExpressions.Regex.Match(ParentURL, String.Format("\d+/{0}", Me._DomainControl.ServiceID))

                            ' Not assigned, so assign!
                            If Not mR_Parent.Success Then
                                Dim RewrittenPath As String =
                                    String.Format("{0}{1}/{2}", [Shared].Configurations.ApplicationRoot.BrowserImplementation, [Shared].Helpers.HashCode, Me._DomainControl.ServiceID)
                                Dim QueryString As String =
                                    [Shared].Helpers.Context.Request.ServerVariables.Item("QUERY_STRING")

                                If Not String.IsNullOrEmpty(QueryString) Then _
                                    RewrittenPath = String.Format("{0}?{1}", RewrittenPath, QueryString)

                                [Shared].Helpers.Context.RewritePath(RewrittenPath)
                            End If
                        End If

                        If Me._DomainControl.IsAuthenticationRequired Then
                            Me.RedirectToAuthenticationPage(Me._DomainControl.ServiceID)
                        Else
                            Dim MethodResultContent As String = Nothing

                            ' If it is a template call with some postbackdata so you shoud handle the postback first before handling page.
                            If Me._DomainControl.ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template AndAlso
                                String.Compare([Shared].Helpers.Context.Request.HttpMethod, "POST", True) = 0 AndAlso
                                Not String.IsNullOrEmpty([Shared].Helpers.Context.Request.Form.Item("PostBackInformation")) Then

                                ' Decode Encoded Call Function to Readable
                                Dim BindInfo As [Shared].Execution.BindInfo =
                                    [Shared].Execution.BindInfo.Make(
                                        Manager.Assembly.DecodeFunction(
                                            [Shared].Helpers.Context.Request.Form.Item("PostBackInformation"))
                                    )
                                Dim ParameterValues As Object() = Nothing

                                ' Parse Required Values Of BindInfo ProcedureParams
                                If Not BindInfo Is Nothing Then _
                                    ParameterValues = Controller.PropertyController.ParseProperties(Nothing, Nothing, BindInfo.ProcedureParams, New Controller.Directive.IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As [Shared].IDomain)
                                                                                                                                                                                                                             Instance = Site.DomainControl.Domain
                                                                                                                                                                                                                         End Sub))

                                Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                                    Manager.Assembly.InvokeBind(
                                        BindInfo,
                                        ParameterValues,
                                        Manager.Assembly.ExecuterTypes.Undefined
                                    )

                                If BindInvokeResult.ReloadRequired Then
                                    ' This is application dependency problem, force to apply auto recovery!
                                    Me._DomainControl.ClearDomainCache()
                                    RequestModule.ReloadApplication([Shared].Helpers.CurrentRequestID)

                                    Exit Sub
                                Else
                                    If Not BindInvokeResult.InvokeResult Is Nothing AndAlso
                                        TypeOf BindInvokeResult.InvokeResult Is System.Exception Then

                                        Me._MessageResult = New [Shared].ControlResult.Message(CType(BindInvokeResult.InvokeResult, System.Exception).ToString())

                                        Me.WritePage(String.Empty)
                                    ElseIf Not BindInvokeResult.InvokeResult Is Nothing AndAlso
                                        TypeOf BindInvokeResult.InvokeResult Is [Shared].ControlResult.Message Then

                                        Me._MessageResult = CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.Message)

                                        Me.WritePage(String.Empty)
                                    ElseIf Not BindInvokeResult.InvokeResult Is Nothing AndAlso
                                        TypeOf BindInvokeResult.InvokeResult Is [Shared].ControlResult.RedirectOrder Then

                                        [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                                        [Shared].Helpers.Context.Items.Add(
                                            "RedirectLocation",
                                            CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.RedirectOrder).Location
                                        )
                                    Else
                                        MethodResultContent = [Shared].Execution.GetPrimitiveValue(BindInvokeResult.InvokeResult)

                                        Select Case Me._DomainControl.ServiceType
                                            Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                                                Me.WritePage(MethodResultContent)

                                            Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.WebService
                                                Me.WritePage()

                                        End Select
                                    End If
                                End If
                            Else
                                Select Case Me._DomainControl.ServiceType
                                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                                        Me.WritePage(MethodResultContent)

                                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.WebService
                                        Me.WritePage()

                                End Select
                            End If
                        End If
                    End If
                    ' !--
QUICKFINISH:
                Catch ex As System.Exception
                    If TypeOf ex Is System.Web.HttpException AndAlso Not [Shared].Configurations.LogHTTPExceptions Then Exit Try

                    Dim CurrentContextCatch As System.Web.HttpContext =
                        [Shared].Helpers.Context

                    If CurrentContextCatch Is Nothing Then CurrentContextCatch = Context

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
                        Catch exSession As System.Exception
                            ' The collection was modified after the enumerator was created.

                            LogResult.AppendLine(String.Format(" Exception Occured -> {0}", exSession.Message))
                        End Try
                    End If
                    ' !--
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Request POST Variables --")
                    For Each KeyItem As String In CurrentContextCatch.Request.Form
                        LogResult.AppendLine(
                            String.Format(" {0} -> {1}", KeyItem, CurrentContextCatch.Request.Form.Item(KeyItem))
                        )
                    Next
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Request URL & Query String --")
                    LogResult.AppendLine(
                        String.Format("{0}?{1}",
                            CurrentContextCatch.Request.ServerVariables.Item("URL"),
                            CurrentContextCatch.Request.ServerVariables.Item("QUERY_STRING")
                        )
                    )
                    LogResult.AppendLine("")
                    LogResult.AppendLine("-- Error Content --")
                    LogResult.Append(ex.ToString())

                    Try
                        Helper.EventLogging.WriteToLog(LogResult.ToString())
                    Catch exLogging As System.Exception
                        CompiledExceptions.AppendLine("-- TXT LOGGING EXCEPTION --")
                        CompiledExceptions.Append(exLogging.ToString())
                        CompiledExceptions.AppendLine()

                        Try
                            If Not EventLog.SourceExists("XeoraCube") Then EventLog.CreateEventSource("XeoraCube", "XeoraCube")

                            EventLog.WriteEntry("XeoraCube", exLogging.ToString(), EventLogEntryType.Error)
                        Catch exLogging02 As System.Exception
                            CompiledExceptions.AppendLine("-- SYSTEM EVENT LOGGING EXCEPTION --")
                            CompiledExceptions.Append(exLogging02.ToString())
                            CompiledExceptions.AppendLine()
                        End Try
                    End Try

                    If [Shared].Configurations.Debugging Then
                        CurrentContextCatch.Response.Clear()
                        CurrentContextCatch.Response.Write("<h2 align=""center"" style=""color:#CC0000"">" & [Global].SystemMessages.SYSTEM_ERROROCCURED & "!</h2>")
                        CurrentContextCatch.Response.Write("<hr size=""1px"">")
                        CurrentContextCatch.Response.Write("<pre>" & CompiledExceptions.ToString() & "</pre>")
                        CurrentContextCatch.Response.End()

                        IsEO = True
                    Else
                        If Not Me._DomainControl Is Nothing Then
                            [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                            [Shared].Helpers.Context.Items.Add(
                                "RedirectLocation",
                                String.Format("http://{0}{1}",
                                    CurrentContextCatch.Request.ServerVariables("HTTP_HOST"),
                                    [Shared].Helpers.GetRedirectURL(
                                        False,
                                        Site.DomainControl.Domain.Settings.Configurations.DefaultPage,
                                        Site.DomainControl.Domain.Settings.Configurations.DefaultCaching
                                    )
                                )
                            )
                        Else
                            CurrentContextCatch.Response.Clear()
                            CurrentContextCatch.Response.Write("<h2 align=""center"" style=""color:#CC0000"">" & [Global].SystemMessages.SYSTEM_ERROROCCURED & "!</h2>")
                            CurrentContextCatch.Response.Write("<h4 align=""center"">" & ex.Message & "</h4>")
                            CurrentContextCatch.Response.End()

                            IsEO = True
                        End If
                    End If
                Finally
                    If Not Me._DomainControl Is Nothing Then Me._DomainControl.Dispose()

                    If [Shared].Helpers.Context.Items.Contains("RedirectLocation") Then

                        If CType([Shared].Helpers.Context.Items.Item("RedirectLocation"), String).IndexOf("://") = -1 Then
                            Dim RedirectLocation As String =
                                String.Format("http://{0}{1}",
                                        [Shared].Helpers.Context.Request.ServerVariables("HTTP_HOST"),
                                        [Shared].Helpers.Context.Items.Item("RedirectLocation")
                                    )

                            [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                            [Shared].Helpers.Context.Items.Add(
                                "RedirectLocation",
                                RedirectLocation
                            )
                        End If

                        If [Shared].Helpers.Context.Request.Headers.Item("X-BlockRenderingID") Is Nothing Then
                            Try
                                [Shared].Helpers.Context.Response.Redirect(CType([Shared].Helpers.Context.Items.Item("RedirectLocation"), String), True)
                            Catch ex As System.Web.HttpException
                                ' Just Handle Exceptions (Remote host closed the connection )
                            Catch ex As System.Exception
                                Throw
                            End Try
                        Else
                            [Shared].Helpers.Context.Response.Clear()
                            [Shared].Helpers.Context.Response.Buffer = True
                            [Shared].Helpers.Context.Response.Write(
                                String.Format("rl:{0}", CType([Shared].Helpers.Context.Items.Item("RedirectLocation"), String))
                            )
                            [Shared].Helpers.Context.Response.End()
                        End If
                    End If
                End Try
            End Sub

            Private Sub PostRequestedStaticFileToClient()
                ' This is a common file located somewhere in the webserver
                Dim RequestFilePath As String =
                    [Shared].Helpers.Context.Request.PhysicalPath

                If Not IO.File.Exists(RequestFilePath) Then
                    [Shared].Helpers.Context.Response.StatusCode = 404
                Else
                    Dim RequestFileStream As IO.Stream =
                        New IO.FileStream(RequestFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                    Me.WriteOutput(
                        [Shared].Helpers.GetMimeType(
                            IO.Path.GetExtension(RequestFilePath)
                        ),
                        RequestFileStream,
                        Me._SupportCompression
                    )

                    RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                    ' Remove RedirectLocation if it exists
                    [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                End If
            End Sub

            Private Sub PostDomainContentFileToClient(ByVal RequestedFilePathInDomainContents As String)
                Dim RequestFileStream As IO.Stream = Nothing

                ' Write File Content To Theme File Stream
                Site.DomainControl.ProvideFileStream(RequestFileStream, RequestedFilePathInDomainContents)

                If RequestFileStream Is Nothing Then
                    [Shared].Helpers.Context.Response.StatusCode = 404
                Else
                    Me.WriteOutput(
                        [Shared].Helpers.GetMimeType(
                            IO.Path.GetExtension(RequestedFilePathInDomainContents)
                        ),
                        RequestFileStream,
                        Me._SupportCompression
                    )

                    RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                    [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                End If
            End Sub

            Private Sub PostBuildInJavaScriptToClient()
                ' This is a Script file request
                Dim RequestFileStream As IO.Stream = Nothing

                ' Write File Content To BuiltIn Script Stream
                Me._DomainControl.ProvideXeoraJSStream(RequestFileStream)

                Me.WriteOutput(
                    [Shared].Helpers.GetMimeType(".js"),
                    RequestFileStream,
                    Me._SupportCompression
                )

                RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)

                ' Remove RedirectLocation if it exists
                [Shared].Helpers.Context.Items.Remove("RedirectLocation")
            End Sub

            Private Sub RedirectToAuthenticationPage(Optional ByVal CurrentRequestedTemplate As String = Nothing)
                Select Case Me._DomainControl.ServiceType
                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                        ' Get AuthenticationPage 
                        Dim AuthenticationPage As String =
                            Site.DomainControl.Domain.Settings.Configurations.AuthenticationPage

                        If Not String.IsNullOrEmpty(CurrentRequestedTemplate) AndAlso
                            String.Compare(AuthenticationPage, CurrentRequestedTemplate, True) <> 0 Then

                            [Shared].Helpers.Context.Session.Contents.Item("_sys_Referrer") =
                                [Shared].Helpers.Context.Request.RawUrl
                        End If

                        ' Remove Redirect Location IF Exists
                        [Shared].Helpers.Context.Items.Remove("RedirectLocation")
                        ' Reset Redirect Location to AuthenticationPage
                        [Shared].Helpers.Context.Items.Add(
                            "RedirectLocation",
                            [Shared].Helpers.GetRedirectURL(
                                True,
                                AuthenticationPage,
                                [Shared].Globals.PageCachingTypes.TextsOnly
                            )
                        )

                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.WebService
                        Me.WritePage()

                End Select
            End Sub

            Private Overloads Sub WritePage()
                Try
                    Me._DomainControl.RenderService(Me._MessageResult, String.Empty)
                Catch ex As Exception.ReloadRequiredException
                    Me._DomainControl.ClearDomainCache()
                    RequestModule.ReloadApplication([Shared].Helpers.CurrentRequestID)

                    Exit Sub
                End Try

                Dim sW As New IO.StringWriter

                sW.Write("<?xml version=""1.0"" encoding=""utf-8"" ?>")
                sW.Write(Me._DomainControl.ServiceResult)

                sW.Flush()
                sW.Close()

                Me.WriteOutput(Me._DomainControl.ServiceMimeType, sW.ToString(), Me._SupportCompression)
            End Sub

            Private Overloads Sub WritePage(ByVal MethodResultContent As String)
                Dim UpdateBlockControlID As String =
                    [Shared].Helpers.Context.Request.Headers.Item("X-BlockRenderingID")

                Try
                    Me._DomainControl.RenderService(Me._MessageResult, UpdateBlockControlID)
                Catch ex As Exception.ReloadRequiredException
                    Me._DomainControl.ClearDomainCache()
                    RequestModule.ReloadApplication([Shared].Helpers.CurrentRequestID)

                    Exit Sub
                End Try

                Dim CurrentContextCatch As System.Web.HttpContext = [Shared].Helpers.Context

                Dim sW As New IO.StringWriter

                If Not Me._DomainControl.IsWorkingAsStandAlone AndAlso
                    String.IsNullOrEmpty(UpdateBlockControlID) Then

                    If [Shared].Configurations.UseHTML5Header Then
                        sW.WriteLine("<!doctype html>")
                    Else
                        sW.WriteLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">")
                    End If

                    sW.WriteLine("<html>")
                    sW.WriteLine("<head>")

                    Dim IsContentTypeAdded As Boolean = False, IsPragmaAdded As Boolean = False, IsCacheControlAdded As Boolean = False, IsExpiresAdded As Boolean = False

                    For Each kVP As KeyValuePair(Of [Shared].MetaRecord.MetaTags, String) In [Shared].MetaRecord.RegisteredMetaRecords
                        Select Case [Shared].MetaRecord.QueryMetaTagSpace(kVP.Key)
                            Case [Shared].MetaRecord.MetaTagSpace.name
                                sW.WriteLine(
                                        String.Format(
                                            "<meta name=""{0}"" content=""{1}"" />",
                                            [Shared].MetaRecord.GetMetaTagHtmlName(kVP.Key),
                                            kVP.Value
                                        )
                                    )
                            Case [Shared].MetaRecord.MetaTagSpace.httpequiv
                                sW.WriteLine(
                                    String.Format(
                                        "<meta http-equiv=""{0}"" content=""{1}"" />",
                                        [Shared].MetaRecord.GetMetaTagHtmlName(kVP.Key),
                                        kVP.Value
                                    )
                                )
                        End Select

                        Select Case kVP.Key
                            Case [Shared].MetaRecord.MetaTags.contenttype
                                IsContentTypeAdded = True
                            Case [Shared].MetaRecord.MetaTags.pragma
                                IsPragmaAdded = True
                            Case [Shared].MetaRecord.MetaTags.cachecontrol
                                IsCacheControlAdded = True
                            Case [Shared].MetaRecord.MetaTags.expires
                                IsExpiresAdded = True
                        End Select
                    Next

                    Dim KeyName As String = String.Empty

                    For Each kVP As KeyValuePair(Of String, String) In [Shared].MetaRecord.RegisteredCustomMetaRecords
                        KeyName = kVP.Key

                        Select Case [Shared].MetaRecord.QueryMetaTagSpace(KeyName)
                            Case [Shared].MetaRecord.MetaTagSpace.name
                                sW.WriteLine(
                                        String.Format(
                                            "<meta name=""{0}"" content=""{1}"" />",
                                            KeyName,
                                            kVP.Value
                                        )
                                    )
                            Case [Shared].MetaRecord.MetaTagSpace.httpequiv
                                sW.WriteLine(
                                    String.Format(
                                        "<meta http-equiv=""{0}"" content=""{1}"" />",
                                        KeyName,
                                        kVP.Value
                                    )
                                )
                        End Select
                    Next

                    If Not IsContentTypeAdded Then
                        sW.WriteLine(
                            String.Format(
                                "<meta http-equiv=""Content-Type"" content=""{0}; charset={1}"" />",
                                Me._DomainControl.ServiceMimeType,
                                CurrentContextCatch.Response.ContentEncoding.WebName
                            )
                        )
                    End If

                    If Not CurrentContextCatch.Request.QueryString.Item("nocache") Is Nothing AndAlso
                        (
                            String.Compare(CurrentContextCatch.Request.QueryString.Item("nocache"), "L2", True) = 0 OrElse
                            String.Compare(CurrentContextCatch.Request.QueryString.Item("nocache"), "L2XC", True) = 0
                        ) Then

                        If Not IsPragmaAdded Then sW.WriteLine("<meta http-equiv=""Pragma"" content=""no-cache"" />")
                        If Not IsCacheControlAdded Then sW.WriteLine("<meta http-equiv=""Cache-Control"" content=""no-cache"" />")
                        If Not IsExpiresAdded Then sW.WriteLine("<meta http-equiv=""Expires"" content=""0"" />")
                    End If

                    sW.WriteLine(
                        String.Format(
                            "<title>{0}</title>",
                            [Shared].Helpers.SiteTitle
                        )
                    )

                    If Not String.IsNullOrEmpty([Shared].Helpers.SiteIconURL) Then
                        sW.WriteLine(
                            String.Format(
                                "<link href=""{0}"" rel=""shortcut icon"">",
                                [Shared].Helpers.SiteIconURL
                            )
                        )
                    End If

                    sW.WriteLine(
                        String.Format(
                            "<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                            IO.Path.Combine(
                                [Shared].Helpers.GetDomainContentsPath(
                                    Site.DomainControl.Domain.IDAccessTree,
                                    Site.DomainControl.Domain.Language.ID
                                ), "styles.css").Replace("\"c, "/"c)
                        )
                    )

                    sW.WriteLine(
                        String.Format(
                            "<script language=""javascript"" src=""{0}_bi_sps_v{1}.js"" type=""text/javascript""></script>",
                            [Shared].Configurations.ApplicationRoot.BrowserImplementation,
                            Me._DomainControl.XeoraJSVersion
                        )
                    )

                    sW.WriteLine("</head>")
                    sW.WriteLine("<body>")
                    sW.WriteLine(
                        String.Format(
                            "<form method=""post"" action=""{0}?{1}"" enctype=""multipart/form-data"" style=""margin: 0px; padding: 0px;"">",
                            CurrentContextCatch.Request.Path,
                            CurrentContextCatch.Request.ServerVariables.Item("QUERY_STRING")
                        )
                    )
                    sW.WriteLine("<input type=""hidden"" name=""PostBackInformation"" id=""PostBackInformation"" />")
                End If

                sW.Write("{0}")

                If Not Me._DomainControl.IsWorkingAsStandAlone AndAlso
                    String.IsNullOrEmpty(UpdateBlockControlID) Then sW.WriteLine("</form>")

                sW.Write(MethodResultContent)

                If Not Me._DomainControl.IsWorkingAsStandAlone AndAlso
                    String.IsNullOrEmpty(UpdateBlockControlID) Then

                    sW.WriteLine("</body>")
                    sW.WriteLine("</html>")
                End If

                sW.Flush()
                sW.Close()

                Dim ServiceResult As String = String.Empty

                ServiceResult = sW.ToString()
                ServiceResult = String.Format(ServiceResult, Me._DomainControl.ServiceResult)

                Dim sys_RenderDurationMark As String = "<!--_sys_PAGERENDERDURATION-->"
                Dim idxRenderDurationMark As Integer =
                    ServiceResult.IndexOf(sys_RenderDurationMark)

                If idxRenderDurationMark > -1 Then
                    Dim EndRequestTimeSpan As TimeSpan = Date.Now.Subtract(Me._BeginRequestTime)

                    ServiceResult = ServiceResult.Remove(idxRenderDurationMark, sys_RenderDurationMark.Length)
                    ServiceResult = ServiceResult.Insert(idxRenderDurationMark, EndRequestTimeSpan.TotalMilliseconds.ToString())
                End If

                Me.WriteOutput(Me._DomainControl.ServiceMimeType, ServiceResult, Me._SupportCompression)
            End Sub

            Private Overloads Sub WriteOutput(ByVal ContentType As String, ByVal OutputContent As String, ByVal SendAsCompressed As Boolean)
                ' This header for examing Response.End and also sent to the browser about the version of Solid Web Content
                Dim ResponseEndRaised As Boolean = False
                Try
                    Dim vI As Version = Reflection.Assembly.GetExecutingAssembly().GetName().Version

                    [Shared].Helpers.Context.Response.AppendHeader("X-FrameworkVersion", String.Format("{0}.{1}.{2}", vI.Major, vI.Minor, vI.Build))
                Catch ex As System.Web.HttpException
                    ResponseEndRaised = True
                Catch ex As System.Exception
                    Throw
                End Try
                ' !--

                If Not ResponseEndRaised AndAlso
                    Not [Shared].Helpers.Context.Items.Contains("RedirectLocation") Then

                    Dim OutputStream As IO.Stream =
                        New IO.MemoryStream(
                            Text.Encoding.UTF8.GetBytes(OutputContent)
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
                    Dim gzipCompression As IO.Compression.GZipStream = Nothing

                    Try
                        gzipCompression = New IO.Compression.GZipStream(gzippedStream, IO.Compression.CompressionMode.Compress, True)

                        Do
                            bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                            gzipCompression.Write(ContentBuffer, 0, bC)
                        Loop While bC > 0
                    Catch ex As System.Exception
                        ExceptionText = ex.ToString()
                    Finally
                        If Not gzipCompression Is Nothing Then gzipCompression.Close() : GC.SuppressFinalize(gzipCompression)
                    End Try

                    If Not ExceptionText Is Nothing Then
                        If Not gzippedStream Is Nothing Then gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)

                        Throw New System.Exception(ExceptionText)
                    End If

                    If gzippedStream.Length < OutputStream.Length Then
                        [Shared].Helpers.Context.Response.AppendHeader("Content-Type", ContentType)
                        [Shared].Helpers.Context.Response.AppendHeader("Content-Encoding", "gzip")

                        Me.WriteOutput(ContentType, CType(gzippedStream, IO.Stream), False)

                        gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)
                    Else
                        Me.WriteOutput(ContentType, OutputStream, False)
                    End If
                Else
                    [Shared].Helpers.Context.Response.AppendHeader("Content-Type", ContentType)

                    If Me.Bandwidth > 0 Then ContentBuffer = CType(Array.CreateInstance(GetType(Byte), Me.Bandwidth), Byte())

                    OutputStream.Seek(0, IO.SeekOrigin.Begin)

                    Do
                        bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                        If bC > 0 Then
                            [Shared].Helpers.Context.Response.OutputStream.Write(ContentBuffer, 0, bC)

                            If Me.Bandwidth > 0 AndAlso bC = Me.Bandwidth Then Threading.Thread.Sleep(1000)
                        End If
                    Loop Until bC = 0
                End If
            End Sub

            Private Shared _Bandwidth As Long = -1
            Private ReadOnly Property Bandwidth() As Long
                Get
                    If XeoraHandler._Bandwidth = -1 Then
                        Dim _Bandwidth As String = Configuration.ConfigurationManager.AppSettings.Item("Bandwidth")

                        If Not String.IsNullOrEmpty(_Bandwidth) Then
                            Try
                                XeoraHandler._Bandwidth = Long.Parse(_Bandwidth)
                            Catch ex As System.Exception
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