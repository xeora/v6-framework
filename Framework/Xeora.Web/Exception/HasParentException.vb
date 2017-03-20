Option Strict On

Namespace Xeora.Web.Exception
    Public Class HasParentException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Controller does not accept Parent!")
        End Sub
    End Class
End Namespace