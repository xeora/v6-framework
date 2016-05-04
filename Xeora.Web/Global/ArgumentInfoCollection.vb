Option Strict On

Namespace Xeora.Web.Global
    Public Class ArgumentInfoCollection
        Private _ArgumentInfoIndexes As Generic.Dictionary(Of String, Integer)
        Private _ValueList As Object()

        Public Sub New()
            Me._ArgumentInfoIndexes = New Generic.Dictionary(Of String, Integer)(StringComparer.InvariantCultureIgnoreCase)
            Me._ValueList = New Object() {}
        End Sub

        Public Overloads Sub Reset()
            Me._ArgumentInfoIndexes.Clear()
            Me._ValueList = New Object() {}
        End Sub

        Public Overloads Sub Reset(ByVal Keys As String())
            Me._ArgumentInfoIndexes.Clear()
            Me._ValueList = New Object() {}

            If Not Keys Is Nothing Then
                For Each Key As String In Keys
                    Me.AppendKey(Key)
                Next
            End If
        End Sub

        Public Overloads Sub Reset(ByVal Values As Object())
            Dim Length As Integer = Me._ValueList.Length

            Me._ValueList = New Object(Length - 1) {}

            If Not Values Is Nothing Then
                If Values.Length <> Length Then
                    Throw New ArgumentOutOfRangeException(SystemMessages.ARGUMENT_KEYVALUELENGTHMATCH)
                Else
                    Me._ValueList = Values
                End If
            End If
        End Sub

        Public Sub Replace(ByVal AIC As ArgumentInfoCollection)
            If Not AIC Is Nothing Then
                Me._ArgumentInfoIndexes = AIC._ArgumentInfoIndexes
                Me._ValueList = AIC._ValueList
            End If
        End Sub

        Public Sub AppendKey(ByVal Key As String)
            If Not String.IsNullOrEmpty(Key) AndAlso
                Not Me._ArgumentInfoIndexes.ContainsKey(Key) Then

                ' Add Key
                Me._ArgumentInfoIndexes.Add(Key, Me._ArgumentInfoIndexes.Count)

                ' Add Dummy Value
                Array.Resize(Of Object)(Me._ValueList, Me._ValueList.Length + 1)
            End If
        End Sub

        Public Sub AppendKeyWithValue(ByVal Key As String, ByVal Value As Object)
            If Not String.IsNullOrEmpty(Key) AndAlso
                Not Me._ArgumentInfoIndexes.ContainsKey(Key) Then

                ' Add Key
                Me._ArgumentInfoIndexes.Add(Key, Me._ArgumentInfoIndexes.Count)

                ' Add Value
                Array.Resize(Of Object)(Me._ValueList, Me._ValueList.Length + 1)
                Me._ValueList(Me._ValueList.Length - 1) = Value
            Else
                Throw New ArgumentException(SystemMessages.ARGUMENT_EXISTS)
            End If
        End Sub

        Public Property Item(ByVal Key As String) As Object
            Get
                Dim rValue As Object = Nothing

                If Not String.IsNullOrEmpty(Key) AndAlso
                    Me._ArgumentInfoIndexes.ContainsKey(Key) Then

                    Dim Index As Integer = Me._ArgumentInfoIndexes.Item(Key)

                    If Index < Me._ValueList.Length Then _
                        rValue = Me._ValueList(Index)
                End If

                Return rValue
            End Get
            Set(ByVal value As Object)
                If Not String.IsNullOrEmpty(Key) AndAlso
                    Me._ArgumentInfoIndexes.ContainsKey(Key) Then

                    Dim Index As Integer = Me._ArgumentInfoIndexes.Item(Key)

                    Me._ValueList(Index) = value
                Else
                    Throw New ArgumentException(SystemMessages.ARGUMENT_NOTEXISTS)
                End If
            End Set
        End Property

        Public ReadOnly Property Count As Integer
            Get
                Return Me._ArgumentInfoIndexes.Count
            End Get
        End Property
    End Class
End Namespace