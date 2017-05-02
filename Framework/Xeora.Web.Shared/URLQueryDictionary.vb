Option Strict On

Namespace Xeora.Web.Shared
    <Serializable()>
    Public Class URLQueryDictionary
        Inherits Generic.Dictionary(Of String, String)

        Public Overloads Shared Function ResolveQueryItems() As URLQueryDictionary
            Return URLQueryDictionary.ResolveQueryItems(
                Helpers.Context.Request.URL.QueryString)
        End Function

        Public Overloads Shared Function ResolveQueryItems(ByVal QueryString As String) As URLQueryDictionary
            Dim URLQueryDictionary As New URLQueryDictionary()

            If Not String.IsNullOrEmpty(QueryString) Then
                Dim Key As String, Value As String
                For Each QueryStringItem As String In QueryString.Split("&"c)
                    Dim SplittedQueryStringItem As String() =
                        QueryStringItem.Split("="c)

                    Key = SplittedQueryStringItem(0)
                    Value = String.Join("=", SplittedQueryStringItem, 1, SplittedQueryStringItem.Length - 1)

                    If Not String.IsNullOrEmpty(Key) AndAlso Not String.IsNullOrEmpty(Value) Then _
                        URLQueryDictionary.Item(Key) = Value
                Next
            End If

            Return URLQueryDictionary
        End Function

        Public Shared Function Make(ByVal ParamArray QueryStrings As Generic.KeyValuePair(Of String, String)()) As URLQueryDictionary
            Dim URLQueryDictionary As New URLQueryDictionary()

            If Not QueryStrings Is Nothing Then
                For Each Item As Generic.KeyValuePair(Of String, String) In QueryStrings
                    If Not Item.Equals(Nothing) AndAlso Not String.IsNullOrEmpty(Item.Key) AndAlso Not String.IsNullOrEmpty(Item.Value) Then _
                        URLQueryDictionary.Item(Item.Key) = Item.Value
                Next
            End If

            Return URLQueryDictionary
        End Function

        Public Shadows Function ToString() As String
            Dim rSB As New Text.StringBuilder()

            Dim Enumerator As Generic.IEnumerator(Of Generic.KeyValuePair(Of String, String)) =
                Me.GetEnumerator()

            Do While (Enumerator.MoveNext())
                If rSB.Length > 0 Then rSB.Append("&")

                rSB.AppendFormat("{0}={1}", Enumerator.Current.Key, Enumerator.Current.Value)
            Loop

            Return rSB.ToString()
        End Function
    End Class
End Namespace