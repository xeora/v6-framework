Option Strict On

Namespace Xeora.Web.Exception
    Public Class RequestBlockException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Request Block must not be placed inside in another Request Block")
        End Sub
    End Class
End Namespace