Namespace Xeora.VSAddIn.Tools
    Public Class InputXeoraFileInfo
        Private _RegistrationPath As String
        Private _FileLocation As String

        Public Sub New(ByVal RegistrationPath As String, ByVal FileLocation As String)
            Me._RegistrationPath = RegistrationPath
            Me._FileLocation = FileLocation
        End Sub

        Public Property RegistrationPath() As String
            Get
                Return Me._RegistrationPath
            End Get
            Set(ByVal value As String)
                Me._RegistrationPath = value
            End Set
        End Property

        Public Property FileLocation() As String
            Get
                Return Me._FileLocation
            End Get
            Set(ByVal value As String)
                Me._FileLocation = value
            End Set
        End Property
    End Class
End Namespace