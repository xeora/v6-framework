Option Strict On

Namespace Xeora.Web.Exception
    Public Class DirectivePointerException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Directive Pointer must be capital!")
        End Sub
    End Class
End Namespace