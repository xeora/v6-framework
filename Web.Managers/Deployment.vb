Option Strict On

Namespace SolidDevelopment.Web.Managers
    Friend Class ThemeDeployment
        Inherits DeploymentBase
        Implements IDisposable

        Private _IntegrityCheckID As String

        Private _ThemeAddons As Theme.AddonsClass = Nothing
        Private _ThemeSettings As Theme.SettingsClass = Nothing
        Private _ThemeTranslation As Theme.TranslationClass = Nothing
        Private _ThemeWebService As Theme.WebServiceClass

        Private Sub New(ByVal ThemeID As String, ByVal ThemePassword As Byte())
            MyBase.New(PGlobals.ThemeBase.DeploymentStyles.Theme, Nothing, ThemeID, Nothing, ThemePassword)

            ' To Set Authentication Value Do Blind Read For Configuration File
            MyBase.ProvideConfigurationContent()
        End Sub

        Public Sub New(ByVal ThemeID As String, ByVal ThemeTranslationID As String, ByVal ThemePassword As Byte())
            MyBase.New(PGlobals.ThemeBase.DeploymentStyles.Theme, Nothing, ThemeID, ThemeTranslationID, ThemePassword)

            Me._IntegrityCheckID = _
                String.Format("ByPass{0}_{1}_{2}_{3}_{4}", _
                                    SolidDevelopment.Web.Configurations.ApplicationRoot.BrowserSystemImplementation.Replace("/"c, "_"c), _
                                    MyBase.ParentID, _
                                    MyBase.CurrentID, _
                                    MyBase.DeploymentStyle, _
                                    MyBase.DeploymentType)

            Dim QuickIntegrityCheck As Boolean = False

            Boolean.TryParse( _
                CType(SolidDevelopment.Web.General.Context.Application.Item(Me._IntegrityCheckID), String), _
                QuickIntegrityCheck)

            ' This Quick Integrity Check is for bypassing Theme Public Content Folder Checks
            Me.CheckIntegrity(QuickIntegrityCheck)
        End Sub

        Private Sub CheckIntegrity(ByVal QuickIntegrityCheck As Boolean)
            Me._ThemeAddons = New Theme.AddonsClass(MyBase.CurrentID, MyBase.CurrentTranslationID)

            Select Case MyBase.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    If Not QuickIntegrityCheck Then
                        ' Control Theme Folder Language Folder, Template Folder
                        If Not IO.Directory.Exists(MyBase.TranslationsPath) Then Throw New Exception(String.Format("Theme {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                        If Not IO.Directory.Exists(MyBase.TemplatesPath) Then Throw New Exception(String.Format("Theme {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                        ' !--
                    End If

                    Me._ThemeSettings = New Theme.SettingsClass( _
                                            MyBase.ProvideConfigurationContent() _
                                        )

                    If MyBase.CurrentTranslationID Is Nothing Then
                        Me._ThemeTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            Me._ThemeSettings.Configurations.DefaultTranslation _
                                                        ) _
                                                    )
                    Else
                        Me._ThemeTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            MyBase.CurrentTranslationID _
                                                        ) _
                                                    )
                    End If

                    Me._ThemeWebService = New Theme.WebServiceClass(Nothing, Nothing)

                    If Not QuickIntegrityCheck Then
                        ' Control Theme Folder Language Folder, Template Folder
                        Dim ThemeTranslationDirectory As New IO.DirectoryInfo(MyBase.TranslationsPath)

                        For Each ttDI As IO.DirectoryInfo In ThemeTranslationDirectory.GetDirectories()
                            If Not IO.Directory.Exists( _
                                MyBase.PublicContentsPath(ttDI.Name)) Then _
                                    Throw New Exception(String.Format("Theme {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                        Next
                        ' !--

                        ' -- Control Those System Essential Files are Exists! --
                        Dim SystemMessage As String = Nothing

                        Dim ControlsMapXML As String = IO.Path.Combine(MyBase.TemplatesPath, "ControlsMap.xml")
                        Dim ConfigurationXML As String = IO.Path.Combine(MyBase.TemplatesPath, "Configuration.xml")

                        If Not IO.File.Exists(ConfigurationXML) Then
                            SystemMessage = Me._ThemeTranslation.GetTranslation("CONFIGURATIONNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                            Throw New Exception(SystemMessage & "!")
                        End If

                        If Not IO.File.Exists(ControlsMapXML) Then
                            SystemMessage = Me._ThemeTranslation.GetTranslation("CONTROLSMAPNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND

                            Throw New Exception(SystemMessage & "!")
                        End If
                        ' !--
                    End If
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Me._ThemeSettings = New Theme.SettingsClass( _
                                            MyBase.ProvideConfigurationContent() _
                                        )

                    If Me.CurrentTranslationID Is Nothing Then
                        Me._ThemeTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            Me._ThemeSettings.Configurations.DefaultTranslation _
                                                        ) _
                                                    )
                    Else
                        Me._ThemeTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            MyBase.CurrentTranslationID _
                                                        ) _
                                                    )
                    End If

                    Me._ThemeWebService = New Theme.WebServiceClass(Nothing, Nothing)

                    If Not QuickIntegrityCheck Then
                        ' -- Control Those System Essential Files are Exists! --
                        Dim SystemMessage As String = Nothing

                        Dim ControlsMapFileInfo As Decompiler.swctFileInfo = MyBase.SWCTDecompiler.GetswctFileInfo(MyBase.TemplatesRegistration, "ControlsMap.xml")
                        Dim ConfigurationFileInfo As Decompiler.swctFileInfo = MyBase.SWCTDecompiler.GetswctFileInfo(MyBase.TemplatesRegistration, "Configuration.xml")

                        If ConfigurationFileInfo.Index = -1 Then
                            SystemMessage = Me._ThemeTranslation.GetTranslation("CONFIGURATIONNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                            Throw New Exception(SystemMessage & "!")
                        End If

                        If ControlsMapFileInfo.Index = -1 Then
                            SystemMessage = Me._ThemeTranslation.GetTranslation("CONTROLSMAPNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND

                            Throw New Exception(SystemMessage & "!")
                        End If
                        ' !--
                    End If
            End Select

            If SolidDevelopment.Web.General.Context.Application.Contents.Item(Me._IntegrityCheckID) Is Nothing Then
                SolidDevelopment.Web.General.Context.Application.Contents.Add(Me._IntegrityCheckID, True)
            End If
        End Sub

        Public Overrides ReadOnly Property Settings() As PGlobals.ITheme.ISettings
            Get
                Return Me._ThemeSettings
            End Get
        End Property

        Public Overrides ReadOnly Property Translation() As PGlobals.ITheme.ITranslation
            Get
                Return Me._ThemeTranslation
            End Get
        End Property

        Public Overrides ReadOnly Property WebService() As PGlobals.ITheme.IWebService
            Get
                Return Me._ThemeWebService
            End Get
        End Property

        Public Overrides ReadOnly Property Addons() As PGlobals.ITheme.IAddons
            Get
                Return Me._ThemeAddons
            End Get
        End Property

        Public Overrides Function ProvideTemplateContent(ByVal TemplateID As String) As String
            ' -- Check is template file is exists
            If Not MyBase.CheckTemplateExists(TemplateID) Then
                Dim SystemMessage As String = Me._ThemeTranslation.GetTranslation("TEMPLATE_NOFOUND")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.TEMPLATE_NOFOUND

                Throw New Exception(String.Format(SystemMessage & "!", TemplateID))
            End If
            ' !--

            Return MyBase.ProvideTemplateContent(TemplateID)
        End Function

        Public Overrides Sub ProvideFileStream(ByRef ThemeFileStream As IO.Stream, ByVal PublicContentWebPath As String, ByVal RequestedFilePath As String)
            If Not String.IsNullOrEmpty(RequestedFilePath) AndAlso _
                ( _
                    RequestedFilePath.Chars(0) = "/"c OrElse _
                    RequestedFilePath.Chars(0) = "\"c _
                ) Then

                RequestedFilePath = RequestedFilePath.Substring(1)
            End If

            Dim RequestedPublicContentAddonID As String = Nothing
            Dim RequestedPublicContentWebPath As String = _
                                        PublicContentWebPath.Substring( _
                                                PublicContentWebPath.IndexOf( _
                                                    String.Format("{0}_{1}", MyBase.CurrentID, MyBase.CurrentTranslationID) _
                                                ) _
                                            )

            ' Check Requested Public Content is an Addon Public Content
            Dim ThemePublicContentWebPath As String = _
                String.Format("{0}_{1}", MyBase.CurrentID, MyBase.CurrentTranslationID)

            If ThemePublicContentWebPath.Length < RequestedPublicContentWebPath.Length Then
                ' skip "ThemeID_ThemeTranslationID_" and read "AddonID"

                If RequestedPublicContentWebPath.Chars(ThemePublicContentWebPath.Length).CompareTo("/"c) = 0 OrElse _
                    RequestedPublicContentWebPath.Chars(ThemePublicContentWebPath.Length).CompareTo("\"c) = 0 Then

                    RequestedFilePath = IO.Path.Combine( _
                                            RequestedPublicContentWebPath.Substring(ThemePublicContentWebPath.Length + 1), _
                                            RequestedFilePath)
                Else
                    RequestedPublicContentAddonID = RequestedPublicContentWebPath.Substring(ThemePublicContentWebPath.Length + 1)

                    If RequestedPublicContentAddonID.IndexOf("/"c) > -1 Then
                        RequestedFilePath = IO.Path.Combine( _
                                                RequestedPublicContentAddonID.Substring(RequestedPublicContentAddonID.IndexOf("/"c) + 1), _
                                                RequestedFilePath)
                        RequestedPublicContentAddonID = RequestedPublicContentAddonID.Substring(0, RequestedPublicContentAddonID.IndexOf("/"c))
                    ElseIf RequestedPublicContentAddonID.IndexOf("\"c) > -1 Then
                        RequestedFilePath = IO.Path.Combine( _
                                                RequestedPublicContentAddonID.Substring(RequestedPublicContentAddonID.IndexOf("\"c) + 1), _
                                                RequestedFilePath)
                        RequestedPublicContentAddonID = RequestedPublicContentAddonID.Substring(0, RequestedPublicContentAddonID.IndexOf("\"c))
                    End If
                End If
            End If

            Select Case MyBase.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    If RequestedPublicContentAddonID Is Nothing Then
TRYPARENT_Development:
                        ' Theme Public Content Search
                        Dim RequestedFileFullPath As String = _
                                IO.Path.Combine( _
                                    MyBase.PublicContentsPath(Me._ThemeTranslation.CurrentTranslationID), _
                                    RequestedFilePath)

                        If IO.File.Exists(RequestedFileFullPath) Then
                            Try
                                ThemeFileStream = New IO.FileStream(RequestedFileFullPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                            Catch ex As Exception
                                ' Theme File Reading Error Exception Handling. Take No Action

                                ThemeFileStream = Nothing
                            End Try
                        Else
                            ThemeFileStream = Nothing
                        End If
                    Else
                        ' Addon Public Content Search
                        For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._ThemeAddons.AddonInfos
                            If String.Compare(RequestedPublicContentAddonID, Addon.AddonID, True) = 0 Then
                                Dim AddonDeployment As AddonDeployment = _
                                        New AddonDeployment(Me.CurrentID, Addon.AddonID, Me.CurrentTranslationID, Addon.AddonPassword)
                                AddonDeployment.ProvideFileStream(ThemeFileStream, Nothing, RequestedFilePath)

                                Exit For
                            End If
                        Next

                        If ThemeFileStream Is Nothing Then GoTo TRYPARENT_Development
                    End If
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    ' RequestedFilePath = RequestedFilePath.Replace("/"c, IO.Path.DirectorySeparatorChar)
                    ' RequestedFilePath = RequestedFilePath.Replace("\"c, IO.Path.DirectorySeparatorChar)

                    If RequestedPublicContentAddonID Is Nothing Then
TRYPARENT_Release:
                        Dim swctFileInfo As Decompiler.swctFileInfo = _
                                                    MyBase.SWCTDecompiler.GetswctFileInfo( _
                                                            IO.Path.Combine( _
                                                                MyBase.PublicContentsRegistration(Me._ThemeTranslation.CurrentTranslationID), _
                                                                RequestedFilePath.Replace( _
                                                                    IO.Path.GetFileName(RequestedFilePath), _
                                                                    Nothing _
                                                                ) _
                                                            ), _
                                                            IO.Path.GetFileName(RequestedFilePath) _
                                                        )

                        If swctFileInfo.Index > -1 Then
                            ThemeFileStream = New IO.MemoryStream()

                            MyBase.SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(ThemeFileStream, IO.Stream))

                            If MyBase.SWCTDecompiler.RequestStatus = Decompiler.RequestResults.PasswordError Then _
                                Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                        Else
                            ' So Theme is not contain any File named like that

                            ThemeFileStream = Nothing
                        End If
                    Else
                        ' Addon Public Content Search
                        For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._ThemeAddons.AddonInfos
                            If String.Compare(RequestedPublicContentAddonID, Addon.AddonID, True) = 0 Then
                                Dim AddonDeployment As AddonDeployment = _
                                        New AddonDeployment(Me.CurrentID, Addon.AddonID, Me.CurrentTranslationID, Addon.AddonPassword)
                                AddonDeployment.ProvideFileStream(ThemeFileStream, Nothing, RequestedFilePath)

                                Exit For
                            End If
                        Next

                        If ThemeFileStream Is Nothing Then GoTo TRYPARENT_Release
                    End If
            End Select
        End Sub

        Public Shared Function AvailableTranslationInfos(ByVal ThemeID As String, ByVal ThemePassword As Byte()) As PGlobals.ThemeInfo.ThemeTranslationInfo()
            Dim rThemeTranslationInfos As New Generic.List(Of PGlobals.ThemeInfo.ThemeTranslationInfo)

            Dim tThemeDeployment1 As New ThemeDeployment(ThemeID, ThemePassword), tThemeDeployment2 As ThemeDeployment

            Select Case tThemeDeployment1.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Select Case tThemeDeployment1.SWCTDecompiler.RequestStatus
                        Case Decompiler.RequestResults.Authenticated
                            For Each sFI As Decompiler.swctFileInfo In tThemeDeployment1.SWCTDecompiler.swctFilesList
                                If sFI.RegistrationPath.IndexOf(tThemeDeployment1.TranslationsRegistration) > -1 Then
                                    tThemeDeployment2 = New ThemeDeployment(ThemeID, IO.Path.GetFileNameWithoutExtension(sFI.FileName), ThemePassword)

                                    rThemeTranslationInfos.Add(tThemeDeployment2.Translation.CurrentTranslationInfo)

                                    tThemeDeployment2.Dispose()
                                End If
                            Next
                        Case Decompiler.RequestResults.PasswordError
                            Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                    End Select

                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    If IO.Directory.Exists(tThemeDeployment1.TranslationsPath) Then
                        Dim TranslationsDI As New IO.DirectoryInfo(tThemeDeployment1.TranslationsPath)

                        For Each TFI As IO.FileInfo In TranslationsDI.GetFiles()
                            tThemeDeployment2 = New ThemeDeployment(ThemeID, IO.Path.GetFileNameWithoutExtension(TFI.Name), ThemePassword)

                            rThemeTranslationInfos.Add(tThemeDeployment2.Translation.CurrentTranslationInfo)

                            tThemeDeployment2.Dispose()
                        Next
                    End If

            End Select

            Return rThemeTranslationInfos.ToArray()
        End Function

        Public Shared Sub ExtractApplication(ByVal ThemeID As String, ByVal ThemePassword As Byte(), ByVal ExtractLocation As String)
            Dim ThemeDeployment As New ThemeDeployment(ThemeID, ThemePassword)
            Dim ApplicationPath As String = _
                ThemeDeployment.ApplicationPath
            ThemeDeployment.Dispose()

            Dim DI As New IO.DirectoryInfo(ApplicationPath)

            For Each fI As IO.FileInfo In DI.GetFiles()
                If Not IO.File.Exists( _
                    IO.Path.Combine(ExtractLocation, fI.Name)) Then

                    Try
                        fI.CopyTo( _
                            IO.Path.Combine(ExtractLocation, fI.Name))
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not Me._ThemeAddons Is Nothing Then Me._ThemeAddons.Dispose()
                If Not Me._ThemeSettings Is Nothing Then Me._ThemeSettings.Dispose()
                If Not Me._ThemeTranslation Is Nothing Then Me._ThemeTranslation.Dispose()
            End If

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overrides Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

    Friend Class AddonDeployment
        Inherits DeploymentBase
        Implements IDisposable

        Private _AddonSettings As Theme.SettingsClass = Nothing
        Private _AddonTranslation As Theme.TranslationClass = Nothing
        Private _AddonWebService As Theme.WebServiceClass = Nothing

        Private Sub New(ByVal ThemeID As String, ByVal AddonID As String, ByVal AddonPassword As Byte())
            MyBase.New(PGlobals.ThemeBase.DeploymentStyles.Addon, ThemeID, AddonID, Nothing, AddonPassword)

            ' To Set Authentication Value Do Blind Read For Configuration File
            MyBase.ProvideConfigurationContent()
        End Sub

        Public Sub New(ByVal ThemeID As String, ByVal AddonID As String, ByVal AddonTranslationID As String, ByVal AddonPassword As Byte())
            MyBase.New(PGlobals.ThemeBase.DeploymentStyles.Addon, ThemeID, AddonID, AddonTranslationID, AddonPassword)

            Me.CheckIntegrity()
        End Sub

        Private Sub CheckIntegrity()
            Select Case MyBase.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    ' Control Theme Folder Language Folder, Template Folder
                    If Not IO.Directory.Exists(MyBase.TranslationsPath) Then Throw New Exception(String.Format("Addon {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                    If Not IO.Directory.Exists(MyBase.TemplatesPath) Then Throw New Exception(String.Format("Addon {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                    ' !--

                    Me._AddonSettings = New Theme.SettingsClass( _
                                            MyBase.ProvideConfigurationContent() _
                                        )

                    If MyBase.CurrentTranslationID Is Nothing Then
                        Me._AddonTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            Me._AddonSettings.Configurations.DefaultTranslation _
                                                        ) _
                                                    )
                    Else
                        Me._AddonTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            MyBase.CurrentTranslationID _
                                                        ) _
                                                    )
                    End If

                    Me._AddonWebService = New Theme.WebServiceClass(MyBase.ParentID, MyBase.CurrentID)

                    ' Control Addon Folder Language Folder, Template Folder
                    Dim CurrentAddonContentPath As String = MyBase.PublicContentsPath(MyBase.CurrentTranslationID)

                    If Not IO.Directory.Exists(CurrentAddonContentPath) Then _
                                Throw New Exception(String.Format("Addon {0}", Globals.SystemMessages.PATH_WRONGSTRUCTURE))
                    ' !--

                    ' -- Control Those System Essential Files are Exists! --
                    Dim SystemMessage As String = Nothing

                    Dim ControlsMapXML As String = IO.Path.Combine(MyBase.TemplatesPath, "ControlsMap.xml")
                    Dim ConfigurationXML As String = IO.Path.Combine(MyBase.TemplatesPath, "Configuration.xml")

                    If Not IO.File.Exists(ConfigurationXML) Then
                        SystemMessage = Me._AddonTranslation.GetTranslation("CONFIGURATIONNOTFOUND")

                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                        Throw New Exception(SystemMessage & "!")
                    End If

                    If Not IO.File.Exists(ControlsMapXML) Then
                        SystemMessage = Me._AddonTranslation.GetTranslation("CONTROLSMAPNOTFOUND")

                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND

                        Throw New Exception(SystemMessage & "!")
                    End If
                    ' !--
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Me._AddonSettings = New Theme.SettingsClass( _
                                            MyBase.ProvideConfigurationContent() _
                                        )

                    If Me.CurrentTranslationID Is Nothing Then
                        Me._AddonTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            Me._AddonSettings.Configurations.DefaultTranslation _
                                                        ) _
                                                    )
                    Else
                        Me._AddonTranslation = New Theme.TranslationClass( _
                                                        MyBase.ProvideTranslationContent( _
                                                            MyBase.CurrentTranslationID _
                                                        ) _
                                                    )
                    End If

                    Me._AddonWebService = New Theme.WebServiceClass(MyBase.ParentID, MyBase.CurrentID)

                    ' -- Control Those System Essential Files are Exists! --
                    Dim SystemMessage As String = Nothing

                    Dim ControlsMapFileInfo As Decompiler.swctFileInfo = MyBase.SWCTDecompiler.GetswctFileInfo(MyBase.TemplatesRegistration, "ControlsMap.xml")
                    Dim ConfigurationFileInfo As Decompiler.swctFileInfo = MyBase.SWCTDecompiler.GetswctFileInfo(MyBase.TemplatesRegistration, "Configuration.xml")

                    If ConfigurationFileInfo.Index = -1 Then
                        SystemMessage = Me._AddonTranslation.GetTranslation("CONFIGURATIONNOTFOUND")

                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                        Throw New Exception(SystemMessage & "!")
                    End If

                    If ControlsMapFileInfo.Index = -1 Then
                        SystemMessage = Me._AddonTranslation.GetTranslation("CONTROLSMAPNOTFOUND")

                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND

                        Throw New Exception(SystemMessage & "!")
                    End If
                    ' !--
            End Select
        End Sub

        Public Overrides ReadOnly Property Settings() As PGlobals.ITheme.ISettings
            Get
                Return Me._AddonSettings
            End Get
        End Property

        Public Overrides ReadOnly Property Translation() As PGlobals.ITheme.ITranslation
            Get
                Return Me._AddonTranslation
            End Get
        End Property

        Public Overrides ReadOnly Property WebService() As PGlobals.ITheme.IWebService
            Get
                Return Me._AddonWebService
            End Get
        End Property

        Public Overrides ReadOnly Property Addons() As PGlobals.ITheme.IAddons
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides Function ProvideTemplateContent(ByVal TemplateID As String) As String
            ' -- Check is template file is exists
            If Not MyBase.CheckTemplateExists(TemplateID) Then
                Dim SystemMessage As String = Me._AddonTranslation.GetTranslation("TEMPLATE_NOFOUND")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.TEMPLATE_NOFOUND

                Throw New IO.FileNotFoundException(String.Format(SystemMessage & "!", TemplateID))
            End If
            ' !--

            Return MyBase.ProvideTemplateContent(TemplateID)
        End Function

        Public Overrides Sub ProvideFileStream(ByRef AddonFileStream As IO.Stream, ByVal PublicContentWebPath As String, ByVal RequestedFilePath As String)
            If Not String.IsNullOrEmpty(RequestedFilePath) AndAlso _
                ( _
                    RequestedFilePath.Chars(0) = "/"c OrElse _
                    RequestedFilePath.Chars(0) = "\"c _
                ) Then

                RequestedFilePath = RequestedFilePath.Substring(1)
            End If

            Select Case MyBase.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    Dim AddonFileFullPath As String = _
                        IO.Path.Combine( _
                            MyBase.PublicContentsPath(Me._AddonTranslation.CurrentTranslationID), _
                            RequestedFilePath)

                    If IO.File.Exists(AddonFileFullPath) Then
                        Try
                            AddonFileStream = New IO.FileStream(AddonFileFullPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        Catch ex As Exception
                            ' Addon File Reading Error Exception Handling. Take No Action

                            AddonFileStream = Nothing
                        End Try
                    Else
                        AddonFileStream = Nothing
                    End If
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    ' RequestedFilePath = RequestedFilePath.Replace("/"c, IO.Path.DirectorySeparatorChar)
                    ' RequestedFilePath = RequestedFilePath.Replace("\"c, IO.Path.DirectorySeparatorChar)

                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            MyBase.SWCTDecompiler.GetswctFileInfo( _
                                    IO.Path.Combine( _
                                        MyBase.PublicContentsRegistration(Me._AddonTranslation.CurrentTranslationID), _
                                        RequestedFilePath.Replace( _
                                            IO.Path.GetFileName(RequestedFilePath), _
                                            Nothing _
                                        ) _
                                    ), _
                                    IO.Path.GetFileName(RequestedFilePath) _
                                )

                    If swctFileInfo.Index > -1 Then
                        AddonFileStream = New IO.MemoryStream()

                        MyBase.SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(AddonFileStream, IO.Stream))

                        If MyBase.SWCTDecompiler.RequestStatus = Decompiler.RequestResults.PasswordError Then _
                            Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                    Else
                        ' So Addon is not contain any File named like that

                        AddonFileStream = Nothing
                    End If
            End Select
        End Sub

        Public Shared Function AvailableTranslationInfos(ByVal ThemeID As String, ByVal AddonID As String, ByVal AddonPassword As Byte()) As PGlobals.ThemeInfo.ThemeTranslationInfo()
            Dim rAddonTranslationInfos As New Generic.List(Of PGlobals.ThemeInfo.ThemeTranslationInfo)

            Dim tAddonDeployment1 As New AddonDeployment(ThemeID, AddonID, AddonPassword), tAddonDeployment2 As AddonDeployment

            Select Case tAddonDeployment1.DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Select Case tAddonDeployment1.SWCTDecompiler.RequestStatus
                        Case Decompiler.RequestResults.Authenticated
                            For Each sFI As Decompiler.swctFileInfo In tAddonDeployment1.SWCTDecompiler.swctFilesList
                                If sFI.RegistrationPath.IndexOf(tAddonDeployment1.TranslationsRegistration) > -1 Then
                                    tAddonDeployment2 = New AddonDeployment(ThemeID, AddonID, IO.Path.GetFileNameWithoutExtension(sFI.FileName), AddonPassword)

                                    rAddonTranslationInfos.Add(tAddonDeployment2.Translation.CurrentTranslationInfo)

                                    tAddonDeployment2.Dispose()
                                End If
                            Next
                        Case Decompiler.RequestResults.PasswordError
                            Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                    End Select

                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    If IO.Directory.Exists(tAddonDeployment1.TranslationsPath) Then
                        Dim TranslationsDI As New IO.DirectoryInfo(tAddonDeployment1.TranslationsPath)

                        For Each TFI As IO.FileInfo In TranslationsDI.GetFiles()
                            tAddonDeployment2 = New AddonDeployment(ThemeID, AddonID, IO.Path.GetFileNameWithoutExtension(TFI.Name), AddonPassword)

                            rAddonTranslationInfos.Add(tAddonDeployment2.Translation.CurrentTranslationInfo)

                            tAddonDeployment2.Dispose()
                        Next
                    End If

            End Select

            Return rAddonTranslationInfos.ToArray()
        End Function

        Public Shared Sub ExtractApplication(ByVal ThemeID As String, ByVal AddonID As String, ByVal AddonPassword As Byte(), ByVal ExtractLocation As String)
            Dim AddonDeployment As New AddonDeployment(ThemeID, AddonID, AddonPassword)
            Dim ApplicationPath As String = _
                AddonDeployment.ApplicationPath
            AddonDeployment.Dispose()

            Dim DI As New IO.DirectoryInfo(ApplicationPath)

            For Each fI As IO.FileInfo In DI.GetFiles()
                If Not IO.File.Exists( _
                    IO.Path.Combine(ExtractLocation, fI.Name)) Then

                    Try
                        fI.CopyTo( _
                            IO.Path.Combine(ExtractLocation, fI.Name))
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not Me._AddonSettings Is Nothing Then Me._AddonSettings.Dispose()
                If Not Me._AddonTranslation Is Nothing Then Me._AddonTranslation.Dispose()
            End If

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overrides Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

    Friend MustInherit Class DeploymentBase
        Private _DeploymentType As PGlobals.ThemeBase.DeploymentTypes
        Private _SWCTDecompiler As Decompiler

        Private _DeploymentStyle As PGlobals.ThemeBase.DeploymentStyles
        Private _ParentID As String
        Private _CurrentID As String
        Private _CurrentTranslationID As String

        Private _Location As String
        Private Shared _DeploymentWorkingRoot As String

        Public MustOverride Sub Dispose()
        Public MustOverride ReadOnly Property Settings() As PGlobals.ITheme.ISettings
        Public MustOverride ReadOnly Property Translation() As PGlobals.ITheme.ITranslation
        Public MustOverride ReadOnly Property WebService() As PGlobals.ITheme.IWebService
        Public MustOverride ReadOnly Property Addons() As PGlobals.ITheme.IAddons
        Public MustOverride Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal PublicContentWebPath As String, ByVal RequestedFilePath As String)

        Public Sub New(ByVal DeploymentStyle As PGlobals.ThemeBase.DeploymentStyles, ByVal ParentID As String, ByVal ID As String, ByVal TranslationID As String, ByVal Password As Byte())
            Me._DeploymentStyle = DeploymentStyle
            Me._ParentID = ParentID
            Me._CurrentID = ID
            Me._CurrentTranslationID = TranslationID

            If Me._CurrentID Is Nothing OrElse _
                Me._CurrentID.Trim().Length = 0 Then

                Throw New Exception( _
                    String.Format("{0}{1}", Me._DeploymentStyle, Globals.SystemMessages.IDMUSTBESET) _
                )
            End If

            Me._Location = IO.Path.Combine(Configurations.PyhsicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)

            Dim SystemFileID As String = Me._CurrentID
            Select Case Me._DeploymentStyle
                Case PGlobals.ThemeBase.DeploymentStyles.Addon
                    Me._Location = IO.Path.Combine(Me._Location, String.Format("Themes{2}{0}{2}Addons{2}{1}", Me._ParentID, Me._CurrentID, IO.Path.DirectorySeparatorChar))
                    DeploymentBase._DeploymentWorkingRoot = Me._Location
                    Me._Location = IO.Path.Combine(Me._Location, "Content.swct")

                    SystemFileID = String.Format("{0}_{1}", SystemFileID, Me._CurrentID)
                Case PGlobals.ThemeBase.DeploymentStyles.Theme
                    Me._Location = IO.Path.Combine(Me._Location, "Themes")
                    DeploymentBase._DeploymentWorkingRoot = Me._Location
                    Me._Location = IO.Path.Combine(Me._Location, String.Format("{0}.swct", Me._CurrentID))
            End Select

            If IO.File.Exists(Me._Location) Then
                Me._DeploymentType = PGlobals.ThemeBase.DeploymentTypes.Release

                Me._SWCTDecompiler = New Decompiler(Me._Location, Password)
            Else
                Me._Location = Me.GetLocationPath()

                Me._DeploymentType = PGlobals.ThemeBase.DeploymentTypes.Development
            End If
        End Sub

        Private Function GetLocationPath() As String
            Dim LocationPath As String = Nothing

            Select Case Me._DeploymentStyle
                Case PGlobals.ThemeBase.DeploymentStyles.Addon
                    LocationPath = IO.Path.Combine(Configurations.PyhsicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)
                    LocationPath = IO.Path.Combine(LocationPath, String.Format("Themes{2}{0}{2}Addons{2}{1}{2}Content", Me._ParentID, Me._CurrentID, IO.Path.DirectorySeparatorChar))

                    If Not IO.Directory.Exists(LocationPath) Then Throw New Exception( _
                                                                    String.Format("{0} {1}", Me._DeploymentStyle, Globals.SystemMessages.PATH_NOTEXISTS) _
                                                                )
                Case PGlobals.ThemeBase.DeploymentStyles.Theme
                    LocationPath = IO.Path.Combine(Configurations.PyhsicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)
                    LocationPath = IO.Path.Combine(LocationPath, String.Format("Themes{1}{0}", Me._CurrentID, IO.Path.DirectorySeparatorChar))

                    If Not IO.Directory.Exists(LocationPath) Then
                        Me._CurrentID = SolidDevelopment.Web.Configurations.DefaultTheme

                        LocationPath = IO.Path.Combine(Configurations.PyhsicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)
                        LocationPath = IO.Path.Combine(LocationPath, String.Format("Themes{1}{0}", Me._CurrentID, IO.Path.DirectorySeparatorChar))

                        If Not IO.Directory.Exists(LocationPath) Then Throw New Exception( _
                                                                    String.Format("{0} {1}", Me._DeploymentStyle, Globals.SystemMessages.PATH_NOTEXISTS) _
                                                                )

                        SolidDevelopment.Web.General.CurrentThemeID = Me._CurrentID
                    End If
            End Select

            Return LocationPath
        End Function

        Public ReadOnly Property DeploymentType() As PGlobals.ThemeBase.DeploymentTypes
            Get
                Return Me._DeploymentType
            End Get
        End Property

        Public ReadOnly Property DeploymentStyle() As PGlobals.ThemeBase.DeploymentStyles
            Get
                Return Me._DeploymentStyle
            End Get
        End Property

        Public ReadOnly Property ParentID() As String
            Get
                Return Me._ParentID
            End Get
        End Property

        Public ReadOnly Property CurrentID() As String
            Get
                Return Me._CurrentID
            End Get
        End Property

        Public ReadOnly Property CurrentTranslationID() As String
            Get
                Return Me._CurrentTranslationID
            End Get
        End Property

        Protected ReadOnly Property SWCTDecompiler() As Decompiler
            Get
                Return Me._SWCTDecompiler
            End Get
        End Property

        Protected Shared ReadOnly Property DeploymentWorkingRoot() As String
            Get
                Return DeploymentBase._DeploymentWorkingRoot
            End Get
        End Property

#Region " Development Deployment Type Related Path Properties "
        Protected ReadOnly Property ApplicationPath() As String
            Get
                Return IO.Path.Combine(DeploymentBase._DeploymentWorkingRoot, "Dlls")
            End Get
        End Property

        Protected ReadOnly Property TemplatesPath() As String
            Get
                Return IO.Path.Combine(Me._Location, "Templates")
            End Get
        End Property

        Protected ReadOnly Property PublicContentsPath(ByVal TranslationLanguage As String) As String
            Get
                Return IO.Path.Combine(Me._Location, String.Format("PublicContents{1}{0}", TranslationLanguage, IO.Path.DirectorySeparatorChar))
            End Get
        End Property

        Protected ReadOnly Property TranslationsPath() As String
            Get
                Return IO.Path.Combine(Me._Location, "Translations")
            End Get
        End Property
#End Region

#Region " Release Deployment Type Related Path Properties "
        Protected ReadOnly Property TemplatesRegistration() As String
            Get
                Return "\Templates\"
            End Get
        End Property

        Protected ReadOnly Property PublicContentsRegistration(ByVal TranslationLanguage As String) As String
            Get
                Return String.Format("\PublicContents\{0}\", TranslationLanguage)
            End Get
        End Property

        Protected ReadOnly Property TranslationsRegistration() As String
            Get
                Return "\Translations\"
            End Get
        End Property
#End Region

        Private Function DetectEncoding(ByRef inStream As IO.Stream) As System.Text.Encoding
            Dim rEncoding As System.Text.Encoding = System.Text.Encoding.UTF8

            inStream.Seek(0, IO.SeekOrigin.Begin)

            Dim bC As Integer, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 4), Byte())

            bC = inStream.Read(buffer, 0, buffer.Length)

            If bC > 0 Then
                If bC >= 2 AndAlso _
                    buffer(0) = 254 AndAlso _
                    buffer(1) = 255 Then

                    rEncoding = New System.Text.UnicodeEncoding(True, True)

                    inStream.Seek(2, IO.SeekOrigin.Begin)
                ElseIf bC >= 2 AndAlso _
                        buffer(0) = 255 AndAlso _
                        buffer(1) = 254 Then

                    If bC = 4 AndAlso _
                        buffer(2) = 0 AndAlso _
                        buffer(3) = 0 Then

                        rEncoding = New System.Text.UTF32Encoding(False, True)

                        inStream.Seek(4, IO.SeekOrigin.Begin)
                    Else
                        rEncoding = New System.Text.UnicodeEncoding(False, True)

                        inStream.Seek(2, IO.SeekOrigin.Begin)
                    End If
                ElseIf bC >= 3 AndAlso _
                    buffer(0) = 239 AndAlso _
                    buffer(1) = 187 AndAlso _
                    buffer(2) = 191 Then

                    rEncoding = New System.Text.UTF8Encoding()

                    inStream.Seek(3, IO.SeekOrigin.Begin)
                ElseIf bC = 4 AndAlso _
                    buffer(0) = 0 AndAlso _
                    buffer(1) = 0 AndAlso _
                    buffer(2) = 254 AndAlso _
                    buffer(3) = 255 Then

                    rEncoding = New System.Text.UTF32Encoding(True, True)

                    inStream.Seek(4, IO.SeekOrigin.Begin)
                Else
                    inStream.Seek(0, IO.SeekOrigin.Begin)
                End If
            End If

            Return rEncoding
        End Function

        Public Function CheckTemplateExists(ByVal TemplateID As String) As Boolean
            Dim rBoolean As Boolean = False

            Select Case Me._DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    rBoolean = IO.File.Exists( _
                                    IO.Path.Combine( _
                                        Me.TemplatesPath, String.Format("{0}.htm", TemplateID) _
                                    ) _
                                )
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            Me._SWCTDecompiler.GetswctFileInfo( _
                                    Me.TemplatesRegistration, String.Format("{0}.htm", TemplateID) _
                                )

                    rBoolean = swctFileInfo.Index > -1
            End Select

            Return rBoolean
        End Function

        Public Overridable Function ProvideTemplateContent(ByVal TemplateID As String) As String
            Dim rTemplateContent As New System.Text.StringBuilder

            ' Function Variables
            Dim TemplateFile As String
            ' !--

            Select Case Me._DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    TemplateFile = _
                        IO.Path.Combine( _
                            Me.TemplatesPath, String.Format("{0}.htm", TemplateID))

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As System.Text.Encoding

                    Try
                        fS = New IO.FileStream(TemplateFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then rTemplateContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0
                    Catch ex As Exception
                        rTemplateContent = Nothing
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            Me._SWCTDecompiler.GetswctFileInfo( _
                                    Me.TemplatesRegistration, String.Format("{0}.htm", TemplateID) _
                                )

                    If swctFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Me._SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case Me._SWCTDecompiler.RequestStatus
                            Case Decompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rTemplateContent.Append(sR.ReadToEnd())

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case Decompiler.RequestResults.PasswordError
                                Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Dim rTemplateContent_s As String = Nothing

            If Not rTemplateContent Is Nothing Then rTemplateContent_s = rTemplateContent.ToString()

            Return rTemplateContent_s
        End Function

        Public Function ProvideTranslationContent(ByVal TranslationCode As String) As String
            Dim rTranslationContent As New System.Text.StringBuilder

            ' Function Variables
            Dim TranslationFile As String, TryDefaultTranslation As Boolean = False
            ' !--

RETRY_TRANSLATIONCONTENT:
            Select Case Me._DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    TranslationFile = _
                        IO.Path.Combine( _
                            Me.TranslationsPath, _
                            String.Format("{0}.xml", TranslationCode) _
                        )

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As System.Text.Encoding

                    Try
                        fS = New IO.FileStream(TranslationFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then rTranslationContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0

                        Me._CurrentTranslationID = TranslationCode
                    Catch ex As IO.FileNotFoundException
                        If Not TryDefaultTranslation Then
                            rTranslationContent = Nothing

                            TranslationCode = Me.Settings.Configurations.DefaultTranslation
                            TryDefaultTranslation = True

                            GoTo RETRY_TRANSLATIONCONTENT
                        End If
                    Catch ex As Exception
                        rTranslationContent = Nothing
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            Me._SWCTDecompiler.GetswctFileInfo( _
                                    Me.TranslationsRegistration, String.Format("{0}.xml", TranslationCode) _
                                )

                    If swctFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Me._SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case Me._SWCTDecompiler.RequestStatus
                            Case Decompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rTranslationContent.Append(sR.ReadToEnd())

                                Me._CurrentTranslationID = TranslationCode

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case Decompiler.RequestResults.ContentNotExists
                                If Not TryDefaultTranslation Then
                                    rTranslationContent = Nothing

                                    TranslationCode = Me.Settings.Configurations.DefaultTranslation
                                    TryDefaultTranslation = True

                                    GoTo RETRY_TRANSLATIONCONTENT
                                End If
                            Case Decompiler.RequestResults.PasswordError
                                Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Dim rTranslationContent_s As String = Nothing

            If Not rTranslationContent Is Nothing Then rTranslationContent_s = rTranslationContent.ToString()

            Return rTranslationContent_s
        End Function

        Public Function ProvideConfigurationContent() As String
            Dim rConfigurationContent As New System.Text.StringBuilder

            ' Function Variables
            Dim ConfigurationFile As String
            ' !--

            Select Case Me._DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    ConfigurationFile = _
                        IO.Path.Combine( _
                            Me.TemplatesPath, "Configuration.xml")

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As System.Text.Encoding

                    Try
                        fS = New IO.FileStream(ConfigurationFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then rConfigurationContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0
                    Catch ex As IO.FileNotFoundException
                        Throw New Exception(Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND)
                    Catch ex As Exception
                        rConfigurationContent = Nothing
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            Me._SWCTDecompiler.GetswctFileInfo( _
                                    Me.TemplatesRegistration, "Configuration.xml" _
                                )

                    If swctFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Me._SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case Me._SWCTDecompiler.RequestStatus
                            Case Decompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rConfigurationContent.Append(sR.ReadToEnd())

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case Decompiler.RequestResults.ContentNotExists
                                Throw New Exception(Globals.SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND)
                            Case Decompiler.RequestResults.PasswordError
                                Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Dim rConfigurationContent_s As String = Nothing

            If Not rConfigurationContent Is Nothing Then rConfigurationContent_s = rConfigurationContent.ToString()

            Return rConfigurationContent_s
        End Function

        Public Function ProvideControlMapContent() As String
            Dim rControlMapContent As New System.Text.StringBuilder

            ' Function Variables
            Dim ControlsMapFile As String
            ' !--

            Select Case Me._DeploymentType
                Case PGlobals.ThemeBase.DeploymentTypes.Development
                    ControlsMapFile = _
                        IO.Path.Combine( _
                            Me.TemplatesPath, "ControlsMap.xml")

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As System.Text.Encoding

                    Try
                        fS = New IO.FileStream(ControlsMapFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then rControlMapContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0
                    Catch ex As IO.FileNotFoundException
                        Throw New Exception(Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND)
                    Catch ex As Exception
                        rControlMapContent = Nothing
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case PGlobals.ThemeBase.DeploymentTypes.Release
                    Dim swctFileInfo As Decompiler.swctFileInfo = _
                            Me._SWCTDecompiler.GetswctFileInfo( _
                                    Me.TemplatesRegistration, "ControlsMap.xml" _
                                )

                    If swctFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Me._SWCTDecompiler.ReadFile(swctFileInfo.Index, swctFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case Me._SWCTDecompiler.RequestStatus
                            Case Decompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rControlMapContent.Append(sR.ReadToEnd())

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case Decompiler.RequestResults.ContentNotExists
                                Throw New Exception(Globals.SystemMessages.ESSENTIAL_CONTROLSMAPNOTFOUND)
                            Case Decompiler.RequestResults.PasswordError
                                Throw New Exception(Globals.SystemMessages.PASSWORD_WRONG)
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Dim rControlMapContent_s As String = Nothing

            If Not rControlMapContent Is Nothing Then rControlMapContent_s = rControlMapContent.ToString()

            Return rControlMapContent_s
        End Function

#Region " Solid Web Content Theme Decompiler "
        Protected Class Decompiler
            Public Class swctFileInfo
                Private _Index As Long
                Private _RegistrationPath As String
                Private _FileName As String
                Private _Length As Long
                Private _CompressedLength As Long

                Friend Sub New(ByVal Index As Long, ByVal RegistrationPath As String, ByVal FileName As String, ByVal Length As Long, ByVal CompressedLength As Long)
                    Me._Index = Index
                    Me._RegistrationPath = RegistrationPath
                    Me._FileName = FileName
                    Me._Length = Length
                    Me._CompressedLength = CompressedLength
                End Sub

                Public ReadOnly Property Index() As Long
                    Get
                        Return Me._Index
                    End Get
                End Property

                Public ReadOnly Property RegistrationPath() As String
                    Get
                        Return Me._RegistrationPath
                    End Get
                End Property

                Public ReadOnly Property FileName() As String
                    Get
                        Return Me._FileName
                    End Get
                End Property

                Public ReadOnly Property Length() As Long
                    Get
                        Return Me._Length
                    End Get
                End Property

                Public ReadOnly Property CompressedLength() As Long
                    Get
                        Return Me._CompressedLength
                    End Get
                End Property
            End Class

            Private _swctFileLocation As String
            Private _PasswordHash As Byte() = Nothing
            Private Shared _LastModifiedDate As Date = Date.MinValue

            Private _RequestStatus As RequestResults

            Public Enum RequestResults As Integer
                None = 0
                Authenticated = 1
                PasswordError = 2
                ContentNotExists = 3
            End Enum

            Public Sub New(ByVal swctFileLocation As String, ByVal Password As Byte())
                Me._swctFileLocation = swctFileLocation
                Me._PasswordHash = Password

                Dim FI As IO.FileInfo = _
                    New IO.FileInfo(Me._swctFileLocation)

                If FI.Exists Then Decompiler._LastModifiedDate = FI.CreationTime

                Me._RequestStatus = RequestResults.None
            End Sub

            Public ReadOnly Property RequestStatus() As RequestResults
                Get
                    Return Me._RequestStatus
                End Get
            End Property

            Private Shared _swctFilesList As System.Collections.Generic.List(Of swctFileInfo) = Nothing
            Public ReadOnly Property swctFilesList() As System.Collections.Generic.List(Of swctFileInfo)
                Get
                    ' Control Template File Changes
                    Dim FI As IO.FileInfo = _
                        New IO.FileInfo(Me._swctFileLocation)

                    If FI.Exists AndAlso _
                        Date.Compare(Decompiler._LastModifiedDate, FI.CreationTime) <> 0 Then

                        ' Clear All Cache
                        Decompiler._swctFilesList = Nothing
                        Decompiler._streamBytesCache = Nothing
                        Decompiler._LastModifiedDate = FI.CreationTime
                    End If
                    ' !---

                    If Decompiler._swctFilesList Is Nothing Then _
                        Decompiler._swctFilesList = Me.ReadFileListInternal()

                    Return Decompiler._swctFilesList
                End Get
            End Property

            Private Function ReadFileListInternal() As System.Collections.Generic.List(Of swctFileInfo)
                Dim rSWCTFileInfo As New System.Collections.Generic.List(Of swctFileInfo)

                Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

                Dim swctFileStream As IO.FileStream, swctStreamBinaryReader As IO.BinaryReader = Nothing
                Try
                    swctFileStream = New IO.FileStream(Me._swctFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                    swctStreamBinaryReader = New IO.BinaryReader(swctFileStream, System.Text.Encoding.UTF8)

                    Dim ReadC As Integer = 0, IndexTotal As Long = 0
                    Dim MovedIndex As Long = swctStreamBinaryReader.ReadInt64()

                    Do
                        IndexTotal = swctStreamBinaryReader.BaseStream.Position

                        Index = swctStreamBinaryReader.ReadInt64() + MovedIndex + 8
                        localRegistrationPath = swctStreamBinaryReader.ReadString()
                        localFileName = swctStreamBinaryReader.ReadString()
                        Length = swctStreamBinaryReader.ReadInt64()
                        CompressedLength = swctStreamBinaryReader.ReadInt64()

                        ReadC += CType((swctStreamBinaryReader.BaseStream.Position - IndexTotal), Integer)

                        rSWCTFileInfo.Add( _
                            New swctFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength))
                    Loop Until ReadC = MovedIndex
                Finally
                    If Not swctStreamBinaryReader Is Nothing Then swctStreamBinaryReader.Close() : GC.SuppressFinalize(swctStreamBinaryReader)
                End Try

                Return rSWCTFileInfo
            End Function

            Public Function GetswctFileInfo(ByVal RegistrationPath As String, ByVal FileName As String) As swctFileInfo
                ' Search In Cache First
                For Each Item As swctFileInfo In Me.swctFilesList
                    If String.Compare(RegistrationPath, Item.RegistrationPath, True) = 0 AndAlso _
                        String.Compare(FileName, Item.FileName, True) = 0 Then

                        Return Item
                    End If
                Next
                ' !---

                Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

                Dim swctFileStream As IO.FileStream, swctStreamBinaryReader As IO.BinaryReader = Nothing
                Dim IsFound As Boolean = False
                Try
                    swctFileStream = New IO.FileStream(Me._swctFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                    swctStreamBinaryReader = New IO.BinaryReader(swctFileStream, System.Text.Encoding.UTF8)

                    Dim ReadC As Integer = 0, IndexTotal As Long = 0
                    Dim MovedIndex As Long = swctStreamBinaryReader.ReadInt64()

                    Do
                        IndexTotal = swctStreamBinaryReader.BaseStream.Position

                        Index = swctStreamBinaryReader.ReadInt64() + MovedIndex + 8
                        localRegistrationPath = swctStreamBinaryReader.ReadString()
                        localFileName = swctStreamBinaryReader.ReadString()
                        Length = swctStreamBinaryReader.ReadInt64()
                        CompressedLength = swctStreamBinaryReader.ReadInt64()

                        ReadC += CType((swctStreamBinaryReader.BaseStream.Position - IndexTotal), Integer)

                        If String.Compare( _
                                RegistrationPath, localRegistrationPath, True) = 0 AndAlso _
                           String.Compare( _
                                FileName, localFileName, True) = 0 Then

                            IsFound = True

                            Exit Do
                        End If
                    Loop Until ReadC = MovedIndex
                Catch ex As Exception
                    IsFound = False
                Finally
                    If Not swctStreamBinaryReader Is Nothing Then swctStreamBinaryReader.Close() : GC.SuppressFinalize(swctStreamBinaryReader)
                End Try

                If Not IsFound Then
                    Index = -1
                    localRegistrationPath = Nothing
                    localFileName = Nothing
                    Length = -1
                    CompressedLength = -1
                End If

                Return New swctFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength)
            End Function

            Private Shared _streamBytesCache As System.Collections.Hashtable = Nothing
            Public Sub ReadFile(ByVal index As Long, ByVal length As Long, ByRef OutputStream As IO.Stream)
                If index = -1 Then Throw New Exception("Index must be specified!")
                If length < 1 Then Throw New Exception("Length must be specified!")
                If OutputStream Is Nothing Then Throw New Exception("OutputStream must be specified!")

                ' Search in Cache First
                Dim SearchKey As String = _
                    String.Format("i:{0}.l:{1}", index, length)
                Dim InCache As Boolean = False

                If Decompiler._streamBytesCache Is Nothing Then _
                    Decompiler._streamBytesCache = Hashtable.Synchronized(New Hashtable)

                SyncLock Decompiler._streamBytesCache.SyncRoot
                    If Decompiler._streamBytesCache.ContainsKey(SearchKey) Then
                        Dim rbuffer As Byte() = CType(Decompiler._streamBytesCache.Item(SearchKey), Byte())

                        OutputStream.Write(rbuffer, 0, rbuffer.Length)

                        Me._RequestStatus = RequestResults.Authenticated

                        InCache = True
                    End If
                End SyncLock

                If InCache Then GoTo QUICKEXIT
                ' !---

                Dim swctFileStream As IO.FileStream = Nothing
                Dim GZipHelperStream As IO.MemoryStream = Nothing, GZipStream As IO.Compression.GZipStream = Nothing

                Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), length), Byte())

                Try
                    swctFileStream = New IO.FileStream(Me._swctFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                    swctFileStream.Seek(index, IO.SeekOrigin.Begin)
                    swctFileStream.Read(buffer, 0, buffer.Length)

                    ' FILE PROTECTION
                    If Not Me._PasswordHash Is Nothing Then
                        For pBC As Integer = 0 To buffer.Length - 1
                            buffer(pBC) = buffer(pBC) Xor Me._PasswordHash(pBC Mod Me._PasswordHash.Length)
                        Next
                    End If
                    ' !--

                    GZipHelperStream = New IO.MemoryStream(buffer, 0, buffer.Length, False)
                    GZipStream = New IO.Compression.GZipStream(GZipHelperStream, IO.Compression.CompressionMode.Decompress, False)

                    Dim rbuffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte())
                    Dim bC As Integer, tB As Integer = 0

                    Do
                        bC = GZipStream.Read(rbuffer, 0, rbuffer.Length) : tB += bC

                        If bC > 0 Then OutputStream.Write(rbuffer, 0, bC)
                    Loop While bC > 0

                    Me._RequestStatus = RequestResults.Authenticated

                    ' Cache What You Read
                    Dim cacheBytes As Byte() = CType(Array.CreateInstance(GetType(Byte), tB), Byte())

                    OutputStream.Seek(0, IO.SeekOrigin.Begin)
                    OutputStream.Read(cacheBytes, 0, cacheBytes.Length)

                    SyncLock Decompiler._streamBytesCache.SyncRoot
                        Try
                            If Decompiler._streamBytesCache.ContainsKey(SearchKey) Then _
                                Decompiler._streamBytesCache.Remove(SearchKey)

                            Decompiler._streamBytesCache.Add(SearchKey, cacheBytes)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' If an error occur while caching, let it not to be cached.
                        End Try
                    End SyncLock
                    ' !---
                Catch ex As IO.FileNotFoundException
                    Me._RequestStatus = RequestResults.ContentNotExists
                Catch ex As Exception
                    Me._RequestStatus = RequestResults.PasswordError
                Finally
                    If Not swctFileStream Is Nothing Then swctFileStream.Close() : GC.SuppressFinalize(swctFileStream)

                    If Not GZipStream Is Nothing Then GZipStream.Close() : GC.SuppressFinalize(GZipStream)
                    If Not GZipHelperStream Is Nothing Then GZipHelperStream.Close() : GC.SuppressFinalize(GZipHelperStream)
                End Try

QUICKEXIT:
                OutputStream.Seek(0, IO.SeekOrigin.Begin)
            End Sub
        End Class
#End Region

    End Class
End Namespace