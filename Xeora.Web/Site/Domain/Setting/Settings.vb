Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class Settings
        Implements [Shared].IDomain.ISettings

        Private _XPathStream As IO.StringReader = Nothing
        Private _XPathNavigator As Xml.XPath.XPathNavigator

        Private _Configurations As Configurations
        Private _Services As Services
        Private _URLMappings As URLMapping

        Public Sub New(ByVal ConfigurationContent As String)
            If ConfigurationContent Is Nothing OrElse
                ConfigurationContent.Trim().Length = 0 Then

                Throw New System.Exception([Global].SystemMessages.CONFIGURATIONCONTENT & "!")
            End If

            Dim xPathDoc As Xml.XPath.XPathDocument

            Try
                ' Performance Optimization
                Me._XPathStream = New IO.StringReader(ConfigurationContent)
                xPathDoc = New Xml.XPath.XPathDocument(Me._XPathStream)

                Me._XPathNavigator = xPathDoc.CreateNavigator()
                ' !--
            Catch ex As system.Exception
                Me.Dispose(True)

                Throw
            End Try

            Me._Configurations = New Configurations(Me._XPathNavigator)
            Me._Services = New Services(Me._XPathNavigator)
            Me._URLMappings = New URLMapping(Me._XPathNavigator)
        End Sub

        Public ReadOnly Property Configurations() As [Shared].IDomain.ISettings.IConfigurations Implements [Shared].IDomain.ISettings.Configurations
            Get
                Return Me._Configurations
            End Get
        End Property

        Public ReadOnly Property Services() As [Shared].IDomain.ISettings.IServices Implements [Shared].IDomain.ISettings.Services
            Get
                Return Me._Services
            End Get
        End Property

        Public ReadOnly Property URLMappings() As [Shared].IDomain.ISettings.IURLMappings Implements [Shared].IDomain.ISettings.URLMappings
            Get
                Return Me._URLMappings
            End Get
        End Property

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then Me._XPathStream.Close() : GC.SuppressFinalize(Me._XPathStream)

            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace