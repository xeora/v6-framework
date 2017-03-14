Option Strict On

Namespace Xeora.Web.Deployment
    Public Class DomainDeployment
        Inherits DeploymentBase
        Implements IDisposable

        Private _Synchronised As Boolean = False

        Private _IntegrityCheckID As String
        Private _IntegrityCheckRequires As Boolean

        Private _Settings As [Shared].IDomain.ISettings = Nothing
        Private _Language As [Shared].IDomain.ILanguage = Nothing
        Private _xService As [Shared].IDomain.IxService = Nothing
        Private _Children As [Shared].DomainInfo.DomainInfoCollection = Nothing

        Private Sub New(ByVal DomainIDAccessTree As String())
            MyBase.New(DomainIDAccessTree, Nothing)
        End Sub

        Public Sub New(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            MyBase.New(DomainIDAccessTree, LanguageID)

            Me._IntegrityCheckID =
                String.Format("ByPass{0}_{1}_{2}",
                                    [Shared].Configurations.ApplicationRoot.BrowserImplementation.Replace("/"c, "_"c),
                                    String.Join(Of String)(":", Me.DomainIDAccessTree),
                                    Me.DeploymentType)

            If Not Boolean.TryParse(
                    CType([Shared].Helpers.Context.Application.Item(Me._IntegrityCheckID), String),
                    Me._IntegrityCheckRequires) Then Me._IntegrityCheckRequires = False

            Me.Synchronise()
        End Sub

        Private Sub Synchronise()
            If Me.DeploymentType = [Shared].DomainInfo.DeploymentTypes.Development AndAlso
                Not Me._IntegrityCheckRequires AndAlso
                (
                    Not IO.Directory.Exists(Me.LanguagesRegistration) OrElse
                    Not IO.Directory.Exists(Me.TemplatesRegistration)
                ) Then

                Throw New Exception.DeploymentException(String.Format("Domain {0}", [Global].SystemMessages.PATH_WRONGSTRUCTURE))
            End If

            Me._Settings = New Site.Setting.Settings(
                                Me.ProvideConfigurationContent()
                            )

            If String.IsNullOrEmpty(Me.LanguageID) Then _
                MyBase.LanguageID = Me._Settings.Configurations.DefaultLanguage

            Try
                Me._Language = New Site.Setting.Language(
                                    Me.ProvideLanguageContent(Me.LanguageID)
                                )
            Catch ex As System.Exception
                MyBase.LanguageID = Me._Settings.Configurations.DefaultLanguage

                Me._Language = New Site.Setting.Language(
                                    Me.ProvideLanguageContent(Me.LanguageID)
                                )
            End Try

            Me._xService = New Site.Setting.xService()

            ' Compile Children Domains
            Me._Children = New [Shared].DomainInfo.DomainInfoCollection()
            Me.CompileChildrenDomains(Me._Children)

            If Not Me._IntegrityCheckRequires Then
                Select Case Me.DeploymentType
                    Case [Shared].DomainInfo.DeploymentTypes.Development
                        ' Control Domain Language and Template Folders
                        Dim DomainLanguagesDI As New IO.DirectoryInfo(Me.LanguagesRegistration)

                        For Each DomainLanguageDI As IO.DirectoryInfo In DomainLanguagesDI.GetDirectories()
                            If Not IO.Directory.Exists(
                                Me.DomainContentsRegistration(DomainLanguageDI.Name)) Then _
                                    Throw New Exception.DeploymentException(String.Format("Domain {0}", [Global].SystemMessages.PATH_WRONGSTRUCTURE))
                        Next
                        ' !--

                        ' -- Control Those System Essential Files are Exists! --
                        Dim SystemMessage As String = Nothing

                        Dim ControlsXML As String = IO.Path.Combine(Me.TemplatesRegistration, "Controls.xml")
                        Dim ConfigurationXML As String = IO.Path.Combine(Me.TemplatesRegistration, "Configuration.xml")

                        If Not IO.File.Exists(ConfigurationXML) Then
                            SystemMessage = Me._Language.Get("CONFIGURATIONNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                            Throw New Exception.DeploymentException(SystemMessage & "!")
                        End If

                        If Not IO.File.Exists(ControlsXML) Then
                            SystemMessage = Me._Language.Get("CONTROLSXMLNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.ESSENTIAL_CONTROLSXMLNOTFOUND

                            Throw New Exception.DeploymentException(SystemMessage & "!")
                        End If
                            ' !--

                    Case [Shared].DomainInfo.DeploymentTypes.Release
                        ' -- Control Those System Essential Files are Exists! --
                        Dim SystemMessage As String = Nothing

                        Dim ControlsXMLFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                            Me.Decompiler.GetFileInfo(Me.TemplatesRegistration, "Controls.xml")
                        Dim ConfigurationFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                            Me.Decompiler.GetFileInfo(Me.TemplatesRegistration, "Configuration.xml")

                        If ConfigurationFileInfo.Index = -1 Then
                            SystemMessage = Me._Language.Get("CONFIGURATIONNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND

                            Throw New Exception.DeploymentException(SystemMessage & "!")
                        End If

                        If ControlsXMLFileInfo.Index = -1 Then
                            SystemMessage = Me._Language.Get("CONTROLSXMLNOTFOUND")

                            If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.ESSENTIAL_CONTROLSXMLNOTFOUND

                            Throw New Exception.DeploymentException(SystemMessage & "!")
                        End If
                        ' !--

                End Select
            End If

            If [Shared].Helpers.Context.Application.Item(Me._IntegrityCheckID) Is Nothing Then _
                [Shared].Helpers.Context.Application.Item(Me._IntegrityCheckID) = True

            Me._Synchronised = True
        End Sub

        Private Sub CompileChildrenDomains(ByRef ChildrenToFill As [Shared].DomainInfo.DomainInfoCollection)
            Dim ChildrenDI As New IO.DirectoryInfo(Me.ChildrenRootPath)

            If ChildrenDI.Exists Then
                For Each ChildDI As IO.DirectoryInfo In ChildrenDI.GetDirectories()
                    Dim ChildAccessTree As String() = New String(Me.DomainIDAccessTree.Length) {}
                    Array.Copy(Me.DomainIDAccessTree, 0, ChildAccessTree, 0, Me.DomainIDAccessTree.Length)
                    ChildAccessTree(ChildAccessTree.Length - 1) = ChildDI.Name

                    Dim ChildDomainDeployment As New DomainDeployment(ChildAccessTree, Me._Language.ID)

                    Dim DomainInfo As [Shared].DomainInfo =
                        New [Shared].DomainInfo(ChildDomainDeployment.DeploymentType, ChildDI.Name, AvailableLanguageInfos(ChildDomainDeployment))
                    DomainInfo.Children.AddRange(ChildDomainDeployment.Children)

                    ChildrenToFill.Add(DomainInfo)

                    ChildDomainDeployment.Dispose()
                Next
            End If
        End Sub

        Public Overrides ReadOnly Property Settings() As [Shared].IDomain.ISettings
            Get
                Return Me._Settings
            End Get
        End Property

        Public Overrides ReadOnly Property Language() As [Shared].IDomain.ILanguage
            Get
                Return Me._Language
            End Get
        End Property

        Public Overrides ReadOnly Property xService() As [Shared].IDomain.IxService
            Get
                Return Me._xService
            End Get
        End Property

        Public Overrides ReadOnly Property Children() As [Shared].DomainInfo.DomainInfoCollection
            Get
                Return Me._Children
            End Get
        End Property

        Public Overrides Function ProvideTemplateContent(ByVal ServiceFullPath As String) As String
            ' -- Check is template file is exists
            If Not Me.CheckTemplateExists(ServiceFullPath) Then
                Dim SystemMessage As String = Me._Language.Get("TEMPLATE_NOFOUND")

                If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = [Global].SystemMessages.TEMPLATE_NOFOUND

                Throw New Exception.DeploymentException(String.Format(SystemMessage & "!", ServiceFullPath))
            End If
            ' !--

            Return MyBase.ProvideTemplateContent(ServiceFullPath)
        End Function

        Public Overrides Sub ProvideFileStream(ByRef OutputStream As IO.Stream, ByVal RequestedFilePath As String)
            If String.IsNullOrEmpty(RequestedFilePath) Then OutputStream = Nothing : Exit Sub

            RequestedFilePath = RequestedFilePath.Replace("/"c, "\"c)
            If RequestedFilePath.Chars(0) = "\"c Then _
                RequestedFilePath = RequestedFilePath.Substring(1)

            Select Case Me.DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    Dim RequestedFileFullPath As String =
                        IO.Path.Combine(
                            Me.DomainContentsRegistration(),
                            RequestedFilePath)

                    If IO.File.Exists(RequestedFileFullPath) Then
                        Try
                            OutputStream = New IO.FileStream(RequestedFileFullPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        Catch ex As System.Exception
                            OutputStream = Nothing
                        End Try
                    Else
                        OutputStream = Nothing
                    End If
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                        Me.Decompiler.GetFileInfo(
                            IO.Path.Combine(
                                Me.DomainContentsRegistration(),
                                RequestedFilePath.Replace(
                                    IO.Path.GetFileName(RequestedFilePath),
                                    String.Empty
                                )
                            ),
                            IO.Path.GetFileName(RequestedFilePath)
                        )

                    If XeoraFileInfo.Index > -1 Then
                        OutputStream = New IO.MemoryStream()

                        Me.Decompiler.ReadFile(XeoraFileInfo.Index, XeoraFileInfo.CompressedLength, CType(OutputStream, IO.Stream))

                        If Me.Decompiler.RequestStatus = XeoraDomainDecompiler.RequestResults.PasswordError Then _
                            Throw New Exception.DeploymentException([Global].SystemMessages.PASSWORD_WRONG, New Security.SecurityException())
                    Else
                        OutputStream = Nothing
                    End If
            End Select
        End Sub

        Public Overloads Shared Function AvailableLanguageInfos(ByVal DomainIDAccessTree As String()) As [Shared].DomainInfo.LanguageInfo()
            Dim DomainDeployment As New DomainDeployment(DomainIDAccessTree)

            Return DomainDeployment.AvailableLanguageInfos(DomainDeployment)
        End Function

        Public Overloads Shared Function AvailableLanguageInfos(ByRef WorkingDomainDeployment As DomainDeployment) As [Shared].DomainInfo.LanguageInfo()
            Dim rLanguageInfos As New Generic.List(Of [Shared].DomainInfo.LanguageInfo)

            Dim DomainDeployment As DomainDeployment

            Select Case WorkingDomainDeployment.DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    For Each sFI As XeoraDomainDecompiler.XeoraFileInfo In WorkingDomainDeployment.Decompiler.FilesList
                        If sFI.RegistrationPath.IndexOf(WorkingDomainDeployment.LanguagesRegistration) > -1 Then
                            DomainDeployment = New DomainDeployment(WorkingDomainDeployment.DomainIDAccessTree, IO.Path.GetFileNameWithoutExtension(sFI.FileName))

                            rLanguageInfos.Add(DomainDeployment.Language.Info)

                            DomainDeployment.Dispose()
                        End If
                    Next

                Case [Shared].DomainInfo.DeploymentTypes.Development
                    If IO.Directory.Exists(WorkingDomainDeployment.LanguagesRegistration) Then
                        Dim LanguagesDI As New IO.DirectoryInfo(WorkingDomainDeployment.LanguagesRegistration)

                        For Each TFI As IO.FileInfo In LanguagesDI.GetFiles()
                            DomainDeployment = New DomainDeployment(WorkingDomainDeployment.DomainIDAccessTree, IO.Path.GetFileNameWithoutExtension(TFI.Name))

                            rLanguageInfos.Add(DomainDeployment.Language.Info)

                            DomainDeployment.Dispose()
                        Next
                    End If

            End Select

            Return rLanguageInfos.ToArray()
        End Function

        Public Shared Sub ExtractApplication(ByVal DomainIDAccessTree As String(), ByVal ExtractLocation As String)
            Dim DomainDeployment As New DomainDeployment(DomainIDAccessTree)
            Dim ExecutablesPath As String =
                DomainDeployment.ExecutablesPath
            DomainDeployment.Dispose()

            Dim DI As New IO.DirectoryInfo(ExecutablesPath)

            For Each fI As IO.FileInfo In DI.GetFiles()
                If Not IO.File.Exists(
                    IO.Path.Combine(ExtractLocation, fI.Name)) Then

                    Try
                        fI.CopyTo(
                            IO.Path.Combine(ExtractLocation, fI.Name))
                    Catch ex As System.Exception
                        ' Just Handle Exceptions
                    End Try
                End If
            Next
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not Me._Settings Is Nothing Then Me._Settings.Dispose()
                If Not Me._Language Is Nothing Then Me._Language.Dispose()
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
End Namespace