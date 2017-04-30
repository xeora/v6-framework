Option Strict On

Namespace Xeora.VSAddIn.Tools
    Public Class Compiler
        Public Event Progress(ByVal current As Integer, ByVal total As Integer)

        Private _DomainPath As String
        Private _XeoraFiles As List(Of InputXeoraFileInfo)
        Private _SecuredPasswordHash As Byte() = Nothing

        Public Sub New(ByVal DomainPath As String)
            Me._DomainPath = DomainPath
            Me._XeoraFiles = New List(Of InputXeoraFileInfo)
        End Sub

        Public ReadOnly Property PasswordHash As Byte()
            Get
                Return Me._SecuredPasswordHash
            End Get
        End Property

        Public Sub AddFile(ByVal FullFilePath As String)
            Me._XeoraFiles.Add(
                New InputXeoraFileInfo(Me._DomainPath, FullFilePath))
        End Sub

        Public Sub RemoveFile(ByVal FullFilePath As String)
            Dim InputXeoraFileInfo As New InputXeoraFileInfo(Me._DomainPath, FullFilePath)

            For XFC As Integer = Me._XeoraFiles.Count - 1 To 0 Step -1
                If Me._XeoraFiles.Item(XFC).GetHashCode() = InputXeoraFileInfo.GetHashCode() Then
                    Me._XeoraFiles.RemoveAt(XFC)

                    Exit For
                End If
            Next
        End Sub

        Public Overloads Sub CreateDomainFile(ByRef OutputStream As IO.Stream)
            Me.CreateDomainFile(Nothing, OutputStream)
        End Sub

        Public Overloads Sub CreateDomainFile(ByVal Password As String, ByRef OutputStream As IO.Stream)
            If Me._XeoraFiles.Count = 0 Then Throw New Exception("File List must not be empty!")
            If OutputStream Is Nothing Then Throw New Exception("Output Stream must be defined!")

            Dim MD5 As System.Security.Cryptography.MD5CryptoServiceProvider = Nothing
            Dim PasswordHash As Byte() = Nothing

            If Not String.IsNullOrEmpty(Password) Then
                MD5 = New System.Security.Cryptography.MD5CryptoServiceProvider
                PasswordHash = MD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password))
            End If

            ' 1 For File Preperation
            ' 1 For Index Creating
            ' Total 2
            RaiseEvent Progress(0, Me._XeoraFiles.Count + 2)

            ' Compiler Streams
            Dim ContentPartStream As IO.Stream, ContentPartFileStream As IO.FileStream, GZipStream As IO.Compression.GZipStream

            Dim IndexStream As New IO.MemoryStream, IndexBinaryWriter As New IO.BinaryWriter(IndexStream, Text.Encoding.UTF8)
            Dim ContentStream As New IO.MemoryStream
            ' !--

            ' Helper Variables
            Dim rC As Integer, buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte())

            Dim eC As Integer = 1
            ' !--

            For Each XFI As InputXeoraFileInfo In Me._XeoraFiles
                ContentPartStream = New IO.MemoryStream()

                ContentPartFileStream = New IO.FileStream(XFI.FullFilePath, IO.FileMode.Open, IO.FileAccess.Read)
                GZipStream = New IO.Compression.GZipStream(ContentPartStream, IO.Compression.CompressionMode.Compress, True)

                Do
                    rC = ContentPartFileStream.Read(buffer, 0, buffer.Length)

                    If rC > 0 Then GZipStream.Write(buffer, 0, rC)
                Loop Until rC = 0

                GZipStream.Flush() : GZipStream.Close() : GC.SuppressFinalize(GZipStream)
                ContentPartFileStream.Close() : GC.SuppressFinalize(ContentPartFileStream)

                ' CREATE INDEX
                ' Write Index Info
                IndexBinaryWriter.Write(ContentStream.Position)

                ' Write RegistrationPath
                IndexBinaryWriter.Write(XFI.RegistrationPath)

                ' Write FileName
                IndexBinaryWriter.Write(XFI.FileName)

                ' Write Original Size
                IndexBinaryWriter.Write(XFI.FileSize)

                ' Write Compressed Size
                IndexBinaryWriter.Write(ContentPartStream.Length)

                ' Flush to Underlying Stream
                IndexBinaryWriter.Flush()

                ' !--

                ' PROTECT FILE
                If Not PasswordHash Is Nothing Then
                    ContentPartStream.Seek(0, IO.SeekOrigin.Begin)

                    Dim LastIndex As Integer = 0
                    Do
                        rC = ContentPartStream.Read(buffer, 0, buffer.Length)

                        If rC > 0 Then
                            ContentPartStream.Seek(-rC, IO.SeekOrigin.Current)

                            For bC As Integer = 0 To rC - 1
                                ContentPartStream.WriteByte(
                                    buffer(bC) Xor PasswordHash((bC + LastIndex) Mod PasswordHash.Length))
                            Next

                            LastIndex += rC
                        End If
                    Loop Until rC = 0
                End If
                ' !--

                ' WRITE CONTENT
                ContentPartStream.Seek(0, IO.SeekOrigin.Begin)
                Do
                    rC = ContentPartStream.Read(buffer, 0, buffer.Length)

                    If rC > 0 Then ContentStream.Write(buffer, 0, rC)
                Loop Until rC = 0
                ' !--

                ContentPartStream.Close() : GC.SuppressFinalize(ContentPartStream)

                RaiseEvent Progress(eC, Me._XeoraFiles.Count + 2)

                eC += 1
            Next

            Dim OutputBinaryWriter As New IO.BinaryWriter(OutputStream)

            ' Write Index Length
            OutputBinaryWriter.Write(IndexStream.Position)

            ' Write Index Content
            IndexStream.Seek(0, IO.SeekOrigin.Begin)
            Do
                rC = IndexStream.Read(buffer, 0, buffer.Length)

                If rC > 0 Then OutputStream.Write(buffer, 0, rC)
            Loop Until rC = 0
            ' !--

            OutputBinaryWriter.Flush()

            ' Write Content
            ContentStream.Seek(0, IO.SeekOrigin.Begin)
            Do
                rC = ContentStream.Read(buffer, 0, buffer.Length)

                If rC > 0 Then OutputStream.Write(buffer, 0, rC)
            Loop Until rC = 0
            ' !--

            IndexBinaryWriter.Close() : GC.SuppressFinalize(IndexBinaryWriter)
            ContentStream.Close() : GC.SuppressFinalize(ContentStream)

            RaiseEvent Progress(eC + 1, Me._XeoraFiles.Count + 2)

            If Not PasswordHash Is Nothing Then
                OutputStream.Flush()
                OutputStream.Seek(0, IO.SeekOrigin.Begin)

                Dim FileHash As Byte() = MD5.ComputeHash(OutputStream)
                Me._SecuredPasswordHash = New Byte(FileHash.Length - 1) {}

                For hC As Integer = 0 To FileHash.Length - 1
                    Me._SecuredPasswordHash(hC) = FileHash(hC) Xor PasswordHash(hC)
                Next
            End If
        End Sub
    End Class
End Namespace