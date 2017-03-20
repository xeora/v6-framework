Namespace WebDynamics
    Public Class SecurityTest
        Implements Xeora.Web.Shared.IDomainExecutable

        Public Class GlobalControl
            Public Shared Function TryOut(ByVal Text As Object) As String
                Return CType(Text, String)
            End Function

            Public Shared Function Redirect(ByVal url As Object) As Xeora.Web.Shared.ControlResult.RedirectOrder
                Return New Xeora.Web.Shared.ControlResult.RedirectOrder(url.ToString())
            End Function

            Public Shared Function DoLogin(ByVal UserName As String) As String
                Return UserName
            End Function
        End Class

        Public Class SecurityControl
            Public Shared Function CheckSecurity(ByVal ControlID As String, ByVal UserMail As String) As Xeora.Web.Shared.ControlResult.Protection
                Dim rSCR As New Xeora.Web.Shared.ControlResult.Protection(Xeora.Web.Shared.ControlResult.Protection.Results.None)
                Select Case ControlID
                    Case "SaveMail"
                        rSCR = New Xeora.Web.Shared.ControlResult.Protection(Xeora.Web.Shared.ControlResult.Protection.Results.ReadOnly)
                    Case "Username"
                        rSCR = New Xeora.Web.Shared.ControlResult.Protection(Xeora.Web.Shared.ControlResult.Protection.Results.ReadOnly)
                    Case "Login"
                        rSCR = New Xeora.Web.Shared.ControlResult.Protection(Xeora.Web.Shared.ControlResult.Protection.Results.ReadOnly)
                    Case Else
                        rSCR = New Xeora.Web.Shared.ControlResult.Protection(Xeora.Web.Shared.ControlResult.Protection.Results.ReadOnly)
                End Select

                Return rSCR
            End Function
        End Class
    End Class
End Namespace