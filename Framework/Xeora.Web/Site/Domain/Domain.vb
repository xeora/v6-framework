Option Strict On

Namespace Xeora.Web.Site
    Public Class Domain
        Implements [Shared].IDomain

        Private _Parent As [Shared].IDomain = Nothing
        Private _Renderer As Renderer = Nothing

        Private _Deployment As Deployment.DomainDeployment = Nothing

        Private _LanguageHolder As LanguageHolder
        Private _ControlsXPathNavigator As Xml.XPath.XPathNavigator

#Region " Constructors "

        Public Sub New(ByVal DomainIDAccessTree As String())
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            Me.BuildDomain(DomainIDAccessTree, LanguageID)
        End Sub

        Private Sub BuildDomain(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            If DomainIDAccessTree Is Nothing Then _
                DomainIDAccessTree = [Shared].Configurations.DefaultDomain

            Try
                Me._Deployment = Deployment.InstanceFactory.Current.GetOrCreate(DomainIDAccessTree, LanguageID)
            Catch ex As Exception.DomainNotExistsException
                ' Try with the default one if requested one is not the default one
                If String.Compare(String.Join("\", DomainIDAccessTree), String.Join("\"c, [Shared].Configurations.DefaultDomain)) <> 0 Then
                    Me._Deployment = Deployment.InstanceFactory.Current.GetOrCreate([Shared].Configurations.DefaultDomain, LanguageID)
                Else
                    Throw
                End If
            Catch ex As System.Exception
                Throw
            End Try

            Me._LanguageHolder = New LanguageHolder(Me, Me._Deployment.Language)

            If DomainIDAccessTree.Length > 1 Then
                Dim ParentDomainIDAccessTree As String() =
                    CType(Array.CreateInstance(GetType(String), DomainIDAccessTree.Length - 1), String())
                Array.Copy(DomainIDAccessTree, 0, ParentDomainIDAccessTree, 0, ParentDomainIDAccessTree.Length)

                Me._Parent = New Domain(ParentDomainIDAccessTree, Me._Deployment.LanguageID)
            End If
            ' !---

            If Me._Renderer Is Nothing Then _
                Me._Renderer = New Renderer()

            Me._Renderer.Inject(Me)
        End Sub

#End Region

        Public ReadOnly Property Parent() As [Shared].IDomain Implements [Shared].IDomain.Parent
            Get
                Return Me._Parent
            End Get
        End Property

        Public ReadOnly Property IDAccessTree() As String() Implements [Shared].IDomain.IDAccessTree
            Get
                Return Me._Deployment.DomainIDAccessTree
            End Get
        End Property

        Public ReadOnly Property DeploymentType() As [Shared].DomainInfo.DeploymentTypes Implements [Shared].IDomain.DeploymentType
            Get
                Return Me._Deployment.DeploymentType
            End Get
        End Property

        Public ReadOnly Property ContentsVirtualPath() As String Implements [Shared].IDomain.ContentsVirtualPath
            Get
                Return String.Format("{0}{1}_{2}",
                            [Shared].Configurations.ApplicationRoot.BrowserImplementation,
                            String.Join(Of String)("-", Me._Deployment.DomainIDAccessTree),
                            Me._Deployment.Language.ID)
            End Get
        End Property

        Public ReadOnly Property Settings() As [Shared].IDomain.ISettings Implements [Shared].IDomain.Settings
            Get
                Return Me._Deployment.Settings
            End Get
        End Property

        Public ReadOnly Property Language() As [Shared].IDomain.ILanguage Implements [Shared].IDomain.Language
            Get
                Return Me._LanguageHolder
            End Get
        End Property

        Public ReadOnly Property xService() As [Shared].IDomain.IxService Implements [Shared].IDomain.xService
            Get
                Return Me._Deployment.xService
            End Get
        End Property

        Public ReadOnly Property Children() As [Shared].DomainInfo.DomainInfoCollection Implements [Shared].IDomain.Children
            Get
                Return Me._Deployment.Children
            End Get
        End Property

        Public Function CheckTemplateExists(ByVal ServiceFullPath As String) As Boolean
            Return Me._Deployment.CheckTemplateExists(ServiceFullPath)
        End Function

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String)
            Me._Deployment.ProvideFileStream(FileStream, RequestedFilePath)
        End Sub

        Public Sub PushLanguageChange(ByVal LanguageID As String)
            Me.BuildDomain(Me._Deployment.DomainIDAccessTree, LanguageID)
        End Sub

        Public Sub ClearCache() Implements [Shared].IDomain.ClearCache
            Controller.Directive.PartialCache.ClearCache(Me.IDAccessTree)

            Me._Deployment.ClearCache()
        End Sub

        Public Overloads Function Render(ByVal ServicePathInfo As [Shared].ServicePathInfo, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String Implements [Shared].IDomain.Render
            Return Me._Renderer.Start(ServicePathInfo, MessageResult, UpdateBlockControlID)
        End Function

        Public Overloads Function Render(ByVal XeoraContent As String, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String Implements [Shared].IDomain.Render
            Return Me._Renderer.Start(XeoraContent, MessageResult, UpdateBlockControlID)
        End Function

        Private _xPathStream As IO.StringReader = Nothing
        Private ReadOnly Property ControlsXPathNavigator() As Xml.XPath.XPathNavigator
            Get
                If Me._ControlsXPathNavigator Is Nothing Then
                    Dim xPathDoc As Xml.XPath.XPathDocument = Nothing
                    Dim ControlMapContent As String =
                        Me._Deployment.ProvideControlsContent()

                    If Not Me._xPathStream Is Nothing Then Me._xPathStream.Close() : GC.SuppressFinalize(Me._xPathStream)

                    Me._xPathStream = New IO.StringReader(ControlMapContent)
                    xPathDoc = New Xml.XPath.XPathDocument(Me._xPathStream)

                    Me._ControlsXPathNavigator = xPathDoc.CreateNavigator()
                End If

                Return Me._ControlsXPathNavigator
            End Get
        End Property

        Private Class LanguageHolder
            Implements [Shared].IDomain.ILanguage

            Private _Owner As [Shared].IDomain
            Private _Language As [Shared].IDomain.ILanguage

            Public Sub New(ByVal Owner As [Shared].IDomain, ByVal Language As [Shared].IDomain.ILanguage)
                Me._Owner = Owner
                Me._Language = Language
            End Sub

            Public ReadOnly Property ID As String Implements [Shared].IDomain.ILanguage.ID
                Get
                    Return Me._Language.ID
                End Get
            End Property

            Public ReadOnly Property Info As [Shared].DomainInfo.LanguageInfo Implements [Shared].IDomain.ILanguage.Info
                Get
                    Return Me._Language.Info
                End Get
            End Property

            Public ReadOnly Property Name As String Implements [Shared].IDomain.ILanguage.Name
                Get
                    Return Me._Language.Name
                End Get
            End Property

            Public Function [Get](ByVal TranslationID As String) As String Implements [Shared].IDomain.ILanguage.Get
                Try
                    Return Me._Language.Get(TranslationID)
                Catch ex As Exception.TranslationNotFoundException
                    If Not Me._Owner.Parent Is Nothing Then _
                        Return Me._Owner.Parent.Language.Get(TranslationID)
                End Try

                Return Nothing
            End Function

            Private disposedValue As Boolean = False ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then Me._Language.Dispose()

                Me.disposedValue = True
            End Sub

#Region "IDisposable Support"
            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
            End Sub
#End Region
        End Class

        Private Class Renderer
            Private _Instance As [Shared].IDomain = Nothing

            Public Sub Inject(ByVal Instance As [Shared].IDomain)
                Me._Instance = Instance
            End Sub

#Region " Template Parsing Procedures "
            Public Overloads Function Start(ByVal ServicePathInfo As [Shared].ServicePathInfo, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String
                If Me._Instance Is Nothing Then Throw New System.Exception("Injection required!")

                Return Me.Start(String.Format("$T:{0}$", ServicePathInfo.FullPath), MessageResult, UpdateBlockControlID)
            End Function

            Public Overloads Function Start(ByVal XeoraContent As String, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String
                If Me._Instance Is Nothing Then Throw New System.Exception("Injection required!")

                Dim ExternalController As Controller.ExternalController =
                    New Controller.ExternalController(0, XeoraContent, Nothing)
                ExternalController.UpdateBlockControlID = UpdateBlockControlID
                ExternalController.MessageResult = MessageResult
                AddHandler ExternalController.ParseRequested, AddressOf Me.OnParseRequest

                ExternalController.Render(Nothing)

                Return ExternalController.RenderedValue
            End Function

            Private Sub OnParseRequest(ByVal DraftValue As String, ByRef ContainerController As Controller.ControllerBase)
                Dim MainPattern As New RegularExpressions.MainCapturePattern()

                Dim MainPatternMatches As Text.RegularExpressions.MatchCollection =
                    MainPattern.Matches(DraftValue)

                If MainPatternMatches.Count = 0 Then
                    ContainerController.Children.Add(
                        New Controller.RenderlessController(0, DraftValue, ContainerController.ContentArguments))
                Else
                    Dim LastIndex As Integer = 0

                    Dim MainSearchMatch As Text.RegularExpressions.Match
                    Dim BracketOpenExamMatch As Text.RegularExpressions.Match, DirectiveType As String = Nothing, DirectiveID As String = Nothing ' For Opening Brackets
                    Dim BracketSeparatorExamMatch As Text.RegularExpressions.Match ' For Separator Brackets
                    Dim BracketCloseExamMatch As Text.RegularExpressions.Match ' For Closing Brackets

                    Dim REMEnum As IEnumerator =
                        MainPatternMatches.GetEnumerator()

                    Dim BracketedOpenPattern As New RegularExpressions.BracketedControllerOpenPattern()
                    Dim BracketedSeparatorPattern As New RegularExpressions.BracketedControllerSeparatorPattern()
                    Dim BracketedClosePattern As New RegularExpressions.BracketedControllerClosePattern()

                    Do While REMEnum.MoveNext()
                        MainSearchMatch = CType(REMEnum.Current, Text.RegularExpressions.Match)

                        ' Check till this match any renderless content exists
                        If MainSearchMatch.Index > LastIndex Then
                            ContainerController.Children.Add(
                                New Controller.RenderlessController(LastIndex, DraftValue.Substring(LastIndex, MainSearchMatch.Index - LastIndex), ContainerController.ContentArguments)
                            )
                            LastIndex = MainSearchMatch.Index
                        End If

                        ' Exam For Bracketed Regex Result
                        BracketOpenExamMatch = BracketedOpenPattern.Match(MainSearchMatch.Value)

                        If BracketOpenExamMatch.Success Then
                            DirectiveType = BracketOpenExamMatch.Result("${DirectiveType}")
                            DirectiveID = BracketOpenExamMatch.Result("${ItemID}")

                            If Not DirectiveID Is Nothing Then
                                Dim InnerMatch As Integer = 0
                                Dim SeparatorIndexes As New Generic.List(Of Integer)

                                Do While REMEnum.MoveNext()
                                    Dim MainSearchMatchExam As Text.RegularExpressions.Match =
                                        CType(REMEnum.Current, Text.RegularExpressions.Match)

                                    ' Exam For Opening Bracketed Regex Result
                                    BracketOpenExamMatch = BracketedOpenPattern.Match(MainSearchMatchExam.Value)
                                    If BracketOpenExamMatch.Success AndAlso
                                        String.Compare(DirectiveID, BracketOpenExamMatch.Result("${ItemID}")) = 0 Then ' Check is Another Same Named Control Internally Opened Bracket

                                        InnerMatch += 1

                                        Continue Do
                                    End If

                                    ' Exam For Separator Bracketed Regex Result
                                    BracketSeparatorExamMatch = BracketedSeparatorPattern.Match(MainSearchMatchExam.Value)
                                    If BracketSeparatorExamMatch.Success AndAlso
                                        String.Compare(DirectiveID, BracketSeparatorExamMatch.Result("${ItemID}")) = 0 AndAlso
                                        InnerMatch = 0 Then ' Check is Same Named Highlevel Control Separator Bracket

                                        ' Point the location of Separator Bracket index
                                        SeparatorIndexes.Add(MainSearchMatchExam.Index - MainSearchMatch.Index)

                                        Continue Do
                                    End If

                                    ' Exam For Closing Bracketed Regex Result
                                    BracketCloseExamMatch = BracketedClosePattern.Match(MainSearchMatchExam.Value)
                                    If BracketCloseExamMatch.Success AndAlso
                                        String.Compare(DirectiveID, BracketCloseExamMatch.Result("${ItemID}")) = 0 Then ' Check is Same Named Control Internally Closed Bracket

                                        If InnerMatch > 0 Then
                                            InnerMatch -= 1

                                            Continue Do
                                        End If

                                        Dim ModifierText As String = String.Format("~{0}", MainSearchMatch.Index)
                                        Dim PointedOriginalValue As String =
                                            DraftValue.Substring(
                                                MainSearchMatch.Index,
                                                (MainSearchMatchExam.Index + MainSearchMatchExam.Length) - MainSearchMatch.Index
                                            )

                                        PointedOriginalValue = PointedOriginalValue.Insert(PointedOriginalValue.Length - 1, ModifierText)
                                        For idxID As Integer = SeparatorIndexes.Count - 1 To 0 Step -1
                                            PointedOriginalValue = PointedOriginalValue.Insert(
                                                                        (SeparatorIndexes(idxID) + String.Format("}}:{0}", DirectiveID).Length),
                                                                        ModifierText
                                                                    )
                                        Next
                                        PointedOriginalValue = PointedOriginalValue.Insert(MainSearchMatch.Length - 2, ModifierText)

                                        Dim WorkingDirective As Controller.DirectiveControllerBase = Nothing

                                        Select Case Controller.DirectiveControllerBase.CaptureDirectiveType(String.Format("${0}:", IIf(String.IsNullOrEmpty(DirectiveType), DirectiveID, DirectiveType)))
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.Control
                                                WorkingDirective = Controller.Directive.ControlBase.MakeControl(MainSearchMatch.Index, PointedOriginalValue, Nothing, AddressOf Me.OnControlResolveRequest)
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.InLineStatement
                                                WorkingDirective = New Controller.Directive.InLineStatement(MainSearchMatch.Index, PointedOriginalValue, Nothing)
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.UpdateBlock
                                                WorkingDirective = New Controller.Directive.UpdateBlock(MainSearchMatch.Index, PointedOriginalValue, Nothing)
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.EncodedExecution
                                                WorkingDirective = New Controller.Directive.EncodedExecution(MainSearchMatch.Index, PointedOriginalValue, Nothing)
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.MessageBlock
                                                WorkingDirective = New Controller.Directive.MessageBlock(MainSearchMatch.Index, PointedOriginalValue, Nothing)
                                            Case Controller.DirectiveControllerBase.DirectiveTypes.PartialCache
                                                WorkingDirective = New Controller.Directive.PartialCache(MainSearchMatch.Index, PointedOriginalValue, Nothing)
                                        End Select

                                        If Not WorkingDirective Is Nothing Then
                                            If TypeOf WorkingDirective Is Controller.Directive.IParsingRequires Then _
                                                AddHandler CType(WorkingDirective, Controller.Directive.IParsingRequires).ParseRequested, AddressOf Me.OnParseRequest

                                            If TypeOf WorkingDirective Is Controller.Directive.IDeploymentAccessRequires Then _
                                                AddHandler CType(WorkingDirective, Controller.Directive.IDeploymentAccessRequires).DeploymentAccessRequested, AddressOf Me.OnDeploymentAccessRequest

                                            If TypeOf WorkingDirective Is Controller.Directive.IInstanceRequires Then _
                                                AddHandler CType(WorkingDirective, Controller.Directive.IInstanceRequires).InstanceRequested, AddressOf Me.OnInstanceRequest

                                            If TypeOf WorkingDirective Is Controller.Directive.Control.IControl Then _
                                                AddHandler CType(WorkingDirective, Controller.Directive.Control.IControl).ControlResolveRequested, AddressOf Me.OnControlResolveRequest

                                            ContainerController.Children.Add(WorkingDirective)
                                        End If

                                        LastIndex = (MainSearchMatchExam.Index + MainSearchMatchExam.Length)

                                        Exit Do
                                    End If
                                Loop
                            End If
                        Else
                            Select Case Controller.ControllerBase.CaptureControllerType(MainSearchMatch.Value)
                                Case Controller.ControllerBase.ControllerTypes.Property
                                    Dim PropertyDirective As Controller.PropertyController =
                                        New Controller.PropertyController(MainSearchMatch.Index, MainSearchMatch.Value, ContainerController.ContentArguments)
                                    AddHandler PropertyDirective.InstanceRequested, AddressOf Me.OnInstanceRequest

                                    ContainerController.Children.Add(PropertyDirective)

                                Case Controller.ControllerBase.ControllerTypes.Directive
                                    Dim WorkingDirective As Controller.DirectiveControllerBase = Nothing

                                    Select Case Controller.DirectiveControllerBase.CaptureDirectiveType(MainSearchMatch.Value)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Control
                                            WorkingDirective = Controller.Directive.ControlBase.MakeControl(MainSearchMatch.Index, MainSearchMatch.Value, Nothing, AddressOf Me.OnControlResolveRequest)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Template
                                            WorkingDirective = New Controller.Directive.Template(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Translation
                                            WorkingDirective = New Controller.Directive.Translation(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.HashCodePointedTemplate
                                            WorkingDirective = New Controller.Directive.HashCodePointedTemplate(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Execution
                                            WorkingDirective = New Controller.Directive.Execution(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.InLineStatement
                                            WorkingDirective = New Controller.Directive.InLineStatement(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.UpdateBlock
                                            WorkingDirective = New Controller.Directive.UpdateBlock(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.EncodedExecution
                                            WorkingDirective = New Controller.Directive.EncodedExecution(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.PartialCache
                                            WorkingDirective = New Controller.Directive.PartialCache(MainSearchMatch.Index, MainSearchMatch.Value, Nothing)
                                    End Select

                                    If Not WorkingDirective Is Nothing Then
                                        If TypeOf WorkingDirective Is Controller.Directive.IParsingRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IParsingRequires).ParseRequested, AddressOf Me.OnParseRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.IDeploymentAccessRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IDeploymentAccessRequires).DeploymentAccessRequested, AddressOf Me.OnDeploymentAccessRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.IInstanceRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IInstanceRequires).InstanceRequested, AddressOf Me.OnInstanceRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.Control.IControl Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.Control.IControl).ControlResolveRequested, AddressOf Me.OnControlResolveRequest

                                        ContainerController.Children.Add(WorkingDirective)
                                    End If

                            End Select

                            LastIndex = (MainSearchMatch.Index + MainSearchMatch.Value.Length)
                        End If
                    Loop

                    ContainerController.Children.Add(
                        New Controller.RenderlessController(LastIndex, DraftValue.Substring(LastIndex), ContainerController.ContentArguments)
                    )
                End If
            End Sub

            Private Sub OnDeploymentAccessRequest(ByRef WorkingInstance As [Shared].IDomain, ByRef DomainDeployment As Deployment.DomainDeployment)
                DomainDeployment = CType(WorkingInstance, Domain)._Deployment
            End Sub

            Private Shared _ControlsCache As New Concurrent.ConcurrentDictionary(Of String, Generic.Dictionary(Of String, Object))
            Private Sub OnControlResolveRequest(ByVal ControlID As String, ByRef WorkingInstance As [Shared].IDomain, ByRef ResultDictionary As Generic.Dictionary(Of String, Object))
                ResultDictionary = Nothing

                If String.IsNullOrEmpty(ControlID) Then Exit Sub

                If WorkingInstance Is Nothing Then _
                    WorkingInstance = Me._Instance

                Do
                    Dim CurrentDomainIDAccessTreeString As String =
                        String.Join(Of String)("-", WorkingInstance.IDAccessTree)
                    Dim CacheSearchKey As String =
                        String.Format("{0}_{1}", CurrentDomainIDAccessTreeString, ControlID)

                    If Renderer._ControlsCache.TryGetValue(CacheSearchKey, ResultDictionary) Then Exit Do

                    Dim ControlsXPathNavigator As Xml.XPath.XPathNavigator =
                        CType(WorkingInstance, Domain).ControlsXPathNavigator

                    If Not ControlsXPathNavigator Is Nothing Then
                        Dim XPathControlNav As Xml.XPath.XPathNavigator =
                            ControlsXPathNavigator.SelectSingleNode(String.Format("/Controls/Control[@id='{0}']", ControlID))

                        If Not XPathControlNav Is Nothing AndAlso
                            XPathControlNav.MoveToFirstChild() Then

                            ResultDictionary = New Generic.Dictionary(Of String, Object)

                            Dim CompareCulture As New Globalization.CultureInfo("en-US")

                            Do
                                Select Case XPathControlNav.Name.ToLower(CompareCulture)
                                    Case "type"
                                        ResultDictionary.Add("type", Controller.Directive.ControlBase.CaptureControlType(XPathControlNav.Value))

                                    Case "bind"
                                        ResultDictionary.Add("bind", [Shared].Execution.BindInfo.Make(XPathControlNav.Value))

                                    Case "attributes"
                                        Dim ChildReader As Xml.XPath.XPathNavigator =
                                        XPathControlNav.Clone()

                                        If ChildReader.MoveToFirstChild() Then
                                            Dim AttributesCol As New Controller.Directive.Control.AttributeInfo.AttributeInfoCollection()
                                            Do
                                                AttributesCol.Add(
                                                    ChildReader.GetAttribute("key", ChildReader.BaseURI).ToLower(),
                                                    ChildReader.Value
                                                )
                                            Loop While ChildReader.MoveToNext()
                                            ResultDictionary.Add("attributes", AttributesCol)
                                        End If

                                    Case "security"
                                        Dim ChildReader As Xml.XPath.XPathNavigator =
                                        XPathControlNav.Clone()

                                        If ChildReader.MoveToFirstChild() Then
                                            Dim SecurityInfo As New Controller.Directive.ControlBase.SecurityInfo()
                                            Do
                                                Select Case ChildReader.Name.ToLower(CompareCulture)
                                                    Case "registeredgroup"
                                                        SecurityInfo.RegisteredGroup = ChildReader.Value

                                                    Case "friendlyname"
                                                        SecurityInfo.FriendlyName = ChildReader.Value

                                                    Case "bind"
                                                        SecurityInfo.BindInfo = [Shared].Execution.BindInfo.Make(ChildReader.Value)

                                                    Case "disabled"
                                                        SecurityInfo.Disabled.IsSet = True
                                                        If Not [Enum].TryParse(Of Controller.Directive.ControlBase.SecurityInfo.DisabledClass.DisabledTypes)(
                                                            ChildReader.GetAttribute("type", ChildReader.NamespaceURI),
                                                            SecurityInfo.Disabled.Type
                                                        ) Then SecurityInfo.Disabled.Type = Controller.Directive.ControlBase.SecurityInfo.DisabledClass.DisabledTypes.Inherited
                                                        SecurityInfo.Disabled.Value = ChildReader.Value

                                                End Select
                                            Loop While ChildReader.MoveToNext()
                                            ResultDictionary.Add("security", SecurityInfo)
                                        End If

                                    Case "blockidstoupdate"
                                        Dim UpdateLocalBlock As Boolean
                                        If Not Boolean.TryParse(
                                                XPathControlNav.GetAttribute("localupdate", XPathControlNav.BaseURI),
                                                UpdateLocalBlock
                                            ) Then UpdateLocalBlock = True
                                        ResultDictionary.Add("blockidstoupdate.localupdate", UpdateLocalBlock)

                                        Dim ChildReader As Xml.XPath.XPathNavigator =
                                        XPathControlNav.Clone()

                                        If ChildReader.MoveToFirstChild() Then
                                            Dim BlockIDsToUpdate As New Generic.List(Of String)
                                            Do
                                                BlockIDsToUpdate.Add(ChildReader.Value)
                                            Loop While ChildReader.MoveToNext()
                                            ResultDictionary.Add("blockidstoupdate", BlockIDsToUpdate)
                                        End If

                                    Case "defaultbuttonid"
                                        ResultDictionary.Add("defaultbuttonid", XPathControlNav.Value)

                                    Case "text"
                                        ResultDictionary.Add("text", XPathControlNav.Value)

                                    Case "url"
                                        ResultDictionary.Add("url", XPathControlNav.Value)

                                    Case "content"
                                        ResultDictionary.Add("content", XPathControlNav.Value)

                                    Case "source"
                                        ResultDictionary.Add("source", XPathControlNav.Value)

                                End Select
                            Loop While XPathControlNav.MoveToNext()

                            Renderer._ControlsCache.TryAdd(CacheSearchKey, ResultDictionary)

                            Exit Do
                        Else
                            WorkingInstance = WorkingInstance.Parent
                        End If
                    End If
                Loop Until WorkingInstance Is Nothing
            End Sub

            Private Sub OnInstanceRequest(ByRef Instance As [Shared].IDomain)
                Instance = Me._Instance
            End Sub

            '            Private Function CheckSecurityInfo(ByVal CustomControl As [Global].CustomControl.ICustomControl, ByVal ArgumentInfos As [Global].ArgumentInfo.ArgumentInfoCollection) As PGlobals.MapControls.SecurityControlResult.Results
            '                Dim rSCR As PGlobals.MapControls.SecurityControlResult.Results =
            '                    PGlobals.MapControls.SecurityControlResult.Results.ReadWrite

            '                If Not CustomControl.SecurityInfo.SecuritySet Then _
            '                    Return rSCR

            '                If String.Compare(CustomControl.SecurityInfo.CallFunction, "#GLOBAL") = 0 Then _
            '                    Throw New Exception("Security Information Call Function must be set!")

            '                CustomControl.SecurityInfo.CallFunction =
            '                    CustomControl.SecurityInfo.CallFunction.Replace("[ControlID]", "#ControlID")
            '                ArgumentInfos.Add("ControlID", CustomControl.ControlID)

            '                ' Call Related Function and Exam It
            '                Dim tAssembleResultInfo As PGlobals.Execution.AssembleResultInfo
            '                If Me._ParentInstance Is Nothing Then
            'PARENTCALL:
            '                    tAssembleResultInfo =
            '                        [Assembly].AssemblePostBackInformation(
            '                            Control.SecurityInfo.CallFunction, ArgumentInfos, Assembly.ExecuterTypes.Control)
            '                Else
            '                    tAssembleResultInfo =
            '                        [Assembly].AssemblePostBackInformation(
            '                            Me._ParentInstance.CurrentID,
            '                            Me._CurrentInstance.CurrentID,
            '                            Control.SecurityInfo.CallFunction, ArgumentInfos, Assembly.ExecuterTypes.Control)

            '                    If tAssembleResultInfo.ReloadRequired Then GoTo PARENTCALL
            '                End If

            '                If tAssembleResultInfo.ReloadRequired Then
            '                    Throw New Exception("Security Information Call Function Error!", New System.IO.FileNotFoundException("Application cache has been corrupted, Reload Required!", tAssembleResultInfo.ApplicationPath))
            '                Else
            '                    If Not tAssembleResultInfo.MethodResult Is Nothing AndAlso
            '                        TypeOf tAssembleResultInfo.MethodResult Is Exception Then

            '                        Throw New Exception("Security Information Call Function Error!", Me.PrepareException("PlugIn Execution Error!", CType(tAssembleResultInfo.MethodResult, Exception).Message, CType(tAssembleResultInfo.MethodResult, Exception).InnerException))
            '                    Else
            '                        Dim sCR As PGlobals.MapControls.SecurityControlResult =
            '                            CType(tAssembleResultInfo.MethodResult, PGlobals.MapControls.SecurityControlResult)

            '                        rSCR = sCR.SecurityResult

            '                        If rSCR = PGlobals.MapControls.SecurityControlResult.Results.None Then _
            '                            rSCR = PGlobals.MapControls.SecurityControlResult.Results.ReadWrite
            '                    End If
            '                End If
            '                ' ----

            '                Return rSCR
            '            End Function
#End Region
        End Class

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