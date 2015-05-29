Option Strict On

Namespace SolidDevelopment.Web
    <CLSCompliant(True)> _
    Public Class PGlobals
        Public Enum PageCachingTypes
            AllContent
            AllContentCookiless
            TextsOnly
            TextsOnlyCookiless
            NoCache
            NoCacheCookiless
        End Enum

        Public Enum RequestTagFilteringTypes
            None
            OnlyForm
            OnlyQuery
            Both
        End Enum

        Public Class PlugInMarkers
            Public Interface ITheme
                ' Just Implementation
            End Interface

            Public Interface IAddon
                ' Just Implementation
            End Interface
        End Class

        Public Interface ITheme
            Inherits IDisposable

            ReadOnly Property CurrentID() As String
            ReadOnly Property DeploymentType() As ThemeBase.DeploymentTypes
            ReadOnly Property DeploymentStyle() As ThemeBase.DeploymentStyles
            ReadOnly Property Settings() As ITheme.ISettings
            ReadOnly Property Translation() As ITheme.ITranslation
            ReadOnly Property WebService() As ITheme.IWebService
            ReadOnly Property Addons() As ITheme.IAddons

            Public Interface ISettings
                ReadOnly Property Configurations() As ITheme.ISettings.IConfigurationClass

                Public Interface IConfigurationClass
                    ReadOnly Property AuthenticationPage() As String
                    ReadOnly Property DefaultPage() As String
                    ReadOnly Property DefaultTranslation() As String
                    ReadOnly Property DefaultCaching() As PageCachingTypes
                    ReadOnly Property DefaultSecurityCallFunction() As String
                End Interface
            End Interface

            Public Interface ITranslation
                ReadOnly Property CurrentTranslationID() As String
                ReadOnly Property CurrentTranslationName() As String
                ReadOnly Property CurrentTranslationInfo() As ThemeInfo.ThemeTranslationInfo
                Function GetTranslation(ByVal id As String) As String
            End Interface

            Public Interface IWebService
                ReadOnly Property ReadSessionVariable(ByVal PublicKey As String, ByVal name As String) As Object
                Function CreateWebServiceAuthentication(ByVal ParamArray dItems() As System.Collections.DictionaryEntry) As String
            End Interface

            Public Interface IAddons
                Inherits IDisposable

                ReadOnly Property AddonInfos() As ThemeInfo.AddonInfo()
                ReadOnly Property CurrentInstance() As ITheme
                Sub CreateInstance(ByVal AddonInfo As ThemeInfo.AddonInfo)
                Sub DisposeInstance()
            End Interface
        End Interface

        Public MustInherit Class ThemeBase
            Private _dT As DeploymentTypes
            Private _dS As DeploymentStyles

            Public Enum DeploymentTypes
                Development
                Release
            End Enum

            Public Enum DeploymentStyles
                Addon
                Theme
            End Enum

            Public Sub New(ByVal dT As DeploymentTypes, ByVal dS As DeploymentStyles)
                Me._dT = dT
                Me._dS = dS
            End Sub

            Public ReadOnly Property DeploymentType() As DeploymentTypes
                Get
                    Return Me._dT
                End Get
            End Property

            Public ReadOnly Property DeploymentStyle() As DeploymentStyles
                Get
                    Return Me._dS
                End Get
            End Property
        End Class

        Public Class ThemeInfo
            Inherits ThemeBase

            Private _ThemeID As String
            Private _ThemeTranslations As ThemeTranslationInfo()
            Private _ThemeAddons As AddonInfo()

            Public Sub New(ByVal DeploymentType As DeploymentTypes, ByVal ThemeID As String, ByVal ThemeTranslations As ThemeTranslationInfo(), ByVal ThemeAddons As AddonInfo())
                MyBase.New(DeploymentType, DeploymentStyles.Theme)

                Me._ThemeID = ThemeID
                Me._ThemeTranslations = ThemeTranslations
                Me._ThemeAddons = ThemeAddons
            End Sub

            Public ReadOnly Property ThemeID() As String
                Get
                    Return Me._ThemeID
                End Get
            End Property

            Public ReadOnly Property ThemeTranslations() As ThemeTranslationInfo()
                Get
                    Return Me._ThemeTranslations
                End Get
            End Property

            Public ReadOnly Property ThemeAddons() As AddonInfo()
                Get
                    Return Me._ThemeAddons
                End Get
            End Property

            Public Class ThemeTranslationInfo
                Private _LanguageName As String
                Private _TranslationCode As String

                Public Sub New(ByVal LanguageName As String, ByVal TranslationCode As String)
                    Me._LanguageName = LanguageName
                    Me._TranslationCode = TranslationCode
                End Sub

                Public ReadOnly Property Name() As String
                    Get
                        Return Me._LanguageName
                    End Get
                End Property

                Public ReadOnly Property Code() As String
                    Get
                        Return Me._TranslationCode
                    End Get
                End Property
            End Class

            Public Class AddonInfo
                Inherits ThemeBase

                Private _AddonID As String
                Private _AddonVersion As String
                Private _AddonPassword As Byte()

                Public Sub New( _
                    ByVal DeploymentType As DeploymentTypes, _
                    ByVal AddonID As String, _
                    ByVal AddonVersion As String, _
                    ByVal AddonPassword As Byte())

                    MyBase.New(DeploymentType, DeploymentStyles.Addon)

                    Me._AddonID = AddonID
                    Me._AddonVersion = AddonVersion
                    Me._AddonPassword = AddonPassword
                End Sub

                Public ReadOnly Property AddonID() As String
                    Get
                        Return Me._AddonID
                    End Get
                End Property

                Public ReadOnly Property AddonVersion() As String
                    Get
                        Return Me._AddonVersion
                    End Get
                End Property

                Public ReadOnly Property AddonPassword() As Byte()
                    Get
                        Return Me._AddonPassword
                    End Get
                End Property
            End Class
        End Class

        Public Class ThemeInfoCollection
            Inherits ArrayList

            Public Sub New()
                MyBase.New()
            End Sub

            Public Shadows Sub Add(ByVal value As ThemeInfo)
                MyBase.Add(value)
            End Sub

            Public Shadows Sub Remove(ByVal ThemeID As String)
                For Each item As ThemeInfo In Me
                    If String.Compare(ThemeID, item.ThemeID, True) = 0 Then
                        MyBase.Remove(item)

                        Exit For
                    End If
                Next
            End Sub

            Public Shadows Sub Remove(ByVal value As ThemeInfo)
                Me.Remove(value.ThemeID)
            End Sub

            Public Shadows Property Item(ByVal Index As Integer) As ThemeInfo
                Get
                    Dim rThemeInfo As ThemeInfo = Nothing

                    If Index < Me.Count Then rThemeInfo = CType(Me(Index), ThemeInfo)

                    Return rThemeInfo
                End Get
                Set(ByVal Value As ThemeInfo)
                    Me.Remove(Value.ThemeID)
                    Me.Add(Value)
                End Set
            End Property

            Public Shadows Property Item(ByVal ThemeID As String) As ThemeInfo
                Get
                    Dim rThemeInfo As ThemeInfo = Nothing

                    For Each tI As ThemeInfo In Me
                        If String.Compare(ThemeID, tI.ThemeID, True) = 0 Then
                            rThemeInfo = tI

                            Exit For
                        End If
                    Next

                    Return rThemeInfo
                End Get
                Set(ByVal Value As ThemeInfo)
                    Me.Remove(Value.ThemeID)
                    Me.Add(Value)
                End Set
            End Property

            Public Shadows Function ToArray() As ThemeInfo()
                Return CType(MyBase.ToArray(GetType(ThemeInfo)), ThemeInfo())
            End Function
        End Class

        Public Class URLQueryInfo
            Public ID As String
            Public Value As String

            Public Sub New(ByVal ID As String, ByVal Value As String)
                Me.ID = ID
                Me.Value = Value
            End Sub

            Public Overloads Shared Function ResolveQueryItems() As URLQueryInfo()
                Return URLQueryInfo.ResolveQueryItems( _
                        General.Context.Request.ServerVariables("QUERY_STRING"))
            End Function

            Public Overloads Shared Function ResolveQueryItems(ByVal QueryString As String) As URLQueryInfo()
                Dim rQSs As New Generic.List(Of URLQueryInfo)

                If Not String.IsNullOrEmpty(QueryString) Then
                    Dim ID As String, Value As String
                    For Each qSItem As String In QueryString.Split("&"c)
                        ID = qSItem.Split("="c)(0)
                        Value = String.Join("=", qSItem.Split("="c), 1, qSItem.Split("="c).Length - 1)

                        rQSs.Add( _
                            New URLQueryInfo(ID, Value) _
                        )
                    Next
                End If

                Return rQSs.ToArray()
            End Function
        End Class

        Public Class URLMappingInfos
            Private Shared _CurrentInstance As URLMappingInfos = Nothing

            Private _URLMapping As Boolean
            Private _URLMappingItems As PGlobals.URLMappingInfos.URLMappingItem.URLMappingItemCollection

            Public Sub New()
                Me._URLMapping = False
                Me._URLMappingItems = New URLMappingItem.URLMappingItemCollection

                URLMappingInfos._CurrentInstance = Me
            End Sub

            Public Property URLMapping() As Boolean
                Get
                    Return Me._URLMapping
                End Get
                Set(ByVal value As Boolean)
                    Me._URLMapping = value
                End Set
            End Property

            Public ReadOnly Property URLMappingItems() As URLMappingItem.URLMappingItemCollection
                Get
                    Return Me._URLMappingItems
                End Get
            End Property

            Public Shared ReadOnly Property Current As URLMappingInfos
                Get
                    Return URLMappingInfos._CurrentInstance
                End Get
            End Property

            Public Function ResolveMappedURL(ByVal RequestFilePath As String) As ResolvedMapped
                Dim rResolvedMapped As ResolvedMapped = Nothing

                If Me.URLMapping Then
                    Dim TemplateID As String = Nothing, QueryString As New Generic.List(Of PGlobals.URLQueryInfo)

                    Dim IsResolved As Boolean = False
                    Dim rqMatch As System.Text.RegularExpressions.Match = Nothing
                    For Each mItem As URLMappingItem In Me.URLMappingItems
                        rqMatch = System.Text.RegularExpressions.Regex.Match(RequestFilePath, mItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then
                            IsResolved = True
                            TemplateID = mItem.ResolveInfo.TemplateID

                            Dim medItemValue As String
                            For Each medItem As ResolveInfos.MappedItem In mItem.ResolveInfo.MappedItems
                                medItemValue = String.Empty

                                If Not String.IsNullOrEmpty(medItem.ID) Then medItemValue = rqMatch.Groups.Item(medItem.ID).Value

                                QueryString.Add( _
                                    New PGlobals.URLQueryInfo( _
                                        medItem.QueryStringKey, _
                                        CType(IIf(String.IsNullOrEmpty(medItemValue), medItem.DefaultValue, medItemValue), String) _
                                    ) _
                                )
                            Next

                            Exit For
                        End If
                    Next

                    rResolvedMapped = New ResolvedMapped(IsResolved, TemplateID)
                    rResolvedMapped.QueryStrings.AddRange(QueryString.ToArray())
                End If

                Return rResolvedMapped
            End Function

            Public Class URLMappingItem
                Private _Overridable As Boolean
                Private _Priority As Integer
                Private _RequestMap As String
                Private _ResolveInfo As ResolveInfos

                Public Sub New()
                    Me._Overridable = False
                    Me._Priority = 0
                    Me._RequestMap = String.Empty
                End Sub

                Public Property [Overridable]() As Boolean
                    Get
                        Return Me._Overridable
                    End Get
                    Set(ByVal value As Boolean)
                        Me._Overridable = value
                    End Set
                End Property

                Public Property Priority() As Integer
                    Get
                        Return Me._Priority
                    End Get
                    Set(ByVal value As Integer)
                        Me._Priority = value
                    End Set
                End Property

                Public Property RequestMap() As String
                    Get
                        Return Me._RequestMap
                    End Get
                    Set(ByVal Value As String)
                        Me._RequestMap = Value
                    End Set
                End Property

                Public Property ResolveInfo() As ResolveInfos
                    Get
                        Return Me._ResolveInfo
                    End Get
                    Set(ByVal value As ResolveInfos)
                        Me._ResolveInfo = value
                    End Set
                End Property

                Public Class URLMappingItemCollection
                    Inherits Generic.List(Of URLMappingItem)

                    Public Sub New()
                        MyBase.New()
                    End Sub

                    Public Shadows Sub Add(ByVal item As URLMappingItem)
                        MyBase.Add(item)
                        Me.Sort()
                    End Sub

                    Public Shadows Sub AddRange(ByVal Collection As Generic.IEnumerable(Of URLMappingItem))
                        MyBase.AddRange(Collection)
                        Me.Sort()
                    End Sub

                    Public Shadows Sub Sort()
                        MyBase.Sort(New PriorityComparer())
                    End Sub

                    Private Class PriorityComparer
                        Implements System.Collections.Generic.IComparer(Of URLMappingItem)

                        Public Function Compare(ByVal x As URLMappingItem, ByVal y As URLMappingItem) As Integer Implements System.Collections.Generic.IComparer(Of URLMappingItem).Compare
                            Dim rCR As Integer

                            If x.Priority > y.Priority Then rCR = -1
                            If x.Priority = y.Priority Then rCR = 0
                            If x.Priority < y.Priority Then rCR = 1

                            Return rCR
                        End Function
                    End Class
                End Class
            End Class

            Public Class ResolvedMapped
                Private _IsResolved As Boolean

                Private _TemplateID As String
                Private _QueryString As Generic.List(Of PGlobals.URLQueryInfo)

                Public Sub New(ByVal IsResolved As Boolean, ByVal TemplateID As String)
                    Me._IsResolved = IsResolved

                    Me._TemplateID = TemplateID
                    Me._QueryString = New Generic.List(Of PGlobals.URLQueryInfo)
                End Sub

                Public ReadOnly Property IsResolved() As Boolean
                    Get
                        Return Me._IsResolved
                    End Get
                End Property

                Public ReadOnly Property TemplateID() As String
                    Get
                        Return Me._TemplateID
                    End Get
                End Property

                Public ReadOnly Property QueryStrings() As Generic.List(Of PGlobals.URLQueryInfo)
                    Get
                        Return Me._QueryString
                    End Get
                End Property
            End Class

            Public Class ResolveInfos
                Private _TemplateID As String
                Private _MapFormat As String
                Private _MappedItems As MappedItem.MappedItemCollection

                Public Sub New(ByVal TemplateID As String)
                    Me._TemplateID = TemplateID
                    Me._MapFormat = String.Empty
                    Me._MappedItems = New MappedItem.MappedItemCollection
                End Sub

                Public ReadOnly Property TemplateID() As String
                    Get
                        Return Me._TemplateID
                    End Get
                End Property

                Public Property MapFormat() As String
                    Get
                        Return Me._MapFormat
                    End Get
                    Set(ByVal value As String)
                        Me._MapFormat = value
                    End Set
                End Property

                Public ReadOnly Property MappedItems() As MappedItem.MappedItemCollection
                    Get
                        Return Me._MappedItems
                    End Get
                End Property

                Public Class MappedItem
                    Private _ID As String
                    Private _DefaultValue As String
                    Private _QueryStringKey As String

                    Public Sub New(ByVal ID As String)
                        Me._ID = ID
                        Me._DefaultValue = String.Empty
                        Me._QueryStringKey = ID
                    End Sub

                    Public ReadOnly Property ID() As String
                        Get
                            Return Me._ID
                        End Get
                    End Property

                    Public Property DefaultValue() As String
                        Get
                            Return Me._DefaultValue
                        End Get
                        Set(ByVal value As String)
                            Me._DefaultValue = value
                        End Set
                    End Property

                    Public Property QueryStringKey() As String
                        Get
                            Return Me._QueryStringKey
                        End Get
                        Set(ByVal value As String)
                            Me._QueryStringKey = value
                        End Set
                    End Property

                    Public Class MappedItemCollection
                        Inherits Generic.List(Of MappedItem)

                        Public Sub New()
                            MyBase.New()
                        End Sub

                        Public Shadows ReadOnly Property Item(ByVal ID As String) As MappedItem
                            Get
                                Dim rMappedItem As MappedItem = _
                                    New MappedItem(ID)

                                For Each medItem As MappedItem In Me
                                    If String.Compare(medItem.ID, ID, True) = 0 Then
                                        rMappedItem = medItem

                                        Exit For
                                    End If
                                Next

                                Return rMappedItem
                            End Get
                        End Property
                    End Class
                End Class
            End Class
        End Class

        Public Class MapControls
            Public Class SecurityControlResult
                Inherits Object

                Private _SecurityResult As Results

                Public Sub New(ByVal SecurityResult As Results)
                    Me._SecurityResult = SecurityResult
                End Sub

                Public Enum Results As Byte
                    None = 0
                    [ReadOnly] = 1
                    ReadWrite = 2
                End Enum

                Public ReadOnly Property SecurityResult() As Results
                    Get
                        Return Me._SecurityResult
                    End Get
                End Property
            End Class

            Public Class DirectDataReader
                Inherits Object

                Private _MessageResult As MessageResult
                Private _DBCommand As IDbCommand

                Public Sub New()
                    Me._MessageResult = Nothing
                    Me._DBCommand = Nothing
                End Sub

                Public Property MessageResult() As MessageResult
                    Get
                        Return Me._MessageResult
                    End Get
                    Set(ByVal Value As MessageResult)
                        Me._MessageResult = Value
                    End Set
                End Property

                Public Property DatabaseCommand() As IDbCommand
                    Get
                        Return Me._DBCommand
                    End Get
                    Set(ByVal Value As IDbCommand)
                        Me._DBCommand = Value

                        If Not Me._DBCommand Is Nothing Then
                            If Me._DBCommand.Connection Is Nothing Then Throw New Exception("Connection Parameter of Database Command must available and valid!")
                            If String.IsNullOrEmpty(Me._DBCommand.CommandText) Then Throw New Exception("CommandText Parameter of Database Command must available and valid!")
                        End If
                    End Set
                End Property
            End Class

            Public Class PartialDataTable
                Inherits Object

                Private _MessageResult As MessageResult
                Private _ThisContainer As DataTable
                Private _TotalCount As Integer

                Public Sub New()
                    Me._MessageResult = Nothing
                    Me._ThisContainer = New DataTable
                End Sub

                Public Property MessageResult() As MessageResult
                    Get
                        Return Me._MessageResult
                    End Get
                    Set(ByVal Value As MessageResult)
                        Me._MessageResult = Value

                        Me._ThisContainer = New DataTable
                        Me._TotalCount = 0
                    End Set
                End Property

                Public Property ThisContainer() As DataTable
                    Get
                        Return Me._ThisContainer
                    End Get
                    Set(ByVal Value As DataTable)
                        Me._ThisContainer = Value.Copy()
                    End Set
                End Property

                Public Property TotalCount() As Integer
                    Get
                        If Me._TotalCount = 0 Then Me._TotalCount = Me.ThisContainer.Rows.Count

                        Return Me._TotalCount
                    End Get
                    Set(ByVal Value As Integer)
                        Me._TotalCount = Value
                    End Set
                End Property
            End Class

            Public Class ConditionalStatementResult
                Inherits Object

                Private _ConditionResult As Conditions

                Public Sub New(ByVal ConditionResult As Conditions)
                    Me._ConditionResult = ConditionResult
                End Sub

                Public Enum Conditions As Byte
                    [False] = 0
                    [True] = 1
                    [UnKnown] = 99
                End Enum

                Public ReadOnly Property ConditionResult() As Conditions
                    Get
                        Return Me._ConditionResult
                    End Get
                End Property
            End Class

            Public Class RedirectOrder
                Inherits Object

                Private _RedirectLocation As String

                Public Sub New(ByVal Location As String)
                    Me._RedirectLocation = Location
                End Sub

                Public ReadOnly Property Location() As String
                    Get
                        Return Me._RedirectLocation
                    End Get
                End Property
            End Class

            <Serializable()> _
            Public Class MessageResult
                Inherits Object

                Public Enum MessageTypes
                    [Error]
                    Warning
                    Success
                End Enum

                Private _Message As String
                Private _MessageType As MessageTypes

                Public Sub New(ByVal Message As String, Optional ByVal MessageType As MessageTypes = MessageTypes.Error)
                    Me._Message = Message
                    Me._MessageType = MessageType
                End Sub

                Public ReadOnly Property Message() As String
                    Get
                        Return Me._Message
                    End Get
                End Property

                Public ReadOnly Property Type() As MessageTypes
                    Get
                        Return Me._MessageType
                    End Get
                End Property
            End Class

            Public Class VariableBlockResult
                Inherits System.Collections.Generic.Dictionary(Of String, Object)

                Public Shadows Sub Add(ByVal key As String, ByVal value As Object)
                    MyBase.Remove(key)
                    MyBase.Add(key, value)
                End Sub

                Public Shadows Property Item(ByVal key As String) As Object
                    Get
                        Dim rValue As Object = Nothing

                        If MyBase.ContainsKey(key) Then _
                            rValue = MyBase.Item(key)

                        Return rValue
                    End Get
                    Set(ByVal value As Object)
                        Me.Add(key, value)
                    End Set
                End Property
            End Class
        End Class

        Public Class Execution
            Public Interface IScheduledTasks
                Delegate Sub ScheduleTaskHandler(ByVal params As Object())

                Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String
                Overloads Function RegisterScheduleTask(ByVal ScheduleCallBack As ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As TimeSpan) As String
                Sub UnRegisterScheduleTask(ByVal ScheduleID As String)

                Function PingToRemoteEndPoint() As Boolean
            End Interface

            Public Interface IVariablePool
                Function GetVariableFromPool(ByVal SessionKeyID As String, ByVal name As String) As Byte()

                Sub DoMassRegistration(ByVal SessionKeyID As String, ByVal serializedSerializableDictionary As Byte())
                Sub RegisterVariableToPool(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
                'Sub RegisterVariableToPoolAsync(ByVal SessionKeyID As String, ByVal name As String, ByVal serializedValue As Byte())
                Sub UnRegisterVariableFromPool(ByVal SessionKeyID As String, ByVal name As String)
                'Sub UnRegisterVariableFromPoolAsync(ByVal SessionKeyID As String, ByVal name As String)

                Sub TransferRegistrations(ByVal FromSessionKeyID As String, ByVal CurrentSessionKeyID As String)

                Sub ConfirmRegistrations(ByVal SessionKeyID As String)
                Sub DoCleanUp()

                Function PingToRemoteEndPoint() As Boolean
            End Interface

            Public Overloads Shared Function CrossCall(ByVal DllName As String, ByVal ClassName As String, ByVal FunctionName As String, ByVal FunctionParameters As FunctionParameter.FunctionParameterCollection) As Object
                Dim rMethodResult As Object = Nothing
                Dim WebManagerAsm As System.Reflection.Assembly, objAssembly As System.Type

                WebManagerAsm = System.Reflection.Assembly.Load("WebManagers")
                objAssembly = WebManagerAsm.GetType("SolidDevelopment.Web.Managers.Assembly", False, True)

                Dim AssembleInfo As String

                If FunctionParameters Is Nothing OrElse _
                    FunctionParameters.Count = 0 Then

                    AssembleInfo = String.Format("{0}?{1}.{2}", DllName, ClassName, FunctionName)
                Else
                    AssembleInfo = String.Format("{0}?{1}.{2},{3}", DllName, ClassName, FunctionName, String.Join("|", FunctionParameters.ParameterNames))
                End If

                Try
                    rMethodResult = CType( _
                                        objAssembly.InvokeMember( _
                                            "AssemblePostBackInformation", _
                                            Reflection.BindingFlags.InvokeMethod, _
                                            Nothing, _
                                            Nothing, _
                                            New Object() { _
                                                    AssembleInfo, _
                                                    Execution.GenerateArgumentInfos(FunctionParameters) _
                                                } _
                                        ),  _
                                        AssembleResultInfo _
                                    ).MethodResult
                Catch ex As Exception
                    rMethodResult = New Exception("CrossCall Execution Error!", ex)
                End Try

                Return rMethodResult
            End Function

            Public Overloads Shared Function CrossCall(ByVal AddonID As String, ByVal DllName As String, ByVal ClassName As String, ByVal FunctionName As String, ByVal FunctionParameters As FunctionParameter.FunctionParameterCollection) As Object
                Dim rMethodResult As Object = Nothing
                Dim WebManagerAsm As System.Reflection.Assembly, objAssembly As System.Type

                WebManagerAsm = System.Reflection.Assembly.Load("WebManagers")
                objAssembly = WebManagerAsm.GetType("SolidDevelopment.Web.Managers.Assembly", False, True)

                Dim AssembleInfo As String

                If FunctionParameters Is Nothing OrElse _
                    FunctionParameters.Count = 0 Then

                    AssembleInfo = String.Format("{0}?{1}.{2}", DllName, ClassName, FunctionName)
                Else
                    AssembleInfo = String.Format("{0}?{1}.{2},{3}", DllName, ClassName, FunctionName, String.Join("|", FunctionParameters.ParameterNames))
                End If

                Try
                    rMethodResult = CType( _
                                        objAssembly.InvokeMember( _
                                            "AssemblePostBackInformation", _
                                            Reflection.BindingFlags.InvokeMethod, _
                                            Nothing, _
                                            Nothing, _
                                            New Object() { _
                                                    SolidDevelopment.Web.General.CurrentThemeID, _
                                                    AddonID, _
                                                    AssembleInfo, _
                                                    Execution.GenerateArgumentInfos(FunctionParameters) _
                                                } _
                                        ),  _
                                        AssembleResultInfo _
                                    ).MethodResult
                Catch ex As Exception
                    rMethodResult = New Exception("CrossCall Execution Error!", ex)
                End Try

                Return rMethodResult
            End Function

            Private Shared Function GenerateArgumentInfos(ByVal FunctionParameters As FunctionParameter.FunctionParameterCollection) As Object
                Dim rObject As Object = Nothing

                If Not FunctionParameters Is Nothing AndAlso _
                    FunctionParameters.Count > 0 Then

                    Dim WebManagerAsm As System.Reflection.Assembly, objArgumentInfoCol As System.Type

                    WebManagerAsm = System.Reflection.Assembly.Load("WebManagers")
                    objArgumentInfoCol = WebManagerAsm.GetType("SolidDevelopment.Web.Globals+ArgumentInfo+ArgumentInfoCollection", False, True)

                    rObject = Activator.CreateInstance(objArgumentInfoCol)

                    For Each funcParam As FunctionParameter In FunctionParameters
                        rObject.GetType().InvokeMember( _
                                               "Add", _
                                               Reflection.BindingFlags.InvokeMethod, _
                                               Nothing, _
                                               rObject, _
                                               New Object() { _
                                                       funcParam.ParameterName, _
                                                       funcParam.ParameterValue _
                                                   } _
                                           )
                    Next
                End If

                Return rObject
            End Function

            Public Class FunctionParameter
                Private _ParameterName As String
                Private _ParameterValue As Object

                Public Sub New(ByVal ParamName As String, ByVal ParamValue As Object)
                    Me._ParameterName = ParamName
                    Me._ParameterValue = ParamValue
                End Sub

                Public ReadOnly Property ParameterName() As String
                    Get
                        Return Me._ParameterName
                    End Get
                End Property

                Public Property ParameterValue() As Object
                    Get
                        Return Me._ParameterValue
                    End Get
                    Set(ByVal value As Object)
                        Me._ParameterValue = value
                    End Set
                End Property

                Public Class FunctionParameterCollection
                    Inherits Collections.Generic.List(Of FunctionParameter)

                    Public Class Declaration
                        Public Sub New()
                        End Sub
                    End Class

                    Public Shared Function CreateCollection(ByVal ParamArray FunctionParameters As FunctionParameter()) As FunctionParameterCollection
                        Dim rFunctionParameterCol As New FunctionParameterCollection

                        If Not FunctionParameters Is Nothing Then
                            For Each FunctionParameter As FunctionParameter In FunctionParameters
                                rFunctionParameterCol.Add(FunctionParameter)
                            Next
                        End If

                        Return rFunctionParameterCol
                    End Function

                    Public Sub New()
                        MyBase.New()
                    End Sub

                    Public Shadows Sub Add(ByVal FuncParam As FunctionParameter)
                        Me.Add(FuncParam.ParameterName, FuncParam.ParameterValue)
                    End Sub

                    Public Shadows Sub Add(ByVal ParameterName As String)
                        Me.Add(ParameterName, New Declaration)
                    End Sub

                    Public Shadows Sub Add(ByVal ParameterName As String, ByVal ParameterValue As Object)
                        Dim IsSet As Boolean = False

                        For pC As Integer = 0 To Me.Count - 1
                            If String.Compare(Me.Item(pC).ParameterName, ParameterName, True) = 0 Then
                                Me.Item(pC).ParameterValue = ParameterValue

                                IsSet = True

                                Exit For
                            End If
                        Next

                        If Not IsSet Then MyBase.Add(New FunctionParameter(ParameterName, ParameterValue))
                    End Sub

                    Public Overloads Sub Remove(ByVal ParameterName As String)
                        For pC As Integer = 0 To Me.Count - 1
                            If String.Compare(Me.Item(pC).ParameterName, ParameterName, True) = 0 Then
                                Me.RemoveAt(pC)

                                Exit For
                            End If
                        Next
                    End Sub

                    Public Overloads Sub Remove(ByVal FuncParam As FunctionParameter)
                        For pC As Integer = 0 To Me.Count - 1
                            If String.Compare(Me.Item(pC).ParameterName, FuncParam.ParameterName, True) = 0 AndAlso _
                                Me.Item(pC).ParameterValue Is FuncParam.ParameterValue Then
                                Me.RemoveAt(pC)

                                Exit For
                            End If
                        Next
                    End Sub

                    Public ReadOnly Property ParameterNames() As String()
                        Get
                            Dim rStringList As New System.Collections.Generic.List(Of String)

                            For pC As Integer = 0 To Me.Count - 1
                                If TypeOf Me.Item(pC).ParameterValue Is Declaration Then
                                    rStringList.Add(Me.Item(pC).ParameterName.ToString())
                                Else
                                    rStringList.Add(String.Format("#{0}", Me.Item(pC).ParameterName.ToString()))
                                End If
                            Next

                            Return rStringList.ToArray()
                        End Get
                    End Property
                End Class
            End Class

            <CLSCompliant(True), Serializable()> _
            Public Class SerializableDictionary
                Inherits System.Collections.Generic.List(Of SerializableKeyValuePair)

                <Serializable()> _
                Public Class SerializableKeyValuePair
                    Private _Key As String
                    Private _Value As Byte()

                    Public Sub New(ByVal Key As String, ByVal Value As Byte())
                        Me._Key = Key
                        Me._Value = Value
                    End Sub

                    Public ReadOnly Property Key() As String
                        Get
                            Return Me._Key
                        End Get
                    End Property

                    Public ReadOnly Property Value() As Byte()
                        Get
                            Return Me._Value
                        End Get
                    End Property
                End Class
            End Class

            <CLSCompliant(True), Serializable()> _
            Public Class AssembleResultInfo
                Private _DllName As String
                Private _ClassName As String
                Private _FunctionName As String
                Private _FunctionParams As String()
                Private _MethodResult As Object

                Public Sub New(ByVal DllName As String, ByVal ClassName As String, ByVal FunctionName As String, ByVal FunctionParams As String())
                    Me._DllName = DllName
                    Me._ClassName = ClassName
                    Me._FunctionName = FunctionName
                    Me._FunctionParams = FunctionParams
                    Me._MethodResult = New Exception("Null Method Result Exception!")
                End Sub

                Public ReadOnly Property DllName() As String
                    Get
                        Return Me._DllName
                    End Get
                End Property

                Public ReadOnly Property ClassName() As String
                    Get
                        Return Me._ClassName
                    End Get
                End Property

                Public ReadOnly Property FunctionName() As String
                    Get
                        Return Me._FunctionName
                    End Get
                End Property

                Public ReadOnly Property FunctionParams() As String()
                    Get
                        Return Me._FunctionParams
                    End Get
                End Property

                Public Property MethodResult() As Object
                    Get
                        Return Me._MethodResult
                    End Get
                    Set(ByVal Value As Object)
                        Me._MethodResult = Value
                    End Set
                End Property
            End Class

            Public Shared Sub ExamMethodExecuted(ByRef MethodResult As Object, ByRef IsDone As Boolean)
                If Not MethodResult Is Nothing AndAlso _
                    TypeOf MethodResult Is Exception Then

                    IsDone = False
                Else
                    IsDone = True
                End If
            End Sub

            Public Shared Function GetPrimitiveValue(ByRef MethodResult As Object) As String
                Dim rString As String = Nothing

                If Not MethodResult Is Nothing AndAlso _
                    ( _
                        MethodResult.GetType().IsPrimitive OrElse _
                        TypeOf MethodResult Is String _
                    ) Then

                    rString = CType(MethodResult, String)
                End If

                Return rString
            End Function
        End Class

        Public Class Helpers
            Public Class WebServiceExecuteParameters
                Private _PublicKey As String
                Private _VariableHash As Hashtable
                Private _ParamXML As String

                Public Sub New()
                    Me._PublicKey = Nothing
                    Me._VariableHash = New Hashtable
                    Me._ParamXML = Nothing
                End Sub

                Public Property ExecuteParametersXML() As String
                    Get
                        Return Me.RenderExecuteParametersXML()
                    End Get
                    Set(ByVal value As String)
                        Me.ParseExecuteParametersXML(value)
                    End Set
                End Property

                Private Function RenderExecuteParametersXML() As String
                    Dim xmlStream As New IO.StringWriter()
                    Dim xmlWriter As New Xml.XmlTextWriter(xmlStream)

                    Dim Enumerator As IDictionaryEnumerator = _
                        Me._VariableHash.GetEnumerator()

                    ' Start Document Element
                    xmlWriter.WriteStartElement("ServiceParameters")

                    If Not Me._PublicKey Is Nothing Then
                        xmlWriter.WriteStartElement("Item")
                        xmlWriter.WriteAttributeString("key", "PublicKey")

                        xmlWriter.WriteString(Me._PublicKey)
                        xmlWriter.WriteEndElement()
                    End If

                    Do While Enumerator.MoveNext()
                        xmlWriter.WriteStartElement("Item")
                        xmlWriter.WriteAttributeString("key", CType(Enumerator.Key, String))

                        xmlWriter.WriteCData(CType(Enumerator.Value, String))
                        xmlWriter.WriteEndElement()
                    Loop

                    ' End Document Element
                    xmlWriter.WriteEndElement()

                    xmlWriter.Flush()
                    xmlWriter.Close()
                    xmlStream.Close()

                    Return xmlStream.ToString()
                End Function

                Private Sub ParseExecuteParametersXML(ByVal DataXML As String)
                    Me._VariableHash.Clear()

                    If Not String.IsNullOrEmpty(DataXML) Then
                        Try
                            Dim xPathTextReader As IO.StringReader
                            Dim xPathDoc As Xml.XPath.XPathDocument = Nothing

                            xPathTextReader = New IO.StringReader(DataXML)
                            xPathDoc = New Xml.XPath.XPathDocument(xPathTextReader)

                            Dim xPathNavigator As Xml.XPath.XPathNavigator
                            Dim xPathIter As Xml.XPath.XPathNodeIterator

                            xPathNavigator = xPathDoc.CreateNavigator()
                            xPathIter = xPathNavigator.Select("/ServiceParameters/Item")

                            Dim Key As String, Value As String

                            Do While xPathIter.MoveNext()
                                Key = xPathIter.Current.GetAttribute("key", xPathIter.Current.NamespaceURI)
                                Value = xPathIter.Current.Value

                                If String.Compare(Key, "PublicKey") = 0 Then
                                    Me._PublicKey = Value
                                Else
                                    Me.AddParameter(Key, Value)
                                End If
                            Loop

                            ' Close Reader
                            xPathTextReader.Close()

                            ' Garbage Collection Cleanup
                            GC.SuppressFinalize(xPathTextReader)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        End Try
                    End If
                End Sub

                Public Sub AddParameter(ByVal ParamID As String, ByVal ParamValue As String)
                    If Me._VariableHash.ContainsKey(ParamID) Then
                        Me._VariableHash.Item(ParamID) = ParamValue
                    Else
                        Me._VariableHash.Add(ParamID, ParamValue)
                    End If
                End Sub

                Public Sub RemoveParameter(ByVal ParamID As String)
                    If Me._VariableHash.ContainsKey(ParamID) Then Me._VariableHash.Remove(ParamID)
                End Sub

                Public Property PublicKey() As String
                    Get
                        Return Me._PublicKey
                    End Get
                    Set(ByVal value As String)
                        Me._PublicKey = value
                    End Set
                End Property

                Public Property Item(ByVal ParamID As String) As String
                    Get
                        Return CType(Me._VariableHash.Item(ParamID), String)
                    End Get
                    Set(ByVal value As String)
                        Me.AddParameter(ParamID, value)
                    End Set
                End Property
            End Class

            Public Class OverrideBinder
                Inherits System.Runtime.Serialization.SerializationBinder

                Private Shared _AssemblyCache As New Generic.Dictionary(Of String, System.Reflection.Assembly)

                Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As System.Type
                    Dim typeToDeserialize As Type = Nothing

                    Dim sShortAssemblyName As String = _
                        assemblyName.Substring(0, assemblyName.IndexOf(","c))

                    If OverrideBinder._AssemblyCache.ContainsKey(sShortAssemblyName) Then
                        typeToDeserialize = Me.GetDeserializeType(OverrideBinder._AssemblyCache.Item(sShortAssemblyName), typeName)
                    Else
                        Dim ayAssemblies As System.Reflection.Assembly() = AppDomain.CurrentDomain.GetAssemblies()

                        For Each ayAssembly As System.Reflection.Assembly In ayAssemblies
                            If sShortAssemblyName = ayAssembly.FullName.Substring(0, assemblyName.IndexOf(","c)) Then
                                OverrideBinder._AssemblyCache.Add(sShortAssemblyName, ayAssembly)

                                typeToDeserialize = Me.GetDeserializeType(ayAssembly, typeName)

                                Exit For
                            End If
                        Next
                    End If

                    Return typeToDeserialize
                End Function

                Private Function GetDeserializeType(ByVal assembly As System.Reflection.Assembly, typeName As String) As Type
                    Dim rTypeToDeserialize As Type = Nothing

                    Dim remainAssemblyNames As String() = Nothing
                    Dim typeName_L As String = Me.GetTypeFullNames(typeName, remainAssemblyNames)

                    Dim tempType As System.Type = _
                        assembly.GetType(typeName_L)

                    If Not tempType Is Nothing AndAlso tempType.IsGenericType Then
                        Dim typeParameters As New Generic.List(Of Type)

                        For Each remainAssemblyName As String In remainAssemblyNames
                            Dim eBI As Integer = _
                                remainAssemblyName.LastIndexOf("]"c)
                            Dim qAssemblyName As String, qTypeName As String

                            If eBI = -1 Then
                                qTypeName = remainAssemblyName.Split(","c)(0)
                                qAssemblyName = remainAssemblyName.Substring(qTypeName.Length + 2)
                            Else
                                qTypeName = remainAssemblyName.Substring(0, eBI + 1)
                                qAssemblyName = remainAssemblyName.Substring(eBI + 3)
                            End If

                            typeParameters.Add(Me.BindToType(qAssemblyName, qTypeName))
                        Next

                        rTypeToDeserialize = tempType.MakeGenericType(typeParameters.ToArray())
                    Else
                        rTypeToDeserialize = tempType
                    End If

                    Return rTypeToDeserialize
                End Function

                Private Function GetTypeFullNames(ByVal typeName As String, ByRef remainAssemblyNames As String()) As String
                    Dim rString As String

                    Dim bI As Integer = typeName.IndexOf("["c, 0)

                    If bI = -1 Then
                        rString = typeName
                        remainAssemblyNames = New String() {}
                    Else
                        rString = typeName.Substring(0, bI)

                        Dim fullNameList_L As New Generic.List(Of String)
                        Dim remainFullName As String = _
                            typeName.Substring(bI + 1, typeName.Length - (bI + 1) - 1)

                        Dim eI As Integer = 0, bIc As Integer = 0 : bI = 0
                        Do
                            bI = remainFullName.IndexOf("["c, bI)

                            If bI > -1 Then
                                eI = remainFullName.IndexOf("]"c, bI + 1)
                                bIc = remainFullName.IndexOf("["c, bI + 1)

                                If bIc > -1 AndAlso bIc < eI Then
                                    Do While bIc > -1 AndAlso bIc < eI
                                        bIc = remainFullName.IndexOf("["c, bIc + 1)

                                        If bIc > -1 AndAlso bIc < eI Then _
                                            eI = remainFullName.IndexOf("]"c, eI + 1)
                                    Loop

                                    eI = remainFullName.IndexOf("]"c, eI + 1)
                                End If

                                fullNameList_L.Add(remainFullName.Substring(bI + 1, eI - (bI + 1)))

                                bI = eI + 1
                            End If
                        Loop Until bI = -1

                        remainAssemblyNames = fullNameList_L.ToArray()
                    End If

                    Return rString
                End Function
            End Class
        End Class

    End Class

    Public Class General
        Private Shared _SiteTitles As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared Property SiteTitle() As String
            Get
                Dim rString As String = Nothing

                If General._SiteTitles.ContainsKey(General.CurrentRequestID) Then _
                    rString = CType(General._SiteTitles.Item(General.CurrentRequestID), String)

                Return rString
            End Get
            Set(ByVal value As String)
                System.Threading.Monitor.Enter(General._SiteTitles.SyncRoot)
                Try
                    If General._SiteTitles.ContainsKey(General.CurrentRequestID) Then _
                        General._SiteTitles.Remove(General.CurrentRequestID)
                    General._SiteTitles.Add(General.CurrentRequestID, value)
                Finally
                    System.Threading.Monitor.Exit(General._SiteTitles.SyncRoot)
                End Try
            End Set
        End Property

        Private Shared _Favicons As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared Property SiteIconURL() As String
            Get
                Dim rString As String = Nothing

                If General._Favicons.ContainsKey(General.CurrentRequestID) Then _
                    rString = CType(General._Favicons.Item(General.CurrentRequestID), String)

                Return rString
            End Get
            Set(ByVal value As String)
                System.Threading.Monitor.Enter(General._Favicons.SyncRoot)
                Try
                    If General._Favicons.ContainsKey(General.CurrentRequestID) Then _
                        General._Favicons.Remove(General.CurrentRequestID)
                    General._Favicons.Add(General.CurrentRequestID, value)
                Finally
                    System.Threading.Monitor.Exit(General._Favicons.SyncRoot)
                End Try
            End Set
        End Property

        Public Class MetaRecords
            Public Shared _MetaRecords As Hashtable = Hashtable.Synchronized(New Hashtable)
            Public Shared _CustomMetaRecords As Hashtable = Hashtable.Synchronized(New Hashtable)

            Public Enum MetaTags As Byte
                author = 1
                cachecontrol = 2
                contentlanguage = 3
                contenttype = 4
                copyright = 5
                description = 6
                expires = 7
                keywords = 8
                pragma = 9
                refresh = 10
                robots = 11
                googlebot = 12
            End Enum

            Public Enum MetaTagSpace As Byte
                name = 1
                httpequiv = 2
            End Enum

            Public Shared Function GetMetaTagHtmlName(ByVal mT As MetaTags) As String
                Dim rString As String = String.Empty

                Select Case mT
                    Case MetaTags.author
                        rString = "Author"
                    Case MetaTags.cachecontrol
                        rString = "Cache-Control"
                    Case MetaTags.contentlanguage
                        rString = "Content-Language"
                    Case MetaTags.contenttype
                        rString = "Content-Type"
                    Case MetaTags.copyright
                        rString = "Copyright"
                    Case MetaTags.description
                        rString = "Description"
                    Case MetaTags.expires
                        rString = "Expires"
                    Case MetaTags.googlebot
                        rString = "Googlebot"
                    Case MetaTags.keywords
                        rString = "Keywords"
                    Case MetaTags.pragma
                        rString = "Pragma"
                    Case MetaTags.refresh
                        rString = "Refresh"
                    Case MetaTags.robots
                        rString = "Robots"
                End Select

                Return rString
            End Function

            Public Shared Sub AddCustomMetaRecord(ByVal mTS As MetaTagSpace, ByVal name As String, ByVal value As String)
                System.Threading.Monitor.Enter(MetaRecords._CustomMetaRecords.SyncRoot)
                Try
                    Select Case mTS
                        Case MetaTagSpace.name
                            name = String.Format("name::{0}", name)
                        Case MetaTagSpace.httpequiv
                            name = String.Format("httpequiv::{0}", name)
                    End Select

                    Dim cMRs As Generic.Dictionary(Of String, String)

                    If MetaRecords._CustomMetaRecords.ContainsKey(General.CurrentRequestID) Then
                        cMRs = CType(MetaRecords._CustomMetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of String, String))
                    Else
                        cMRs = New Generic.Dictionary(Of String, String)
                    End If

                    If cMRs.ContainsKey(name) Then cMRs.Remove(name)
                    cMRs.Add(name, value)

                    MetaRecords._CustomMetaRecords.Item(General.CurrentRequestID) = cMRs
                Finally
                    System.Threading.Monitor.Exit(MetaRecords._CustomMetaRecords.SyncRoot)
                End Try
            End Sub

            Public Shared Sub RemoveCustomMetaRecord(ByVal name As String)
                System.Threading.Monitor.Enter(MetaRecords._CustomMetaRecords.SyncRoot)
                Try
                    Dim cMRs As Generic.Dictionary(Of String, String)

                    If MetaRecords._CustomMetaRecords.ContainsKey(General.CurrentRequestID) Then
                        cMRs = CType(MetaRecords._CustomMetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of String, String))
                    Else
                        cMRs = New Generic.Dictionary(Of String, String)
                    End If

                    If cMRs.ContainsKey(name) Then cMRs.Remove(name)

                    MetaRecords._CustomMetaRecords.Item(General.CurrentRequestID) = cMRs
                Finally
                    System.Threading.Monitor.Exit(MetaRecords._MetaRecords.SyncRoot)
                End Try
            End Sub

            Public Shared Sub AddMetaRecord(ByVal mT As MetaTags, ByVal value As String)
                System.Threading.Monitor.Enter(MetaRecords._MetaRecords.SyncRoot)
                Try
                    Dim mRs As Generic.Dictionary(Of MetaTags, String)

                    If MetaRecords._MetaRecords.ContainsKey(General.CurrentRequestID) Then
                        mRs = CType(MetaRecords._MetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                    Else
                        mRs = New Generic.Dictionary(Of MetaTags, String)
                    End If

                    If mRs.ContainsKey(mT) Then mRs.Remove(mT)
                    mRs.Add(mT, value)

                    MetaRecords._MetaRecords.Item(General.CurrentRequestID) = mRs
                Finally
                    System.Threading.Monitor.Exit(MetaRecords._MetaRecords.SyncRoot)
                End Try
            End Sub

            Public Shared Sub RemoveMetaRecord(ByVal mT As MetaTags)
                System.Threading.Monitor.Enter(MetaRecords._MetaRecords.SyncRoot)
                Try
                    Dim mRs As Generic.Dictionary(Of MetaTags, String)

                    If MetaRecords._MetaRecords.ContainsKey(General.CurrentRequestID) Then
                        mRs = CType(MetaRecords._MetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                    Else
                        mRs = New Generic.Dictionary(Of MetaTags, String)
                    End If

                    If mRs.ContainsKey(mT) Then mRs.Remove(mT)

                    MetaRecords._MetaRecords.Item(General.CurrentRequestID) = mRs
                Finally
                    System.Threading.Monitor.Exit(MetaRecords._MetaRecords.SyncRoot)
                End Try
            End Sub

            Public Shared ReadOnly Property RegisteredMetaRecords() As Generic.Dictionary(Of MetaTags, String)
                Get
                    Dim mRs As Generic.Dictionary(Of MetaTags, String)

                    If MetaRecords._MetaRecords.ContainsKey(General.CurrentRequestID) Then
                        mRs = CType(MetaRecords._MetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                    Else
                        mRs = New Generic.Dictionary(Of MetaTags, String)
                    End If

                    Return mRs
                End Get
            End Property

            Public Shared ReadOnly Property RegisteredCustomMetaRecords() As Generic.Dictionary(Of String, String)
                Get
                    Dim cMRs As Generic.Dictionary(Of String, String)

                    If MetaRecords._CustomMetaRecords.ContainsKey(General.CurrentRequestID) Then
                        cMRs = CType(MetaRecords._CustomMetaRecords.Item(General.CurrentRequestID), Generic.Dictionary(Of String, String))
                    Else
                        cMRs = New Generic.Dictionary(Of String, String)
                    End If

                    Return cMRs
                End Get
            End Property

            Public Overloads Shared Function QueryMetaTagSpace(ByVal mT As MetaTags) As MetaTagSpace
                Dim rMTS As MetaTagSpace

                Select Case mT
                    Case MetaTags.author, MetaTags.copyright, MetaTags.description, MetaTags.keywords, MetaTags.robots, MetaTags.googlebot
                        rMTS = MetaTagSpace.name
                    Case Else
                        rMTS = MetaTagSpace.httpequiv
                End Select

                Return rMTS
            End Function

            Public Overloads Shared Function QueryMetaTagSpace(ByRef name As String) As MetaTagSpace
                Dim rMTS As MetaTagSpace

                If name Is Nothing Then name = String.Empty

                If name.IndexOf("name::") = 0 Then
                    rMTS = MetaTagSpace.name

                    name = name.Substring(6)
                ElseIf name.IndexOf("httpequiv::") = 0 Then
                    rMTS = MetaTagSpace.httpequiv

                    name = name.Substring(11)
                End If

                Return rMTS
            End Function
        End Class

        'Public Overloads Shared Function GetPageToRedirect(ByVal TemplateID As String, ByVal CachingType As PGlobals.PageCachingTypes, ByVal ParamArray qSL As PGlobals.URLQueryInfo()) As String
        '    Return General.GetPageToRedirect(False, TemplateID, CachingType, qSL)
        'End Function

        Public Overloads Shared Function GetPageToRedirect(ByVal UseSameVariablePool As Boolean, ByVal TemplateID As String, ByVal CachingType As PGlobals.PageCachingTypes, ByVal ParamArray qSL As PGlobals.URLQueryInfo()) As String
            Dim rString As String

            Dim QueryStrings As New Generic.List(Of String)

            Select Case CachingType
                Case PGlobals.PageCachingTypes.AllContentCookiless
                    QueryStrings.Add("nocache=L0XC")
                Case PGlobals.PageCachingTypes.TextsOnly
                    QueryStrings.Add("nocache=L1")
                Case PGlobals.PageCachingTypes.TextsOnlyCookiless
                    QueryStrings.Add("nocache=L1XC")
                Case PGlobals.PageCachingTypes.NoCache
                    QueryStrings.Add("nocache=L2")
                Case PGlobals.PageCachingTypes.NoCacheCookiless
                    QueryStrings.Add("nocache=L2XC")
            End Select

            If Not qSL Is Nothing Then
                For Each qS As PGlobals.URLQueryInfo In qSL
                    QueryStrings.Add(String.Format("{0}={1}", qS.ID, qS.Value))
                Next
            End If

            If Not UseSameVariablePool Then
                rString = String.Format("{0}{1}", Configurations.ApplicationRoot.BrowserSystemImplementation, TemplateID)
            Else
                rString = String.Format("{0}{1}/{2}", Configurations.ApplicationRoot.BrowserSystemImplementation, General.HashCode, TemplateID)
            End If

            If QueryStrings.Count > 0 Then _
                rString = String.Concat(rString, "?", String.Join("&", QueryStrings.ToArray()))

            Return rString
        End Function

        Public Overloads Shared Function GetThemePublicContentsPath() As String
            Return General.GetThemePublicContentsPath(General.CurrentThemeID, General.CurrentThemeTranslationID, General.CurrentInstantAddonID)
        End Function

        Public Overloads Shared Function GetThemePublicContentsPath(ByVal ThemeID As String, ByVal ThemeTranslationID As String, Optional ByVal AddonID As String = Nothing) As String
            Dim ThemeWebPath As String = _
                String.Format("{0}{1}_{2}", _
                    Configurations.ApplicationRoot.BrowserSystemImplementation, _
                    ThemeID, _
                    ThemeTranslationID)

            If Not AddonID Is Nothing Then ThemeWebPath = String.Format("{0}_{1}", ThemeWebPath, AddonID)

            Return ThemeWebPath
        End Function

        Public Shared Function ResolveTemplateFromURL(ByVal RequestFilePath As String) As String
            Dim RequestedTemplate As String = Nothing

            If Not String.IsNullOrEmpty(RequestFilePath) Then
                Dim SearchNotMapped As Boolean = True

                Dim URLMI As PGlobals.URLMappingInfos = _
                    PGlobals.URLMappingInfos.Current

                If URLMI.URLMapping Then
                    Dim rqMatch As System.Text.RegularExpressions.Match = Nothing
                    For Each mItem As SolidDevelopment.Web.PGlobals.URLMappingInfos.URLMappingItem In URLMI.URLMappingItems
                        rqMatch = System.Text.RegularExpressions.Regex.Match(RequestFilePath, mItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then
                            RequestedTemplate = mItem.ResolveInfo.TemplateID
                            SearchNotMapped = False

                            Exit For
                        End If
                    Next
                End If

                If SearchNotMapped Then
                    ' Manage Query String
                    RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserSystemImplementation) + Configurations.ApplicationRoot.BrowserSystemImplementation.Length)

                    Dim mR As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(RequestFilePath, "\d+/")

                    If mR.Success AndAlso mR.Index = 0 Then _
                        RequestFilePath = RequestFilePath.Substring(mR.Length)

                    If RequestFilePath.IndexOf("?"c) > -1 Then
                        RequestFilePath = RequestFilePath.Substring(0, RequestFilePath.IndexOf("?"c))
                    Else
                        If RequestFilePath.Length = 0 Then
                            RequestFilePath = String.Empty
                        ElseIf RequestFilePath.IndexOf("/"c) > -1 Then
                            RequestFilePath = Nothing
                        Else
                            ' If requested path is like this /VDIR/APPPATH/392901
                            ' it is probably wrong string passed
                            ' but never the less do not fix
                        End If
                    End If

                    RequestedTemplate = RequestFilePath
                    ' !-- Manage Query String
                End If
            End If

            Return RequestedTemplate
        End Function

        Public Shared Function GetMimeType(ByVal FileExtension As String) As String
            Dim rString As Object = "application/octet-stream"

            Try
                Dim HelperAsm As System.Reflection.Assembly, objHelper As System.Type
                Dim HelperInstance As Object = Nothing

                HelperAsm = System.Reflection.Assembly.Load("WebManagers")
                objHelper = HelperAsm.GetType("SolidDevelopment.Web.Helpers.RegistryManager")

                For Each _ctorInfo As System.Reflection.ConstructorInfo In objHelper.GetConstructors()
                    If _ctorInfo.IsConstructor AndAlso _
                        _ctorInfo.IsPublic AndAlso _
                        _ctorInfo.GetParameters().Length = 1 Then

                        HelperInstance = _ctorInfo.Invoke(New Object() {0})

                        Exit For
                    End If
                Next

                If Not HelperInstance Is Nothing Then
                    Dim AccessPathProp As System.Reflection.PropertyInfo
                    AccessPathProp = HelperInstance.GetType().GetProperty("AccessPath", GetType(String))
                    AccessPathProp.SetValue(HelperInstance, FileExtension, Nothing)

                    rString = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Content Type", Object)})

                    If rString Is Nothing Then rString = "application/octet-stream"
                End If
            Catch ex As Exception
                ' Do Nothing Just Handle Exception
            End Try

            Return rString.ToString()
        End Function

        Public Shared Function GetExtensionFromMimeType(ByVal MimeType As String) As String
            Dim rString As Object = ".dat"

            Try
                Dim HelperAsm As System.Reflection.Assembly, objHelper As System.Type
                Dim HelperInstance As Object = Nothing

                HelperAsm = System.Reflection.Assembly.Load("WebManagers")
                objHelper = HelperAsm.GetType("SolidDevelopment.Web.Helpers.RegistryManager")

                For Each _ctorInfo As System.Reflection.ConstructorInfo In objHelper.GetConstructors()
                    If _ctorInfo.IsConstructor AndAlso _
                        _ctorInfo.IsPublic AndAlso _
                        _ctorInfo.GetParameters().Length = 1 Then

                        HelperInstance = _ctorInfo.Invoke(New Object() {0})

                        Exit For
                    End If
                Next

                If Not HelperInstance Is Nothing Then
                    Dim AccessPathProp As System.Reflection.PropertyInfo
                    AccessPathProp = HelperInstance.GetType().GetProperty("AccessPath", GetType(String))
                    AccessPathProp.SetValue(HelperInstance, String.Format("Mime\Database\Content Type\{0}", MimeType), Nothing)

                    rString = HelperInstance.GetType().GetMethod("GetRegistryValue").Invoke(HelperInstance, New Object() {CType("Extension", Object)})

                    If rString Is Nothing Then rString = ".dat"
                End If
            Catch ex As Exception
                ' Do Nothing Just Handle Exception
            End Try

            Return rString.ToString()
        End Function

        Public Shared Function ProvidePublicContentFileStream(ByRef OutputStream As IO.Stream, ByVal FileName As String) As Boolean
            Dim rBoolean As Boolean = True

            Dim HttpRequest As Net.HttpWebRequest
            Dim HttpResponse As Net.WebResponse = Nothing

            Dim cLoc As String = _
                        String.Format( _
                            "http://{0}{1}/{2}", _
                            General.Context.Request.ServerVariables.Item("HTTP_HOST"), _
                            General.GetThemePublicContentsPath( _
                                General.CurrentThemeID, _
                                General.CurrentThemeTranslationID, _
                                General.CurrentInstantAddonID _
                            ), _
                            FileName _
                        )
            Dim xmlUri As New Uri(cLoc)

            ' Strings
            Dim ThemeSearchString As String = _
                        String.Format("{0}_ThemeID", Configurations.VirtualRoot.Replace("/"c, "_"c))
            Dim TranslationSearchString As String = _
                        String.Format("{0}_ThemeTranslationID", Configurations.VirtualRoot.Replace("/"c, "_"c))

            ' NET Cookies
            Dim ThemeID_Cookie As New System.Net.Cookie(ThemeSearchString, General.CurrentThemeID)
            Dim ThemeTranslation_Cookie As New System.Net.Cookie(TranslationSearchString, General.CurrentThemeTranslationID)

            ' Set Cookies
            ThemeID_Cookie.Domain = General.Context.Request.ServerVariables.Item("SERVER_NAME")
            ThemeID_Cookie.Path = "/"
            ThemeID_Cookie.Expires = Date.Now().AddYears(1)

            ThemeTranslation_Cookie.Domain = General.Context.Request.ServerVariables.Item("SERVER_NAME")
            ThemeTranslation_Cookie.Path = "/"
            ThemeTranslation_Cookie.Expires = Date.Now().AddYears(1)

            ' Prepare Request
            HttpRequest = CType(Net.HttpWebRequest.Create(xmlUri), Net.HttpWebRequest)
            HttpRequest.CookieContainer = New Net.CookieContainer()
            HttpRequest.CookieContainer.Add(ThemeID_Cookie)
            HttpRequest.CookieContainer.Add(ThemeTranslation_Cookie)

            Try
                ' Do Request
                HttpResponse = HttpRequest.GetResponse()

                OutputStream = HttpResponse.GetResponseStream()
            Catch ex As Exception
                rBoolean = False
            End Try

            Return rBoolean
        End Function

        Private Shared ReadOnly Property IsCookiless() As Boolean
            Get
                Dim CookilessSessionString As String = _
                        String.Format("{0}_Cookieless", Configurations.VirtualRoot.Replace("/"c, "_"c))
                Dim _IsCookiless As Boolean = False

                Boolean.TryParse(CType(General.Context.Session.Contents.Item(CookilessSessionString), String), _IsCookiless)

                Return _IsCookiless
            End Get
        End Property

        Public Shared Property CurrentThemeID() As String
            Get
                Dim rCurrentThemeID As String = Nothing
                Dim ThemeSearchString As String = _
                        String.Format("{0}_ThemeID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If General.IsCookiless Then
                    If General.Context.Session.Contents.Item(ThemeSearchString) Is Nothing Then
                        rCurrentThemeID = Configurations.DefaultTheme

                        ' Set Default ThemeID
                        General.CurrentThemeID = rCurrentThemeID
                    Else
                        rCurrentThemeID = CType(General.Context.Session.Contents.Item(ThemeSearchString), String)
                    End If
                Else
                    If General.Context.Request.Cookies.Item(ThemeSearchString) Is Nothing OrElse _
                        General.Context.Request.Cookies.Item(ThemeSearchString).Value Is Nothing OrElse _
                        General.Context.Request.Cookies.Item(ThemeSearchString).Value.Trim().Length = 0 Then

                        rCurrentThemeID = Configurations.DefaultTheme

                        ' Set Default ThemeID
                        General.CurrentThemeID = rCurrentThemeID
                    Else
                        rCurrentThemeID = General.Context.Request.Cookies.Item(ThemeSearchString).Value
                    End If
                End If

                Return rCurrentThemeID
            End Get
            Set(ByVal Value As String)
                Dim ThemeSearchString As String = _
                        String.Format("{0}_ThemeID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If General.IsCookiless Then
                    SyncLock General.Context.Session.SyncRoot
                        Try
                            General.Context.Session.Contents.Item(ThemeSearchString) = Value
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' TODO: Must investigate SESSION "KEY ADDED" PROBLEMS
                        End Try
                    End SyncLock
                Else
                    Dim ThemeID_Cookie As New System.Web.HttpCookie(ThemeSearchString, Value)

                    ThemeID_Cookie.Expires = Date.Now().AddYears(1)
                    General.Context.Response.Cookies.Add(ThemeID_Cookie)
                End If
            End Set
        End Property

        Public Shared Property CurrentThemeTranslationID() As String
            Get
                Dim rCurrentThemeTranslation As String = Nothing

                Dim TranslationSearchString As String = _
                        String.Format("{0}_ThemeTranslationID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If General.IsCookiless Then
                    If General.Context.Session.Contents.Item(TranslationSearchString) Is Nothing Then
                        Dim ThemeInstance As PGlobals.ITheme = General.CreateThemeInstance(General.CurrentThemeID)
                        rCurrentThemeTranslation = ThemeInstance.Settings.Configurations.DefaultTranslation

                        ' Set Default TranslationID
                        General.CurrentThemeTranslationID = rCurrentThemeTranslation
                    Else
                        rCurrentThemeTranslation = CType(General.Context.Session.Contents.Item(TranslationSearchString), String)
                    End If
                Else
                    If General.Context.Request.Cookies.Item(TranslationSearchString) Is Nothing OrElse _
                        General.Context.Request.Cookies.Item(TranslationSearchString).Value Is Nothing OrElse _
                        General.Context.Request.Cookies.Item(TranslationSearchString).Value.Trim().Length = 0 Then

                        Dim ThemeInstance As PGlobals.ITheme = General.CreateThemeInstance(General.CurrentThemeID)
                        rCurrentThemeTranslation = ThemeInstance.Settings.Configurations.DefaultTranslation

                        ' Set Default TranslationID
                        General.CurrentThemeTranslationID = rCurrentThemeTranslation
                    Else
                        rCurrentThemeTranslation = General.Context.Request.Cookies.Item(TranslationSearchString).Value
                    End If
                End If

                Return rCurrentThemeTranslation
            End Get
            Set(ByVal Value As String)
                Dim TranslationSearchString As String = _
                        String.Format("{0}_ThemeTranslationID", Configurations.VirtualRoot.Replace("/"c, "_"c))

                If General.IsCookiless Then
                    SyncLock General.Context.Session.SyncRoot
                        Try
                            General.Context.Session.Contents.Item(TranslationSearchString) = Value
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' TODO: Must investigate SESSION "KEY ADDED" PROBLEMS
                        End Try
                    End SyncLock
                Else
                    Dim ThemeTranslation_Cookie As New System.Web.HttpCookie(TranslationSearchString, Value)

                    ThemeTranslation_Cookie.Expires = Date.Now().AddYears(1)
                    General.Context.Response.Cookies.Add(ThemeTranslation_Cookie)
                End If
            End Set
        End Property

        Public Shared Property CurrentInstantAddonID() As String
            Get
                Return CType( _
                            General.Context.Session.Contents.Item( _
                                    String.Format( _
                                        "_systemvars-{0}-instantaddonid", _
                                        General.Context.Session.SessionID _
                                    ) _
                                ), _
                            String _
                        )
            End Get
            Set(ByVal value As String)
                Dim SessionKey As String = String.Format( _
                                                "_systemvars-{0}-instantaddonid", _
                                                General.Context.Session.SessionID _
                                            )

                SyncLock General.Context.Session.SyncRoot
                    General.Context.Session.Contents.Remove(SessionKey)
                    If Not value Is Nothing Then
                        Try
                            General.Context.Session.Contents.Item(SessionKey) = value
                        Catch ex As Exception
                            ' Just Handle Exceptions
                            ' TODO: Must investigate SESSION "KEY ADDED" PROBLEMS
                        End Try
                    End If
                End SyncLock
            End Set
        End Property

        Public Overloads Shared Function CreateThemeInstance() As PGlobals.ITheme
            Return General.CreateThemeInstance(General.CurrentThemeID, General.CurrentThemeTranslationID, General.CurrentInstantAddonID)
        End Function

        Public Overloads Shared Function CreateThemeInstance(ByVal ThemeID As String) As PGlobals.ITheme
            Return General.CreateThemeInstance(ThemeID, Nothing)
        End Function

        Public Overloads Shared Function CreateThemeInstance(ByVal ThemeID As String, ByVal ThemeTranslationID As String) As PGlobals.ITheme
            Return General.CreateThemeInstance(ThemeID, ThemeTranslationID, Nothing)
        End Function

        Public Overloads Shared Function CreateThemeInstance(ByVal ThemeID As String, ByVal ThemeTranslationID As String, ByVal PreparedAddonID As String) As PGlobals.ITheme
            Dim rITheme As PGlobals.ITheme
            Dim ThemeAsm As System.Reflection.Assembly, objTheme As System.Type

            ThemeAsm = System.Reflection.Assembly.Load("WebManagers")
            objTheme = ThemeAsm.GetType("SolidDevelopment.Web.Managers.Theme", False, True)

            rITheme = CType(Activator.CreateInstance(objTheme, New Object() {ThemeID, ThemeTranslationID, PreparedAddonID}), PGlobals.ITheme)

            Return rITheme
        End Function

        Public Shared ReadOnly Property Themes() As PGlobals.ThemeInfoCollection
            Get
                Dim rITheme As PGlobals.ThemeInfoCollection
                Dim ThemeAsm As System.Reflection.Assembly, objTheme As System.Type

                ThemeAsm = System.Reflection.Assembly.Load("WebManagers")
                objTheme = ThemeAsm.GetType("SolidDevelopment.Web.Managers.ThemeWebControl", False, True)

                rITheme = CType(objTheme.InvokeMember("GetAvailableThemes", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, Nothing), PGlobals.ThemeInfoCollection)

                Return rITheme
            End Get
        End Property

        Public Shared ReadOnly Property Context() As System.Web.HttpContext
            Get
                Dim rContext As System.Web.HttpContext
                Dim RequestAsm As System.Reflection.Assembly, objRequest As System.Type

                RequestAsm = System.Reflection.Assembly.Load("WebHandler")
                objRequest = RequestAsm.GetType("SolidDevelopment.Web.RequestModule", False, True)

                rContext = CType(objRequest.InvokeMember("Context", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {General.CurrentRequestID}), System.Web.HttpContext)

                Return rContext
            End Get
        End Property

        Public Shared ReadOnly Property ScriptRefererURL() As String
            Get
                Dim rString As String
                Try
                    rString = CType(General.Context.Session.Contents.Item("_sys_Referer"), String)
                Catch ex As Exception
                    rString = String.Empty
                End Try

                Return rString
            End Get
        End Property

        Public Shared Function CreateThreadContext() As String
            Dim RequestAsm As System.Reflection.Assembly, objRequest As System.Type

            RequestAsm = System.Reflection.Assembly.Load("WebHandler")
            objRequest = RequestAsm.GetType("SolidDevelopment.Web.RequestModule", False, True)

            Return CType(objRequest.InvokeMember("CreateThreadContext", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {General.CurrentRequestID}), String)
        End Function

        Public Shared Sub DestroyThreadContext(ByVal ThreadRequestID As String)
            Dim RequestAsm As System.Reflection.Assembly, objRequest As System.Type

            RequestAsm = System.Reflection.Assembly.Load("WebHandler")
            objRequest = RequestAsm.GetType("SolidDevelopment.Web.RequestModule", False, True)

            objRequest.InvokeMember("DestroyThreadContext", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {ThreadRequestID})
        End Sub

        Public Shared Sub AssignRequestID(ByVal RequestID As String)
            System.AppDomain.CurrentDomain.SetData( _
                    String.Format("RequestID_{0}", _
                        System.Threading.Thread.CurrentThread.ManagedThreadId), RequestID)
        End Sub

        Public Shared ReadOnly Property CurrentRequestID() As String
            Get
                Return CType( _
                            System.AppDomain.CurrentDomain.GetData( _
                                String.Format("RequestID_{0}", _
                                    System.Threading.Thread.CurrentThread.ManagedThreadId) _
                                ), _
                            String _
                        )
            End Get
        End Property

        Public Shared ReadOnly Property HashCode() As String
            Get
                Dim _HashCode As String

                Dim RequestFilePath As String = _
                    Context.Request.FilePath

                RequestFilePath = RequestFilePath.Remove(0, RequestFilePath.IndexOf(Configurations.ApplicationRoot.BrowserSystemImplementation) + Configurations.ApplicationRoot.BrowserSystemImplementation.Length)

                Dim mR As System.Text.RegularExpressions.Match = _
                    System.Text.RegularExpressions.Regex.Match(RequestFilePath, "\d+/")

                If mR.Success AndAlso _
                    mR.Index = 0 Then

                    _HashCode = mR.Value.Substring(0, mR.Value.Length - 1)
                Else
                    _HashCode = Context.GetHashCode().ToString()
                End If

                Return _HashCode
            End Get
        End Property

        Public Shared ReadOnly Property CachingType() As PGlobals.PageCachingTypes
            Get
                Dim _CachingType As PGlobals.PageCachingTypes = _
                    PGlobals.PageCachingTypes.AllContent

                Dim URLMI As PGlobals.URLMappingInfos = _
                    PGlobals.URLMappingInfos.Current

                If URLMI.URLMapping Then
                    Dim SHCMatch As System.Text.RegularExpressions.Match = _
                        System.Text.RegularExpressions.Regex.Match(Context.Request.FilePath, "\d+(L\d(XC)?)?/")

                    If SHCMatch.Success Then
                        ' Search and set request caching variable if it's exists
                        Dim RequestCaching As String = String.Empty
                        Dim rCIdx As Integer = SHCMatch.Value.IndexOf(","c)
                        If rCIdx > -1 Then _
                            RequestCaching = SHCMatch.Value.Substring(rCIdx + 1, SHCMatch.Length - (rCIdx + 2))
                        ' !--

                        Select Case RequestCaching
                            Case "L1"
                                _CachingType = PGlobals.PageCachingTypes.TextsOnly
                            Case "L2"
                                _CachingType = PGlobals.PageCachingTypes.NoCache
                            Case "L0XC"
                                _CachingType = PGlobals.PageCachingTypes.AllContentCookiless
                            Case "L1XC"
                                _CachingType = PGlobals.PageCachingTypes.TextsOnlyCookiless
                            Case "L2XC"
                                _CachingType = PGlobals.PageCachingTypes.NoCacheCookiless
                        End Select
                    End If
                Else
                    Select Case General.Context.Request.QueryString.Item("nocache")
                        Case "L1"
                            _CachingType = PGlobals.PageCachingTypes.TextsOnly
                        Case "L2"
                            _CachingType = PGlobals.PageCachingTypes.NoCache
                        Case "L0XC"
                            _CachingType = PGlobals.PageCachingTypes.AllContentCookiless
                        Case "L1XC"
                            _CachingType = PGlobals.PageCachingTypes.TextsOnlyCookiless
                        Case "L2XC"
                            _CachingType = PGlobals.PageCachingTypes.NoCacheCookiless
                    End Select
                End If

                Return _CachingType
            End Get
        End Property

        Public Shared Function GetCachingTypeText(ByVal CachingType As PGlobals.PageCachingTypes) As String
            Dim rCachingTypeText As String = String.Empty

            Select Case CachingType
                Case PGlobals.PageCachingTypes.TextsOnly
                    rCachingTypeText = "L1"
                Case PGlobals.PageCachingTypes.NoCache
                    rCachingTypeText = "L2"
                Case PGlobals.PageCachingTypes.AllContentCookiless
                    rCachingTypeText = "L0XC"
                Case PGlobals.PageCachingTypes.TextsOnlyCookiless
                    rCachingTypeText = "L1XC"
                Case PGlobals.PageCachingTypes.NoCacheCookiless
                    rCachingTypeText = "L2XC"
            End Select

            Return rCachingTypeText
        End Function

        Public Shared Sub SetVariable(ByVal key As String, ByVal value As Object)
            If Not String.IsNullOrWhiteSpace(key) AndAlso key.Length > 128 Then _
                Throw New Exception("VariableName can not be longer than 128 characters!")

            If value Is Nothing Then
                General.VariablePool.UnRegisterVariableFromPool(key)
            Else
                General.VariablePool.RegisterVariableToPool(key, value)
            End If
        End Sub

        Public Shared Function GetVariable(ByVal key As String) As Object
            Return General.VariablePool.GetVariableFromPool(key)
        End Function

        Public Shared Sub TransferVariables(ByVal FromSessionID As String)
            General.VariablePool.TransferRegistrations( _
                    String.Format( _
                        "{0}_{1}", FromSessionID, General.HashCode) _
                )
        End Sub

        Public Shared Sub ConfirmVariables()
            Try
                General.VariablePool.ConfirmRegistrations()
                General.VariablePoolForWebService.ConfirmRegistrations()
            Catch ex As Exception
                ' Just Handle Exceptions
                '   Null Object Referance
                '   Null SessionID Parameter
            End Try
        End Sub

        Public Shared ReadOnly Property ScheduledTasks() As General.ScheduledTasksOperationsClass
            Get
                Return New ScheduledTasksOperationsClass()
            End Get
        End Property

        Public Shared ReadOnly Property VariablePool() As General.VariablePoolOperationsClass
            Get
                Return New VariablePoolOperationsClass( _
                                String.Format( _
                                    "{0}_{1}", General.Context.Session.SessionID, General.HashCode) _
                            )
            End Get
        End Property

        Public Shared ReadOnly Property VariablePoolForWebService() As General.VariablePoolOperationsClass
            Get
                Return New VariablePoolOperationsClass("000000000000000000000000_00000001")
            End Get
        End Property

        Private Shared _VariablePoolOperationCache As PGlobals.Execution.IVariablePool = Nothing
        Public NotInheritable Class VariablePoolOperationsClass
            Private _SessionKeyID As String

            Public Sub New(ByVal SessionKeyID As String)
                If General._VariablePoolOperationCache Is Nothing Then
                    Try
                        Dim RequestAsm As System.Reflection.Assembly, objRequest As System.Type

                        RequestAsm = System.Reflection.Assembly.Load("WebHandler")
                        objRequest = RequestAsm.GetType("SolidDevelopment.Web.RequestModule", False, True)

                        General._VariablePoolOperationCache = _
                            CType(objRequest.InvokeMember("VariablePool", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing), PGlobals.Execution.IVariablePool)
                    Catch ex As Exception
                        Throw New Exception("Communication Error! Variable Pool is not accessable...")
                    End Try
                End If

                Me._SessionKeyID = SessionKeyID
            End Sub

            Public Function GetVariableFromPool(ByVal name As String) As Object
                If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New Exception("SessionID must be set!")

                Dim rObject As Object = _
                    VariablePoolPreCacheClass.GetCachedVariable(Me._SessionKeyID, name)

                If rObject Is Nothing Then
                    Dim serializedValue As Byte() = _
                        General._VariablePoolOperationCache.GetVariableFromPool(Me._SessionKeyID, name)

                    If Not serializedValue Is Nothing Then
                        Dim forStream As IO.Stream = Nothing

                        Try
                            Dim binFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                            binFormater.Binder = New SolidDevelopment.Web.PGlobals.Helpers.OverrideBinder()

                            forStream = New IO.MemoryStream(serializedValue)

                            rObject = binFormater.Deserialize(forStream)

                            VariablePoolPreCacheClass.CacheVariable(Me._SessionKeyID, name, rObject)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        Finally
                            If Not forStream Is Nothing Then
                                forStream.Close()
                                GC.SuppressFinalize(forStream)
                            End If
                        End Try
                    End If
                End If

                Return rObject
            End Function

            Public Sub RegisterVariableToPool(ByVal name As String, ByVal value As Object)
                If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New Exception("SessionID must be set!")

                VariablePoolPreCacheClass.CacheVariable(Me._SessionKeyID, name, value)
            End Sub

            Public Sub UnRegisterVariableFromPool(ByVal name As String)
                VariablePoolPreCacheClass.CacheVariable(Me._SessionKeyID, name, Nothing)

                ' Unregister Variable From Pool Immidiately. 
                ' Otherwise it will cause cache reload in the same domain call
                General._VariablePoolOperationCache.UnRegisterVariableFromPool(Me._SessionKeyID, name)
            End Sub

            Public Sub TransferRegistrations(ByVal FromSessionID As String)
                If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New Exception("ToSessionID must be set!")
                If String.IsNullOrEmpty(FromSessionID) Then Throw New Exception("FromSessionID must be set!")

                General._VariablePoolOperationCache.TransferRegistrations(FromSessionID, Me._SessionKeyID)
            End Sub

            Private Function _SerializeNameValuePairs(ByVal NameValuePairs As Generic.Dictionary(Of String, Object)) As Byte()
                Dim sD As New PGlobals.Execution.SerializableDictionary

                Dim serializedValue As Byte() = New Byte() {}

                If Not NameValuePairs Is Nothing Then
                    Dim forStream As IO.Stream = Nothing

                    For Each VariableName As String In NameValuePairs.Keys
                        serializedValue = New Byte() {} : forStream = Nothing

                        Try
                            forStream = New IO.MemoryStream

                            Dim binFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                            binFormater.Serialize(forStream, NameValuePairs.Item(VariableName))

                            serializedValue = _
                                CType(forStream, IO.MemoryStream).ToArray()

                            sD.Add( _
                                New PGlobals.Execution.SerializableDictionary.SerializableKeyValuePair(VariableName, serializedValue) _
                            )
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        Finally
                            If Not forStream Is Nothing Then
                                forStream.Close()
                                GC.SuppressFinalize(forStream)
                            End If
                        End Try
                    Next

                    serializedValue = New Byte() {} : forStream = Nothing

                    Try
                        forStream = New IO.MemoryStream

                        Dim binFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                        binFormater.Serialize(forStream, sD)

                        serializedValue = _
                            CType(forStream, IO.MemoryStream).ToArray()
                    Catch ex As Exception
                        ' Just Handle Exceptions
                    Finally
                        If Not forStream Is Nothing Then
                            forStream.Close()
                            GC.SuppressFinalize(forStream)
                        End If
                    End Try
                End If

                Return serializedValue
            End Function

            Public Sub ConfirmRegistrations()
                ' CONFIRMATION HAS TO RUN ALWAYS ON MAIN THREAD. WE NEED BLOCKAGE!
                If String.IsNullOrEmpty(Me._SessionKeyID) Then Throw New Exception("SessionID must be set!")

                Dim serializedHashTable As Byte() = Nothing

                SyncLock VariablePoolPreCacheClass.VariablePreCache.SyncRoot
                    serializedHashTable = _
                        Me._SerializeNameValuePairs( _
                            VariablePoolPreCacheClass.UnsafeGetCachedSession(Me._SessionKeyID) _
                        )
                End SyncLock

                General._VariablePoolOperationCache.DoMassRegistration(Me._SessionKeyID, serializedHashTable)
                General._VariablePoolOperationCache.ConfirmRegistrations(Me._SessionKeyID)

                VariablePoolPreCacheClass.CleanCachedVariables(Me._SessionKeyID)
            End Sub
        End Class

        Private Class VariablePoolPreCacheClass
            Private Shared _VariablePreCache As Hashtable = Nothing

            Friend Shared ReadOnly Property VariablePreCache() As Hashtable
                Get
                    If VariablePoolPreCacheClass._VariablePreCache Is Nothing Then _
                        VariablePoolPreCacheClass._VariablePreCache = Hashtable.Synchronized(New Hashtable)

                    Return VariablePoolPreCacheClass._VariablePreCache
                End Get
            End Property

            Friend Shared Function UnsafeGetCachedSession(ByVal SessionKeyID As String) As Generic.Dictionary(Of String, Object)
                Dim rNameValuePairs As Generic.Dictionary(Of String, Object) = Nothing

                If VariablePoolPreCacheClass.VariablePreCache.ContainsKey(SessionKeyID) Then
                    rNameValuePairs = _
                        CType(VariablePoolPreCacheClass.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))
                End If

                Return rNameValuePairs
            End Function

            Public Shared Function GetCachedVariable(ByVal SessionKeyID As String, ByVal name As String) As Object
                Dim rObject As Object = Nothing

                SyncLock VariablePoolPreCacheClass.VariablePreCache.SyncRoot
                    If VariablePoolPreCacheClass.VariablePreCache.ContainsKey(SessionKeyID) Then
                        Dim NameValuePairs As Generic.Dictionary(Of String, Object) = _
                            CType(VariablePoolPreCacheClass.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))

                        If Not NameValuePairs Is Nothing AndAlso _
                            NameValuePairs.ContainsKey(name) Then _
                                rObject = NameValuePairs.Item(name)
                    End If
                End SyncLock

                Return rObject
            End Function

            Public Shared Sub CacheVariable(ByVal SessionKeyID As String, ByVal name As String, ByVal value As Object)
                System.Threading.Monitor.Enter(VariablePoolPreCacheClass.VariablePreCache.SyncRoot)
                Try
                    Dim NameValuePairs As Generic.Dictionary(Of String, Object) = Nothing
                    If VariablePoolPreCacheClass.VariablePreCache.ContainsKey(SessionKeyID) Then
                        NameValuePairs = CType(VariablePoolPreCacheClass.VariablePreCache.Item(SessionKeyID), Generic.Dictionary(Of String, Object))

                        If value Is Nothing Then
                            NameValuePairs.Remove(name)
                        Else
                            NameValuePairs.Item(name) = value
                        End If

                        VariablePoolPreCacheClass.VariablePreCache.Item(SessionKeyID) = NameValuePairs
                    Else
                        If Not value Is Nothing Then
                            NameValuePairs = New Generic.Dictionary(Of String, Object)
                            NameValuePairs.Add(name, value)

                            VariablePoolPreCacheClass.VariablePreCache.Add(SessionKeyID, NameValuePairs)
                        End If
                    End If
                Finally
                    System.Threading.Monitor.Exit(VariablePoolPreCacheClass.VariablePreCache.SyncRoot)
                End Try
            End Sub

            Public Shared Sub CleanCachedVariables(ByVal SessionKeyID As String)
                System.Threading.Monitor.Enter(VariablePoolPreCacheClass.VariablePreCache.SyncRoot)
                Try
                    If VariablePoolPreCacheClass.VariablePreCache.ContainsKey(SessionKeyID) Then _
                        VariablePoolPreCacheClass.VariablePreCache.Remove(SessionKeyID)
                Finally
                    System.Threading.Monitor.Exit(VariablePoolPreCacheClass.VariablePreCache.SyncRoot)
                End Try
            End Sub
        End Class

        Private Shared _ScheduledTasksOperationsCache As PGlobals.Execution.IScheduledTasks = Nothing
        Public NotInheritable Class ScheduledTasksOperationsClass
            Public Sub New()
                If General._ScheduledTasksOperationsCache Is Nothing Then
                    Try
                        Dim ThemeAsm As System.Reflection.Assembly, objTheme As System.Type

                        ThemeAsm = System.Reflection.Assembly.Load("WebManagers")
                        objTheme = ThemeAsm.GetType("SolidDevelopment.Web.Managers.ScheduledTasksOperationClass", False, True)

                        General._ScheduledTasksOperationsCache = CType(Activator.CreateInstance(objTheme), PGlobals.Execution.IScheduledTasks)
                    Catch ex As Exception
                        Throw New Exception("Communication Error! Scheduled Tasks Service is not accessable...")
                    End Try
                End If
            End Sub

            Public Function RegisterScheduleTask(ByVal ScheduleCallBack As SolidDevelopment.Web.PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As Date) As String
                Return General._ScheduledTasksOperationsCache.RegisterScheduleTask( _
                            ScheduleCallBack, params, TaskExecutionTime)
            End Function

            Public Function RegisterScheduleTask(ByVal ScheduleCallBack As SolidDevelopment.Web.PGlobals.Execution.IScheduledTasks.ScheduleTaskHandler, ByVal params As Object(), ByVal TaskExecutionTime As TimeSpan) As String
                Return General._ScheduledTasksOperationsCache.RegisterScheduleTask( _
                            ScheduleCallBack, params, TaskExecutionTime)
            End Function

            Public Sub UnRegisterScheduleTask(ByVal ScheduleID As String)
                General._ScheduledTasksOperationsCache.UnRegisterScheduleTask(ScheduleID)
            End Sub
        End Class
    End Class

    Public Class Configurations
        Public Shared ReadOnly Property DefaultTheme() As String
            Get
                Return System.Configuration.ConfigurationManager.AppSettings.Item("DefaultTheme")
            End Get
        End Property

        Public Shared ReadOnly Property PyhsicalRoot() As String
            Get
                Return System.Configuration.ConfigurationManager.AppSettings.Item("PyhsicalRoot")
            End Get
        End Property

        Public Shared ReadOnly Property VirtualRoot() As String
            Get
                Dim virRootValue As String = _
                    System.Configuration.ConfigurationManager.AppSettings.Item("VirtualRoot")

                If String.IsNullOrEmpty(virRootValue) Then
                    virRootValue = "/"
                Else
                    virRootValue = virRootValue.Replace("\"c, "/"c)

                    If virRootValue.IndexOf("/"c) <> 0 Then virRootValue = String.Format("/{0}", virRootValue)
                    If String.Compare(virRootValue.Substring(virRootValue.Length - 1), "/") <> 0 Then virRootValue = String.Format("{0}/", virRootValue)
                End If

                Return virRootValue
            End Get
        End Property

        Private Shared _VariablePoolServicePort As Short = 0
        Public Shared ReadOnly Property VariablePoolServicePort() As Short
            Get
                If Configurations._VariablePoolServicePort = 0 Then
                    Dim vpPort As String = _
                        System.Configuration.ConfigurationManager.AppSettings.Item("VariablePoolServicePort")

                    If String.IsNullOrEmpty(vpPort) Then vpPort = "12005"

                    If Not Short.TryParse(vpPort, Configurations._VariablePoolServicePort) Then _
                        Configurations._VariablePoolServicePort = 12005
                End If

                Return Configurations._VariablePoolServicePort
            End Get
        End Property

        Private Shared _ScheduledTasksServicePort As Short = 0
        Public Shared ReadOnly Property ScheduledTasksServicePort() As Short
            Get
                If Configurations._ScheduledTasksServicePort = 0 Then
                    Dim stPort As String = _
                        System.Configuration.ConfigurationManager.AppSettings.Item("ScheduledTasksServicePort")

                    If String.IsNullOrEmpty(stPort) Then stPort = "-1"

                    If Not Short.TryParse(stPort, Configurations._ScheduledTasksServicePort) Then _
                        Configurations._ScheduledTasksServicePort = -1
                End If

                Return Configurations._ScheduledTasksServicePort
            End Get
        End Property

        Public Class ApplicationRootFormat
            Private _FileSystemImplementation As String
            Private _BrowserSystemImplementation As String

            Public Sub New()
                Me._FileSystemImplementation = String.Empty
                Me._BrowserSystemImplementation = String.Empty
            End Sub

            Public Property FileSystemImplementation() As String
                Get
                    Return Me._FileSystemImplementation
                End Get
                Friend Set(ByVal value As String)
                    Me._FileSystemImplementation = value
                End Set
            End Property

            Public Property BrowserSystemImplementation() As String
                Get
                    Return Me._BrowserSystemImplementation
                End Get
                Friend Set(ByVal value As String)
                    Me._BrowserSystemImplementation = value
                End Set
            End Property
        End Class

        Private Shared _ApplicationRootFormat As ApplicationRootFormat = Nothing
        Public Shared ReadOnly Property ApplicationRoot() As ApplicationRootFormat
            Get
                Dim rApplicationRootFormat As ApplicationRootFormat = Configurations._ApplicationRootFormat

                If rApplicationRootFormat Is Nothing Then
                    rApplicationRootFormat = New ApplicationRootFormat

                    Dim appRootValue As String = _
                        System.Configuration.ConfigurationManager.AppSettings.Item("ApplicationRoot")

                    If String.IsNullOrEmpty(appRootValue) Then
                        rApplicationRootFormat.FileSystemImplementation = ".\"
                        rApplicationRootFormat.BrowserSystemImplementation = Configurations.VirtualRoot
                    Else
                        If appRootValue.IndexOf("\"c) = 0 Then appRootValue = String.Format(".{0}", appRootValue)
                        If appRootValue.IndexOf(".\") <> 0 Then appRootValue = String.Format(".\{0}", appRootValue)
                        If String.Compare(appRootValue.Substring(appRootValue.Length - 1), "\") <> 0 Then appRootValue = String.Format("{0}\", appRootValue)

                        rApplicationRootFormat.FileSystemImplementation = appRootValue
                        rApplicationRootFormat.BrowserSystemImplementation = _
                            String.Format("{0}{1}", Configurations.VirtualRoot, appRootValue.Substring(2).Replace("\"c, "/"c))
                    End If

                    Configurations._ApplicationRootFormat = rApplicationRootFormat
                End If

                Return rApplicationRootFormat
            End Get
        End Property

        Public Class WorkingPathFormat
            Private _WorkingPath As String
            Private _WorkingPathID As String

            Public Sub New()
                Me._WorkingPath = String.Empty
                Me._WorkingPathID = String.Empty
            End Sub

            Public Property WorkingPath() As String
                Get
                    Return Me._WorkingPath
                End Get
                Friend Set(ByVal value As String)
                    Me._WorkingPath = value
                End Set
            End Property

            Public Property WorkingPathID() As String
                Get
                    Return Me._WorkingPathID
                End Get
                Friend Set(ByVal value As String)
                    Me._WorkingPathID = value
                End Set
            End Property
        End Class

        Private Shared _WorkingPathFormat As WorkingPathFormat = Nothing
        Public Shared ReadOnly Property WorkingPath() As WorkingPathFormat
            Get
                Dim rWorkingPathFormat As WorkingPathFormat = Configurations._WorkingPathFormat

                If rWorkingPathFormat Is Nothing Then
                    rWorkingPathFormat = New WorkingPathFormat()

                    Dim wPath As String = IO.Path.Combine(Configurations.PyhsicalRoot, Configurations.ApplicationRoot.FileSystemImplementation)
                    rWorkingPathFormat.WorkingPath = wPath

                    For Each match As System.Text.RegularExpressions.Match In System.Text.RegularExpressions.Regex.Matches(wPath, "\W")
                        If match.Success Then
                            wPath = wPath.Remove(match.Index, match.Length)
                            wPath = wPath.Insert(match.Index, "_")
                        End If
                    Next
                    rWorkingPathFormat.WorkingPathID = wPath

                    Configurations._WorkingPathFormat = rWorkingPathFormat
                End If

                Return rWorkingPathFormat
            End Get
        End Property

        Public Shared ReadOnly Property Debugging() As Boolean
            Get
                Dim rBoolean As Boolean = False
                Dim _Debugging As String = System.Configuration.ConfigurationManager.AppSettings.Item("Debugging")

                If Not String.IsNullOrEmpty(_Debugging) Then
                    Try
                        rBoolean = Boolean.Parse(_Debugging)
                    Catch ex As Exception
                        ' Just Handle Exception
                    End Try
                End If

                Return rBoolean
            End Get
        End Property

        Private Shared _LogHTTPExceptions As String = Nothing
        Public Shared ReadOnly Property LogHTTPExceptions() As Boolean
            Get
                If String.IsNullOrEmpty(Configurations._LogHTTPExceptions) Then
                    Try
                        Configurations._LogHTTPExceptions = _
                            Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Item("LogHTTPExceptions")).ToString()
                    Catch ex As Exception
                        Configurations._LogHTTPExceptions = Boolean.TrueString
                    End Try
                End If

                Return Boolean.Parse(Configurations._LogHTTPExceptions)
            End Get
        End Property

        Public Shared ReadOnly Property RequestTagFiltering() As PGlobals.RequestTagFilteringTypes
            Get
                Dim rRequestTagFilteringType As PGlobals.RequestTagFilteringTypes = PGlobals.RequestTagFilteringTypes.None

                Dim _RequestTagFiltering As String = System.Configuration.ConfigurationManager.AppSettings.Item("RequestTagFiltering")
                Dim _RequestTagFilteringItems As String = System.Configuration.ConfigurationManager.AppSettings.Item("RequestTagFilteringItems")

                If Not String.IsNullOrEmpty(_RequestTagFiltering) AndAlso _
                    Not String.IsNullOrEmpty(_RequestTagFilteringItems) Then

                    Try
                        rRequestTagFilteringType = _
                            CType( _
                                [Enum].Parse( _
                                    GetType(PGlobals.RequestTagFilteringTypes), _
                                    _RequestTagFiltering _
                                ), PGlobals.RequestTagFilteringTypes _
                            )
                    Catch ex As Exception
                        ' Just Handle Exception
                    End Try
                End If

                Return rRequestTagFilteringType
            End Get
        End Property

        Private Shared _RequestTagFilteringItemList As String() = Nothing
        Public Shared ReadOnly Property RequestTagFilteringItems() As String()
            Get
                Dim rStringList As String() = Configurations._RequestTagFilteringItemList

                If rStringList Is Nothing Then
                    Dim _RequestTagFilteringItems As String = System.Configuration.ConfigurationManager.AppSettings.Item("RequestTagFilteringItems")

                    If Not String.IsNullOrEmpty(_RequestTagFilteringItems) Then
                        Dim RequestTagFilteringItemList As New Generic.List(Of String)

                        Try
                            For Each item As String In _RequestTagFilteringItems.Split("|"c)
                                If Not item Is Nothing AndAlso _
                                    item.Trim().Length > 0 Then _
                                    RequestTagFilteringItemList.Add(item.Trim().ToLower(New Globalization.CultureInfo("en-US")))
                            Next

                            rStringList = RequestTagFilteringItemList.ToArray()
                        Catch ex As Exception
                            ' Just Handle Exception
                        End Try
                    End If
                    If rStringList Is Nothing Then rStringList = New String() {}
                End If

                Return rStringList
            End Get
        End Property

        Private Shared _RequestTagFilteringExceptionsList As String() = Nothing
        Public Shared ReadOnly Property RequestTagFilteringExceptions() As String()
            Get
                Dim rStringList As String() = Configurations._RequestTagFilteringExceptionsList

                If rStringList Is Nothing Then
                    Dim _RequestTagFilteringExceptions As String = System.Configuration.ConfigurationManager.AppSettings.Item("RequestTagFilteringExceptions")

                    If Not String.IsNullOrEmpty(_RequestTagFilteringExceptions) Then
                        Dim RequestTagFilteringExceptionList As New Generic.List(Of String)

                        Try
                            For Each item As String In _RequestTagFilteringExceptions.Split("|"c)
                                If Not item Is Nothing AndAlso _
                                    item.Trim().Length > 0 Then _
                                    RequestTagFilteringExceptionList.Add(item.Trim().ToLower(New Globalization.CultureInfo("en-US")))
                            Next

                            rStringList = RequestTagFilteringExceptionList.ToArray()
                        Catch ex As Exception
                            ' Just Handle Exception
                        End Try
                    End If
                    If rStringList Is Nothing Then rStringList = New String() {}
                End If

                Return rStringList
            End Get
        End Property

        Private Shared _TemporaryRoot As String = Nothing
        Public Shared ReadOnly Property TemporaryRoot() As String
            Get
                If Configurations._TemporaryRoot Is Nothing Then
                    Configurations._TemporaryRoot = System.IO.Path.GetTempPath()

                    If String.IsNullOrEmpty(Configurations._TemporaryRoot) Then _
                        Configurations._TemporaryRoot = IO.Path.Combine(Configurations.PyhsicalRoot, "tmp")

                    If Not IO.Directory.Exists(Configurations._TemporaryRoot) Then _
                        IO.Directory.CreateDirectory(Configurations._TemporaryRoot)
                End If

                Return Configurations._TemporaryRoot
            End Get
        End Property

        Private Shared _UseHTML5Header As String = Nothing
        Public Shared ReadOnly Property UseHTML5Header() As Boolean
            Get
                Dim rValue As Boolean = False

                If String.IsNullOrEmpty(Configurations._UseHTML5Header) Then
                    Configurations._UseHTML5Header = System.Configuration.ConfigurationManager.AppSettings.Item("UseHTML5Header")

                    If String.IsNullOrEmpty(Configurations._UseHTML5Header) Then Configurations._UseHTML5Header = Boolean.FalseString
                End If

                Boolean.TryParse(Configurations._UseHTML5Header, rValue)

                Return rValue
            End Get
        End Property
    End Class

    Public Class WebService
        Public Enum DataFlowTypes
            Outgoing
            Incoming
        End Enum

        Public Shared Event TransferProgress(ByVal DataFlow As DataFlowTypes, ByVal Current As Long, Total As Long)

        Public Shared Function AuthenticateToWebService(ByVal WebServiceAuthenticationURL As String, ByVal AuthenticationFunction As String, ByVal AuthenticationParametersXML As String, ByRef IsAuthenticationDone As Boolean) As Object
            Dim rMethodResult As Object = _
                WebService.CallWebService(WebServiceAuthenticationURL, AuthenticationFunction, AuthenticationParametersXML)

            PGlobals.Execution.ExamMethodExecuted(rMethodResult, IsAuthenticationDone)

            If IsAuthenticationDone Then
                If TypeOf rMethodResult Is SolidDevelopment.Web.PGlobals.MapControls.MessageResult AndAlso _
                    CType(rMethodResult, SolidDevelopment.Web.PGlobals.MapControls.MessageResult).Type <> PGlobals.MapControls.MessageResult.MessageTypes.Success Then _
                    IsAuthenticationDone = False
            End If

            Return rMethodResult
        End Function

        Public Overloads Shared Function CallWebService(ByVal WebServiceURL As String, ByVal CallFunction As String, ByVal ExecuteParametersXML As String) As Object
            Return WebService.CallWebService(WebServiceURL, CallFunction, ExecuteParametersXML, 60000)
        End Function

        Public Overloads Shared Function CallWebService(ByVal WebServiceURL As String, ByVal CallFunction As String, ByVal ExecuteParametersXML As String, ByVal ResponseTimeout As Integer) As Object
            Dim rMethodResult As Object = Nothing

            Dim HttpWebRequest As System.Net.HttpWebRequest
            Dim HttpWebResponse As System.Net.HttpWebResponse

            Dim RequestMS As IO.MemoryStream = Nothing

            Try
                RequestMS = New IO.MemoryStream( _
                                System.Text.Encoding.UTF8.GetBytes( _
                                    String.Format("execParams={0}", System.Web.HttpUtility.UrlEncode(ExecuteParametersXML)) _
                                ) _
                            )
                RequestMS.Seek(0, IO.SeekOrigin.Begin)

                Dim ResponseString As String
                Dim pageURL As String = String.Format("{0}?call={1}", WebServiceURL, CallFunction)

                ' Prepare Service Request Connection
                HttpWebRequest = CType(System.Net.HttpWebRequest.Create(pageURL), System.Net.HttpWebRequest)

                With HttpWebRequest
                    .Method = "POST"

                    .Timeout = ResponseTimeout
                    .Accept = "*/*"
                    .UserAgent = "Mozilla/4.0 (compatible; MSIE 5.00; Windows 98)"
                    .KeepAlive = False

                    .ContentType = "application/x-www-form-urlencoded"
                    .ContentLength = RequestMS.Length
                End With
                ' !--

                ' Post ExecuteParametersXML to the Web Service
                Dim buffer As Byte() = CType(Array.CreateInstance(GetType(Byte), 512), Byte()), bC As Integer = 0, Current As Long = 0
                Dim TransferStream As IO.Stream = HttpWebRequest.GetRequestStream()

                Do
                    bC = RequestMS.Read(buffer, 0, buffer.Length)

                    If bC > 0 Then
                        Current += bC

                        TransferStream.Write(buffer, 0, bC)

                        RaiseEvent TransferProgress(DataFlowTypes.Outgoing, Current, RequestMS.Length)
                    End If
                Loop Until bC = 0

                TransferStream.Close()
                ' !--

                HttpWebResponse = CType(HttpWebRequest.GetResponse(), System.Net.HttpWebResponse)

                ' Read and Parse Response Datas
                Dim resStream As IO.Stream = HttpWebResponse.GetResponseStream()

                ResponseString = String.Empty : Current = 0
                Do
                    bC = resStream.Read(buffer, 0, buffer.Length)

                    If bC > 0 Then
                        Current += bC

                        ResponseString &= System.Text.Encoding.UTF8.GetString(buffer, 0, bC)

                        RaiseEvent TransferProgress(DataFlowTypes.Incoming, Current, HttpWebResponse.ContentLength)
                    End If
                Loop Until bC = 0

                HttpWebResponse.Close()
                GC.SuppressFinalize(HttpWebResponse)

                rMethodResult = WebService.ParseWebServiceResultObject(ResponseString)
                ' !--
            Catch ex As Exception
                rMethodResult = New Exception("WebService Connection Error!", ex)
            Finally
                If Not RequestMS Is Nothing Then _
                    RequestMS.Close() : GC.SuppressFinalize(RequestMS)
            End Try

            Return rMethodResult
        End Function

        Private Shared Function ParseWebServiceResultObject(ByVal ResultXML As String) As Object
            Dim rMethodResult As Object = Nothing

            If String.IsNullOrEmpty(ResultXML) Then
                rMethodResult = New Exception("WebService Response Error!")
            Else
                Try
                    Dim xPathTextReader As IO.StringReader
                    Dim xPathDoc As Xml.XPath.XPathDocument = Nothing

                    xPathTextReader = New IO.StringReader(ResultXML)
                    xPathDoc = New Xml.XPath.XPathDocument(xPathTextReader)

                    Dim xPathNavigator As Xml.XPath.XPathNavigator
                    Dim xPathIter As Xml.XPath.XPathNodeIterator

                    xPathNavigator = xPathDoc.CreateNavigator()
                    xPathIter = xPathNavigator.Select("/ServiceResult")

                    Dim IsDone As Boolean

                    If xPathIter.MoveNext() Then
                        IsDone = Boolean.Parse(xPathIter.Current.GetAttribute("isdone", xPathIter.Current.NamespaceURI))

                        xPathIter = xPathNavigator.Select("/ServiceResult/Item")

                        If xPathIter.MoveNext() Then
                            Dim xType As String = _
                                xPathIter.Current.GetAttribute("type", xPathIter.Current.NamespaceURI)

                            If Not String.IsNullOrEmpty(xType) Then
                                Select Case xType
                                    Case "RedirectOrder"
                                        rMethodResult = New SolidDevelopment.Web.PGlobals.MapControls.RedirectOrder( _
                                                                    xPathIter.Current.Value _
                                                                )
                                    Case "MessageResult"
                                        rMethodResult = New SolidDevelopment.Web.PGlobals.MapControls.MessageResult( _
                                                                    xPathIter.Current.Value, _
                                                                    CType( _
                                                                        [Enum].Parse( _
                                                                            GetType(PGlobals.MapControls.MessageResult.MessageTypes), _
                                                                            xPathIter.Current.GetAttribute("messagetype", xPathIter.Current.NamespaceURI) _
                                                                        ), PGlobals.MapControls.MessageResult.MessageTypes _
                                                                    ) _
                                                                )
                                    Case "VariableBlockResult"
                                        Dim VariableBlockResult As New SolidDevelopment.Web.PGlobals.MapControls.VariableBlockResult

                                        Dim CultureInfo As System.Globalization.CultureInfo = _
                                            New System.Globalization.CultureInfo( _
                                                xPathIter.Current.GetAttribute("cultureinfo", xPathIter.Current.NamespaceURI))

                                        If xPathIter.Current.MoveToFirstChild() Then
                                            Dim xPathIter_V As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_V.Current.MoveToFirstChild() Then
                                                Do
                                                    VariableBlockResult.Add( _
                                                        xPathIter_V.Current.GetAttribute("key", xPathIter_V.Current.NamespaceURI), _
                                                        System.Convert.ChangeType( _
                                                            xPathIter_V.Current.Value.ToString(CultureInfo), _
                                                            WebService.LoadTypeFromDomain( _
                                                                System.AppDomain.CurrentDomain, _
                                                                xPathIter_V.Current.GetAttribute("type", xPathIter_V.Current.NamespaceURI) _
                                                            ) _
                                                        ) _
                                                    )
                                                Loop While xPathIter_V.Current.MoveToNext()
                                            End If
                                        End If
                                    Case "ConditionalStatementResult"
                                        rMethodResult = New SolidDevelopment.Web.PGlobals.MapControls.ConditionalStatementResult( _
                                                                CType( _
                                                                    [Enum].Parse( _
                                                                        GetType(SolidDevelopment.Web.PGlobals.MapControls.ConditionalStatementResult.Conditions), _
                                                                        xPathIter.Current.Value _
                                                                    ),  _
                                                                    SolidDevelopment.Web.PGlobals.MapControls.ConditionalStatementResult.Conditions _
                                                                ) _
                                                            )
                                    Case "PartialDataTable"
                                        Dim TotalCount As Integer = _
                                            Integer.Parse( _
                                                xPathIter.Current.GetAttribute("totalcount", xPathIter.Current.NamespaceURI))
                                        Dim CultureInfo As System.Globalization.CultureInfo = _
                                            New System.Globalization.CultureInfo( _
                                                xPathIter.Current.GetAttribute("cultureinfo", xPathIter.Current.NamespaceURI))

                                        Dim tDT As New DataTable
                                        tDT.Locale = CultureInfo

                                        If xPathIter.Current.MoveToFirstChild() Then
                                            Dim xPathIter_C As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_C.Current.MoveToFirstChild() Then
                                                Do
                                                    tDT.Columns.Add( _
                                                        xPathIter_C.Current.GetAttribute("name", xPathIter_C.Current.NamespaceURI), _
                                                        WebService.LoadTypeFromDomain( _
                                                            System.AppDomain.CurrentDomain, _
                                                            xPathIter_C.Current.GetAttribute("type", xPathIter_C.Current.NamespaceURI)) _
                                                    )
                                                Loop While xPathIter_C.Current.MoveToNext()
                                            End If
                                        End If

                                        If xPathIter.Current.MoveToNext() Then
                                            Dim xPathIter_R As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            If xPathIter_R.Current.MoveToFirstChild() Then
                                                Dim xPathIter_RR As Xml.XPath.XPathNodeIterator
                                                Dim tDR As DataRow

                                                Do
                                                    tDR = tDT.NewRow()
                                                    xPathIter_RR = xPathIter_R.Clone()

                                                    If xPathIter_RR.Current.MoveToFirstChild() Then
                                                        Do
                                                            tDR.Item( _
                                                                xPathIter_RR.Current.GetAttribute("name", xPathIter_RR.Current.NamespaceURI) _
                                                            ) = _
                                                                xPathIter_RR.Current.Value.ToString(CultureInfo)
                                                        Loop While xPathIter_RR.Current.MoveToNext()
                                                    End If

                                                    tDT.Rows.Add(tDR)
                                                Loop While xPathIter_R.Current.MoveToNext()
                                            End If
                                        End If

                                        Dim pDT As New SolidDevelopment.Web.PGlobals.MapControls.PartialDataTable

                                        pDT.TotalCount = TotalCount
                                        pDT.ThisContainer = tDT.Copy()

                                        If xPathIter.Current.MoveToNext() Then
                                            Dim xPathIter_E As Xml.XPath.XPathNodeIterator = xPathIter.Clone()

                                            pDT.MessageResult = New SolidDevelopment.Web.PGlobals.MapControls.MessageResult( _
                                                                    xPathIter_E.Current.Value.ToString(CultureInfo), _
                                                                    CType( _
                                                                        [Enum].Parse( _
                                                                            GetType(PGlobals.MapControls.MessageResult.MessageTypes), _
                                                                            xPathIter_E.Current.GetAttribute("messagetype", xPathIter_E.Current.NamespaceURI) _
                                                                        ), PGlobals.MapControls.MessageResult.MessageTypes _
                                                                    ) _
                                                                )
                                        End If

                                        rMethodResult = pDT
                                    Case Else
                                        Dim xTypeObject As Type = _
                                            WebService.LoadTypeFromDomain(System.AppDomain.CurrentDomain, xType)

                                        If xTypeObject Is Nothing Then
                                            rMethodResult = xPathIter.Current.Value
                                        Else
                                            If WebService.SearchIsBaseType(xTypeObject, GetType(Exception)) Then
                                                rMethodResult = Activator.CreateInstance(xTypeObject, xPathIter.Current.Value, New Exception())
                                            Else
                                                If xTypeObject.IsPrimitive OrElse _
                                                    xTypeObject Is GetType(Short) OrElse _
                                                    xTypeObject Is GetType(Integer) OrElse _
                                                    xTypeObject Is GetType(Long) Then

                                                    rMethodResult = Convert.ChangeType( _
                                                                        xPathIter.Current.Value, _
                                                                        xTypeObject, _
                                                                        New Globalization.CultureInfo("en-US") _
                                                                    )
                                                Else
                                                    Try
                                                        If Not String.IsNullOrEmpty(xPathIter.Current.Value) Then
                                                            Dim UnSerializedObject As Object = _
                                                                SerializeHelpers.Base64ToBinaryDeSerializer(xPathIter.Current.Value)

                                                            rMethodResult = UnSerializedObject
                                                        Else
                                                            rMethodResult = String.Empty
                                                        End If
                                                    Catch ex As Exception
                                                        rMethodResult = Activator.CreateInstance(ex.GetType(), ex.Message, New Exception())
                                                    End Try
                                                End If
                                            End If
                                        End If
                                End Select
                            End If
                        End If
                    Else
                        rMethodResult = New Exception("WebService Response Error!")
                    End If

                    ' Close Reader
                    xPathTextReader.Close()

                    ' Garbage Collection Cleanup
                    GC.SuppressFinalize(xPathTextReader)
                Catch ex As Exception
                    rMethodResult = New Exception("WebService Response Error!", ex)
                End Try
            End If

            Return rMethodResult
        End Function

        Private Shared Function SearchIsBaseType(ByVal [Type] As Type, ByVal SearchType As Type) As Boolean
            Dim rBoolean As Boolean = False

            Do
                If [Type] Is SearchType Then
                    rBoolean = True

                    Exit Do
                Else
                    [Type] = [Type].BaseType
                End If
            Loop Until [Type] Is Nothing OrElse [Type] Is GetType(Object)

            Return rBoolean
        End Function

        Private Shared Function LoadTypeFromDomain(ByVal AppDomain As System.AppDomain, ByVal SearchType As String) As Type
            Dim rType As Type = _
                Type.GetType(SearchType)

            If rType Is Nothing Then
                Dim assms As System.Reflection.Assembly() = _
                    AppDomain.GetAssemblies()

                For Each assm As System.Reflection.Assembly In assms
                    rType = assm.GetType(SearchType)

                    If Not rType Is Nothing Then Exit For
                Next
            End If

            Return rType
        End Function

        Private Class SerializeHelpers
            Public Shared Function BinaryToBase64Serializer(ByVal [Object] As Object) As String
                Dim serializedBytes As Byte() = SerializeHelpers.BinarySerializer([Object])

                Return System.Convert.ToBase64String(serializedBytes)
            End Function

            Public Shared Function BinarySerializer(ByVal [Object] As Object) As Byte()
                Dim rByte As Byte()

                Dim BinaryFormater As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                BinaryFormater.Binder = New SolidDevelopment.Web.PGlobals.Helpers.OverrideBinder()

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
                BinaryFormater.Binder = New SolidDevelopment.Web.PGlobals.Helpers.OverrideBinder()

                Dim SerializationStream As New IO.MemoryStream(SerializedBytes)

                rObject = BinaryFormater.Deserialize(SerializationStream)

                SerializationStream.Close()

                GC.SuppressFinalize(SerializationStream)

                Return rObject
            End Function

        End Class
    End Class
End Namespace