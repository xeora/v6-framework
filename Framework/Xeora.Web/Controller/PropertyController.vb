Option Strict On

Imports Xeora.Web.Controller.Directive
Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller
    Public Class PropertyController
        Inherits ControllerBase
        Implements IInstanceRequires

        Private _ObjectResult As Object
        Private _ByPassUpdateBlockCheck As Boolean = False

        Public Event InstanceRequested(ByRef Instance As IDomain) Implements IInstanceRequires.InstanceRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControllerTypes.Property, ContentArguments)

            Me._ObjectResult = Nothing
        End Sub

        Public ReadOnly Property ObjectResult() As Object
            Get
                Return Me._ObjectResult
            End Get
        End Property

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            ' PropertyController should always be rendered if it is requested from Internal Property parser
            If Not Me._ByPassUpdateBlockCheck Then
                If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                    Me.DefineRenderedValue(String.Empty)

                    Exit Sub
                End If
            End If

            If String.IsNullOrEmpty(Me.InsideValue) Then
                If String.Compare(Me.DraftValue, Me.InsideValue) <> 0 Then
                    Me.DefineRenderedValue("$")
                    Me._ObjectResult = CObj("$")
                Else
                    Me.DefineRenderedValue(String.Empty)
                    Me._ObjectResult = Nothing
                End If

                Exit Sub
            End If

            Select Case Me.InsideValue
                Case "DomainContents"
                    Me.RenderDomainContents()

                Case "PageRenderDuration"
                    Me.RenderPageRenderDuration()

                Case Else
                    Select Case Me.InsideValue.Chars(0)
                        Case "^"c ' QueryString Value
                            Me.RenderQueryString()

                        Case "~"c ' Form Post Value
                            Me.RenderFormPost()

                        Case "-"c ' Session Value
                            Me.RenderSessionItem()

                        Case "+"c ' Cookies Value
                            Me.RenderCookieItem()

                        Case "="c ' Value which following after '='
                            Me.RenderStaticString()

                        Case "#"c ' DataTable Field
                            Me.RenderDataItem()

                        Case "*"c ' Search in All orderby : [InData, DataField, Session, Form Post, QueryString, Cookie] (DOES NOT SUPPORT FILE POSTS)
                            Dim SearchArgKey As String = Me.InsideValue.Substring(1)
                            Dim SearchArgValue As Object

                            ' Search InDatas
                            SearchArgValue = Me.ContentArguments.Item(SearchArgKey)

                            ' Search In VariablePool
                            If SearchArgValue Is Nothing Then _
                                SearchArgValue = Helpers.VariablePool.Get(SearchArgKey)

                            ' Search In Session
                            If SearchArgValue Is Nothing Then _
                                SearchArgValue = Helpers.Context.Session.Item(SearchArgKey)

                            ' Cookie
                            If SearchArgValue Is Nothing AndAlso
                                Not Helpers.Context.Request.Cookie.Item(SearchArgKey) Is Nothing Then

                                SearchArgValue = Helpers.Context.Request.Cookie.Item(SearchArgKey).Value
                            End If

                            ' Search In Form Post First File then Value
                            If SearchArgValue Is Nothing Then _
                                SearchArgValue = Helpers.Context.Request.File.Item(SearchArgKey)
                            If SearchArgValue Is Nothing Then _
                                SearchArgValue = Helpers.Context.Request.Form.Item(SearchArgKey)

                            ' Search QueryString
                            If SearchArgValue Is Nothing Then _
                                SearchArgValue = Helpers.Context.Request.QueryString.Item(SearchArgKey)

                            If Not SearchArgValue Is Nothing Then
                                Me.DefineRenderedValue(SearchArgValue.ToString())
                            Else
                                Me.DefineRenderedValue(String.Empty)
                            End If
                            Me._ObjectResult = SearchArgValue

                        Case "@"c ' Search in Values Set for Current Request Session
                            Me.RenderObjectItem(SenderController)

                        Case Else
                            Me.RenderVariablePoolItem()

                    End Select
            End Select
        End Sub

        Private Sub RenderDomainContents()
            Dim Instance As IDomain = Nothing

            RaiseEvent InstanceRequested(Instance)

            Me.DefineRenderedValue(Instance.ContentsVirtualPath)
            Me._ObjectResult = CObj(Me.RenderedValue)
        End Sub

        Private Sub RenderPageRenderDuration()
            Me.DefineRenderedValue("<!--_sys_PAGERENDERDURATION-->")
            Me._ObjectResult = CObj("<!--_sys_PAGERENDERDURATION-->")
        End Sub

        Private Sub RenderQueryString()
            Dim QueryItemKey As String = Me.InsideValue.Substring(1)
            Dim QueryItemValue As String =
                Helpers.Context.Request.QueryString.Item(QueryItemKey)

            If Not String.IsNullOrEmpty(QueryItemValue) Then
                Select Case Configurations.RequestTagFiltering
                    Case [Enum].RequestTagFilteringTypes.OnlyQuery, [Enum].RequestTagFilteringTypes.Both
                        If Array.IndexOf(Configurations.RequestTagFilteringExceptions, QueryItemKey) = -1 Then _
                            QueryItemValue = Me.CleanHTMLTags(QueryItemValue, Configurations.RequestTagFilteringItems)
                End Select
            End If

            Me.DefineRenderedValue(QueryItemValue)
            Me._ObjectResult = CObj(QueryItemValue)
        End Sub

        Private Sub RenderFormPost()
            Dim FormItemKey As String = Me.InsideValue.Substring(1)

            ' File Post is not supporting XML Http Requests
            Dim RequestFilesKeys As String() =
                Helpers.Context.Request.File.AllKeys
            Dim RequestFileObjects As New Generic.List(Of System.Web.HttpPostedFile)

            For kC As Integer = 0 To RequestFilesKeys.Length - 1
                If String.Compare(RequestFilesKeys(kC), FormItemKey, True) = 0 Then
                    RequestFileObjects.Add(
                        Helpers.Context.Request.File.Item(kC))
                End If
            Next
            ' !--

            If RequestFileObjects.Count > 0 Then
                Me.DefineRenderedValue(String.Empty)
                If RequestFileObjects.Count = 1 Then
                    Me._ObjectResult = RequestFileObjects.Item(0)
                Else
                    Me._ObjectResult = RequestFileObjects.ToArray()
                End If

                Exit Sub
            End If

            Dim FormItemValue As String =
                Helpers.Context.Request.Form.Item(FormItemKey)

            If Not String.IsNullOrEmpty(FormItemValue) Then
                Select Case Configurations.RequestTagFiltering
                    Case [Enum].RequestTagFilteringTypes.OnlyForm, [Enum].RequestTagFilteringTypes.Both
                        If Array.IndexOf(Configurations.RequestTagFilteringExceptions, FormItemKey) = -1 Then _
                            FormItemValue = Me.CleanHTMLTags(FormItemValue, Configurations.RequestTagFilteringItems)
                End Select
            End If

            Me.DefineRenderedValue(FormItemValue)
            Me._ObjectResult = FormItemValue
        End Sub

        Private Sub RenderSessionItem()
            Dim SessionItemKey As String = Me.InsideValue.Substring(1)
            Dim SessionItemValue As Object =
                Helpers.Context.Session.Item(SessionItemKey)

            If SessionItemValue Is Nothing Then
                Me.DefineRenderedValue(String.Empty)
            Else
                Me.DefineRenderedValue(SessionItemValue.ToString())
            End If
            Me._ObjectResult = SessionItemValue
        End Sub

        Private Sub RenderCookieItem()
            Dim CookieItemKey As String = Me.InsideValue.Substring(1)
            Dim CookieItem As System.Web.HttpCookie =
                Helpers.Context.Request.Cookie.Item(CookieItemKey)

            If CookieItem Is Nothing Then
                Me.DefineRenderedValue(String.Empty)
                Me._ObjectResult = Nothing
            Else
                Me.DefineRenderedValue(CookieItem.Value)
                Me._ObjectResult = CookieItem.Value
            End If
        End Sub

        Private Sub RenderStaticString()
            Dim StringValue As String = Me.InsideValue.Substring(1)

            Me.DefineRenderedValue(StringValue)
            Me._ObjectResult = StringValue
        End Sub

        Private Sub RenderDataItem()
            Dim SearchController As ControllerBase = Me
            Dim SearchVariableKey As String = Me.InsideValue

            Me.LocateLeveledContentInfo(SearchVariableKey, SearchController)

            If SearchController Is Nothing Then
                Me.DefineRenderedValue(String.Empty)
                Me._ObjectResult = Nothing

                Exit Sub
            End If

            Dim argItem As Object =
                SearchController.ContentArguments.Item(SearchVariableKey)

            If Not argItem Is Nothing AndAlso
                Not argItem.GetType() Is GetType(DBNull) Then

                Me.DefineRenderedValue(argItem.ToString())
                Me._ObjectResult = argItem
            Else
                Me.DefineRenderedValue(String.Empty)
                Me._ObjectResult = Nothing
            End If
        End Sub

        Private Sub RenderVariablePoolItem()
            Dim PoolValue As Object =
                Helpers.VariablePool.Get(Me.InsideValue)

            If Not PoolValue Is Nothing Then
                Me.DefineRenderedValue(PoolValue.ToString())
            Else
                Me.DefineRenderedValue(String.Empty)
            End If
            Me._ObjectResult = PoolValue
        End Sub

        Private Sub RenderObjectItem(ByRef SenderController As ControllerBase)
            Dim ObjectPath As String =
                Me.InsideValue.Substring(1)

            Dim ObjectPaths As String() =
                ObjectPath.Split("."c)

            If ObjectPaths.Length < 2 Then _
                Throw New Exception.GrammerException()

            Dim ObjectItemKey As String = ObjectPaths(0)
            Dim ObjectItem As Object = Nothing

            Select Case ObjectItemKey.Chars(0)
                Case "-"c
                    ObjectItem = Helpers.Context.Session.Item(ObjectItemKey.Substring(1))

                Case "#"c
                    Dim SearchController As ControllerBase = Me

                    Me.LocateLeveledContentInfo(ObjectItemKey, SearchController)

                    If Not SearchController Is Nothing Then
                        Dim argItem As Object =
                            SearchController.ContentArguments.Item(ObjectItemKey)

                        If Not argItem Is Nothing Then ObjectItem = argItem
                    End If

                Case Else
                    ObjectItem = Helpers.VariablePool.Get(ObjectItemKey)

                    If TypeOf ObjectItem Is DataListOutputInfo Then
                        If TypeOf SenderController Is ControlBase AndAlso
                            TypeOf SenderController Is INamable Then

                            If String.Compare(
                                CType(SenderController, INamable).ControlID, ObjectItemKey, True) <> 0 Then

                                If Not Me.BoundControlRenderWaiting Then Me.RegisterToRenderCompletedOf(ObjectItemKey)

                                Exit Sub
                            End If
                        Else
                            If Not Me.BoundControlRenderWaiting Then Me.RegisterToRenderCompletedOf(ObjectItemKey)

                            Exit Sub
                        End If
                    Else
                        ' DataListOutputInfo is not defined yet and let's put in a queue to render later
                        If ObjectItem Is Nothing Then
                            If Not Me.BoundControlRenderWaiting Then Me.RegisterToRenderCompletedOf(ObjectItemKey)

                            Exit Sub
                        End If
                    End If

            End Select

            If ObjectItem Is Nothing Then
                Me.DefineRenderedValue(String.Empty)
                Me._ObjectResult = Nothing

                Exit Sub
            End If

            Dim ObjectValue As Object

            Try
                For pC As Integer = 1 To ObjectPaths.Length - 1
                    If ObjectItem Is Nothing Then Exit For

                    ObjectItem = ObjectItem.GetType().InvokeMember(ObjectPaths(pC), Reflection.BindingFlags.GetProperty, Nothing, ObjectItem, Nothing)
                Next

                ObjectValue = ObjectItem
            Catch ex As System.Exception
                ObjectValue = Nothing
            End Try

            If Not ObjectValue Is Nothing Then
                Me.DefineRenderedValue(ObjectValue.ToString())
            Else
                Me.DefineRenderedValue(String.Empty)
            End If
            Me._ObjectResult = ObjectValue

            Me.UnRegisterFromRenderCompletedOf(ObjectItemKey)
        End Sub

        Private Sub LocateLeveledContentInfo(ByRef SearchItemKey As String, ByRef Controller As ControllerBase)
            Dim OutsideOfBlock As Boolean =
                (SearchItemKey.LastIndexOf("#"c) > 0)
            Do
                If SearchItemKey.IndexOf("#") = 0 Then
                    Do
                        Controller = Controller.Parent

                        ' Only Controls that have own contents such as Datalist, VariableBlock and MessageBlock!
                        ' however, Datalist can have multiple content that's why we should check it this content
                        ' parent is Datalist or not
                    Loop Until Controller Is Nothing OrElse
                                TypeOf Controller.Parent Is Control.DataList OrElse
                                TypeOf Controller Is Control.VariableBlock OrElse
                                TypeOf Controller Is MessageBlock
                Else
                    If TypeOf Controller Is Control.VariableBlock AndAlso
                            CType(Controller, Control.VariableBlock).Level > 0 AndAlso
                            Not CType(Controller, Control.VariableBlock).LevelExecutionOnly AndAlso
                            OutsideOfBlock Then

                        OutsideOfBlock = False
                        SearchItemKey = SearchItemKey.PadLeft(CType(Controller, Control.VariableBlock).Level + SearchItemKey.Length, "#"c)
                    Else
                        Exit Do
                    End If
                End If

                SearchItemKey = SearchItemKey.Substring(1)
            Loop Until Controller Is Nothing
        End Sub

        Public Shared Function ParseProperty(
                            ByVal [Property] As String,
                            ByRef Parent As ControllerBase,
                            ByVal ContentArguments As ArgumentInfoCollection,
                            ByVal Handler As IInstanceRequires.InstanceRequestedEventHandler) As Object

            Dim rObject As Object = Nothing

            If Not [Property] Is Nothing Then
                Dim PropertyController As PropertyController =
                    New PropertyController(0, [Property], ContentArguments)

                PropertyController._ByPassUpdateBlockCheck = True
                AddHandler PropertyController.InstanceRequested, Handler

                If Not Parent Is Nothing Then
                    ' Fake Add To Parent and Remove it after render. This is for parent relation hack
                    Parent.Children.Add(PropertyController)
                End If

                PropertyController.Render(Nothing)

                ' Fake Add is removing. This is for parent relation hack
                If Not Parent Is Nothing Then Parent.Children.RemoveAt(Parent.Children.Count - 1)

                rObject = PropertyController.ObjectResult
            End If

            Return rObject
        End Function

        Private Function CleanHTMLTags(ByVal Content As String, ByVal CleaningTags As String()) As String
            Dim RegExSearch As Text.RegularExpressions.Regex

            If String.IsNullOrEmpty(Content) OrElse CleaningTags Is Nothing OrElse CleaningTags.Length = 0 Then _
                Return Content

            Dim SearchType As Integer = 0
            Dim LastSearchIndex As Integer, ModifiedContent As Text.StringBuilder
            Dim RegExMatches As Text.RegularExpressions.MatchCollection

            For Each CleaningTag As String In CleaningTags
                If CleaningTag.IndexOf(">"c) = 0 Then
                    RegExSearch = New Text.RegularExpressions.Regex(
                                        String.Format("<{0}(\s+[^>]*)*>", CleaningTag.Substring(1)))
                    SearchType = 1
                Else
                    RegExSearch = New Text.RegularExpressions.Regex(
                                        String.Format("<{0}(\s+[^>]*)*(/)?>", CleaningTag))
                    SearchType = 0
                End If

                RegExMatches = RegExSearch.Matches(Content)

                ModifiedContent = New Text.StringBuilder
                LastSearchIndex = 0

                For Each regMatch As Text.RegularExpressions.Match In RegExMatches
                    ModifiedContent.Append(Content.Substring(LastSearchIndex, regMatch.Index - LastSearchIndex))

                    Select Case SearchType
                        Case 1
                            Dim TailRegExSearch As Text.RegularExpressions.Regex =
                                New Text.RegularExpressions.Regex(String.Format("</{0}>", CleaningTag.Substring(1)))
                            Dim TailRegMatch As Text.RegularExpressions.Match =
                                TailRegExSearch.Match(Content, LastSearchIndex)

                            If TailRegMatch.Success Then
                                LastSearchIndex = TailRegMatch.Index + TailRegMatch.Length
                            Else
                                LastSearchIndex = regMatch.Index + regMatch.Length
                            End If
                        Case Else
                            LastSearchIndex = regMatch.Index + regMatch.Length
                    End Select
                Next
                ModifiedContent.Append(Content.Substring(LastSearchIndex))

                Content = ModifiedContent.ToString()
            Next

            Return Content
        End Function
    End Class
End Namespace