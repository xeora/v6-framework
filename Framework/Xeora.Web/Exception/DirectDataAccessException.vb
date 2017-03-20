Option Strict On

Namespace Xeora.Web.Exception
    Public Class DirectDataAccessException
        Inherits System.Exception

        Public Sub New(ByVal InnerException As System.Exception)
            MyBase.New("DirectDataAccess failed!", InnerException)
        End Sub
    End Class
End Namespace