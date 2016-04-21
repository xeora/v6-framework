Option Strict On

Namespace Xeora.Web.Site
    Public Class Domain
        Implements [Shared].IDomain

        Private _Parent As [Shared].IDomain = Nothing
        Private _Renderer As Renderer

        Private _Deployment As Deployment.DomainDeployment
        Private _ControlMapXPathNavigator As Xml.XPath.XPathNavigator

#Region " Constructors "

        Public Sub New(ByVal DomainIDAccessTree As String())
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal DomainIDAccessTree As String(), ByVal LanguageID As String)
            If DomainIDAccessTree Is Nothing Then _
                DomainIDAccessTree = New String() {[Shared].Configurations.DefaultDomain}

            Me._Deployment = New Deployment.DomainDeployment(DomainIDAccessTree, LanguageID)
            AddHandler Me._Deployment.Language.ResolveTranslationRequested, AddressOf Me.ResolveTranslationRequest

            If DomainIDAccessTree.Length > 1 Then
                Dim DomainIDAccessTreeA As New Generic.List(Of String)
                DomainIDAccessTreeA.AddRange(DomainIDAccessTree)
                DomainIDAccessTreeA.RemoveAt(DomainIDAccessTreeA.Count - 1)

                Dim WorkingInstance As Domain = Me
                Do Until DomainIDAccessTreeA.Count = 0
                    Dim ParentInstance As Domain =
                        New Domain(DomainIDAccessTreeA.ToArray(), Me._Deployment.LanguageID)

                    WorkingInstance._Parent = ParentInstance
                    WorkingInstance = ParentInstance

                    DomainIDAccessTreeA.RemoveAt(DomainIDAccessTreeA.Count - 1)
                Loop
            End If

            Me._Renderer = New Renderer(Me)
        End Sub

        Private Sub ResolveTranslationRequest(ByVal TranslationID As String, ByRef Value As String)
            Dim WorkingInstance As [Shared].IDomain = Me._Parent

            Do Until WorkingInstance Is Nothing OrElse Not String.IsNullOrEmpty(Value)
                Value = WorkingInstance.Language.Get(TranslationID)

                WorkingInstance = WorkingInstance.Parent
            Loop
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

        Public ReadOnly Property Settings() As [Shared].IDomain.ISettings Implements [Shared].IDomain.Settings
            Get
                Return Me._Deployment.Settings
            End Get
        End Property

        Public ReadOnly Property Language() As [Shared].IDomain.ILanguage Implements [Shared].IDomain.Language
            Get
                Return Me._Deployment.Language
            End Get
        End Property

        Public ReadOnly Property WebService() As [Shared].IDomain.IWebService Implements [Shared].IDomain.WebService
            Get
                Return Me._Deployment.WebService
            End Get
        End Property

        Public ReadOnly Property Children() As [Shared].DomainInfo.DomainInfoCollection Implements [Shared].IDomain.Children
            Get
                Return Me._Deployment.Children
            End Get
        End Property

        Public Function CheckTemplateExists(ByVal TemplateID As String) As Boolean
            Return Me._Deployment.CheckTemplateExists(TemplateID)
        End Function

        Public Sub ProvideFileStream(ByRef FileStream As IO.Stream, ByVal RequestedFilePath As String)
            Me._Deployment.ProvideFileStream(FileStream, RequestedFilePath)
        End Sub

        Public Sub ClearDomainCache()
            Me._Deployment.ClearDeploymentCache()
        End Sub

        Public Function Render(ByVal TemplateID As String, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String Implements [Shared].IDomain.Render
            Return Me._Renderer.Start(TemplateID, MessageResult, UpdateBlockControlID)
        End Function

        Private _xPathStream As IO.StringReader = Nothing
        Private ReadOnly Property ControlMapXPathNavigator() As Xml.XPath.XPathNavigator
            Get
                If Me._ControlMapXPathNavigator Is Nothing Then
                    Dim xPathDoc As Xml.XPath.XPathDocument = Nothing
                    Dim ControlMapContent As String =
                        Me._Deployment.ProvideControlMapContent()

                    If Not Me._xPathStream Is Nothing Then Me._xPathStream.Close() : GC.SuppressFinalize(Me._xPathStream)

                    Me._xPathStream = New IO.StringReader(ControlMapContent)
                    xPathDoc = New Xml.XPath.XPathDocument(Me._xPathStream)

                    Me._ControlMapXPathNavigator = xPathDoc.CreateNavigator()
                End If

                Return Me._ControlMapXPathNavigator
            End Get
        End Property

        Private Class Renderer
            Private _Instance As [Shared].IDomain

            Public Sub New(ByVal Instance As [Shared].IDomain)
                Me._Instance = Instance
            End Sub

#Region " Template Parsing Procedures "
            Public Function Start(ByVal TemplateID As String, ByVal MessageResult As [Shared].ControlResult.Message, Optional ByVal UpdateBlockControlID As String = Nothing) As String
                Dim TemplateDirective As Controller.Directive.Template =
                    New Controller.Directive.Template(0, String.Format("$T:{0}$", TemplateID), Nothing)
                TemplateDirective.UpdateBlockControlID = UpdateBlockControlID
                TemplateDirective.MessageResult = MessageResult
                AddHandler TemplateDirective.ParseRequested, AddressOf Me.OnParseRequest
                AddHandler TemplateDirective.DeploymentAccessRequested, AddressOf Me.OnDeploymentAccessRequest
                AddHandler TemplateDirective.InstanceRequested, AddressOf Me.OnInstanceRequest

                TemplateDirective.Render(Nothing)

                Return TemplateDirective.RenderedValue
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

                    Dim lMatchItem01 As Text.RegularExpressions.Match, lMatchItem02 As Text.RegularExpressions.Match
                    Dim tMatchItem01 As Text.RegularExpressions.Match, MatchDirectiveType01 As String = Nothing, MatchedID01 As String = Nothing ' For Opening Brackets
                    Dim tMatchItem02 As Text.RegularExpressions.Match ' For Separator Brackets
                    Dim tMatchItem03 As Text.RegularExpressions.Match ' For Closing Brackets

                    Dim REMEnum As IEnumerator =
                        MainPatternMatches.GetEnumerator()

                    Dim BracketedOpenPattern As New RegularExpressions.BracketedControllerOpenPattern()
                    Dim BracketedSeparatorPattern As New RegularExpressions.BracketedControllerSeparatorPattern()
                    Dim BracketedClosePattern As New RegularExpressions.BracketedControllerClosePattern()

                    Do While REMEnum.MoveNext()
                        lMatchItem01 = CType(REMEnum.Current, Text.RegularExpressions.Match)

                        ' Check till this match any renderless content exists
                        If lMatchItem01.Index > LastIndex Then
                            ContainerController.Children.Add(
                                New Controller.RenderlessController(LastIndex, DraftValue.Substring(LastIndex, lMatchItem01.Index - LastIndex), ContainerController.ContentArguments)
                            )
                            LastIndex = lMatchItem01.Index
                        End If

                        ' Exam For Bracketed Regex Result
                        tMatchItem01 = BracketedOpenPattern.Match(lMatchItem01.Value)

                        If tMatchItem01.Success Then
                            MatchDirectiveType01 = tMatchItem01.Result("${DirectiveType}")
                            MatchedID01 = tMatchItem01.Result("${ItemID}")

                            If Not MatchedID01 Is Nothing Then
                                Dim InnerMatch As Integer = 0
                                Dim SeparatorIndexes As New Generic.List(Of Integer)

                                Do While REMEnum.MoveNext()
                                    lMatchItem02 = CType(REMEnum.Current, Text.RegularExpressions.Match)

                                    ' Exam For Opening Bracketed Regex Result
                                    tMatchItem01 = BracketedOpenPattern.Match(lMatchItem02.Value)

                                    ' Exam For Separator Bracketed Regex Result
                                    tMatchItem02 = BracketedSeparatorPattern.Match(lMatchItem02.Value)

                                    ' Exam For Closing Bracketed Regex Result
                                    tMatchItem03 = BracketedClosePattern.Match(lMatchItem02.Value)

                                    If tMatchItem01.Success AndAlso
                                        String.Compare(MatchedID01, tMatchItem01.Result("${ItemID}")) = 0 Then ' Check is Another Same Named Control Internally Opened Bracket

                                        InnerMatch += 1
                                    ElseIf tMatchItem02.Success AndAlso
                                        String.Compare(MatchedID01, tMatchItem02.Result("${ItemID}")) = 0 AndAlso
                                        InnerMatch = 0 Then ' Check is Same Named Highlevel Control Separator Bracket

                                        ' Point the location of Separator Bracket index
                                        SeparatorIndexes.Add(lMatchItem02.Index - lMatchItem01.Index)
                                    ElseIf tMatchItem03.Success AndAlso
                                        String.Compare(MatchedID01, tMatchItem03.Result("${ItemID}")) = 0 Then ' Check is Same Named Control Internally Closed Bracket

                                        If InnerMatch = 0 Then
                                            Dim ModifierText As String = String.Format("~{0}", lMatchItem01.Index)
                                            Dim PointedOriginalValue As String =
                                                DraftValue.Substring(
                                                    lMatchItem01.Index,
                                                    (lMatchItem02.Index + lMatchItem02.Length) - lMatchItem01.Index
                                                )

                                            PointedOriginalValue = PointedOriginalValue.Insert(PointedOriginalValue.Length - 1, ModifierText)
                                            For idxID As Integer = SeparatorIndexes.Count - 1 To 0 Step -1
                                                PointedOriginalValue = PointedOriginalValue.Insert(
                                                                            (SeparatorIndexes(idxID) + String.Format("}}:{0}", MatchedID01).Length),
                                                                            ModifierText
                                                                        )
                                            Next
                                            PointedOriginalValue = PointedOriginalValue.Insert(lMatchItem01.Length - 2, ModifierText)

                                            Dim WorkingDirective As Controller.DirectiveControllerBase = Nothing

                                            Select Case Controller.DirectiveControllerBase.CaptureDirectiveType(String.Format("${0}:", IIf(String.IsNullOrEmpty(MatchDirectiveType01), MatchedID01, MatchDirectiveType01)))
                                                Case Controller.DirectiveControllerBase.DirectiveTypes.Control
                                                    WorkingDirective = Controller.Directive.ControlBase.MakeControl(lMatchItem01.Index, PointedOriginalValue, ContainerController.ContentArguments, AddressOf Me.OnControlMapNavigatorRequest)
                                                Case Controller.DirectiveControllerBase.DirectiveTypes.InLineStatement
                                                    WorkingDirective = New Controller.Directive.InLineStatement(lMatchItem01.Index, PointedOriginalValue, ContainerController.ContentArguments)
                                                Case Controller.DirectiveControllerBase.DirectiveTypes.UpdateBlock
                                                    WorkingDirective = New Controller.Directive.UpdateBlock(lMatchItem01.Index, PointedOriginalValue, ContainerController.ContentArguments)
                                                Case Controller.DirectiveControllerBase.DirectiveTypes.EncodedExecution
                                                    WorkingDirective = New Controller.Directive.EncodedExecution(lMatchItem01.Index, PointedOriginalValue, ContainerController.ContentArguments)
                                                Case Controller.DirectiveControllerBase.DirectiveTypes.MessageBlock
                                                    WorkingDirective = New Controller.Directive.MessageBlock(lMatchItem01.Index, PointedOriginalValue, ContainerController.ContentArguments)
                                            End Select

                                            If Not WorkingDirective Is Nothing Then
                                                If TypeOf WorkingDirective Is Controller.Directive.IParsingRequires Then _
                                                    AddHandler CType(WorkingDirective, Controller.Directive.IParsingRequires).ParseRequested, AddressOf Me.OnParseRequest

                                                If TypeOf WorkingDirective Is Controller.Directive.IDeploymentAccessRequires Then _
                                                    AddHandler CType(WorkingDirective, Controller.Directive.IDeploymentAccessRequires).DeploymentAccessRequested, AddressOf Me.OnDeploymentAccessRequest

                                                If TypeOf WorkingDirective Is Controller.Directive.IInstanceRequires Then _
                                                    AddHandler CType(WorkingDirective, Controller.Directive.IInstanceRequires).InstanceRequested, AddressOf Me.OnInstanceRequest

                                                If TypeOf WorkingDirective Is Controller.Directive.Control.IControl Then _
                                                    AddHandler CType(WorkingDirective, Controller.Directive.Control.IControl).ControlMapNavigatorRequested, AddressOf Me.OnControlMapNavigatorRequest

                                                ContainerController.Children.Add(WorkingDirective)
                                            End If

                                            LastIndex = (lMatchItem02.Index + lMatchItem02.Length)

                                            Exit Do
                                        Else
                                            InnerMatch -= 1
                                        End If
                                    End If
                                Loop
                            End If
                        Else
                            Select Case Controller.ControllerBase.CaptureControllerType(lMatchItem01.Value)
                                Case Controller.ControllerBase.ControllerTypes.Property
                                    Dim PropertyDirective As Controller.PropertyController =
                                        New Controller.PropertyController(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                    AddHandler PropertyDirective.InstanceRequested, AddressOf Me.OnInstanceRequest

                                    ContainerController.Children.Add(PropertyDirective)
                                Case Controller.ControllerBase.ControllerTypes.Directive
                                    Dim WorkingDirective As Controller.DirectiveControllerBase = Nothing

                                    Select Case Controller.DirectiveControllerBase.CaptureDirectiveType(lMatchItem01.Value)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Control
                                            WorkingDirective = Controller.Directive.ControlBase.MakeControl(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments, AddressOf Me.OnControlMapNavigatorRequest)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Template
                                            WorkingDirective = New Controller.Directive.Template(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Translation
                                            WorkingDirective = New Controller.Directive.Translation(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.HashCodePointedTemplate
                                            WorkingDirective = New Controller.Directive.HashCodePointedTemplate(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.Execution
                                            WorkingDirective = New Controller.Directive.Execution(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.InLineStatement
                                            WorkingDirective = New Controller.Directive.InLineStatement(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.UpdateBlock
                                            WorkingDirective = New Controller.Directive.UpdateBlock(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                        Case Controller.DirectiveControllerBase.DirectiveTypes.EncodedExecution
                                            WorkingDirective = New Controller.Directive.EncodedExecution(lMatchItem01.Index, lMatchItem01.Value, ContainerController.ContentArguments)
                                    End Select

                                    If Not WorkingDirective Is Nothing Then
                                        If TypeOf WorkingDirective Is Controller.Directive.IParsingRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IParsingRequires).ParseRequested, AddressOf Me.OnParseRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.IDeploymentAccessRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IDeploymentAccessRequires).DeploymentAccessRequested, AddressOf Me.OnDeploymentAccessRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.IInstanceRequires Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.IInstanceRequires).InstanceRequested, AddressOf Me.OnInstanceRequest

                                        If TypeOf WorkingDirective Is Controller.Directive.Control.IControl Then _
                                            AddHandler CType(WorkingDirective, Controller.Directive.Control.IControl).ControlMapNavigatorRequested, AddressOf Me.OnControlMapNavigatorRequest

                                        ContainerController.Children.Add(WorkingDirective)
                                    End If
                            End Select

                            LastIndex = (lMatchItem01.Index + lMatchItem01.Value.Length)
                        End If
                    Loop

                    ContainerController.Children.Add(
                        New Controller.RenderlessController(LastIndex, DraftValue.Substring(LastIndex), ContainerController.ContentArguments)
                    )
                End If
            End Sub

            Private Sub OnDeploymentAccessRequest(ByRef DomainDeployment As Deployment.DomainDeployment)
                DomainDeployment = CType(Me._Instance, Domain)._Deployment
            End Sub

            Private Sub OnControlMapNavigatorRequest(ByRef WorkingInstance As [Shared].IDomain, ByRef ControlMapXPathNavigator As Xml.XPath.XPathNavigator)
                If WorkingInstance Is Nothing Then _
                    WorkingInstance = Me._Instance
                ControlMapXPathNavigator = CType(WorkingInstance, Domain).ControlMapXPathNavigator
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

        Public Class DomainCollection
            Inherits Generic.List(Of Domain)

            Private _ParentHolder As Domain

            Public Sub New(ByVal Parent As Domain)
                Me._ParentHolder = Parent
            End Sub

            Public Shadows Sub Add(ByVal Item As Domain)
                Item._Parent = Me._ParentHolder

                MyBase.Add(Item)
            End Sub

            Public Shadows Sub AddRange(ByVal Collection As DomainCollection)
                For Each Item As Domain In Collection
                    Item._Parent = Me._ParentHolder
                Next

                MyBase.AddRange(Collection)
            End Sub
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