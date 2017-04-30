Namespace Xeora.VSAddIn.Tools
    Public Class InputXeoraFileInfo
        Public Sub New(ByVal DomainPath As String, ByVal FullFilePath As String)
            Dim FI As New IO.FileInfo(FullFilePath)

            Me.RegistrationPath = FI.FullName
            Me.RegistrationPath = Me.RegistrationPath.Replace(DomainPath, String.Empty)
            Me.RegistrationPath = Me.RegistrationPath.Replace(FI.Name, String.Empty)
            Me.FileName = FI.Name

            Me.FullFilePath = FullFilePath
            Me.FileSize = FI.Length
        End Sub

        Public ReadOnly Property RegistrationPath As String
        Public ReadOnly Property FileName As String
        Public ReadOnly Property FullFilePath As String
        Public ReadOnly Property FileSize As Long
    End Class
End Namespace