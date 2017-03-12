Option Strict On

Namespace Xeora.Web.Controller.Directive.Control
    Public Interface IControl
        Inherits INamable

        Property SecurityInfo() As ControlBase.SecurityInfo
        ReadOnly Property Type() As ControlBase.ControlTypes
        Property BindInfo() As [Shared].Execution.BindInfo
        ReadOnly Property Attributes() As AttributeInfo.AttributeInfoCollection
        Sub Clone(ByRef Control As IControl)

        Event ControlResolveRequested(ByVal ControlID As String, ByRef WorkingInstance As [Shared].IDomain, ByRef ResultDictionary As Generic.Dictionary(Of String, Object))
    End Interface
End Namespace