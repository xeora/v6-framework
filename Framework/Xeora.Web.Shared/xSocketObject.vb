Option Strict On

Namespace Xeora.Web.Shared
    <Serializable()>
    Public Class xSocketObject
        Private _InputParameters As EndPoint
        Private _OutputParameters As EndPoint
        Private _Parameters As ParameterCollection

        Private _IsFlushed As Boolean
        Private _FlushHandler As FlushHandler

        Public Delegate Sub FlushHandler()

        Public Sub New(ByRef InputHeaders As Specialized.NameValueCollection, ByRef InputStream As IO.Stream, ByRef OutputHeaders As Specialized.NameValueCollection, ByRef OutputStream As IO.Stream, ByVal Parameters As Generic.KeyValuePair(Of String, Object)(), ByVal FlushHandler As FlushHandler)
            Me._InputParameters = New EndPoint(InputHeaders, InputStream)
            Me._OutputParameters = New EndPoint(OutputHeaders, OutputStream)
            Me._Parameters = New ParameterCollection(Parameters)

            Me._IsFlushed = False
            Me._FlushHandler = FlushHandler
        End Sub

        Public ReadOnly Property Input() As EndPoint
            Get
                Return Me._InputParameters
            End Get
        End Property

        Public ReadOnly Property Output() As EndPoint
            Get
                Return Me._OutputParameters
            End Get
        End Property

        Public ReadOnly Property Parameters() As ParameterCollection
            Get
                Return Me._Parameters
            End Get
        End Property

        Public Sub Flush()
            If Me._IsFlushed Then Return
            Me._IsFlushed = True

            If Not Me._FlushHandler Is Nothing Then Me._FlushHandler.Invoke()
        End Sub

        <Serializable()>
        Public Class ParameterCollection
            Private _Parameters As Generic.Dictionary(Of String, Object)

            Friend Sub New(ByVal Parameters As Generic.KeyValuePair(Of String, Object)())
                Me._Parameters = New Generic.Dictionary(Of String, Object)(StringComparer.InvariantCultureIgnoreCase)

                If Not Parameters Is Nothing Then
                    For Each Item As Generic.KeyValuePair(Of String, Object) In Parameters
                        If Not Me._Parameters.ContainsKey(Item.Key) Then _
                            Me._Parameters.Add(Item.Key, Item.Value)
                    Next
                End If
            End Sub

            Public ReadOnly Property Item(ByVal key As String) As Object
                Get
                    Dim rValue As Object = Nothing

                    If Me._Parameters.ContainsKey(key) Then _
                        rValue = Me._Parameters.Item(key)

                    Return rValue
                End Get
            End Property
        End Class

        <Serializable()>
        Public Class EndPoint
            Private _Header As Specialized.NameValueCollection
            Private _Stream As IO.Stream

            Public Sub New(ByRef Header As Specialized.NameValueCollection, ByRef Stream As IO.Stream)
                Me._Header = Header
                Me._Stream = Stream
            End Sub

            Public ReadOnly Property Header() As Specialized.NameValueCollection
                Get
                    Return Me._Header
                End Get
            End Property

            Public ReadOnly Property Stream() As IO.Stream
                Get
                    Return Me._Stream
                End Get
            End Property
        End Class
    End Class
End Namespace
