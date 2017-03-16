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
            Return Helpers.GetDomainContentsPath(Helpers.CurrentDomainInstance.IDAccessTree, Helpers.CurrentDomainInstance.Language.ID)
        End Function

        Public Overloads Shared Function GetDomainContentsPath(ByVal DomainIDAccessTree As String(), ByVal DomainLanguageID As String) As String
            Dim DomainWebPath As String =
                String.Format("{0}{1}_{2}",
                    Configurations.ApplicationRoot.BrowserImplementation,
                    String.Join(Of String)("-", DomainIDAccessTree),
                    DomainLanguageID)

            Return DomainWebPath
        End Function

        Public Shared Function ResolveServicePathInfoFromURL(ByVal RequestFilePath As String) As ServicePathInfo
            If Not String.IsNullOrEmpty(RequestFilePath) Then
                Dim ByPass As Boolean = False

                Dim URLMappingInstance As URLMapping = URLMapping.Current

                If Not URLMappingInstance Is Nothing AndAlso
                    URLMappingInstance.IsActive Then

                    Dim URLMappingItems As URLMapping.URLMappingItem() =
                        URLMappingInstance.Items.ToArray()
                    Dim rqMatch As Text.RegularExpressions.Match = Nothing

                    For Each URLMapItem As URLMapping.URLMappingItem In URLMappingItems
                        rqMatch = Text.RegularExpressions.Regex.Match(RequestFilePath, URLMapItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then Return URLMapItem.ResolveInfo.ServicePathInfo
                    Next
                End If

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
                    Return Nothing
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
                        Return ServicePathInfo.Parse(Helpers.CurrentDomainInstance.Settings.Configurations.DefaultPage, False)
                    Else
                        Return ServicePathInfo.Parse(RequestFilePath, False)
                    End If
                End If
            End If

            Return Nothing
        End Function

        Private Shared ReadOnly Property DomainControlInstance As IDomainControl
            Get
                Dim DomainAsm As Reflection.Assembly, objDomain As Type

                DomainAsm = Reflection.Assembly.Load("Xeora.Web")
                objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

                Return CType(objDomain.InvokeMember("Instance", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IDomainControl)
            End Get
        End Property

        Public Shared Sub ProvideDomainContentsFileStream(ByRef OutputStream As IO.Stream, ByVal FileName As String)
            Helpers.DomainControlInstance.ProvideFileStream(OutputStream, FileName)
        End Sub

        Public Shared Sub PushLanguageChange(ByVal LanguageID As String)
            Helpers.DomainControlInstance.PushLanguageChange(LanguageID)
        End Sub

        Private Shared ReadOnly Property IsCookiless() As Boolean
            Get
                Dim _IsCookiless As Boolean = False

                Select Case Helpers.CurrentDomainInstance.Settings.Configurations.DefaultCaching
                    Case [Enum].PageCachingTypes.AllContentCookiless,
                         [Enum].PageCachingTypes.NoCacheCookiless,
                         [Enum].PageCachingTypes.TextsOnlyCookiless

                        _IsCookiless = True
                    Case Else
                        _IsCookiless = False
                End Select

                Return _IsCookiless
            End Get
        End Property

        Public Shared ReadOnly Property CurrentDomainInstance() As IDomain
            Get
                Return Helpers.DomainControlInstance.Domain
            End Get
        End Property

        Public Overloads Shared Function CreateNewDomainInstance(ByVal DomainIDAccessTree As String()) As IDomain
            Return Helpers.CreateNewDomainInstance(DomainIDAccessTree, Nothing)
        End Function

        Public Overloads Shared Function CreateNewDomainInstance(ByVal DomainIDAccessTree As String(), ByVal DomainLanguageID As String) As IDomain
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