Option Strict On

Namespace Xeora.Web.Handler.Session
    Friend Class SessionItem
        Private _Items As System.Web.SessionState.SessionStateItemCollection
        Private _StaticObjects As System.Web.HttpStaticObjectsCollection
        Private _Expires As Date

        Public Sub New(ByVal Items As System.Web.SessionState.SessionStateItemCollection, ByVal StaticObjects As System.Web.HttpStaticObjectsCollection, ByVal Expires As Date)
            Me._Items = Items
            Me._StaticObjects = StaticObjects
            Me._Expires = Expires
        End Sub

        Public ReadOnly Property Items As System.Web.SessionState.SessionStateItemCollection
            Get
                Return Me._Items
            End Get
        End Property

        Public ReadOnly Property StaticObjects As System.Web.HttpStaticObjectsCollection
            Get
                Return Me._StaticObjects
            End Get
        End Property

        Public Property Expires As Date
            Get
                Return Me._Expires
            End Get
            Set(ByVal value As Date)
                Me._Expires = value
            End Set
        End Property
    End Class
End Namespace