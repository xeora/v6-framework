Namespace SolidDevelopment.Web.Managers
    Public Class ThemeWebControl
        Implements IDisposable

        Private _Theme As Theme

        Private _ServiceID As String = Nothing
        Private _ServiceType As Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes
        Private _IsAuthenticationRequired As Boolean = False
        Private _IsWorkingAsStandAlone As Boolean = False

        Private _MimeType As String = Nothing
        Private _ExecuteIn As String = Nothing
        Private _BlockRenderingID As String = Nothing
        Private _RenderedService As String = Nothing
        Private _MessageResult As PGlobals.MapControls.MessageResult = Nothing
        Private _ArgumentInfoList As New Globals.ArgumentInfo.ArgumentInfoCollection

        Public Sub New(ByVal ThemeID As String, ByVal ThemeTranslationID As String)
            Me._Theme = New Theme(ThemeID, ThemeTranslationID)
        End Sub

        Public ReadOnly Property Theme() As Theme
            Get
                Return Me._Theme
            End Get
        End Property

        Public ReadOnly Property BuiltInScriptVersion() As String
            Get
                Return My.Settings.ScriptVersion
            End Get
        End Property

        Public Sub ProvideBuiltInScriptStream(ByRef ScriptFileStream As IO.Stream)
            Dim CurrentAssembly As System.Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()

            ScriptFileStream = CurrentAssembly.GetManifestResourceStream( _
                                    String.Format("_sps_v{0}.js", Me.BuiltInScriptVersion) _
                                )
        End Sub

        Public Property ServiceID() As String
            Get
                Return Me._ServiceID
            End Get
            Set(ByVal Value As String)
                Me._ServiceID = Value

                Me.PrepareServiceSettings()
            End Set
        End Property

        Public ReadOnly Property ServiceType() As Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes
            Get
                Return Me._ServiceType
            End Get
        End Property

        Public ReadOnly Property MimeType() As String
            Get
                Return Me._MimeType
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

        Public ReadOnly Property URLMappingInfo() As PGlobals.URLMappingInfos
            Get
                Dim rURLMappingInfo As New PGlobals.URLMappingInfos

                Dim SettingsClass As Theme.SettingsClass = _
                    CType(Me._Theme.Settings, Theme.SettingsClass)
                Dim SettingsClass_loop As Theme.SettingsClass

                rURLMappingInfo.URLMapping = SettingsClass.URLMappings.URLMapping
                rURLMappingInfo.URLMappingItems.AddRange(SettingsClass.URLMappings.MappingItems)

                For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                    Me._Theme.Addons.CreateInstance(Addon)

                    SettingsClass_loop = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass)

                    rURLMappingInfo.URLMapping = rURLMappingInfo.URLMapping Or SettingsClass_loop.URLMappings.URLMapping

                    If SettingsClass_loop.URLMappings.URLMapping Then
                        For Each mItem As PGlobals.URLMappingInfos.URLMappingItem In SettingsClass_loop.URLMappings.MappingItems
                            Dim sItem As Theme.SettingsClass.ServicesClass.ServiceItem = _
                                SettingsClass.Services.ServiceItems.GetServiceItem(Managers.Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, mItem.ResolveInfo.TemplateID)

                            If sItem Is Nothing Then
                                rURLMappingInfo.URLMappingItems.Add(mItem)
                            Else
                                If sItem.Overridable Then
                                    For mIC_r As Integer = 0 To rURLMappingInfo.URLMappingItems.Count - 1
                                        If String.Compare(rURLMappingInfo.URLMappingItems.Item(mIC_r).ResolveInfo.TemplateID, mItem.ResolveInfo.TemplateID, True) = 0 Then
                                            rURLMappingInfo.URLMappingItems.RemoveAt(mIC_r)

                                            Exit For
                                        End If
                                    Next

                                    rURLMappingInfo.URLMappingItems.Add(mItem)
                                End If
                            End If
                        Next
                    End If

                    Me._Theme.Addons.DisposeInstance()
                Next

                If rURLMappingInfo.URLMapping Then
                    If rURLMappingInfo.URLMappingItems.Count = 0 Then rURLMappingInfo.URLMapping = False
                Else
                    rURLMappingInfo.URLMappingItems.Clear()
                End If

                Return rURLMappingInfo
            End Get
        End Property

        Public Property BlockRenderingID() As String
            Get
                Return Me._BlockRenderingID
            End Get
            Set(ByVal Value As String)
                Me._BlockRenderingID = Value
            End Set
        End Property

        Public ReadOnly Property RenderedService() As String
            Get
                Return Me._RenderedService
            End Get
        End Property

        Public Property MessageResult() As PGlobals.MapControls.MessageResult
            Get
                Return Me._MessageResult
            End Get
            Set(ByVal Value As PGlobals.MapControls.MessageResult)
                Me._MessageResult = Value
            End Set
        End Property

        Public ReadOnly Property ArgumentList() As Globals.ArgumentInfo.ArgumentInfoCollection
            Get
                Return Me._ArgumentInfoList
            End Get
        End Property

        Private Delegate Sub RenderServiceDelegate(ByVal RequestID As String)
        Private _RenderServiceAsyncResult As IAsyncResult

        Public ReadOnly Property RenderServiceAsyncResult() As IAsyncResult
            Get
                Return Me._RenderServiceAsyncResult
            End Get
        End Property

        Public Sub BeginRenderService(ByVal RequestID As String, ByVal DelegateAsyncCallBack As System.AsyncCallback)
            Dim RenderServiceDelegate As New RenderServiceDelegate(AddressOf Me.RenderService)

            Me._RenderServiceAsyncResult = RenderServiceDelegate.BeginInvoke(RequestID, DelegateAsyncCallBack, RenderServiceDelegate)
        End Sub

        Public Sub EndRenderService()
            CType(Me._RenderServiceAsyncResult.AsyncState, RenderServiceDelegate).EndInvoke(Me._RenderServiceAsyncResult)
        End Sub

        Public Sub RenderService(ByVal RequestID As String)
            General.AssignRequestID(RequestID)

            If String.IsNullOrEmpty(Me._ServiceID) Then
                Dim SystemMessage As String = Me._Theme.Translation.GetTranslation("TEMPLATE_IDMUSTBESET")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.TEMPLATE_IDMUSTBESET

                Throw New Exception(SystemMessage & "!")
            End If

            Me._ArgumentInfoList.Add("_sys_MessageResult", Me._MessageResult)

            Select Case Me._ServiceType
                Case Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template
                    Me._RenderedService = Me._Theme.RenderTemplate(Me._ServiceID, Me._BlockRenderingID, Me._ArgumentInfoList)
                Case Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice
                    If Me._IsAuthenticationRequired Then
                        Dim PostedExecuteParameters As New SolidDevelopment.Web.PGlobals.Helpers.WebServiceExecuteParameters
                        PostedExecuteParameters.ExecuteParametersXML = SolidDevelopment.Web.General.Context.Request.Form.Item("execParams")

                        If Not PostedExecuteParameters.PublicKey Is Nothing Then
                            Me._IsAuthenticationRequired = False

                            Dim sI_w As Theme.SettingsClass.ServicesClass.ServiceItem

                            If Me._Theme.Addons.CurrentInstance Is Nothing Then
                                sI_w = CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice, _
                                                                Me._ServiceID)
                            Else
                                sI_w = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice, _
                                                                Me._ServiceID)

                                If sI_w.Authentication AndAlso _
                                    sI_w.AuthenticationKeys.Length = 0 Then

                                    sI_w.AuthenticationKeys = _
                                        CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetAuthenticationKeys()
                                End If
                            End If

                            For Each AuthKey As String In sI_w.AuthenticationKeys
                                If Me._Theme.WebService.ReadSessionVariable(PostedExecuteParameters.PublicKey, AuthKey) Is Nothing Then
                                    Me._IsAuthenticationRequired = True

                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If Not Me._IsAuthenticationRequired Then
                        If Me._Theme.Addons.CurrentInstance Is Nothing Then
                            Me._RenderedService = CType(Me._Theme.WebService, Theme.WebServiceClass).RenderWebService(Me._ExecuteIn, Me._ServiceID, Me._ArgumentInfoList)
                        Else
                            Me._RenderedService = CType(Me._Theme.Addons.CurrentInstance.WebService, Theme.WebServiceClass).RenderWebService(Me._ExecuteIn, Me._ServiceID, Me._ArgumentInfoList)
                        End If
                    Else
                        Dim MethodResult As Object = New System.Security.SecurityException( _
                                                            SolidDevelopment.Web.Globals.SystemMessages.WEBSERVICE_AUTH _
                                                        )

                        If Me._Theme.Addons.CurrentInstance Is Nothing Then
                            Me._RenderedService = CType(Me._Theme.WebService, Theme.WebServiceClass).GenerateWebServiceXML(MethodResult)
                        Else
                            Me._RenderedService = CType(Me._Theme.Addons.CurrentInstance.WebService, Theme.WebServiceClass).GenerateWebServiceXML(MethodResult)
                        End If
                    End If
            End Select
        End Sub

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal PublicContentWebPath As String, ByVal RequestedFilePath As String)
            Me._Theme.ProvideFileStream(FileStream, PublicContentWebPath, RequestedFilePath)
        End Sub

        Public Shared Function GetAvailableThemes() As PGlobals.ThemeInfoCollection
            Dim rThemeInfoCollection As New PGlobals.ThemeInfoCollection

            Dim ThemeDI As IO.DirectoryInfo

            ' ---
            Dim ThemeID As String
            Dim ThemeTranslations As PGlobals.ThemeInfo.ThemeTranslationInfo()
            Dim ThemeAddons As PGlobals.ThemeInfo.AddonInfo() = Nothing
            ' !--

            Try
                ThemeDI = New IO.DirectoryInfo(
                                    IO.Path.Combine(
                                        Web.Configurations.PyhsicalRoot,
                                        String.Format("{0}Themes", Web.Configurations.ApplicationRoot.FileSystemImplementation)
                                    )
                                )

                Dim ThemePassword As Byte() = Nothing, ThemePasswordBase64 As String

                ' Deployment Type = Release
                For Each FI As IO.FileInfo In ThemeDI.GetFiles("*.swct")
                    ThemePasswordBase64 =
                        System.Configuration.ConfigurationManager.AppSettings.Item(
                                                String.Format("{0}_Password",
                                                    IO.Path.GetFileNameWithoutExtension(FI.Name)
                                                )
                                            )

                    Try
                        If Not String.IsNullOrEmpty(ThemePasswordBase64) Then ThemePassword = Convert.FromBase64String(ThemePasswordBase64)
                    Catch ex As Exception
                        ' Handle Wrong Base64 Strings

                        ThemePassword = Nothing
                    End Try

                    ThemeID = IO.Path.GetFileNameWithoutExtension(FI.Name)
                    ThemeTranslations = Managers.ThemeDeployment.AvailableTranslationInfos(ThemeID, ThemePassword)
                    Assembly.QueryThemeAddons(ThemeID, ThemeAddons)

                    rThemeInfoCollection.Add(
                                New PGlobals.ThemeInfo(
                                    PGlobals.ThemeBase.DeploymentTypes.Release,
                                    ThemeID,
                                    ThemeTranslations,
                                    ThemeAddons
                                )
                            )
                Next
                ' !--

                ' Deployment Type = Development
                For Each DI As IO.DirectoryInfo In ThemeDI.GetDirectories()
                    If String.Compare(DI.Name, "Dlls", True) <> 0 Then
                        ThemeID = DI.Name
                        ThemeTranslations = Managers.ThemeDeployment.AvailableTranslationInfos(ThemeID, Nothing)
                        Assembly.QueryThemeAddons(ThemeID, ThemeAddons)

                        rThemeInfoCollection.Add(
                                    New PGlobals.ThemeInfo(
                                        PGlobals.ThemeBase.DeploymentTypes.Development,
                                        ThemeID,
                                        ThemeTranslations,
                                        ThemeAddons
                                    )
                                )
                    End If
                Next
                ' !--
            Catch ex As Exception
                ' Just Handle Exceptions No Action Taken
            End Try

            Return rThemeInfoCollection
        End Function

        Public Sub ClearThemeCache()
            Me._Theme.ClearThemeCache()
        End Sub

        Private Sub PrepareServiceSettings()
            If String.IsNullOrEmpty(Me._ServiceID) Then _
                Me._ServiceID = Me._Theme.Settings.Configurations.DefaultPage

            ' Control Template Exists
            If Me._Theme.CheckTemplateExists(Me._ServiceID) Then
                If String.Compare(Me._ServiceID, Me._Theme.Settings.Configurations.AuthenticationPage, True) <> 0 Then
                    Dim sI_t As Theme.SettingsClass.ServicesClass.ServiceItem = _
                        CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                Me._ServiceID)

                    If Not sI_t Is Nothing Then
                        If sI_t.Overridable AndAlso _
                            Not Me._Theme.Addons Is Nothing Then

                            Dim sI_a As Theme.SettingsClass.ServicesClass.ServiceItem

                            For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                                Me._Theme.Addons.CreateInstance(Addon)

                                sI_a = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                    Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                                    sI_t.ID)

                                If Not sI_a Is Nothing Then
                                    If sI_a.Authentication AndAlso _
                                        sI_a.AuthenticationKeys.Length = 0 Then

                                        sI_a.AuthenticationKeys = sI_t.AuthenticationKeys
                                    End If

                                    sI_t = sI_a

                                    Exit For
                                Else
                                    Me._Theme.Addons.DisposeInstance()
                                End If
                            Next
                        End If

                        Me._ServiceType = sI_t.ServiceType
                        If sI_t.Authentication Then
                            For Each AuthKey As String In sI_t.AuthenticationKeys
                                If SolidDevelopment.Web.General.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                                    Me._IsAuthenticationRequired = True

                                    Exit For
                                End If
                            Next
                        End If
                        Me._IsWorkingAsStandAlone = sI_t.StandAlone
                        Me._ExecuteIn = sI_t.ExecuteIn
                        Me._MimeType = sI_t.MimeType
                    End If
                Else
                    Dim sI_t As Theme.SettingsClass.ServicesClass.ServiceItem = _
                        CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                Me._ServiceID)

                    If Not sI_t Is Nothing Then
                        Me._ServiceType = sI_t.ServiceType
                        Me._IsAuthenticationRequired = False
                        Me._IsWorkingAsStandAlone = sI_t.StandAlone
                        Me._ExecuteIn = sI_t.ExecuteIn
                        Me._MimeType = sI_t.MimeType

                        If sI_t.Overridable AndAlso _
                            Not Me._Theme.Addons Is Nothing Then

                            Dim sI_a As Theme.SettingsClass.ServicesClass.ServiceItem

                            For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                                Me._Theme.Addons.CreateInstance(Addon)

                                sI_a = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                    Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                                    sI_t.ID)

                                If Not sI_a Is Nothing Then
                                    sI_t = sI_a

                                    Exit For
                                Else
                                    Me._Theme.Addons.DisposeInstance()
                                End If
                            Next
                        End If
                    End If
                End If
            Else
                ' Control Webservice Exists
                Dim sI_w As Theme.SettingsClass.ServicesClass.ServiceItem = _
                        CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice, _
                                                Me._ServiceID)

                If Not sI_w Is Nothing Then
                    If sI_w.Overridable AndAlso _
                        Not Me._Theme.Addons Is Nothing Then

                        Dim sI_a As Theme.SettingsClass.ServicesClass.ServiceItem

                        For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                            Me._Theme.Addons.CreateInstance(Addon)

                            sI_a = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice, _
                                                                sI_w.ID)

                            If Not sI_a Is Nothing Then sI_w = sI_a : Exit For Else Me._Theme.Addons.DisposeInstance()
                        Next
                    End If

                    Me._ServiceType = sI_w.ServiceType
                    Me._IsAuthenticationRequired = sI_w.Authentication
                    Me._IsWorkingAsStandAlone = sI_w.StandAlone
                    Me._ExecuteIn = sI_w.ExecuteIn
                    Me._MimeType = sI_w.MimeType
                Else
                    ' If Nothing Found, Check Addons

                    If Not Me._Theme.Addons Is Nothing Then
                        ' Check Addons Templates
                        Dim sI_a As Theme.SettingsClass.ServicesClass.ServiceItem = Nothing

                        For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                            Me._Theme.Addons.CreateInstance(Addon)

                            sI_a = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                                Me._ServiceID)

                            If Not sI_a Is Nothing Then
                                If sI_a.Authentication AndAlso _
                                    sI_a.AuthenticationKeys.Length = 0 Then

                                    sI_a.AuthenticationKeys = CType(Me._Theme.Settings, Theme.SettingsClass).Services.ServiceItems.GetAuthenticationKeys()
                                End If

                                Exit For
                            Else
                                Me._Theme.Addons.DisposeInstance()
                            End If
                        Next

                        If Not sI_a Is Nothing Then
                            ' Addon Templates Found

                            Me._ServiceType = sI_a.ServiceType
                            If sI_a.Authentication Then
                                For Each AuthKey As String In sI_a.AuthenticationKeys
                                    If SolidDevelopment.Web.General.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                                        Me._IsAuthenticationRequired = True

                                        Exit For
                                    End If
                                Next
                            End If
                            Me._IsWorkingAsStandAlone = sI_a.StandAlone
                            Me._ExecuteIn = sI_a.ExecuteIn
                            Me._MimeType = sI_a.MimeType
                        Else
                            ' Check Addons WebServices

                            For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._Theme.Addons.AddonInfos
                                Me._Theme.Addons.CreateInstance(Addon)

                                sI_a = CType(Me._Theme.Addons.CurrentInstance.Settings, Theme.SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                    Theme.SettingsClass.ServicesClass.ServiceItem.ServiceTypes.webservice, _
                                                                    Me._ServiceID)

                                If Not sI_a Is Nothing Then Exit For Else Me._Theme.Addons.DisposeInstance()
                            Next

                            If Not sI_a Is Nothing Then
                                ' Addon WebService Found

                                Me._ServiceType = sI_a.ServiceType
                                Me._IsAuthenticationRequired = sI_a.Authentication
                                Me._IsWorkingAsStandAlone = sI_a.StandAlone
                                Me._ExecuteIn = sI_a.ExecuteIn
                                Me._MimeType = sI_a.MimeType
                            Else
                                Me._ServiceID = Nothing
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then Me._Theme.Dispose()

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