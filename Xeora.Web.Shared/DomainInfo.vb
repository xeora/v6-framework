Option Strict On

Namespace Xeora.Web.Shared
    Public Class DomainInfo
        Private _DeploymentType As DeploymentTypes

        Private _ID As String
        Private _Version As String
        Private _Languages As LanguageInfo()
        Private _Children As DomainInfoCollection

        Public Enum DeploymentTypes
            Development
            Release
        End Enum

        Public Sub New(ByVal DeploymentType As DeploymentTypes, ByVal ID As String, ByVal Languages As LanguageInfo())

            Me._DeploymentType = DeploymentType
            Me._ID = ID
            Me._Languages = Languages
            Me._Children = New DomainInfoCollection()
        End Sub

        Public ReadOnly Property DeploymentType() As DeploymentTypes
            Get
                Return Me._DeploymentType
            End Get
        End Property

        Public ReadOnly Property ID() As String
            Get
                Return Me._ID
            End Get
        End Property

        Public ReadOnly Property Languages() As LanguageInfo()
            Get
                Return Me._Languages
            End Get
        End Property

        Public ReadOnly Property Children() As DomainInfoCollection
            Get
                Return Me._Children
            End Get
        End Property

        Public Class LanguageInfo
            Private _ID As String
            Private _Name As String

            Public Sub New(ByVal ID As String, ByVal Name As String)
                Me._ID = ID
                Me._Name = Name
            End Sub

            Public ReadOnly Property ID() As String
                Get
                    Return Me._ID
                End Get
            End Property

            Public ReadOnly Property Name() As String
                Get
                    Return Me._Name
                End Get
            End Property
        End Class

        Public Class DomainInfoCollection
            Inherits Generic.List(Of DomainInfo)

            Private _NameIndexMap As Generic.Dictionary(Of String, Integer)

            Public Sub New()
                MyBase.New()

                Me._NameIndexMap = New Generic.Dictionary(Of String, Integer)
            End Sub

            Public Shadows Sub Add(ByVal value As DomainInfo)
                MyBase.Add(value)

                Me._NameIndexMap.Add(value.ID, MyBase.Count - 1)
            End Sub

            Public Shadows Sub Remove(ByVal ID As String)
                If Me._NameIndexMap.ContainsKey(ID) Then
                    MyBase.RemoveAt(Me._NameIndexMap.Item(ID))

                    Me._NameIndexMap.Clear()

                    ' Rebuild, NameIndexMap
                    Dim Index As Integer = 0
                    For Each item As DomainInfo In Me
                        Me._NameIndexMap.Add(item.ID, Index)

                        Index += 1
                    Next
                End If
            End Sub

            Public Shadows Sub Remove(ByVal value As DomainInfo)
                Me.Remove(value.ID)
            End Sub

            Public Shadows Property Item(ByVal Index As Integer) As DomainInfo
                Get
                    Dim rDomainInfo As DomainInfo = Nothing

                    If Index < Me.Count Then rDomainInfo = Me(Index)

                    Return rDomainInfo
                End Get
                Set(ByVal Value As DomainInfo)
                    Me.Remove(Value.ID)
                    Me.Add(Value)
                End Set
            End Property

            Public Shadows Property Item(ByVal ID As String) As DomainInfo
                Get
                    Dim rDomainInfo As DomainInfo = Nothing

                    If Me._NameIndexMap.ContainsKey(ID) Then rDomainInfo = Me(Me._NameIndexMap.Item(ID))

                    Return rDomainInfo
                End Get
                Set(ByVal Value As DomainInfo)
                    Me.Remove(Value.ID)
                    Me.Add(Value)
                End Set
            End Property
        End Class
    End Class
End Namespace