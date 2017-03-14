Option Strict On

Namespace Xeora.Web.Shared

    Public Class Helpers
        Private Shared _SiteTitles As New Concurrent.ConcurrentDictionary(Of String, String)
        Public Shared Property SiteTitle() As String
            Get
                Dim rString As String = Nothing

                If Not Helpers._SiteTitles.TryGetValue(Helpers.CurrentRequestID, rString) Then _
                    rString = Nothing

                Return rString
            End Get
            Set(ByVal value As String)
                Helpers._SiteTitles.TryAdd(Helpers.CurrentRequestID, value)
            End Set
        End Property

        Private Shared _Favicons As New Concurrent.ConcurrentDictionary(Of String, String)
        Public Shared Property SiteIconURL() As String
            Get
                Dim rString As String = Nothing

                If Helpers._Favicons.TryGetValue(Helpers.CurrentRequestID, rString) Then _
                    rString = Nothing

                Return rString
            End Get
            Set(ByVal value As String)
                Helpers._Favicons.TryAdd(Helpers.CurrentRequestID, value)
            End Set
        End Property

        Public Overloads Shared Function GetRedirectURL(ByVal ServiceFullPath As String, ParamArray ByVal QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Return Helpers.GetRedirectURL(True, ServiceFullPath, QueryStrings)
        End Function

        Public Overloads Shared Function GetRedirectURL(ByVal UseSameVariablePool As Boolean, ByVal ServiceFullPath As String, ParamArray ByVal QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Dim rString As String

            Dim URLQueryDictionary As URLQueryDictionary =
                URLQueryDictionary.Make(QueryStrings)

            If Not UseSameVariablePool Then
                rString = String.Format("{0}{1}", Configurations.ApplicationRoot.BrowserImplementation, ServiceFullPath)
            Else
                rString = String.Format("{0}{1}/{2}", Configurations.ApplicationRoot.BrowserImplementation, Helpers.Context.Request.HashCode, ServiceFullPath)
            End If

            If URLQueryDictionary.Count > 0 Then _
                rString = String.Concat(rString, "?", URLQueryDictionary.ToString())

            Return rString
        End Function

        Public Overloads Shared Function GetDomainContentsPath() As String
            Return Helpers.GetDomainContentsPath(Helpers.CurrentDomainIDAccessTree, Helpers.CurrentDomainLanguageID)
        End Function

        Public Overloads Shared Function GetDomainContentsPath(ByVal DomainIDAccessTree As String(), ByVal DomainLanguageID As String) As String
            Dim DomainWebPath As String =
                String.Format("{0}{1}_{2}",
                    Configurations.ApplicationRoot.BrowserImplementation,
                    String.Join(Of String)("-", DomainIDAccessTree),
                    DomainLanguageID)

            Return DomainWebPath
        End Function

        Public Shared Function ResolveServicePathInfoFromURL(ByVal RequestFilePath As String, ByRef UseDefaultTemplate As Boolean) As ServicePathInfo
            Dim rServicePathInfo As ServicePathInfo = Nothing

            If Not String.IsNullOrEmpty(RequestFilePath) Then
                Dim ByPass As Boolean = False

                Dim URLMappingInstance As URLMapping = URLMapping.Current

                If URLMappingInstance.IsActive Then
                    Dim URLMappingItems As URLMapping.URLMappingItem() =
                        URLMappingInstance.Items.ToArray()
                    Dim rqMatch As Text.RegularExpressions.Match = Nothing

                    For Each URLMapItem As URLMapping.URLMappingItem In URLMappingItems
                        rqMatch = Text.RegularExpressions.Regex.Match(RequestFilePath, URLMapItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then
                            rServicePathInfo = URLMapItem.ResolveInfo.ServicePathInfo

                            Return rServicePathInfo
                        End If
                    Next
                End If

                ' RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserImplementation) + Configurations.ApplicationRoot.BrowserImplementation.Length)

                ' Take Care Application Path and HashCode if it is exists work with application browser path
                ' this comes /APPPATH(/path?somekey=withquery)?
                ' or this /APPPATH/432432/(path?somekey=withquery)?
                ' or this /Standart_tr-TR/somefile.png
                ' take care of it!
                Dim CurrentDomainContentPath As String =
                    Helpers.GetDomainContentsPath()

                ' first test if it is domain content path
                If RequestFilePath.IndexOf(CurrentDomainContentPath) = 0 Then
                    ' This is a DomainContents Request
                    ' So no Template and also no default template usage
                    UseDefaultTemplate = False
                Else
                    Dim ApplicationRootPath As String = Configurations.ApplicationRoot.BrowserImplementation
                    Dim mR As Text.RegularExpressions.Match =
                        Text.RegularExpressions.Regex.Match(RequestFilePath, String.Format("{0}(\d+/)?", ApplicationRootPath))
                    If mR.Success AndAlso mR.Index = 0 Then _
                        RequestFilePath = RequestFilePath.Remove(0, mR.Length)

                    ' Check if there is any query string exists! if so, template will be till there. 
                    If RequestFilePath.IndexOf("?"c) > -1 Then _
                        RequestFilePath = RequestFilePath.Substring(0, RequestFilePath.IndexOf("?"c))

                    If String.IsNullOrEmpty(RequestFilePath) Then
                        UseDefaultTemplate = True
                    Else
                        rServicePathInfo = ServicePathInfo.Parse(RequestFilePath)

                        If String.IsNullOrEmpty(rServicePathInfo.ServiceID) Then UseDefaultTemplate = True
                    End If
                End If
            End If

            Return rServicePathInfo
        End Function

        Public Shared Function GetMimeType(ByVal FileExtension As String) As String
            Dim rString As Object = "application/octet-stream"

            Try
                Dim HelperAsm As Reflection.Assembly, objHelper As Type
                Dim HelperInstance As Object = Nothing

                HelperAsm = Reflection.Assembly.Load("Xeora.Web")
                objHelper = HelperAsm.GetType("Xeora.Web.Helper.Registry")

                For Each _ctorInfo As Reflection.ConstructorInfo In objHelper.GetConstructors()
                    If _ctorInfo.IsConstructor AndAlso
                        _ctorInfo.IsPublic AndAlso
                        _ctorInfo.GetParameters().Length = 1 Then

                        HelperInstance = _ctorInfo.Invoke(New Object() {0})

                        Exit For
                    End If
                Next

                If Not HelperInstance Is Nothing Then
                    Dim AccessPathProp As Reflection.PropertyInfo =
                        HelperInstance.GetType().GetProperty("AccessPath", GetType(String))
                    AccessPathProp.SetValue(HelperInstance, FileExtension, Nothing)

                    rString = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Content Type", Object)})

                    If rString Is Nothing Then rString = "application/octet-stream"
                End If
            Catch ex As Exception
                ' Do Nothing Just Handle Exception
            End Try

            Return rString.ToString()
        End Function

        Public Shared Function GetExtensionFromMimeType(ByVal MimeType As String) As String
            Dim rString As Object = ".dat"

            Try
                Dim HelperAsm As Reflection.Assembly, objHelper As Type
                Dim HelperInstance As Object = Nothing

                HelperAsm = Reflection.Assembly.Load("Xeora.Web")
                objHelper = HelperAsm.GetType("Xeora.Web.Helper.Registry")

                For Each _ctorInfo As Reflection.ConstructorInfo In objHelper.GetConstructors()
                    If _ctorInfo.IsConstructor AndAlso
                        _ctorInfo.IsPublic AndAlso
                        _ctorInfo.GetParameters().Length = 1 Then

                        HelperInstance = _ctorInfo.Invoke(New Object() {0})

                        Exit For
                    End If
                Next

                If Not HelperInstance Is Nothing Then
                    Dim AccessPathProp As Reflection.PropertyInfo =
                        HelperInstance.GetType().GetProperty("AccessPath", GetType(String))
                    AccessPathProp.SetValue(HelperInstance, String.Format("Mime\Database\Content Type\{0}", MimeType), Nothing)

                    rString = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Extension", Object)})

                    If rString Is Nothing Then rString = ".dat"
                End If
            Catch ex As Exception
                ' Do Nothing Just Handle Exception
            End Try

            Return rString.ToString()
        End Function

        Public Shared Sub ProvideDomainContentsFileStream(ByRef OutputStream As IO.Stream, ByVal FileName As String)
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

            Dim workingDomainControl As IDomainControl =
                CType(objDomain.InvokeMember("Instance", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IDomainControl)

            workingDomainControl.ProvideFileStream(OutputStream, FileName)
        End Sub

        Public Shared Sub PushLanguageChange(ByVal LanguageID As String)
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

            Dim workingDomainControl As IDomainControl =
                CType(objDomain.InvokeMember("Instance", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IDomainControl)

            workingDomainControl.PushLanguageChange(LanguageID)
        End Sub

        Private Shared ReadOnly Property IsCookiless() As Boolean
            Get
                Dim _IsCookiless As Boolean = False

                Select Case Globals.PageCaching.DefaultType
                    Case Globals.PageCaching.Types.AllContentCookiless,
                         Globals.PageCaching.Types.NoCacheCookiless,
                         Globals.PageCaching.Types.TextsOnlyCookiless

                        _IsCookiless = True
                    Case Else
                        _IsCookiless = False
                End Select

                Return _IsCookiless
            End Get
        End Property

        Public Shared Property CurrentDomainIDAccessTree() As String()
            Get
                Dim rCurrentDomainIDAccessTree As String() = Nothing
                Dim SearchString As String =
                    String.Format("{0}_DomainIDAccessTree", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Not Helpers.Context.Session.Item(SearchString) Is Nothing Then _
                    rCurrentDomainIDAccessTree = CType(Helpers.Context.Session.Item(SearchString), String())

                If Not Helpers.IsCookiless AndAlso rCurrentDomainIDAccessTree Is Nothing Then
                    If Helpers.Context.Request.Cookie.Item(SearchString) Is Nothing OrElse
                        Helpers.Context.Request.Cookie.Item(SearchString).Value Is Nothing OrElse
                        Helpers.Context.Request.Cookie.Item(SearchString).Value.Trim().Length = 0 Then

                        rCurrentDomainIDAccessTree = Configurations.DefaultDomain
                        Helpers.CurrentDomainIDAccessTree = rCurrentDomainIDAccessTree
                    Else
                        rCurrentDomainIDAccessTree = Helpers.Context.Request.Cookie.Item(SearchString).Value.Split("\"c)
                    End If
                Else
                    If rCurrentDomainIDAccessTree Is Nothing Then
                        rCurrentDomainIDAccessTree = Configurations.DefaultDomain
                        Helpers.CurrentDomainIDAccessTree = rCurrentDomainIDAccessTree
                    End If
                End If

                Return rCurrentDomainIDAccessTree
            End Get
            Set(ByVal Value As String())
                Dim SearchString As String =
                    String.Format("{0}_DomainIDAccessTree", Configurations.VirtualRoot.Replace("/"c, "_"c))

                Helpers.Context.Session.Item(SearchString) = Value

                If Not Helpers.IsCookiless Then
                    Dim DomainID_Cookie As New System.Web.HttpCookie(SearchString, String.Join(Of String)("\", Value))

                    DomainID_Cookie.Expires = Date.Now.AddMonths(1)
                    Helpers.Context.Response.Cookie.Add(DomainID_Cookie)
                End If
            End Set
        End Property

        Public Shared Property CurrentDomainLanguageID() As String
            Get
                Dim rCurrentDomainLanguage As String = Nothing
                Dim LanguageSearchString As String =
                    String.Format("{0}_DomainLanguageID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Not Helpers.Context.Session.Item(LanguageSearchString) Is Nothing Then _
                    rCurrentDomainLanguage = CType(Helpers.Context.Session.Item(LanguageSearchString), String)

                If Not Helpers.IsCookiless AndAlso String.IsNullOrEmpty(rCurrentDomainLanguage) Then
                    If Helpers.Context.Request.Cookie.Item(LanguageSearchString) Is Nothing OrElse
                        Helpers.Context.Request.Cookie.Item(LanguageSearchString).Value Is Nothing OrElse
                        Helpers.Context.Request.Cookie.Item(LanguageSearchString).Value.Trim().Length = 0 Then

                        rCurrentDomainLanguage = Nothing
                    Else
                        rCurrentDomainLanguage = Helpers.Context.Request.Cookie.Item(LanguageSearchString).Value
                    End If
                End If

                Return rCurrentDomainLanguage
            End Get
            Set(ByVal Value As String)
                Dim LanguageSearchString As String =
                    String.Format("{0}_DomainLanguageID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                Helpers.Context.Session.Item(LanguageSearchString) = Value

                If Not Helpers.IsCookiless Then
                    Dim DomainLanguage_Cookie As New System.Web.HttpCookie(LanguageSearchString, Value)

                    DomainLanguage_Cookie.Expires = Date.Now.AddMonths(1)
                    Helpers.Context.Response.Cookie.Add(DomainLanguage_Cookie)
                End If
            End Set
        End Property

        Public Overloads Shared Function CreateDomainInstance() As IDomain
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

            Dim workingDomainControl As IDomainControl =
                CType(objDomain.InvokeMember("Instance", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IDomainControl)

            Return workingDomainControl.Domain
        End Function

        Public Overloads Shared Function CreateDomainInstance(ByVal DomainIDAccessTree As String()) As IDomain
            Return Helpers.CreateDomainInstance(DomainIDAccessTree, Nothing)
        End Function

        Public Overloads Shared Function CreateDomainInstance(ByVal DomainIDAccessTree As String(), ByVal DomainLanguageID As String) As IDomain
            Dim rIDomain As IDomain
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.Domain", False, True)

            rIDomain = CType(Activator.CreateInstance(objDomain, New Object() {DomainIDAccessTree, DomainLanguageID}), IDomain)

            Return rIDomain
        End Function

        Public Shared ReadOnly Property Domains() As DomainInfo.DomainInfoCollection
            Get
                Dim rIDomains As DomainInfo.DomainInfoCollection
                Dim DomainAsm As Reflection.Assembly, objDomain As Type

                DomainAsm = Reflection.Assembly.Load("Xeora.Web")
                objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

                rIDomains = CType(objDomain.InvokeMember("GetAvailableDomains", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, Nothing), DomainInfo.DomainInfoCollection)

                Return rIDomains
            End Get
        End Property

        Public Shared ReadOnly Property Context() As IHttpContext
            Get
                Dim rContext As IHttpContext
                Dim RequestAsm As Reflection.Assembly, objRequest As Type

                RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
                objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

                rContext = CType(objRequest.InvokeMember("Context", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IHttpContext)

                Return rContext
            End Get
        End Property

        Public Shared ReadOnly Property URLReferrer() As String
            Get
                Dim rString As String
                Try
                    rString = CType(Helpers.Context.Session.Item("_sys_Referrer"), String)
                Catch ex As Exception
                    rString = String.Empty
                End Try

                Return rString
            End Get
        End Property

        Public Shared Function CreateThreadContext() As String
            Dim RequestAsm As Reflection.Assembly, objRequest As Type

            RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
            objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

            Return CType(objRequest.InvokeMember("CreateThreadContext", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), String)
        End Function

        Public Shared Sub DestroyThreadContext(ByVal ThreadRequestID As String)
            Dim RequestAsm As Reflection.Assembly, objRequest As Type

            RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
            objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

            objRequest.InvokeMember("DestroyThreadContext", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {ThreadRequestID})
        End Sub

        Public Shared Sub AssignRequestID(ByVal RequestID As String)
            AppDomain.CurrentDomain.SetData(
                    String.Format("RequestID_{0}",
                        Threading.Thread.CurrentThread.ManagedThreadId), RequestID)
        End Sub

        Public Shared ReadOnly Property CurrentRequestID() As String
            Get
                Return CType(
                            AppDomain.CurrentDomain.GetData(
                                String.Format("RequestID_{0}",
                                    Threading.Thread.CurrentThread.ManagedThreadId)
                                ),
                            String
                        )
            End Get
        End Property

        'Public Shared ReadOnly Property HashCode() As String
        '    Get
        '        Dim _HashCode As String

        '        Dim RequestFilePath As String =
        '            Context.Request.FilePath

        '        RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserImplementation) + Configurations.ApplicationRoot.BrowserImplementation.Length)

        '        Dim mR As Text.RegularExpressions.Match =
        '            Text.RegularExpressions.Regex.Match(RequestFilePath, "\d+/")

        '        If mR.Success AndAlso
        '            mR.Index = 0 Then

        '            _HashCode = mR.Value.Substring(0, mR.Value.Length - 1)
        '        Else
        '            _HashCode = Context.GetHashCode().ToString()
        '        End If

        '        Return _HashCode
        '    End Get
        'End Property

        Public Shared Sub ClearStaticCache()
            Dim RequestAsm As Reflection.Assembly, objRequest As Type

            RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
            objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

            objRequest.InvokeMember("ClearStaticCache", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, Nothing)
        End Sub

        Public Shared Sub ReloadApplication()
            Dim RequestAsm As Reflection.Assembly, objRequest As Type

            RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
            objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

            objRequest.InvokeMember("ReloadApplication", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {Helpers.CurrentRequestID})
        End Sub

        Public Shared ReadOnly Property ScheduledTasks() As Service.IScheduledTasks
            Get
                Return Service.ScheduledTasksOperation.Instance
            End Get
        End Property

        Public Shared ReadOnly Property VariablePool() As Service.VariablePoolOperation
            Get
                Return New Service.VariablePoolOperation(
                            String.Format("{0}_{1}", Helpers.Context.Session.SessionID, Helpers.Context.Request.HashCode))
            End Get
        End Property

        Public Shared ReadOnly Property VariablePoolForxService() As Service.VariablePoolOperation
            Get
                Return New Service.VariablePoolOperation("000000000000000000000000_00000001")
            End Get
        End Property
    End Class

End Namespace