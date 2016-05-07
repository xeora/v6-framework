Imports System.Runtime.InteropServices
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Language.Intellisense
Imports Microsoft.VisualStudio.OLE.Interop
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Editor

Namespace Xeora.VSAddIn.IDE.Editor.Completion
    Public Class ControlsCommandHandler
        Implements IOleCommandTarget

        Private _CurrentSession As ICompletionSession

        Private _Broker As ICompletionBroker
        Private _TextView As ITextView

        Public Enum Directives
            Tag

            Bind
            Control
            Type

            Special
            Translation
            TemplateWithVariablePool

            None
        End Enum

        Public Sub New(ByVal broker As ICompletionBroker, ByVal textView As ITextView)
            Me._CurrentSession = Nothing

            Me._Broker = broker
            Me._TextView = textView

            ControlsCommandHandler.CurrentDirective = Directives.None
            ControlsCommandHandler.CurrentDirectiveID = String.Empty
        End Sub

        Public Property NextCommandHandler As IOleCommandTarget

        Public Shared Property CurrentDirective As Directives
        Public Shared Property CurrentDirectiveID As String
        Private _HandleFollowingAction As Action

        Private Function StartSession(ByVal Directive As Directives) As Boolean
            If Not Me._CurrentSession Is Nothing OrElse (Not Me._CurrentSession Is Nothing AndAlso Me._CurrentSession.IsStarted) Then Return False

            Me._Broker.DismissAllSessions(Me._TextView)

            Dim snapshot As ITextSnapshot =
                Me._TextView.Caret.Position.BufferPosition.Snapshot

            ControlsCommandHandler.CurrentDirective = Directive

            Me._CurrentSession =
                Me._Broker.CreateCompletionSession(
                    Me._TextView,
                    snapshot.CreateTrackingPoint(Me._TextView.Caret.Position.BufferPosition, PointTrackingMode.Positive),
                    True
                )
            AddHandler Me._CurrentSession.Dismissed, AddressOf Me.OnSessionDismissed
            AddHandler Me._CurrentSession.Committed, AddressOf Me.OnSessionCommitted

            Me._CurrentSession.Start()

            Return True
        End Function

        Private Function Complete() As Boolean
            If Me._CurrentSession Is Nothing OrElse (Not Me._CurrentSession Is Nothing AndAlso Me._CurrentSession.IsDismissed) Then Return False

            Me._CurrentSession.Commit()

            Return True
        End Function

        Private Function Cancel() As Boolean
            If Me._CurrentSession Is Nothing OrElse (Not Me._CurrentSession Is Nothing AndAlso Me._CurrentSession.IsDismissed) Then Return False

            Me._CurrentSession.Dismiss()

            Return True
        End Function

        Private Sub Filter()
            If Me._CurrentSession Is Nothing OrElse (Not Me._CurrentSession Is Nothing AndAlso Me._CurrentSession.IsDismissed) Then Exit Sub

            Me._CurrentSession.SelectedCompletionSet.SelectBestMatch()
            Me._CurrentSession.SelectedCompletionSet.Recalculate()
        End Sub

        Private Function GetTypedChar(ByVal pvaIn As IntPtr) As Char
            Dim rChar As Char = Char.MinValue

            If pvaIn <> IntPtr.Zero Then _
                rChar = ChrW(CType(Marshal.GetObjectForNativeVariant(pvaIn), UShort))

            Return rChar
        End Function

        Public Function Exec(ByRef pguidCmdGroup As Guid, ByVal nCmdID As UInteger, ByVal nCmdexecopt As UInteger, ByVal pvaIn As IntPtr, ByVal pvaOut As IntPtr) As Integer Implements IOleCommandTarget.Exec
            If Not PackageControl.IDEControl.CheckIsXeoraCubeProject() Then Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
            If Not PackageControl.IDEControl.CheckIsXeoraTemplateFile() Then Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

            Dim TextDocument As ITextDocument = Nothing
            If Me._TextView.TextBuffer.Properties.TryGetProperty(Of ITextDocument)(GetType(ITextDocument), TextDocument) Then
                Dim WorkingFileName As String =
                    IO.Path.GetFileName(TextDocument.FilePath)

                If String.Compare(WorkingFileName, "Controls.xml", True) <> 0 Then _
                    Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
            Else
                Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
            End If

            Dim Handled As Boolean = False
            Dim rExecResult As Integer = VSConstants.S_OK

            Dim TrackingChars As Char() = New Char() {">"c, "?"c, "."c, "$"c, ":"c}
            Dim ch As Char = Me.GetTypedChar(pvaIn)

            Select Case CType(nCmdID, VSConstants.VSStd2KCmdID)
                Case VSConstants.VSStd2KCmdID.AUTOCOMPLETE, VSConstants.VSStd2KCmdID.COMPLETEWORD
                    Me.HandleFollowingCompletion(Char.MinValue)

                    Handled = True
                Case VSConstants.VSStd2KCmdID.RETURN, VSConstants.VSStd2KCmdID.TAB
                    Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(Char.MinValue))

                    Handled = Me.Complete()

                    If Not Handled Then Me._HandleFollowingAction = Nothing
                Case CType(103, VSConstants.VSStd2KCmdID) 'VSConstants.VSStd2KCmdID.CANCEL
                    Handled = Me.Cancel()
                Case Else
                    If ch <> Char.MinValue Then
                        If Not Me._CurrentSession Is Nothing AndAlso Not Me._CurrentSession.IsDismissed Then
                            If Array.IndexOf(TrackingChars, ch) > -1 Then
                                Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(CType(IIf(ch = ":"c, ch, Char.MinValue), Char)))

                                Handled = Me.Complete()

                                If Not Handled Then Me._HandleFollowingAction = Nothing
                            ElseIf Char.IsWhiteSpace(ch) Then
                                Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(Char.MinValue))

                                Handled = Me.Complete()

                                If Not Handled Then Me._HandleFollowingAction = Nothing
                            End If
                        End If
                    End If
            End Select

            If Not Handled Then
                Select Case CType(nCmdID, VSConstants.VSStd2KCmdID)
                    Case VSConstants.VSStd2KCmdID.TYPECHAR
                        If ch <> Char.MinValue Then
                            Select Case ch
                                Case "<"c, "$"c
                                    Dim edit As ITextEdit =
                                        Me._TextView.TextBuffer.CreateEdit()

                                    If Not Me._TextView.Selection.IsEmpty Then
                                        edit.Replace(Me._TextView.Selection.SelectedSpans.Item(0), ch)
                                    Else
                                        edit.Insert(Me._TextView.Caret.Position.BufferPosition, ch)
                                    End If
                                    edit.Apply()

                                    If ch = "<"c Then StartSession(Directives.Tag)
                                    If ch = "$"c Then StartSession(Directives.Special)
                                Case ">"c, "?"c, "."c, "$"c, ":"c
                                    rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                                    HandleFollowingCompletion(Char.MinValue)
                                Case Else
                                    rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                                    Me.Filter()
                            End Select
                        End If
                    Case VSConstants.VSStd2KCmdID.BACKSPACE
                        rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                        Me.Filter()
                    Case Else
                        Try
                            rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                        Catch ex As Exception
                            ' Just Handle Exceptions
                        End Try
                End Select
            End If

            Return rExecResult
        End Function

        Public Function QueryStatus(ByRef pguidCmdGroup As Guid, ByVal cCmds As UInteger, ByVal prgCmds() As OLECMD, ByVal pCmdText As IntPtr) As Integer Implements IOleCommandTarget.QueryStatus
            If pguidCmdGroup = VSConstants.VSStd2K Then
                Select Case CType(prgCmds(0).cmdID, VSConstants.VSStd2KCmdID)
                    Case VSConstants.VSStd2KCmdID.AUTOCOMPLETE, VSConstants.VSStd2KCmdID.COMPLETEWORD
                        prgCmds(0).cmdf = CType(OLECMDF.OLECMDF_ENABLED, UInteger) Or CType(OLECMDF.OLECMDF_SUPPORTED, UInteger)

                        Return VSConstants.S_OK
                End Select
            End If

            Return Me.NextCommandHandler.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText)
        End Function

        Private Function ExtractXeoraStatement(ByVal SearchChar As Char, ByRef DirectiveType As Directives) As String
            DirectiveType = Directives.None

            Dim PageContent As String =
                Me._TextView.TextSnapshot.GetText()

            Dim caret As ITextCaret =
                Me._TextView.Caret
            Dim CurrentPosition As SnapshotPoint =
                caret.Position.BufferPosition

            If SearchChar = Char.MinValue Then _
                SearchChar = "$"c
            Dim StatementText As String = String.Empty
            Dim TagIndex As Integer = PageContent.LastIndexOf(SearchChar, CurrentPosition - 1)
            If TagIndex = -1 Then
                SearchChar = "<"c
                TagIndex = PageContent.LastIndexOf(SearchChar, CurrentPosition - 1)
            End If

            If TagIndex > -1 Then
                StatementText = PageContent.Substring(TagIndex, CurrentPosition - TagIndex)

                If SearchChar = "$"c Then
                    If StatementText.IndexOf("$"c) <> 0 OrElse
                        (StatementText.IndexOf("$"c) = 0 AndAlso StatementText.Length = 1) Then StatementText = String.Empty
                End If

                If Not String.IsNullOrEmpty(StatementText) Then
                    ' Control if Statement has whitespace!
                    For cC As Integer = 0 To StatementText.Length - 1
                        If Char.IsWhiteSpace(StatementText.Chars(cC)) Then
                            Return String.Empty
                        End If
                    Next

                    If SearchChar = "$"c Then
                        If SearchChar = Char.MinValue Then _
                            SearchChar = StatementText.Chars(StatementText.Length - 1)

                        'Dim MainPattern As New Xeora.Web.RegularExpressions.MainCapturePattern()

                        'Dim StatementMatch As System.Text.RegularExpressions.Match =
                        '    MainPattern.Match(PageContent, TagIndex)

                        'If StatementMatch.Success AndAlso StatementMatch.Index = 0 Then
                        'StatementText = StatementMatch.Value

                        Dim ColonIndex As Integer = StatementText.IndexOf(":"c)

                            If ColonIndex > -1 Then
                                Select Case StatementText.Substring(1, ColonIndex - 1)
                                    Case "P"
                                        DirectiveType = Directives.TemplateWithVariablePool
                                    Case "L"
                                        DirectiveType = Directives.Translation
                                End Select
                            End If
                        'End If
                    Else
                        Dim Patterns As String() = New String() {"\<Bind\>", "\<DefaultButtonID\>", "\<Type\>"}
                        For p As Integer = 0 To Patterns.Length - 1
                            Dim mI As System.Text.RegularExpressions.Match =
                                System.Text.RegularExpressions.Regex.Match(StatementText, Patterns(p), System.Text.RegularExpressions.RegexOptions.RightToLeft)

                            If mI.Success AndAlso mI.Index = 0 Then
                                StatementText = StatementText.Substring(mI.Length)

                                Select Case p
                                    Case 0
                                        DirectiveType = Directives.Bind
                                    Case 1
                                        DirectiveType = Directives.Control
                                    Case 2
                                        DirectiveType = Directives.Type
                                    Case Else
                                        DirectiveType = Directives.None
                                End Select

                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            Return StatementText
        End Function

        Private Sub HandleFollowingCompletion(ByVal SearchChar As Char)
            Me._HandleFollowingAction = Nothing

            Dim Directive As Directives
            Dim StatementText As String =
                Me.ExtractXeoraStatement(SearchChar, Directive)

            Select Case Directive
                Case Directives.Bind
                    ControlsCommandHandler.CurrentDirectiveID = StatementText

                    Me.StartSession(Directives.Bind)
                Case Directives.Control
                    If String.IsNullOrEmpty(StatementText) Then _
                        Me.StartSession(Directives.Control)
                Case Directives.Type
                    If String.IsNullOrEmpty(StatementText) Then _
                        Me.StartSession(Directives.Type)

                    ' Xeora Tag Requests
                Case Directives.Special
                    Dim TestStatementText As String =
                        Me.ExtractXeoraStatement("<"c, Directive)

                    If Directive = Directives.None Then _
                        Me.StartSession(Directives.Special)
                Case Directives.TemplateWithVariablePool
                    Dim TestStatementText As String =
                        Me.ExtractXeoraStatement("<"c, Directive)

                    If Directive = Directives.None Then _
                        Me.StartSession(Directives.TemplateWithVariablePool)
                Case Directives.Translation
                    Dim TestStatementText As String =
                        Me.ExtractXeoraStatement("<"c, Directive)

                    If Directive = Directives.None Then _
                        Me.StartSession(Directives.Translation)
                Case Else
                    ControlsCommandHandler.CurrentDirective = Directives.None
                    ControlsCommandHandler.CurrentDirectiveID = String.Empty
            End Select
        End Sub

        Private Sub OnSessionCommitted(sender As Object, e As EventArgs)
            RemoveHandler Me._CurrentSession.Committed, AddressOf OnSessionCommitted

            If ControlsCommandHandler.CurrentDirective = Directives.Tag Then
                Dim PageContent As String =
                    Me._TextView.TextSnapshot.GetText()
                Dim CurrentPosition As SnapshotPoint =
                    Me._TextView.Caret.Position.BufferPosition

                Dim SearchRegex As New System.Text.RegularExpressions.Regex("\<\w+\>\<\/\w+\>")
                Dim mIs As System.Text.RegularExpressions.MatchCollection =
                    SearchRegex.Matches(PageContent.Substring(0, CurrentPosition))

                If mIs.Count > 0 Then
                    Dim m As System.Text.RegularExpressions.Match =
                        mIs(mIs.Count - 1)

                    Me._TextView.Caret.MoveTo(New SnapshotPoint(Me._TextView.TextSnapshot, (m.Index + (m.Length \ 2))))
                End If
            End If
        End Sub

        Private Sub OnSessionDismissed(ByVal sender As Object, ByVal e As EventArgs)
            RemoveHandler Me._CurrentSession.Dismissed, AddressOf OnSessionDismissed

            Me._CurrentSession = Nothing

            If Not Me._HandleFollowingAction Is Nothing Then Me._HandleFollowingAction.Invoke()

            ControlsCommandHandler.CurrentDirective = Directives.None
            ControlsCommandHandler.CurrentDirectiveID = String.Empty
        End Sub
    End Class
End Namespace