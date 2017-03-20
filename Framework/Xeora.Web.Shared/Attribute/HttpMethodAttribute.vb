Option Strict On

Namespace Xeora.Web.Shared.Attribute
    Public Class HttpMethodAttribute
        Inherits System.Attribute

        Public Enum Methods
            [GET]
            POST
            PUT
            DELETE
        End Enum

        Private _Method As Methods
        Private _BindProcedureName As String

        Public Sub New()
            Me.New(Methods.GET, String.Empty)
        End Sub

        Public Sub New(ByVal Method As Methods)
            Me.New(Method, String.Empty)
        End Sub

        Public Sub New(ByVal Method As Methods, ByVal BindProcedureName As String)
            Me._Method = Method

            If BindProcedureName Is Nothing Then BindProcedureName = String.Empty
            Me._BindProcedureName = BindProcedureName
        End Sub

        Public ReadOnly Property Method As Methods
            Get
                Return Me._Method
            End Get
        End Property

        Public ReadOnly Property BindProcedureName As String
            Get
                Return Me._BindProcedureName
            End Get
        End Property
    End Class
End Namespace