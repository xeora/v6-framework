Option Strict On

Namespace Xeora.Web.Global
    <Serializable()>
    Public Class xServiceSessionInfo
        Implements IEnumerable

        Private _SessionItems As Generic.List(Of DictionaryEntry)
        Private _SessionDate As Date
        Private _PublicKey As String

        Public Sub New(ByVal PublicKey As String, ByVal SessionDate As Date)
            Me._SessionItems = New Generic.List(Of DictionaryEntry)

            Me._PublicKey = PublicKey
            Me._SessionDate = SessionDate
        End Sub

        Public ReadOnly Property PublicKey() As String
            Get
                Return Me._PublicKey
            End Get
        End Property

        Public Property SessionDate() As Date
            Get
                Return Me._SessionDate
            End Get
            Set(ByVal value As Date)
                Me._SessionDate = value
            End Set
        End Property

        Public Sub AddSessionItem(ByVal Key As String, ByVal Value As Object)
            Me.RemoveSessionItem(Key)

            Me._SessionItems.Add(
                New DictionaryEntry(Key, Value))
        End Sub

        Public Sub RemoveSessionItem(ByVal Key As String)
            For iC As Integer = Me._SessionItems.Count - 1 To 0 Step -1
                If String.Compare(Me._SessionItems(iC).Key.ToString(), Key, True) = 0 Then
                    Me._SessionItems.RemoveAt(iC)
                End If
            Next
        End Sub

        Public ReadOnly Property Item(ByVal Key As String) As Object
            Get
                Dim rValue As Object = Nothing

                For Each dItem As DictionaryEntry In Me._SessionItems
                    If String.Compare(dItem.Key.ToString(), Key, True) = 0 Then
                        rValue = dItem.Value

                        Exit For
                    End If
                Next

                Return rValue
            End Get
        End Property

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me._SessionItems.GetEnumerator()
        End Function
    End Class
End Namespace