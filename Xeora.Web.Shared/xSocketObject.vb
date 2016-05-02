Option Strict On

Namespace Xeora.Web.Shared
    <Serializable()>
    Public Class xSocketObject
        Private _InputHeaders As Specialized.NameValueCollection
        Private _InputStream As IO.Stream
        Private _OutputHeaders As Specialized.NameValueCollection
        Private _OuputStream As IO.Stream

        Private _Parameters As ParameterCollection
        Private _FlushHandler As FlushHandler

        Public Delegate Sub FlushHandler()

        Public Sub New(ByRef InputHeaders As Specialized.NameValueCollection, ByRef InputStream As IO.Stream, ByRef OutputHeaders As Specialized.NameValueCollection, ByRef OutputStream As IO.Stream, ByVal Parameters As Generic.KeyValuePair(Of String, Object)(), ByVal FlushHandler As FlushHandler)
            Me._InputHeaders = InputHeaders
            Me._InputStream = InputStream
            Me._OutputHeaders = OutputHeaders
            Me._OuputStream = OutputStream

            Me._Parameters = New ParameterCollection(Parameters)

            Me._FlushHandler = FlushHandler
        End Sub

        Public ReadOnly Property InputHeaders() As Specialized.NameValueCollection
            Get
                Return Me._InputHeaders
            End Get
        End Property

        Public ReadOnly Property InputStream() As IO.Stream
            Get
                Return Me._InputStream
            End Get
        End Property

        Public ReadOnly Property OutputHeaders() As Specialized.NameValueCollection
            Get
                Return Me._OutputHeaders
            End Get
        End Property

        Public ReadOnly Property OutputStream() As IO.Stream
            Get
                Return Me._OuputStream
            End Get
        End Property

        Public ReadOnly Property Parameters() As ParameterCollection
            Get
                Return Me._Parameters
            End Get
        End Property

        Public Sub Flush()
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
    End Class
End Namespace
