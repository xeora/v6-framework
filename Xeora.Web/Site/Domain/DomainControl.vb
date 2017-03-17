Option Strict On

Namespace Xeora.Web.Site
    Public Class DomainControl
        Implements [Shared].IDomainControl
        Implements IDisposable

        Private _RequestID As String
        Private _Domain As [Shared].IDomain
        Private Shared _ReferenceTable As Hashtable

        Private _ServicePathInfo As [Shared].ServicePathInfo
        Private _ServiceType As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes
        Private _AuthenticationKeys As String()
        Private _IsAuthenticationRequired As Boolean
        Private _IsWorkingAsStandAlone As Boolean

        Private _MimeType As String
        Private _ExecuteIn As String
        Private _ServiceResult As String

        Private _CookieSearchKeyForLanguage As String

        Public Sub New(ByVal RequestID As String, ByVal URL As [Shared].URL)
            Me.Build(RequestID, URL)
        End Sub

        Private Sub Build(ByVal RequestID As String, ByVal URL As [Shared].URL)
            Me._RequestID = RequestID
            Me._Domain = Nothing

            Me._ServicePathInfo = Nothing
            Me._MimeType = String.Empty
            Me._ExecuteIn = String.Empty
            Me._IsAuthenticationRequired = False
            Me._IsWorkingAsStandAlone = False

            Me._ServiceResult = String.Empty

            If DomainControl._ReferenceTable Is Nothing Then _
                DomainControl._ReferenceTable = Hashtable.Synchronized(New Hashtable())

            Threading.Monitor.Enter(DomainControl._ReferenceTable.SyncRoot)
            Try
                DomainControl._ReferenceTable.Item(RequestID) = Me
            Finally
                Threading.Monitor.Exit(DomainControl._ReferenceTable.SyncRoot)
            End Try

            ' Check has ever user changed the Language
            Me._CookieSearchKeyForLanguage =
                String.Format("{0}_LanguageID", [Shared].Configurations.ApplicationRoot.BrowserImplementation.Replace("/"c, "_"c))

            Dim LanguageID As String = String.Empty
            Dim LanguageCookie As System.Web.HttpCookie =
                [Shared].Helpers.Context.Request.Cookie.Item(Me._CookieSearchKeyForLanguage)

            If Not LanguageCookie Is Nothing AndAlso
                Not String.IsNullOrEmpty(LanguageCookie.Value) Then

                LanguageID = LanguageCookie.Value
            End If
            ' !---

            Me.SelectDomain(URL, LanguageID)
        End Sub

        Public Shared ReadOnly Property Instance(ByVal RequestID As String) As [Shared].IDomainControl
            Get
                If String.IsNullOrEmpty(RequestID) OrElse
                    Not DomainControl._ReferenceTable.ContainsKey(RequestID) Then _
                    Return Nothing

                Return CType(DomainControl._ReferenceTable.Item(RequestID), [Shared].IDomainControl)
            End Get
        End Property

        Public ReadOnly Property Domain() As [Shared].IDomain Implements [Shared].IDomainControl.Domain
            Get
                Return Me._Domain
            End Get
        End Property

        Public Sub OverrideDomain(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            Me._Domain = New Domain(DomainIDAccessTree, LanguageID)
        End Sub

        Public ReadOnly Property XeoraJSVersion() As String
            Get
                Return My.Settings.ScriptVersion
            End Get
        End Property

        Public Sub ProvideXeoraJSStream(ByRef FileStream As IO.Stream)
            Dim CurrentAssembly As Reflection.Assembly =
                Reflection.Assembly.GetExecutingAssembly()

            FileStream = CurrentAssembly.GetManifestResourceStream(
                                String.Format("_sps_v{0}.js", Me.XeoraJSVersion))
        End Sub

        Public ReadOnly Property ServicePathInfo() As [Shared].ServicePathInfo
            Get
                Return Me._ServicePathInfo
            End Get
        End Property

        Public ReadOnly Property ServiceType() As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes
            Get
                Return Me._ServiceType
            End Get
        End Property

        Public ReadOnly Property ServiceMimeType() As String
            Get
                Return Me._MimeType
            End Get
        End Property

        Public ReadOnly Property SocketEndPoint() As [Shared].Execution.BindInfo
            Get
                Dim rBindInfo As [Shared].Execution.BindInfo = Nothing

                If Me._ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket AndAlso
                    Not String.IsNullOrEmpty(Me._ExecuteIn) Then _
                    rBindInfo = [Shared].Execution.BindInfo.Make(Me._ExecuteIn)

                Return rBindInfo
            End Get
        End Property

        Public ReadOnly Property IsAuthenticationRequired() As Boolean
            Get
                Return Me._IsAuthenticationRequired
            End Get
        End Property

        Public ReadOnly Property IsWorkingAsStandAlone() As Boolean
            Get
                Return Me._IsWorkingAsStandAlone
            End Get
        End Property

        Public Sub RenderService(ByVal MessageResult As [Shared].ControlResult.Message, ByVal UpdateBlockControlID As String)
            If Me._ServicePathInfo Is Nothing Then
                Dim SystemMessage As String = Me._Domain.Language.Get("TEMPLATE_IDMUSTBESET")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.TEMPLATE_IDMUSTBESET

                Throw New System.Exception(SystemMessage & "!")
            End If

            Select Case Me._ServiceType
                Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                    Me._ServiceResult = Me._Domain.Render(Me._ServicePathInfo, MessageResult, UpdateBlockControlID)
                Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                    If Me._IsAuthenticationRequired Then
                        Dim PostedExecuteParameters As [Shared].xService.Parameters =
                            New [Shared].xService.Parameters([Shared].Helpers.Context.Request.Form.Item("execParams"))

                        If Not PostedExecuteParameters.PublicKey Is Nothing Then
                            Me._IsAuthenticationRequired = False

                            For Each AuthKey As String In Me._AuthenticationKeys
                                If Me._Domain.xService.ReadSessionVariable(PostedExecuteParameters.PublicKey, AuthKey) Is Nothing Then
                                    Me._IsAuthenticationRequired = True

                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If Not Me._IsAuthenticationRequired Then
                        Me._ServiceResult = Me._Domain.xService.RenderxService(Me._ExecuteIn, Me._ServicePathInfo.ServiceID)
                    Else
                        Dim MethodResult As Object =
                            New Security.SecurityException([Global].SystemMessages.XSERVICE_AUTH)

                        Me._ServiceResult = Me._Domain.xService.GeneratexServiceXML(MethodResult)
                    End If
            End Select
        End Sub

        Public ReadOnly Property ServiceResult As String
            Get
                Return Me._ServiceResult
            End Get
        End Property

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String) Implements [Shared].IDomainControl.ProvideFileStream
            Dim WorkingInstance As Domain =
                CType(Me._Domain, Domain)
            Do
                WorkingInstance.ProvideFileStream(FileStream, RequestedFilePath)

                If FileStream Is Nothing Then WorkingInstance = CType(WorkingInstance.Parent, Domain)
            Loop Until WorkingInstance Is Nothing OrElse Not FileStream Is Nothing
        End Sub

        Public Sub PushLanguageChange(ByVal LanguageID As String) Implements [Shared].IDomainControl.PushLanguageChange
            ' Make the language Persist
            Dim LanguageCookie As System.Web.HttpCookie =
                [Shared].Helpers.Context.Request.Cookie.Item(Me._CookieSearchKeyForLanguage)

            If LanguageCookie Is Nothing Then _
                LanguageCookie = New System.Web.HttpCookie(Me._CookieSearchKeyForLanguage)

            LanguageCookie.Value = LanguageID
            LanguageCookie.Expires = Date.Now.AddDays(30)

            [Shared].Helpers.Context.Response.Cookie.Add(LanguageCookie)
            '!---

            CType(Me._Domain, Domain).PushLanguageChange(LanguageID)
        End Sub

        Public Function QueryURLResolver(ByVal RequestFilePath As String) As [Shared].URLMapping.ResolvedMapped Implements [Shared].IDomainControl.QueryURLResolver
            Return Me.QueryURLResolver(Nothing, RequestFilePath)
        End Function

        Private Function QueryURLResolver(ByRef WorkingInstance As [Shared].IDomain, ByVal RequestFilePath As String) As [Shared].URLMapping.ResolvedMapped
            If WorkingInstance Is Nothing Then WorkingInstance = Me._Domain

            If Not WorkingInstance Is Nothing AndAlso
                WorkingInstance.Settings.URLMappings.IsActive AndAlso
                Not String.IsNullOrEmpty(WorkingInstance.Settings.URLMappings.ResolverExecutable) Then

                Dim ResolverBindInfo As [Shared].Execution.BindInfo =
                    [Shared].Execution.BindInfo.Make(
                        String.Format("{0}?URLResolver,rfp", WorkingInstance.Settings.URLMappings.ResolverExecutable))
                ResolverBindInfo.PrepareProcedureParameters(
                        New [Shared].Execution.BindInfo.ProcedureParser(
                            Sub(ByRef ProcedureParameter As [Shared].Execution.BindInfo.ProcedureParameter)
                                ProcedureParameter.Value = RequestFilePath
                            End Sub)
                    )
                ResolverBindInfo.InstanceExecution = True

                Dim ResolverBindInvokeResult As [Shared].Execution.BindInvokeResult =
                    Manager.Assembly.InvokeBind(ResolverBindInfo, Manager.Assembly.ExecuterTypes.Undefined)

                If Not ResolverBindInvokeResult.ReloadRequired AndAlso
                    TypeOf ResolverBindInvokeResult.InvokeResult Is [Shared].URLMapping.ResolvedMapped Then

                    Return CType(ResolverBindInvokeResult.InvokeResult, [Shared].URLMapping.ResolvedMapped)
                End If
            End If

            Return Nothing
        End Function

        ' Cache for performance consideration
        Private Shared _AvailableDomains As [Shared].DomainInfo.DomainInfoCollection = Nothing
        Public Shared Function GetAvailableDomains() As [Shared].DomainInfo.DomainInfoCollection
            Dim DomainDI As IO.DirectoryInfo =
                New IO.DirectoryInfo(
                    IO.Path.Combine(
                        [Shared].Configurations.PhysicalRoot,
                        String.Format("{0}Domains", [Shared].Configurations.ApplicationRoot.FileSystemImplementation)
                    )
                )

            If DomainControl._AvailableDomains Is Nothing Then
                Dim rDomainInfoCollection As New [Shared].DomainInfo.DomainInfoCollection()

                Try
                    For Each DI As IO.DirectoryInfo In DomainDI.GetDirectories()
                        Dim Languages As [Shared].DomainInfo.LanguageInfo() =
                            Deployment.DomainDeployment.AvailableLanguageInfos(New String() {DI.Name})

                        Dim DomainDeployment As Deployment.DomainDeployment =
                            New Deployment.DomainDeployment(New String() {DI.Name}, Languages(0).ID)

                        Dim DomainInfo As New [Shared].DomainInfo(DomainDeployment.DeploymentType, DI.Name, Languages)
                        DomainInfo.Children.AddRange(DomainDeployment.Children)

                        rDomainInfoCollection.Add(DomainInfo)
                    Next

                    DomainControl._AvailableDomains = rDomainInfoCollection
                Catch ex As System.Exception
                    Helper.EventLogger.LogToSystemEvent(ex.ToString(), EventLogEntryType.Error)
                End Try
            Else
                If DomainControl._AvailableDomains.Count <> DomainDI.GetDirectories().Length Then
                    DomainControl._AvailableDomains = Nothing

                    Return DomainControl.GetAvailableDomains()
                End If

                For Each DI As IO.DirectoryInfo In DomainDI.GetDirectories()
                    If DomainControl._AvailableDomains.Item(DI.Name) Is Nothing Then
                        DomainControl._AvailableDomains = Nothing

                        Return DomainControl.GetAvailableDomains()
                    End If
                Next
            End If

            Return DomainControl._AvailableDomains
        End Function

        Public Sub ClearCache()
            CType(Me._Domain, Domain).ClearCache()
        End Sub

        Private Sub SelectDomain(ByVal URL As [Shared].URL, ByVal LanguageID As String)
            Dim RequestedServiceID As String = Me.GetRequestedServiceID(URL)

            If String.IsNullOrEmpty(RequestedServiceID) Then
                Me._Domain = New Domain([Shared].Configurations.DefaultDomain, LanguageID)
                Me.PrepareService(URL, False)

                If Not Me._ServicePathInfo Is Nothing Then Exit Sub

                Me._Domain.Dispose()
            Else
                ' First search the request on top domains
                For Each DI As [Shared].DomainInfo In DomainControl.GetAvailableDomains()
                    Me._Domain = New Domain(New String() {DI.ID}, LanguageID)
                    Me.PrepareService(URL, False)

                    If Not Me._ServicePathInfo Is Nothing Then Exit Sub

                    Me._Domain.Dispose()
                Next

                ' If no results, start again by including children
                For Each DI As [Shared].DomainInfo In DomainControl.GetAvailableDomains()
                    Me._Domain = New Domain(New String() {DI.ID}, LanguageID)
                    Me.PrepareService(URL, True)

                    If Not Me._ServicePathInfo Is Nothing Then Exit Sub

                    Me._Domain.Dispose()
                Next
            End If
        End Sub

        Private Sub PrepareService(ByVal URL As [Shared].URL, ByVal ActivateChildrenSearch As Boolean)
            Dim WorkingInstance As [Shared].IDomain = Me._Domain

            Me._ServicePathInfo = Me.TryResolveURL(WorkingInstance, URL)
            If Me._ServicePathInfo Is Nothing Then Exit Sub

            Dim ServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem =
                WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(Me._ServicePathInfo.FullPath)

            If Not ServiceItem Is Nothing Then
                Dim CachedInstance As [Shared].IDomain = WorkingInstance
                Dim CachedServiceItem As [Shared].IDomain.ISettings.IServices.IServiceItem = ServiceItem

                Do While CachedServiceItem.Overridable
                    WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, URL)

                    ' If not null, it means WorkingInstance contains a service definition which will override
                    If Not WorkingInstance Is Nothing Then
                        CachedInstance = WorkingInstance
                        CachedServiceItem = WorkingInstance.Settings.Services.ServiceItems.GetServiceItem(Me._ServicePathInfo.FullPath)

                        If ServiceItem.ServiceType <> CachedServiceItem.ServiceType Then Exit Do

                        ' Merge or set the authenticationkeys
                        If CachedServiceItem.Authentication Then
                            If CachedServiceItem.AuthenticationKeys.Length = 0 Then
                                CachedServiceItem.AuthenticationKeys = ServiceItem.AuthenticationKeys
                            Else
                                ' Merge
                                Dim Keys As String() =
                                    CType(Array.CreateInstance(GetType(String), CachedServiceItem.AuthenticationKeys.Length + ServiceItem.AuthenticationKeys.Length), String())

                                Array.Copy(CachedServiceItem.AuthenticationKeys, 0, Keys, 0, CachedServiceItem.AuthenticationKeys.Length)
                                Array.Copy(ServiceItem.AuthenticationKeys, 0, Keys, CachedServiceItem.AuthenticationKeys.Length, ServiceItem.AuthenticationKeys.Length)

                                CachedServiceItem.AuthenticationKeys = Keys
                            End If
                        End If

                        ServiceItem = CachedServiceItem
                    Else
                        Exit Do
                    End If
                Loop

                Me._ServiceType = ServiceItem.ServiceType
                Me._AuthenticationKeys = ServiceItem.AuthenticationKeys

                Select Case CachedServiceItem.ServiceType
                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
                        If String.Compare(Me._ServicePathInfo.FullPath, CachedInstance.Settings.Configurations.AuthenticationPage, True) = 0 Then
                            ' Overrides that page does not need authentication even it has been marked as authentication required in Configuration definition
                            Me._IsAuthenticationRequired = False
                        Else
                            If ServiceItem.Authentication Then
                                For Each AuthKey As String In ServiceItem.AuthenticationKeys
                                    If [Shared].Helpers.Context.Session.Item(AuthKey) Is Nothing Then
                                        Me._IsAuthenticationRequired = True

                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xService
                        Me._IsAuthenticationRequired = ServiceItem.Authentication
                    Case [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.xSocket
                        Me._IsAuthenticationRequired = ServiceItem.Authentication
                End Select

                Me._IsWorkingAsStandAlone = ServiceItem.StandAlone
                Me._ExecuteIn = ServiceItem.ExecuteIn
                Me._MimeType = ServiceItem.MimeType

                Me._Domain = CachedInstance
            Else
                ' If ServiceItem is null but ServicePathInfo is not, then there should be a map match
                ' with the a service on other domain. So start the whole process with the rewritten url
                If Not Me._ServicePathInfo Is Nothing AndAlso
                    Me._ServicePathInfo.IsMapped Then

                    Me.Build(Me._RequestID, [Shared].Helpers.Context.Request.URL)

                    Exit Sub
                End If

                If Not ActivateChildrenSearch Then
                    Me._ServicePathInfo = Nothing
                Else
                    ' Search SubDomains For Match
                    WorkingInstance = Me.SearchChildrenThatOverrides(WorkingInstance, URL)

                    If Not WorkingInstance Is Nothing Then
                        ' Set the Working domain as child domain for this call because call requires the child domain access!
                        Me._Domain = WorkingInstance
                        Me.PrepareService(URL, True)
                    Else
                        ' Nothing Found in Anywhere
                        '[Shared].Helpers.Context.Response.StatusCode = 404

                        Me._ServicePathInfo = Nothing
                    End If
                End If
            End If
        End Sub

        Private Function TryResolveURL(ByRef WorkingInstance As [Shared].IDomain, ByVal URL As [Shared].URL) As [Shared].ServicePathInfo
            If WorkingInstance.Settings.URLMappings.IsActive Then
                ' First Try Dynamic Resolve
                Dim ResolvedMapped As [Shared].URLMapping.ResolvedMapped =
                    Me.QueryURLResolver(WorkingInstance, URL.RelativePath)

                If ResolvedMapped Is Nothing OrElse Not ResolvedMapped.IsResolved Then
                    ' No Result So Check Static Definitions
                    For Each URLMapItem As [Shared].URLMapping.URLMappingItem In WorkingInstance.Settings.URLMappings.Items
                        Dim rqMatch As Text.RegularExpressions.Match =
                            Text.RegularExpressions.Regex.Match(URL.RelativePath, URLMapItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then
                            ResolvedMapped = New [Shared].URLMapping.ResolvedMapped(True, URLMapItem.ResolveInfo.ServicePathInfo)

                            Dim medItemValue As String
                            For Each medItem As [Shared].URLMapping.ResolveInfos.MappedItem In URLMapItem.ResolveInfo.MappedItems
                                medItemValue = String.Empty

                                If Not String.IsNullOrEmpty(medItem.ID) Then medItemValue = rqMatch.Groups.Item(medItem.ID).Value

                                ResolvedMapped.URLQueryDictionary.Item(medItem.QueryStringKey) =
                                    CType(IIf(String.IsNullOrEmpty(medItemValue), medItem.DefaultValue, medItemValue), String)
                            Next

                            Exit For
                        End If
                    Next
                End If

                If Not ResolvedMapped Is Nothing AndAlso ResolvedMapped.IsResolved Then
                    Me.RectifyRequestPath(ResolvedMapped)

                    Return ResolvedMapped.ServicePathInfo
                End If
            End If

            ' Take Care Application Path and HashCode if it is exists work with application browser path
            ' this comes /APPPATH(/path?somekey=withquery)?
            ' or this /APPPATH/432432/(path?somekey=withquery)?
            ' or this /Standart_tr-TR/somefile.png
            ' take care of it!
            Dim CurrentDomainContentPath As String =
                WorkingInstance.ContentsVirtualPath

            ' first test if it is domain content path
            If URL.RelativePath.IndexOf(CurrentDomainContentPath) <> 0 Then
                Dim RequestFilePath As String = Me.GetRequestedServiceID(URL)

                If Not String.IsNullOrEmpty(RequestFilePath) Then
                    Dim rServicePathInfo As [Shared].ServicePathInfo =
                        [Shared].ServicePathInfo.Parse(RequestFilePath, False)

                    If String.IsNullOrEmpty(rServicePathInfo.ServiceID) Then Return Nothing

                    Return rServicePathInfo
                Else
                    Return [Shared].ServicePathInfo.Parse(WorkingInstance.Settings.Configurations.DefaultPage, False)
                End If
            End If

            Return Nothing
        End Function

        Private Function GetRequestedServiceID(ByVal URL As [Shared].URL) As String
            Dim RequestFilePath As String = URL.RelativePath

            Dim ApplicationRootPath As String = [Shared].Configurations.ApplicationRoot.BrowserImplementation
            Dim mR As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(RequestFilePath, String.Format("{0}(\d+/)?", ApplicationRootPath))
            If mR.Success AndAlso mR.Index = 0 Then _
                RequestFilePath = RequestFilePath.Remove(0, mR.Length)

            ' Check if there is any query string exists! if so, template will be till there. 
            If RequestFilePath.IndexOf("?"c) > -1 Then _
                RequestFilePath = RequestFilePath.Substring(0, RequestFilePath.IndexOf("?"c))

            Return RequestFilePath
        End Function

        Private Sub RectifyRequestPath(ByVal ResolvedMapped As [Shared].URLMapping.ResolvedMapped)
            Dim RequestURL As String =
                String.Format("{0}{1}",
                    [Shared].Configurations.ApplicationRoot.BrowserImplementation,
                    ResolvedMapped.ServicePathInfo.FullPath
                )
            If ResolvedMapped.URLQueryDictionary.Count > 0 Then _
                RequestURL = String.Concat(RequestURL, "?", ResolvedMapped.URLQueryDictionary.ToString())

            ' Let the server understand what this URL is about...
            [Shared].Helpers.Context.Request.RewritePath(RequestURL)
        End Sub

        Private Function SearchChildrenThatOverrides(ByRef WorkingInstance As [Shared].IDomain, ByRef URL As [Shared].URL) As [Shared].IDomain
            If WorkingInstance Is Nothing Then Return Nothing

            Dim ChildDomainIDAccessTree As New Generic.List(Of String)
            ChildDomainIDAccessTree.AddRange(WorkingInstance.IDAccessTree)

            For Each ChildDI As [Shared].DomainInfo In WorkingInstance.Children
                ChildDomainIDAccessTree.Add(ChildDI.ID)

                Dim rDomainInstance As [Shared].IDomain =
                    New Domain(ChildDomainIDAccessTree.ToArray(), Me._Domain.Language.ID)

                Dim ServicePathInfo As [Shared].ServicePathInfo =
                    Me.TryResolveURL(rDomainInstance, URL)
                If ServicePathInfo Is Nothing Then Continue For

                If rDomainInstance.Settings.Services.ServiceItems.GetServiceItem(ServicePathInfo.FullPath) Is Nothing Then
                    If rDomainInstance.Children.Count > 0 Then
                        rDomainInstance = Me.SearchChildrenThatOverrides(rDomainInstance, URL)

                        If Not rDomainInstance Is Nothing Then Return rDomainInstance
                    End If

                    If ServicePathInfo.IsMapped Then
                        URL = [Shared].Helpers.Context.Request.URL

                        Return WorkingInstance
                    End If
                Else
                    Return rDomainInstance
                End If

                rDomainInstance.Dispose()
                ChildDomainIDAccessTree.RemoveAt(ChildDomainIDAccessTree.Count - 1)
            Next

            Return Nothing
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                Me._Domain.Dispose()

                Threading.Monitor.Enter(DomainControl._ReferenceTable.SyncRoot)
                Try
                    If DomainControl._ReferenceTable.ContainsKey(Me._RequestID) Then _
                        DomainControl._ReferenceTable.Remove(Me._RequestID)
                Finally
                    Threading.Monitor.Exit(DomainControl._ReferenceTable.SyncRoot)
                End Try
            End If

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace