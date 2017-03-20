Option Strict On

Namespace Xeora.Web.Exception
    Public Class EmptyBlockException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Empty Block is not allowed!")
        End Sub
    End Class
End Namespace