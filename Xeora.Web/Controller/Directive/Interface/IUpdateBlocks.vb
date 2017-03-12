Option Strict On

Namespace Xeora.Web.Controller.Directive
    Public Interface IUpdateBlocks
        ReadOnly Property BlockIDsToUpdate() As Generic.List(Of String)
        Property UpdateLocalBlock() As Boolean
    End Interface
End Namespace