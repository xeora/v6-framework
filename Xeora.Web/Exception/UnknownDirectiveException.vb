Option Strict On

Namespace Xeora.Web.Exception
    Public Class UnknownDirectiveException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Directive pointer is not a known one!")
        End Sub
    End Class
End Namespace