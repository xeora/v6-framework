Option Strict On

Namespace Xeora.Web.Global
    Public Class ContentDescription
        Private Structure PartCache
            Public Content As String
            Public Parts As Generic.List(Of String)
            Public MessageTemplate As String
        End Structure
        Private Shared _PartsCache As New Concurrent.ConcurrentDictionary(Of String, PartCache)

        Private _Parts As Generic.List(Of String)

        Private Const TemplatePointerText As String = "!MESSAGETEMPLATE"
        Private _MessageTemplate As String

        Public Sub New(ByVal DirectiveInnerValue As String)
            ' Parse Block Content
            Dim FirstContentIndex As Integer =
                DirectiveInnerValue.IndexOf(":{")

            Dim DirectiveIdentifier As String =
                DirectiveInnerValue.Substring(0, FirstContentIndex)

            Dim BlockContent As String
            Dim ColonIndex As Integer =
                DirectiveIdentifier.IndexOf(":"c)

            If ColonIndex = -1 Then
                ' Special Directive such as PC, MB, XF
                BlockContent = DirectiveInnerValue
            Else
                ' Common Directive such as DirectiveType:ControlID
                BlockContent = DirectiveInnerValue.Substring(ColonIndex + 1)
            End If

            ' Update First Content Index
            FirstContentIndex = BlockContent.IndexOf(":{")

            ' ControlIDWithIndex Like ControlID~INDEX
            Dim ControlIDWithIndex As String = BlockContent.Substring(0, FirstContentIndex)

            Dim CoreContent As String = Nothing
            Dim idxCoreContStart As Integer, idxCoreContEnd As Integer

            Dim OpeningTag As String = String.Format("{0}:{{", ControlIDWithIndex)
            Dim ClosingTag As String = String.Format("}}:{0}", ControlIDWithIndex)

            idxCoreContStart = BlockContent.IndexOf(OpeningTag) + OpeningTag.Length
            idxCoreContEnd = BlockContent.LastIndexOf(ClosingTag, BlockContent.Length)

            If idxCoreContStart <> OpeningTag.Length OrElse idxCoreContEnd <> (BlockContent.Length - OpeningTag.Length) Then _
                Throw New Exception.ParseException()

            CoreContent = BlockContent.Substring(idxCoreContStart, idxCoreContEnd - idxCoreContStart)
            CoreContent = CoreContent.Trim()

            Me._Parts = New Generic.List(Of String)
            Me._MessageTemplate = String.Empty

            Dim PartCache As PartCache = Nothing
            If ContentDescription._PartsCache.TryGetValue(ControlIDWithIndex, PartCache) Then
                If String.Compare(PartCache.Content, CoreContent) = 0 Then
                    Me._Parts = PartCache.Parts
                    Me._MessageTemplate = PartCache.MessageTemplate

                    Exit Sub
                End If
            End If

            Me.PrepareDesciption(CoreContent, ControlIDWithIndex)
        End Sub

        Private Sub PrepareDesciption(ByVal Content As String, ByVal ControlIDWithIndex As String)
            Dim SearchString As String = String.Format("}}:{0}:{{", ControlIDWithIndex)
            Dim sIdx As Integer, cIdx As Integer = 0

            Dim ContentPart As String
            Do
                ContentPart = String.Empty
                sIdx = Content.IndexOf(SearchString, cIdx)

                If sIdx > -1 Then
                    ContentPart = Content.Substring(cIdx, sIdx - cIdx)

                    ' Set cIdx and Move Forward to Length of SearchString
                    cIdx = sIdx + SearchString.Length
                Else
                    ContentPart = Content.Substring(cIdx)
                End If
                ContentPart = ContentPart.Trim()

                If ContentPart.IndexOf(ContentDescription.TemplatePointerText) = 0 Then
                    If Not String.IsNullOrEmpty(Me._MessageTemplate) Then _
                        Throw New Exception.MultipleBlockException("Only One Message Template Block Allowed!")

                    Me._MessageTemplate = ContentPart.Substring(ContentDescription.TemplatePointerText.Length)
                Else
                    If Not String.IsNullOrEmpty(ContentPart) Then _
                        Me._Parts.Add(ContentPart)
                End If
            Loop Until sIdx = -1

            If Not Me.HasParts Then _
                Throw New Exception.EmptyBlockException()

            ' Cache Result
            Dim PartCache As PartCache
            With PartCache
                .Content = Content
                .Parts = Me._Parts
                .MessageTemplate = Me._MessageTemplate
            End With
            ContentDescription._PartsCache.TryAdd(ControlIDWithIndex, PartCache)
        End Sub

        Public ReadOnly Property Parts As Generic.List(Of String)
            Get
                Return Me._Parts
            End Get
        End Property

        Public ReadOnly Property HasParts As Boolean
            Get
                Return Me._Parts.Count > 0
            End Get
        End Property

        Public ReadOnly Property MessageTemplate As String
            Get
                Return Me._MessageTemplate
            End Get
        End Property

        Public ReadOnly Property HasMessageTemplate As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._MessageTemplate)
            End Get
        End Property
    End Class
End Namespace