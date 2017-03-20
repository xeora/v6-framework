Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Interface IParsingRequires
        Event ParseRequested(ByVal DraftValue As String, ByRef ContainerController As ControllerBase)
    End Interface
End Namespace
