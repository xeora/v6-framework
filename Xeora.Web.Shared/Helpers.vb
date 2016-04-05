Option Strict On

Namespace Xeora.Web.Shared

    Public Class Helpers
        Private Shared _SiteTitles As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared Property SiteTitle() As String
            Get
                Dim rString As String = Nothing

                If Helpers._SiteTitles.ContainsKey(Helpers.CurrentRequestID) Then _
                    rString = CType(Helpers._SiteTitles.Item(Helpers.CurrentRequestID), String)

                Return rString
            End Get
            Set(ByVal value As String)
                Threading.Monitor.Enter(Helpers._SiteTitles.SyncRoot)
                Try
                    If Helpers._SiteTitles.ContainsKey(Helpers.CurrentRequestID) Then _
                        Helpers._SiteTitles.Remove(Helpers.CurrentRequestID)
                    Helpers._SiteTitles.Add(Helpers.CurrentRequestID, value)
                Finally
                    Threading.Monitor.Exit(Helpers._SiteTitles.SyncRoot)
                End Try
            End Set
        End Property

        Private Shared _Favicons As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared Property SiteIconURL() As String
            Get
                Dim rString As String = Nothing

                If Helpers._Favicons.ContainsKey(Helpers.CurrentRequestID) Then _
                    rString = CType(Helpers._Favicons.Item(Helpers.CurrentRequestID), String)

                Return rString
            End Get
            Set(ByVal value As String)
                Threading.Monitor.Enter(Helpers._Favicons.SyncRoot)
                Try
                    If Helpers._Favicons.ContainsKey(Helpers.CurrentRequestID) Then _
                        Helpers._Favicons.Remove(Helpers.CurrentRequestID)
                    Helpers._Favicons.Add(Helpers.CurrentRequestID, value)
                Finally
                    Threading.Monitor.Exit(Helpers._Favicons.SyncRoot)
                End Try
            End Set
        End Property

        Public Overloads Shared Function GetRedirectURL(ByVal TemplateID As String, ParamArray ByVal QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Return Helpers.GetRedirectURL(True, TemplateID, Globals.PageCaching.DefaultType, QueryStrings)
        End Function

        Public Overloads Shared Function GetRedirectURL(ByVal TemplateID As String, ByVal CachingType As Globals.PageCaching.Types, ParamArray ByVal QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Return Helpers.GetRedirectURL(True, TemplateID, CachingType, QueryStrings)
        End Function

        Public Overloads Shared Function GetRedirectURL(ByVal UseSameVariablePool As Boolean, ByVal TemplateID As String, ParamArray ByVal QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Return Helpers.GetRedirectURL(UseSameVariablePool, TemplateID, Globals.PageCaching.DefaultType, QueryStrings)
        End Function

        Public Overloads Shared Function GetRedirectURL(ByVal UseSameVariablePool As Boolean, ByVal TemplateID As String, ByVal CachingType As Globals.PageCaching.Types, ByVal ParamArray QueryStrings As Generic.KeyValuePair(Of String, String)()) As String
            Dim rString As String

            Dim URLQueryDictionary As URLQueryDictionary =
                URLQueryDictionary.Make(QueryStrings)

            If CachingType <> Globals.PageCaching.DefaultType Then _
                URLQueryDictionary.Item("nocache") = Globals.PageCaching.ParseForQueryString(CachingType)

            If Not UseSameVariablePool Then
                rString = String.Format("{0}{1}", Configurations.ApplicationRoot.BrowserImplementation, TemplateID)
            Else
                rString = String.Format("{0}{1}/{2}", Configurations.ApplicationRoot.BrowserImplementation, Helpers.HashCode, TemplateID)
            End If

            If URLQueryDictionary.Count > 0 Then _
                rString = String.Concat(rString, "?", URLQueryDictionary.ToString())

            Return rString
        End Function

        Public Overloads Shared Function GetDomainContentsPath() As String
            Return Helpers.GetDomainContentsPath(Helpers.CurrentDomainIDAccessTree, Helpers.CurrentDomainLanguageID)
        End Function

        Public Overloads Shared Function GetDomainContentsPath(ByVal DomainIDAccessTree As String(), ByVal DomainTranslationID As String) As String
            Dim DomainWebPath As String =
                String.Format("{0}{1}_{2}",
                    Configurations.ApplicationRoot.BrowserImplementation,
                    String.Join(Of String)("-", DomainIDAccessTree),
                    DomainTranslationID)

            Return DomainWebPath
        End Function

        Public Shared Function ResolveTemplateFromURL(ByVal RequestFilePath As String, ByRef UseDefaultTemplate As Boolean) As String
            Dim RequestedTemplate As String = String.Empty

            If Not String.IsNullOrEmpty(RequestFilePath) Then
                Dim ByPass As Boolean = False

                SyncLock URLMapping.InstanceLock
                    Dim URLMI As URLMapping = URLMapping.Current

                    If URLMI.IsActive Then
                        Dim URLMI_C As URLMapping.URLMappingItem() =
                            URLMI.Items.ToArray()
                        Dim rqMatch As Text.RegularExpressions.Match = Nothing

                        For Each mItem As URLMapping.URLMappingItem In URLMI_C
                            rqMatch = Text.RegularExpressions.Regex.Match(RequestFilePath, mItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                            If rqMatch.Success Then
                                RequestedTemplate = mItem.ResolveInfo.TemplateID
                                ByPass = True

                                Exit For
                            End If
                        Next
                    End If
                End SyncLock

                If Not ByPass Then
                    ' RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserImplementation) + Configurations.ApplicationRoot.BrowserImplementation.Length)

                    ' Take Care HashCode if it is exists
                    ' work with application browser path
                    Dim ApplicationRootPath As String = Configurations.ApplicationRoot.BrowserImplementation
                    Dim mR As Text.RegularExpressions.Match =
                        Text.RegularExpressions.Regex.Match(RequestFilePath, String.Format("{0}\d+/", ApplicationRootPath))
                    If mR.Success AndAlso mR.Index = 0 Then _
                        RequestFilePath = RequestFilePath.Remove(ApplicationRootPath.Length, (mR.Length - ApplicationRootPath.Length))

                    ' Take Care DomainContentsPath if exists
                    Dim CurrentDomainContentPath As String =
                        Helpers.GetDomainContentsPath()
                    If RequestFilePath.IndexOf(CurrentDomainContentPath) = 0 Then
                        ' This is a DomainContents Request
                        ' So no Template and also no default template usage
                        RequestFilePath = String.Empty : UseDefaultTemplate = False
                    Else
                        ' this comes /APPPATH/path?somekey=withquery, take care of it!
                        RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(ApplicationRootPath) + ApplicationRootPath.Length)

                        ' Check if there is any query string exists! if so, template will be till there. 
                        If RequestFilePath.IndexOf("?"c) > -1 Then _
                            RequestFilePath = RequestFilePath.Substring(0, RequestFilePath.IndexOf("?"c))

                        If RequestFilePath.IndexOf("/"c) > -1 Then
                            ' Template request can not contains slash! This is a wrong request or Static file Request
                            RequestFilePath = String.Empty : UseDefaultTemplate = False
                        Else
                            If String.IsNullOrEmpty(RequestFilePath) Then UseDefaultTemplate = True
                        End If
                    End If

                    RequestedTemplate = RequestFilePath
                End If
            End If

            Return RequestedTemplate
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

        Public Shared Function ProvideDomainContentsFileStream(ByRef OutputStream As IO.Stream, ByVal FileName As String) As Boolean
            Dim rBoolean As Boolean = True

            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

            objDomain.InvokeMember("ProvideFileStream", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {OutputStream, FileName})

            Return rBoolean
        End Function

        Private Shared ReadOnly Property IsCookiless() As Boolean
            Get
                Dim CookilessSessionString As String =
                    String.Format("{0}_Cookieless", Configurations.VirtualRoot.Replace("/"c, "_"c))
                Dim _IsCookiless As Boolean = False

                Boolean.TryParse(CType(Helpers.Context.Session.Contents.Item(CookilessSessionString), String), _IsCookiless)

                Return _IsCookiless
            End Get
        End Property

        Public Shared Property CurrentDomainIDAccessTree() As String()
            Get
                Dim rCurrentDomainIDAccessTree As String() = New String() {}
                Dim SearchString As String =
                    String.Format("{0}_DomainIDAccessTree", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Helpers.IsCookiless Then
                    If Helpers.Context.Session.Contents.Item(SearchString) Is Nothing Then
                        rCurrentDomainIDAccessTree = New String() {Configurations.DefaultDomain}

                        Helpers.CurrentDomainIDAccessTree = rCurrentDomainIDAccessTree
                    Else
                        rCurrentDomainIDAccessTree = CType(Helpers.Context.Session.Contents.Item(SearchString), String())
                    End If
                Else
                    If Helpers.Context.Request.Cookies.Item(SearchString) Is Nothing OrElse
                        Helpers.Context.Request.Cookies.Item(SearchString).Value Is Nothing OrElse
                        Helpers.Context.Request.Cookies.Item(SearchString).Value.Trim().Length = 0 Then

                        rCurrentDomainIDAccessTree = New String() {Configurations.DefaultDomain}

                        Helpers.CurrentDomainIDAccessTree = rCurrentDomainIDAccessTree
                    Else
                        rCurrentDomainIDAccessTree = Helpers.Context.Request.Cookies.Item(SearchString).Value.Split("-"c)
                    End If
                End If

                Return rCurrentDomainIDAccessTree
            End Get
            Set(ByVal Value As String())
                Dim SearchString As String =
                    String.Format("{0}_DomainIDAccessTree", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Helpers.IsCookiless Then
                    SyncLock Helpers.Context.Session.SyncRoot
                        Try
                            Helpers.Context.Session.Contents.Item(SearchString) = Value
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' TODO: Must investigate SESSION "KEY ADDED" PROBLEMS
                        End Try
                    End SyncLock
                Else
                    Dim DomainID_Cookie As New System.Web.HttpCookie(SearchString, String.Join(Of String)("-", Value))

                    DomainID_Cookie.Expires = Date.Now().AddYears(1)
                    Helpers.Context.Response.Cookies.Add(DomainID_Cookie)
                End If
            End Set
        End Property

        Public Shared Property CurrentDomainLanguageID() As String
            Get
                Dim rCurrentDomainLanguage As String = Nothing

                Dim LanguageSearchString As String =
                    String.Format("{0}_DomainLanguageID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Helpers.IsCookiless Then
                    If Helpers.Context.Session.Contents.Item(LanguageSearchString) Is Nothing Then
                        rCurrentDomainLanguage = Nothing
                    Else
                        rCurrentDomainLanguage = CType(Helpers.Context.Session.Contents.Item(LanguageSearchString), String)
                    End If
                Else
                    If Helpers.Context.Request.Cookies.Item(LanguageSearchString) Is Nothing OrElse
                        Helpers.Context.Request.Cookies.Item(LanguageSearchString).Value Is Nothing OrElse
                        Helpers.Context.Request.Cookies.Item(LanguageSearchString).Value.Trim().Length = 0 Then

                        rCurrentDomainLanguage = Nothing
                    Else
                        rCurrentDomainLanguage = Helpers.Context.Request.Cookies.Item(LanguageSearchString).Value
                    End If
                End If

                Return rCurrentDomainLanguage
            End Get
            Set(ByVal Value As String)
                Dim LanguageSearchString As String =
                    String.Format("{0}_DomainLanguageID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If Helpers.IsCookiless Then
                    SyncLock Helpers.Context.Session.SyncRoot
                        Try
                            Helpers.Context.Session.Contents.Item(LanguageSearchString) = Value
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' TODO: Must investigate SESSION "KEY ADDED" PROBLEMS
                        End Try
                    End SyncLock
                Else
                    Dim DomainLanguage_Cookie As New System.Web.HttpCookie(LanguageSearchString, Value)

                    DomainLanguage_Cookie.Expires = Date.Now().AddYears(1)
                    Helpers.Context.Response.Cookies.Add(DomainLanguage_Cookie)
                End If
            End Set
        End Property

        Public Overloads Shared Function CreateDomainInstance() As IDomain
            Dim rIDomain As IDomain
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

            rIDomain = CType(objDomain.InvokeMember("Domain", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing), IDomain)

            Return rIDomain
        End Function

        Public Overloads Shared Function CreateDomainInstance(ByVal DomainIDAccessTree As String()) As IDomain
            Return Helpers.CreateDomainInstance(DomainIDAccessTree, Nothing)
        End Function

        Public Overloads Shared Function CreateDomainInstance(ByVal DomainIDAccessTree As String(), ByVal DomainTranslationID As String) As IDomain
            Dim rIDomain As IDomain
            Dim DomainAsm As Reflection.Assembly, objDomain As Type

            DomainAsm = Reflection.Assembly.Load("Xeora.Web")
            objDomain = DomainAsm.GetType("Xeora.Web.Site.Domain", False, True)

            rIDomain = CType(Activator.CreateInstance(objDomain, New Object() {DomainIDAccessTree, DomainTranslationID}), IDomain)

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

        Public Shared ReadOnly Property Context() As System.Web.HttpContext
            Get
                Dim rContext As System.Web.HttpContext
                Dim RequestAsm As Reflection.Assembly, objRequest As Type

                RequestAsm = Reflection.Assembly.Load("Xeora.Web.Handler")
                objRequest = RequestAsm.GetType("Xeora.Web.Handler.RequestModule", False, True)

                rContext = CType(objRequest.InvokeMember("Context", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), System.Web.HttpContext)

                Return rContext
            End Get
        End Property

        Public Shared ReadOnly Property URLReferrer() As String
            Get
                Dim rString As String
                Try
                    rString = CType(Helpers.Context.Session.Contents.Item("_sys_Referrer"), String)
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

        Public Shared ReadOnly Property HashCode() As String
            Get
                Dim _HashCode As String

                Dim RequestFilePath As String =
                    Context.Request.FilePath

                RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserImplementation) + Configurations.ApplicationRoot.BrowserImplementation.Length)

                Dim mR As Text.RegularExpressions.Match =
                    Text.RegularExpressions.Regex.Match(RequestFilePath, "\d+/")

                If mR.Success AndAlso
                    mR.Index = 0 Then

                    _HashCode = mR.Value.Substring(0, mR.Value.Length - 1)
                Else
                    _HashCode = Context.GetHashCode().ToString()
                End If

                Return _HashCode
            End Get
        End Property

        Public Shared ReadOnly Property CachingType() As Globals.PageCaching.Types
            Get
                Dim _CachingType As Globals.PageCaching.Types =
                    Globals.PageCaching.Types.AllContent

                SyncLock URLMapping.InstanceLock
                    Dim URLMI As URLMapping = URLMapping.Current

                    If URLMI.IsActive Then
                        Dim SHCMatch As Text.RegularExpressions.Match =
                            Text.RegularExpressions.Regex.Match(Context.Request.FilePath, "\d+(L\d(XC)?)?/")

                        If SHCMatch.Success Then
                            ' Search and set request caching variable if it's exists
                            Dim RequestCaching As String = String.Empty
                            Dim rCIdx As Integer = SHCMatch.Value.IndexOf(","c)
                            If rCIdx > -1 Then _
                                RequestCaching = SHCMatch.Value.Substring(rCIdx + 1, SHCMatch.Length - (rCIdx + 2))
                            ' !--

                            _CachingType = Globals.PageCaching.ParseFromQueryString(RequestCaching)
                        End If
                    Else
                        _CachingType = Globals.PageCaching.ParseFromQueryString(
                                        Helpers.Context.Request.QueryString.Item("nocache"))
                    End If
                End SyncLock

                Return _CachingType
            End Get
        End Property

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
                            String.Format("{0}_{1}", Helpers.Context.Session.SessionID, Helpers.HashCode))
            End Get
        End Property

        Public Shared ReadOnly Property VariablePoolForWebService() As Service.VariablePoolOperation
            Get
                Return New Service.VariablePoolOperation("000000000000000000000000_00000001")
            End Get
        End Property
    End Class

End Namespace