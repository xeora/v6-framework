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

        Private _IsBuilt As Boolean

        Private _Leveling As Integer
        Private _LevelingExecutionOnly As Boolean
        Private _BoundControlID As String

        Private _ControlID As String
        Private _Security As SecurityInfo
        Private _ControlType As ControlTypes
        Private _BindInfo As [Shared].Execution.BindInfo
        Private _Attributes As AttributeInfo.AttributeInfoCollection

        Public Event ParseRequested(DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested
        Protected Sub RequestParse(ByVal DraftValue As String, ByRef ContainerController As ControllerBase)
            RaiseEvent ParseRequested(DraftValue, ContainerController)
        End Sub

        Public Event ControlResolveRequested(ByVal ControlID As String, ByRef WorkingInstance As [Shared].IDomain, ByRef ResultDictionary As Generic.Dictionary(Of String, Object)) Implements IControl.ControlResolveRequested
        Protected Sub RequestControlResolve(ByVal ControlID As String, ByRef WorkingInstance As [Shared].IDomain, ByRef ResultDictionary As Generic.Dictionary(Of String, Object))
            RaiseEvent ControlResolveRequested(ControlID, WorkingInstance, ResultDictionary)
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

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.Control, ContentArguments)

            Me._IsBuilt = False
        End Sub

        Public Sub Build()
            If Me._IsBuilt Then Exit Sub

            Me._IsBuilt = True
            Me._ControlID = Me.CaptureControlID()
            Me._Leveling = Me.CaptureLeveling(Me._LevelingExecutionOnly)
            Me._BoundControlID = Me.CaptureBoundControlID()
            Me._Security = New SecurityInfo()
            Me._ControlType = ControlTypes.Unknown
            Me._BindInfo = Nothing
            Me._Attributes = New AttributeInfo.AttributeInfoCollection

            Dim WorkingInstance As [Shared].IDomain = Nothing
            Dim XPathNavigator As XPathNavigator = Nothing

            Do
                Dim ResultDictionary As Generic.Dictionary(Of String, Object) = Nothing

                RaiseEvent ControlResolveRequested(Me._ControlID, WorkingInstance, ResultDictionary)

                If Not ResultDictionary Is Nothing Then
                    For Each Key As String In ResultDictionary.Keys
                        Select Case Key
                            Case "type"
                                Me._ControlType = CType(ResultDictionary.Item(Key), ControlTypes)

                            Case "bind"
                                If Not ResultDictionary.Item(Key) Is Nothing Then _
                                    CType(ResultDictionary.Item(Key), [Shared].Execution.BindInfo).Clone(Me._BindInfo)

                            Case "attributes"
                                If Not ResultDictionary.Item(Key) Is Nothing Then
                                    Me._Attributes.AddRange(
                                        CType(ResultDictionary.Item(Key), AttributeInfo.AttributeInfoCollection).ToArray())
                                End If

                            Case "security"
                                If Not ResultDictionary.Item(Key) Is Nothing Then _
                                    CType(ResultDictionary.Item(Key), SecurityInfo).Clone(Me._Security)

                            Case "blockidstoupdate.localupdate"
                                If TypeOf Me Is IUpdateBlocks AndAlso Not ResultDictionary.Item(Key) Is Nothing Then _
                                    CType(Me, IUpdateBlocks).UpdateLocalBlock = CType(ResultDictionary.Item(Key), Boolean)

                            Case "blockidstoupdate"
                                If TypeOf Me Is IUpdateBlocks AndAlso Not ResultDictionary.Item(Key) Is Nothing Then _
                                    CType(Me, IUpdateBlocks).BlockIDsToUpdate.AddRange(CType(ResultDictionary.Item(Key), Generic.List(Of String)))

                            Case "defaultbuttonid"
                                If TypeOf Me Is IHasDefaultButton Then _
                                    CType(Me, IHasDefaultButton).DefaultButtonID = CType(ResultDictionary.Item(Key), String)

                            Case "text"
                                If TypeOf Me Is IHasText Then _
                                        CType(Me, IHasText).Text = CType(ResultDictionary.Item(Key), String)

                            Case "url"
                                If TypeOf Me Is IHasURL Then _
                                        CType(Me, IHasURL).URL = CType(ResultDictionary.Item(Key), String)

                            Case "content"
                                If TypeOf Me Is IHasContent Then _
                                        CType(Me, IHasContent).Content = CType(ResultDictionary.Item(Key), String)

                            Case "source"
                                If TypeOf Me Is IHasSource Then _
                                        CType(Me, IHasSource).Source = CType(ResultDictionary.Item(Key), String)

                        End Select
                    Next

                    Exit Do
                Else
                    If WorkingInstance Is Nothing Then Exit Do

                    WorkingInstance = WorkingInstance.Parent
                End If
            Loop Until WorkingInstance Is Nothing
        End Sub

        Public ReadOnly Property Level As Integer Implements ILevelable.Level
            Get
                Return Me._Leveling
            End Get
        End Property

        Public ReadOnly Property LevelExecutionOnly As Boolean Implements ILevelable.LevelExecutionOnly
            Get
                Return Me._LevelingExecutionOnly
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

        Public Property Security() As SecurityInfo Implements IControl.SecurityInfo
            Get
                Return Me._Security
            End Get
            Set(ByVal value As SecurityInfo)
                Me._Security = value

                If Me._Security Is Nothing Then Me._Security = New SecurityInfo()
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

        Public Overridable Sub Clone(ByRef Control As IControl) Implements IControl.Clone
            If Control Is Nothing Then Exit Sub

            With CType(Control, ControlBase)
                ._Security = Me._Security
                ._Attributes.AddRange(Me._Attributes.ToArray())
                ._BindInfo = Me._BindInfo
            End With
        End Sub

        Public Shared Function CaptureControlType(ByVal ControlTypeName As String) As ControlTypes
            Dim rControlType As ControlTypes

            If Not [Enum].TryParse(ControlTypeName, True, rControlType) Then _
                rControlType = ControlTypes.Unknown

            Return rControlType
        End Function

        Public Shared Function MakeControl(ByVal DraftIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection, ByVal ControlResolveRequested As IControl.ControlResolveRequestedEventHandler) As ControlBase
            Dim rControl As ControlBase = Nothing

            ' Dummy Control just to check the Control Type
            Dim Control As New Unknown(DraftIndex, DraftValue, ContentArguments)
            AddHandler Control.ControlResolveRequested, ControlResolveRequested
            Control.Build()

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
                AddHandler rControl.ControlResolveRequested, ControlResolveRequested
                rControl.Build()
            End If

            Return rControl
        End Function

        Protected Sub RenderBindInfoParams()
            If Not Me.BindInfo Is Nothing AndAlso
                Not Me.BindInfo.ProcedureParams Is Nothing Then

                Dim ProcedureParams As String() =
                    CType(Array.CreateInstance(GetType(String), Me.BindInfo.ProcedureParams.Length), String())

                ' Render Params One By One (this render process is mainly controls with bind which fired when a control get interaction with user)
                ' The aim is rendering static values comes from dinamic ones like =$#SomeID$
                Dim DummyControllerContainer As ControllerBase
                For pC As Integer = 0 To Me.BindInfo.ProcedureParams.Length - 1
                    DummyControllerContainer = ControllerBase.ProvideDummyController(Me, Me.ContentArguments)
                    Me.RequestParse(Me.BindInfo.ProcedureParams(pC).Query, DummyControllerContainer)
                    DummyControllerContainer.Render(Me)
                    ProcedureParams(pC) = DummyControllerContainer.RenderedValue
                Next
                ' !---

                Me.BindInfo.OverrideProcedureParameters(ProcedureParams)
            End If
        End Sub

        Public Class SecurityInfo
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

            Public Sub Clone(ByRef Security As SecurityInfo)
                Security = New SecurityInfo()

                With Security
                    ._SecuritySet = Me._SecuritySet
                    ._RegisteredGroup = Me._RegisteredGroup
                    ._FriendlyName = Me._FriendlyName

                    If Not Me._BindInfo Is Nothing Then _
                        Me._BindInfo.Clone(._BindInfo)

                    ._Disabled = Me._Disabled
                End With
            End Sub

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
