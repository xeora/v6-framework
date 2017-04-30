Option Strict On

Namespace Xeora.Web.Deployment
    Public MustInherit Class DeploymentBase
        Private _DeploymentType As [Shared].DomainInfo.DeploymentTypes
        Private _Decompiler As XeoraDomainDecompiler

        Private _DomainIDAccessTree As String()
        Private _LanguageID As String

        Private _WorkingRoot As String

        Public MustOverride Sub Dispose()
        Public MustOverride ReadOnly Property Settings() As [Shared].IDomain.ISettings
        Public MustOverride ReadOnly Property Language() As [Shared].IDomain.ILanguage
        Public MustOverride ReadOnly Property xService() As [Shared].IDomain.IxService
        Public MustOverride ReadOnly Property Children() As [Shared].DomainInfo.DomainInfoCollection
        Public MustOverride Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String)

        Public Sub New(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            Me._DomainIDAccessTree = DomainIDAccessTree
            Me._LanguageID = LanguageID

            If Me._DomainIDAccessTree Is Nothing OrElse
                Me._DomainIDAccessTree.Length = 0 Then

                Throw New Exception.DeploymentException([Global].SystemMessages.IDMUSTBESET)
            End If

            Me._WorkingRoot =
                IO.Path.GetFullPath(
                    IO.Path.Combine(
                        [Shared].Configurations.PhysicalRoot,
                        [Shared].Configurations.ApplicationRoot.FileSystemImplementation,
                        "Domains",
                        Me.CreateDomainAccessPathString()
                    )
                )

            If Not IO.Directory.Exists(Me._WorkingRoot) Then _
                Throw New Exception.DomainNotExistsException([Global].SystemMessages.PATH_NOTEXISTS, New IO.DirectoryNotFoundException(String.Format("WorkingPath: {0}", Me._WorkingRoot)))

            Dim ReleaseTestPath As String =
                IO.Path.Combine(Me._WorkingRoot, "Content.xeora")

            If IO.File.Exists(ReleaseTestPath) Then
                Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Release

                Me._Decompiler = New XeoraDomainDecompiler(Me._WorkingRoot)
            Else
                Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Development
            End If
        End Sub

        Public ReadOnly Property DeploymentType() As [Shared].DomainInfo.DeploymentTypes
            Get
                Return Me._DeploymentType
            End Get
        End Property

        Public ReadOnly Property DomainIDAccessTree() As String()
            Get
                Return Me._DomainIDAccessTree
            End Get
        End Property

        Public Property LanguageID() As String
            Get
                Return Me._LanguageID
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    Me._LanguageID = value
                Else
                    Throw New Exception.DeploymentException("LanguageID must not be null", New NoNullAllowedException())
                End If
            End Set
        End Property

        Protected ReadOnly Property Decompiler() As XeoraDomainDecompiler
            Get
                Return Me._Decompiler
            End Get
        End Property

        Protected ReadOnly Property WorkingRoot() As String
            Get
                Return Me._WorkingRoot
            End Get
        End Property

        Protected ReadOnly Property ExecutablesPath() As String
            Get
                Return IO.Path.Combine(Me._WorkingRoot, "Executables")
            End Get
        End Property

        Protected ReadOnly Property TemplatesRegistration() As String
            Get
                If Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Release Then
                    Return "\Templates\"
                Else
                    Return IO.Path.Combine(Me._WorkingRoot, "Templates")
                End If
            End Get
        End Property

        Protected ReadOnly Property DomainContentsRegistration(Optional ByVal LanguageID As String = Nothing) As String
            Get
                If String.IsNullOrEmpty(LanguageID) Then LanguageID = Me._LanguageID

                If Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Release Then
                    Return String.Format("\Contents\{0}\", LanguageID)
                Else
                    Return IO.Path.Combine(Me._WorkingRoot, "Contents", LanguageID)
                End If
            End Get
        End Property

        Protected ReadOnly Property LanguagesRegistration() As String
            Get
                If Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Release Then
                    Return "\Languages\"
                Else
                    Return IO.Path.Combine(Me._WorkingRoot, "Languages")
                End If
            End Get
        End Property

        Protected ReadOnly Property ChildrenRootPath() As String
            Get
                Return IO.Path.Combine(Me._WorkingRoot, "Addons")
            End Get
        End Property

        Private Function CreateDomainAccessPathString() As String
            Dim rDomainAccessPath As String = Me._DomainIDAccessTree(0)

            For iC As Integer = 1 To Me._DomainIDAccessTree.Length - 1
                rDomainAccessPath = IO.Path.Combine(rDomainAccessPath, "Addons", Me._DomainIDAccessTree(iC))
            Next

            Return rDomainAccessPath
        End Function

        Private Function DetectEncoding(ByRef inStream As IO.Stream) As Text.Encoding
            Dim rEncoding As Text.Encoding = Text.Encoding.UTF8

            inStream.Seek(0, IO.SeekOrigin.Begin)

            Dim bC As Integer, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 4), Byte())

            bC = inStream.Read(buffer, 0, buffer.Length)

            If bC > 0 Then
                If bC >= 2 AndAlso
                    buffer(0) = 254 AndAlso
                    buffer(1) = 255 Then

                    rEncoding = New System.Text.UnicodeEncoding(True, True)

                    inStream.Seek(2, IO.SeekOrigin.Begin)
                ElseIf bC >= 2 AndAlso
                        buffer(0) = 255 AndAlso
                        buffer(1) = 254 Then

                    If bC = 4 AndAlso
                        buffer(2) = 0 AndAlso
                        buffer(3) = 0 Then

                        rEncoding = New Text.UTF32Encoding(False, True)

                        inStream.Seek(4, IO.SeekOrigin.Begin)
                    Else
                        rEncoding = New Text.UnicodeEncoding(False, True)

                        inStream.Seek(2, IO.SeekOrigin.Begin)
                    End If
                ElseIf bC >= 3 AndAlso
                    buffer(0) = 239 AndAlso
                    buffer(1) = 187 AndAlso
                    buffer(2) = 191 Then

                    rEncoding = New Text.UTF8Encoding()

                    inStream.Seek(3, IO.SeekOrigin.Begin)
                ElseIf bC = 4 AndAlso
                    buffer(0) = 0 AndAlso
                    buffer(1) = 0 AndAlso
                    buffer(2) = 254 AndAlso
                    buffer(3) = 255 Then

                    rEncoding = New Text.UTF32Encoding(True, True)

                    inStream.Seek(4, IO.SeekOrigin.Begin)
                Else
                    inStream.Seek(0, IO.SeekOrigin.Begin)
                End If
            End If

            Return rEncoding
        End Function

        Public Function CheckTemplateExists(ByVal ServiceFullPath As String) As Boolean
            Dim rBoolean As Boolean = False

            Select Case Me._DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    rBoolean = IO.File.Exists(
                                    IO.Path.Combine(
                                        Me.TemplatesRegistration, String.Format("{0}.xchtml", ServiceFullPath)
                                    )
                                )
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                        Me._Decompiler.GetFileInfo(
                            Me.TemplatesRegistration, String.Format("{0}.xchtml", ServiceFullPath)
                        )

                    rBoolean = XeoraFileInfo.Index > -1
            End Select

            Return rBoolean
        End Function

        Public Overridable Function ProvideTemplateContent(ByVal ServiceFullPath As String) As String
            Dim rTemplateContent As String = String.Empty

            Select Case Me._DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    Dim TemplateFile As String =
                        IO.Path.Combine(
                            Me.TemplatesRegistration, String.Format("{0}.xchtml", ServiceFullPath))

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As Text.Encoding, TemplateContent As New Text.StringBuilder()

                    Try
                        fS = New IO.FileStream(TemplateFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then TemplateContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0

                        rTemplateContent = TemplateContent.ToString()
                    Catch ex As System.Exception
                        rTemplateContent = String.Empty
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                        Me._Decompiler.GetFileInfo(
                            Me.TemplatesRegistration, String.Format("{0}.xchtml", ServiceFullPath)
                        )

                    If XeoraFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Dim RequestResult As XeoraDomainDecompiler.RequestResults =
                            Me._Decompiler.ReadFile(XeoraFileInfo.Index, XeoraFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case RequestResult
                            Case XeoraDomainDecompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rTemplateContent = sR.ReadToEnd()

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case XeoraDomainDecompiler.RequestResults.PasswordError
                                Throw New Exception.DeploymentException([Global].SystemMessages.PASSWORD_WRONG, New Security.SecurityException())
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Return rTemplateContent
        End Function

        Public Function ProvideLanguageContent(ByVal LanguageID As String) As String
            Dim rLanguageContent As String = String.Empty

            Select Case Me._DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    Dim LanguageFile As String =
                        IO.Path.Combine(
                            Me.LanguagesRegistration,
                            String.Format("{0}.xml", LanguageID)
                        )

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As Text.Encoding, LanguageContent As New Text.StringBuilder()

                    Try
                        fS = New IO.FileStream(LanguageFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then LanguageContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0

                        Me._LanguageID = LanguageID
                        rLanguageContent = LanguageContent.ToString()
                    Catch ex As IO.FileNotFoundException
                        If String.Compare(LanguageID, Me.Settings.Configurations.DefaultLanguage) <> 0 Then _
                            rLanguageContent = Me.ProvideLanguageContent(Me.Settings.Configurations.DefaultLanguage)
                    Catch ex As System.Exception
                        rLanguageContent = String.Empty
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                            Me._Decompiler.GetFileInfo(
                                    Me.LanguagesRegistration, String.Format("{0}.xml", LanguageID)
                                )

                    If XeoraFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Dim RequestResult As XeoraDomainDecompiler.RequestResults =
                            Me._Decompiler.ReadFile(XeoraFileInfo.Index, XeoraFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case RequestResult
                            Case XeoraDomainDecompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rLanguageContent = sR.ReadToEnd()

                                Me._LanguageID = LanguageID

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case XeoraDomainDecompiler.RequestResults.ContentNotExists
                                If String.Compare(LanguageID, Me.Settings.Configurations.DefaultLanguage) <> 0 Then _
                                    rLanguageContent = Me.ProvideLanguageContent(Me.Settings.Configurations.DefaultLanguage)
                            Case XeoraDomainDecompiler.RequestResults.PasswordError
                                Throw New Exception.DeploymentException([Global].SystemMessages.PASSWORD_WRONG, New Security.SecurityException())
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Return rLanguageContent
        End Function

        Public Function ProvideConfigurationContent() As String
            Dim rConfigurationContent As String = String.Empty

            Select Case Me._DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    Dim ConfigurationFile As String =
                        IO.Path.Combine(
                            Me.TemplatesRegistration, "Configuration.xml")

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As Text.Encoding, ConfigurationContent As New Text.StringBuilder

                    Try
                        fS = New IO.FileStream(ConfigurationFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then ConfigurationContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0

                        rConfigurationContent = ConfigurationContent.ToString()
                    Catch ex As IO.FileNotFoundException
                        Throw New Exception.DeploymentException([Global].SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND, ex)
                    Catch ex As system.Exception
                        rConfigurationContent = String.Empty
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                        Me._Decompiler.GetFileInfo(
                            Me.TemplatesRegistration, "Configuration.xml"
                        )

                    If XeoraFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Dim RequestResult As XeoraDomainDecompiler.RequestResults =
                            Me._Decompiler.ReadFile(XeoraFileInfo.Index, XeoraFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case RequestResult
                            Case XeoraDomainDecompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rConfigurationContent = sR.ReadToEnd()

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case XeoraDomainDecompiler.RequestResults.ContentNotExists
                                Throw New Exception.DeploymentException([Global].SystemMessages.ESSENTIAL_CONFIGURATIONNOTFOUND, New IO.FileNotFoundException())
                            Case XeoraDomainDecompiler.RequestResults.PasswordError
                                Throw New Exception.DeploymentException([Global].SystemMessages.PASSWORD_WRONG, New Security.SecurityException())
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Return rConfigurationContent
        End Function

        Public Function ProvideControlsContent() As String
            Dim rControlMapContent As String = String.Empty

            Select Case Me._DeploymentType
                Case [Shared].DomainInfo.DeploymentTypes.Development
                    Dim ControlsXMLFile As String =
                        IO.Path.Combine(
                            Me.TemplatesRegistration, "Controls.xml")

                    Dim fS As IO.FileStream = Nothing, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 102400), Byte()), rB As Integer
                    Dim encoding As Text.Encoding, ControlMapContent As New Text.StringBuilder()

                    Try
                        fS = New IO.FileStream(ControlsXMLFile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                        encoding = Me.DetectEncoding(CType(fS, IO.Stream))

                        Do
                            rB = fS.Read(buffer, 0, buffer.Length)

                            If rB > 0 Then ControlMapContent.Append(encoding.GetString(buffer, 0, rB))
                        Loop Until rB = 0

                        rControlMapContent = ControlMapContent.ToString()
                    Catch ex As IO.FileNotFoundException
                        Throw New Exception.DeploymentException([Global].SystemMessages.ESSENTIAL_CONTROLSXMLNOTFOUND, ex)
                    Catch ex As System.Exception
                        rControlMapContent = String.Empty
                    Finally
                        If Not fS Is Nothing Then fS.Close() : GC.SuppressFinalize(fS)
                    End Try
                Case [Shared].DomainInfo.DeploymentTypes.Release
                    Dim XeoraFileInfo As XeoraDomainDecompiler.XeoraFileInfo =
                        Me._Decompiler.GetFileInfo(
                            Me.TemplatesRegistration, "Controls.xml"
                        )

                    If XeoraFileInfo.Index > -1 Then
                        Dim contentStream As New IO.MemoryStream

                        Dim RequestResult As XeoraDomainDecompiler.RequestResults =
                            Me._Decompiler.ReadFile(XeoraFileInfo.Index, XeoraFileInfo.CompressedLength, CType(contentStream, IO.Stream))

                        Select Case RequestResult
                            Case XeoraDomainDecompiler.RequestResults.Authenticated
                                Dim sR As New IO.StreamReader(contentStream)

                                rControlMapContent = sR.ReadToEnd()

                                sR.Close() : GC.SuppressFinalize(sR)
                            Case XeoraDomainDecompiler.RequestResults.ContentNotExists
                                Throw New Exception.DeploymentException([Global].SystemMessages.ESSENTIAL_CONTROLSXMLNOTFOUND, New IO.FileNotFoundException())
                            Case XeoraDomainDecompiler.RequestResults.PasswordError
                                Throw New System.Exception([Global].SystemMessages.PASSWORD_WRONG)
                        End Select

                        If Not contentStream Is Nothing Then contentStream.Close() : GC.SuppressFinalize(contentStream)
                    End If
            End Select

            Return rControlMapContent
        End Function

        Public Sub ClearCache()
            If Me._DeploymentType = [Shared].DomainInfo.DeploymentTypes.Release Then _
                Me._Decompiler.ClearCache()
        End Sub

#Region " Xeora Domain Decompiler "
        Protected Class XeoraDomainDecompiler
            Public Class XeoraFileInfo
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

                Public ReadOnly Property SearchKey() As String
                    Get
                        Return XeoraFileInfo.CreateSearchKey(Me._RegistrationPath, Me._FileName)
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

                Public Shared Function CreateSearchKey(ByVal RegistrationPath As String, ByVal FileName As String) As String
                    Return String.Format("{0}${1}", RegistrationPath, FileName)
                End Function
            End Class

            Private _XeoraDomainFileLocation As String
            Private _PasswordHash As Byte() = Nothing

            Private Shared _XeoraDomainFilesListCache As New Concurrent.ConcurrentDictionary(Of String, Generic.Dictionary(Of String, XeoraFileInfo))
            Private Shared _XeoraDomainFileStreamBytesCache As Hashtable = Hashtable.Synchronized(New Hashtable())
            Private Shared _XeoraDomainFileLastModifiedDate As New Concurrent.ConcurrentDictionary(Of String, Date)

            Public Enum RequestResults
                None
                Authenticated
                PasswordError
                ContentNotExists
            End Enum

            Public Sub New(ByVal XeoraDomainRoot As String)
                Me._XeoraDomainFileLocation = IO.Path.Combine(XeoraDomainRoot, "Content.xeora")

                Dim DomainPasswordFileLocation As String =
                    IO.Path.Combine(XeoraDomainRoot, "Content.secure")

                If IO.File.Exists(DomainPasswordFileLocation) Then
                    Me._PasswordHash = Nothing

                    Dim SecuredHash As Byte() = New Byte(15) {}
                    Dim PasswordFS As IO.Stream = Nothing
                    Try
                        PasswordFS = New IO.FileStream(DomainPasswordFileLocation, IO.FileMode.Open, IO.FileAccess.Read)
                        PasswordFS.Read(SecuredHash, 0, SecuredHash.Length)
                    Catch ex As System.Exception
                        SecuredHash = Nothing
                    Finally
                        If Not PasswordFS Is Nothing Then PasswordFS.Close()
                    End Try

                    Dim FileHash As Byte()
                    Dim ContentFS As IO.Stream = Nothing
                    Try
                        ContentFS = New IO.FileStream(Me._XeoraDomainFileLocation, IO.FileMode.Open, IO.FileAccess.Read)

                        Dim MD5 As New Security.Cryptography.MD5CryptoServiceProvider()
                        FileHash = MD5.ComputeHash(ContentFS)
                    Catch ex As System.Exception
                        FileHash = Nothing
                    Finally
                        If Not ContentFS Is Nothing Then ContentFS.Close()
                    End Try

                    If Not SecuredHash Is Nothing AndAlso Not FileHash Is Nothing Then
                        Me._PasswordHash = New Byte(15) {}

                        For hC As Integer = 0 To Me._PasswordHash.Length - 1
                            Me._PasswordHash(hC) = SecuredHash(hC) Xor FileHash(hC)
                        Next
                    End If
                End If

                Dim FI As IO.FileInfo =
                    New IO.FileInfo(Me._XeoraDomainFileLocation)

                If FI.Exists Then XeoraDomainDecompiler._XeoraDomainFileLastModifiedDate.TryAdd(Me._XeoraDomainFileLocation, FI.CreationTime)
            End Sub

            Public ReadOnly Property FilesList() As Generic.Dictionary(Of String, XeoraFileInfo)
                Get
                    ' Control Template File Changes
                    Dim CachedFileDate As Date
                    If Not XeoraDomainDecompiler._XeoraDomainFileLastModifiedDate.TryGetValue(Me._XeoraDomainFileLocation, CachedFileDate) Then _
                        CachedFileDate = Date.MinValue

                    Dim FI As IO.FileInfo =
                        New IO.FileInfo(Me._XeoraDomainFileLocation)

                    If FI.Exists AndAlso
                        Date.Compare(CachedFileDate, FI.CreationTime) <> 0 Then

                        Me.ClearCache()
                    End If
                    ' !---

                    Dim rFileList As New Generic.Dictionary(Of String, XeoraFileInfo)

                    If XeoraDomainDecompiler._XeoraDomainFilesListCache.ContainsKey(Me._XeoraDomainFileLocation) Then
                        XeoraDomainDecompiler._XeoraDomainFilesListCache.TryGetValue(Me._XeoraDomainFileLocation, rFileList)
                    Else
                        Dim XeoraFileInfoList As XeoraFileInfo() = Me.ReadFileList()

                        For Each XeoraFileInfo As XeoraFileInfo In XeoraFileInfoList
                            rFileList.Add(XeoraFileInfo.SearchKey, XeoraFileInfo)
                        Next

                        XeoraDomainDecompiler._XeoraDomainFilesListCache.TryAdd(Me._XeoraDomainFileLocation, rFileList)
                    End If

                    Return rFileList
                End Get
            End Property

            Private Function ReadFileList() As XeoraFileInfo()
                Dim rXeoraFileInfo As New Generic.List(Of XeoraFileInfo)

                Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

                Dim XeoraFileStream As IO.FileStream, XeoraStreamBinaryReader As IO.BinaryReader = Nothing
                Try
                    XeoraFileStream = New IO.FileStream(Me._XeoraDomainFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                    XeoraStreamBinaryReader = New IO.BinaryReader(XeoraFileStream, System.Text.Encoding.UTF8)

                    Dim ReadC As Integer = 0, IndexTotal As Long = 0
                    Dim MovedIndex As Long = XeoraStreamBinaryReader.ReadInt64()

                    Do
                        IndexTotal = XeoraStreamBinaryReader.BaseStream.Position

                        Index = XeoraStreamBinaryReader.ReadInt64() + MovedIndex + 8
                        localRegistrationPath = XeoraStreamBinaryReader.ReadString()
                        localFileName = XeoraStreamBinaryReader.ReadString()
                        Length = XeoraStreamBinaryReader.ReadInt64()
                        CompressedLength = XeoraStreamBinaryReader.ReadInt64()

                        ReadC += CType((XeoraStreamBinaryReader.BaseStream.Position - IndexTotal), Integer)

                        rXeoraFileInfo.Add(
                            New XeoraFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength))
                    Loop Until ReadC = MovedIndex
                Finally
                    If Not XeoraStreamBinaryReader Is Nothing Then XeoraStreamBinaryReader.Close() : GC.SuppressFinalize(XeoraStreamBinaryReader)
                End Try

                Return rXeoraFileInfo.ToArray()
            End Function

            Public Function GetFileInfo(ByVal RegistrationPath As String, ByVal FileName As String) As XeoraFileInfo
                ' Search In Cache First
                Dim FilesList As Generic.Dictionary(Of String, XeoraFileInfo) = Me.FilesList
                Dim CacheSearchKey As String = XeoraFileInfo.CreateSearchKey(RegistrationPath, FileName)

                If FilesList.ContainsKey(CacheSearchKey) Then Return FilesList.Item(CacheSearchKey)
                ' !---

                Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

                Dim XeoraFileStream As IO.FileStream, XeoraStreamBinaryReader As IO.BinaryReader = Nothing
                Dim IsFound As Boolean = False
                Try
                    XeoraFileStream = New IO.FileStream(Me._XeoraDomainFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                    XeoraStreamBinaryReader = New IO.BinaryReader(XeoraFileStream, System.Text.Encoding.UTF8)

                    Dim ReadC As Integer = 0, IndexTotal As Long = 0
                    Dim MovedIndex As Long = XeoraStreamBinaryReader.ReadInt64()

                    Do
                        IndexTotal = XeoraStreamBinaryReader.BaseStream.Position

                        Index = XeoraStreamBinaryReader.ReadInt64() + MovedIndex + 8
                        localRegistrationPath = XeoraStreamBinaryReader.ReadString()
                        localFileName = XeoraStreamBinaryReader.ReadString()
                        Length = XeoraStreamBinaryReader.ReadInt64()
                        CompressedLength = XeoraStreamBinaryReader.ReadInt64()

                        ReadC += CType((XeoraStreamBinaryReader.BaseStream.Position - IndexTotal), Integer)

                        If String.Compare(
                                RegistrationPath, localRegistrationPath, True) = 0 AndAlso
                           String.Compare(
                                FileName, localFileName, True) = 0 Then

                            IsFound = True

                            Exit Do
                        End If
                    Loop Until ReadC = MovedIndex
                Catch ex As System.Exception
                    IsFound = False
                Finally
                    If Not XeoraStreamBinaryReader Is Nothing Then XeoraStreamBinaryReader.Close() : GC.SuppressFinalize(XeoraStreamBinaryReader)
                End Try

                If Not IsFound Then
                    Index = -1
                    localRegistrationPath = Nothing
                    localFileName = Nothing
                    Length = -1
                    CompressedLength = -1
                End If

                Return New XeoraFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength)
            End Function

            Public Function ReadFile(ByVal index As Long, ByVal length As Long, ByRef OutputStream As IO.Stream) As RequestResults
                Dim rRequestResult As RequestResults = RequestResults.None

                If index = -1 Then Throw New IndexOutOfRangeException()
                If length < 1 Then Throw New ArgumentOutOfRangeException()
                If OutputStream Is Nothing Then Throw New NullReferenceException()

                ' Search in Cache First
                Dim SearchKey As String =
                    String.Format("{0}$i:{1}.l:{2}", Me._XeoraDomainFileLocation, index, length)
                Dim InCache As Boolean = False

                SyncLock XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.SyncRoot
                    If XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.ContainsKey(SearchKey) Then
                        Dim rbuffer As Byte() = CType(XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Item(SearchKey), Byte())

                        OutputStream.Write(rbuffer, 0, rbuffer.Length)

                        rRequestResult = RequestResults.Authenticated

                        InCache = True
                    End If
                End SyncLock

                If InCache Then GoTo QUICKEXIT
                ' !---

                Dim XeoraFileStream As IO.FileStream = Nothing
                Dim GZipHelperStream As IO.MemoryStream = Nothing, GZipStream As IO.Compression.GZipStream = Nothing

                Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), length), Byte())

                Try
                    XeoraFileStream = New IO.FileStream(Me._XeoraDomainFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

                    XeoraFileStream.Seek(index, IO.SeekOrigin.Begin)
                    XeoraFileStream.Read(buffer, 0, buffer.Length)

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

                    rRequestResult = RequestResults.Authenticated

                    ' Cache What You Read
                    Dim cacheBytes As Byte() = CType(Array.CreateInstance(GetType(Byte), tB), Byte())

                    OutputStream.Seek(0, IO.SeekOrigin.Begin)
                    OutputStream.Read(cacheBytes, 0, cacheBytes.Length)

                    SyncLock XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.SyncRoot
                        Try
                            If XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.ContainsKey(SearchKey) Then _
                                XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Remove(SearchKey)

                            XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Add(SearchKey, cacheBytes)
                        Catch ex As System.Exception
                            ' Just Handle Exceptions
                            ' If an error occur while caching, let it not to be cached.
                        End Try
                    End SyncLock
                    ' !---
                Catch ex As IO.FileNotFoundException
                    rRequestResult = RequestResults.ContentNotExists
                Catch ex As System.Exception
                    rRequestResult = RequestResults.PasswordError
                Finally
                    If Not XeoraFileStream Is Nothing Then XeoraFileStream.Close() : GC.SuppressFinalize(XeoraFileStream)

                    If Not GZipStream Is Nothing Then GZipStream.Close() : GC.SuppressFinalize(GZipStream)
                    If Not GZipHelperStream Is Nothing Then GZipHelperStream.Close() : GC.SuppressFinalize(GZipHelperStream)
                End Try

QUICKEXIT:
                OutputStream.Seek(0, IO.SeekOrigin.Begin)

                Return rRequestResult
            End Function

            Public Sub ClearCache()
                SyncLock XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.SyncRoot
                    Dim Keys As Array = Array.CreateInstance(GetType(Object), XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Keys.Count)
                    XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Keys.CopyTo(Keys, 0)

                    For Each Key As Object In Keys
                        Dim Key_s As String = CType(Key, String)

                        If Key_s.IndexOf(String.Format("{0}$", Me._XeoraDomainFileLocation)) = 0 Then _
                            XeoraDomainDecompiler._XeoraDomainFileStreamBytesCache.Remove(Key_s)
                    Next
                End SyncLock

                XeoraDomainDecompiler._XeoraDomainFilesListCache.TryRemove(Me._XeoraDomainFileLocation, Nothing)
                XeoraDomainDecompiler._XeoraDomainFileLastModifiedDate.TryRemove(Me._XeoraDomainFileLocation, Nothing)
            End Sub
        End Class
#End Region
    End Class
End Namespace