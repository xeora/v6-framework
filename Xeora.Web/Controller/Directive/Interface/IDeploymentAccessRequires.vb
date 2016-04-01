Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Interface IDeploymentAccessRequires
        Event DeploymentAccessRequested(ByRef DomainDeployment As Deployment.DomainDeployment)
    End Interface
End Namespace
