Option Strict On

Namespace Xeora.Web.Exception
    Public Class GrammerException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Grammer has error!")
        End Sub
    End Class
End Namespace