Option Strict On

Namespace Xeora.Web.Exception
    Public Class DeploymentException
        Inherits System.Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

        Public Sub New(ByVal Message As String, ByVal InnerException As System.Exception)
            MyBase.New(Message, InnerException)
        End Sub
    End Class
End Namespace