Option Strict On

Namespace SolidDevelopment.Web.Managers
    Public Class Theme
        Implements PGlobals.ITheme

        Private _Deployment As DeploymentBase
        Private _ControlMapXPathNavigator As Xml.XPath.XPathNavigator

#Region " Constructers "

        Public Sub New()
            Me.New( _
                SolidDevelopment.Web.Configurations.DefaultTheme _
            )
        End Sub

        Public Sub New(ByVal ThemeID As String)
            Me.New( _
                ThemeID, _
                Nothing _
            )
        End Sub

        Public Sub New(ByVal ThemeID As String, ByVal ThemeTranslationID As String)
            Me.New( _
                ThemeID, _
                ThemeTranslationID, _
                Nothing _
            )
        End Sub

        Public Sub New(ByVal ThemeID As String, ByVal ThemeTranslationID As String, ByVal PrepareAddonID As String)
            Dim ThemePassword As Byte() = Nothing, ThemePasswordBase64 As String

            ThemePasswordBase64 = System.Configuration.ConfigurationManager.AppSettings.Item( _
                                                                                String.Format("{0}_Password", ThemeID) _
                                                                            )

            Try
                If Not String.IsNullOrEmpty(ThemePasswordBase64) Then ThemePassword = Convert.FromBase64String(ThemePasswordBase64)
            Catch ex As Exception
                ' Handle Wrong Base64 Strings

                ThemePassword = Nothing
            End Try

            Me._Deployment = New ThemeDeployment(ThemeID, ThemeTranslationID, ThemePassword)

            If Not PrepareAddonID Is Nothing Then
                Dim AddonInfos As PGlobals.ThemeInfo.AddonInfo() = Me._Deployment.Addons.AddonInfos

                If Not AddonInfos Is Nothing Then
                    For Each AddonInfo As PGlobals.ThemeInfo.AddonInfo In AddonInfos
                        If String.Compare(AddonInfo.AddonID, PrepareAddonID, True) = 0 Then
                            Me._Deployment.Addons.CreateInstance(AddonInfo)

                            Exit For
                        End If
                    Next
                End If
            Else
                SolidDevelopment.Web.General.CurrentInstantAddonID = Nothing
            End If
        End Sub

        Public Sub New(ByVal ThemeID As String, ByVal AddonID As String, ByVal AddonTranslationID As String, ByVal AddonPassword As Byte())
            Me._Deployment = New AddonDeployment(ThemeID, AddonID, AddonTranslationID, AddonPassword)
        End Sub

#End Region

        Public ReadOnly Property CurrentID() As String Implements PGlobals.ITheme.CurrentID
            Get
                Return Me._Deployment.CurrentID
            End Get
        End Property

        Public ReadOnly Property DeploymentType() As PGlobals.ThemeBase.DeploymentTypes Implements PGlobals.ITheme.DeploymentType
            Get
                Return Me._Deployment.DeploymentType
            End Get
        End Property

        Public ReadOnly Property DeploymentStyle() As PGlobals.ThemeBase.DeploymentStyles Implements PGlobals.ITheme.DeploymentStyle
            Get
                Return Me._Deployment.DeploymentStyle
            End Get
        End Property

        Public Function CheckTemplateExists(ByVal TemplateID As String) As Boolean
            Return Me._Deployment.CheckTemplateExists(TemplateID)
        End Function

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal PublicContentWebPath As String, ByVal RequestedFilePath As String)
            Me._Deployment.ProvideFileStream(FileStream, PublicContentWebPath, RequestedFilePath)
        End Sub

        Public ReadOnly Property Settings() As PGlobals.ITheme.ISettings Implements PGlobals.ITheme.Settings
            Get
                Return Me._Deployment.Settings
            End Get
        End Property

        Public ReadOnly Property Translation() As PGlobals.ITheme.ITranslation Implements PGlobals.ITheme.Translation
            Get
                Dim tClass As PGlobals.ITheme.ITranslation

                If Not Me._Deployment.Addons Is Nothing AndAlso _
                    Not Me._Deployment.Addons.CurrentInstance Is Nothing Then

                    tClass = Me._Deployment.Addons.CurrentInstance.Translation

                    ' Set Parent Translation
                    CType(tClass, TranslationClass).ParentTranslation = Me._Deployment.Translation
                Else
                    tClass = Me._Deployment.Translation
                End If

                Return tClass
            End Get
        End Property

        Public ReadOnly Property WebService() As PGlobals.ITheme.IWebService Implements PGlobals.ITheme.WebService
            Get
                Return Me._Deployment.WebService
            End Get
        End Property

        Public ReadOnly Property Addons() As PGlobals.ITheme.IAddons Implements PGlobals.ITheme.Addons
            Get
                Return Me._Deployment.Addons
            End Get
        End Property

        Private _xPathStream As IO.StringReader = Nothing
        Private ReadOnly Property ControlMapXPathNavigator() As Xml.XPath.XPathNavigator
            Get
                If Me._ControlMapXPathNavigator Is Nothing Then
                    Dim xPathDoc As Xml.XPath.XPathDocument = Nothing
                    Dim ControlMapContent As String = _
                            Me._Deployment.ProvideControlMapContent()

                    If Not Me._xPathStream Is Nothing Then Me._xPathStream.Close() : GC.SuppressFinalize(Me._xPathStream)

                    Me._xPathStream = New IO.StringReader(ControlMapContent)
                    xPathDoc = New Xml.XPath.XPathDocument(Me._xPathStream)

                    Me._ControlMapXPathNavigator = xPathDoc.CreateNavigator()
                End If

                Return Me._ControlMapXPathNavigator
            End Get
        End Property

        Public Function RenderTemplate(ByVal TemplateID As String, ByVal BlockID As String, ByVal Arguments As Globals.ArgumentInfo.ArgumentInfoCollection) As String
            Dim ThemeRenderer As New ThemeRenderer(Me)

            Dim RRI As Globals.RenderRequestInfo = _
                New Globals.RenderRequestInfo(TemplateID, BlockID, Arguments)
            RRI.RequestingBlockRendering = Not String.IsNullOrEmpty(BlockID)

            Return ThemeRenderer.RenderContent(RRI, RRI.MainContent)
        End Function

        Private Class ThemeRenderer
            Private _ParentInstance As Theme = Nothing
            Private _CurrentInstance As Theme = Nothing

            Public Sub New(ByVal ThemeInstance As Theme)
                If Not ThemeInstance.Addons.CurrentInstance Is Nothing Then
                    Me._ParentInstance = ThemeInstance
                    Me._CurrentInstance = CType(ThemeInstance.Addons.CurrentInstance, Theme)
                Else
                    Me._CurrentInstance = ThemeInstance
                End If
            End Sub

#Region " Template Rendering Procedures "
            Public Function RenderContent(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.ContentInfo) As String
                ' Define Control SecurityInfo Map
                Dim ControlSIM As Hashtable = _
                    CType( _
                        SolidDevelopment.Web.General.GetVariable("_sys_ControlSIM"),  _
                        Hashtable _
                    )
                If ControlSIM Is Nothing Then _
                    ControlSIM = Hashtable.Synchronized(New Hashtable)

                RRI.GlobalArguments.Add("_sys_ControlSIM", ControlSIM)
                ' !---

                Me.CompileRealTime(RRI, cI)
                Me.PrepareRenderedContent(RRI, cI)

                ' Save Control SecurityInfo Map to VariablePool
                SolidDevelopment.Web.General.SetVariable( _
                    "_sys_ControlSIM", RRI.GlobalArguments.Item("_sys_ControlSIM").Value)
                ' !---

                Return cI.RenderedContentValue
            End Function

            Private Sub CompileRealTime(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.ContentInfo)
                '\$  ( ( ([#]+|[\^\-\+\*\~])?(\w+) | (\w+\.)*\w+\@[\w+\.]+  ) \$ | [\.\w\-]+\:\{ | \w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\: ( [\.\w\-]+\$ | [\.\w\-]+\:\{ | [\.\w\-]+\?[\.\w\-]+   (\,   (   (\|)?    ( [#\.\^\-\+\*\~]*\w+  |  \=[\S]+  |  (\w+\.)*\w+\@[\w+\.]+ )?   )*  )?  \$ )) | \}\:[\.\w\-]+\:\{ | \}\:[\.\w\-]+ \$           [\w\.\,\-\+]
                Dim CaptureRegEx As String = "\$((([#]+|[\^\-\+\*\~])?(\w+)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)\$|[\.\w\-]+\:\{|\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:([\.\w\-]+\$|[\.\w\-]+\:\{|[\.\w\-]+\?[\.\w\-]+(\,((\|)?([#\.\^\-\+\*\~]*([\w+][^\$]*)|\=([\S+][^\$]*)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)?)*)?\$))|\}\:[\.\w\-]+\:\{|\}\:[\.\w\-]+\$"
                Dim BracketedRegExOpening As String = "\$((?<ItemID>\w+)|\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?\:(?<ItemID>[\.\w\-]+))\:\{"
                Dim BracketedRegExSeparator As String = "\}:(?<ItemID>[\.\w\-]+)\:\{"
                Dim BracketedRegExClosing As String = "\}:(?<ItemID>[\.\w\-]+)\$"

                Dim lMatchItem01 As System.Text.RegularExpressions.Match, lMatchItem02 As System.Text.RegularExpressions.Match
                Dim tMatchItem01 As System.Text.RegularExpressions.Match, MatchedID01 As String = Nothing ' For Opening Brackets
                Dim tMatchItem02 As System.Text.RegularExpressions.Match ' For Separator Brackets
                Dim tMatchItem03 As System.Text.RegularExpressions.Match ' For Closing Brackets
                Dim tRegExMatches As System.Text.RegularExpressions.MatchCollection = _
                        System.Text.RegularExpressions.Regex.Matches( _
                                                            cI.OriginalValue, _
                                                            CaptureRegEx, _
                                                            Text.RegularExpressions.RegexOptions.Multiline And Text.RegularExpressions.RegexOptions.Compiled _
                                                        )

                Dim REMEnum As IEnumerator = _
                    tRegExMatches.GetEnumerator()

                Do While REMEnum.MoveNext()
                    lMatchItem01 = CType(REMEnum.Current, System.Text.RegularExpressions.Match)

                    ' Exam For Bracketed Regex Result
                    tMatchItem01 = System.Text.RegularExpressions.Regex.Match(lMatchItem01.Value, BracketedRegExOpening)

                    If tMatchItem01.Success Then
                        MatchedID01 = tMatchItem01.Result("${ItemID}")

                        If Not MatchedID01 Is Nothing Then
                            Dim InnerMatch As Integer = 0
                            Dim SeparatorIndexes As New Generic.List(Of Integer)

                            Do While REMEnum.MoveNext()
                                lMatchItem02 = CType(REMEnum.Current, System.Text.RegularExpressions.Match)

                                ' Exam For Opening Bracketed Regex Result
                                tMatchItem01 = System.Text.RegularExpressions.Regex.Match(lMatchItem02.Value, BracketedRegExOpening)

                                ' Exam For Separator Bracketed Regex Result
                                tMatchItem02 = System.Text.RegularExpressions.Regex.Match(lMatchItem02.Value, BracketedRegExSeparator)

                                ' Exam For Closing Bracketed Regex Result
                                tMatchItem03 = System.Text.RegularExpressions.Regex.Match(lMatchItem02.Value, BracketedRegExClosing)

                                If tMatchItem01.Success AndAlso _
                                    String.Compare(MatchedID01, tMatchItem01.Result("${ItemID}")) = 0 Then ' Check is Another Same Named Control Internally Opened Bracket

                                    InnerMatch += 1
                                ElseIf tMatchItem02.Success AndAlso _
                                    String.Compare(MatchedID01, tMatchItem02.Result("${ItemID}")) = 0 AndAlso _
                                    InnerMatch = 0 Then ' Check is Same Named Highlevel Control Separator Bracket

                                    ' Point the location of Separator Bracket index
                                    SeparatorIndexes.Add(lMatchItem02.Index - lMatchItem01.Index)
                                ElseIf tMatchItem03.Success AndAlso _
                                    String.Compare(MatchedID01, tMatchItem03.Result("${ItemID}")) = 0 Then ' Check is Same Named Control Internally Closed Bracket

                                    If InnerMatch = 0 Then
                                        Dim ModifierText As String = String.Format("~{0}", lMatchItem01.Index)
                                        Dim PointedOriginalValue As String = _
                                            cI.OriginalValue.Substring( _
                                                lMatchItem01.Index, _
                                                (lMatchItem02.Index + lMatchItem02.Length) - lMatchItem01.Index _
                                            )

                                        PointedOriginalValue = PointedOriginalValue.Insert(PointedOriginalValue.Length - 1, ModifierText)
                                        For idxID As Integer = SeparatorIndexes.Count - 1 To 0 Step -1
                                            PointedOriginalValue = PointedOriginalValue.Insert( _
                                                                        (SeparatorIndexes(idxID) + String.Format("}}:{0}", MatchedID01).Length), _
                                                                        ModifierText _
                                                                    )
                                        Next
                                        PointedOriginalValue = PointedOriginalValue.Insert(lMatchItem01.Length - 2, ModifierText)

                                        Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                            New Globals.RenderRequestInfo.CommonControlContent( _
                                                                            cI, _
                                                                            lMatchItem01.Index, _
                                                                            PointedOriginalValue, _
                                                                            (ModifierText.Length) * (SeparatorIndexes.Count + 2), _
                                                                            cI.ContentArguments _
                                                                        )

                                        ' Render Content
                                        Me.RenderContentInfo(RRI, tCI, True)

                                        If RRI.RequestingBlockRendering Then
                                            Select Case RRI.BlockRenderingStatus
                                                Case Globals.RenderRequestInfo.BlockRenderingStatuses.Rendered
                                                    cI = tCI
                                                Case Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering
                                                    cI.ContentItems.Add(tCI)
                                            End Select
                                        Else
                                            cI.ContentItems.Add(tCI)
                                        End If

                                        Exit Do
                                    Else
                                        InnerMatch -= 1
                                    End If
                                End If
                            Loop
                        End If
                    Else
                        Dim tCI As Globals.RenderRequestInfo.ContentInfo

                        Select Case Me.CaptureContentType(lMatchItem01.Value)
                            Case Globals.RenderRequestInfo.ContentInfo.ContentTypes.CommonControl
                                tCI = New Globals.RenderRequestInfo.CommonControlContent(cI, lMatchItem01.Index, lMatchItem01.Value, 0, cI.ContentArguments)
                            Case Globals.RenderRequestInfo.ContentInfo.ContentTypes.SpecialProperty
                                tCI = New Globals.RenderRequestInfo.SpecialPropertyContent(cI.Parent, lMatchItem01.Index, lMatchItem01.Value, cI.ContentArguments)
                            Case Else
                                Throw New Exception("Match value must have content type!")
                        End Select

                        ' Render Content
                        Me.RenderContentInfo(RRI, tCI, True)

                        If RRI.RequestingBlockRendering Then
                            Select Case RRI.BlockRenderingStatus
                                Case Globals.RenderRequestInfo.BlockRenderingStatuses.Rendered
                                    cI = tCI
                                Case Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering
                                    cI.ContentItems.Add(tCI)
                            End Select
                        Else
                            cI.ContentItems.Add(tCI)
                        End If
                    End If

                    If RRI.RequestingBlockRendering AndAlso _
                        RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendered Then

                        Exit Do
                    End If
                Loop
                cI.HelperSpace.DefineAsRendered()
            End Sub

            Private Sub PrepareRenderedContent(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.ContentInfo)
                Me.RenderContentInfo(RRI, cI, False)

                Dim shift As Integer = 0

                cI.HelperSpace.Space = cI.RenderedContentValue
                For Each iCI As Globals.RenderRequestInfo.ContentInfo In cI.ContentItems
                    If iCI.ContentItems.Count > 0 Then Me.PrepareRenderedContent(RRI, iCI)

                    Me.RenderContentInfo(RRI, iCI, False)

                    cI.HelperSpace.Space = cI.HelperSpace.Space.Remove(iCI.OriginalStartIndex + shift, (iCI.OriginalLength - iCI.ModifierTuneLength))
                    cI.HelperSpace.Space = cI.HelperSpace.Space.Insert(iCI.OriginalStartIndex + shift, iCI.RenderedContentValue)

                    shift += iCI.RenderedContentValue.Length - (iCI.OriginalLength - iCI.ModifierTuneLength)
                Next
                If cI.ContentType <> Globals.RenderRequestInfo.ContentInfo.ContentTypes.Renderless AndAlso _
                    String.Compare(cI.OriginalValue, cI.HelperSpace.Space) = 0 Then

                    cI.HelperSpace.Space = Nothing
                End If
                cI.HelperSpace.DefineAsRendered()
            End Sub

            Private Function CaptureContentType(ByVal Content As String) As Globals.RenderRequestInfo.ContentInfo.ContentTypes
                Dim rContentType As Globals.RenderRequestInfo.ContentInfo.ContentTypes = _
                    Globals.RenderRequestInfo.ContentInfo.ContentTypes.Renderless

                If Not String.IsNullOrEmpty(Content) Then

                    Dim CPIDMatch As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(Content, "\$((\w(\<\d+(\+)?\>)?(\[[\.\w\-]+\])?)|(\w+))\:")

                    If CPIDMatch.Success Then
                        rContentType = Globals.RenderRequestInfo.ContentInfo.ContentTypes.CommonControl
                    Else
                        rContentType = Globals.RenderRequestInfo.ContentInfo.ContentTypes.SpecialProperty
                    End If
                End If

                Return rContentType
            End Function

            Private Sub RenderContentInfo(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.ContentInfo, ByVal IsRunTime As Boolean)
                Try
                    If Not cI.IsContentRendered Then
                        Select Case cI.ContentType
                            Case Globals.RenderRequestInfo.ContentInfo.ContentTypes.CommonControl
                                Me.RenderCommonControl(RRI, CType(cI, Globals.RenderRequestInfo.CommonControlContent))
                            Case Globals.RenderRequestInfo.ContentInfo.ContentTypes.SpecialProperty
                                If Not IsRunTime Then Me.RenderSpecialProperty(RRI, CType(cI, Globals.RenderRequestInfo.SpecialPropertyContent))
                            Case Globals.RenderRequestInfo.ContentInfo.ContentTypes.Renderless
                                If Not IsRunTime Then cI.HelperSpace.DefineAsRendered()
                        End Select
                    End If
                Catch ex As Exception
                    If SolidDevelopment.Web.Configurations.Debugging Then
                        Dim ExceptionString As String = Nothing

                        Do
                            ExceptionString = _
                                String.Format( _
                                    "<div align='left' style='border: solid 1px #660000; background-color: #FFFFFF'><div align='left' style='font-weight: bolder; color:#FFFFFF; background-color:#CC0000; padding: 4px;'>{0}</div><br><div align='left' style='padding: 4px'>{1}{2}</div></div>", ex.Message, ex.Source, IIf(Not ExceptionString Is Nothing, "<hr size='1px' />" & ExceptionString, Nothing))

                            ex = ex.InnerException
                        Loop Until ex Is Nothing

                        cI.HelperSpace.Space = ExceptionString
                        cI.HelperSpace.DefineAsRendered()
                    End If
                End Try
            End Sub

            Private Sub RenderTemplateInternal(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.CommonControlContent)
                ' Function Variables
                Dim IsInstanceShifted As Boolean = False
                ' !--

                ' Shift Parent Instance To Current
                If Me._ParentInstance Is Nothing AndAlso _
                    Not Me._CurrentInstance.Addons.CurrentInstance Is Nothing Then

                    Me._ParentInstance = Me._CurrentInstance
                    Me._CurrentInstance = CType(Me._CurrentInstance.Addons.CurrentInstance, Theme)

                    IsInstanceShifted = True
                End If
                ' !--

                Dim TemplateContent As String = String.Empty
                Try
                    Dim tCI As Globals.RenderRequestInfo.RenderlessContent = _
                        New Globals.RenderRequestInfo.RenderlessContent( _
                            cI.Parent, _
                            0, _
                            Me._CurrentInstance._Deployment.ProvideTemplateContent(cI.CommonControlID) _
                        )

                    cI.HelperSpace.Space = Me.RenderContent(RRI, CType(tCI, Globals.RenderRequestInfo.ContentInfo))
                Catch ex As IO.FileNotFoundException
                    If Not Me._ParentInstance Is Nothing Then
                        Try
                            Dim tCI As Globals.RenderRequestInfo.RenderlessContent = _
                                New Globals.RenderRequestInfo.RenderlessContent( _
                                    cI.Parent, _
                                    0, _
                                    Me._ParentInstance._Deployment.ProvideTemplateContent(cI.CommonControlID) _
                               )

                            cI.HelperSpace.Space = Me.RenderContent(RRI, CType(tCI, Globals.RenderRequestInfo.ContentInfo))
                        Catch exSub As Exception
                            RRI.ReturnException = exSub
                        End Try
                    Else
                        RRI.ReturnException = ex
                    End If
                Catch ex As Exception
                    RRI.ReturnException = ex
                End Try
                cI.HelperSpace.DefineAsRendered()

                ' Shift Parent Instance To Current
                If IsInstanceShifted Then
                    Me._CurrentInstance = Me._ParentInstance
                    Me._ParentInstance = Nothing
                End If
                ' !--
            End Sub

            Private Sub RenderSpecialProperty(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.SpecialPropertyContent)
                If cI.ClearedValue Is Nothing OrElse _
                    cI.ClearedValue.Trim().Length = 0 Then

                    cI.HelperSpace.Space = "$"
                Else
                    Select Case cI.ClearedValue
                        Case "ThemePublicContents"
                            If Me._ParentInstance Is Nothing Then
                                cI.HelperSpace.Space = SolidDevelopment.Web.General.GetThemePublicContentsPath(Me._CurrentInstance.CurrentID, Me._CurrentInstance.Translation.CurrentTranslationID, Nothing)
                            Else
                                cI.HelperSpace.Space = SolidDevelopment.Web.General.GetThemePublicContentsPath(Me._ParentInstance.CurrentID, Me._ParentInstance.Translation.CurrentTranslationID, Me._CurrentInstance.CurrentID)
                            End If

                        Case "PageRenderDuration"
                            cI.HelperSpace.Space = "<!--_sys_PAGERENDERDURATION-->"

                        Case Else
                            Select Case cI.ClearedValue.Chars(0)
                                Case "^"c ' QueryString Value
                                    Dim QueryValue As String = _
                                        SolidDevelopment.Web.General.Context.Request.QueryString.Item(cI.ClearedValue.Substring(1))

                                    Select Case SolidDevelopment.Web.Configurations.RequestTagFiltering
                                        Case PGlobals.RequestTagFilteringTypes.OnlyQuery, PGlobals.RequestTagFilteringTypes.Both
                                            Dim ArgumentNameForCompare As String = _
                                                cI.ClearedValue.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                            If Array.IndexOf(SolidDevelopment.Web.Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                                QueryValue = Assembly.CleanHTMLTags(QueryValue, SolidDevelopment.Web.Configurations.RequestTagFilteringItems)
                                    End Select

                                    cI.HelperSpace.Space = QueryValue

                                Case "~"c ' Form Post Value
                                    If String.Compare(General.Context.Request.HttpMethod, "POST", True) = 0 Then
                                        Dim ControlSIM As Hashtable = _
                                            CType(RRI.GlobalArguments.Item("_sys_ControlSIM").Value, Hashtable)

                                        If Not ControlSIM Is Nothing AndAlso _
                                            ControlSIM.ContainsKey(cI.ClearedValue.Substring(1)) Then

                                            cI.HelperSpace.Space = CType(ControlSIM.Item(cI.ClearedValue.Substring(1)), String)
                                        Else
                                            Dim FormValue As String = _
                                                SolidDevelopment.Web.General.Context.Request.Form.Item(cI.ClearedValue.Substring(1))

                                            Select Case SolidDevelopment.Web.Configurations.RequestTagFiltering
                                                Case PGlobals.RequestTagFilteringTypes.OnlyForm, PGlobals.RequestTagFilteringTypes.Both
                                                    Dim ArgumentNameForCompare As String = _
                                                        cI.ClearedValue.Substring(1).ToLower(New Globalization.CultureInfo("en-US"))

                                                    If Array.IndexOf(SolidDevelopment.Web.Configurations.RequestTagFilteringExceptions, ArgumentNameForCompare) = -1 Then _
                                                        FormValue = Assembly.CleanHTMLTags(FormValue, SolidDevelopment.Web.Configurations.RequestTagFilteringItems)
                                            End Select

                                            cI.HelperSpace.Space = FormValue
                                        End If
                                    Else
                                        cI.HelperSpace.Space = Nothing
                                    End If

                                Case "-"c ' Session Value
                                    cI.HelperSpace.Space = CType(SolidDevelopment.Web.General.Context.Session.Contents.Item(cI.ClearedValue.Substring(1)), String)

                                Case "+"c ' Cookies Value
                                    Try
                                        cI.HelperSpace.Space = SolidDevelopment.Web.General.Context.Request.Cookies.Item(cI.ClearedValue.Substring(1)).Value
                                    Catch ex As Exception
                                        cI.HelperSpace.Space = Nothing
                                    End Try

                                Case "="c ' Value which following after '='
                                    cI.HelperSpace.Space = cI.ClearedValue.Substring(1)

                                Case "#"c ' DataTable Field
                                    Dim searchContentInfo As Globals.RenderRequestInfo.ContentInfo = cI
                                    Dim searchVariableName As String = cI.ClearedValue

                                    Do
                                        searchVariableName = searchVariableName.Substring(1)

                                        If searchVariableName.IndexOf("#") = 0 Then
                                            searchContentInfo = searchContentInfo.Parent
                                        Else
                                            Exit Do
                                        End If
                                    Loop Until searchContentInfo Is Nothing

                                    If Not searchContentInfo Is Nothing Then
                                        Dim argItem As Globals.ArgumentInfo = _
                                            searchContentInfo.ContentArguments.Item(searchVariableName)

                                        If Not argItem.Value Is Nothing AndAlso _
                                            Not argItem.Value.GetType() Is GetType(System.DBNull) Then

                                            cI.HelperSpace.Space = CType(argItem.Value, String)
                                        Else
                                            cI.HelperSpace.Space = Nothing
                                        End If
                                    Else
                                        cI.HelperSpace.Space = Nothing
                                    End If
                                Case "*"c ' Search in All orderby : [InData, DataField, Session, Form Post, QueryString, Cookie]
                                    Dim searchArgName As String = cI.ClearedValue.Substring(1)
                                    Dim searchArgValue As String

                                    ' Search InDatas
                                    searchArgValue = CType(SolidDevelopment.Web.General.GetVariable(searchArgName), String)

                                    ' Search In DataFields (GlobalArguments, ContentArguments)
                                    If String.IsNullOrEmpty(searchArgValue) Then
                                        Dim argItem As Globals.ArgumentInfo = _
                                            Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, cI.ContentArguments).Item(searchArgName)

                                        If Not argItem.Value Is Nothing AndAlso _
                                            Not argItem.Value.GetType() Is GetType(System.DBNull) Then

                                            searchArgValue = CType(argItem.Value, String)
                                        Else
                                            searchArgValue = Nothing
                                        End If
                                    End If

                                    ' Search In Session
                                    If String.IsNullOrEmpty(searchArgValue) Then searchArgValue = CType(SolidDevelopment.Web.General.Context.Session.Contents.Item(searchArgName), String)

                                    ' Search In Form Post
                                    If String.IsNullOrEmpty(searchArgValue) AndAlso _
                                        String.Compare(General.Context.Request.HttpMethod, "POST", True) = 0 Then

                                        Dim ControlSIM As Hashtable = _
                                            CType(RRI.GlobalArguments.Item("_sys_ControlSIM").Value, Hashtable)

                                        If Not ControlSIM Is Nothing AndAlso _
                                            ControlSIM.ContainsKey(searchArgName) Then

                                            searchArgValue = CType(ControlSIM.Item(searchArgName), String)
                                        Else
                                            searchArgValue = SolidDevelopment.Web.General.Context.Request.Form.Item(searchArgName)
                                        End If
                                    Else
                                        searchArgValue = Nothing
                                    End If

                                    ' Search QueryString
                                    If String.IsNullOrEmpty(searchArgValue) Then searchArgValue = SolidDevelopment.Web.General.Context.Request.QueryString.Item(searchArgName)

                                    ' Cookie
                                    If String.IsNullOrEmpty(searchArgValue) AndAlso _
                                        Not SolidDevelopment.Web.General.Context.Request.Cookies.Item(searchArgName) Is Nothing Then

                                        searchArgValue = SolidDevelopment.Web.General.Context.Request.Cookies.Item(searchArgName).Value
                                    End If

                                    cI.HelperSpace.Space = searchArgValue

                                Case Else ' Search in Values Set for Current Request Session
                                    If cI.ClearedValue.IndexOf("@"c) > 0 Then
                                        Dim ArgumentQueryObjectName As String = _
                                            cI.ClearedValue.Substring(cI.ClearedValue.IndexOf("@"c) + 1)

                                        Dim ArgumentQueryObject As Object = Nothing

                                        Select Case ArgumentQueryObjectName.Chars(0)
                                            Case "-"c
                                                ArgumentQueryObject = SolidDevelopment.Web.General.Context.Session.Contents.Item(ArgumentQueryObjectName.Substring(1))
                                            Case "#"c
                                                Dim searchContentInfo As Globals.RenderRequestInfo.ContentInfo = cI
                                                Dim searchVariableName As String = ArgumentQueryObjectName

                                                Do
                                                    searchVariableName = searchVariableName.Substring(1)

                                                    If searchVariableName.IndexOf("#") = 0 Then
                                                        searchContentInfo = searchContentInfo.Parent
                                                    Else
                                                        Exit Do
                                                    End If
                                                Loop Until searchContentInfo Is Nothing

                                                If Not searchContentInfo Is Nothing Then
                                                    Dim argItem As Globals.ArgumentInfo = _
                                                        searchContentInfo.ContentArguments.Item(searchVariableName)

                                                    If Not argItem.Value Is Nothing Then ArgumentQueryObject = argItem.Value
                                                End If
                                            Case Else
                                                ArgumentQueryObject = SolidDevelopment.Web.General.GetVariable(ArgumentQueryObjectName)
                                        End Select

                                        If Not ArgumentQueryObject Is Nothing Then
                                            Dim ArgumentCallList As String() = cI.ClearedValue.Substring(0, cI.ClearedValue.IndexOf("@"c)).Split("."c)
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
                                            Catch ex As Exception
                                                ArgumentValue = Nothing
                                            End Try

                                            cI.HelperSpace.Space = CType(ArgumentValue, String)
                                        Else
                                            cI.HelperSpace.Space = Nothing
                                        End If
                                    Else
                                        cI.HelperSpace.Space = CType(SolidDevelopment.Web.General.GetVariable(cI.ClearedValue), String)
                                    End If

                            End Select

                    End Select
                End If
                cI.HelperSpace.DefineAsRendered()
            End Sub

            Private Sub RenderCommonControl(ByRef RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.CommonControlContent)
                Try
                    Select Case cI.ClearedValue.Split(":"c)(0).Chars(0)
                        Case "T"c ' Template Prefix
                            cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Template

                            Dim AddonSearched As Boolean = False
                            Dim TemplateID As String = cI.ClearedValue.Split(":"c)(1)

                            Dim sI_t As SettingsClass.ServicesClass.ServiceItem = _
                                CType(Me._CurrentInstance.Settings, SettingsClass).Services.ServiceItems.GetServiceItem( _
                                            SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                            TemplateID)

                            If sI_t Is Nothing AndAlso _
                                Not Me._ParentInstance Is Nothing Then

                                sI_t = CType(Me._ParentInstance.Settings, SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                    SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                    TemplateID)

                                AddonSearched = True
                            End If

                            If Not sI_t Is Nothing Then
                                If sI_t.Overridable AndAlso _
                                    Not Me._CurrentInstance.Addons Is Nothing AndAlso _
                                    Not AddonSearched Then

                                    Dim sI_a As SettingsClass.ServicesClass.ServiceItem

                                    For Each Addon As PGlobals.ThemeInfo.AddonInfo In Me._CurrentInstance.Addons.AddonInfos
                                        Me._CurrentInstance.Addons.CreateInstance(Addon)

                                        sI_a = CType(Me._CurrentInstance.Addons.CurrentInstance.Settings, SettingsClass).Services.ServiceItems.GetServiceItem( _
                                                                            SettingsClass.ServicesClass.ServiceItem.ServiceTypes.template, _
                                                                            sI_t.ID)

                                        If Not sI_a Is Nothing Then
                                            If sI_a.Authentication AndAlso _
                                                sI_a.AuthenticationKeys.Length = 0 Then

                                                sI_a.AuthenticationKeys = sI_t.AuthenticationKeys
                                            End If

                                            sI_t = sI_a

                                            Exit For
                                        Else
                                            Me._CurrentInstance.Addons.DisposeInstance()
                                        End If
                                    Next
                                End If

                                If sI_t.Authentication Then
                                    Dim LocalAuthenticationNotAccepted As Boolean = False

                                    For Each AuthKey As String In sI_t.AuthenticationKeys
                                        If SolidDevelopment.Web.General.Context.Session.Contents.Item(AuthKey) Is Nothing Then
                                            LocalAuthenticationNotAccepted = True

                                            Exit For
                                        End If
                                    Next

                                    If Not LocalAuthenticationNotAccepted Then
                                        GoTo RenderTemplate
                                    Else
                                        Dim SystemMessage As String = Me._CurrentInstance.Translation.GetTranslation("TEMPLATE_AUTH")

                                        If String.IsNullOrEmpty(SystemMessage) Then SystemMessage = Globals.SystemMessages.TEMPLATE_AUTH

                                        cI.HelperSpace.Space = "<div style='width:100%; font-weight:bolder; color:#CC0000; text-align:center'>" & SystemMessage & "!</div>"
                                    End If
                                Else
RENDERTEMPLATE:
                                    cI.CommonControlID = TemplateID

                                    ' Render Template
                                    Me.RenderTemplateInternal(RRI, cI)
                                End If
                            End If

                            If Not RRI.ReturnException Is Nothing Then Throw New Exception("Parsing Error!", RRI.ReturnException)

                        Case "L"c ' Translation Prefix
                            cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Translation

                            If Not RRI.RequestingBlockRendering Then
TRANSLATEVALUE:
                                cI.CommonControlID = cI.ClearedValue.Split(":"c)(1)
                                cI.HelperSpace.Space = Me._CurrentInstance.Translation.GetTranslation(cI.CommonControlID)

                                If String.IsNullOrEmpty(cI.HelperSpace.Space) AndAlso _
                                    Not Me._ParentInstance Is Nothing Then

                                    cI.HelperSpace.Space = Me._ParentInstance.Translation.GetTranslation(cI.CommonControlID)
                                End If
                            Else
                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then GoTo TRANSLATEVALUE
                            End If

                        Case "C"c ' Control Prefix
                            Dim CapturedParentID As String = String.Empty

                            Dim IsLocalLeveling As Boolean = True
                            Dim CLevelingMatch As System.Text.RegularExpressions.Match = _
                                System.Text.RegularExpressions.Regex.Match(cI.ClearedValue.Split(":"c)(0), "\<\d+(\+)?\>")

                            If CLevelingMatch.Success Then
                                Dim Leveling As Integer
                                ' Trim < and > character from match result
                                Dim CleanValue As String = _
                                    CLevelingMatch.Value.Substring(1, CLevelingMatch.Value.Length - 2)

                                If CleanValue.IndexOf("+"c) > -1 Then
                                    IsLocalLeveling = False

                                    CleanValue = CleanValue.Substring(0, CleanValue.IndexOf("+"c))
                                End If

                                Integer.TryParse(CleanValue, Leveling)

                                cI.Leveling(IsLocalLeveling) = Leveling
                            End If

                            If cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Undefined Then
                                Dim CPIDMatch As System.Text.RegularExpressions.Match = _
                                    System.Text.RegularExpressions.Regex.Match(cI.ClearedValue.Split(":"c)(0), "\[[\.\w\-]+\]")

                                If CPIDMatch.Success Then
                                    cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.ControlWithParent

                                    ' Trim [ and ] character from match result
                                    CapturedParentID = CPIDMatch.Value.Substring(1, CPIDMatch.Value.Length - 2)
                                Else
                                    cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Control
                                End If
                            End If

                            If Not String.IsNullOrEmpty(CapturedParentID) Then
                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = cI

                                Do Until tCI.Parent Is Nothing
                                    If TypeOf tCI.Parent Is Globals.RenderRequestInfo.CommonControlContent Then
                                        If String.Compare( _
                                            CType(tCI.Parent, Globals.RenderRequestInfo.CommonControlContent).CommonControlID, CapturedParentID, True) = 0 Then

                                            Throw New Exception("Parented Control can not be located inside its parent!")
                                        End If
                                    End If

                                    tCI = tCI.Parent
                                Loop

                                ' Register ControlInfo to ParentPendingControlInfoList
                                Dim PPCI As New Globals.RenderRequestInfo.CommonControlContent( _
                                                    cI.Parent, _
                                                    cI.OriginalStartIndex, _
                                                    cI.OriginalValue, _
                                                    cI.ModifierTuneLength, _
                                                    cI.ContentArguments _
                                                )
                                PPCI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Control
                                PPCI.Leveling(IsLocalLeveling) = cI.Leveling

                                cI = PPCI

                                If Not RRI.ParentPendingCommonControlContents.ContainsKey(CapturedParentID) Then _
                                    RRI.ParentPendingCommonControlContents.Add(CapturedParentID, New Generic.List(Of Globals.RenderRequestInfo.CommonControlContent))

                                RRI.ParentPendingCommonControlContents.Item(CapturedParentID).Add(PPCI)
                            Else
                                Me.RenderControlContent(RRI, cI)
                            End If

                        Case "P"c ' HashCode Parser
                            cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.HashCodeParser

                            Dim controlValueSplitted As String() = _
                                cI.ClearedValue.Split(":"c)

                            cI.HelperSpace.Space = String.Format("{0}/{1}", General.HashCode, controlValueSplitted(1))

                        Case "F"c ' Direct Function Call Prefix
                            If Not RRI.RequestingBlockRendering Then
RENDERMETHODRESULT:
                                Dim controlValueSplitted As String() = _
                                    cI.ClearedValue.Split(":"c)

                                Dim IsLocalLeveling As Boolean = True
                                Dim CLevelingMatch As System.Text.RegularExpressions.Match = _
                                    System.Text.RegularExpressions.Regex.Match(controlValueSplitted(0), "\<\d+(\+)?\>")

                                If CLevelingMatch.Success Then
                                    Dim Leveling As Integer
                                    ' Trim < and > character from match result
                                    Dim CleanValue As String = _
                                        CLevelingMatch.Value.Substring(1, CLevelingMatch.Value.Length - 2)

                                    If CleanValue.IndexOf("+"c) > -1 Then
                                        IsLocalLeveling = False

                                        CleanValue = CleanValue.Substring(0, CleanValue.IndexOf("+"c))
                                    End If

                                    Integer.TryParse(CleanValue, Leveling)

                                    cI.Leveling(IsLocalLeveling) = Leveling
                                End If

                                Dim CapturedParentID As String = String.Empty

                                If cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.Undefined Then
                                    Dim CPIDMatch As System.Text.RegularExpressions.Match = _
                                        System.Text.RegularExpressions.Regex.Match(controlValueSplitted(0), "\[[\.\w\-]+\]")

                                    If CPIDMatch.Success Then
                                        cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.DirectCallFunctionWithParent

                                        ' Trim [ and ] character from match result
                                        CapturedParentID = CPIDMatch.Value.Substring(1, CPIDMatch.Value.Length - 2)
                                    Else
                                        cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.DirectCallFunction
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(CapturedParentID) Then
                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = cI

                                    Do Until tCI.Parent Is Nothing
                                        If TypeOf tCI.Parent Is Globals.RenderRequestInfo.CommonControlContent Then
                                            If String.Compare( _
                                                CType(tCI.Parent, Globals.RenderRequestInfo.CommonControlContent).CommonControlID, CapturedParentID, True) = 0 Then

                                                Throw New Exception("Parented Direct Call Function can not be located inside its parent!")
                                            End If
                                        End If

                                        tCI = tCI.Parent
                                    Loop

                                    ' Register ControlInfo to ParentPendingControlInfoList
                                    Dim PPCI As New Globals.RenderRequestInfo.CommonControlContent( _
                                                        cI.Parent, _
                                                        cI.OriginalStartIndex, _
                                                        cI.OriginalValue, _
                                                        cI.ModifierTuneLength, _
                                                        cI.ContentArguments _
                                                    )
                                    PPCI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.DirectCallFunction
                                    PPCI.Leveling(IsLocalLeveling) = cI.Leveling

                                    cI = PPCI

                                    If Not RRI.ParentPendingCommonControlContents.ContainsKey(CapturedParentID) Then _
                                        RRI.ParentPendingCommonControlContents.Add(CapturedParentID, New Generic.List(Of Globals.RenderRequestInfo.CommonControlContent))

                                    RRI.ParentPendingCommonControlContents.Item(CapturedParentID).Add(PPCI)
                                Else
                                    Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                                    Dim ControlContentArguments As SolidDevelopment.Web.Globals.ArgumentInfo.ArgumentInfoCollection = _
                                        cI.Parent.ContentArguments
                                    Dim Leveling As Integer = cI.Leveling

                                    Do
                                        If Leveling > 0 Then _
                                            ControlContentArguments = ControlContentArguments.Parent : Leveling -= 1
                                    Loop Until ControlContentArguments Is Nothing OrElse Leveling = 0

                                    If Me._ParentInstance Is Nothing Then
PARENTCALL:
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1), Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))
                                    Else
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            Me._ParentInstance.CurrentID, _
                                                            Me._CurrentInstance.CurrentID, _
                                                            String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1), Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                                            GoTo PARENTCALL
                                        End If
                                    End If

                                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                        TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                                        Throw New Exception("Direct Function Call Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
                                    Else
                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder Then

                                            If SolidDevelopment.Web.General.Context.Items.Contains("RedirectLocation") Then
                                                SolidDevelopment.Web.General.Context.Items.Remove("RedirectLocation")
                                            End If

                                            SolidDevelopment.Web.General.Context.Items.Add( _
                                                "RedirectLocation", _
                                                CType(tAssembleResultInfo.MethodResult, SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder).Location _
                                            )

                                            cI.HelperSpace.Space = Nothing
                                        Else
                                            cI.HelperSpace.Space = SolidDevelopment.Web.PGlobals.Execution.GetPrimitiveValue(tAssembleResultInfo.MethodResult)
                                        End If
                                    End If
                                End If
                            Else
                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then GoTo RENDERMETHODRESULT
                            End If

                        Case "S"c ' Primitive Statement
                            cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.StatementBlock

                            If Not RRI.RequestingBlockRendering Then
COMPILEPRIMITIVESTATEMENT:
                                Dim controlValueSplitted As String() = _
                                    cI.ClearedValue.Split(":"c)

                                Dim BlockValue As String = String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1)

                                ' Check This Control has a Content
                                Dim idxCon As Integer = _
                                    BlockValue.IndexOf(":"c)

                                ' Get ControlID Accourding to idxCon Value -1 = no content, else has content
                                If idxCon = -1 Then
                                    ' No Content

                                    Throw New Exception("Statement Grammer Error!")
                                Else
                                    cI.CommonControlID = BlockValue.Substring(0, idxCon)
                                End If

                                Dim _sys_CommonControlID As String = cI.CommonControlID
                                Dim idxTuner As Integer = cI.CommonControlID.IndexOf("~"c)
                                If idxTuner > -1 Then _
                                    cI.CommonControlID = cI.CommonControlID.Substring(0, idxTuner)

                                Dim CoreContent As String = Nothing
                                Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

                                idxCoreContStart = BlockValue.IndexOf(String.Format("{0}:{{", _sys_CommonControlID)) + String.Format("{0}:{{", _sys_CommonControlID).Length

RESEARCH_S_END:
                                idxCoreContEnd = BlockValue.IndexOf(String.Format("}}:{0}", _sys_CommonControlID), idxCoreContEnd + 1)
                                Dim TestEndContent As String = BlockValue.Substring(idxCoreContEnd + String.Format("}}:{0}", _sys_CommonControlID).Length)
                                If TestEndContent.Length > 0 AndAlso _
                                    Not Char.IsWhiteSpace(TestEndContent, 0) Then

                                    GoTo RESEARCH_S_END
                                End If

                                If idxCoreContStart <> -1 AndAlso _
                                    idxCoreContEnd <> -1 Then

                                    CoreContent = BlockValue.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                                    If Not CoreContent Is Nothing AndAlso _
                                        CoreContent.Trim().Length > 0 Then

                                        Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                            New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, CoreContent)

                                        CoreContent = Me.RenderContent(RRI, tCI)

                                        If Not CoreContent Is Nothing AndAlso _
                                            CoreContent.Trim().Length > 0 Then

                                            Dim tMethodResultInfo As Object = Nothing

                                            If Me._ParentInstance Is Nothing Then
                                                tMethodResultInfo = [Assembly].ExecuteStatement(Me._CurrentInstance.CurrentID, Nothing, cI.CommonControlID, CoreContent.Trim())
                                            Else
                                                tMethodResultInfo = [Assembly].ExecuteStatement(Me._ParentInstance.CurrentID, Me._CurrentInstance.CurrentID, cI.CommonControlID, CoreContent.Trim())
                                            End If

                                            If Not tMethodResultInfo Is Nothing AndAlso _
                                                TypeOf tMethodResultInfo Is Exception Then

                                                Throw New Exception("Statement Execution Error!", Me.PrepareException("External Call Error!", CType(tMethodResultInfo, Exception).Message, CType(tMethodResultInfo, Exception).InnerException))
                                            Else
                                                cI.HelperSpace.Space = SolidDevelopment.Web.PGlobals.Execution.GetPrimitiveValue(tMethodResultInfo)
                                            End If
                                        Else
                                            Throw New Exception("Empty Statement Block Error!")
                                        End If
                                    Else
                                        Throw New Exception("Empty Statement Block Error!")
                                    End If
                                Else
                                    Throw New Exception("Statement Grammer Error!")
                                End If
                            Else
                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then GoTo TRANSLATEVALUE
                            End If

                        Case "H"c
                            cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.RequestBlock

                            If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then
                                Throw New Exception("Request Block must not Contain Inner Request Block")
                            Else
                                Dim controlValueSplitted As String() = _
                                    cI.ClearedValue.Split(":"c)

                                Dim BlockValue As String = String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1)

                                ' Check This Control has a Content
                                Dim idxCon As Integer = _
                                    BlockValue.IndexOf(":"c)

                                ' Get ControlID Accourding to idxCon Value -1 = no content, else has content
                                If idxCon = -1 Then
                                    ' No Content

                                    Throw New Exception("Statement Grammer Error!")
                                Else
                                    cI.CommonControlID = BlockValue.Substring(0, idxCon)
                                End If

                                Dim _sys_CommonControlID As String = cI.CommonControlID
                                Dim idxTuner As Integer = cI.CommonControlID.IndexOf("~"c)
                                If idxTuner > -1 Then _
                                    cI.CommonControlID = cI.CommonControlID.Substring(0, idxTuner)

                                Dim CoreContent As String = Nothing
                                Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

                                idxCoreContStart = BlockValue.IndexOf(String.Format("{0}:{{", _sys_CommonControlID)) + String.Format("{0}:{{", _sys_CommonControlID).Length
RESEARCH_H_END:
                                idxCoreContEnd = BlockValue.IndexOf(String.Format("}}:{0}", _sys_CommonControlID), idxCoreContEnd + 1)
                                Dim TestEndContent As String = BlockValue.Substring(idxCoreContEnd + String.Format("}}:{0}", _sys_CommonControlID).Length)
                                If TestEndContent.Length > 0 AndAlso _
                                    Not Char.IsWhiteSpace(TestEndContent, 0) Then

                                    GoTo RESEARCH_H_END
                                End If

                                If idxCoreContStart <> -1 AndAlso _
                                    idxCoreContEnd <> -1 Then

                                    CoreContent = BlockValue.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                                    If Not CoreContent Is Nothing AndAlso _
                                        CoreContent.Trim().Length > 0 Then

                                        If RRI.RequestingBlockRendering Then
                                            If Not String.Compare(RRI.BlockID, cI.CommonControlID) = 0 Then GoTo BLOCKSTATEMENT_FINISH
                                        Else
                                            RRI.BlockID = cI.CommonControlID
                                        End If
                                        RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering

                                        Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                            New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, CoreContent)

                                        CoreContent = Me.RenderContent(RRI, tCI)

                                        If Not CoreContent Is Nothing AndAlso _
                                            CoreContent.Trim().Length > 0 Then

                                            If RRI.RequestingBlockRendering Then
                                                cI.HelperSpace.Space = CoreContent
                                            Else
                                                cI.HelperSpace.Space = String.Format("<div id=""{0}"">{1}</div>", cI.CommonControlID, CoreContent)
                                            End If

                                            If RRI.RequestingBlockRendering Then
                                                RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendered
                                            Else
                                                RRI.BlockID = Nothing
                                                RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Undefined
                                            End If
                                        Else
                                            Throw New Exception("Empty Statement Block Error!")
                                        End If
                                    Else
                                        Throw New Exception("Empty Statement Block Error!")
                                    End If
                                Else
                                    Throw New Exception("Statement Grammer Error!")
                                End If
BLOCKSTATEMENT_FINISH:
                            End If

                        Case "X"c ' [POINTER] Encode Direct Call Function
                            Dim matchXF As System.Text.RegularExpressions.Match = _
                                System.Text.RegularExpressions.Regex.Match(cI.ClearedValue, "XF~\d+\:\{")

                            If matchXF.Success Then
                                If String.Compare(matchXF.Value.Split("~"c)(0), "XF") = 0 Then ' Encode Direct Call Function
                                    cI.CommonControlType = Globals.RenderRequestInfo.CommonControlContent.CommonControlTypes.EncodedCallFunction

                                    Dim controlValueSplitted As String() = _
                                        cI.ClearedValue.Split(":"c)
                                    Dim ContentInfo As String = _
                                        String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 2)

                                    If Not ContentInfo Is Nothing AndAlso _
                                        ContentInfo.Trim().Length >= 2 Then

                                        ContentInfo = ContentInfo.Substring(1, ContentInfo.Length - 2)

                                        If Not ContentInfo Is Nothing AndAlso _
                                            ContentInfo.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentInfo.Trim())
                                            ContentInfo = Me.RenderContent(RRI, tCI)

                                            If Not ContentInfo Is Nothing AndAlso _
                                                ContentInfo.Trim().Length > 0 Then

                                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then
                                                    cI.HelperSpace.Space = String.Format("javascript:__swcProcs.doRequest('{0}', '{1}');", RRI.BlockID, [Assembly].EncodeCallFunction(General.HashCode, ContentInfo.Trim()))
                                                Else
                                                    cI.HelperSpace.Space = String.Format("javascript:__swcProcs.postForm('{0}');", [Assembly].EncodeCallFunction(General.HashCode, ContentInfo.Trim()))
                                                End If
                                            Else
                                                Throw New Exception("Empty Statement Block Error!")
                                            End If
                                        Else
                                            Throw New Exception("Empty Statement Block Error!")
                                        End If
                                    Else
                                        Throw New Exception("Empty Statement Block Error!")
                                    End If
                                Else ' Standart Value
                                    If String.Compare(matchXF.Value.Split("~"c)(0), "XF", True) = 0 Then
                                        Throw New Exception("Property Pointer must be Capital!")
                                    Else
                                        cI.HelperSpace.Space = cI.ClearedValue
                                    End If

                                    cI.HelperSpace.Space = cI.ClearedValue
                                End If
                            End If

                        Case "M"c ' [POINTER] MessageInformation << First letter of MessageInformation
                            Dim matchMI As System.Text.RegularExpressions.Match = _
                                System.Text.RegularExpressions.Regex.Match(cI.ClearedValue, "MessageInformation~\d+\:\{")

                            If matchMI.Success Then
                                If String.Compare(matchMI.Value.Split("~"c)(0), "MessageInformation") = 0 Then
                                    Dim MessageResult As PGlobals.MapControls.MessageResult = _
                                            CType(RRI.GlobalArguments.Item("_sys_MessageResult").Value, PGlobals.MapControls.MessageResult)

                                    If MessageResult Is Nothing Then
                                        cI.HelperSpace.Space = Nothing
                                    Else
                                        Dim controlValueSplitted As String() = _
                                            cI.ClearedValue.Split(":"c)

                                        Dim ContentInfo As String = _
                                            String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 2)

                                        If Not ContentInfo Is Nothing AndAlso _
                                            ContentInfo.Trim().Length >= 2 Then

                                            ContentInfo = ContentInfo.Substring(1, ContentInfo.Length - 2)

                                            If Not ContentInfo Is Nothing AndAlso _
                                                ContentInfo.Trim().Length > 0 Then

                                                Dim dataArgs As New Globals.ArgumentInfo.ArgumentInfoCollection

                                                dataArgs.Add("MessageType", MessageResult.Type)
                                                dataArgs.Add("Message", MessageResult.Message)

                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentInfo, dataArgs)

                                                cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                            End If
                                        End If
                                    End If
                                Else ' Standart Value
                                    If String.Compare(matchMI.Value.Split("~"c)(0), "MessageInformation", True) = 0 Then
                                        Throw New Exception(matchMI.Value.Split("~"c)(0) & " must write down as 'MessageInformation'!")
                                    Else
                                        cI.HelperSpace.Space = cI.ClearedValue
                                    End If
                                End If
                            End If

                        Case Else
                            If cI.ClearedValue.Split(":"c)(0).Length = 1 Then
                                Select Case cI.ClearedValue.Split(":"c)(0)
                                    Case "t", "l", "c", "p", "f", "s", "h"
                                        Throw New Exception("Property Pointer must be Capital!")
                                    Case Else
                                        Throw New Exception("Undefined Property!")
                                End Select
                            Else ' Standart Value
                                Dim matchXF As System.Text.RegularExpressions.Match = _
                                    System.Text.RegularExpressions.Regex.Match(cI.ClearedValue, "XF~\d+\:\{")
                                Dim matchMI As System.Text.RegularExpressions.Match = _
                                    System.Text.RegularExpressions.Regex.Match(cI.ClearedValue, "MessageInformation~\d+\:\{")

                                If matchXF.Success AndAlso _
                                    String.Compare(matchXF.Value.Split("~"c)(0), "XF", True) = 0 Then

                                    Throw New Exception("Property Pointer must be Capital!")
                                ElseIf matchMI.Success AndAlso _
                                    String.Compare(matchMI.Value.Split("~"c)(0), "MessageInformation", True) = 0 Then

                                    Throw New Exception(matchMI.Value.Split("~"c)(0) & " must write down as 'MessageInformation'!")
                                Else
                                    cI.HelperSpace.Space = cI.ClearedValue
                                End If
                            End If
                    End Select

                    If Not String.IsNullOrEmpty(cI.CommonControlID) AndAlso _
                        RRI.ParentPendingCommonControlContents.ContainsKey(cI.CommonControlID) Then

                        Dim CCCList As Generic.List(Of Globals.RenderRequestInfo.CommonControlContent) = _
                            RRI.ParentPendingCommonControlContents.Item(cI.CommonControlID)

                        For Each ppCI As Globals.RenderRequestInfo.ContentInfo In CCCList
                            Me.RenderContentInfo(RRI, ppCI, True)
                        Next

                        RRI.ParentPendingCommonControlContents.Remove(cI.CommonControlID)
                    End If
                Catch ex As Exception
                    Throw Me.PrepareException(ex.Message, String.Format("${0}$", cI.ClearedValue), ex.InnerException)
                End Try

                cI.HelperSpace.DefineAsRendered()
            End Sub

            Private Function CheckSecurityInfo(ByVal Control As Globals.Controls.IControlBase, ByVal argumentInfos As Globals.ArgumentInfo.ArgumentInfoCollection) As PGlobals.MapControls.SecurityControlResult.Results
                Dim rSCR As PGlobals.MapControls.SecurityControlResult.Results = _
                    PGlobals.MapControls.SecurityControlResult.Results.ReadWrite

                If Not Control.SecurityInfo.SecuritySet Then _
                    Return rSCR

                If String.Compare(Control.SecurityInfo.CallFunction, "#GLOBAL") = 0 Then _
                    Throw New Exception("Security Information Call Function must be set!")

                Control.SecurityInfo.CallFunction = _
                    Control.SecurityInfo.CallFunction.Replace("[ControlID]", "#ControlID")
                argumentInfos.Add("ControlID", Control.ControlID)

                ' Call Related Function and Exam It
                Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                If Me._ParentInstance Is Nothing Then
PARENTCALL:
                    tAssembleResultInfo = _
                        [Assembly].AssemblePostBackInformation( _
                            Control.SecurityInfo.CallFunction, argumentInfos)
                Else
                    tAssembleResultInfo = _
                        [Assembly].AssemblePostBackInformation( _
                            Me._ParentInstance.CurrentID, _
                            Me._CurrentInstance.CurrentID, _
                            Control.SecurityInfo.CallFunction, argumentInfos)

                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                        TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                        GoTo PARENTCALL
                    End If
                End If

                If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                    TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                    Throw New Exception("Security Information Call Function Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
                Else
                    Dim sCR As PGlobals.MapControls.SecurityControlResult = _
                        CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.SecurityControlResult)

                    rSCR = sCR.SecurityResult

                    If rSCR = PGlobals.MapControls.SecurityControlResult.Results.None Then _
                        rSCR = PGlobals.MapControls.SecurityControlResult.Results.ReadWrite
                End If
                ' ----

                Return rSCR
            End Function

            Private Sub RenderControlContent(ByVal RRI As Globals.RenderRequestInfo, ByRef cI As Globals.RenderRequestInfo.CommonControlContent)
                ' Clear Control Modifier
                Dim controlValueSplitted As String() = _
                    cI.ClearedValue.Split(":"c)
                Dim ControlContent As String = _
                    String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 1)
                ' !---

                ' Check This Control has a Content
                Dim idxCon As Integer = _
                    ControlContent.IndexOf(":"c)

                ' Get ControlID Accourding to idxCon Value -1 = no content, else has content
                If idxCon = -1 Then
                    ' No Content

                    cI.CommonControlID = ControlContent
                Else
                    cI.CommonControlID = ControlContent.Substring(0, idxCon)
                End If

                Dim _sys_CommonControlID As String = cI.CommonControlID
                If Not String.IsNullOrEmpty(cI.CommonControlID) AndAlso _
                    cI.CommonControlID.IndexOf("~"c) > -1 Then cI.CommonControlID = cI.CommonControlID.Substring(0, cI.CommonControlID.IndexOf("~"c))

                Dim CompareCulture As New Globalization.CultureInfo("en-US")
                Dim objControl As Globals.Controls.IControlBase = Me.GetControlMap(cI.CommonControlID)

                Dim ControlSecurityInfoResult As PGlobals.MapControls.SecurityControlResult.Results = _
                    Me.CheckSecurityInfo(objControl, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, cI.Parent.ContentArguments))
                Dim ControlSIM As Hashtable = _
                    CType(RRI.GlobalArguments.Item("_sys_ControlSIM").Value, Hashtable)

                If objControl.SecurityInfo.Disabled.IsSet Then
                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, objControl.SecurityInfo.Disabled.Value, cI.Parent.ContentArguments)

                    objControl.SecurityInfo.Disabled.Value = Me.RenderContent(RRI, tCI)
                End If

                Try
                    Select Case objControl.Type
                        Case Globals.Controls.ControlTypes.DataList
                            If Not RRI.RequestingBlockRendering Then
GENERATEDATALISTRESULT:
                                Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

                                idxCoreContStart = ControlContent.IndexOf(String.Format("{0}:{{", _sys_CommonControlID)) + String.Format("{0}:{{", _sys_CommonControlID).Length

RESEARCH_DataList_END:
                                idxCoreContEnd = ControlContent.IndexOf(String.Format("}}:{0}", _sys_CommonControlID), idxCoreContEnd + 1)
                                Dim TestEndContent As String = ControlContent.Substring(idxCoreContEnd + String.Format("}}:{0}", _sys_CommonControlID).Length)
                                If TestEndContent.Length > 0 AndAlso _
                                    Not Char.IsWhiteSpace(TestEndContent, 0) Then

                                    GoTo RESEARCH_DataList_END
                                End If

                                If idxCoreContStart <> -1 AndAlso _
                                    idxCoreContEnd <> -1 Then

                                    Dim CoreContent As String
                                    CoreContent = ControlContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                                    Dim ContentCollection As String() = Me.SplitContentByControlID(_sys_CommonControlID, CoreContent)
                                    If ContentCollection.Length > 0 Then
                                        ' Catch Error Template Block (Is Exists)
                                        Dim ReContentCollection As New Generic.List(Of String)
                                        Dim MessageTemplate As String = String.Empty, TemplatePointerText As String = "!MESSAGETEMPLATE"

                                        For cC As Integer = 0 To ContentCollection.Length - 1
                                            If ContentCollection(cC).IndexOf(TemplatePointerText) = 0 Then
                                                If String.IsNullOrEmpty(MessageTemplate) Then
                                                    MessageTemplate = ContentCollection(cC).Substring(TemplatePointerText.Length)
                                                Else
                                                    Throw New Exception("Only One Message Template Block Allowed for a DataList Control!")
                                                End If
                                            Else
                                                ReContentCollection.Add(ContentCollection(cC))
                                            End If
                                        Next
                                        ContentCollection = ReContentCollection.ToArray()
                                        ' ---- 

                                        ' Reset Variables
                                        SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, Nothing)

                                        ' Call Related Function and Exam It
                                        Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                                        Dim ControlContentArguments As SolidDevelopment.Web.Globals.ArgumentInfo.ArgumentInfoCollection = _
                                            cI.Parent.ContentArguments
                                        Dim Leveling As Integer = cI.Leveling

                                        Do
                                            If Leveling > 0 Then _
                                                ControlContentArguments = ControlContentArguments.Parent : Leveling -= 1
                                        Loop Until ControlContentArguments Is Nothing OrElse Leveling = 0

                                        If Me._ParentInstance Is Nothing Then
PARENTCALL_DataList:
                                            tAssembleResultInfo = _
                                                [Assembly].AssemblePostBackInformation( _
                                                                objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))
                                        Else
                                            tAssembleResultInfo = _
                                                [Assembly].AssemblePostBackInformation( _
                                                                Me._ParentInstance.CurrentID, _
                                                                Me._CurrentInstance.CurrentID, _
                                                                objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))

                                            If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                                TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                                                GoTo PARENTCALL_DataList
                                            End If
                                        End If

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                                            Throw New Exception("Datalist Call Function Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
                                        Else
                                            If tAssembleResultInfo.MethodResult Is Nothing OrElse _
                                                ( _
                                                    Not TypeOf tAssembleResultInfo.MethodResult Is PGlobals.MapControls.PartialDataTable AndAlso _
                                                    Not TypeOf tAssembleResultInfo.MethodResult Is PGlobals.MapControls.DirectDataReader _
                                                ) Then

                                                SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(0, 0))
                                            Else
                                                If TypeOf tAssembleResultInfo.MethodResult Is PGlobals.MapControls.PartialDataTable Then
                                                    Dim RepeaterList As PGlobals.MapControls.PartialDataTable = _
                                                        CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.PartialDataTable)

                                                    Dim dataArgs As New Globals.ArgumentInfo.ArgumentInfoCollection

                                                    If Not RepeaterList.MessageResult Is Nothing Then
                                                        SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(0, 0))

                                                        If String.IsNullOrEmpty(MessageTemplate) Then
                                                            cI.HelperSpace.Space = RepeaterList.MessageResult.Message
                                                        Else
                                                            dataArgs.Add("MessageType", RepeaterList.MessageResult.Type)
                                                            dataArgs.Add("Message", RepeaterList.MessageResult.Message)

                                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, MessageTemplate, dataArgs)

                                                            cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                                        End If
                                                    Else
                                                        ' Set Variables
                                                        SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(RepeaterList.ThisContainer.Rows.Count, RepeaterList.TotalCount))

                                                        Dim RenderedContent As New System.Text.StringBuilder
                                                        Dim ContentIndex As Integer = 0, rC As Integer = 0
                                                        Dim IsItemIndexColumnExists As Boolean = False

                                                        For Each dR As DataRow In RepeaterList.ThisContainer.Rows
                                                            dataArgs.Add("_sys_ItemIndex", rC)
                                                            For Each dC As DataColumn In RepeaterList.ThisContainer.Columns
                                                                If CompareCulture.CompareInfo.Compare(dC.ColumnName, "ItemIndex", Globalization.CompareOptions.IgnoreCase) = 0 Then IsItemIndexColumnExists = True

                                                                dataArgs.Add(dC.ColumnName, dR.Item(dC.ColumnName))
                                                            Next
                                                            ' this is for user interaction
                                                            If Not IsItemIndexColumnExists Then dataArgs.Add("ItemIndex", rC)

                                                            ContentIndex = rC Mod ContentCollection.Length

                                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentCollection(ContentIndex), dataArgs)

                                                            RenderedContent.Append( _
                                                                Me.RenderContent(RRI, tCI) _
                                                            )

                                                            rC += 1
                                                        Next
                                                        cI.HelperSpace.Space = RenderedContent.ToString()
                                                    End If
                                                ElseIf TypeOf tAssembleResultInfo.MethodResult Is PGlobals.MapControls.DirectDataReader Then
                                                    Dim DataReaderInfo As PGlobals.MapControls.DirectDataReader = _
                                                        CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.DirectDataReader)

                                                    Dim dataArgs As New Globals.ArgumentInfo.ArgumentInfoCollection

                                                    If DataReaderInfo.DatabaseCommand Is Nothing Then
                                                        If Not DataReaderInfo.MessageResult Is Nothing Then
                                                            SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(0, 0))

                                                            If String.IsNullOrEmpty(MessageTemplate) Then
                                                                cI.HelperSpace.Space = DataReaderInfo.MessageResult.Message
                                                            Else
                                                                dataArgs.Add("MessageType", DataReaderInfo.MessageResult.Type)
                                                                dataArgs.Add("Message", DataReaderInfo.MessageResult.Message)

                                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, MessageTemplate, dataArgs)

                                                                cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                                            End If

                                                            SolidDevelopment.Web.Helpers.EventLogging.WriteToLog("DirectDataReader Error! DatabaseCommand can not be null!")
                                                        Else
                                                            Throw New Exception("DirectDataReader Error! DatabaseCommand can not be null!")
                                                        End If
                                                    Else
                                                        Dim DBConnection As IDbConnection = _
                                                            DataReaderInfo.DatabaseCommand.Connection
                                                        Dim DBCommand As IDbCommand = _
                                                            DataReaderInfo.DatabaseCommand
                                                        Dim DBReader As IDataReader

                                                        Try
                                                            DBConnection.Open()
                                                            DBReader = DBCommand.ExecuteReader()

                                                            ' Set Variables
                                                            Dim RenderedContent As New System.Text.StringBuilder
                                                            Dim ContentIndex As Integer = 0, rC As Integer = 0
                                                            Dim IsItemIndexColumnExists As Boolean = False

                                                            If DBReader.Read() Then
                                                                Do
                                                                    dataArgs.Add("_sys_ItemIndex", rC)
                                                                    For cC As Integer = 0 To DBReader.FieldCount - 1
                                                                        If CompareCulture.CompareInfo.Compare(DBReader.GetName(cC), "ItemIndex", Globalization.CompareOptions.IgnoreCase) = 0 Then IsItemIndexColumnExists = True

                                                                        dataArgs.Add(DBReader.GetName(cC), DBReader.GetValue(cC))
                                                                    Next
                                                                    ' this is for user interaction
                                                                    If Not IsItemIndexColumnExists Then dataArgs.Add("ItemIndex", rC)

                                                                    ContentIndex = rC Mod ContentCollection.Length

                                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentCollection(ContentIndex), dataArgs)

                                                                    RenderedContent.Append( _
                                                                        Me.RenderContent(RRI, tCI) _
                                                                    )

                                                                    rC += 1
                                                                Loop While DBReader.Read()
                                                                SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(rC, rC))
                                                                cI.HelperSpace.Space = RenderedContent.ToString()
                                                            Else
                                                                SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(0, 0))

                                                                If Not DataReaderInfo.MessageResult Is Nothing Then
                                                                    If String.IsNullOrEmpty(MessageTemplate) Then
                                                                        cI.HelperSpace.Space = DataReaderInfo.MessageResult.Message
                                                                    Else
                                                                        dataArgs.Add("MessageType", DataReaderInfo.MessageResult.Type)
                                                                        dataArgs.Add("Message", DataReaderInfo.MessageResult.Message)

                                                                        Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                            New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, MessageTemplate, dataArgs)

                                                                        cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                                                    End If
                                                                Else
                                                                    cI.HelperSpace.Space = String.Empty
                                                                End If
                                                            End If

                                                            ' Close and Dispose Database Reader
                                                            DBReader.Close()
                                                            DBReader.Dispose()
                                                            GC.SuppressFinalize(DBReader)
                                                            ' ----

                                                            ' Close and Dispose Database Command
                                                            DBCommand.Dispose()
                                                            GC.SuppressFinalize(DBCommand)
                                                            ' ----

                                                            ' Close and Dispose Database Connection
                                                            DBConnection.Close()
                                                            DBConnection.Dispose()
                                                            GC.SuppressFinalize(DBConnection)
                                                            ' ----
                                                        Catch ex As Exception
                                                            If Not DataReaderInfo.MessageResult Is Nothing Then
                                                                SolidDevelopment.Web.General.SetVariable(cI.CommonControlID, New Globals.DataListOutputInfo(0, 0))

                                                                If String.IsNullOrEmpty(MessageTemplate) Then
                                                                    cI.HelperSpace.Space = DataReaderInfo.MessageResult.Message
                                                                Else
                                                                    dataArgs.Add("MessageType", DataReaderInfo.MessageResult.Type)
                                                                    dataArgs.Add("Message", DataReaderInfo.MessageResult.Message)

                                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, MessageTemplate, dataArgs)

                                                                    cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                                                End If

                                                                SolidDevelopment.Web.Helpers.EventLogging.WriteToLog(ex)
                                                            Else
                                                                Throw New Exception("DirectDataReader Error!", ex)
                                                            End If
                                                        End Try
                                                    End If
                                                End If
                                            End If
                                        End If
                                        ' ----
                                    Else
                                        Throw New Exception("Parsing Error!")
                                    End If
                                Else
                                    Throw New Exception("Parsing Error!")
                                End If
                            Else
                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then GoTo GENERATEDATALISTRESULT
                            End If

                        Case Globals.Controls.ControlTypes.ConditionalStatement
                            Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

                            idxCoreContStart = ControlContent.IndexOf(String.Format("{0}:{{", _sys_CommonControlID)) + String.Format("{0}:{{", _sys_CommonControlID).Length

RESEARCH_ConditionalStatement_END:
                            idxCoreContEnd = ControlContent.IndexOf(String.Format("}}:{0}", _sys_CommonControlID), idxCoreContEnd + 1)
                            Dim TestEndContent As String = ControlContent.Substring(idxCoreContEnd + String.Format("}}:{0}", _sys_CommonControlID).Length)
                            If TestEndContent.Length > 0 AndAlso _
                                Not Char.IsWhiteSpace(TestEndContent, 0) Then

                                GoTo RESEARCH_ConditionalStatement_END
                            End If

                            If idxCoreContStart <> -1 AndAlso _
                                idxCoreContEnd <> -1 Then

                                Dim ConditionResult As PGlobals.MapControls.ConditionalStatementResult = Nothing

                                Dim CoreContent As String
                                Dim ContentTrue As String = Nothing, ContentFalse As String = Nothing

                                CoreContent = ControlContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                                Dim ContentCollection As String() = Me.SplitContentByControlID(_sys_CommonControlID, CoreContent)

                                If ContentCollection.Length > 0 Then
                                    For mC As Integer = 0 To ContentCollection.Length - 1
                                        Select Case mC
                                            Case 0
                                                ContentTrue = ContentCollection(mC)
                                            Case 1
                                                ContentFalse = ContentCollection(mC)
                                        End Select
                                    Next

                                    ' Call Related Function and Exam It
                                    Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                                    Dim ControlContentArguments As SolidDevelopment.Web.Globals.ArgumentInfo.ArgumentInfoCollection = _
                                        cI.Parent.ContentArguments
                                    Dim Leveling As Integer = cI.Leveling

                                    Do
                                        If Leveling > 0 Then _
                                            ControlContentArguments = ControlContentArguments.Parent : Leveling -= 1
                                    Loop Until ControlContentArguments Is Nothing OrElse Leveling = 0

                                    If Me._ParentInstance Is Nothing Then
PARENTCALL_ConditionalStatement:
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))
                                    Else
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            Me._ParentInstance.CurrentID, _
                                                            Me._CurrentInstance.CurrentID, _
                                                            objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                                            GoTo PARENTCALL_ConditionalStatement
                                        End If
                                    End If

                                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                        TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                                        Throw New Exception("Conditional Statement Call Function Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
                                    Else
                                        ConditionResult = CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.ConditionalStatementResult)
                                    End If
                                    ' ----

                                    ' if ConditionResult is not nothing, Render Results
                                    If Not ConditionResult Is Nothing Then
                                        Dim RenderedContent As String = String.Empty

                                        Select Case ConditionResult.ConditionResult
                                            Case PGlobals.MapControls.ConditionalStatementResult.Conditions.True
                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentTrue, cI.Parent.ContentArguments)

                                                RenderedContent = Me.RenderContent(RRI, tCI)
                                            Case PGlobals.MapControls.ConditionalStatementResult.Conditions.False
                                                If Not ContentFalse Is Nothing Then
                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, ContentFalse, cI.Parent.ContentArguments)

                                                    RenderedContent = Me.RenderContent(RRI, tCI)
                                                End If
                                            Case PGlobals.MapControls.ConditionalStatementResult.Conditions.UnKnown
                                                ' Reserved For Future Uses
                                        End Select

                                        cI.HelperSpace.Space = RenderedContent
                                    End If
                                    ' ----
                                Else
                                    Throw New Exception("Parsing Error!")
                                End If
                            Else
                                Throw New Exception("Parsing Error!")
                            End If

                        Case Globals.Controls.ControlTypes.VariableBlock
                            Dim CoreContent As String = Nothing
                            Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

                            idxCoreContStart = ControlContent.IndexOf(String.Format("{0}:{{", _sys_CommonControlID)) + String.Format("{0}:{{", _sys_CommonControlID).Length

RESEARCH_VariableBlock_END:
                            idxCoreContEnd = ControlContent.IndexOf(String.Format("}}:{0}", _sys_CommonControlID), idxCoreContEnd + 1)
                            Dim TestEndContent As String = ControlContent.Substring(idxCoreContEnd + String.Format("}}:{0}", _sys_CommonControlID).Length)
                            If TestEndContent.Length > 0 AndAlso _
                                Not Char.IsWhiteSpace(TestEndContent, 0) Then

                                GoTo RESEARCH_VariableBlock_END
                            End If

                            If idxCoreContStart <> -1 AndAlso _
                                idxCoreContEnd <> -1 Then

                                CoreContent = ControlContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)

                                If Not CoreContent Is Nothing AndAlso _
                                    CoreContent.Trim().Length > 0 Then

                                    Dim VariableBlockResult As PGlobals.MapControls.VariableBlockResult = Nothing

                                    ' Call Related Function and Exam It
                                    Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                                    Dim ControlContentArguments As SolidDevelopment.Web.Globals.ArgumentInfo.ArgumentInfoCollection = _
                                        cI.Parent.ContentArguments
                                    Dim Leveling As Integer = cI.Leveling

                                    Do
                                        If Leveling > 0 Then _
                                            ControlContentArguments = ControlContentArguments.Parent : Leveling -= 1
                                    Loop Until ControlContentArguments Is Nothing OrElse Leveling = 0

                                    If Me._ParentInstance Is Nothing Then
PARENTCALL_VariableBlock:
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))
                                    Else
                                        tAssembleResultInfo = _
                                            [Assembly].AssemblePostBackInformation( _
                                                            Me._ParentInstance.CurrentID, _
                                                            Me._CurrentInstance.CurrentID, _
                                                            objControl.CallFunction, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, ControlContentArguments))

                                        If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                            TypeOf tAssembleResultInfo.MethodResult Is IO.FileNotFoundException Then

                                            GoTo PARENTCALL_VariableBlock
                                        End If
                                    End If

                                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso _
                                        TypeOf tAssembleResultInfo.MethodResult Is Exception Then

                                        Throw New Exception("Variable Block Statement Call Function Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
                                    Else
                                        VariableBlockResult = CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.VariableBlockResult)
                                    End If
                                    ' ----

                                    ' if VariableBlockResult is not nothing, Set Variable Values
                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo

                                    If Not VariableBlockResult Is Nothing Then
                                        Dim dataArgs As New Globals.ArgumentInfo.ArgumentInfoCollection

                                        For Each Key As String In VariableBlockResult.Keys
                                            dataArgs.Add(Key, VariableBlockResult.Item(Key))
                                        Next

                                        tCI = New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, CoreContent, dataArgs)
                                    Else
                                        tCI = New Globals.RenderRequestInfo.RenderlessContent(cI.Parent, 0, CoreContent)
                                    End If

                                    cI.HelperSpace.Space = Me.RenderContent(RRI, tCI)
                                    ' ----
                                Else
                                    Throw New Exception("Empty Statement Block Error!")
                                End If
                            Else
                                Throw New Exception("Parsing Error!")
                            End If

                        Case Globals.Controls.ControlTypes.Unknown
                            Throw New Exception("UnKnown Control Type!")

                        Case Else
                            If Not RRI.RequestingBlockRendering Then
RENDERCONTROL:
                                If RRI.GlobalArguments.ContainsKey(cI.CommonControlID) Then _
                                    objControl.Attributes.Item("value") = CType(RRI.GlobalArguments.Item(cI.CommonControlID).Value, String)

                                ' Some Unique Custom Variables
                                Dim TextBoxValue As String = Nothing
                                Dim PasswordValue As String = Nothing
                                Dim ButtonValue As String = Nothing
                                Dim TextareaValue As String = Nothing
                                Dim AnchorLinkValue As String = Nothing, AnchorLinkHref As String = Nothing
                                Dim ImageSource As String = Nothing
                                Dim CheckBoxLabel As String = Nothing, CheckBoxID As String = cI.CommonControlID
                                Dim RadioButtonLabel As String = Nothing, RadioButtonID As String = cI.CommonControlID
                                ' ----

                                Select Case objControl.Type
                                    Case Globals.Controls.ControlTypes.Textbox
                                        objControl.Attributes.Remove("value")

                                        TextBoxValue = CType(objControl, Globals.Controls.Textbox).Text

                                        If Not TextBoxValue Is Nothing AndAlso _
                                            TextBoxValue.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, TextBoxValue, cI.Parent.ContentArguments)

                                            TextBoxValue = Me.RenderContent(RRI, tCI)
                                        End If

                                        If Not String.IsNullOrEmpty(CType(objControl, Globals.Controls.Textbox).DefaultButtonID) Then
                                            Dim buttonControl As Globals.Controls.IControlBase = _
                                                Me.GetControlMap(CType(objControl, Globals.Controls.Textbox).DefaultButtonID)

                                            Dim buttonControlSCR As PGlobals.MapControls.SecurityControlResult.Results = _
                                                Me.CheckSecurityInfo(buttonControl, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, cI.Parent.ContentArguments))

                                            If buttonControlSCR <> PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                                Select Case buttonControl.Type
                                                    Case Globals.Controls.ControlTypes.Button, Globals.Controls.ControlTypes.Image, Globals.Controls.ControlTypes.Link
                                                        If Not String.IsNullOrEmpty(buttonControl.Attributes.Item("onclick")) Then
                                                            objControl.Attributes.Item("DefaultButtonOnClick") = buttonControl.Attributes.Item("onclick").Replace("javascript:", Nothing)
                                                        End If

                                                        objControl.CallFunction = buttonControl.CallFunction
                                                End Select
                                            End If
                                        End If

                                        If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                            objControl.Attributes.Add("readonly", "readonly")

                                            ControlSIM.Item(objControl.ControlID) = TextBoxValue

                                            If objControl.SecurityInfo.Disabled.IsSet Then
                                                TextBoxValue = objControl.SecurityInfo.Disabled.Value

                                                If Not TextBoxValue Is Nothing AndAlso _
                                                    TextBoxValue.Trim().Length > 0 Then

                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, TextBoxValue, cI.Parent.ContentArguments)

                                                    TextBoxValue = Me.RenderContent(RRI, tCI)
                                                End If
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Password
                                        objControl.Attributes.Remove("value")

                                        PasswordValue = CType(objControl, Globals.Controls.Password).Text

                                        If Not PasswordValue Is Nothing AndAlso _
                                            PasswordValue.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, PasswordValue, cI.Parent.ContentArguments)

                                            PasswordValue = Me.RenderContent(RRI, tCI)
                                        End If

                                        If Not String.IsNullOrEmpty(CType(objControl, Globals.Controls.Password).DefaultButtonID) Then
                                            Dim buttonControl As Globals.Controls.IControlBase = _
                                                Me.GetControlMap(CType(objControl, Globals.Controls.Password).DefaultButtonID)

                                            Dim buttonControlSCR As PGlobals.MapControls.SecurityControlResult.Results = _
                                                Me.CheckSecurityInfo(buttonControl, Globals.ArgumentInfo.ArgumentInfoCollection.Combine(RRI.GlobalArguments, cI.Parent.ContentArguments))

                                            If buttonControlSCR <> PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                                Select Case buttonControl.Type
                                                    Case Globals.Controls.ControlTypes.Button, Globals.Controls.ControlTypes.Image, Globals.Controls.ControlTypes.Link
                                                        If Not String.IsNullOrEmpty(buttonControl.Attributes.Item("onclick")) Then
                                                            objControl.Attributes.Item("DefaultButtonOnClick") = buttonControl.Attributes.Item("onclick").Replace("javascript:", Nothing)
                                                        End If

                                                        objControl.CallFunction = buttonControl.CallFunction
                                                End Select
                                            End If
                                        End If

                                        If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                            objControl.Attributes.Add("readonly", "readonly")

                                            ControlSIM.Item(objControl.ControlID) = PasswordValue

                                            If objControl.SecurityInfo.Disabled.IsSet Then
                                                PasswordValue = objControl.SecurityInfo.Disabled.Value

                                                If Not PasswordValue Is Nothing AndAlso _
                                                    PasswordValue.Trim().Length > 0 Then

                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, PasswordValue, cI.Parent.ContentArguments)

                                                    PasswordValue = Me.RenderContent(RRI, tCI)
                                                End If
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Button
                                        objControl.Attributes.Remove("value")

                                        ButtonValue = CType(objControl, Globals.Controls.Button).Text

                                        If Not ButtonValue Is Nothing AndAlso _
                                            ButtonValue.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, ButtonValue, cI.Parent.ContentArguments)

                                            ButtonValue = Me.RenderContent(RRI, tCI)
                                        End If

                                        If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                            objControl.Attributes.Add("readonly", "readonly")

                                            ControlSIM.Item(objControl.ControlID) = ButtonValue

                                            If objControl.SecurityInfo.Disabled.IsSet Then
                                                ButtonValue = objControl.SecurityInfo.Disabled.Value

                                                If Not ButtonValue Is Nothing AndAlso _
                                                    ButtonValue.Trim().Length > 0 Then

                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, ButtonValue, cI.Parent.ContentArguments)

                                                    ButtonValue = Me.RenderContent(RRI, tCI)
                                                End If
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Textarea
                                        objControl.Attributes.Remove("value")

                                        TextareaValue = CType(objControl, Globals.Controls.Textarea).Content

                                        If Not TextareaValue Is Nothing AndAlso _
                                            TextareaValue.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, TextareaValue, cI.Parent.ContentArguments)

                                            TextareaValue = Me.RenderContent(RRI, tCI)
                                        End If

                                        If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly Then
                                            objControl.Attributes.Add("readonly", "readonly")

                                            ControlSIM.Item(objControl.ControlID) = TextareaValue

                                            If objControl.SecurityInfo.Disabled.IsSet Then
                                                TextareaValue = objControl.SecurityInfo.Disabled.Value

                                                If Not TextareaValue Is Nothing AndAlso _
                                                    TextareaValue.Trim().Length > 0 Then

                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, TextareaValue, cI.Parent.ContentArguments)

                                                    TextareaValue = Me.RenderContent(RRI, tCI)
                                                End If
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Link
                                        ' href attribute is disabled always, use url attribute instand of...
                                        objControl.Attributes.Remove("href")
                                        objControl.Attributes.Remove("value")

                                        AnchorLinkValue = CType(objControl, Globals.Controls.Link).Text

                                        If Not AnchorLinkValue Is Nothing AndAlso _
                                            AnchorLinkValue.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, AnchorLinkValue, cI.Parent.ContentArguments)

                                            AnchorLinkValue = Me.RenderContent(RRI, tCI)
                                        End If

                                        AnchorLinkHref = CType(objControl, Globals.Controls.Link).Url

                                        If Not AnchorLinkHref Is Nothing AndAlso _
                                            AnchorLinkHref.Trim().Length > 0 Then

                                            If AnchorLinkHref.IndexOf("~/") = 0 Then
                                                AnchorLinkHref = AnchorLinkHref.Remove(0, 2)
                                                AnchorLinkHref = AnchorLinkHref.Insert(0, Configurations.ApplicationRoot.BrowserSystemImplementation)
                                            ElseIf AnchorLinkHref.IndexOf("¨/") = 0 Then
                                                AnchorLinkHref = AnchorLinkHref.Remove(0, 2)
                                                AnchorLinkHref = AnchorLinkHref.Insert(0, Configurations.VirtualRoot)
                                            End If

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, AnchorLinkHref, cI.Parent.ContentArguments)

                                            AnchorLinkHref = Me.RenderContent(RRI, tCI)
                                        End If

                                        If Not String.IsNullOrEmpty(objControl.CallFunction) Then
                                            AnchorLinkHref = "#_action0"

                                            If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly AndAlso _
                                                objControl.SecurityInfo.Disabled.IsSet Then

                                                AnchorLinkValue = objControl.SecurityInfo.Disabled.Value

                                                If Not AnchorLinkValue Is Nothing AndAlso _
                                                    AnchorLinkValue.Trim().Length > 0 Then

                                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, AnchorLinkValue, cI.Parent.ContentArguments)

                                                    AnchorLinkValue = Me.RenderContent(RRI, tCI)
                                                End If
                                            End If
                                        Else
                                            If AnchorLinkHref Is Nothing OrElse _
                                                (Not AnchorLinkHref Is Nothing AndAlso AnchorLinkHref.Trim().Length = 0) Then

                                                AnchorLinkHref = "#_action1"
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Image
                                        ' href attribute is disabled always, use url attribute instand of...
                                        objControl.Attributes.Remove("src")

                                        ImageSource = CType(objControl, Globals.Controls.Image).Source

                                        If Not ImageSource Is Nothing AndAlso _
                                            ImageSource.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, ImageSource, cI.Parent.ContentArguments)

                                            ImageSource = Me.RenderContent(RRI, tCI)
                                        End If

                                        If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadOnly AndAlso _
                                            objControl.SecurityInfo.Disabled.IsSet Then

                                            ImageSource = objControl.SecurityInfo.Disabled.Value

                                            If Not ImageSource Is Nothing AndAlso _
                                                ImageSource.Trim().Length > 0 Then

                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, ImageSource, cI.Parent.ContentArguments)

                                                ImageSource = Me.RenderContent(RRI, tCI)
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Checkbox
                                        If Not String.IsNullOrEmpty(CType(objControl, Globals.Controls.CheckBox).Text) Then
                                            CheckBoxLabel = CType(objControl, Globals.Controls.CheckBox).Text

                                            If CheckBoxLabel.Trim().Length > 0 Then
                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, CheckBoxLabel, cI.Parent.ContentArguments)

                                                CheckBoxLabel = Me.RenderContent(RRI, tCI)

                                                Dim ItemIndex As String = _
                                                    CType(RRI.GlobalArguments.Item("_sys_ItemIndex").Value, String)

                                                If ItemIndex Is Nothing Then
                                                    CheckBoxLabel = String.Format("<label for=""{0}"">{1}</label>", cI.CommonControlID, CheckBoxLabel)
                                                Else
                                                    CheckBoxLabel = String.Format("<label for=""{0}_{1}"">{2}</label>", cI.CommonControlID, ItemIndex, CheckBoxLabel)

                                                    CheckBoxID = String.Format("{0}_{1}", cI.CommonControlID, ItemIndex)
                                                End If
                                            End If
                                        End If

                                    Case Globals.Controls.ControlTypes.Radio
                                        If Not String.IsNullOrEmpty(CType(objControl, Globals.Controls.RadioButton).Text) Then
                                            RadioButtonLabel = CType(objControl, Globals.Controls.RadioButton).Text

                                            If RadioButtonLabel.Trim().Length > 0 Then
                                                Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                    New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, RadioButtonLabel, cI.Parent.ContentArguments)

                                                RadioButtonLabel = Me.RenderContent(RRI, tCI)

                                                Dim ItemIndex As String = _
                                                    CType(RRI.GlobalArguments.Item("_sys_ItemIndex").Value, String)

                                                If ItemIndex Is Nothing Then
                                                    RadioButtonLabel = String.Format("<label for=""{0}"">{1}</label>", cI.CommonControlID, RadioButtonLabel)
                                                Else
                                                    RadioButtonLabel = String.Format("<label for=""{0}_{1}"">{2}</label>", cI.CommonControlID, ItemIndex, RadioButtonLabel)

                                                    RadioButtonID = String.Format("{0}_{1}", cI.CommonControlID, ItemIndex)
                                                End If
                                            End If
                                        End If

                                End Select

                                If Not String.IsNullOrEmpty(objControl.CallFunction) Then
                                    Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                        New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, objControl.CallFunction, cI.Parent.ContentArguments)

                                    objControl.CallFunction = Me.RenderContent(RRI, tCI)

                                    Select Case objControl.Type
                                        Case Globals.Controls.ControlTypes.Textbox, Globals.Controls.ControlTypes.Password
                                            If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering OrElse _
                                                objControl.BlockIDsToUpdate.Count > 0 Then

                                                If Not objControl.BlockIDsToUpdate.Contains(RRI.BlockID) AndAlso _
                                                    objControl.BlockLocalUpdate Then

                                                    objControl.BlockIDsToUpdate.Add(RRI.BlockID)
                                                End If

                                                If Not String.IsNullOrEmpty(objControl.Attributes.Item("onkeydown")) Then
                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("DefaultButtonOnClick")) Then
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:var eO=false;try{{{2}; {3}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.doRequest('{0}', '{1}')}};", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onkeydown"), objControl.Attributes.Item("DefaultButtonOnClick"))
                                                    Else
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:var eO=false;try{{{2}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.doRequest('{0}', '{1}')}};", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onkeydown"))
                                                    End If
                                                Else
                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("DefaultButtonOnClick")) Then
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:if(event.keyCode==13){{var eO=false;try{{{2}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.doRequest('{0}', '{1}')}};}}", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("DefaultButtonOnClick"))
                                                    Else
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:if(event.keyCode==13){{__swcProcs.doRequest('{0}', '{1}');}}", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction))
                                                    End If
                                                End If
                                            Else
                                                If Not String.IsNullOrEmpty(objControl.Attributes.Item("onkeydown")) Then
                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("DefaultButtonOnClick")) Then
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:var eO=false;try{{{1}; {2}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.postForm('{0}')}};", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onkeydown"), objControl.Attributes.Item("DefaultButtonOnClick"))
                                                    Else
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:var eO=false;try{{{1}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.postForm('{0}')}};", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onkeydown"))
                                                    End If
                                                Else
                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("DefaultButtonOnClick")) Then
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:if(event.keyCode==13){{var eO=false;try{{{1}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.postForm('{0}')}};}}", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("DefaultButtonOnClick"))
                                                    Else
                                                        objControl.Attributes.Item("onkeydown") = String.Format("javascript:if(event.keyCode==13){{__swcProcs.postForm('{0}');}}", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction))
                                                    End If
                                                End If
                                            End If

                                        Case Globals.Controls.ControlTypes.Textarea
                                            ' Do not implement any call function for this control

                                        Case Else
                                            If ControlSecurityInfoResult = PGlobals.MapControls.SecurityControlResult.Results.ReadWrite Then
                                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering OrElse _
                                                    objControl.BlockIDsToUpdate.Count > 0 Then

                                                    If Not objControl.BlockIDsToUpdate.Contains(RRI.BlockID) AndAlso _
                                                        objControl.BlockLocalUpdate Then

                                                        objControl.BlockIDsToUpdate.Add(RRI.BlockID)
                                                    End If

                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("onclick")) Then
                                                        objControl.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{2}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.doRequest('{0}', '{1}')}};", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onclick"))
                                                    Else
                                                        objControl.Attributes.Item("onclick") = String.Format("javascript:__swcProcs.doRequest('{0}', '{1}');", String.Join(",", objControl.BlockIDsToUpdate.ToArray()), [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction))
                                                    End If
                                                Else
                                                    If Not String.IsNullOrEmpty(objControl.Attributes.Item("onclick")) Then
                                                        objControl.Attributes.Item("onclick") = String.Format("javascript:var eO=false;try{{{1}}}catch(ex){{eO=true}};if(!eO){{__swcProcs.postForm('{0}')}};", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction), objControl.Attributes.Item("onclick"))
                                                    Else
                                                        objControl.Attributes.Item("onclick") = String.Format("javascript:__swcProcs.postForm('{0}');", [Assembly].EncodeCallFunction(General.HashCode, objControl.CallFunction))
                                                    End If
                                                End If
                                            End If

                                    End Select
                                End If

                                Dim tAttribs As New System.Text.StringBuilder, tAttribHelper As String
                                For Each aI As Globals.Controls.AttributeInfo In objControl.Attributes.ToArray()
                                    If CompareCulture.CompareInfo.Compare(aI.Id, "id", Globalization.CompareOptions.IgnoreCase) <> 0 Then
                                        tAttribHelper = aI.Value

                                        If Not tAttribHelper Is Nothing AndAlso _
                                            tAttribHelper.Trim().Length > 0 Then

                                            Dim tCI As Globals.RenderRequestInfo.ContentInfo = _
                                                New Globals.RenderRequestInfo.RenderlessContent(cI.Parent.Parent, 0, tAttribHelper, cI.Parent.ContentArguments)

                                            tAttribHelper = Me.RenderContent(RRI, tCI)
                                        End If

                                        If aI.Id Is Nothing OrElse _
                                            aI.Id.Trim().Length = 0 Then

                                            tAttribs.AppendFormat(" {0}", tAttribHelper)
                                        Else
                                            tAttribs.AppendFormat(" {0}=""{1}""", aI.Id, tAttribHelper)
                                        End If
                                    End If
                                Next

                                Select Case objControl.Type
                                    Case Globals.Controls.ControlTypes.Textbox
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<input type=""text"" name=""{0}"" id=""{0}""{1}{2}>", cI.CommonControlID, IIf(Not TextBoxValue Is Nothing, String.Format(" value=""{0}""", TextBoxValue), Nothing), tAttribs.ToString())
                                        End If
                                    Case Globals.Controls.ControlTypes.Password
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<input type=""password"" name=""{0}"" id=""{0}""{1}{2}>", cI.CommonControlID, IIf(Not PasswordValue Is Nothing, String.Format(" value=""{0}""", PasswordValue), Nothing), tAttribs.ToString())
                                        End If
                                    Case Globals.Controls.ControlTypes.Checkbox
                                        cI.HelperSpace.Space = String.Format("<input type=""checkbox"" name=""{0}"" id=""{1}""{2}>{3}", cI.CommonControlID, CheckBoxID, tAttribs.ToString(), CheckBoxLabel)
                                    Case Globals.Controls.ControlTypes.Radio
                                        cI.HelperSpace.Space = String.Format("<input type=""radio"" name=""{0}"" id=""{1}""{2}>{3}", cI.CommonControlID, RadioButtonID, tAttribs.ToString(), RadioButtonLabel)
                                    Case Globals.Controls.ControlTypes.Button
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<input type=""button"" name=""{0}"" id=""{0}""{1}{2}>", cI.CommonControlID, IIf(Not ButtonValue Is Nothing, String.Format(" value=""{0}""", ButtonValue), Nothing), tAttribs.ToString())
                                        End If
                                    Case Globals.Controls.ControlTypes.Textarea
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<textarea name=""{0}"" id=""{0}""{1}>{2}</textarea>", cI.CommonControlID, tAttribs.ToString(), TextareaValue)
                                        End If
                                    Case Globals.Controls.ControlTypes.Image
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<img name=""{0}"" alt="""" id=""{0}"" src=""{2}""{1} />", cI.CommonControlID, tAttribs.ToString(), ImageSource)
                                        End If
                                    Case Globals.Controls.ControlTypes.Link
                                        If objControl.SecurityInfo.Disabled.IsSet AndAlso _
                                            objControl.SecurityInfo.Disabled.Type = Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes.Dynamic Then

                                            cI.HelperSpace.Space = objControl.SecurityInfo.Disabled.Value
                                        Else
                                            cI.HelperSpace.Space = String.Format("<a href=""{3}"" name=""{0}"" id=""{0}""{1}>{2}</a>", cI.CommonControlID, tAttribs.ToString(), AnchorLinkValue, AnchorLinkHref)
                                        End If
                                End Select
                            Else
                                If RRI.BlockRenderingStatus = Globals.RenderRequestInfo.BlockRenderingStatuses.Rendering Then GoTo RENDERCONTROL
                            End If

                    End Select
                Catch ex As Exception
                    If Not ex.Source Is Nothing AndAlso _
                        ex.Source.IndexOf("!"c) = 0 Then

                        Throw Me.PrepareException(ex.Message, ex.Source, ex.InnerException)
                    Else
                        Throw Me.PrepareException(ex.Message, String.Format("$C:{0}$", ControlContent), ex.InnerException)
                    End If
                End Try

                cI.HelperSpace.DefineAsRendered()
            End Sub

            Private _QuickControlMapCache As New Generic.Dictionary(Of String, Globals.Controls.IControlBase)
            Private Function GetControlMap(ByVal ControlID As String) As Globals.Controls.IControlBase
                Dim rControl As Globals.Controls.IControlBase = _
                        New Globals.Controls.ControlBase(Nothing, Globals.Controls.ControlTypes.Unknown)

                Dim ParentSearched As Boolean = False
                Dim WorkingInstance As Theme = Me._CurrentInstance

                Try
SEARCHPARENT:
                    Dim SearchingControl As String = _
                        String.Format("{0}_{1}", WorkingInstance.CurrentID, ControlID)

                    Dim IsFound As Boolean = _
                        Me._QuickControlMapCache.ContainsKey(SearchingControl)

                    If IsFound Then
                        rControl = Nothing
                        Me._QuickControlMapCache.Item(SearchingControl).Clone(rControl)
                    Else
                        Dim xPathNav As Xml.XPath.XPathNavigator

                        xPathNav = WorkingInstance.ControlMapXPathNavigator.SelectSingleNode(String.Format("//Map[@controlid='{0}']", ControlID))

                        If Not xPathNav Is Nothing Then
                            IsFound = True

                            ' Prepare variables from topnode attributes before going to first child
                            Dim ControlID_local As String = _
                                xPathNav.GetAttribute("controlid", xPathNav.BaseURI)

                            If xPathNav.MoveToFirstChild() Then
                                Dim CompareCulture As New Globalization.CultureInfo("en-US")

                                Dim SecurityInfo As New Globals.Controls.ControlBase.SecurityInfos
                                Dim ControlType As Globals.Controls.ControlTypes
                                Dim CallFunction As String = String.Empty
                                Dim Attributes As New Globals.Controls.AttributeInfo.AttributeInfoCollection
                                Dim BlockIDsToUpdate As New System.Collections.Generic.List(Of String)
                                Dim BlockLocalUpdate As String = String.Empty

                                ' Custom Control Variables
                                Dim DefaultButtonID As String = String.Empty
                                Dim LabelText As String = String.Empty
                                Dim ContentText As String = String.Empty
                                Dim LinkURL As String = String.Empty
                                Dim ImageSource As String = String.Empty

                                Do
                                    Select Case xPathNav.Name.ToLower(CompareCulture)
                                        Case "type"
                                            Dim xControlType As String = _
                                                    xPathNav.Value

                                            ControlType = Me.ParseControlType(xControlType)

                                        Case "callfunction"
                                            CallFunction = xPathNav.Value

                                        Case "attributes"
                                            Dim ChildReader As Xml.XPath.XPathNavigator = _
                                                    xPathNav.Clone()

                                            If ChildReader.MoveToFirstChild() Then
                                                Dim xAttName As String, xAttValue As String

                                                Do
                                                    xAttName = ChildReader.GetAttribute("id", ChildReader.BaseURI)
                                                    xAttValue = ChildReader.Value

                                                    Attributes.Add(xAttName.ToLower(), xAttValue)
                                                Loop While ChildReader.MoveToNext()
                                            End If

                                        Case "security"
                                            Dim ChildReader As Xml.XPath.XPathNavigator = _
                                                    xPathNav.Clone()

                                            If ChildReader.MoveToFirstChild() Then
                                                Dim RegisteredGroup As String = String.Empty, FriendlyName As String = String.Empty, _
                                                    SecurityCallFunction As String = "#GLOBAL", DisabledValue As String = String.Empty, _
                                                    DisabledType As String = "Inherited", DisabledSet As Boolean = False

                                                Do
                                                    Select Case ChildReader.Name.ToLower(CompareCulture)
                                                        Case "registeredgroup"
                                                            RegisteredGroup = ChildReader.Value

                                                        Case "friendlyname"
                                                            FriendlyName = ChildReader.Value

                                                        Case "callfunction"
                                                            SecurityCallFunction = ChildReader.Value

                                                        Case "disabled"
                                                            DisabledSet = True
                                                            DisabledType = ChildReader.GetAttribute("type", ChildReader.NamespaceURI)
                                                            If String.IsNullOrEmpty(DisabledType) Then DisabledType = "Inherited"
                                                            DisabledValue = ChildReader.Value

                                                    End Select
                                                Loop While ChildReader.MoveToNext()

                                                With SecurityInfo
                                                    .SecuritySet = True
                                                    .RegisteredGroup = RegisteredGroup
                                                    .FriendlyName = FriendlyName
                                                    .CallFunction = SecurityCallFunction
                                                    .Disabled.IsSet = DisabledSet
                                                    .Disabled.Type = CType( _
                                                                            [Enum].Parse( _
                                                                                GetType(Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes), _
                                                                                DisabledType, True _
                                                                            ), Globals.Controls.ControlBase.SecurityInfos.DisabledClass.DisabledTypes _
                                                                        )
                                                    .Disabled.Value = DisabledValue
                                                End With
                                            End If

                                        Case "blockidstoupdate"
                                            BlockLocalUpdate = xPathNav.GetAttribute("localupdate", xPathNav.BaseURI)

                                            Dim ChildReader As Xml.XPath.XPathNavigator = _
                                                    xPathNav.Clone()

                                            If ChildReader.MoveToFirstChild() Then
                                                Do
                                                    BlockIDsToUpdate.Add(ChildReader.Value)
                                                Loop While ChildReader.MoveToNext()
                                            End If

                                        Case "defaultbuttonid"
                                            DefaultButtonID = xPathNav.Value

                                        Case "text"
                                            LabelText = xPathNav.Value

                                        Case "url"
                                            LinkURL = xPathNav.Value

                                        Case "content"
                                            ContentText = xPathNav.Value

                                        Case "source"
                                            ImageSource = xPathNav.Value

                                    End Select
                                Loop While xPathNav.MoveToNext()

                                ' Correct LocalUpdateValue
                                If String.Compare(BlockLocalUpdate, "True", True) = 0 Then
                                    BlockLocalUpdate = "True"
                                ElseIf String.Compare(BlockLocalUpdate, "False", True) = 0 Then
                                    BlockLocalUpdate = "False"
                                Else
                                    BlockLocalUpdate = "True"
                                End If

                                Select Case ControlType
                                    Case Globals.Controls.ControlTypes.Textbox
                                        rControl = New Globals.Controls.Textbox(ControlID_local)

                                        CType(rControl, Globals.Controls.Textbox).DefaultButtonID = DefaultButtonID.Trim()
                                        CType(rControl, Globals.Controls.Textbox).Text = LabelText

                                    Case Globals.Controls.ControlTypes.Password
                                        rControl = New Globals.Controls.Password(ControlID_local)

                                        CType(rControl, Globals.Controls.Password).DefaultButtonID = DefaultButtonID.Trim()
                                        CType(rControl, Globals.Controls.Password).Text = LabelText

                                    Case Globals.Controls.ControlTypes.Checkbox
                                        rControl = New Globals.Controls.CheckBox(ControlID_local)

                                        CType(rControl, Globals.Controls.CheckBox).Text = LabelText

                                    Case Globals.Controls.ControlTypes.Radio
                                        rControl = New Globals.Controls.RadioButton(ControlID_local)

                                        CType(rControl, Globals.Controls.RadioButton).Text = LabelText

                                    Case Globals.Controls.ControlTypes.Button
                                        rControl = New Globals.Controls.Button(ControlID_local)

                                        CType(rControl, Globals.Controls.Button).Text = LabelText

                                    Case Globals.Controls.ControlTypes.Textarea
                                        rControl = New Globals.Controls.Textarea(ControlID_local)

                                        CType(rControl, Globals.Controls.Textarea).Content = ContentText

                                    Case Globals.Controls.ControlTypes.Image
                                        rControl = New Globals.Controls.Image(ControlID_local)

                                        CType(rControl, Globals.Controls.Image).Source = ImageSource

                                    Case Globals.Controls.ControlTypes.Link
                                        rControl = New Globals.Controls.Link(ControlID_local)

                                        CType(rControl, Globals.Controls.Link).Url = LinkURL
                                        CType(rControl, Globals.Controls.Link).Text = LabelText

                                    Case Globals.Controls.ControlTypes.DataList
                                        rControl = New Globals.Controls.DataList(ControlID_local)

                                    Case Globals.Controls.ControlTypes.ConditionalStatement
                                        rControl = New Globals.Controls.ConditionalStatement(ControlID_local)

                                    Case Globals.Controls.ControlTypes.VariableBlock
                                        rControl = New Globals.Controls.VariableBlock(ControlID_local)

                                    Case Globals.Controls.ControlTypes.Unknown
                                        rControl = New Globals.Controls.ControlBase( _
                                                        ControlID_local, Globals.Controls.ControlTypes.Unknown)

                                End Select

                                With rControl
                                    If String.Compare(SecurityInfo.CallFunction, "#GLOBAL") = 0 Then
                                        SecurityInfo.CallFunction = Me._CurrentInstance.Settings.Configurations.DefaultSecurityCallFunction
                                        If String.Compare(SecurityInfo.CallFunction, "#GLOBAL") = 0 AndAlso _
                                            Not Me._ParentInstance Is Nothing Then

                                            SecurityInfo.CallFunction = Me._ParentInstance.Settings.Configurations.DefaultSecurityCallFunction
                                        End If
                                    End If

                                    .SecurityInfo = SecurityInfo
                                    .CallFunction = CallFunction.Trim()
                                    .Attributes.AddRange(Attributes)
                                    .BlockIDsToUpdate.AddRange(BlockIDsToUpdate)
                                    .BlockLocalUpdate = Boolean.Parse(BlockLocalUpdate)
                                End With
                            End If

                            Dim rControl_clone As Globals.Controls.IControlBase = Nothing
                            rControl.Clone(rControl_clone)
                            Me._QuickControlMapCache.Item(SearchingControl) = rControl_clone
                        End If
                    End If

                    If Not IsFound AndAlso Not ParentSearched Then
                        ' Search Parent For Control
                        WorkingInstance = Me._ParentInstance

                        ParentSearched = True

                        GoTo SEARCHPARENT
                    End If
                Catch ex As Exception
                    ' Just Handle Exceptions
                End Try

                Return rControl
            End Function

            Private Function ParseControlType(ByVal cTString As String) As Globals.Controls.ControlTypes
                Dim rControlType As Globals.Controls.ControlTypes

                If Not [Enum].TryParse(Of Globals.Controls.ControlTypes)(cTString, True, rControlType) Then _
                    rControlType = Globals.Controls.ControlTypes.Unknown

                Return rControlType
            End Function

            Private Function PrepareException(ByVal Message As String, ByVal Source As String, ByVal InnerException As Exception) As Exception
                Dim rException As Exception

                If InnerException Is Nothing Then
                    rException = New Exception(Message)
                Else
                    rException = New Exception(Message, InnerException)
                End If

                ' Set Exception Source
                rException.Source = Source

                Return rException
            End Function

            Private Function SplitContentByControlID(ByVal ControlID As String, ByVal Content As String) As String()
                Dim rResultList As New ArrayList
                Dim SearchString As String = String.Format("}}:{0}:{{", ControlID)

                Dim sIdx As Integer, cIdx As Integer = 0

                Do
                    sIdx = Content.IndexOf(SearchString, cIdx)

                    If sIdx > -1 Then
                        rResultList.Add(Content.Substring(cIdx, sIdx - cIdx))

                        ' Set cIdx and Move Forward to Length of SearchString
                        cIdx = sIdx + SearchString.Length
                    Else
                        rResultList.Add(Content.Substring(cIdx))
                    End If
                Loop Until sIdx = -1

                Return CType(rResultList.ToArray(GetType(String)), String())
            End Function

#End Region
        End Class

#Region " Theme Web Service Class "
        Public Class WebServiceClass
            Implements PGlobals.ITheme.IWebService

            Private _ParentID As String
            Private _CurrentID As String

            Public Sub New(ByVal ParentID As String, ByVal CurrentID As String)
                Me._ParentID = ParentID
                Me._CurrentID = CurrentID
            End Sub

            Public Function CreateWebServiceAuthentication(ByVal ParamArray dItems() As System.Collections.DictionaryEntry) As String Implements PGlobals.ITheme.IWebService.CreateWebServiceAuthentication
                Dim rString As String = Nothing

                If Not dItems Is Nothing Then
                    rString = Guid.NewGuid.ToString()

                    Dim SessionInfo As New Globals.WebServiceSessionInfo(rString, Date.Now)
                    For Each dItem As DictionaryEntry In dItems
                        SessionInfo.AddSessionItem(dItem.Key.ToString(), dItem.Value)
                    Next

                    Me.VariablePool.RegisterVariableToPool(rString, SessionInfo)
                End If

                Return rString
            End Function

            Public ReadOnly Property ReadSessionVariable(ByVal PublicKey As String, ByVal name As String) As Object Implements PGlobals.ITheme.IWebService.ReadSessionVariable
                Get
                    Dim rObject As Object = Nothing

                    Dim SessionInfo As Globals.WebServiceSessionInfo = _
                        CType( _
                            Me.VariablePool.GetVariableFromPool(PublicKey),  _
                            Globals.WebServiceSessionInfo _
                        )

                    If Not SessionInfo Is Nothing Then
                        Dim cfg As System.Configuration.Configuration = _
                                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration( _
                                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
                        Dim wConfig As System.Web.Configuration.SessionStateSection = _
                            CType(cfg.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)
                        Dim TimeoutMinute As Integer = wConfig.Timeout.Minutes
                        If TimeoutMinute = 0 Then TimeoutMinute = 20

                        If SessionInfo.SessionDate.AddMinutes(TimeoutMinute) > Date.Now Then
                            rObject = SessionInfo.Item(name)

                            ' Keep session is active
                            SessionInfo.SessionDate = Date.Now
                            ' !--
                        Else
                            SessionInfo = Nothing
                        End If

                        Me.VariablePool.RegisterVariableToPool(PublicKey, SessionInfo)
                    End If

                    Return rObject
                End Get
            End Property

            Public Function RenderWebService(ByVal ExecuteIn As String, ByVal TemplateID As String, ByVal Arguments As Globals.ArgumentInfo.ArgumentInfoCollection) As String
                ' call = Calling Function Providing in Query String
                Dim callFunction As String = _
                        String.Format( _
                            "{0}?{1}.{2},~execParams", _
                            ExecuteIn, _
                            TemplateID, _
                            SolidDevelopment.Web.General.Context.Request.QueryString.Item("call"))

                Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
                If Me._ParentID Is Nothing Then
                    tAssembleResultInfo = [Assembly].AssemblePostBackInformation( _
                                                    callFunction, Arguments)
                Else
                    tAssembleResultInfo = [Assembly].AssemblePostBackInformation( _
                                                    Me._ParentID, Me._CurrentID, callFunction, Arguments)
                End If

                Return Me.GenerateWebServiceXML(tAssembleResultInfo.MethodResult)
            End Function

            Friend Function GenerateWebServiceXML(ByRef MethodResult As Object) As String
                Dim xmlStream As New IO.StringWriter()
                Dim xmlWriter As New Xml.XmlTextWriter(xmlStream)

                ' Start Document Element
                Dim IsDone As Boolean

                SolidDevelopment.Web.PGlobals.Execution.ExamMethodExecuted(MethodResult, IsDone)

                xmlWriter.WriteStartElement("ServiceResult")
                xmlWriter.WriteAttributeString("isdone", IsDone.ToString())

                xmlWriter.WriteStartElement("Item")

                If Not MethodResult Is Nothing Then
                    If TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)

                        xmlWriter.WriteCData(CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder).Location)
                    ElseIf TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.MessageResult Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                        xmlWriter.WriteAttributeString("messagetype", CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.MessageResult).Type.ToString())

                        xmlWriter.WriteCData(CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.MessageResult).Message)
                    ElseIf TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.ConditionalStatementResult Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)

                        xmlWriter.WriteCData(CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.ConditionalStatementResult).ConditionResult.ToString())
                    ElseIf TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.VariableBlockResult Then
                        Dim VariableBlockResult As SolidDevelopment.Web.PGlobals.MapControls.VariableBlockResult = _
                            CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.VariableBlockResult)

                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                        xmlWriter.WriteAttributeString("cultureinfo", System.Globalization.CultureInfo.CurrentCulture.ToString())

                        xmlWriter.WriteStartElement("VariableList")
                        For Each key As String In VariableBlockResult.Keys
                            xmlWriter.WriteStartElement("Variable")
                            xmlWriter.WriteAttributeString("key", key)

                            If MethodResult.GetType().IsPrimitive Then
                                xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                                xmlWriter.WriteCData(CType(MethodResult, String))
                            Else
                                xmlWriter.WriteAttributeString("type", GetType(String).FullName)

                                xmlWriter.WriteCData(MethodResult.ToString())
                            End If

                            If VariableBlockResult.Item(key) Is Nothing Then
                                xmlWriter.WriteAttributeString("type", GetType(Object).FullName)
                                xmlWriter.WriteCData(String.Empty)
                            Else
                                If VariableBlockResult.Item(key).GetType().IsPrimitive Then
                                    xmlWriter.WriteAttributeString("type", VariableBlockResult.Item(key).GetType().FullName)
                                Else
                                    xmlWriter.WriteAttributeString("type", GetType(String).FullName)
                                End If

                                xmlWriter.WriteCData( _
                                    CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.VariableBlockResult).Item(key).ToString() _
                                )
                            End If

                            xmlWriter.WriteEndElement()
                        Next
                        xmlWriter.WriteEndElement()
                    ElseIf TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.DirectDataReader Then
                        Dim ex As New Exception("DirectDataReader is not a transferable object!")

                        xmlWriter.WriteAttributeString("type", ex.GetType().FullName)
                        xmlWriter.WriteCData(ex.Message)
                    ElseIf TypeOf MethodResult Is SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().Name)
                        xmlWriter.WriteAttributeString("totalcount", CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable).TotalCount.ToString())
                        xmlWriter.WriteAttributeString("cultureinfo", System.Globalization.CultureInfo.CurrentCulture.ToString())

                        Dim tDT As DataTable = CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable).ThisContainer.Copy()

                        xmlWriter.WriteStartElement("Columns")
                        For Each dC As DataColumn In tDT.Columns
                            xmlWriter.WriteStartElement("Column")
                            xmlWriter.WriteAttributeString("name", dC.ColumnName)
                            xmlWriter.WriteAttributeString("type", dC.DataType.FullName)
                            xmlWriter.WriteEndElement()
                        Next
                        xmlWriter.WriteEndElement()

                        Dim rowIndex As Integer = 0

                        xmlWriter.WriteStartElement("Rows")
                        For Each dR As DataRow In tDT.Rows
                            xmlWriter.WriteStartElement("Row")
                            xmlWriter.WriteAttributeString("index", rowIndex.ToString())
                            For Each dC As DataColumn In tDT.Columns
                                xmlWriter.WriteStartElement("Item")
                                xmlWriter.WriteAttributeString("name", dC.ColumnName)
                                xmlWriter.WriteCData(dR.Item(dC.ColumnName).ToString())
                                xmlWriter.WriteEndElement()
                            Next
                            xmlWriter.WriteEndElement()

                            rowIndex += 1
                        Next
                        xmlWriter.WriteEndElement()

                        If Not CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable).MessageResult Is Nothing Then
                            xmlWriter.WriteStartElement("MessageResult")
                            xmlWriter.WriteAttributeString("messagetype", CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable).MessageResult.Type.ToString())
                            xmlWriter.WriteCData( _
                                CType(MethodResult, SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable).MessageResult.Message _
                            )
                            xmlWriter.WriteEndElement()
                        End If
                    ElseIf TypeOf MethodResult Is System.Exception Then
                        xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                        xmlWriter.WriteCData(CType(MethodResult, System.Exception).Message)
                    Else
                        If MethodResult.GetType().IsPrimitive Then
                            xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                            xmlWriter.WriteCData(CType(MethodResult, String))
                        Else
                            Try
                                Dim SerializedValue As String = _
                                    SerializeHelpers.BinaryToBase64Serializer(MethodResult)

                                xmlWriter.WriteAttributeString("type", MethodResult.GetType().FullName)

                                xmlWriter.WriteCData(SerializedValue)
                            Catch ex As Exception
                                xmlWriter.WriteAttributeString("type", ex.GetType().FullName)

                                xmlWriter.WriteCData(ex.Message)
                            End Try
                        End If
                    End If
                Else
                    xmlWriter.WriteAttributeString("type", GetType(String).FullName)
                    xmlWriter.WriteCData(String.Empty)
                End If

                xmlWriter.WriteEndElement()

                ' End Document Element
                xmlWriter.WriteEndElement()

                xmlWriter.Flush()
                xmlWriter.Close()
                xmlStream.Close()

                Return xmlStream.ToString()
            End Function

            Private ReadOnly Property VariablePool() As General.VariablePoolOperationsClass
                Get
                    Return General.VariablePoolForWebService
                End Get
            End Property

            Private Class SerializeHelpers
                Public Shared Function BinaryToBase64Serializer(ByVal [Object] As Object) As String
                    Dim serializedBytes As Byte() = SerializeHelpers.BinarySerializer([Object])

                    Return System.Convert.ToBase64String(serializedBytes)
                End Function

                Public Shared Function BinarySerializer(ByVal [Object] As Object) As Byte()
                    Dim rByte As Byte()

                    Dim BinaryFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    BinaryFormater.Binder = New SolidDevelopment.Web.Helpers.OverrideBinder()

                    Dim SerializationStream As New IO.MemoryStream

                    BinaryFormater.Serialize(SerializationStream, [Object])

                    rByte = CType(Array.CreateInstance(GetType(Byte), SerializationStream.Position), Byte())

                    SerializationStream.Seek(0, IO.SeekOrigin.Begin)
                    SerializationStream.Read(rByte, 0, rByte.Length)

                    SerializationStream.Close()

                    GC.SuppressFinalize(SerializationStream)

                    Return rByte
                End Function

                Public Shared Function Base64ToBinaryDeSerializer(ByVal SerializedString As String) As Object
                    Dim serializedBytes As Byte() = System.Convert.FromBase64String(SerializedString)

                    Return SerializeHelpers.BinaryDeSerializer(serializedBytes)
                End Function

                Public Shared Function BinaryDeSerializer(ByVal SerializedBytes As Byte()) As Object
                    Dim rObject As Object

                    Dim BinaryFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    BinaryFormater.Binder = New SolidDevelopment.Web.Helpers.OverrideBinder()

                    Dim SerializationStream As New IO.MemoryStream(SerializedBytes)

                    rObject = BinaryFormater.Deserialize(SerializationStream)

                    SerializationStream.Close()

                    GC.SuppressFinalize(SerializationStream)

                    Return rObject
                End Function
            End Class
        End Class
#End Region

#Region " Theme Addons "
        Public Class AddonsClass
            Implements PGlobals.ITheme.IAddons

            Private _AddonInfos As PGlobals.ThemeInfo.AddonInfo()

            Private _CurrentInstance As PGlobals.ITheme
            Private _CurrentThemeID As String
            Private _CurrentThemeTranslationID As String

            Public Sub New(ByVal CurrentThemeID As String, ByVal CurrentThemeTranslationID As String)
                Me._AddonInfos = Nothing

                Me._CurrentInstance = Nothing
                Me._CurrentThemeID = CurrentThemeID
                Me._CurrentThemeTranslationID = CurrentThemeTranslationID
            End Sub

            Public ReadOnly Property AddonInfos() As PGlobals.ThemeInfo.AddonInfo() Implements PGlobals.ITheme.IAddons.AddonInfos
                Get
                    If Me._AddonInfos Is Nothing Then [Assembly].QueryThemeAddons(Me._CurrentThemeID, Me._AddonInfos)

                    Return Me._AddonInfos
                End Get
            End Property

            Public ReadOnly Property CurrentInstance() As PGlobals.ITheme Implements PGlobals.ITheme.IAddons.CurrentInstance
                Get
                    Return Me._CurrentInstance
                End Get
            End Property

            Public Sub CreateInstance(ByVal AddonInfo As PGlobals.ThemeInfo.AddonInfo) Implements PGlobals.ITheme.IAddons.CreateInstance
                If Not Me._CurrentInstance Is Nothing Then Me._CurrentInstance.Dispose()

                Me._CurrentInstance = New Theme(Me._CurrentThemeID, AddonInfo.AddonID, Me._CurrentThemeTranslationID, AddonInfo.AddonPassword)

                SolidDevelopment.Web.General.CurrentInstantAddonID = AddonInfo.AddonID
            End Sub

            Public Sub DisposeInstance() Implements PGlobals.ITheme.IAddons.DisposeInstance
                If Not Me._CurrentInstance Is Nothing Then
                    SolidDevelopment.Web.General.CurrentInstantAddonID = Nothing

                    Me._CurrentInstance.Dispose()
                    Me._CurrentInstance = Nothing
                End If
            End Sub

            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then Me.DisposeInstance()

                Me.disposedValue = True
            End Sub

#Region " IDisposable Support "
            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region

#Region " Theme Settings "
        Public Class SettingsClass
            Implements PGlobals.ITheme.ISettings
            Implements IDisposable

            Private _xPathConfigurationStream As IO.StringReader = Nothing
            Private _ConfigurationClassNavigator As Xml.XPath.XPathNavigator

            Private _Configurations As ConfigurationsClass
            Private _Services As ServicesClass
            Private _URLMappings As URLMappingClass

            Public Sub New(ByVal ConfigurationContent As String)
                If ConfigurationContent Is Nothing OrElse _
                    ConfigurationContent.Trim().Length = 0 Then

                    Throw New Exception(Globals.SystemMessages.CONFIGURATIONCONTENT & "!")
                End If

                Dim Exception As Exception = Nothing

                Dim xPathDoc As Xml.XPath.XPathDocument

                Try
                    ' Performance Optimization
                    Me._xPathConfigurationStream = New IO.StringReader(ConfigurationContent)
                    xPathDoc = New Xml.XPath.XPathDocument(Me._xPathConfigurationStream)

                    Me._ConfigurationClassNavigator = xPathDoc.CreateNavigator()
                    ' !--
                Catch ex As Exception
                    If Not Me._xPathConfigurationStream Is Nothing Then Me._xPathConfigurationStream.Close() : GC.SuppressFinalize(Me._xPathConfigurationStream)

                    Exception = ex
                End Try

                If Not Exception Is Nothing Then Throw Exception

                Me._Configurations = New ConfigurationsClass(Me._ConfigurationClassNavigator)
                Me._Services = New ServicesClass(Me._ConfigurationClassNavigator)
                Me._URLMappings = New URLMappingClass(Me._ConfigurationClassNavigator)
            End Sub

            Public ReadOnly Property Configurations() As PGlobals.ITheme.ISettings.IConfigurationClass Implements PGlobals.ITheme.ISettings.Configurations
                Get
                    Return Me._Configurations
                End Get
            End Property

            Public ReadOnly Property Services() As SettingsClass.ServicesClass
                Get
                    Return Me._Services
                End Get
            End Property

            Public ReadOnly Property URLMappings() As SettingsClass.URLMappingClass
                Get
                    Return Me._URLMappings
                End Get
            End Property

#Region " Theme Settings Configuration Class "
            Public Class ConfigurationsClass
                Implements PGlobals.ITheme.ISettings.IConfigurationClass

                Private _ConfigurationClassNavigator As Xml.XPath.XPathNavigator

                Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
                    Me._ConfigurationClassNavigator = ConfigurationNavigator.Clone()
                End Sub

                Public ReadOnly Property AuthenticationPage() As String Implements PGlobals.ITheme.ISettings.IConfigurationClass.AuthenticationPage
                    Get
                        Dim _AuthenticationPage As String = _
                            Me.ReadConfiguration("authenticationpage")

                        If _AuthenticationPage Is Nothing Then _AuthenticationPage = Me.DefaultPage

                        Return _AuthenticationPage
                    End Get
                End Property

                Public ReadOnly Property DefaultPage() As String Implements PGlobals.ITheme.ISettings.IConfigurationClass.DefaultPage
                    Get
                        Return Me.ReadConfiguration("defaultpage")
                    End Get
                End Property

                Public ReadOnly Property DefaultTranslation() As String Implements PGlobals.ITheme.ISettings.IConfigurationClass.DefaultTranslation
                    Get
                        Return Me.ReadConfiguration("defaulttranslation")
                    End Get
                End Property

                Public ReadOnly Property DefaultCaching() As PGlobals.PageCachingTypes Implements PGlobals.ITheme.ISettings.IConfigurationClass.DefaultCaching
                    Get
                        Dim rPageCaching As PGlobals.PageCachingTypes = PGlobals.PageCachingTypes.AllContent
                        Dim configString As String = Me.ReadConfiguration("defaultcaching")

                        For Each nItem As String In [Enum].GetNames(GetType(PGlobals.PageCachingTypes))
                            If String.Compare(nItem, configString, True) = 0 Then
                                rPageCaching = CType([Enum].Parse(GetType(PGlobals.PageCachingTypes), nItem), PGlobals.PageCachingTypes)

                                Exit For
                            End If
                        Next

                        Return rPageCaching
                    End Get
                End Property

                Public ReadOnly Property DefaultSecurityCallFunction() As String Implements PGlobals.ITheme.ISettings.IConfigurationClass.DefaultSecurityCallFunction
                    Get
                        Return Me.ReadConfiguration("defaultsecuritycallfunction")
                    End Get
                End Property

                Private Function ReadConfiguration(ByVal key As String) As String
                    Dim rString As String = Nothing

                    Dim xPathIter As Xml.XPath.XPathNodeIterator

                    Try
                        xPathIter = Me._ConfigurationClassNavigator.Select(String.Format("//Configuration/Item[@key='{0}']", key))

                        If xPathIter.MoveNext() Then rString = xPathIter.Current.GetAttribute("value", xPathIter.Current.BaseURI)
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try

                    Return rString
                End Function

            End Class
#End Region

#Region " Theme Settings Services Class "
            Public Class ServicesClass
                Private _ConfigurationClassNavigator As Xml.XPath.XPathNavigator

                Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
                    Me._ConfigurationClassNavigator = ConfigurationNavigator.Clone()
                End Sub

                Public ReadOnly Property ServiceItems() As ServiceItem.ServiceItemCollection
                    Get
                        Return Me.ReadServiceOptions()
                    End Get
                End Property

                Private Function ReadServiceOptions() As ServiceItem.ServiceItemCollection
                    Dim rCollection As New ServiceItem.ServiceItemCollection

                    Dim xPathIter As Xml.XPath.XPathNodeIterator

                    Try
                        ' Read Authentication Keys
                        xPathIter = Me._ConfigurationClassNavigator.Select("//Services/AuthenticationKeys/Item")

                        Dim AuthenticationKeys As New System.Collections.Generic.List(Of String)

                        Do While xPathIter.MoveNext()
                            AuthenticationKeys.Add(xPathIter.Current.GetAttribute("id", xPathIter.Current.BaseURI))
                        Loop

                        xPathIter = Me._ConfigurationClassNavigator.Select("//Services/Item")

                        Dim tServiceItem As ServiceItem
                        Dim ID As String, mimeType As String, Authentication As String, Type As String, ExecuteIn As String, StandAlone As String, [Overridable] As String

                        Do While xPathIter.MoveNext()
                            ID = xPathIter.Current.GetAttribute("id", xPathIter.Current.BaseURI)
                            Type = xPathIter.Current.GetAttribute("type", xPathIter.Current.BaseURI)
                            [Overridable] = xPathIter.Current.GetAttribute("overridable", xPathIter.Current.BaseURI)
                            Authentication = xPathIter.Current.GetAttribute("authentication", xPathIter.Current.BaseURI)
                            StandAlone = xPathIter.Current.GetAttribute("standalone", xPathIter.Current.BaseURI)
                            ExecuteIn = xPathIter.Current.GetAttribute("executein", xPathIter.Current.BaseURI)
                            mimeType = xPathIter.Current.GetAttribute("mime", xPathIter.Current.BaseURI)

                            tServiceItem = New ServiceItem(ID)

                            tServiceItem.ServiceType = CType([Enum].Parse(GetType(ServiceItem.ServiceTypes), Type, False), ServiceItem.ServiceTypes)
                            Boolean.TryParse([Overridable], tServiceItem.Overridable)
                            Boolean.TryParse(Authentication, tServiceItem.Authentication)
                            tServiceItem.AuthenticationKeys = AuthenticationKeys.ToArray()
                            Boolean.TryParse(StandAlone, tServiceItem.StandAlone)
                            tServiceItem.ExecuteIn = ExecuteIn

                            If tServiceItem.ServiceType = ServiceItem.ServiceTypes.webservice Then
                                tServiceItem.MimeType = "text/xml"
                            Else
                                If Not String.IsNullOrEmpty(mimeType) Then _
                                    tServiceItem.MimeType = mimeType
                            End If

                            rCollection.Add(tServiceItem)
                        Loop
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try

                    Return rCollection
                End Function

                Public Class ServiceItem
                    Private _ID As String
                    Private _MimeType As String
                    Private _ServiceType As ServiceTypes
                    Private _ExecuteIn As String
                    Private _Authentication As Boolean
                    Private _AuthenticationKeys As String()
                    Private _AuthenticationTypes As String()
                    Private _StandAlone As Boolean
                    Private _Overridable As Boolean

                    Public Enum ServiceTypes As Byte
                        template = 1
                        webservice = 2
                    End Enum

                    Public Sub New(ByVal ID As String)
                        Me._ID = ID
                        Me._MimeType = "text/html"
                        Me._ServiceType = ServiceTypes.template
                        Me._ExecuteIn = String.Empty
                        Me._Authentication = False
                        Me._AuthenticationKeys = New String() {}
                        Me._StandAlone = False
                        Me._Overridable = False
                    End Sub

                    Public Property ID() As String
                        Get
                            Return Me._ID
                        End Get
                        Set(ByVal Value As String)
                            Me._ID = Value
                        End Set
                    End Property

                    Public Property MimeType() As String
                        Get
                            Return Me._MimeType
                        End Get
                        Set(ByVal value As String)
                            Me._MimeType = value
                        End Set
                    End Property

                    Public Property Authentication() As Boolean
                        Get
                            Return Me._Authentication
                        End Get
                        Set(ByVal Value As Boolean)
                            Me._Authentication = Value
                        End Set
                    End Property

                    Public Property AuthenticationKeys() As String()
                        Get
                            Return Me._AuthenticationKeys
                        End Get
                        Set(ByVal Value As String())
                            Me._AuthenticationKeys = Value
                        End Set
                    End Property

                    Public Property StandAlone() As Boolean
                        Get
                            Return Me._StandAlone
                        End Get
                        Set(ByVal Value As Boolean)
                            Me._StandAlone = Value
                        End Set
                    End Property

                    Public Property [Overridable]() As Boolean
                        Get
                            Return Me._Overridable
                        End Get
                        Set(ByVal Value As Boolean)
                            Me._Overridable = Value
                        End Set
                    End Property

                    Public Property ServiceType() As ServiceTypes
                        Get
                            Return Me._ServiceType
                        End Get
                        Set(ByVal Value As ServiceTypes)
                            Me._ServiceType = Value
                        End Set
                    End Property

                    Public Property ExecuteIn() As String
                        Get
                            Return Me._ExecuteIn
                        End Get
                        Set(ByVal Value As String)
                            Me._ExecuteIn = Value
                        End Set
                    End Property

                    Public Class ServiceItemCollection
                        Inherits Generic.List(Of ServiceItem)

                        Public Sub New()
                            MyBase.New()
                        End Sub

                        Public Function GetServiceItem(ByVal ServiceType As ServiceTypes, ByVal ID As String) As ServiceItem
                            Dim rServiceItem As ServiceItem = Nothing

                            For Each sI As ServiceItem In Me
                                If sI.ServiceType = ServiceType AndAlso _
                                    String.Compare(sI.ID, ID, True) = 0 Then

                                    rServiceItem = sI

                                    Exit For
                                End If
                            Next

                            Return rServiceItem
                        End Function

                        Public Function GetServiceItems(ByVal ServiceType As ServiceTypes) As ServiceItemCollection
                            Dim rCollection As New ServiceItemCollection

                            For Each sI As ServiceItem In Me.ToArray()
                                If sI.ServiceType = ServiceType Then rCollection.Add(sI)
                            Next

                            Return rCollection
                        End Function

                        Public Function GetAuthenticationKeys() As String()
                            Dim rAuthenticationKeys As String() = New String() {}

                            If Me.Count > 0 Then rAuthenticationKeys = Me.Item(0).AuthenticationKeys

                            Return rAuthenticationKeys
                        End Function
                    End Class
                End Class

            End Class
#End Region

#Region " Theme Settings URLMapping Class "
            Public Class URLMappingClass
                Private _ConfigurationClassNavigator As Xml.XPath.XPathNavigator

                Private _MappingItems As SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem.URLMappingItemCollection
                Private _IsURLMappingActive As Boolean

                Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
                    Me._ConfigurationClassNavigator = ConfigurationNavigator.Clone()

                    Me.PrepareMappingOptions()
                End Sub

                Public ReadOnly Property URLMapping() As Boolean
                    Get
                        Return Me._IsURLMappingActive
                    End Get
                End Property

                Public ReadOnly Property MappingItems() As SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem.URLMappingItemCollection
                    Get
                        Return Me._MappingItems
                    End Get
                End Property

                Private Sub PrepareMappingOptions()
                    Me._MappingItems = New SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem.URLMappingItemCollection()

                    Dim xPathIter As Xml.XPath.XPathNodeIterator, xPathIter_in As Xml.XPath.XPathNodeIterator

                    Try
                        xPathIter = Me._ConfigurationClassNavigator.Select(String.Format("//URLMapping"))

                        If xPathIter.MoveNext() Then
                            If Not Boolean.TryParse( _
                                    xPathIter.Current.GetAttribute("active", xPathIter.Current.BaseURI), _
                                    Me._IsURLMappingActive) Then

                                Me._IsURLMappingActive = False
                            End If
                        End If

                        ' If mapping is active then read mapping items
                        If Me._IsURLMappingActive Then
                            ' Read URLMapping Options
                            xPathIter = Me._ConfigurationClassNavigator.Select(String.Format("//URLMapping/Map"))

                            Dim tMappingItem As SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem
                            Dim [Overridable] As Boolean = False, Priority As Integer, Priority_t As String, Request As String = String.Empty

                            Dim Reverse_ID As String = String.Empty, Reverse_Mapped As String = String.Empty
                            Dim Reverse_MappedItems As SolidDevelopment.Web.PGlobals.URLMappingInfos.ResolveInfos.MappedItem.MappedItemCollection = Nothing

                            Do While xPathIter.MoveNext()
                                Priority_t = xPathIter.Current.GetAttribute("priority", xPathIter.Current.BaseURI)

                                If Not Integer.TryParse(Priority_t, Priority) Then Priority = 0

                                xPathIter_in = xPathIter.Clone()

                                If xPathIter_in.Current.MoveToFirstChild() Then
                                    Do
                                        Select Case xPathIter_in.Current.Name
                                            Case "Request"
                                                xPathIter_in.Current.MoveToFirstChild()

                                                Request = xPathIter_in.Current.Value

                                                xPathIter_in.Current.MoveToParent()
                                            Case "Reverse"
                                                Reverse_ID = xPathIter_in.Current.GetAttribute("id", xPathIter_in.Current.BaseURI)

                                                Dim xPathIter_servicetest As System.Xml.XPath.XPathNodeIterator = _
                                                    Me._ConfigurationClassNavigator.Select(String.Format("//Services/Item[@id='{0}']", Reverse_ID))

                                                If xPathIter_servicetest.MoveNext() Then _
                                                    If Not Boolean.TryParse(xPathIter.Current.GetAttribute("overridable", xPathIter.Current.BaseURI), [Overridable]) Then [Overridable] = False

                                                Reverse_Mapped = xPathIter_in.Current.GetAttribute("mapped", xPathIter_in.Current.BaseURI)
                                                Reverse_MappedItems = New SolidDevelopment.Web.PGlobals.URLMappingInfos.ResolveInfos.MappedItem.MappedItemCollection

                                                If xPathIter_in.Current.MoveToFirstChild() Then
                                                    Do
                                                        Select Case xPathIter_in.Current.Name
                                                            Case "MappedItem"
                                                                Dim MappedItem_ID As String, MappedItem_DefaultValue As String, MappedItem_QueryStringKey As String

                                                                MappedItem_ID = xPathIter_in.Current.GetAttribute("id", xPathIter_in.Current.BaseURI)
                                                                MappedItem_DefaultValue = xPathIter_in.Current.GetAttribute("default", xPathIter_in.Current.BaseURI)

                                                                xPathIter_in.Current.MoveToFirstChild()

                                                                MappedItem_QueryStringKey = xPathIter_in.Current.Value

                                                                xPathIter_in.Current.MoveToParent()

                                                                Dim MappedItem As New SolidDevelopment.Web.PGlobals.URLMappingInfos.ResolveInfos.MappedItem(MappedItem_ID)

                                                                With MappedItem
                                                                    .DefaultValue = MappedItem_DefaultValue
                                                                    .QueryStringKey = MappedItem_QueryStringKey
                                                                End With

                                                                Reverse_MappedItems.Add(MappedItem)
                                                        End Select
                                                    Loop While xPathIter_in.Current.MoveToNext()
                                                End If
                                        End Select
                                    Loop While xPathIter_in.Current.MoveToNext()
                                End If

                                tMappingItem = New SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem()

                                With tMappingItem
                                    .Overridable = [Overridable]
                                    .Priority = Priority
                                    .RequestMap = Request

                                    Dim resInfo As New SolidDevelopment.Web.PGlobals.URLMappingInfos.ResolveInfos(Reverse_ID)

                                    resInfo.MapFormat = Reverse_Mapped
                                    resInfo.MappedItems.AddRange(Reverse_MappedItems)

                                    .ResolveInfo = resInfo
                                End With

                                Me._MappingItems.Add(tMappingItem)
                            Loop
                        End If
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    End Try
                End Sub
            End Class
#End Region

            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then Me._xPathConfigurationStream.Close() : GC.SuppressFinalize(Me._xPathConfigurationStream)

                Me.disposedValue = True
            End Sub

#Region " IDisposable Support "
            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region

#Region " Theme Translation "
        Public Class TranslationClass
            Implements PGlobals.ITheme.ITranslation
            Implements IDisposable

            Private _ParentTranslation As PGlobals.ITheme.ITranslation = Nothing

            Private _CurrentTranslation As String
            Private _CurrentTranslationName As String

            Private _TranslationXPathStream As IO.StringReader = Nothing
            Private _TranslationXPathNavigator As Xml.XPath.XPathNavigator

            Public Sub New(ByVal TranslationContent As String)
                If TranslationContent Is Nothing OrElse _
                    TranslationContent.Trim().Length = 0 Then

                    Throw New Exception(Globals.SystemMessages.TRANSLATIONCONTENT & "!")
                End If

                Dim Exception As Exception = Nothing

                Dim xPathDoc As Xml.XPath.XPathDocument
                Dim xPathIter As Xml.XPath.XPathNodeIterator

                Try
                    ' Performance Optimization
                    Me._TranslationXPathStream = New IO.StringReader(TranslationContent)
                    xPathDoc = New Xml.XPath.XPathDocument(Me._TranslationXPathStream)

                    Me._TranslationXPathNavigator = xPathDoc.CreateNavigator()
                    ' !--

                    xPathIter = Me._TranslationXPathNavigator.Select("/translations")

                    If xPathIter.MoveNext() Then
                        Me._CurrentTranslation = xPathIter.Current.GetAttribute("code", xPathIter.Current.NamespaceURI)
                        Me._CurrentTranslationName = xPathIter.Current.GetAttribute("name", xPathIter.Current.NamespaceURI)
                    End If
                Catch ex As Exception
                    If Not Me._TranslationXPathStream Is Nothing Then Me._TranslationXPathStream.Close() : GC.SuppressFinalize(Me._TranslationXPathStream)

                    Exception = ex
                End Try

                If Not Exception Is Nothing Then Throw Exception
            End Sub

            Friend WriteOnly Property ParentTranslation() As PGlobals.ITheme.ITranslation
                Set(ByVal value As PGlobals.ITheme.ITranslation)
                    Me._ParentTranslation = value
                End Set
            End Property

            Public ReadOnly Property CurrentTranslationInfo() As PGlobals.ThemeInfo.ThemeTranslationInfo Implements PGlobals.ITheme.ITranslation.CurrentTranslationInfo
                Get
                    Return New PGlobals.ThemeInfo.ThemeTranslationInfo(Me._CurrentTranslationName, Me._CurrentTranslation)
                End Get
            End Property

            Public ReadOnly Property CurrentTranslationID() As String Implements PGlobals.ITheme.ITranslation.CurrentTranslationID
                Get
                    Return Me._CurrentTranslation
                End Get
            End Property

            Public ReadOnly Property CurrentTranslationName() As String Implements PGlobals.ITheme.ITranslation.CurrentTranslationName
                Get
                    Return Me._CurrentTranslationName
                End Get
            End Property

            Public Function GetTranslation(ByVal ID As String) As String Implements PGlobals.ITheme.ITranslation.GetTranslation
                Dim rString As String = Nothing

                Dim xPathIter As Xml.XPath.XPathNodeIterator

                Try
                    xPathIter = Me._TranslationXPathNavigator.Select(String.Format("//translation[@id='{0}']", ID))

                    If xPathIter.MoveNext() Then rString = xPathIter.Current.Value

                    If String.IsNullOrEmpty(rString) AndAlso _
                        Not Me._ParentTranslation Is Nothing Then

                        rString = Me._ParentTranslation.GetTranslation(ID)
                    End If
                Catch ex As Exception
                    ' Just Handle Exceptions
                End Try

                Return rString
            End Function

            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue AndAlso Not Me._TranslationXPathStream Is Nothing Then Me._TranslationXPathStream.Close() : GC.SuppressFinalize(Me._TranslationXPathStream)

                Me.disposedValue = True
            End Sub

#Region " IDisposable Support "
            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If Not Me._xPathStream Is Nothing Then Me._xPathStream.Close() : GC.SuppressFinalize(Me._xPathStream)

                Me._Deployment.Dispose()
            End If

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


    End Class
End Namespace