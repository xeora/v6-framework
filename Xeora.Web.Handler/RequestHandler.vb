Option Strict On

Namespace Xeora.Web.Handler
    Public Class RequestHandler
        Implements System.Web.IHttpAsyncHandler
        Implements System.Web.SessionState.IRequiresSessionState

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Public Function BeginProcessRequest(ByVal context As System.Web.HttpContext, ByVal CallBack As AsyncCallback, ByVal State As Object) As IAsyncResult Implements System.Web.IHttpAsyncHandler.BeginProcessRequest
            ' Prepare Context
            Dim RequestID As String =
                CType(context.Items.Item("RequestID"), String)

            Dim XeoraHandler As DoXeoraDelegate =
                New DoXeoraDelegate(AddressOf Me.DoXeora)

            Return XeoraHandler.BeginInvoke(RequestID,
                                            New AsyncCallback(
                                                Sub(aR As IAsyncResult)
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
                Dim Context As [Shared].IHttpContext =
                    RequestModule.Context(Me._RequestID)
                If Context Is Nothing Then Exit Sub

                Me._BeginRequestTime = Date.Now
                Me._SupportCompression = False

                ' Make Available Context for Current Async Call
                [Shared].Helpers.AssignRequestID(Me._RequestID)
                ' !--

                Context.Content.Item("_sys_TemplateRequest") = False

                Dim IsEO As Boolean = False
                Try
                    Me._DomainControl = New Site.DomainControl(Me._RequestID, Context.Request.URL, True)

                    ' Caching Settings
                    If [Shared].Globals.PageCaching.DefaultType <> [Shared].Globals.PageCaching.Types.AllContent AndAlso
                        [Shared].Globals.PageCaching.DefaultType <> [Shared].Globals.PageCaching.Types.AllContentCookiless Then

                        Select Case [Shared].Globals.PageCaching.DefaultType
                            Case [Shared].Globals.PageCaching.Types.NoCache,
                                 [Shared].Globals.PageCaching.Types.NoCacheCookiless

                                [Shared].Helpers.Context.Response.Header.Item("Cache-Control") = "no-store, must-revalidate"
                            Case Else
                                [Shared].Helpers.Context.Response.Header.Item("Cache-Control") = "no-cache"
                                [Shared].Helpers.Context.Response.Header.Item("Pragma") = "no-cache"
                        End Select

                        [Shared].Helpers.Context.Response.Header.Item("Expires") = "0"
                    Else
                        ' 1 month cache
                        [Shared].Helpers.Context.Response.Header.Item("Expires") = Date.Now.AddMonths(1).ToString("r")
                    End If
                    ' !---

                    ' Empty Content-Encoding should be replaced
                    If [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") Is Nothing Then _
                        [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = Text.Encoding.UTF8.WebName

                    Dim AcceptEncodings As String =
                        [Shared].Helpers.Context.Request.Server.Item("HTTP_ACCEPT_ENCODING")
                    If Not AcceptEncodings Is Nothing Then _
                        Me._SupportCompression = (AcceptEncodings.IndexOf("gzip") > -1)

                    If Me._DomainControl.ServicePathInfo Is Nothing Then
                        ' Requested File is not a xService Or Template

                        Dim DomainContentsPath As String =
                            [Shared].Helpers.GetDomainContentsPath(Me._DomainControl.Domain.IDAccessTree, Me._DomainControl.Domain.Language.ID)
                        Dim RequestedFileVirtualPath As String =
                            [Shared].Helpers.Context.Request.URL.RelativePath

                        If RequestedFileVirtualPath.IndexOf(DomainContentsPath) = -1 Then
                            ' This is also not a request for default DomainContents

                            ' Extract the ChildDomainIDAccessTree and LanguageID using RequestPath
                            Dim RequestedDomainWebPath As String = RequestedFileVirtualPath
                            Dim BrowserImplementation As String =
                                [Shared].Configurations.ApplicationRoot.BrowserImplementation
                            RequestedDomainWebPath = RequestedDomainWebPath.Remove(RequestedDomainWebPath.IndexOf(BrowserImplementation), BrowserImplementation.Length)

                            If RequestedDomainWebPath.IndexOf([Shared].Helpers.Context.Request.HashCode) = 0 Then _
                                RequestedDomainWebPath = RequestedDomainWebPath.Substring([Shared].Helpers.Context.Request.HashCode.Length + 1)

                            If RequestedDomainWebPath.IndexOf("/"c) > -1 Then
                                RequestedDomainWebPath = RequestedDomainWebPath.Split("/"c)(0)

                                If Not String.IsNullOrEmpty(RequestedDomainWebPath) Then
                                    Dim SplittedRequestedDomainWebPath As String() =
                                        RequestedDomainWebPath.Split("_"c)

                                    If SplittedRequestedDomainWebPath.Length = 2 Then
                                        Dim ChildDomainIDAccessTree As String(), ChildDomainLanguageID As String = String.Empty

                                        ChildDomainIDAccessTree = SplittedRequestedDomainWebPath(0).Split("-"c)
                                        ChildDomainLanguageID = SplittedRequestedDomainWebPath(1)

                                        Me._DomainControl.OverrideDomain(ChildDomainIDAccessTree, ChildDomainLanguageID)

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
                        Context.Content.Item("_sys_TemplateRequest") = True

                        [Shared].Helpers.SiteTitle = Me._DomainControl.Domain.Language.Get("SITETITLE")

                        ' TODO: Simplify
                        If String.Compare([Shared].Helpers.Context.Request.Method, "GET", True) = 0 Then
                            ' Check if hashcode is assign to the requested template
                            Dim ParentURL As String =
                                [Shared].Helpers.Context.Request.URL.RelativePath
                            ParentURL = ParentURL.Remove(0, ParentURL.IndexOf([Shared].Configurations.ApplicationRoot.BrowserImplementation) + [Shared].Configurations.ApplicationRoot.BrowserImplementation.Length)

                            Dim mR_Parent As Text.RegularExpressions.Match =
                                Text.RegularExpressions.Regex.Match(ParentURL, String.Format("\d+/{0}", Me._DomainControl.ServicePathInfo.FullPath))

                            ' Not assigned, so assign!
                            If Not mR_Parent.Success Then
                                Dim RewrittenPath As String =
                                    String.Format("{0}{1}/{2}", [Shared].Configurations.ApplicationRoot.BrowserImplementation, [Shared].Helpers.Context.Request.HashCode, Me._DomainControl.ServicePathInfo.FullPath)
                                Dim QueryString As String =
                                    [Shared].Helpers.Context.Request.Server.Item("QUERY_STRING")

                                If Not String.IsNullOrEmpty(QueryString) Then _
                                    RewrittenPath = String.Format("{0}?{1}", RewrittenPath, QueryString)

                                [Shared].Helpers.Context.Request.RewritePath(RewrittenPath)
                            End If
                        End If

                        If Me._DomainControl.IsAuthenticationRequired Then
                            Me.RedirectToAuthenticationPage(Me._DomainControl.ServicePathInfo.FullPath)
                        Else
                            Dim MethodResultContent As String = Nothing

                            ' If it is a template call with some postbackdata so you shoud handle the postback first before handling page.
                            If Me._DomainControl.ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template AndAlso
                                String.Compare([Shared].Helpers.Context.Request.Method, "POST", True) = 0 AndAlso
                                Not String.IsNullOrEmpty([Shared].Helpers.Context.Request.Form.Item("PostBackInformation")) Then

                                ' Decode Encoded Call Function to Readable
                                Dim BindInfo As [Shared].Execution.BindInfo =
                                    [Shared].Execution.BindInfo.Make(
                                        Manager.Assembly.DecodeFunction(
                                            [Shared].Helpers.Context.Request.Form.Item("PostBackInformation"))
                                    )

                                BindInfo.PrepareProcedureParameters(
                                    New [Shared].Execution.BindInfo.ProcedureParser(
                                        Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                            ProcedureParameter.Value = Controller.PropertyController.ParseProperty(
                                                                           ProcedureParameter.Query,
                                                                           Nothing,
                                                                           Nothing,
                                                                           New Controller.Directive.IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As [Shared].IDomain)
                                                                                                                                                        Instance = Me._DomainControl.Domain
                                                                                                                                                    End Sub)
                                                                        )
                                        End Sub)
                                )

                                Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                                    Manager.Assembly.InvokeBind(BindInfo, Manager.Assembly.ExecuterTypes.Undefined)

                                If BindInvokeResult.ReloadRequired Then
                                    ' This is application dependency problem, force to apply auto recovery!
                                    Me._DomainControl.ClearCache()
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

                                        [Shared].Helpers.Context.Content.Item("RedirectLocation") =
                                            CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.RedirectOrder).Location
                                    Else
                                        MethodResultContent = [Shared].Execution.GetPrimitiveValue(BindInvokeResult.InvokeResult)

                                        Select Case Me._DomainControl.ServiceType
                                            Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                                                Me.WritePage(MethodResultContent)

                                            Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                                                Me.WritePage()

                                        End Select
                                    End If
                                End If
                            Else
                                Select Case Me._DomainControl.ServiceType
                                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                                        Me.WritePage(MethodResultContent)

                                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                                        Me.WritePage()

                                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket
                                        Context.Response.Header.Item("Content-Type") = Me._DomainControl.ServiceMimeType
                                        Context.Response.Header.Item("Content-Encoding") = "identity"

                                        ' Decode Encoded Call Function to Readable
                                        Dim BindInfo As [Shared].Execution.BindInfo =
                                            Me._DomainControl.SocketEndPoint

                                        BindInfo.PrepareProcedureParameters(
                                            New [Shared].Execution.BindInfo.ProcedureParser(
                                                Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                                    ProcedureParameter.Value = Controller.PropertyController.ParseProperty(
                                                                                    ProcedureParameter.Query,
                                                                                    Nothing,
                                                                                    Nothing,
                                                                                    New Controller.Directive.IInstanceRequires.InstanceRequestedEventHandler(Sub(ByRef Instance As [Shared].IDomain)
                                                                                                                                                                 Instance = Me._DomainControl.Domain
                                                                                                                                                             End Sub)
                                                                                )
                                                End Sub)
                                        )

                                        Dim KeyValueList As New List(Of KeyValuePair(Of String, Object))
                                        For Each Item As [Shared].Execution.BindInfo.ProcedureParameter In BindInfo.ProcedureParams
                                            KeyValueList.Add(New KeyValuePair(Of String, Object)(Item.Key, Item.Value))
                                        Next

                                        Dim xSocketObject As [Shared].xSocketObject =
                                            New [Shared].xSocketObject(
                                                Context.Request.Header,
                                                Context.Request.Stream,
                                                Context.Response.Header,
                                                Context.Response.Stream,
                                                KeyValueList.ToArray(),
                                                New [Shared].xSocketObject.FlushHandler(Sub()
                                                                                            Context.Response.Stream.Flush()
                                                                                        End Sub)
                                            )

                                        BindInfo.OverrideProcedureParameters(New String() {"xso"})
                                        BindInfo.PrepareProcedureParameters(
                                            New [Shared].Execution.BindInfo.ProcedureParser(
                                                Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                                    ProcedureParameter.Value = xSocketObject
                                                End Sub)
                                        )

                                        Dim BindInvokeResult As [Shared].Execution.BindInvokeResult =
                                            Manager.Assembly.InvokeBind(BindInfo, Manager.Assembly.ExecuterTypes.Undefined)

                                        If BindInvokeResult.ReloadRequired Then
                                            ' This is application dependency problem, force to apply auto recovery!
                                            Me._DomainControl.ClearCache()
                                            RequestModule.ReloadApplication([Shared].Helpers.CurrentRequestID)

                                            Exit Sub
                                        Else
                                            If Not BindInvokeResult.InvokeResult Is Nothing Then
                                                If TypeOf BindInvokeResult.InvokeResult Is System.Exception Then
                                                    Throw New Exception.ServiceSocketException(CType(BindInvokeResult.InvokeResult, System.Exception).ToString())
                                                ElseIf TypeOf BindInvokeResult.InvokeResult Is [Shared].ControlResult.Message Then
                                                    Dim MessageResult As [Shared].ControlResult.Message =
                                                        CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.Message)

                                                    If MessageResult.Type = [Shared].ControlResult.Message.Types.Error Then _
                                                        Throw New Exception.ServiceSocketException(CType(BindInvokeResult.InvokeResult, [Shared].ControlResult.Message).Message)
                                                Else
                                                    ' Just Ignore any result other than Exception and Error Message Result Object
                                                End If
                                            End If
                                        End If

                                End Select
                            End If
                        End If
                    End If
                    ' !--
QUICKFINISH:
                Catch ex As System.Exception
                    If TypeOf ex Is System.Web.HttpException AndAlso Not [Shared].Configurations.LogHTTPExceptions Then Exit Try

                    Dim CurrentContextCatch As [Shared].IHttpContext =
                        [Shared].Helpers.Context
                    If CurrentContextCatch Is Nothing Then CurrentContextCatch = Context

                    ' Prepare For Exception List
                    Dim CompiledExceptions As New Text.StringBuilder()

                    CompiledExceptions.AppendLine("-- APPLICATION EXCEPTION --")
                    CompiledExceptions.Append(ex.ToString())
                    CompiledExceptions.AppendLine()
                    ' ----

                    Dim LogResult As New Text.StringBuilder()

                    LogResult.AppendLine("-- Session Variables --")
                    ' -- Session Log Text
                    Dim SessionItems As KeyValuePair(Of String, Object)() =
                        CurrentContextCatch.Session.Items

                    If Not SessionItems Is Nothing Then
                        Try
                            For Each Item As KeyValuePair(Of String, Object) In SessionItems
                                LogResult.AppendLine(String.Format(" {0} -> {1}", Item.Key, Item.Value))
                            Next
                        Catch exSession As System.Exception
                            ' The collection was modified after the enumerator was created.

                            LogResult.AppendLine(String.Format(" Exception Occured -> {0}", exSession.Message))
                        End Try
                    End If
                    ' !--
                    LogResult.AppendLine()
                    LogResult.AppendLine("-- Request POST Variables --")
                    For Each KeyItem As String In CurrentContextCatch.Request.Form
                        LogResult.AppendLine(
                            String.Format(" {0} -> {1}", KeyItem, CurrentContextCatch.Request.Form.Item(KeyItem))
                        )
                    Next
                    LogResult.AppendLine()
                    LogResult.AppendLine("-- Request URL & Query String --")
                    LogResult.AppendLine(
                        String.Format("{0}?{1}",
                            CurrentContextCatch.Request.Server.Item("URL"),
                            CurrentContextCatch.Request.Server.Item("QUERY_STRING")
                        )
                    )
                    LogResult.AppendLine()
                    LogResult.AppendLine("-- Error Content --")
                    LogResult.Append(ex.ToString())

                    Try
                        Helper.EventLogger.Log(LogResult.ToString())
                    Catch exLogging As System.Exception
                        CompiledExceptions.AppendLine("-- LOGGING EXCEPTION --")
                        CompiledExceptions.Append(exLogging.ToString())
                        CompiledExceptions.AppendLine()

                        CompiledExceptions.AppendLine("-- ORIGINAL LOG CONTENT --")
                        CompiledExceptions.Append(LogResult.ToString())
                        CompiledExceptions.AppendLine()

                        Helper.EventLogger.LogToSystemEvent(
                            String.Format(" --- Logging Exception --- {0}{0}{1}{0}{0} --- Original Log Content --- {2}",
                                  Environment.NewLine, exLogging.ToString(), LogResult.ToString()), EventLogEntryType.Error)
                    End Try

                    If [Shared].Configurations.Debugging Then
                        Dim OutputSB As New Text.StringBuilder()
                        OutputSB.AppendFormat("<h2 align=""center"" style=""color:#CC0000"">{0}!</h2>", [Global].SystemMessages.SYSTEM_ERROROCCURED)
                        OutputSB.Append("<hr size=""1px"">")
                        OutputSB.AppendFormat("<pre>{0}</pre>", CompiledExceptions.ToString())

                        Dim OutputBytes As Byte() =
                            Text.Encoding.UTF8.GetBytes(OutputSB.ToString())

                        [Shared].Helpers.Context.Response.Header.Item("Content-Type") = "text/html"
                        [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = "identity"

                        CurrentContextCatch.Response.Stream.Write(OutputBytes, 0, OutputBytes.Length)

                        IsEO = True
                    Else
                        If Not Me._DomainControl Is Nothing Then
                            [Shared].Helpers.Context.Content.Item("RedirectLocation") =
                                String.Format("http://{0}{1}",
                                    CurrentContextCatch.Request.Server("HTTP_HOST"),
                                    [Shared].Helpers.GetRedirectURL(
                                        False,
                                        Me._DomainControl.Domain.Settings.Configurations.DefaultPage
                                    )
                                )
                        Else
                            Dim OutputSB As New Text.StringBuilder()
                            OutputSB.AppendFormat("<h2 align=""center"" style=""color:#CC0000"">{0}!</h2>", [Global].SystemMessages.SYSTEM_ERROROCCURED)
                            OutputSB.AppendFormat("<h4 align=""center"">{0}</h4>", ex.Message)

                            Dim OutputBytes As Byte() =
                                Text.Encoding.UTF8.GetBytes(OutputSB.ToString())

                            [Shared].Helpers.Context.Response.Header.Item("Content-Type") = "text/html"
                            [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = "identity"

                            CurrentContextCatch.Response.Stream.Write(OutputBytes, 0, OutputBytes.Length)

                            IsEO = True
                        End If
                    End If
                Finally
                    If Not Me._DomainControl Is Nothing Then Me._DomainControl.Dispose()

                    If Not [Shared].Helpers.Context.Content.Item("RedirectLocation") Is Nothing Then
                        If CType([Shared].Helpers.Context.Content.Item("RedirectLocation"), String).IndexOf("://") = -1 Then
                            Dim RedirectLocation As String =
                                String.Format("http://{0}{1}",
                                        [Shared].Helpers.Context.Request.Server("HTTP_HOST"),
                                        [Shared].Helpers.Context.Content.Item("RedirectLocation")
                                    )

                            [Shared].Helpers.Context.Content.Item("RedirectLocation") = RedirectLocation
                        End If

                        If [Shared].Helpers.Context.Request.Header.Item("X-BlockRenderingID") Is Nothing Then
                            Try
                                [Shared].Helpers.Context.Response.Redirect(
                                    CType([Shared].Helpers.Context.Content.Item("RedirectLocation"), String))
                            Catch ex As System.Web.HttpException
                                ' Just Handle Exceptions (Remote host closed the connection )
                            Catch ex As System.Exception
                                Throw
                            End Try
                        Else
                            Dim RedirectBytes As Byte() =
                                Text.Encoding.UTF8.GetBytes(String.Format("rl:{0}", CType([Shared].Helpers.Context.Content.Item("RedirectLocation"), String)))

                            [Shared].Helpers.Context.Response.Header.Item("Content-Type") = "text/html"
                            [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = "identity"

                            [Shared].Helpers.Context.Response.Stream.Write(RedirectBytes, 0, RedirectBytes.Length)
                        End If
                    End If
                End Try
            End Sub

            Private Sub PostRequestedStaticFileToClient()
                ' This is a common file located somewhere in the webserver
                Dim Context As [Shared].IHttpContext =
                    [Shared].Helpers.Context
                Dim RequestFilePath As String =
                    Context.Request.PhysicalPath

                If Not IO.File.Exists(RequestFilePath) Then
                    Context.Response.StatusCode = 404
                Else
                    Dim ContentType As String =
                        [Shared].Helpers.GetMimeType(
                            IO.Path.GetExtension(RequestFilePath)
                        )

                    Dim Range As String =
                        Context.Request.Header.Item("Range")
                    Dim IsPartialRequest As Boolean =
                        Not String.IsNullOrEmpty(Range)

                    Dim RequestFileStream As IO.Stream = Nothing

                    If Not IsPartialRequest Then
                        Context.Response.Header.Item("Accept-Ranges") = "bytes"

                        Try
                            RequestFileStream =
                                New IO.FileStream(RequestFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                            Context.Response.Header.Item("Content-Length") = RequestFileStream.Length.ToString()

                            Me.WriteOutput(ContentType, RequestFileStream, False)
                        Catch ex As System.Exception
                            Throw
                        Finally
                            If Not RequestFileStream Is Nothing Then RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)
                        End Try
                    Else
                        Dim beginRange As Integer = 0, endRange As Integer = -1

                        If Range.IndexOf("bytes=") = 0 Then
                            Range = Range.Remove(0, "bytes=".Length)

                            If Not Integer.TryParse(Range.Split("-"c)(0), beginRange) Then beginRange = 0
                            If Not Integer.TryParse(Range.Split("-"c)(1), endRange) Then endRange = -1
                        End If

                        Context.Response.Header.Item("Content-Type") = ContentType
                        Context.Response.Header.Item("Content-Encoding") = "identity"
                        Context.Response.StatusCode = 206

                        Try
                            RequestFileStream = New IO.FileStream(RequestFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                            Dim requestingLength As Long = RequestFileStream.Length

                            If endRange = -1 Then
                                requestingLength = requestingLength - beginRange

                                Context.Response.Header.Item("Content-Range") = String.Format("bytes {0}-{1}/{2}", beginRange, requestingLength - 1, RequestFileStream.Length)
                            Else
                                requestingLength = endRange - beginRange

                                Context.Response.Header.Item("Content-Range") = String.Format("bytes {0}-{1}/{2}", beginRange, endRange, RequestFileStream.Length)
                            End If
                            Context.Response.Header.Item("Content-Length") = requestingLength.ToString()

                            RequestFileStream.Seek(beginRange, IO.SeekOrigin.Begin)

                            Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 8192), Byte()), bR As Integer
                            Do
                                bR = RequestFileStream.Read(buffer, 0, buffer.Length)

                                If requestingLength < bR Then bR = CType(requestingLength, Integer)

                                Context.Response.Stream.Write(buffer, 0, bR)

                                requestingLength -= bR
                            Loop Until requestingLength = 0 OrElse bR = 0
                        Catch ex As System.Exception
                            Throw
                        Finally
                            If Not RequestFileStream Is Nothing Then RequestFileStream.Close() : GC.SuppressFinalize(RequestFileStream)
                        End Try
                    End If

                    ' Remove RedirectLocation if it exists
                    [Shared].Helpers.Context.Content.Remove("RedirectLocation")
                End If
            End Sub

            Private Sub PostDomainContentFileToClient(ByVal RequestedFilePathInDomainContents As String)
                Dim RequestFileStream As IO.Stream = Nothing

                ' Write File Content To Theme File Stream
                Me._DomainControl.ProvideFileStream(RequestFileStream, RequestedFilePathInDomainContents)

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

                    [Shared].Helpers.Context.Content.Remove("RedirectLocation")
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
                [Shared].Helpers.Context.Content.Remove("RedirectLocation")
            End Sub

            Private Sub RedirectToAuthenticationPage(Optional ByVal CurrentRequestedTemplate As String = Nothing)
                Select Case Me._DomainControl.ServiceType
                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                        ' Get AuthenticationPage 
                        Dim AuthenticationPage As String =
                            Me._DomainControl.Domain.Settings.Configurations.AuthenticationPage

                        If Not String.IsNullOrEmpty(CurrentRequestedTemplate) AndAlso
                            String.Compare(AuthenticationPage, CurrentRequestedTemplate, True) <> 0 Then

                            [Shared].Helpers.Context.Session.Item("_sys_Referrer") =
                                [Shared].Helpers.Context.Request.URL.Raw
                        End If

                        ' Remove Redirect Location IF Exists
                        [Shared].Helpers.Context.Content.Remove("RedirectLocation")
                        ' Reset Redirect Location to AuthenticationPage
                        [Shared].Helpers.Context.Content.Item("RedirectLocation") =
                            [Shared].Helpers.GetRedirectURL(
                                True,
                                AuthenticationPage
                            )

                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                        Me.WritePage()

                End Select
            End Sub

            Private Overloads Sub WritePage()
                Try
                    Me._DomainControl.RenderService(Me._MessageResult, String.Empty)
                Catch ex As Exception.ReloadRequiredException
                    Me._DomainControl.ClearCache()
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
                    [Shared].Helpers.Context.Request.Header.Item("X-BlockRenderingID")

                Try
                    Me._DomainControl.RenderService(Me._MessageResult, UpdateBlockControlID)
                Catch ex As Exception.ReloadRequiredException
                    Me._DomainControl.ClearCache()
                    RequestModule.ReloadApplication([Shared].Helpers.CurrentRequestID)

                    Exit Sub
                End Try

                Dim CurrentContextCatch As [Shared].IHttpContext = [Shared].Helpers.Context

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

                    For Each kVP As KeyValuePair(Of [Shared].MetaRecord.Tags, String) In [Shared].MetaRecord.RegisteredRecords
                        Select Case [Shared].MetaRecord.QueryTagSpace(kVP.Key)
                            Case [Shared].MetaRecord.TagSpaces.name
                                sW.WriteLine(
                                        String.Format(
                                            "<meta name=""{0}"" content=""{1}"" />",
                                            [Shared].MetaRecord.GetTagHtmlName(kVP.Key),
                                            kVP.Value
                                        )
                                    )
                            Case [Shared].MetaRecord.TagSpaces.httpequiv
                                sW.WriteLine(
                                    String.Format(
                                        "<meta http-equiv=""{0}"" content=""{1}"" />",
                                        [Shared].MetaRecord.GetTagHtmlName(kVP.Key),
                                        kVP.Value
                                    )
                                )
                            Case [Shared].MetaRecord.TagSpaces.property
                                sW.WriteLine(
                                    String.Format(
                                        "<meta property=""{0}"" content=""{1}"" />",
                                        [Shared].MetaRecord.GetTagHtmlName(kVP.Key),
                                        kVP.Value
                                    )
                                )
                        End Select

                        Select Case kVP.Key
                            Case [Shared].MetaRecord.Tags.contenttype
                                IsContentTypeAdded = True
                            Case [Shared].MetaRecord.Tags.pragma
                                IsPragmaAdded = True
                            Case [Shared].MetaRecord.Tags.cachecontrol
                                IsCacheControlAdded = True
                            Case [Shared].MetaRecord.Tags.expires
                                IsExpiresAdded = True
                        End Select
                    Next

                    Dim KeyName As String = String.Empty

                    For Each kVP As KeyValuePair(Of String, String) In [Shared].MetaRecord.RegisteredCustomRecords
                        KeyName = kVP.Key

                        Select Case [Shared].MetaRecord.QueryTagSpace(KeyName)
                            Case [Shared].MetaRecord.TagSpaces.name
                                sW.WriteLine(
                                        String.Format(
                                            "<meta name=""{0}"" content=""{1}"" />",
                                            KeyName,
                                            kVP.Value
                                        )
                                    )
                            Case [Shared].MetaRecord.TagSpaces.httpequiv
                                sW.WriteLine(
                                    String.Format(
                                        "<meta http-equiv=""{0}"" content=""{1}"" />",
                                        KeyName,
                                        kVP.Value
                                    )
                                )
                            Case [Shared].MetaRecord.TagSpaces.property
                                sW.WriteLine(
                                    String.Format(
                                        "<meta property=""{0}"" content=""{1}"" />",
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
                                Text.Encoding.UTF8.WebName
                            )
                        )
                    End If

                    If [Shared].Globals.PageCaching.DefaultType = [Shared].Globals.PageCaching.Types.NoCache OrElse
                        [Shared].Globals.PageCaching.DefaultType = [Shared].Globals.PageCaching.Types.NoCacheCookiless Then

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
                                    Me._DomainControl.Domain.IDAccessTree,
                                    Me._DomainControl.Domain.Language.ID
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
                            "<form method=""post"" action=""{0}{1}/{2}?{3}"" enctype=""multipart/form-data"" style=""margin: 0px; padding: 0px;"">",
                            [Shared].Configurations.ApplicationRoot.BrowserImplementation,
                            CurrentContextCatch.Request.HashCode,
                            Me._DomainControl.ServicePathInfo.FullPath,
                            CurrentContextCatch.Request.QueryString
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
                ' This header is for testing Response.End
                Dim ResponseEndRaised As Boolean = False
                Try
                    [Shared].Helpers.Context.Response.Header.Item("_Dummy_") = "XCT"
                    [Shared].Helpers.Context.Response.Header.Remove("_Dummy_")
                Catch ex As System.Web.HttpException
                    ResponseEndRaised = True
                Catch ex As System.Exception
                    Throw
                End Try
                ' !--

                If Not ResponseEndRaised AndAlso
                    [Shared].Helpers.Context.Content.Item("RedirectLocation") Is Nothing Then

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
                If SendAsCompressed Then
                    Dim ContentBuffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 4096), Byte()), bC As Integer

                    Dim gzippedStream As New IO.MemoryStream()
                    Dim gzipCompression As IO.Compression.GZipStream = Nothing

                    Try
                        gzipCompression = New IO.Compression.GZipStream(gzippedStream, IO.Compression.CompressionMode.Compress, True)

                        Do
                            bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                            gzipCompression.Write(ContentBuffer, 0, bC)
                        Loop While bC > 0
                    Catch ex As System.Exception
                        If Not gzippedStream Is Nothing Then gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)

                        Throw
                    Finally
                        If Not gzipCompression Is Nothing Then gzipCompression.Close() : GC.SuppressFinalize(gzipCompression)
                    End Try

                    If gzippedStream.Length < OutputStream.Length Then
                        [Shared].Helpers.Context.Response.Header.Item("Content-Type") = ContentType
                        [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = "gzip"

                        Me.WriteToSocket(CType(gzippedStream, IO.Stream))

                        gzippedStream.Close() : GC.SuppressFinalize(gzippedStream)
                    Else
                        Me.WriteOutput(ContentType, OutputStream, False)
                    End If
                Else
                    [Shared].Helpers.Context.Response.Header.Item("Content-Type") = ContentType
                    [Shared].Helpers.Context.Response.Header.Item("Content-Encoding") = "identity"

                    Me.WriteToSocket(OutputStream)
                End If
            End Sub

            Private Sub WriteToSocket(ByRef OutputStream As IO.Stream)
                [Shared].Helpers.Context.Response.ReleaseHeader()

                Dim ContentBuffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 4096), Byte()), bC As Integer
                Dim Bandwidth As Long =
                    CType(CType(RequestModule.XeoraSettings, Configuration.XeoraSection).Main.Bandwidth, Long)

                If Bandwidth > 0 Then ContentBuffer = CType(Array.CreateInstance(GetType(Byte), Bandwidth), Byte())

                OutputStream.Seek(0, IO.SeekOrigin.Begin)

                Do
                    bC = OutputStream.Read(ContentBuffer, 0, ContentBuffer.Length)

                    If bC > 0 Then
                        [Shared].Helpers.Context.Response.Stream.Write(ContentBuffer, 0, bC)

                        If Bandwidth > 0 AndAlso bC = Bandwidth Then Threading.Thread.Sleep(1000)
                    End If
                Loop Until bC = 0
            End Sub
        End Class
    End Class
End Namespace