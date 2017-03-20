Option Strict On

Namespace Xeora.Web.Exception
    Public Class ReloadRequiredException
        Inherits IO.FileNotFoundException

        Public Sub New(ByVal ApplicationPath As String)
            MyBase.New("Application cache has been corrupted, Reload Required!", ApplicationPath)
        End Sub
    End Class
End Namespace