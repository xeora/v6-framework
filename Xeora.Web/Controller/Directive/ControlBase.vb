Option Strict On

Imports System.Xml.XPath
Imports Xeora.Web.Controller.Directive.Control
Imports Xeora.Web.Global

Namespace Xeora.Web.Controller.Directive
    Public MustInherit Class ControlBase
        Inherits DirectiveControllerBase
        Implements ILevelable
        Implements IBoundable
        Implements IParsingRequires
        Implements IControl

        Private _Leveling As Integer
        Private _BoundControlID As String

        Private _ControlID As String
        Private _SecurityInfo As SecurityInfos
        Private _ControlType As ControlTypes
        Private _BindInfo As [Shared].Execution.BindInfo
        Private _Attributes As AttributeInfo.AttributeInfoCollection
        Private _BlockIDsToUpdate As Generic.List(Of String)
        Private _UpdateLocalBlock As Boolean

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Protected Sub RequestParse(ByVal DraftValue As String, ByRef ContainerController As ControllerBase)
            RaiseEvent ParseRequested(DraftValue, ContainerController)
        End Sub

        Public Event ControlMapNavigatorRequested(ByRef WorkingInstance As [Shared].IDomain, ByRef ControlMapXPathNavigator As XPathNavigator) Implements IControl.ControlMapNavigatorRequested
        Protected Sub RequestControlMapNavigator(ByRef WorkingInstance As [Shared].IDomain, ByRef ControlMapXPathNavigator As XPathNavigator)
            RaiseEvent ControlMapNavigatorRequested(WorkingInstance, ControlMapXPathNavigator)
        End Sub

        Public Enum ControlTypes
            Textbox
            Password
            Checkbox
            Button
            RadioButton
            Textarea
            ImageButton
            LinkButton

            DataList
            ConditionalStatement
            VariableBlock

            Unknown
        End Enum

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ControlType As ControlTypes, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.Control, ContentArguments)

            Me._ControlID = Me.CaptureControlID()
            Me._Leveling = Me.CaptureLeveling()
            Me._BoundControlID = Me.CaptureBoundControlID()
            Me._SecurityInfo = New SecurityInfos()
            Me._ControlType = Me.CaptureControlType()
            Me._BindInfo = Nothing
            Me._Attributes = New AttributeInfo.AttributeInfoCollection
            Me._BlockIDsToUpdate = New Generic.List(Of String)
            Me._UpdateLocalBlock = True
        End Sub

        Public ReadOnly Property Level As Integer Implements ILevelable.Level
            Get
                Return Me._Leveling
            End Get
        End Property

        Public ReadOnly Property BoundControlID As String Implements IBoundable.BoundControlID
            Get
                Return Me._BoundControlID
            End Get
        End Property

        Public ReadOnly Property ControlID() As String Implements IControl.ControlID
            Get
                Return Me._ControlID
            End Get
        End Property

        Public Property SecurityInfo() As SecurityInfos Implements IControl.SecurityInfo
            Get
                Return Me._SecurityInfo
            End Get
            Set(ByVal value As SecurityInfos)
                Me._SecurityInfo = value

                If Me._SecurityInfo Is Nothing Then Me._SecurityInfo = New SecurityInfos
            End Set
        End Property

        Public ReadOnly Property Type() As ControlBase.ControlTypes Implements IControl.Type
            Get
                Return Me._ControlType
            End Get
        End Property

        Public Property BindInfo() As [Shared].Execution.BindInfo Implements IControl.BindInfo
            Get
                Return Me._BindInfo
            End Get
            Set(ByVal Value As [Shared].Execution.BindInfo)
                Me._BindInfo = Value
            End Set
        End Property

        Public ReadOnly Property Attributes() As AttributeInfo.AttributeInfoCollection Implements IControl.Attributes
            Get
                Return Me._Attributes
            End Get
        End Property

        Public Property UpdateLocalBlock() As Boolean Implements IControl.UpdateLocalBlock
            Get
                Return Me._UpdateLocalBlock
            End Get
            Set(ByVal Value As Boolean)
                Me._UpdateLocalBlock = Value
            End Set
        End Property

        Public ReadOnly Property BlockIDsToUpdate() As Generic.List(Of String) Implements IControl.BlockIDsToUpdate
            Get
                Return Me._BlockIDsToUpdate
            End Get
        End Property

        Public Overridable Sub Clone(ByRef Control As IControl) Implements IControl.Clone
            If Control Is Nothing Then Exit Sub

            With CType(Control, ControlBase)
                ._SecurityInfo = Me._SecurityInfo
                ._Attributes.AddRange(Me._Attributes.ToArray())
                ._BlockIDsToUpdate.AddRange(Me._BlockIDsToUpdate.ToArray())
                ._UpdateLocalBlock = Me._UpdateLocalBlock
                ._BindInfo = Me._BindInfo
            End With
        End Sub

        Public Shared Function CaptureControlType(ByVal ControlTypeName As String) As ControlTypes
            Dim rControlType As ControlTypes

            If Not [Enum].TryParse(ControlTypeName, True, rControlType) Then _
                rControlType = ControlTypes.Unknown

            Return rControlType
        End Function

        Public Shared Function MakeControl(ByVal DraftIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection, ByVal ControlMapNavigatorRequested As IControl.ControlMapNavigatorRequestedEventHandler) As ControlBase
            Dim rControl As ControlBase = Nothing

            ' Dummy Control just to check the Control Type
            Dim Control As New Unknown(DraftIndex, DraftValue, ContentArguments)
            AddHandler Control.ControlMapNavigatorRequested, ControlMapNavigatorRequested
            Control.SyncronizeWithDefinition()

            Select Case Control.Type
                Case ControlTypes.Button
                    rControl = New Button(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.Checkbox
                    rControl = New Checkbox(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.ConditionalStatement
                    rControl = New ConditionalStatement(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.DataList
                    rControl = New DataList(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.ImageButton
                    rControl = New ImageButton(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.LinkButton
                    rControl = New LinkButton(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.Password
                    rControl = New Password(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.RadioButton
                    rControl = New RadioButton(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.Textarea
                    rControl = New Textarea(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.Textbox
                    rControl = New Textbox(DraftIndex, DraftValue, ContentArguments)
                Case ControlTypes.VariableBlock
                    rControl = New VariableBlock(DraftIndex, DraftValue, ContentArguments)
            End Select
            If Not rControl Is Nothing Then
                AddHandler rControl.ControlMapNavigatorRequested, ControlMapNavigatorRequested
                rControl.SyncronizeWithDefinition()
            End If

            Return rControl
        End Function

        Private Function CaptureControlType() As ControlTypes
            Dim rControlType As ControlTypes = ControlTypes.Unknown

            Dim WorkingInstance As [Shared].IDomain = Nothing
            Dim XPathNavigator As XPathNavigator = Nothing

            Do
                RaiseEvent ControlMapNavigatorRequested(WorkingInstance, XPathNavigator)

                If Not XPathNavigator Is Nothing Then
                    Dim XPathControlNav As XPathNavigator

                    XPathControlNav = XPathNavigator.SelectSingleNode(String.Format("//Control[@id='{0}']", Me.ControlID))

                    If Not XPathControlNav Is Nothing AndAlso
                        XPathControlNav.MoveToFirstChild() Then

                        Dim CompareCulture As New Globalization.CultureInfo("en-US")

                        Do
                            Select Case XPathControlNav.Name.ToLower(CompareCulture)
                                Case "type"
                                    Me._ControlType = ControlBase.CaptureControlType(XPathControlNav.Value)

                                    Exit Do
                            End Select
                        Loop While XPathControlNav.MoveToNext()
                    Else
                        WorkingInstance = WorkingInstance.Parent
                        XPathNavigator = Nothing
                    End If
                End If
            Loop Until WorkingInstance Is Nothing

            Return rControlType
        End Function

        Protected Sub SyncronizeWithDefinition()
            Dim WorkingInstance As [Shared].IDomain = Nothing
            Dim XPathNavigator As XPathNavigator = Nothing

            Do
                RaiseEvent ControlMapNavigatorRequested(WorkingInstance, XPathNavigator)

                If Not XPathNavigator Is Nothing Then
                    Dim XPathControlNav As XPathNavigator

                    XPathControlNav = XPathNavigator.SelectSingleNode(String.Format("//Control[@id='{0}']", Me.ControlID))

                    If Not XPathControlNav Is Nothing AndAlso
                        XPathControlNav.MoveToFirstChild() Then

                        Dim CompareCulture As New Globalization.CultureInfo("en-US")

                        Do
                            Select Case XPathControlNav.Name.ToLower(CompareCulture)
                                Case "type"
                                    Me._ControlType = ControlBase.CaptureControlType(XPathControlNav.Value)

                                Case "bind"
                                    Me._BindInfo = [Shared].Execution.BindInfo.Make(XPathControlNav.Value)

                                Case "attributes"
                                    Dim ChildReader As XPathNavigator =
                                        XPathControlNav.Clone()

                                    If ChildReader.MoveToFirstChild() Then
                                        Do
                                            Me._Attributes.Add(
                                                ChildReader.GetAttribute("key", ChildReader.BaseURI).ToLower(),
                                                ChildReader.Value
                                            )
                                        Loop While ChildReader.MoveToNext()
                                    End If

                                Case "security"
                                    Dim ChildReader As XPathNavigator =
                                        XPathControlNav.Clone()

                                    If ChildReader.MoveToFirstChild() Then
                                        Dim RegisteredGroup As String = String.Empty, FriendlyName As String = String.Empty,
                                            SecurityBind As [Shared].Execution.BindInfo = Nothing, DisabledValue As String = String.Empty,
                                            DisabledType As String = "Inherited", DisabledSet As Boolean = False

                                        Do
                                            Select Case ChildReader.Name.ToLower(CompareCulture)
                                                Case "registeredgroup"
                                                    Me._SecurityInfo.RegisteredGroup = ChildReader.Value

                                                Case "friendlyname"
                                                    Me._SecurityInfo.FriendlyName = ChildReader.Value

                                                Case "bind"
                                                    Me._SecurityInfo.BindInfo = [Shared].Execution.BindInfo.Make(ChildReader.Value)

                                                Case "disabled"
                                                    Me._SecurityInfo.Disabled.IsSet = True
                                                    If Not [Enum].TryParse(Of SecurityInfos.DisabledClass.DisabledTypes)(
                                                            ChildReader.GetAttribute("type", ChildReader.NamespaceURI),
                                                            Me._SecurityInfo.Disabled.Type
                                                        ) Then Me._SecurityInfo.Disabled.Type = SecurityInfos.DisabledClass.DisabledTypes.Inherited
                                                    Me._SecurityInfo.Disabled.Value = ChildReader.Value

                                            End Select
                                        Loop While ChildReader.MoveToNext()
                                    End If

                                Case "blockidstoupdate"
                                    If Not Boolean.TryParse(
                                                XPathControlNav.GetAttribute("localupdate", XPathControlNav.BaseURI),
                                                Me._UpdateLocalBlock
                                            ) Then Me._UpdateLocalBlock = True

                                    Dim ChildReader As XPathNavigator =
                                        XPathControlNav.Clone()

                                    If ChildReader.MoveToFirstChild() Then
                                        Do
                                            Me._BlockIDsToUpdate.Add(ChildReader.Value)
                                        Loop While ChildReader.MoveToNext()
                                    End If

                                Case "defaultbuttonid"
                                    If TypeOf Me Is IHasDefaultButton Then _
                                        CType(Me, IHasDefaultButton).DefaultButtonID = XPathControlNav.Value

                                Case "text"
                                    If TypeOf Me Is IHasText Then _
                                        CType(Me, IHasText).Text = XPathControlNav.Value

                                Case "url"
                                    If TypeOf Me Is IHasURL Then _
                                        CType(Me, IHasURL).URL = XPathControlNav.Value

                                Case "content"
                                    If TypeOf Me Is IHasContent Then _
                                        CType(Me, IHasContent).Content = XPathControlNav.Value

                                Case "source"
                                    If TypeOf Me Is IHasSource Then _
                                        CType(Me, IHasSource).Source = XPathControlNav.Value

                            End Select
                        Loop While XPathControlNav.MoveToNext()

                        Exit Do
                    Else
                        WorkingInstance = WorkingInstance.Parent
                        XPathNavigator = Nothing
                    End If
                End If
            Loop Until WorkingInstance Is Nothing
        End Sub

        Protected Sub RenderBindInfoParams()
            If Not Me.BindInfo Is Nothing AndAlso
                Not Me.BindInfo.ProcedureParams Is Nothing Then

                Dim DummyControllerContainer As ControllerBase
                For pC As Integer = 0 To Me.BindInfo.ProcedureParams.Length - 1
                    DummyControllerContainer = ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
                    Me.RequestParse(Me.BindInfo.ProcedureParams(pC), DummyControllerContainer)
                    DummyControllerContainer.Render(Me)
                    Me.BindInfo.ProcedureParams(pC) = DummyControllerContainer.RenderedValue
                Next
            End If
        End Sub

        Protected Function SplitContentByControlIDWithIndex(ByVal Content As String, ByVal ControlIDWithIndex As String) As String()
            Dim rResultList As New Generic.List(Of String)

            Dim SearchString As String = String.Format("}}:{0}:{{", ControlIDWithIndex)
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

            Return rResultList.ToArray()
        End Function

        Public Class SecurityInfos
            Private _SecuritySet As Boolean
            Private _RegisteredGroup As String
            Private _FriendlyName As String
            Private _BindInfo As [Shared].Execution.BindInfo
            Private _Disabled As DisabledClass

            Public Sub New()
                Me._SecuritySet = False
                Me._RegisteredGroup = String.Empty
                Me._FriendlyName = String.Empty
                Me._BindInfo = Nothing
                Me._Disabled = New DisabledClass
            End Sub

            Public Property SecuritySet() As Boolean
                Get
                    Return Me._SecuritySet
                End Get
                Set(ByVal value As Boolean)
                    Me._SecuritySet = value

                    If Me._SecuritySet Then
                        If String.IsNullOrEmpty(Me._FriendlyName) Then Me._FriendlyName = "Unknown"
                    End If
                End Set
            End Property

            Public Property RegisteredGroup() As String
                Get
                    Return Me._RegisteredGroup
                End Get
                Set(ByVal value As String)
                    Me._RegisteredGroup = value
                End Set
            End Property

            Public Property FriendlyName() As String
                Get
                    Return Me._FriendlyName
                End Get
                Set(ByVal value As String)
                    Me._FriendlyName = value

                    If String.IsNullOrEmpty(Me._FriendlyName) Then
                        Me._FriendlyName = "Unknown"
                    End If

                    Me._SecuritySet = True
                End Set
            End Property

            Public Property BindInfo() As [Shared].Execution.BindInfo
                Get
                    Return Me._BindInfo
                End Get
                Set(ByVal value As [Shared].Execution.BindInfo)
                    Me._BindInfo = value
                End Set
            End Property

            Public ReadOnly Property Disabled() As DisabledClass
                Get
                    Return Me._Disabled
                End Get
            End Property

            Public Class DisabledClass
                Private _DisabledSet As Boolean
                Private _DisabledType As DisabledTypes
                Private _DisabledValue As String

                Public Enum DisabledTypes
                    Inherited
                    Dynamic
                End Enum

                Public Sub New()
                    Me._DisabledSet = False
                    Me._DisabledType = DisabledTypes.Inherited
                    Me._DisabledValue = String.Empty
                End Sub

                Public Property IsSet() As Boolean
                    Get
                        Return Me._DisabledSet
                    End Get
                    Set(ByVal value As Boolean)
                        Me._DisabledSet = value
                    End Set
                End Property

                Public Property Type() As DisabledTypes
                    Get
                        Return Me._DisabledType
                    End Get
                    Set(ByVal value As DisabledTypes)
                        Me._DisabledType = value
                    End Set
                End Property

                Public Property Value() As String
                    Get
                        Return Me._DisabledValue
                    End Get
                    Set(ByVal value As String)
                        Me._DisabledValue = value
                    End Set
                End Property
            End Class
        End Class
    End Class
End Namespace
