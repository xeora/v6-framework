Option Strict On

Namespace Xeora.Web.Global
    Public Class ContentDescription
        Private _Parts As Generic.List(Of String)

        Private Const TemplatePointerText As String = "!MESSAGETEMPLATE"
        Private _MessageTemplate As String

        Public Sub New(ByVal Content As String, ByVal ControlIDWithIndex As String)
            Me._Parts = New Generic.List(Of String)
            Me._MessageTemplate = String.Empty

            Me.PrepareDesciption(Content, ControlIDWithIndex)
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

                If ContentPart.IndexOf(ContentDescription.TemplatePointerText) = 0 Then
                    If String.IsNullOrEmpty(Me._MessageTemplate) Then
                        Me._MessageTemplate = ContentPart.Substring(ContentDescription.TemplatePointerText.Length)
                    Else
                        Throw New Exception.MultipleBlockException("Only One Message Template Block Allowed!")
                    End If
                Else
                    Me._Parts.Add(ContentPart)
                End If
            Loop Until sIdx = -1
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