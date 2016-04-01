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

            Public Sub New()
                MyBase.New()
            End Sub

            Public Shadows Sub Add(ByVal value As DomainInfo)
                MyBase.Add(value)
            End Sub

            Public Shadows Sub Remove(ByVal ID As String)
                For Each item As DomainInfo In Me
                    If String.Compare(ID, item.ID, True) = 0 Then
                        MyBase.Remove(item)

                        Exit For
                    End If
                Next
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

                    For Each tI As DomainInfo In Me
                        If String.Compare(ID, tI.ID, True) = 0 Then
                            rDomainInfo = tI

                            Exit For
                        End If
                    Next

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