Namespace SolidDevelopment.Web
    Public Class RequestHandlerFactory
        Implements System.Web.IHttpHandlerFactory

        Public Function GetHandler(ByVal context As System.Web.HttpContext, ByVal requestType As String, ByVal url As String, ByVal pathTranslated As String) As System.Web.IHttpHandler Implements System.Web.IHttpHandlerFactory.GetHandler
            Return New RequestHandler( _
                        CType(context.Items.Item("RequestID"), String))
        End Function

        Public Sub ReleaseHandler(ByVal handler As System.Web.IHttpHandler) Implements System.Web.IHttpHandlerFactory.ReleaseHandler
            CType(handler, RequestHandler).KillHandler()
        End Sub
    End Class
End Namespace