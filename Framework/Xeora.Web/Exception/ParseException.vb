Option Strict On

Namespace Xeora.Web.Exception
    Public Class ParseException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Parser is unable to complete!")
        End Sub
    End Class
End Namespace