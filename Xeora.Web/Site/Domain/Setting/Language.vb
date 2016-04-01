Option Strict On

Namespace Xeora.Web.Site.Setting
    Public Class Language
        Implements [Shared].IDomain.ILanguage

        Private _Parent As [Shared].IDomain

        Private _ID As String
        Private _Name As String

        Private _XPathStream As IO.StringReader = Nothing
        Private _XPathNavigator As Xml.XPath.XPathNavigator

        Public Sub New(ByVal Parent As [Shared].IDomain, ByVal LanguageXMLContent As String)
            Me._Parent = Parent

            If LanguageXMLContent Is Nothing OrElse
                LanguageXMLContent.Trim().Length = 0 Then

                Throw New System.Exception([Global].SystemMessages.TRANSLATIONCONTENT & "!")
            End If

            Dim xPathDoc As Xml.XPath.XPathDocument
            Dim xPathIter As Xml.XPath.XPathNodeIterator

            Try
                ' Performance Optimization
                Me._XPathStream = New IO.StringReader(LanguageXMLContent)
                xPathDoc = New Xml.XPath.XPathDocument(Me._XPathStream)

                Me._XPathNavigator = xPathDoc.CreateNavigator()
                ' !--

                xPathIter = Me._XPathNavigator.Select("/language")

                If xPathIter.MoveNext() Then
                    Me._ID = xPathIter.Current.GetAttribute("code", xPathIter.Current.NamespaceURI)
                    Me._Name = xPathIter.Current.GetAttribute("name", xPathIter.Current.NamespaceURI)
                End If
            Catch ex As System.Exception
                Me.Dispose(True)

                Throw
            End Try
        End Sub

        Public ReadOnly Property ID() As String Implements [Shared].IDomain.ILanguage.ID
            Get
                Return Me._ID
            End Get
        End Property

        Public ReadOnly Property Name() As String Implements [Shared].IDomain.ILanguage.Name
            Get
                Return Me._Name
            End Get
        End Property

        Public ReadOnly Property Info() As [Shared].DomainInfo.LanguageInfo Implements [Shared].IDomain.ILanguage.Info
            Get
                Return New [Shared].DomainInfo.LanguageInfo(Me._ID, Me._Name)
            End Get
        End Property

        Public Function [Get](ByVal TranslationID As String) As String Implements [Shared].IDomain.ILanguage.Get
            Dim rString As String = Nothing

            Dim xPathIter As Xml.XPath.XPathNodeIterator

            Try
                xPathIter = Me._XPathNavigator.Select(String.Format("//translation[@id='{0}']", TranslationID))

                If xPathIter.MoveNext() Then rString = xPathIter.Current.Value

                If String.IsNullOrEmpty(rString) Then
                    Dim WorkingInstance As [Shared].IDomain = Me._Parent

                    Do Until WorkingInstance Is Nothing OrElse Not String.IsNullOrEmpty(rString)
                        WorkingInstance = WorkingInstance.Parent

                        If Not WorkingInstance Is Nothing Then _
                            rString = WorkingInstance.Language.Get(TranslationID)
                    Loop
                End If
            Catch ex As System.Exception
                ' Just Handle Exceptions
            End Try

            Return rString
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue AndAlso Not Me._XPathStream Is Nothing Then Me._XPathStream.Close() : GC.SuppressFinalize(Me._XPathStream)

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