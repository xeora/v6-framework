Option Strict On

Namespace Xeora.Web.Exception
    Public Class MultipleBlockException
        Inherits System.Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub
    End Class
End Namespace