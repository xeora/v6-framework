Option Strict On
Imports System.Collections.Specialized
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports Xeora.Web.Shared

Namespace Xeora.Web.Context
    Public Class BuildInContext
        Implements IHttpContext

        Private _UnderlyingContext As System.Web.HttpContext

        Private _XeoraRequestID As String
        Private _Context As IHttpContext.IHttpContextContent(Of Object, Object)
        Private _Application As IHttpContext.IHttpApplication
        Private _Session As IHttpContext.IHttpSession
        Private _Request As IHttpContext.IHttpRequest
        Private _Response As IHttpContext.IHttpResponse

        Public Sub New(ByRef HttpContext As System.Web.HttpContext)
            Me._UnderlyingContext = HttpContext

            Me._XeoraRequestID = CType(HttpContext.Items.Item("RequestID"), String)

            Me._Context = New HttpContext(HttpContext)
            Me._Application = New HttpApplication(HttpContext.Application)
            Me._Session = New HttpSession(HttpContext.Session)
            Me._Request = New HttpRequest(HttpContext.Request, Me._XeoraRequestID)
            AddHandler Me._Request.RewritePathRaised,
                New IHttpContext.IHttpRequest.RewritePathRaisedEventHandler(Sub(ByVal RawURL As String)
                                                                                Me._UnderlyingContext.RewritePath(RawURL)

                                                                                Me._Request = New HttpRequest(Me._UnderlyingContext.Request, Me._XeoraRequestID)
                                                                                CType(Me._Request, HttpRequest).Build()
                                                                            End Sub)
            CType(Me._Request, HttpRequest).Build()
            Me._Response = New HttpResponse(HttpContext.Response)
        End Sub

        Public Function CreateThreadID() As String Implements IHttpContext.CreateThreadID
            Return Handler.RequestModule.DuplicateContext(Me.XeoraRequestID)
        End Function

        Public Sub Dispose() Implements IHttpContext.Dispose
            If Not Handler.RequestModule.DisposeContext(Me._XeoraRequestID) Then _
                Throw New System.Exception("You can not dispose a Context which does not belong to a Thread!")
        End Sub

        Public ReadOnly Property UnderlyingContext As System.Web.HttpContext Implements IHttpContext.UnderlyingContext
            Get
                Return Me._UnderlyingContext
            End Get
        End Property

        Public ReadOnly Property Application As IHttpContext.IHttpApplication Implements IHttpContext.Application
            Get
                Return Me._Application
            End Get
        End Property

        Public ReadOnly Property Content As IHttpContext.IHttpContextContent(Of Object, Object) Implements IHttpContext.Content
            Get
                Return Me._Context
            End Get
        End Property

        Public ReadOnly Property Request As IHttpContext.IHttpRequest Implements IHttpContext.Request
            Get
                Return Me._Request
            End Get
        End Property

        Public ReadOnly Property XeoraRequestID As String Implements IHttpContext.XeoraRequestID
            Get
                Return Me._XeoraRequestID
            End Get
        End Property

        Public ReadOnly Property Response As IHttpContext.IHttpResponse Implements IHttpContext.Response
            Get
                Return Me._Response
            End Get
        End Property

        Public ReadOnly Property Session As IHttpContext.IHttpSession Implements IHttpContext.Session
            Get
                Return Me._Session
            End Get
        End Property

        Private Class HttpContext
            Implements IHttpContext.IHttpContextContent(Of Object, Object)

            Private _Context As System.Web.HttpContext

            Public Sub New(ByRef HttpContext As System.Web.HttpContext)
                Me._Context = HttpContext
            End Sub

            Public ReadOnly Property Count As Integer Implements IHttpContext.IHttpContextContent(Of Object, Object).Count
                Get
                    Return Me._Context.Items.Count
                End Get
            End Property

            Public Property Item(ByVal Key As Object) As Object Implements IHttpContext.IHttpContextContent(Of Object, Object).Item
                Get
                    Return Me._Context.Items.Item(Key)
                End Get
                Set(ByVal value As Object)
                    Me._Context.Items.Item(Key) = value
                End Set
            End Property

            Public ReadOnly Property Items As KeyValuePair(Of Object, Object)() Implements IHttpContext.IHttpContextContent(Of Object, Object).Items
                Get
                    Dim rItems As New List(Of KeyValuePair(Of Object, Object))

                    For Each Key As Object In Me._Context.Items.Keys
                        rItems.Add(New KeyValuePair(Of Object, Object)(Key, Me._Context.Items.Item(Key)))
                    Next

                    Return rItems.ToArray()
                End Get
            End Property

            Public Sub Clear() Implements IHttpContext.IHttpContextContent(Of Object, Object).Clear
                Me._Context.Items.Clear()
            End Sub

            Public Sub Remove(ByVal Key As Object) Implements IHttpContext.IHttpContextContent(Of Object, Object).Remove
                Me._Context.Items.Remove(Key)
            End Sub
        End Class

        Private Class HttpApplication
            Implements IHttpContext.IHttpApplication

            Private _Application As System.Web.HttpApplicationState

            Public Sub New(ByRef HttpApplication As System.Web.HttpApplicationState)
                Me._Application = HttpApplication
            End Sub

            Public ReadOnly Property Count As Integer Implements IHttpContext.IHttpContextContent(Of String, Object).Count
                Get
                    Return Me._Application.Count
                End Get
            End Property

            Public Property Item(ByVal Key As String) As Object Implements IHttpContext.IHttpContextContent(Of String, Object).Item
                Get
                    Return Me._Application.Item(Key)
                End Get
                Set(ByVal value As Object)
                    Me._Application.Item(Key) = value
                End Set
            End Property

            Public ReadOnly Property Items As KeyValuePair(Of String, Object)() Implements IHttpContext.IHttpContextContent(Of String, Object).Items
                Get
                    Dim rItems As New List(Of KeyValuePair(Of String, Object))

                    For Each Key As String In Me._Application.AllKeys
                        rItems.Add(New KeyValuePair(Of String, Object)(Key, Me._Application.Contents.Item(Key)))
                    Next

                    Return rItems.ToArray()
                End Get
            End Property

            Public Sub Clear() Implements IHttpContext.IHttpContextContent(Of String, Object).Clear
                Me._Application.Clear()
            End Sub

            Public Sub Remove(ByVal Key As String) Implements IHttpContext.IHttpContextContent(Of String, Object).Remove
                Me._Application.Remove(Key)
            End Sub
        End Class

        Private Class HttpSession
            Implements IHttpContext.IHttpSession

            Private _Session As HttpSessionState

            Public Sub New(ByRef HttpSession As HttpSessionState)
                Me._Session = HttpSession
            End Sub

            Public ReadOnly Property Count As Integer Implements IHttpContext.IHttpContextContent(Of String, Object).Count
                Get
                    Return Me._Session.Count
                End Get
            End Property

            Public Property Item(ByVal Key As String) As Object Implements IHttpContext.IHttpContextContent(Of String, Object).Item
                Get
                    Return Me._Session.Item(Key)
                End Get
                Set(ByVal value As Object)
                    Me._Session.Item(Key) = value
                End Set
            End Property

            Public ReadOnly Property Items As KeyValuePair(Of String, Object)() Implements IHttpContext.IHttpContextContent(Of String, Object).Items
                Get
                    Dim rItems As New List(Of KeyValuePair(Of String, Object))

                    For Each Key As String In Me._Session.Keys
                        rItems.Add(New KeyValuePair(Of String, Object)(Key, Me._Session.Item(Key)))
                    Next

                    Return rItems.ToArray()
                End Get
            End Property

            Public ReadOnly Property Mode As SessionStateMode Implements IHttpContext.IHttpSession.Mode
                Get
                    Return Me._Session.Mode
                End Get
            End Property

            Public ReadOnly Property SessionID As String Implements IHttpContext.IHttpSession.SessionID
                Get
                    Return Me._Session.SessionID
                End Get
            End Property

            Public Sub Clear() Implements IHttpContext.IHttpContextContent(Of String, Object).Clear
                Me._Session.Clear()
            End Sub

            Public Sub Remove(ByVal Key As String) Implements IHttpContext.IHttpContextContent(Of String, Object).Remove
                Me._Session.Remove(Key)
            End Sub
        End Class

        Private Class HttpRequest
            Implements IHttpContext.IHttpRequest

            Public Event RewritePathRaised As IHttpContext.IHttpRequest.RewritePathRaisedEventHandler Implements IHttpContext.IHttpRequest.RewritePathRaised

            Private _XeoraRequestID As String
            Private _Request As System.Web.HttpRequest
            Private _URL As URL

            Public Sub New(ByRef HttpRequest As System.Web.HttpRequest, ByVal XeoraRequestID As String)
                Me._XeoraRequestID = XeoraRequestID
                Me._Request = HttpRequest
            End Sub

            Public Sub Build()
                Me._URL = New URL(Me._Request.RawUrl)

                If String.Compare(Me._URL.Raw, Me._Request.RawUrl) <> 0 Then _
                    Me.RewritePath(Me._URL.Raw)
            End Sub

            Public ReadOnly Property Cookie As HttpCookieCollection Implements IHttpContext.IHttpRequest.Cookie
                Get
                    Return Me._Request.Cookies
                End Get
            End Property

            Public ReadOnly Property File As HttpFileCollection Implements IHttpContext.IHttpRequest.File
                Get
                    Return Me._Request.Files
                End Get
            End Property

            Public ReadOnly Property Form As NameValueCollection Implements IHttpContext.IHttpRequest.Form
                Get
                    Return Me._Request.Form
                End Get
            End Property

            Public ReadOnly Property HashCode As String Implements IHttpContext.IHttpRequest.HashCode
                Get
                    Dim CurrentHashCode As String =
                        CType(
                            AppDomain.CurrentDomain.GetData(
                                String.Format("HashCode_{0}", Me._XeoraRequestID)),
                            String
                        )

                    If String.IsNullOrEmpty(CurrentHashCode) Then
                        Dim RequestFilePath As String =
                            Me._Request.FilePath

                        RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserImplementation) + Configurations.ApplicationRoot.BrowserImplementation.Length)

                        Dim mR As Text.RegularExpressions.Match =
                            Text.RegularExpressions.Regex.Match(RequestFilePath, "\d+/")

                        If mR.Success AndAlso mR.Index = 0 Then
                            CurrentHashCode = mR.Value.Substring(0, mR.Value.Length - 1)
                        Else
                            CurrentHashCode = Me._Request.GetHashCode().ToString()
                        End If

                        AppDomain.CurrentDomain.SetData(
                                String.Format("HashCode_{0}", Me._XeoraRequestID),
                                CurrentHashCode
                        )
                    End If

                    Return CurrentHashCode
                End Get
            End Property

            Public ReadOnly Property Header As NameValueCollection Implements IHttpContext.IHttpRequest.Header
                Get
                    Return Me._Request.Headers
                End Get
            End Property

            Public ReadOnly Property Method As String Implements IHttpContext.IHttpRequest.Method
                Get
                    Return Me._Request.HttpMethod
                End Get
            End Property

            Public ReadOnly Property QueryString As NameValueCollection Implements IHttpContext.IHttpRequest.QueryString
                Get
                    Return Me._Request.QueryString
                End Get
            End Property

            Public ReadOnly Property URL As URL Implements IHttpContext.IHttpRequest.URL
                Get
                    Return Me._URL
                End Get
            End Property

            Public ReadOnly Property Server As NameValueCollection Implements IHttpContext.IHttpRequest.Server
                Get
                    Return Me._Request.ServerVariables
                End Get
            End Property

            Public ReadOnly Property Stream As Stream Implements IHttpContext.IHttpRequest.Stream
                Get
                    Return Me._Request.InputStream
                End Get
            End Property

            Public ReadOnly Property PhysicalPath As String Implements IHttpContext.IHttpRequest.PhysicalPath
                Get
                    Return Me._Request.PhysicalPath
                End Get
            End Property

            Public Sub RewritePath(ByVal RawURL As String) Implements IHttpContext.IHttpRequest.RewritePath
                RaiseEvent RewritePathRaised(RawURL)
            End Sub
        End Class

        Private Class HttpResponse
            Implements IHttpContext.IHttpResponse

            Private _Response As System.Web.HttpResponse

            Public Sub New(ByRef HttpResponse As System.Web.HttpResponse)
                HttpResponse.Buffer = False
                HttpResponse.BufferOutput = False

                Me._Response = HttpResponse
            End Sub

            Public ReadOnly Property Cookie As HttpCookieCollection Implements IHttpContext.IHttpResponse.Cookie
                Get
                    Return Me._Response.Cookies
                End Get
            End Property

            Public ReadOnly Property Header As NameValueCollection Implements IHttpContext.IHttpResponse.Header
                Get
                    Return Me._Response.Headers
                End Get
            End Property

            Public Sub ReleaseHeader() Implements IHttpContext.IHttpResponse.ReleaseHeader
                For Each Key As String In Me._Response.Headers.Keys
                    Select Case Key
                        Case "Content-Type"
                            Me._Response.ContentType = Me._Response.Headers.Item(Key)
                        Case "Cache-Control"
                            Me._Response.CacheControl = Me._Response.Headers.Item(Key)
                        Case "Expires"
                            If Not DateTime.TryParse(Me._Response.Headers.Item(Key), Me._Response.ExpiresAbsolute) Then
                                Integer.TryParse(Me._Response.Headers.Item(Key), Me._Response.Expires)
                            End If
                    End Select
                Next
            End Sub

            Public Sub Redirect(ByVal URL As String) Implements IHttpContext.IHttpResponse.Redirect
                If Not String.IsNullOrEmpty(URL) Then
                    Me.Header.Item("Location") = URL
                    Me.StatusCode = 301

                    Me.ReleaseHeader()

                    Me.Stream.Flush()
                    Me.Stream.Close()
                End If
            End Sub

            Public Property StatusCode As Integer Implements IHttpContext.IHttpResponse.StatusCode
                Get
                    Return Me._Response.StatusCode
                End Get
                Set(ByVal code As Integer)
                    Me._Response.StatusCode = code
                End Set
            End Property

            Public ReadOnly Property Stream As Stream Implements IHttpContext.IHttpResponse.Stream
                Get
                    Return Me._Response.OutputStream
                End Get
            End Property
        End Class
    End Class
End Namespace