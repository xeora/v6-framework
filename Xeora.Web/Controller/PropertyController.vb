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

            Dim Instance As IDomain = Nothing
            RaiseEvent InstanceRequested(Instance)

            If String.IsNullOrEmpty(Me.InsideValue) Then
                If String.Compare(Me.DraftValue, Me.InsideValue) <> 0 Then
                    Me.DefineRenderedValue("$")
                    Me._ObjectResult = CObj("$")
                Else
                    Me.DefineRenderedValue(String.Empty)
                    Me._ObjectResult = Nothing
                End If
            Else
                Select Case Me.InsideValue
                    Case "DomainContents"
                        Me.DefineRenderedValue(
                            Helpers.GetDomainContentsPath(
                                Instance.IDAccessTree,
                                Instance.Language.ID
                            )
                        )
                        Me._ObjectResult = CObj(Me.RenderedValue)

                    Case "PageRenderDuration"
                        Me.DefineRenderedValue("<!--_sys_PAGERENDERDURATION-->")
                        Me._ObjectResult = CObj("<!--_sys_PAGERENDERDURATION-->")

                    Case Else
                        Select Case Me.InsideValue.Chars(0)
                            Case "^"c ' QueryString Value
                                Dim QueryValue As String =
                                    [Shared].Helpers.Context.Request.QueryString.Item(Me.InsideValue.Substring(1))

                                Select Case [Shared].Configurations.RequestTagFiltering
                                    Case [Shared].Globals.RequestTagFilteringTypes.OnlyQuery, [Shared].Globals.RequestTagFilteringTypes.Both
                                        Dim ArgumentNameForCompare As String =
                                                Me.InsideValue.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                        If Array.IndexOf([Shared].Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                            QueryValue = Me.CleanHTMLTags(QueryValue, [Shared].Configurations.RequestTagFilteringItems)
                                End Select

                                Me.DefineRenderedValue(QueryValue)
                                Me._ObjectResult = CObj(QueryValue)

                            Case "~"c ' Form Post Value
                                ' File Post is not supporting on XML Http Requests
                                Dim RequestFilesKeys As String() =
                                    [Shared].Helpers.Context.Request.Files.AllKeys
                                Dim RequestFileObjects As New Generic.List(Of System.Web.HttpPostedFile)

                                For kC As Integer = 0 To RequestFilesKeys.Length - 1
                                    If String.Compare(RequestFilesKeys(kC), Me.InsideValue.Substring(1), True) = 0 Then
                                        RequestFileObjects.Add(
                                            [Shared].Helpers.Context.Request.Files.Item(kC))
                                    End If
                                Next
                                ' !--

                                If RequestFileObjects.Count = 0 Then
                                    If String.Compare([Shared].Helpers.Context.Request.HttpMethod, "POST", True) = 0 Then
                                        Dim ControlSIM As Hashtable =
                                            CType(Me.ContentArguments.Item("_sys_ControlSIM"), Hashtable)

                                        If Not ControlSIM Is Nothing AndAlso
                                            ControlSIM.ContainsKey(Me.InsideValue.Substring(1)) Then

                                            Me.DefineRenderedValue(
                                                CType(ControlSIM.Item(Me.InsideValue.Substring(1)), String)
                                            )
                                            Me._ObjectResult = ControlSIM.Item(Me.InsideValue.Substring(1))
                                        Else
                                            Dim FormValue As String =
                                                [Shared].Helpers.Context.Request.Form.Item(Me.InsideValue.Substring(1))

                                            Select Case [Shared].Configurations.RequestTagFiltering
                                                Case [Shared].Globals.RequestTagFilteringTypes.OnlyForm, [Shared].Globals.RequestTagFilteringTypes.Both
                                                    Dim ArgumentNameForCompare As String =
                                                    Me.InsideValue.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                                    If Array.IndexOf([Shared].Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                                    FormValue = Me.CleanHTMLTags(FormValue, [Shared].Configurations.RequestTagFilteringItems)
                                            End Select

                                            Me.DefineRenderedValue(FormValue)
                                            Me._ObjectResult = FormValue
                                        End If
                                    Else
                                        Me.DefineRenderedValue(String.Empty)
                                        Me._ObjectResult = Nothing
                                    End If
                                Else
                                    Me.DefineRenderedValue(String.Empty)
                                    If RequestFileObjects.Count = 1 Then
                                        Me._ObjectResult = RequestFileObjects.Item(0)
                                    Else
                                        Me._ObjectResult = RequestFileObjects.ToArray()
                                    End If
                                End If

                            Case "-"c ' Session Value
                                Dim SessionResult As Object =
                                    [Shared].Helpers.Context.Session.Contents.Item(Me.InsideValue.Substring(1))

                                If SessionResult Is Nothing Then
                                    Me.DefineRenderedValue(String.Empty)
                                Else
                                    Me.DefineRenderedValue(SessionResult.ToString())
                                End If
                                Me._ObjectResult = SessionResult

                            Case "+"c ' Cookies Value
                                Try
                                    Dim CookieValue As String =
                                        [Shared].Helpers.Context.Request.Cookies.Item(Me.InsideValue.Substring(1)).Value

                                    Me.DefineRenderedValue(CookieValue)
                                    Me._ObjectResult = CookieValue
                                Catch ex As System.Exception
                                    Me.DefineRenderedValue(String.Empty)
                                    Me._ObjectResult = Nothing
                                End Try

                            Case "="c ' Value which following after '='
                                Me.DefineRenderedValue(Me.InsideValue.Substring(1))
                                Me._ObjectResult = Me.InsideValue.Substring(1)

                            Case "#"c ' DataTable Field
                                Dim searchContentInfo As ControllerBase = Me
                                Dim searchVariableName As String = Me.InsideValue

                                Do
                                    searchVariableName = searchVariableName.Substring(1)

                                    If searchVariableName.IndexOf("#") = 0 Then
                                        Do
                                            searchContentInfo = searchContentInfo.Parent

                                            ' Only Controls that have own contents such as Datalist, VariableBlock and MessageBlock!
                                        Loop Until searchContentInfo Is Nothing OrElse
                                            TypeOf searchContentInfo Is Control.DataList OrElse
                                            TypeOf searchContentInfo Is Control.VariableBlock OrElse
                                            TypeOf searchContentInfo Is MessageBlock
                                    Else
                                        Exit Do
                                    End If
                                Loop Until searchContentInfo Is Nothing

                                If Not searchContentInfo Is Nothing Then
                                    Dim argItem As Object =
                                        searchContentInfo.ContentArguments.Item(searchVariableName)

                                    If Not argItem Is Nothing AndAlso
                                        Not argItem.GetType() Is GetType(DBNull) Then

                                        Me.DefineRenderedValue(argItem.ToString())
                                        Me._ObjectResult = argItem
                                    Else
                                        Me.DefineRenderedValue(String.Empty)
                                        Me._ObjectResult = Nothing
                                    End If
                                Else
                                    Me.DefineRenderedValue(String.Empty)
                                    Me._ObjectResult = Nothing
                                End If

                                ' Just Needs Object Output

                            Case "*"c ' Search in All orderby : [InData, DataField, Session, Form Post, QueryString, Cookie] (DOES NOT SUPPORT FILE POSTS)
                                Dim searchArgName As String = Me.InsideValue.Substring(1)
                                Dim searchArgValue As Object

                                ' Search InDatas
                                searchArgValue = [Shared].Helpers.VariablePool.Get(searchArgName)

                                ' Search In DataFields (GlobalArguments, ContentArguments)
                                If searchArgValue Is Nothing Then
                                    Dim argItem As Object =
                                        Me.ContentArguments.Item(searchArgName)

                                    If Not argItem Is Nothing AndAlso
                                        Not argItem.GetType() Is GetType(DBNull) Then

                                        searchArgValue = argItem
                                    Else
                                        searchArgValue = Nothing
                                    End If
                                End If

                                ' Search In Session
                                If searchArgValue Is Nothing Then searchArgValue = Helpers.Context.Session.Contents.Item(searchArgName)

                                ' Search In Form Post (NO FILE POST SUPPORT)
                                If searchArgValue Is Nothing AndAlso
                                    String.Compare([Shared].Helpers.Context.Request.HttpMethod, "POST", True) = 0 Then

                                    Dim ControlSIM As Hashtable =
                                        CType(Me.ContentArguments.Item("_sys_ControlSIM"), Hashtable)

                                    If Not ControlSIM Is Nothing AndAlso
                                        ControlSIM.ContainsKey(searchArgName) Then

                                        searchArgValue = ControlSIM.Item(searchArgName)
                                    Else
                                        searchArgValue = [Shared].Helpers.Context.Request.Form.Item(searchArgName)
                                    End If
                                Else
                                    searchArgValue = Nothing
                                End If

                                ' Search QueryString
                                If searchArgValue Is Nothing Then searchArgValue = [Shared].Helpers.Context.Request.QueryString.Item(searchArgName)

                                ' Cookie
                                If searchArgValue Is Nothing AndAlso
                                    Not [Shared].Helpers.Context.Request.Cookies.Item(searchArgName) Is Nothing Then

                                    searchArgValue = [Shared].Helpers.Context.Request.Cookies.Item(searchArgName).Value
                                End If

                                If Not searchArgValue Is Nothing Then
                                    Me.DefineRenderedValue(searchArgValue.ToString())
                                Else
                                    Me.DefineRenderedValue(String.Empty)
                                End If
                                Me._ObjectResult = searchArgValue

                            Case Else ' Search in Values Set for Current Request Session
                                If Me.InsideValue.IndexOf("@"c) > 0 Then
                                    Dim ArgumentQueryObjectName As String =
                                        Me.InsideValue.Substring(Me.InsideValue.IndexOf("@"c) + 1)

                                    Dim ArgumentQueryObject As Object = Nothing

                                    Select Case ArgumentQueryObjectName.Chars(0)
                                        Case "-"c
                                            ArgumentQueryObject = [Shared].Helpers.Context.Session.Contents.Item(ArgumentQueryObjectName.Substring(1))

                                        Case "#"c
                                            Dim searchContentInfo As ControllerBase = Me
                                            Dim searchVariableName As String = ArgumentQueryObjectName

                                            Do
                                                searchVariableName = searchVariableName.Substring(1)

                                                If searchVariableName.IndexOf("#") = 0 Then
                                                    Do
                                                        searchContentInfo = searchContentInfo.Parent

                                                        ' Only Controls that have own contents such as Datalist, VariableBlock and MessageBlock!
                                                    Loop Until searchContentInfo Is Nothing OrElse
                                                        TypeOf searchContentInfo Is Control.DataList OrElse
                                                        TypeOf searchContentInfo Is Control.VariableBlock OrElse
                                                        TypeOf searchContentInfo Is MessageBlock
                                                Else
                                                    Exit Do
                                                End If
                                            Loop Until searchContentInfo Is Nothing

                                            If Not searchContentInfo Is Nothing Then
                                                Dim argItem As Object =
                                                    searchContentInfo.ContentArguments.Item(searchVariableName)

                                                If Not argItem Is Nothing Then ArgumentQueryObject = argItem
                                            End If

                                        Case Else
                                            If Not Me.BoundControlRenderWaiting Then Me.RegisterToRenderCompletedOf(ArgumentQueryObjectName)

                                            If TypeOf SenderController Is ControlBase AndAlso
                                                TypeOf SenderController Is INamable Then

                                                If String.Compare(
                                                    CType(SenderController, INamable).ControlID, ArgumentQueryObjectName, True) <> 0 Then

                                                    Exit Sub
                                                Else
                                                    ArgumentQueryObject = [Shared].Helpers.VariablePool.Get(ArgumentQueryObjectName)
                                                End If
                                            Else
                                                Exit Sub
                                            End If

                                    End Select

                                    If Not ArgumentQueryObject Is Nothing Then
                                        Dim ArgumentCallList As String() = Me.InsideValue.Substring(0, Me.InsideValue.IndexOf("@"c)).Split("."c)
                                        Dim ArgumentValue As Object

                                        Try
                                            For Each ArgumentCall As String In ArgumentCallList
                                                If Not ArgumentQueryObject Is Nothing Then
                                                    ArgumentQueryObject = ArgumentQueryObject.GetType().InvokeMember(ArgumentCall, Reflection.BindingFlags.GetProperty, Nothing, ArgumentQueryObject, Nothing)
                                                Else
                                                    Exit For
                                                End If
                                            Next

                                            ArgumentValue = ArgumentQueryObject
                                        Catch ex As System.Exception
                                            ArgumentValue = Nothing
                                        End Try

                                        If Not ArgumentValue Is Nothing Then
                                            Me.DefineRenderedValue(ArgumentValue.ToString())
                                        Else
                                            Me.DefineRenderedValue(String.Empty)
                                        End If
                                        Me._ObjectResult = ArgumentValue

                                        Me.UnRegisterFromRenderCompletedOf(ArgumentQueryObjectName)
                                    Else
                                        Me.DefineRenderedValue(String.Empty)
                                        Me._ObjectResult = Nothing
                                    End If
                                Else
                                    Dim PoolValue As Object =
                                        [Shared].Helpers.VariablePool.Get(Me.InsideValue)

                                    If Not PoolValue Is Nothing Then
                                        Me.DefineRenderedValue(PoolValue.ToString())
                                    Else
                                        Me.DefineRenderedValue(String.Empty)
                                    End If
                                    Me._ObjectResult = PoolValue
                                End If

                        End Select

                End Select
            End If
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

            If Not String.IsNullOrEmpty(Content) AndAlso
                Not CleaningTags Is Nothing AndAlso
                CleaningTags.Length > 0 Then

                Dim SearchType As Integer = 0, tContent As String = String.Empty
                Dim regMatchs As Text.RegularExpressions.MatchCollection

                For Each CleaningTag As String In CleaningTags
                    If CleaningTag.IndexOf(">"c) = 0 Then
                        RegExSearch = New Text.RegularExpressions.Regex(
                                        String.Format("<{0}(\s+[^>]*)*>", CleaningTag.Substring(1))
                                    )
                        SearchType = 1
                    Else
                        RegExSearch = New Text.RegularExpressions.Regex(
                                        String.Format("<{0}(\s+[^>]*)*(/)?>", CleaningTag)
                                    )
                        SearchType = 0
                    End If

                    regMatchs = RegExSearch.Matches(Content)

                    If regMatchs.Count > 0 Then
                        Dim LastSearchIndex As Integer = 0
                        For Each regMatch As Text.RegularExpressions.Match In regMatchs
                            tContent &= Content.Substring(LastSearchIndex, regMatch.Index - LastSearchIndex)

                            Select Case SearchType
                                Case 1
                                    Dim tailRegExSearch As New Text.RegularExpressions.Regex(
                                        String.Format("</{0}>", CleaningTag.Substring(1)))
                                    Dim tailRegMatch As Text.RegularExpressions.Match =
                                        tailRegExSearch.Match(Content, LastSearchIndex)

                                    If tailRegMatch.Success Then
                                        LastSearchIndex = tailRegMatch.Index + tailRegMatch.Length
                                    Else
                                        LastSearchIndex = regMatch.Index + regMatch.Length
                                    End If
                                Case Else
                                    LastSearchIndex = regMatch.Index + regMatch.Length
                            End Select
                        Next
                        tContent &= Content.Substring(LastSearchIndex)
                        Content = tContent
                    End If
                Next
            End If

            Return Content
        End Function
    End Class
End Namespace