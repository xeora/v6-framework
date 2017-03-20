Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class Configurations
        Implements [Shared].IDomain.ISettings.IConfigurations

        Private _XPathNavigator As Xml.XPath.XPathNavigator

        Public Sub New(ByRef ConfigurationNavigator As Xml.XPath.XPathNavigator)
            Me._XPathNavigator = ConfigurationNavigator.Clone()
        End Sub

        Public ReadOnly Property AuthenticationPage() As String Implements [Shared].IDomain.ISettings.IConfigurations.AuthenticationPage
            Get
                Dim _AuthenticationPage As String =
                    Me.ReadConfiguration("authenticationpage")

                If _AuthenticationPage Is Nothing Then _AuthenticationPage = Me.DefaultPage

                Return _AuthenticationPage
            End Get
        End Property

        Public ReadOnly Property DefaultPage() As String Implements [Shared].IDomain.ISettings.IConfigurations.DefaultPage
            Get
                Return Me.ReadConfiguration("defaultpage")
            End Get
        End Property

        Public ReadOnly Property DefaultLanguage() As String Implements [Shared].IDomain.ISettings.IConfigurations.DefaultLanguage
            Get
                Return Me.ReadConfiguration("defaultlanguage")
            End Get
        End Property

        Public ReadOnly Property DefaultCaching() As [Shared].Enum.PageCachingTypes Implements [Shared].IDomain.ISettings.IConfigurations.DefaultCaching
            Get
                Dim rPageCaching As [Shared].Enum.PageCachingTypes = [Shared].Enum.PageCachingTypes.AllContent
                Dim configString As String = Me.ReadConfiguration("defaultcaching")

                If Not [Enum].TryParse(Of [Shared].Enum.PageCachingTypes)(configString, rPageCaching) Then _
                    rPageCaching = [Shared].Enum.PageCachingTypes.AllContent

                Return rPageCaching
            End Get
        End Property

        Public ReadOnly Property DefaultSecurityBind() As String Implements [Shared].IDomain.ISettings.IConfigurations.DefaultSecurityBind
            Get
                Return Me.ReadConfiguration("defaultsecuritybind")
            End Get
        End Property

        Private Function ReadConfiguration(ByVal key As String) As String
            Dim rString As String = Nothing

            Dim xPathIter As Xml.XPath.XPathNodeIterator

            Try
                xPathIter = Me._XPathNavigator.Select(String.Format("//Configuration/Item[@key='{0}']", key))

                If xPathIter.MoveNext() Then rString = xPathIter.Current.GetAttribute("value", xPathIter.Current.BaseURI)
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            Return rString
        End Function
    End Class
End Namespace