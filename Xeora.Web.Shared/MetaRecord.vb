Option Strict On

Namespace Xeora.Web.Shared
    Public Class MetaRecord
        Public Shared _MetaRecords As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared _CustomMetaRecords As Hashtable = Hashtable.Synchronized(New Hashtable)

        Public Enum MetaTags As Byte
            author = 1
            cachecontrol = 2
            contentlanguage = 3
            contenttype = 4
            copyright = 5
            description = 6
            expires = 7
            keywords = 8
            pragma = 9
            refresh = 10
            robots = 11
            googlebot = 12
        End Enum

        Public Enum MetaTagSpace As Byte
            name = 1
            httpequiv = 2
        End Enum

        Public Shared Function GetMetaTagHtmlName(ByVal MetaTag As MetaTags) As String
            Dim rString As String = String.Empty

            Select Case MetaTag
                Case MetaTags.author
                    rString = "Author"
                Case MetaTags.cachecontrol
                    rString = "Cache-Control"
                Case MetaTags.contentlanguage
                    rString = "Content-Language"
                Case MetaTags.contenttype
                    rString = "Content-Type"
                Case MetaTags.copyright
                    rString = "Copyright"
                Case MetaTags.description
                    rString = "Description"
                Case MetaTags.expires
                    rString = "Expires"
                Case MetaTags.googlebot
                    rString = "Googlebot"
                Case MetaTags.keywords
                    rString = "Keywords"
                Case MetaTags.pragma
                    rString = "Pragma"
                Case MetaTags.refresh
                    rString = "Refresh"
                Case MetaTags.robots
                    rString = "Robots"
            End Select

            Return rString
        End Function

        Public Shared Sub AddCustomMetaRecord(ByVal MetaTagSpace As MetaTagSpace, ByVal Name As String, ByVal Value As String)
            Threading.Monitor.Enter(MetaRecord._CustomMetaRecords.SyncRoot)
            Try
                Select Case MetaTagSpace
                    Case MetaTagSpace.name
                        Name = String.Format("name::{0}", Name)
                    Case MetaTagSpace.httpequiv
                        Name = String.Format("httpequiv::{0}", Name)
                End Select

                Dim cMRs As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomMetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    cMRs = CType(MetaRecord._CustomMetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    cMRs = New Generic.Dictionary(Of String, String)
                End If

                If cMRs.ContainsKey(Name) Then cMRs.Remove(Name)
                cMRs.Add(Name, Value)

                MetaRecord._CustomMetaRecords.Item(Helpers.CurrentRequestID) = cMRs
            Finally
                Threading.Monitor.Exit(MetaRecord._CustomMetaRecords.SyncRoot)
            End Try
        End Sub

        Public Shared Sub RemoveCustomMetaRecord(ByVal Name As String)
            Threading.Monitor.Enter(MetaRecord._CustomMetaRecords.SyncRoot)
            Try
                Dim cMRs As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomMetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    cMRs = CType(MetaRecord._CustomMetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    cMRs = New Generic.Dictionary(Of String, String)
                End If

                If cMRs.ContainsKey(Name) Then cMRs.Remove(Name)

                MetaRecord._CustomMetaRecords.Item(Helpers.CurrentRequestID) = cMRs
            Finally
                Threading.Monitor.Exit(MetaRecord._MetaRecords.SyncRoot)
            End Try
        End Sub

        Public Shared Sub AddMetaRecord(ByVal MetaTag As MetaTags, ByVal Value As String)
            Threading.Monitor.Enter(MetaRecord._MetaRecords.SyncRoot)
            Try
                Dim MetaTags As Generic.Dictionary(Of MetaTags, String)

                If MetaRecord._MetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._MetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of MetaTags, String)
                End If

                If MetaTags.ContainsKey(MetaTag) Then MetaTags.Remove(MetaTag)
                MetaTags.Add(MetaTag, Value)

                MetaRecord._MetaRecords.Item(Helpers.CurrentRequestID) = MetaTags
            Finally
                Threading.Monitor.Exit(MetaRecord._MetaRecords.SyncRoot)
            End Try
        End Sub

        Public Shared Sub RemoveMetaRecord(ByVal MetaTag As MetaTags)
            Threading.Monitor.Enter(MetaRecord._MetaRecords.SyncRoot)
            Try
                Dim MetaTags As Generic.Dictionary(Of MetaTags, String)

                If MetaRecord._MetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._MetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of MetaTags, String)
                End If

                If MetaTags.ContainsKey(MetaTag) Then MetaTags.Remove(MetaTag)

                MetaRecord._MetaRecords.Item(Helpers.CurrentRequestID) = MetaTags
            Finally
                Threading.Monitor.Exit(MetaRecord._MetaRecords.SyncRoot)
            End Try
        End Sub

        Public Shared ReadOnly Property RegisteredMetaRecords() As Generic.Dictionary(Of MetaTags, String)
            Get
                Dim MetaTags As Generic.Dictionary(Of MetaTags, String)

                If MetaRecord._MetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._MetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of MetaTags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of MetaTags, String)
                End If

                Return MetaTags
            End Get
        End Property

        Public Shared ReadOnly Property RegisteredCustomMetaRecords() As Generic.Dictionary(Of String, String)
            Get
                Dim CustomMetaTags As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomMetaRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    CustomMetaTags = CType(MetaRecord._CustomMetaRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    CustomMetaTags = New Generic.Dictionary(Of String, String)
                End If

                Return CustomMetaTags
            End Get
        End Property

        Public Overloads Shared Function QueryMetaTagSpace(ByVal MetaTag As MetaTags) As MetaTagSpace
            Dim rMTS As MetaTagSpace

            Select Case MetaTag
                Case MetaTags.author, MetaTags.copyright, MetaTags.description, MetaTags.keywords, MetaTags.robots, MetaTags.googlebot
                    rMTS = MetaTagSpace.name
                Case Else
                    rMTS = MetaTagSpace.httpequiv
            End Select

            Return rMTS
        End Function

        Public Overloads Shared Function QueryMetaTagSpace(ByRef Name As String) As MetaTagSpace
            Dim rMTS As MetaTagSpace

            If Name Is Nothing Then Name = String.Empty

            If Name.IndexOf("name::") = 0 Then
                rMTS = MetaTagSpace.name

                Name = Name.Substring(6)
            ElseIf Name.IndexOf("httpequiv::") = 0 Then
                rMTS = MetaTagSpace.httpequiv

                Name = Name.Substring(11)
            End If

            Return rMTS
        End Function
    End Class
End Namespace