Option Strict On

Namespace Xeora.Web.Shared.ControlResult
    <Serializable()>
    Public Class VariableBlock
        Inherits Generic.Dictionary(Of String, Object)

        Public Sub New()
            MyBase.New(StringComparer.InvariantCultureIgnoreCase)
        End Sub

        Public Shadows Sub Add(ByVal key As String, ByVal value As Object)
            If Me.ContainsKey(key) Then
                MyBase.Item(key) = value
            Else
                MyBase.Add(key, value)
            End If
        End Sub

        Public Shadows Property Item(ByVal key As String) As Object
            Get
                Dim rValue As Object = Nothing

                If Me.ContainsKey(key) Then _
                    rValue = MyBase.Item(key)

                Return rValue
            End Get
            Set(ByVal value As Object)
                Me.Add(key, value)
            End Set
        End Property
    End Class
End Namespace