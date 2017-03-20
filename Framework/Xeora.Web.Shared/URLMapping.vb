Option Strict On

Namespace Xeora.Web.Shared
    Public Class URLMapping
        Private Shared _URLMappings As New Concurrent.ConcurrentDictionary(Of String(), URLMapping)

        Private _IsActive As Boolean
        Private _ResolverBindInfo As Execution.BindInfo
        Private _Items As URLMappingItem.URLMappingItemCollection

        Private Sub New(ByVal IsActive As Boolean, ByVal ResolverExecutable As String, ByVal MapItems As URLMappingItem())
            Me._IsActive = IsActive
            Me._ResolverBindInfo = Execution.BindInfo.Make(ResolverExecutable)
            Me._Items = New URLMappingItem.URLMappingItemCollection()
            Me._Items.AddRange(MapItems)
        End Sub

        Public Property IsActive() As Boolean
            Get
                Return Me._IsActive
            End Get
            Set(ByVal value As Boolean)
                Me._IsActive = value
            End Set
        End Property

        Public Property ResolverBindInfo As Execution.BindInfo
            Get
                Return Me._ResolverBindInfo
            End Get
            Set(ByVal value As Execution.BindInfo)
                Me._ResolverBindInfo = value
            End Set
        End Property

        Public ReadOnly Property Items() As URLMappingItem.URLMappingItemCollection
            Get
                Return Me._Items
            End Get
        End Property

        Public Shared ReadOnly Property Current As URLMapping
            Get
                If Not URLMapping._URLMappings.ContainsKey(Helpers.CurrentDomainInstance.IDAccessTree) Then
                    Dim DomainInstance As IDomain =
                        Helpers.CurrentDomainInstance

                    Dim URLMappingInstance As URLMapping =
                        New URLMapping(
                            DomainInstance.Settings.URLMappings.IsActive,
                            DomainInstance.Settings.URLMappings.ResolverExecutable,
                            DomainInstance.Settings.URLMappings.Items.ToArray())

                    URLMapping._URLMappings.TryAdd(DomainInstance.IDAccessTree, URLMappingInstance)
                End If

                Dim rURLMappingInstance As URLMapping = Nothing

                URLMapping._URLMappings.TryGetValue(Helpers.CurrentDomainInstance.IDAccessTree, rURLMappingInstance)

                Return rURLMappingInstance
            End Get
        End Property

        Public Function ResolveMappedURL(ByVal RequestFilePath As String) As ResolvedMapped
            Dim rResolvedMapped As ResolvedMapped = Nothing

            If Me._IsActive Then
                Dim DomainAsm As Reflection.Assembly, objDomain As Type

                DomainAsm = Reflection.Assembly.Load("Xeora.Web")
                objDomain = DomainAsm.GetType("Xeora.Web.Site.DomainControl", False, True)

                Dim workingDomainControl As IDomainControl =
                    CType(objDomain.InvokeMember("Instance", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.GetProperty, Nothing, Nothing, New Object() {Helpers.CurrentRequestID}), IDomainControl)

                rResolvedMapped = workingDomainControl.QueryURLResolver(RequestFilePath)

                If rResolvedMapped Is Nothing OrElse Not rResolvedMapped.IsResolved Then
                    For Each URLMapItem As URLMappingItem In Me._Items
                        Dim rqMatch As Text.RegularExpressions.Match =
                            Text.RegularExpressions.Regex.Match(RequestFilePath, URLMapItem.RequestMap, Text.RegularExpressions.RegexOptions.IgnoreCase)

                        If rqMatch.Success Then
                            rResolvedMapped = New ResolvedMapped(True, URLMapItem.ResolveInfo.ServicePathInfo)

                            Dim medItemValue As String
                            For Each medItem As ResolveInfos.MappedItem In URLMapItem.ResolveInfo.MappedItems
                                medItemValue = String.Empty

                                If Not String.IsNullOrEmpty(medItem.ID) Then medItemValue = rqMatch.Groups.Item(medItem.ID).Value

                                rResolvedMapped.URLQueryDictionary.Item(medItem.QueryStringKey) =
                                    CType(IIf(String.IsNullOrEmpty(medItemValue), medItem.DefaultValue, medItemValue), String)
                            Next

                            Exit For
                        End If
                    Next
                End If

                If rResolvedMapped Is Nothing Then _
                    rResolvedMapped = New ResolvedMapped(False, Nothing)
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
                    Implements Generic.IComparer(Of URLMappingItem)

                    Public Function Compare(ByVal x As URLMappingItem, ByVal y As URLMappingItem) As Integer Implements Generic.IComparer(Of URLMappingItem).Compare
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

            Private _ServicePathInfo As ServicePathInfo
            Private _URLQueryDictionary As URLQueryDictionary

            Public Sub New(ByVal IsResolved As Boolean, ByVal ServicePathInfo As ServicePathInfo)
                Me._IsResolved = IsResolved

                Me._ServicePathInfo = ServicePathInfo
                Me._URLQueryDictionary = New URLQueryDictionary
            End Sub

            Public ReadOnly Property IsResolved() As Boolean
                Get
                    Return Me._IsResolved
                End Get
            End Property

            Public ReadOnly Property ServicePathInfo() As ServicePathInfo
                Get
                    Return Me._ServicePathInfo
                End Get
            End Property

            Public ReadOnly Property URLQueryDictionary() As URLQueryDictionary
                Get
                    Return Me._URLQueryDictionary
                End Get
            End Property
        End Class

        Public Class ResolveInfos
            Private _ServicePathInfo As ServicePathInfo
            Private _MapFormat As String
            Private _MappedItems As MappedItem.MappedItemCollection

            Public Sub New(ByVal ServicePathInfo As ServicePathInfo)
                Me._ServicePathInfo = ServicePathInfo
                Me._MapFormat = String.Empty
                Me._MappedItems = New MappedItem.MappedItemCollection
            End Sub

            Public ReadOnly Property ServicePathInfo() As ServicePathInfo
                Get
                    Return Me._ServicePathInfo
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

                    Public Shadows ReadOnly Property Item(ByVal ID As String) As MappedItem
                        Get
                            Dim rMappedItem As MappedItem =
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
End Namespace