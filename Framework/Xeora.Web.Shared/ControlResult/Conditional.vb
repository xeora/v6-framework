Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class Conditional
        Private _Result As Conditions

        Public Sub New(ByVal Result As Conditions)
            Me._Result = Result
        End Sub

        Public Enum Conditions As Byte
            [False] = 0
            [True] = 1
            [UnKnown] = 99
        End Enum

        Public ReadOnly Property Result() As Conditions
            Get
                Return Me._Result
            End Get
        End Property
    End Class
End Namespace