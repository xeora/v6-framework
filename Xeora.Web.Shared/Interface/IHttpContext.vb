Option Strict On

Namespace Xeora.Web.Shared
    Public Class URL
        Private _Raw As String
        Private _Relative As String
        Private _RelativePath As String
        Private _QueryString As String

        Public Sub New(ByVal RawURL As String)
            Dim ApplicationRootPath As String = Configurations.ApplicationRoot.BrowserImplementation
            ' Fix false path request
            If String.Compare(ApplicationRootPath, "/") <> 0 AndAlso
                String.Compare(
                    String.Format("{0}/", RawURL).Substring((RawURL.Length + 1) - ApplicationRootPath.Length), ApplicationRootPath) = 0 Then _
                RawURL = String.Format("{0}/", RawURL)

            Me._Raw = RawURL

            Dim DoubleDashIndex As Integer =
                RawURL.IndexOf("//")
            If DoubleDashIndex > -1 Then RawURL = RawURL.Remove(0, DoubleDashIndex + 2)

            Dim FirstSingleDashIndex As Integer =
                RawURL.IndexOf("/"c)
            If FirstSingleDashIndex > -1 Then RawURL = RawURL.Remove(0, FirstSingleDashIndex)

            Me._Relative = RawURL

            Dim FirstQuestionMarkIndex As Integer =
                RawURL.IndexOf("?"c)
            If FirstQuestionMarkIndex > -1 Then
                Me._RelativePath = RawURL.Substring(0, FirstQuestionMarkIndex)
                Me._QueryString = RawURL.Substring(FirstQuestionMarkIndex + 1)
            Else
                Me._RelativePath = RawURL
                Me._QueryString = String.Empty
            End If
        End Sub

        Public ReadOnly Property Raw As String
            Get
                Return Me._Raw
            End Get
        End Property

        Public ReadOnly Property Relative As String
            Get
                Return Me._Relative
            End Get
        End Property

        Public ReadOnly Property RelativePath As String
            Get
                Return Me._RelativePath
            End Get
        End Property

        Public ReadOnly Property QueryString As String
            Get
                Return Me._QueryString
            End Get
        End Property
    End Class

    Public Interface IHttpContext
        ReadOnly Property UnderlyingContext As System.Web.HttpContext
        ReadOnly Property Application As IHttpApplication
        ReadOnly Property Session As IHttpSession
        ReadOnly Property Request As IHttpRequest
        ReadOnly Property Response As IHttpResponse
        ReadOnly Property Content As IHttpContextContent(Of Object, Object)
        ReadOnly Property XeoraRequestID As String
        Function CreateThreadID() As String
        Sub Dispose()

        Interface IHttpContextContent(Of K, V)
            ReadOnly Property Items As Generic.KeyValuePair(Of K, V)()
            Property Item(ByVal Key As K) As V
            Sub Remove(ByVal Key As K)
            Sub Clear()
            ReadOnly Property Count As Integer
        End Interface

        Interface IHttpApplication
            Inherits IHttpContextContent(Of String, Object)
        End Interface

        Interface IHttpSession
            Inherits IHttpContextContent(Of String, Object)

            ReadOnly Property SessionID As String
            ReadOnly Property Mode As System.Web.SessionState.SessionStateMode
        End Interface

        Interface IHttpRequest
            Event RewritePathRaised(ByVal RawURL As String)

            ReadOnly Property URL As URL
            ReadOnly Property PhysicalPath As String
            ReadOnly Property Cookie As System.Web.HttpCookieCollection
            ReadOnly Property File As System.Web.HttpFileCollection
            ReadOnly Property Form As Specialized.NameValueCollection
            ReadOnly Property QueryString As Specialized.NameValueCollection
            ReadOnly Property Header As Specialized.NameValueCollection
            ReadOnly Property Method As String
            ReadOnly Property Server As Specialized.NameValueCollection
            ReadOnly Property Stream As IO.Stream
            Sub RewritePath(ByVal RawURL As String)
            ReadOnly Property HashCode As String
        End Interface

        Interface IHttpResponse
            Property StatusCode As Integer
            ReadOnly Property Header As Specialized.NameValueCollection
            Sub ReleaseHeader()
            ReadOnly Property Cookie As System.Web.HttpCookieCollection
            ReadOnly Property Stream As IO.Stream
            Sub Redirect(ByVal URL As String)
        End Interface
    End Interface
End Namespace
