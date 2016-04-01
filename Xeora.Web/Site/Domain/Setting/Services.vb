Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class Services
        Implements [Shared].IDomain.ISettings.IServices

        Private _XPathNavigator As Xml.XPath.XPathNavigator

        Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
            Me._XPathNavigator = ConfigurationNavigator.Clone()
        End Sub

        Public ReadOnly Property ServiceItems() As [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection Implements [Shared].IDomain.ISettings.IServices.ServiceItems
            Get
                Return Me.ReadServiceOptions()
            End Get
        End Property

        Private Function ReadServiceOptions() As [Shared].IDomain.ISettings.IServices.IServiceItem.IServiceItemCollection
            Dim rCollection As New ServiceItem.ServiceItemCollection()

            Dim xPathIter As Xml.XPath.XPathNodeIterator

            Try
                ' Read Authentication Keys
                xPathIter = Me._XPathNavigator.Select("//Services/AuthenticationKeys/Item")

                Dim AuthenticationKeys As New Generic.List(Of String)

                Do While xPathIter.MoveNext()
                    AuthenticationKeys.Add(xPathIter.Current.GetAttribute("id", xPathIter.Current.BaseURI))
                Loop

                xPathIter = Me._XPathNavigator.Select("//Services/Item")

                Dim tServiceItem As ServiceItem
                Dim ID As String, mimeType As String, Authentication As String, Type As String, ExecuteIn As String, StandAlone As String, [Overridable] As String

                Do While xPathIter.MoveNext()
                    ID = xPathIter.Current.GetAttribute("id", xPathIter.Current.BaseURI)
                    Type = xPathIter.Current.GetAttribute("type", xPathIter.Current.BaseURI)
                    [Overridable] = xPathIter.Current.GetAttribute("overridable", xPathIter.Current.BaseURI)
                    Authentication = xPathIter.Current.GetAttribute("authentication", xPathIter.Current.BaseURI)
                    StandAlone = xPathIter.Current.GetAttribute("standalone", xPathIter.Current.BaseURI)
                    ExecuteIn = xPathIter.Current.GetAttribute("executein", xPathIter.Current.BaseURI)
                    mimeType = xPathIter.Current.GetAttribute("mime", xPathIter.Current.BaseURI)

                    tServiceItem = New ServiceItem(ID)

                    tServiceItem.ServiceType = CType([Enum].Parse(GetType([Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes), Type, True), [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes)
                    Boolean.TryParse([Overridable], tServiceItem.Overridable)
                    Boolean.TryParse(Authentication, tServiceItem.Authentication)
                    tServiceItem.AuthenticationKeys = AuthenticationKeys.ToArray()
                    Boolean.TryParse(StandAlone, tServiceItem.StandAlone)
                    tServiceItem.ExecuteIn = ExecuteIn

                    If tServiceItem.ServiceType = [Shared].IDomain.ISettings.IServices.IServiceItem.ServiceTypes.WebService Then
                        tServiceItem.MimeType = "text/xml"
                    Else
                        If Not String.IsNullOrEmpty(mimeType) Then _
                            tServiceItem.MimeType = mimeType
                    End If

                    rCollection.Add(tServiceItem)
                Loop
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            Return rCollection
        End Function
    End Class
End Namespace