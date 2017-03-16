Option Strict On

Namespace Xeora.Web.Shared
    Public Class MetaRecord
        Public Shared _Records As Hashtable = Hashtable.Synchronized(New Hashtable)
        Public Shared _CustomRecords As Hashtable = Hashtable.Synchronized(New Hashtable)

        Public Enum Tags
            author
            cachecontrol
            contentlanguage
            contenttype
            copyright
            description
            expires
            keywords
            pragma
            refresh
            robots
            googlebot
        End Enum

        Public Enum TagSpaces
            name
            httpequiv
            [property]
        End Enum

        Public Shared Function GetTagHtmlName(ByVal Tag As Tags) As String
            Dim rString As String = String.Empty

            Select Case Tag
                Case Tags.author
                    rString = "Author"
                Case Tags.cachecontrol
                    rString = "Cache-Control"
                Case Tags.contentlanguage
                    rString = "Content-Language"
                Case Tags.contenttype
                    rString = "Content-Type"
                Case Tags.copyright
                    rString = "Copyright"
                Case Tags.description
                    rString = "Description"
                Case Tags.expires
                    rString = "Expires"
                Case Tags.googlebot
                    rString = "Googlebot"
                Case Tags.keywords
                    rString = "Keywords"
                Case Tags.pragma
                    rString = "Pragma"
                Case Tags.refresh
                    rString = "Refresh"
                Case Tags.robots
                    rString = "Robots"
            End Select

            Return rString
        End Function

        Public Overloads Shared Sub Add(ByVal TagSpace As TagSpaces, ByVal Name As String, ByVal Value As String)
            If String.IsNullOrEmpty(Name) Then _
                Throw New NullReferenceException("Name can not be null!")

            If String.IsNullOrEmpty(Value) Then Value = String.Empty

            Threading.Monitor.Enter(MetaRecord._CustomRecords.SyncRoot)
            Try
                Select Case TagSpace
                    Case TagSpaces.name
                        Name = String.Format("name::{0}", Name)
                    Case TagSpaces.httpequiv
                        Name = String.Format("httpequiv::{0}", Name)
                    Case TagSpaces.property
                        Name = String.Format("property::{0}", Name)
                End Select

                Dim cMRs As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    cMRs = CType(MetaRecord._CustomRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    cMRs = New Generic.Dictionary(Of String, String)
                End If

                If cMRs.ContainsKey(Name) Then cMRs.Remove(Name)
                cMRs.Add(Name, Value)

                MetaRecord._CustomRecords.Item(Helpers.CurrentRequestID) = cMRs
            Finally
                Threading.Monitor.Exit(MetaRecord._CustomRecords.SyncRoot)
            End Try
        End Sub

        Public Overloads Shared Sub Add(ByVal Tag As Tags, ByVal Value As String)
            If String.IsNullOrEmpty(Value) Then Value = String.Empty

            Threading.Monitor.Enter(MetaRecord._Records.SyncRoot)
            Try
                Dim MetaTags As Generic.Dictionary(Of Tags, String)

                If MetaRecord._Records.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._Records.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of Tags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of Tags, String)
                End If

                If MetaTags.ContainsKey(Tag) Then MetaTags.Remove(Tag)
                MetaTags.Add(Tag, Value)

                MetaRecord._Records.Item(Helpers.CurrentRequestID) = MetaTags
            Finally
                Threading.Monitor.Exit(MetaRecord._Records.SyncRoot)
            End Try
        End Sub

        Public Overloads Shared Sub Remove(ByVal Name As String)
            If String.IsNullOrEmpty(Name) Then _
                Throw New NullReferenceException("Name can not be null!")

            Threading.Monitor.Enter(MetaRecord._CustomRecords.SyncRoot)
            Try
                Dim cMRs As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    cMRs = CType(MetaRecord._CustomRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    cMRs = New Generic.Dictionary(Of String, String)
                End If

                If cMRs.ContainsKey(Name) Then cMRs.Remove(Name)

                MetaRecord._CustomRecords.Item(Helpers.CurrentRequestID) = cMRs
            Finally
                Threading.Monitor.Exit(MetaRecord._CustomRecords.SyncRoot)
            End Try
        End Sub

        Public Overloads Shared Sub Remove(ByVal Tag As Tags)
            Threading.Monitor.Enter(MetaRecord._Records.SyncRoot)
            Try
                Dim MetaTags As Generic.Dictionary(Of Tags, String)

                If MetaRecord._Records.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._Records.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of Tags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of Tags, String)
                End If

                If MetaTags.ContainsKey(Tag) Then MetaTags.Remove(Tag)

                MetaRecord._Records.Item(Helpers.CurrentRequestID) = MetaTags
            Finally
                Threading.Monitor.Exit(MetaRecord._Records.SyncRoot)
            End Try
        End Sub

        Public Shared ReadOnly Property RegisteredRecords() As Generic.Dictionary(Of Tags, String)
            Get
                Dim MetaTags As Generic.Dictionary(Of Tags, String)

                If MetaRecord._Records.ContainsKey(Helpers.CurrentRequestID) Then
                    MetaTags = CType(MetaRecord._Records.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of Tags, String))
                Else
                    MetaTags = New Generic.Dictionary(Of Tags, String)
                End If

                Return MetaTags
            End Get
        End Property

        Public Shared ReadOnly Property RegisteredCustomRecords() As Generic.Dictionary(Of String, String)
            Get
                Dim CustomTags As Generic.Dictionary(Of String, String)

                If MetaRecord._CustomRecords.ContainsKey(Helpers.CurrentRequestID) Then
                    CustomTags = CType(MetaRecord._CustomRecords.Item(Helpers.CurrentRequestID), Generic.Dictionary(Of String, String))
                Else
                    CustomTags = New Generic.Dictionary(Of String, String)
                End If

                Return CustomTags
            End Get
        End Property

        Public Overloads Shared Function QueryTagSpace(ByVal Tag As Tags) As TagSpaces
            Dim rMTS As TagSpaces

            Select Case Tag
                Case Tags.author, Tags.copyright, Tags.description, Tags.keywords, Tags.robots, Tags.googlebot
                    rMTS = TagSpaces.name
                Case Else
                    rMTS = TagSpaces.httpequiv
            End Select

            Return rMTS
        End Function

        Public Overloads Shared Function QueryTagSpace(ByRef Name As String) As TagSpaces
            Dim rMTS As TagSpaces

            If String.IsNullOrEmpty(Name) Then Name = String.Empty

            If Name.IndexOf("name::") = 0 Then
                rMTS = TagSpaces.name

                Name = Name.Substring(6)
            ElseIf Name.IndexOf("httpequiv::") = 0 Then
                rMTS = TagSpaces.httpequiv

                Name = Name.Substring(11)
            ElseIf Name.IndexOf("property::") = 0 Then
                rMTS = TagSpaces.property

                Name = Name.Substring(10)
            End If

            Return rMTS
        End Function
    End Class
End Namespace