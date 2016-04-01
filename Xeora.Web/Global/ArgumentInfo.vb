Option Strict On

Namespace Xeora.Web.Global
    Public Class ArgumentInfo
        Private _Key As String
        Private _Value As Object

        Public Sub New(ByVal Key As String, Optional ByVal Value As Object = Nothing)
            Me._Key = Key
            Me._Value = Value
        End Sub

        Public ReadOnly Property Key() As String
            Get
                Return Me._Key
            End Get
        End Property

        Public ReadOnly Property Value() As Object
            Get
                Return Me._Value
            End Get
        End Property

        Public Class ArgumentInfoCollection
            Implements IEnumerable
            Implements ICloneable

            Private _ArgumentInfos As Hashtable

            Public Sub New()
                Me.New(Nothing)
            End Sub

            Public Sub New(ByVal Parent As ArgumentInfoCollection)
                Me._ArgumentInfos = New Hashtable
            End Sub

            Public Shadows Sub Add(ByVal key As String, ByVal value As Object)
                Me._ArgumentInfos.Item(key) = New ArgumentInfo(key, value)
            End Sub

            Public Shadows Sub Add(ByVal item As ArgumentInfo)
                Me._ArgumentInfos.Item(item.Key) = item
            End Sub

            Public Shadows Sub Remove(ByVal key As String)
                If Me._ArgumentInfos.ContainsKey(key) Then Me._ArgumentInfos.Remove(key)
            End Sub

            Public Shadows Sub Remove(ByVal value As ArgumentInfo)
                Me.Remove(value.Key)
            End Sub

            Public Property Item(ByVal key As String) As ArgumentInfo
                Get
                    Dim rArgumentInfo As New ArgumentInfo(key, Nothing)

                    If Me._ArgumentInfos.ContainsKey(key) Then rArgumentInfo = CType(Me._ArgumentInfos.Item(key), ArgumentInfo)

                    Return rArgumentInfo
                End Get
                Set(ByVal value As ArgumentInfo)
                    Me._ArgumentInfos.Item(value.Key) = value
                End Set
            End Property

            Public Function ContainsKey(ByVal key As String) As Boolean
                Return Me._ArgumentInfos.ContainsKey(key)
            End Function

            Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return Me._ArgumentInfos.GetEnumerator()
            End Function

            Public Shared Function Combine(ByVal Arguments1 As ArgumentInfoCollection, ByVal Arguments2 As ArgumentInfoCollection) As ArgumentInfoCollection
                Dim rArguments As ArgumentInfoCollection =
                    New ArgumentInfoCollection()
                Dim argEnumerator As System.Collections.IEnumerator

                If Not Arguments1 Is Nothing Then
                    argEnumerator = Arguments1.GetEnumerator()
                    Do While argEnumerator.MoveNext()
                        Dim currentEnumItem As DictionaryEntry =
                                CType(argEnumerator.Current, DictionaryEntry)

                        rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                    Loop
                End If

                If Not Arguments2 Is Nothing Then
                    argEnumerator = Arguments2.GetEnumerator()
                    Do While argEnumerator.MoveNext()
                        Dim currentEnumItem As DictionaryEntry =
                                CType(argEnumerator.Current, DictionaryEntry)

                        rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                    Loop
                End If

                Return rArguments
            End Function

            Public Overloads Function Clone() As Object Implements ICloneable.Clone
                Dim rArguments As New ArgumentInfoCollection
                Dim argEnumerator As IEnumerator

                argEnumerator = Me.GetEnumerator()
                Do While argEnumerator.MoveNext()
                    Dim currentEnumItem As DictionaryEntry =
                            CType(argEnumerator.Current, DictionaryEntry)

                    rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                Loop

                Return rArguments
            End Function
        End Class
    End Class
End Namespace