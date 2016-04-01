Option Strict On

Namespace Xeora.Web.Controller.Directive.Control
    Public Interface IControl
        Inherits INamable

        Property SecurityInfo() As ControlBase.SecurityInfos
        ReadOnly Property Type() As ControlBase.ControlTypes
        Property BindInfo() As [Shared].Execution.BindInfo
        ReadOnly Property Attributes() As AttributeInfo.AttributeInfoCollection
        ReadOnly Property BlockIDsToUpdate() As Generic.List(Of String)
        Property UpdateLocalBlock() As Boolean
        Sub Clone(ByRef Control As IControl)

        Event ControlMapNavigatorRequested(ByRef WorkingInstance As [Shared].IDomain, ByRef ControlMapXPathNavigator As Xml.XPath.XPathNavigator)
    End Interface
End Namespace