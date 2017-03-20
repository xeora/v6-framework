Option Strict On

Namespace Xeora.VSAddIn.Tools
    Public Class Compiler
        Public Event Progress(ByVal current As Integer, ByVal total As Integer)

        Private _XeoraFiles As List(Of InputXeoraFileInfo)
        Private _PasswordHash As Byte() = Nothing

        Public Sub New(ByVal Password As String)
            Me._XeoraFiles = New List(Of InputXeoraFileInfo)

            If Not String.IsNullOrEmpty(Password) Then
                Dim MD5 As New System.Security.Cryptography.MD5CryptoServiceProvider

                Me._PasswordHash = MD5.ComputeHash(
                                    System.Text.Encoding.UTF8.GetBytes(Password))
            End If
        End Sub

        Public Sub AddFile(ByVal RegistrationPath As String, ByVal FileLocation As String)
            Me._XeoraFiles.Add(
                New InputXeoraFileInfo(RegistrationPath, FileLocation))
        End Sub

        Public Sub RemoveFile(ByVal RegistrationPath As String, ByVal FileLocation As String)
            For fC As Integer = Me._XeoraFiles.Count - 1 To 0 Step -1
                If String.Compare(Me._XeoraFiles(fC).RegistrationPath, RegistrationPath) = 0 AndAlso
                    String.Compare(Me._XeoraFiles(fC).FileLocation, FileLocation) = 0 Then

                    Me._XeoraFiles.RemoveAt(fC)
                End If
            Next
        End Sub

        Public Sub CreateDomainFile(ByRef OutputStream As IO.Stream)
            If Me._XeoraFiles.Count = 0 Then Throw New Exception("File List must not be empty!")
            If OutputStream Is Nothing Then Throw New Exception("Output Stream must be defined!")

            ' 1 For File Preperation
            ' 1 For Index Creating
            ' Total 2
            RaiseEvent Progress(0, Me._XeoraFiles.Count + 2)

            ' Compiler Streams
            Dim IndexStream As New IO.MemoryStream, IndexBinaryWriter As New IO.BinaryWriter(IndexStream, System.Text.Encoding.UTF8)
            Dim tContentStream As IO.MemoryStream, RealContentStream As New IO.MemoryStream
            Dim FileStream As IO.FileStream, GZipStream As IO.Compression.GZipStream
            ' !--

            ' Helper Variables
            Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte())
            Dim rC As Integer
            Dim FI As IO.FileInfo

            Dim eC As Integer = 1
            ' !--

            For Each XFI As InputXeoraFileInfo In Me._XeoraFiles
                tContentStream = New IO.MemoryStream()

                FileStream = New IO.FileStream(XFI.FileLocation, IO.FileMode.Open, IO.FileAccess.Read)
                GZipStream = New IO.Compression.GZipStream(tContentStream, IO.Compression.CompressionMode.Compress, True)

                Do
                    rC = FileStream.Read(buffer, 0, buffer.Length)

                    If rC > 0 Then GZipStream.Write(buffer, 0, rC)
                Loop Until rC = 0

                GZipStream.Flush()
                GZipStream.Close() : GC.SuppressFinalize(GZipStream)
                FileStream.Close() : GC.SuppressFinalize(FileStream)

                ' CREATE INDEX
                FI = New IO.FileInfo(XFI.FileLocation)

                ' Write Index Info
                IndexBinaryWriter.Write(RealContentStream.Position)

                ' Write RegistrationPath
                IndexBinaryWriter.Write(XFI.RegistrationPath)

                ' Write FileName
                IndexBinaryWriter.Write(FI.Name)

                ' Write Original Size
                IndexBinaryWriter.Write(FI.Length)

                ' Write Compressed Size
                IndexBinaryWriter.Write(tContentStream.Length)

                ' Flush to Underlying Stream
                IndexBinaryWriter.Flush()

                ' !--

                ' PROTECT FILE
                If Not Me._PasswordHash Is Nothing Then
                    tContentStream.Seek(0, IO.SeekOrigin.Begin)

                    Do
                        rC = tContentStream.Read(buffer, 0, buffer.Length)

                        If rC > 0 Then
                            tContentStream.Seek(-rC, IO.SeekOrigin.Current)

                            For bC As Integer = 0 To rC - 1
                                tContentStream.WriteByte(
                                buffer(bC) Xor Me._PasswordHash(bC Mod Me._PasswordHash.Length))
                            Next
                        End If
                    Loop Until rC = 0
                End If
                ' !--

                ' WRITE CONTENT
                tContentStream.Seek(0, IO.SeekOrigin.Begin)

                Do
                    rC = tContentStream.Read(buffer, 0, buffer.Length)

                    If rC > 0 Then RealContentStream.Write(buffer, 0, rC)
                Loop Until rC = 0
                ' !--

                tContentStream.Close() : GC.SuppressFinalize(tContentStream)

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

            ' Write Content
            RealContentStream.Seek(0, IO.SeekOrigin.Begin)

            Do
                rC = RealContentStream.Read(buffer, 0, buffer.Length)

                If rC > 0 Then OutputStream.Write(buffer, 0, rC)
            Loop Until rC = 0
            ' !--

            OutputBinaryWriter.Flush()

            IndexBinaryWriter.Close() : GC.SuppressFinalize(IndexBinaryWriter)
            RealContentStream.Close() : GC.SuppressFinalize(RealContentStream)

            RaiseEvent Progress(eC + 1, Me._XeoraFiles.Count + 2)
        End Sub
    End Class
End Namespace