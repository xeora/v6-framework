Option Strict On

Namespace Xeora.Web.Exception
    Public Class ExecutionException
        Inherits System.Exception

        Public Sub New()
            MyBase.New("Execution failed!")
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(String.Format("Execution failed! - {0}", Message))
        End Sub

        Public Sub New(ByVal Message As String, ByVal InnerException As System.Exception)
            MyBase.New(String.Format("Execution failed! - {0}", Message), InnerException)
        End Sub
    End Class
End Namespace