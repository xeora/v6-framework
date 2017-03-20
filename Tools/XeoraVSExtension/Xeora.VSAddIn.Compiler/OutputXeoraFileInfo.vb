Namespace Xeora.VSAddIn.Tools
    Public Class OutputXeoraFileInfo
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
End Namespace