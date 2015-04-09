Option Strict On

Public Class Decompiler
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

    Private _Authenticated As Boolean

    Public Sub New(ByVal swctFileLocation As String, ByVal Password As Byte())
        Me._swctFileLocation = swctFileLocation
        Me._PasswordHash = Password

        Me._Authenticated = False
    End Sub

    Public ReadOnly Property Authenticated() As Boolean
        Get
            Return Me._Authenticated
        End Get
    End Property

    Public ReadOnly Property swctFilesList() As System.Collections.Generic.List(Of swctFileInfo)
        Get
            Return Me.ReadFileListInternal()
        End Get
    End Property

    Private Function ReadFileListInternal() As System.Collections.Generic.List(Of swctFileInfo)
        Dim rSWCTFileInfo As New System.Collections.Generic.List(Of swctFileInfo)

        Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

        Dim swctFileStream As IO.FileStream, swctStreamBinaryReader As IO.BinaryReader = Nothing
        Try
            swctFileStream = New IO.FileStream(Me._swctFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            swctStreamBinaryReader = New IO.BinaryReader(swctFileStream, System.Text.Encoding.UTF8)

            Dim ReadC As Integer = 0
            Dim MovedIndex As Long = swctStreamBinaryReader.ReadInt64()

            Do
                Index = swctStreamBinaryReader.ReadInt64() + MovedIndex
                localRegistrationPath = swctStreamBinaryReader.ReadString()
                localFileName = swctStreamBinaryReader.ReadString()
                Length = swctStreamBinaryReader.ReadInt64()
                CompressedLength = swctStreamBinaryReader.ReadInt64()

                ReadC += 8 + localRegistrationPath.Length + localFileName.Length + 8 + 8

                rSWCTFileInfo.Add( _
                    New swctFileInfo(Index, localRegistrationPath, localFileName, Length, CompressedLength))
            Loop Until ReadC = MovedIndex
        Catch ex As Exception
            ' Just Handle Exceptions
        Finally
            If Not swctStreamBinaryReader Is Nothing Then swctStreamBinaryReader.Close() : GC.SuppressFinalize(swctStreamBinaryReader)
        End Try

        Return rSWCTFileInfo
    End Function

    Public Function GetswctFileInfo(ByVal RegistrationPath As String, ByVal FileName As String) As swctFileInfo
        Dim Index As Long = -1, localRegistrationPath As String = Nothing, localFileName As String = Nothing, Length As Long = -1, CompressedLength As Long = -1

        Dim swctFileStream As IO.FileStream, swctStreamBinaryReader As IO.BinaryReader = Nothing
        Dim IsFound As Boolean = False
        Try
            swctFileStream = New IO.FileStream(Me._swctFileLocation, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            swctStreamBinaryReader = New IO.BinaryReader(swctFileStream, System.Text.Encoding.UTF8)

            Dim ReadC As Integer = 0
            Dim MovedIndex As Long = swctStreamBinaryReader.ReadInt64()

            Do
                Index = swctStreamBinaryReader.ReadInt64() + MovedIndex + 8
                localRegistrationPath = swctStreamBinaryReader.ReadString()
                localFileName = swctStreamBinaryReader.ReadString()
                Length = swctStreamBinaryReader.ReadInt64()
                CompressedLength = swctStreamBinaryReader.ReadInt64()

                ReadC += 8 + (1 + localRegistrationPath.Length) + (1 + localFileName.Length) + 8 + 8

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

    Public Sub ReadFile(ByVal index As Long, ByVal length As Long, ByRef OutputStream As IO.Stream)
        If index = -1 Then Throw New Exception("Index must be specified!")
        If length < 1 Then Throw New Exception("Length must be specified!")
        If OutputStream Is Nothing Then Throw New Exception("OutputStream must be specified!")

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
            Dim bC As Integer

            Do
                bC = GZipStream.Read(rbuffer, 0, rbuffer.Length)

                If bC > 0 Then OutputStream.Write(rbuffer, 0, bC)
            Loop While bC > 0

            Me._Authenticated = True
        Catch ex As Exception
            Me._Authenticated = False
        Finally
            If Not swctFileStream Is Nothing Then swctFileStream.Close() : GC.SuppressFinalize(swctFileStream)

            If Not GZipStream Is Nothing Then GZipStream.Close() : GC.SuppressFinalize(GZipStream)
            If Not GZipHelperStream Is Nothing Then GZipHelperStream.Close() : GC.SuppressFinalize(GZipHelperStream)
        End Try

        OutputStream.Seek(0, IO.SeekOrigin.Begin)
    End Sub
End Class
