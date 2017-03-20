Option Strict On

Namespace Xeora.VSAddIn.Tools
    Public Class Decompiler
        Private _XeoraFileLocation As String
        Private _PasswordHash As Byte() = Nothing

        Private _Authenticated As Boolean

        Public Sub New(ByVal XeoraFileLocation As String, ByVal Password As Byte())
            Me._XeoraFileLocation = XeoraFileLocation
            Me._PasswordHash = Password

            Me._Authenticated = False
        End Sub

        Public ReadOnly Property Authenticated() As Boolean
            Get
                Return Me._Authenticated
            End Get
        End Property

        Public ReadOnly Property XeoraFilesList() As List(Of OutputXeoraFileInfo)
            Get
                Return Me.ReadFileListInternal()
            End Get
        End Property

        Private Function ReadFileListInternal() As List(Of OutputXeoraFileInfo)
            Dim rXeoraFileInfoList As New List(Of OutputXeoraFileInfo)

            Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

            Dim XeoraFileStream As IO.FileStream, XeoraStreamBinaryReader As IO.BinaryReader = Nothing
            Try
                XeoraFileStream = New IO.FileStream(Me._XeoraFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                XeoraStreamBinaryReader = New IO.BinaryReader(XeoraFileStream, System.Text.Encoding.UTF8)

                Dim ReadC As Integer = 0
                Dim MovedIndex As Long = XeoraStreamBinaryReader.ReadInt64()

                Do
                    Index = XeoraStreamBinaryReader.ReadInt64() + MovedIndex
                    localRegistrationPath = XeoraStreamBinaryReader.ReadString()
                    localFileName = XeoraStreamBinaryReader.ReadString()
                    Length = XeoraStreamBinaryReader.ReadInt64()
                    CompressedLength = XeoraStreamBinaryReader.ReadInt64()

                    ReadC += 8 + localRegistrationPath.Length + localFileName.Length + 8 + 8

                    rXeoraFileInfoList.Add(
                    New OutputXeoraFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength))
                Loop Until ReadC = MovedIndex
            Catch ex As Exception
                ' Just Handle Exceptions
            Finally
                If Not XeoraStreamBinaryReader Is Nothing Then XeoraStreamBinaryReader.Close() : GC.SuppressFinalize(XeoraStreamBinaryReader)
            End Try

            Return rXeoraFileInfoList
        End Function

        Public Function GetXeoraFileInfo(ByVal RegistrationPath As String, ByVal FileName As String) As OutputXeoraFileInfo
            Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

            Dim XeoraFileStream As IO.FileStream, XeoraStreamBinaryReader As IO.BinaryReader = Nothing
            Dim IsFound As Boolean = False
            Try
                XeoraFileStream = New IO.FileStream(Me._XeoraFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                XeoraStreamBinaryReader = New IO.BinaryReader(XeoraFileStream, System.Text.Encoding.UTF8)

                Dim ReadC As Integer = 0
                Dim MovedIndex As Long = XeoraStreamBinaryReader.ReadInt64()

                Do
                    Index = XeoraStreamBinaryReader.ReadInt64() + MovedIndex + 8
                    localRegistrationPath = XeoraStreamBinaryReader.ReadString()
                    localFileName = XeoraStreamBinaryReader.ReadString()
                    Length = XeoraStreamBinaryReader.ReadInt64()
                    CompressedLength = XeoraStreamBinaryReader.ReadInt64()

                    ReadC += 8 + (1 + localRegistrationPath.Length) + (1 + localFileName.Length) + 8 + 8

                    If String.Compare(
                        RegistrationPath, localRegistrationPath, True) = 0 AndAlso
                        String.Compare(FileName, localFileName, True) = 0 Then

                        IsFound = True

                        Exit Do
                    End If
                Loop Until ReadC = MovedIndex
            Catch ex As Exception
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

            Return New OutputXeoraFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength)
        End Function

        Public Sub ReadFile(ByVal index As Long, ByVal length As Long, ByRef OutputStream As IO.Stream)
            If index = -1 Then Throw New Exception("Index must be specified!")
            If length < 1 Then Throw New Exception("Length must be specified!")
            If OutputStream Is Nothing Then Throw New Exception("OutputStream must be specified!")

            Dim XeoraFileStream As IO.FileStream = Nothing
            Dim GZipHelperStream As IO.MemoryStream = Nothing, GZipStream As IO.Compression.GZipStream = Nothing

            Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), length), Byte())

            Try
                XeoraFileStream = New IO.FileStream(Me._XeoraFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)

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
                Dim bC As Integer

                Do
                    bC = GZipStream.Read(rbuffer, 0, rbuffer.Length)

                    If bC > 0 Then OutputStream.Write(rbuffer, 0, bC)
                Loop While bC > 0

                Me._Authenticated = True
            Catch ex As Exception
                Me._Authenticated = False
            Finally
                If Not XeoraFileStream Is Nothing Then XeoraFileStream.Close() : GC.SuppressFinalize(XeoraFileStream)

                If Not GZipStream Is Nothing Then GZipStream.Close() : GC.SuppressFinalize(GZipStream)
                If Not GZipHelperStream Is Nothing Then GZipHelperStream.Close() : GC.SuppressFinalize(GZipHelperStream)
            End Try

            OutputStream.Seek(0, IO.SeekOrigin.Begin)
        End Sub
    End Class
End Namespace