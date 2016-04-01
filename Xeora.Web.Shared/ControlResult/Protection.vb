Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class Protection
        Private _Result As Results

        Public Sub New(ByVal Result As Results)
            Me._Result = Result
        End Sub

        Public Enum Results As Byte
            None = 0
            [ReadOnly] = 1
            ReadWrite = 2
        End Enum

        Public ReadOnly Property Result() As Results
            Get
                Return Me._Result
            End Get
        End Property
    End Class
End Namespace