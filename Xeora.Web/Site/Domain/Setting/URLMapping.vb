Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class URLMapping
        Implements [Shared].IDomain.ISettings.IURLMappings

        Private _XPathNavigator As Xml.XPath.XPathNavigator

        Private _IsActive As Boolean
        Private _ResolverExecutable As String
        Private _Items As [Shared].URLMapping.URLMappingItem.URLMappingItemCollection

        Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
            Me._XPathNavigator = ConfigurationNavigator.Clone()

            Me.PrepareOptions()
        End Sub

        Public ReadOnly Property IsActive() As Boolean Implements [Shared].IDomain.ISettings.IURLMappings.IsActive
            Get
                Return Me._IsActive
            End Get
        End Property

        Public ReadOnly Property ResolverExecutable() As String Implements [Shared].IDomain.ISettings.IURLMappings.ResolverExecutable
            Get
                Return Me._ResolverExecutable
            End Get
        End Property

        Public ReadOnly Property Items() As [Shared].URLMapping.URLMappingItem.URLMappingItemCollection Implements [Shared].IDomain.ISettings.IURLMappings.Items
            Get
                Return Me._Items
            End Get
        End Property

        Private Sub PrepareOptions()
            Me._Items = New [Shared].URLMapping.URLMappingItem.URLMappingItemCollection()

            Dim xPathIter As Xml.XPath.XPathNodeIterator, xPathIter_in As Xml.XPath.XPathNodeIterator

            Try
                xPathIter = Me._XPathNavigator.Select(String.Format("//URLMapping"))

                If xPathIter.MoveNext() Then
                    If Not Boolean.TryParse(
                        xPathIter.Current.GetAttribute("active", xPathIter.Current.BaseURI),
                        Me._IsActive) Then

                        Me._IsActive = False
                    End If

                    Me._ResolverExecutable = xPathIter.Current.GetAttribute("resolverExecutable", xPathIter.Current.BaseURI)
                End If

                ' If mapping is active then read mapping items
                If Me._IsActive Then
                    ' Read URLMapping Options
                    xPathIter = Me._XPathNavigator.Select(String.Format("//URLMapping/Map"))

                    Dim tMappingItem As [Shared].URLMapping.URLMappingItem
                    Dim [Overridable] As Boolean = False, Priority As Integer, Priority_t As String, Request As String = String.Empty

                    Dim Reverse_ID As String = String.Empty, Reverse_Mapped As String = String.Empty
                    Dim Reverse_MappedItems As [Shared].URLMapping.ResolveInfos.MappedItem.MappedItemCollection = Nothing

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

                                        Dim xPathIter_servicetest As Xml.XPath.XPathNodeIterator =
                                            Me._XPathNavigator.Select(String.Format("//Services/Item[@id='{0}']", Reverse_ID))

                                        If xPathIter_servicetest.MoveNext() Then _
                                            If Not Boolean.TryParse(xPathIter.Current.GetAttribute("overridable", xPathIter.Current.BaseURI), [Overridable]) Then [Overridable] = False

                                        ' TODO: mapped is not in use. The logic was creating the formatted URL from resolved request.
                                        Reverse_Mapped = xPathIter_in.Current.GetAttribute("mapped", xPathIter_in.Current.BaseURI)
                                        Reverse_MappedItems = New [Shared].URLMapping.ResolveInfos.MappedItem.MappedItemCollection

                                        If xPathIter_in.Current.MoveToFirstChild() Then
                                            Do
                                                Select Case xPathIter_in.Current.Name
                                                    Case "MappedItem"
                                                        Dim MappedItem_ID As String, MappedItem_DefaultValue As String, MappedItem_QueryStringKey As String

                                                        MappedItem_ID = xPathIter_in.Current.GetAttribute("id", xPathIter_in.Current.BaseURI)
                                                        MappedItem_QueryStringKey = xPathIter_in.Current.GetAttribute("key", xPathIter_in.Current.BaseURI)
                                                        MappedItem_DefaultValue = xPathIter_in.Current.GetAttribute("defaultValue", xPathIter_in.Current.BaseURI)

                                                        Dim MappedItem As New [Shared].URLMapping.ResolveInfos.MappedItem(MappedItem_ID)

                                                        With MappedItem
                                                            .QueryStringKey = MappedItem_QueryStringKey
                                                            .DefaultValue = MappedItem_DefaultValue
                                                        End With

                                                        Reverse_MappedItems.Add(MappedItem)
                                                End Select
                                            Loop While xPathIter_in.Current.MoveToNext()
                                        End If
                                End Select
                            Loop While xPathIter_in.Current.MoveToNext()
                        End If

                        tMappingItem = New [Shared].URLMapping.URLMappingItem()

                        With tMappingItem
                            .Overridable = [Overridable]
                            .Priority = Priority
                            .RequestMap = Request

                            Dim resInfo As New [Shared].URLMapping.ResolveInfos([Shared].ServicePathInfo.Parse(Reverse_ID, True))

                            resInfo.MapFormat = Reverse_Mapped
                            resInfo.MappedItems.AddRange(Reverse_MappedItems)

                            .ResolveInfo = resInfo
                        End With

                        Me._Items.Add(tMappingItem)
                    Loop
                End If
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try
        End Sub
    End Class
End Namespace