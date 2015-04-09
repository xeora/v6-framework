Option Strict On

Namespace SolidDevelopment.Web
    Public Class Globals
        Public Class SystemMessages
            Public Const IDMUSTBESET As String = "ID must be set"
            Public Const PATH_NOTEXISTS As String = "Path is not Exists"
            Public Const PATH_WRONGSTRUCTURE As String = "Path Structure is Wrong"

            Public Const PASSWORD_WRONG As String = "Password is Wrong"

            Public Const ESSENTIAL_CONFIGURATIONNOTFOUND As String = "ConfigurationXML file is not found"
            Public Const ESSENTIAL_CONTROLSMAPNOTFOUND As String = "ControlsMapXML file is not found"

            Public Const CONFIGURATIONCONTENT As String = "Configuration Content value can not be null"
            Public Const TRANSLATIONCONTENT As String = "Translation Content value can not be null"

            Public Const TEMPLATE_AUTH As String = "This Template Requires Authentication"
            Public Const TEMPLATE_NOFOUND As String = "{0} Named Template file is not found"

            Public Const TEMPLATE_IDMUSTBESET As String = "TemplateID must be set"

            Public Const WEBSERVICE_AUTH As String = "This Webservice Requires Authentication"

            Public Const SYSTEM_ERROROCCURED As String = "System Error Occured"
            Public Const SYSTEM_APPLICATIONLOADINGERROR As String = "Application Loading Error Occured"
        End Class

        Public Class Controls
            Public Enum ControlTypes
                Textbox
                Password
                Checkbox
                Button
                Radio
                Textarea
                Image
                Link

                DataList
                ConditionalStatement
                VariableBlock

                Unknown
            End Enum

            Public Class AttributeInfo
                Private _Id As String
                Private _Value As String

                Public Sub New(ByVal Id As String, ByVal Value As String)
                    Me._Id = Id
                    Me._Value = Value
                End Sub

                Public ReadOnly Property Id() As String
                    Get
                        Return Me._Id
                    End Get
                End Property

                Public ReadOnly Property Value() As String
                    Get
                        Return Me._Value
                    End Get
                End Property

                Public Class AttributeInfoCollection
                    Inherits ArrayList

                    Public Sub New()
                        MyBase.New()
                    End Sub

                    Public Shadows Sub Add(ByVal id As String, ByVal value As String)
                        MyBase.Add(New AttributeInfo(id, value))
                    End Sub

                    Public Shadows Sub Add(ByVal value As AttributeInfo)
                        MyBase.Add(value)
                    End Sub

                    Public Shadows Sub Remove(ByVal id As String)
                        For Each item As AttributeInfo In Me
                            If String.Compare(id, item.Id, True) = 0 Then
                                MyBase.Remove(item)

                                Exit For
                            End If
                        Next
                    End Sub

                    Public Shadows Sub Remove(ByVal value As AttributeInfo)
                        MyBase.Remove(value)
                    End Sub

                    Public Shadows Property Item(ByVal id As String) As String
                        Get
                            Dim rString As String = Nothing

                            For Each aI As AttributeInfo In Me
                                If String.Compare(id, aI.Id, True) = 0 Then
                                    rString = aI.Value

                                    Exit For
                                End If
                            Next

                            Return rString
                        End Get
                        Set(ByVal Value As String)
                            Me.Remove(id)
                            Me.Add(id, Value)
                        End Set
                    End Property

                    Public Shadows Function ToArray() As AttributeInfo()
                        Return CType(MyBase.ToArray(GetType(AttributeInfo)), AttributeInfo())
                    End Function
                End Class
            End Class

            Public Interface IControlBase
                ReadOnly Property ControlID() As String
                Property SecurityInfo() As ControlBase.SecurityInfos
                ReadOnly Property Type() As ControlTypes
                Property CallFunction() As String
                ReadOnly Property Attributes() As AttributeInfo.AttributeInfoCollection
                ReadOnly Property BlockIDsToUpdate() As System.Collections.Generic.List(Of String)
                Property BlockLocalUpdate() As Boolean
                Sub Clone(ByRef rControlBase As IControlBase)
            End Interface

            Public Class ControlBase
                Implements IControlBase

                Private _ControlID As String
                Private _SecurityInfo As SecurityInfos
                Private _ControlType As Controls.ControlTypes
                Private _CallFunction As String
                Private _Attributes As AttributeInfo.AttributeInfoCollection
                Private _BlockIDsToUpdate As System.Collections.Generic.List(Of String)
                Private _BlockLocalUpdate As Boolean

                Public Sub New(ByVal ControlID As String, ByVal ControlType As Controls.ControlTypes)
                    Me._ControlID = ControlID
                    Me._SecurityInfo = New SecurityInfos()
                    Me._ControlType = ControlType
                    Me._Attributes = New AttributeInfo.AttributeInfoCollection
                    Me._BlockIDsToUpdate = New System.Collections.Generic.List(Of String)
                    Me._BlockLocalUpdate = True
                End Sub

                Public ReadOnly Property ControlID() As String Implements IControlBase.ControlID
                    Get
                        Return Me._ControlID
                    End Get
                End Property

                Public Property SecurityInfo() As SecurityInfos Implements IControlBase.SecurityInfo
                    Get
                        Return Me._SecurityInfo
                    End Get
                    Set(ByVal value As SecurityInfos)
                        Me._SecurityInfo = value

                        If Me._SecurityInfo Is Nothing Then Me._SecurityInfo = New SecurityInfos
                    End Set
                End Property

                Public ReadOnly Property Type() As Controls.ControlTypes Implements IControlBase.Type
                    Get
                        Return Me._ControlType
                    End Get
                End Property

                Public Property CallFunction() As String Implements IControlBase.CallFunction
                    Get
                        Return Me._CallFunction
                    End Get
                    Set(ByVal Value As String)
                        Me._CallFunction = Value
                    End Set
                End Property

                Public ReadOnly Property Attributes() As AttributeInfo.AttributeInfoCollection Implements IControlBase.Attributes
                    Get
                        Return Me._Attributes
                    End Get
                End Property

                Public Property BlockLocalUpdate() As Boolean Implements IControlBase.BlockLocalUpdate
                    Get
                        Return Me._BlockLocalUpdate
                    End Get
                    Set(ByVal Value As Boolean)
                        Me._BlockLocalUpdate = Value
                    End Set
                End Property

                Public ReadOnly Property BlockIDsToUpdate() As System.Collections.Generic.List(Of String) Implements IControlBase.BlockIDsToUpdate
                    Get
                        Return Me._BlockIDsToUpdate
                    End Get
                End Property

                Public Overridable Sub Clone(ByRef rControlBase As IControlBase) Implements IControlBase.Clone
                    If rControlBase Is Nothing Then rControlBase = New ControlBase(Me._ControlID, Me._ControlType)

                    With CType(rControlBase, ControlBase)
                        ._SecurityInfo = Me._SecurityInfo
                        ._Attributes.AddRange(Me._Attributes.ToArray())
                        ._BlockIDsToUpdate.AddRange(Me._BlockIDsToUpdate.ToArray())
                        ._BlockLocalUpdate = Me._BlockLocalUpdate
                        ._CallFunction = Me._CallFunction
                    End With
                End Sub

                Public Class SecurityInfos
                    Private _SecuritySet As Boolean
                    Private _RegisteredGroup As String
                    Private _FriendlyName As String
                    Private _CallFunction As String
                    Private _Disabled As DisabledClass

                    Public Sub New()
                        Me._SecuritySet = False
                        Me._RegisteredGroup = String.Empty
                        Me._FriendlyName = String.Empty
                        Me._CallFunction = String.Empty
                        Me._Disabled = New DisabledClass
                    End Sub

                    Public Property SecuritySet() As Boolean
                        Get
                            Return Me._SecuritySet
                        End Get
                        Set(ByVal value As Boolean)
                            Me._SecuritySet = value

                            If Me._SecuritySet Then
                                If String.IsNullOrEmpty(Me._CallFunction) Then Me._CallFunction = "#GLOBAL"
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
                                If String.IsNullOrEmpty(Me._CallFunction) Then Me._CallFunction = "#GLOBAL"
                            End If

                            Me._SecuritySet = True
                        End Set
                    End Property

                    Public Property CallFunction() As String
                        Get
                            Return Me._CallFunction
                        End Get
                        Set(ByVal value As String)
                            Me._CallFunction = value

                            If String.IsNullOrEmpty(Me._CallFunction) Then Me._CallFunction = "#GLOBAL"
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

            Public Class Textbox
                Inherits ControlBase

                Private _Text As String
                Private _DefaultButtonID As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Textbox)
                End Sub

                Public Property DefaultButtonID() As String
                    Get
                        Return Me._DefaultButtonID
                    End Get
                    Set(ByVal Value As String)
                        Me._DefaultButtonID = Value
                    End Set
                End Property

                Public Property Text() As String
                    Get
                        Return Me._Text
                    End Get
                    Set(ByVal Value As String)
                        Me._Text = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Textbox(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Textbox)
                        ._DefaultButtonID = Me._DefaultButtonID
                        ._Text = Me._Text
                    End With
                End Sub
            End Class

            Public Class Password
                Inherits ControlBase

                Private _Text As String
                Private _DefaultButtonID As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Password)
                End Sub

                Public Property DefaultButtonID() As String
                    Get
                        Return Me._DefaultButtonID
                    End Get
                    Set(ByVal Value As String)
                        Me._DefaultButtonID = Value
                    End Set
                End Property

                Public Property Text() As String
                    Get
                        Return Me._Text
                    End Get
                    Set(ByVal Value As String)
                        Me._Text = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Password(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Password)
                        ._DefaultButtonID = Me._DefaultButtonID
                        ._Text = Me._Text
                    End With
                End Sub
            End Class

            Public Class CheckBox
                Inherits ControlBase

                Private _LabelText As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Checkbox)
                End Sub

                Public Property Text() As String
                    Get
                        Return Me._LabelText
                    End Get
                    Set(ByVal Value As String)
                        Me._LabelText = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New CheckBox(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, CheckBox)
                        ._LabelText = Me._LabelText
                    End With
                End Sub
            End Class

            Public Class RadioButton
                Inherits ControlBase

                Private _LabelText As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Radio)
                End Sub

                Public Property Text() As String
                    Get
                        Return Me._LabelText
                    End Get
                    Set(ByVal Value As String)
                        Me._LabelText = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New RadioButton(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, RadioButton)
                        ._LabelText = Me._LabelText
                    End With
                End Sub
            End Class

            Public Class Button
                Inherits ControlBase

                Private _Text As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Button)
                End Sub

                Public Property Text() As String
                    Get
                        Return Me._Text
                    End Get
                    Set(ByVal Value As String)
                        Me._Text = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Button(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Button)
                        ._Text = Me._Text
                    End With
                End Sub
            End Class

            Public Class Textarea
                Inherits ControlBase

                Private _Content As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Textarea)
                End Sub

                Public Property Content() As String
                    Get
                        Return Me._Content
                    End Get
                    Set(ByVal Value As String)
                        Me._Content = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Textarea(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Textarea)
                        ._Content = Me._Content
                    End With
                End Sub
            End Class

            Public Class Image
                Inherits ControlBase

                Private _Source As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Image)
                End Sub

                Public Property Source() As String
                    Get
                        Return Me._Source
                    End Get
                    Set(ByVal Value As String)
                        Me._Source = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Image(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Image)
                        ._Source = Me._Source
                    End With
                End Sub
            End Class

            Public Class Link
                Inherits ControlBase

                Private _Text As String
                Private _Url As String

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.Link)
                End Sub

                Public Property Url() As String
                    Get
                        Return Me._Url
                    End Get
                    Set(ByVal Value As String)
                        Me._Url = Value
                    End Set
                End Property

                Public Property Text() As String
                    Get
                        Return Me._Text
                    End Get
                    Set(ByVal Value As String)
                        Me._Text = Value
                    End Set
                End Property

                Public Overrides Sub Clone(ByRef rControlBase As IControlBase)
                    rControlBase = New Link(MyBase.ControlID)
                    MyBase.Clone(rControlBase)

                    With CType(rControlBase, Link)
                        ._Text = Me._Text
                        ._Url = Me._Url
                    End With
                End Sub
            End Class

            Public Class DataList
                Inherits ControlBase

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.DataList)
                End Sub
            End Class

            Public Class ConditionalStatement
                Inherits ControlBase

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.ConditionalStatement)
                End Sub
            End Class

            Public Class VariableBlock
                Inherits ControlBase

                Public Sub New(ByVal ControlID As String)
                    MyBase.New(ControlID, ControlTypes.VariableBlock)
                End Sub
            End Class
        End Class

        <Serializable()> _
        Public Class DataListOutputInfo
            Private _PartialRecords As Integer
            Private _TotalRecords As Integer
            Private _Message As PGlobals.MapControls.MessageResult

            Public Sub New(ByVal PartialRecords As Integer, ByVal TotalRecords As Integer)
                Me._Message = Nothing

                Me._PartialRecords = PartialRecords
                Me._TotalRecords = TotalRecords
            End Sub

            Public ReadOnly Property Count() As Integer
                Get
                    Return Me._PartialRecords
                End Get
            End Property

            Public ReadOnly Property Total() As Integer
                Get
                    Return Me._TotalRecords
                End Get
            End Property

            Public Property Message() As PGlobals.MapControls.MessageResult
                Get
                    Return Me._Message
                End Get
                Set(ByVal value As PGlobals.MapControls.MessageResult)
                    Me._Message = value
                End Set
            End Property
        End Class

        Public Class RenderRequestInfo
            Private _ReturnException As Exception

            Public Enum BlockRenderingStatuses As Integer
                Undefined = 0
                Rendering = 1
                Rendered = 2
            End Enum

            Private _BlockID As String
            Private _RequestingBlockRendering As Boolean
            Private _BlockRenderingStatus As BlockRenderingStatuses

            Private _MainContent As CommonControlContent
            Private _GlobalArguments As ArgumentInfo.ArgumentInfoCollection

            Private _ParentPendingCommonControlContents As Generic.Dictionary(Of String, Generic.List(Of CommonControlContent))

            Public Sub New(ByVal BlockID As String, ByVal TemplateID As String, ByVal GlobalsArguments As Globals.ArgumentInfo.ArgumentInfoCollection)
                Me._ReturnException = Nothing

                ' Set Block Id Variables
                Me._BlockID = BlockID

                Me._RequestingBlockRendering = False
                Me._BlockRenderingStatus = BlockRenderingStatuses.Undefined
                ' !---

                Me._MainContent = New CommonControlContent(Nothing, 0, String.Format("$T:{0}$", TemplateID), 0)

                Me._GlobalArguments = GlobalsArguments
                If Me._GlobalArguments Is Nothing Then Me._GlobalArguments = New ArgumentInfo.ArgumentInfoCollection

                Me._ParentPendingCommonControlContents = New Generic.Dictionary(Of String, Generic.List(Of CommonControlContent))
            End Sub

            Public Property ReturnException() As Exception
                Get
                    Return Me._ReturnException
                End Get
                Set(ByVal value As Exception)
                    Me._ReturnException = value
                End Set
            End Property

            Public Property BlockID() As String
                Get
                    Return Me._BlockID
                End Get
                Set(ByVal value As String)
                    Me._BlockID = value
                End Set
            End Property

            Public Property RequestingBlockRendering() As Boolean
                Get
                    Return Me._RequestingBlockRendering
                End Get
                Set(ByVal value As Boolean)
                    Me._RequestingBlockRendering = value
                End Set
            End Property

            Public Property BlockRenderingStatus() As BlockRenderingStatuses
                Get
                    Return Me._BlockRenderingStatus
                End Get
                Set(ByVal value As BlockRenderingStatuses)
                    Me._BlockRenderingStatus = value
                End Set
            End Property

            Public ReadOnly Property MainContent() As CommonControlContent
                Get
                    Return Me._MainContent
                End Get
            End Property

            Public ReadOnly Property GlobalArguments() As ArgumentInfo.ArgumentInfoCollection
                Get
                    Return Me._GlobalArguments
                End Get
            End Property

            Public ReadOnly Property ParentPendingCommonControlContents() As Generic.Dictionary(Of String, Generic.List(Of CommonControlContent))
                Get
                    Return Me._ParentPendingCommonControlContents
                End Get
            End Property

            Public MustInherit Class ContentInfo
                Private _OriginalStartIndex As Integer
                Private _OriginalValue As String
                Private _ModifierTuneLength As Integer
                Private _ClearedValue As String

                Private _Parent As ContentInfo
                Private _Leveling As Integer
                Private _ContentItems As Generic.List(Of ContentInfo)
                Private _ContentArguments As ArgumentInfo.ArgumentInfoCollection

                Private _RenderedContentValue As String
                Private _IsContentRendered As Boolean

                Private _HelperSpace As HelperSpaceClass

                Public Enum ContentTypes As Integer
                    SpecialProperty = 1
                    CommonControl = 2
                    Renderless = 3
                End Enum

                Public MustOverride ReadOnly Property ContentType() As ContentTypes

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ModifierTuneLength As Integer)
                    Me.New(Parent, OriginalStartIndex, OriginalValue, ModifierTuneLength, Nothing)
                End Sub

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ModifierTuneLength As Integer, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
                    If OriginalStartIndex < 0 Then Throw New Exception("Index can not be less than zero!")
                    If OriginalValue Is Nothing Then Throw New Exception("Value can not be null!")

                    If Me.ContentType = ContentTypes.Renderless Then
                        ' Change ~/ values with the exact root path
                        Dim RootPathMatches As System.Text.RegularExpressions.MatchCollection = _
                            System.Text.RegularExpressions.Regex.Matches(OriginalValue, "[""']+~/", Text.RegularExpressions.RegexOptions.Multiline)

                        Dim wR As String = Configurations.ApplicationRoot.BrowserSystemImplementation

                        Dim OriginalValue_temp As New System.Text.StringBuilder, SetTempToOriginal As Boolean = False
                        Dim tMatchItem As System.Text.RegularExpressions.Match
                        Dim PrevLocIndex As Integer = 0
                        Dim REMEnum As IEnumerator = _
                            RootPathMatches.GetEnumerator()
                        Do While REMEnum.MoveNext()
                            SetTempToOriginal = True

                            tMatchItem = CType(REMEnum.Current, System.Text.RegularExpressions.Match)

                            OriginalValue_temp.Append( _
                                OriginalValue.Substring(PrevLocIndex, tMatchItem.Index - PrevLocIndex) _
                            )
                            OriginalValue_temp.AppendFormat( _
                                "{0}{1}", _
                                tMatchItem.Value.Substring(0, 1), _
                                wR _
                            )

                            PrevLocIndex = tMatchItem.Index + tMatchItem.Length
                        Loop
                        ' !---
                        If SetTempToOriginal Then
                            OriginalValue_temp.Append(OriginalValue.Substring(PrevLocIndex))
                            OriginalValue = OriginalValue_temp.ToString()
                        End If
                    End If

                    Me._OriginalStartIndex = OriginalStartIndex
                    Me._OriginalValue = OriginalValue
                    Me._ModifierTuneLength = ModifierTuneLength
                    Me._ClearedValue = OriginalValue

                    ' Remove block signs
                    If Me._ClearedValue.Chars(0) = "$" AndAlso _
                        Me._ClearedValue.Chars(Me._ClearedValue.Length - 1) = "$" Then

                        Me._ClearedValue = Me._ClearedValue.Substring(1, Me._ClearedValue.Length - 2)
                    End If
                    ' !--

                    Me._Parent = Parent
                    If Me._Parent Is Nothing Then Me._Leveling = 0 Else Me._Leveling = Me._Parent._Leveling
                    Me._ContentItems = New Generic.List(Of ContentInfo)

                    If ContentArguments Is Nothing Then
                        If Not Me._Parent Is Nothing Then
                            Me._ContentArguments = New ArgumentInfo.ArgumentInfoCollection(Me._Parent.ContentArguments)
                        Else
                            Me._ContentArguments = New ArgumentInfo.ArgumentInfoCollection
                        End If
                    Else
                        If Not Me._Parent Is Nothing Then
                            Me._ContentArguments = ContentArguments.Clone(Me._Parent.ContentArguments)
                        Else
                            Me._ContentArguments = ContentArguments
                        End If
                    End If

                    Me._RenderedContentValue = String.Empty
                    Me._IsContentRendered = False

                    Me._HelperSpace = New HelperSpaceClass(Me)
                End Sub

                Public ReadOnly Property OriginalStartIndex() As Integer
                    Get
                        Return Me._OriginalStartIndex
                    End Get
                End Property

                Public ReadOnly Property OriginalLength() As Integer
                    Get
                        Return Me._OriginalValue.Length
                    End Get
                End Property

                Public ReadOnly Property ModifierTuneLength() As Integer
                    Get
                        Return Me._ModifierTuneLength
                    End Get
                End Property

                Public ReadOnly Property OriginalValue() As String
                    Get
                        Return Me._OriginalValue
                    End Get
                End Property

                Public ReadOnly Property ClearedValue() As String
                    Get
                        Return Me._ClearedValue
                    End Get
                End Property

                Public ReadOnly Property Parent() As ContentInfo
                    Get
                        Return Me._Parent
                    End Get
                End Property

                Public ReadOnly Property Leveling As Integer
                    Get
                        Return Me._Leveling
                    End Get
                End Property

                Public WriteOnly Property Leveling(ByVal IsLocalLeveling As Boolean) As Integer
                    Set(ByVal value As Integer)
                        If Not Me._Parent Is Nothing AndAlso Not IsLocalLeveling Then Me._Parent.Leveling(IsLocalLeveling) = (value - 1)
                        If value > -1 Then Me._Leveling = value Else Me._Leveling = 0
                    End Set
                End Property

                Public ReadOnly Property ContentArguments() As ArgumentInfo.ArgumentInfoCollection
                    Get
                        Return Me._ContentArguments
                    End Get
                End Property

                Public ReadOnly Property ContentItems() As Generic.List(Of ContentInfo)
                    Get
                        Return Me._ContentItems
                    End Get
                End Property

                Public ReadOnly Property RenderedContentValue() As String
                    Get
                        Return Me._RenderedContentValue
                    End Get
                End Property

                Public ReadOnly Property IsContentRendered() As Boolean
                    Get
                        Return Me._IsContentRendered
                    End Get
                End Property

                Public ReadOnly Property HelperSpace() As HelperSpaceClass
                    Get
                        Return Me._HelperSpace
                    End Get
                End Property

                Public Class HelperSpaceClass
                    Private _Space As String
                    Private _Parent As ContentInfo

                    Friend Sub New(ByVal Parent As ContentInfo)
                        Me._Parent = Parent
                        Me._Space = Me._Parent._OriginalValue
                    End Sub

                    Public Property Space() As String
                        Get
                            Return Me._Space
                        End Get
                        Set(ByVal value As String)
                            Me._Space = value
                        End Set
                    End Property

                    Public Sub DefineAsRendered()
                        Me._Parent._RenderedContentValue = Me._Space

                        If Me._Parent._RenderedContentValue Is Nothing Then Me._Parent._RenderedContentValue = String.Empty
                        Me._Parent._IsContentRendered = True
                    End Sub
                End Class

            End Class

            Public Class SpecialPropertyContent
                Inherits ContentInfo

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String)
                    Me.New(Parent, OriginalStartIndex, OriginalValue, Nothing)
                End Sub

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
                    MyBase.New(Parent, OriginalStartIndex, OriginalValue, 0, ContentArguments)
                End Sub

                Public Overrides ReadOnly Property ContentType() As ContentInfo.ContentTypes
                    Get
                        Return ContentTypes.SpecialProperty
                    End Get
                End Property

            End Class

            Public Class CommonControlContent
                Inherits ContentInfo

                Private _CommonControlType As CommonControlTypes
                Private _CommonControlID As String

                Public Enum CommonControlTypes As Integer
                    Template = 1
                    Translation = 2
                    Control = 3
                    ControlWithParent = 11
                    HashCodeParser = 4
                    DirectCallFunction = 5
                    DirectCallFunctionWithParent = 10
                    StatementBlock = 6
                    RequestBlock = 7
                    EncodedCallFunction = 8
                    SimpleValue = 9
                    Undefined = 99
                End Enum

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ModifierTuneLength As Integer)
                    Me.New(Parent, OriginalStartIndex, OriginalValue, ModifierTuneLength, Nothing)
                End Sub

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ModifierTuneLength As Integer, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
                    MyBase.New(Parent, OriginalStartIndex, OriginalValue, ModifierTuneLength, ContentArguments)

                    Me._CommonControlType = CommonControlTypes.Undefined
                End Sub

                Public Overrides ReadOnly Property ContentType() As ContentInfo.ContentTypes
                    Get
                        Return ContentTypes.CommonControl
                    End Get
                End Property

                Public Property CommonControlType() As CommonControlTypes
                    Get
                        Return Me._CommonControlType
                    End Get
                    Set(ByVal value As CommonControlTypes)
                        Me._CommonControlType = value
                    End Set
                End Property

                Public Property CommonControlID() As String
                    Get
                        Return Me._CommonControlID
                    End Get
                    Set(ByVal value As String)
                        Me._CommonControlID = value
                    End Set
                End Property

            End Class

            Public Class RenderlessContent
                Inherits ContentInfo

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String)
                    Me.New(Parent, OriginalStartIndex, OriginalValue, Nothing)
                End Sub

                Public Sub New(ByVal Parent As ContentInfo, ByVal OriginalStartIndex As Integer, ByVal OriginalValue As String, ByVal ContentArguments As ArgumentInfo.ArgumentInfoCollection)
                    MyBase.New(Parent, OriginalStartIndex, OriginalValue, 0, ContentArguments)
                End Sub

                Public Overrides ReadOnly Property ContentType() As ContentInfo.ContentTypes
                    Get
                        Return ContentTypes.Renderless
                    End Get
                End Property

            End Class

        End Class

        Public Class ArgumentInfo
            Private _Key As String
            Private _Value As Object

            Public Sub New(ByVal Key As String, Optional ByVal Value As Object = Nothing)
                Me._Key = Key
                Me._Value = Value
            End Sub

            Public ReadOnly Property Key() As String
                Get
                    Return Me._Key
                End Get
            End Property

            Public ReadOnly Property Value() As Object
                Get
                    Return Me._Value
                End Get
            End Property

            Public Class ArgumentInfoCollection
                Implements System.Collections.IEnumerable
                Implements System.ICloneable

                Private _Parent As ArgumentInfoCollection
                Private _ArgumentInfos As Hashtable

                Public Enum BaseIndexModifier
                    UseFirstParameterAsBase
                    UseSecondParameterAsBase
                End Enum

                Public Sub New()
                    Me.New(Nothing)
                End Sub

                Public Sub New(ByVal Parent As ArgumentInfoCollection)
                    Me._ArgumentInfos = New Hashtable
                    Me._Parent = Parent
                End Sub

                Public ReadOnly Property Parent() As ArgumentInfoCollection
                    Get
                        Return Me._Parent
                    End Get
                End Property

                Public Shadows Sub Add(ByVal key As String, ByVal value As Object)
                    Me._ArgumentInfos.Item(key) = New ArgumentInfo(key, value)
                End Sub

                Public Shadows Sub Add(ByVal item As ArgumentInfo)
                    Me._ArgumentInfos.Item(item.Key) = item
                End Sub

                Public Shadows Sub Remove(ByVal key As String)
                    If Me._ArgumentInfos.ContainsKey(key) Then Me._ArgumentInfos.Remove(key)
                End Sub

                Public Shadows Sub Remove(ByVal value As ArgumentInfo)
                    Me.Remove(value.Key)
                End Sub

                Public Property Item(ByVal key As String) As ArgumentInfo
                    Get
                        Dim rArgumentInfo As New ArgumentInfo(key, Nothing)

                        If Me._ArgumentInfos.ContainsKey(key) Then rArgumentInfo = CType(Me._ArgumentInfos.Item(key), ArgumentInfo)

                        Return rArgumentInfo
                    End Get
                    Set(ByVal value As ArgumentInfo)
                        Me._ArgumentInfos.Item(value.Key) = value
                    End Set
                End Property

                Public Function ContainsKey(ByVal key As String) As Boolean
                    Return Me._ArgumentInfos.ContainsKey(key)
                End Function

                Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                    Return Me._ArgumentInfos.GetEnumerator()
                End Function

                Public Shared Function Combine(ByVal Arguments1 As ArgumentInfoCollection, ByVal Arguments2 As ArgumentInfoCollection, Optional ByVal bIM As BaseIndexModifier = BaseIndexModifier.UseSecondParameterAsBase) As ArgumentInfoCollection
                    Dim rArguments As ArgumentInfoCollection
                    Dim argEnumerator As System.Collections.IEnumerator

                    Select Case bIM
                        Case BaseIndexModifier.UseFirstParameterAsBase
                            rArguments = New ArgumentInfoCollection(Arguments1.Parent)
                        Case BaseIndexModifier.UseSecondParameterAsBase
                            rArguments = New ArgumentInfoCollection(Arguments2.Parent)
                        Case Else
                            Throw New ArgumentOutOfRangeException("BaseIndex")
                    End Select

                    If Not Arguments1 Is Nothing Then
                        argEnumerator = Arguments1.GetEnumerator()
                        Do While argEnumerator.MoveNext()
                            Dim currentEnumItem As System.Collections.DictionaryEntry = _
                                CType(argEnumerator.Current, System.Collections.DictionaryEntry)

                            rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                        Loop
                    End If

                    If Not Arguments2 Is Nothing Then
                        argEnumerator = Arguments2.GetEnumerator()
                        Do While argEnumerator.MoveNext()
                            Dim currentEnumItem As System.Collections.DictionaryEntry = _
                                CType(argEnumerator.Current, System.Collections.DictionaryEntry)

                            rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                        Loop
                    End If

                    Return rArguments
                End Function

                Public Overloads Function Clone() As Object Implements System.ICloneable.Clone
                    Dim rArguments As New ArgumentInfoCollection
                    Dim argEnumerator As System.Collections.IEnumerator

                    argEnumerator = Me.GetEnumerator()
                    Do While argEnumerator.MoveNext()
                        Dim currentEnumItem As System.Collections.DictionaryEntry = _
                            CType(argEnumerator.Current, System.Collections.DictionaryEntry)

                        rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                    Loop

                    Return rArguments
                End Function

                Public Overloads Function Clone(ByVal Parent As ArgumentInfoCollection) As ArgumentInfoCollection
                    Dim rArguments As New ArgumentInfoCollection(Parent)
                    Dim argEnumerator As System.Collections.IEnumerator

                    argEnumerator = Me.GetEnumerator()
                    Do While argEnumerator.MoveNext()
                        Dim currentEnumItem As System.Collections.DictionaryEntry = _
                            CType(argEnumerator.Current, System.Collections.DictionaryEntry)

                        rArguments.Add(CType(currentEnumItem.Value, ArgumentInfo))
                    Loop

                    Return rArguments
                End Function
            End Class
        End Class

        <Serializable()> _
        Friend Class WebServiceSessionInfo
            Implements IEnumerable

            Private _SessionItems As System.Collections.Generic.List(Of DictionaryEntry)
            Private _SessionDate As DateTime
            Private _PublicKey As String

            Public Sub New(ByVal PublicKey As String, ByVal SessionDate As DateTime)
                Me._SessionItems = New System.Collections.Generic.List(Of DictionaryEntry)

                Me._PublicKey = PublicKey
                Me._SessionDate = SessionDate
            End Sub

            Public ReadOnly Property PublicKey() As String
                Get
                    Return Me._PublicKey
                End Get
            End Property

            Public Property SessionDate() As DateTime
                Get
                    Return Me._SessionDate
                End Get
                Set(ByVal value As DateTime)
                    Me._SessionDate = value
                End Set
            End Property

            Public Sub AddSessionItem(ByVal Key As String, ByVal Value As Object)
                Me.RemoveSessionItem(Key)

                Me._SessionItems.Add( _
                    New DictionaryEntry(Key, Value))
            End Sub

            Public Sub RemoveSessionItem(ByVal Key As String)
                For iC As Integer = Me._SessionItems.Count - 1 To 0 Step -1
                    If String.Compare(Me._SessionItems(iC).Key.ToString(), Key, True) = 0 Then
                        Me._SessionItems.RemoveAt(iC)
                    End If
                Next
            End Sub

            Public ReadOnly Property Item(ByVal Key As String) As Object
                Get
                    Dim rValue As Object = Nothing

                    For Each dItem As DictionaryEntry In Me._SessionItems
                        If String.Compare(dItem.Key.ToString(), Key, True) = 0 Then
                            rValue = dItem.Value

                            Exit For
                        End If
                    Next

                    Return rValue
                End Get
            End Property

            Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return Me._SessionItems.GetEnumerator()
            End Function
        End Class
    End Class
End Namespace