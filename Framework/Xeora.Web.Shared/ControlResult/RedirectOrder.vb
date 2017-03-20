Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class RedirectOrder
        Private _Location As String

        Public Sub New(ByVal Location As String)
            Me._Location = Location
        End Sub

        Public ReadOnly Property Location() As String
            Get
                Return Me._Location
            End Get
        End Property
    End Class
End Namespace