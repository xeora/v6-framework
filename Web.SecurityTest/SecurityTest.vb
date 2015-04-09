Namespace WebDynamics
    Public Class SecurityTest
        Implements SolidDevelopment.Web.PGlobals.PlugInMarkers.IAddon

        Public Class GlobalControl
            Public Shared Function TryOut(ByVal Text As Object) As String
                Return CType(Text, String)
            End Function

            Public Shared Function Redirect(ByVal url As Object) As SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder
                Return New SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder(url.ToString())
            End Function

            Public Shared Function DoLogin(ByVal UserName As String) As String
                Return UserName
            End Function
        End Class

        Public Class SecurityControl
            Public Shared Function CheckSecurity(ByVal ControlID As String, ByVal UserMail As String) As SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult
                Dim rSCR As New SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult(SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult.Results.None)
                Select Case ControlID
                    Case "SaveMail"
                        rSCR = New SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult(SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult.Results.ReadOnly)
                    Case "Username"
                        rSCR = New SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult(SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult.Results.ReadOnly)
                    Case "Login"
                        rSCR = New SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult(SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult.Results.ReadOnly)
                    Case Else
                        rSCR = New SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult(SolidDevelopment.Web.PGlobals.MapControls.SecurityControlResult.Results.ReadOnly)
                End Select

                Return rSCR
            End Function
        End Class
    End Class
End Namespace