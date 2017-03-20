Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class ServiceItem
        Implements [Shared].IDomain.ISettings.IServices.IServiceItem

        Private _ID As String
        Private _MimeType As String
        Private _ServiceType As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes
        Private _ExecuteIn As String
        Private _Authentication As Boolean
        Private _AuthenticationKeys As String()
        Private _AuthenticationTypes As String()
        Private _StandAlone As Boolean
        Private _Overridable As Boolean

        Public Sub New(ByVal ID As String)
            Me._ID = ID
            Me._MimeType = "text/html; charset=utf-8"
            Me._ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.Template
            Me._ExecuteIn = String.Empty
            Me._Authentication = False
            Me._AuthenticationKeys = New String() {}
            Me._StandAlone = False
            Me._Overridable = False
        End Sub

        Public Property ID() As String Implements [Shared].IDomain.ISettings.IServices.IServiceItem.ID
            Get
                Return Me._ID
            End Get
            Set(ByVal Value As String)
                Me._ID = Value
            End Set
        End Property

        Public Property MimeType() As String Implements [Shared].IDomain.ISettings.IServices.IServiceItem.MimeType
            Get
                Return Me._MimeType
            End Get
            Set(ByVal value As String)
                Me._MimeType = value
            End Set
        End Property

        Public Property Authentication() As Boolean Implements [Shared].IDomain.ISettings.IServices.IServiceItem.Authentication
            Get
                Return Me._Authentication
            End Get
            Set(ByVal Value As Boolean)
                Me._Authentication = Value
            End Set
        End Property

        Public Property AuthenticationKeys() As String() Implements [Shared].IDomain.ISettings.IServices.IServiceItem.AuthenticationKeys
            Get
                Return Me._AuthenticationKeys
            End Get
            Set(ByVal Value As String())
                Me._AuthenticationKeys = Value
            End Set
        End Property

        Public Property StandAlone() As Boolean Implements [Shared].IDomain.ISettings.IServices.IServiceItem.StandAlone
            Get
                Return Me._StandAlone
            End Get
            Set(ByVal Value As Boolean)
                Me._StandAlone = Value
            End Set
        End Property

        Public Property [Overridable]() As Boolean Implements [Shared].IDomain.ISettings.IServices.IServiceItem.Overridable
            Get
                Return Me._Overridable
            End Get
            Set(ByVal Value As Boolean)
                Me._Overridable = Value
            End Set
        End Property

        Public Property ServiceType() As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes Implements [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceType
            Get
                Return Me._ServiceType
            End Get
            Set(ByVal Value As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes)
                Me._ServiceType = Value
            End Set
        End Property

        Public Property ExecuteIn() As String Implements [Shared].IDomain.ISettings.IServices.IServiceItem.ExecuteIn
            Get
                Return Me._ExecuteIn
            End Get
            Set(ByVal Value As String)
                Me._ExecuteIn = Value
            End Set
        End Property

        Public Class ServiceItemCollection
            Inherits Generic.List(Of ServiceItem)
            Implements [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection

            Public Function GetServiceItem(ByVal ID As String) As [Shared].IDomain.ISettings.IServices.IServiceItem Implements [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection.GetServiceItem
                For Each sI As ServiceItem In Me
                    If String.Compare(sI.ID, ID, True) = 0 Then Return sI
                Next

                Return Nothing
            End Function

            Public Function GetServiceItems(ByVal ServiceType As [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes) As [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection Implements [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection.GetServiceItems
                Dim rCollection As New ServiceItemCollection

                For Each sI As ServiceItem In Me.ToArray()
                    If sI.ServiceType = ServiceType Then rCollection.Add(sI)
                Next

                Return rCollection
            End Function

            Public Function GetAuthenticationKeys() As String() Implements [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection.GetAuthenticationKeys
                If Me.Count > 0 Then _
                    Return Me.Item(0).AuthenticationKeys

                Return New String() {}
            End Function
        End Class
    End Class
End Namespace