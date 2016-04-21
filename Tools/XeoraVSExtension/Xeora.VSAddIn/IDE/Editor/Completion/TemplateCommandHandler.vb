Imports System.Runtime.InteropServices
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Language.Intellisense
Imports Microsoft.VisualStudio.OLE.Interop
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Editor

Namespace Xeora.VSAddIn.IDE.Editor.Completion
    Public Class TemplateCommandHandler
        Implements IOleCommandTarget

        Private _CurrentSession As ICompletionSession

        Private _Broker As ICompletionBroker
        Private _TextView As ITextView

        Public Enum Directives
            Control
            ControlWithLeveling
            ControlWithParent
            ControlWithLevelingAndParent
            Template
            Translation
            TemplateWithVariablePool
            ServerExecutable
            ClientExecutable
            InLineStatement
            UpdateBlock
            MessageBlock
            Special
        End Enum

        Public Sub New(ByVal broker As ICompletionBroker, ByVal textView As ITextView)
            Me._CurrentSession = Nothing

            Me._Broker = broker
            Me._TextView = textView

            TemplateCommandHandler.CurrentDirective = Directives.Special
            TemplateCommandHandler.CurrentDirectiveID = String.Empty
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

            TemplateCommandHandler.CurrentDirective = Directive

            Me._CurrentSession =
                Me._Broker.CreateCompletionSession(
                    Me._TextView,
                    snapshot.CreateTrackingPoint(Me._TextView.Caret.Position.BufferPosition, PointTrackingMode.Positive),
                    True
                )
            AddHandler Me._CurrentSession.Dismissed, AddressOf Me.OnSessionDismissed

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

            If pguidCmdGroup <> VSConstants.VSStd2K Then
                If pguidCmdGroup = Guid.Parse("5efc7975-14bc-11cf-9b2b-00aa00573819") Then
                    If CType(nCmdID, VSConstants.VSStd97CmdID) = VSConstants.VSStd97CmdID.Delete Then
                        GoTo QuickJumpForDelete
                    Else
                        Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                    End If
                Else
                    Return Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                End If
            End If

            Dim Handled As Boolean = False
            Dim rExecResult As Integer = VSConstants.S_OK

            Dim TrackingChars As Char() = New Char() {"$"c, ":"c, "?"c, "."c, "#"c, "["c}
            Dim OperatorChars As Char() = New Char() {"^"c, "~"c, "-"c, "+"c, "="c, "#"c, "*"c}
            Dim ch As Char = Me.GetTypedChar(pvaIn)

            Select Case CType(nCmdID, VSConstants.VSStd2KCmdID)
                Case VSConstants.VSStd2KCmdID.AUTOCOMPLETE, VSConstants.VSStd2KCmdID.COMPLETEWORD
                    Handled = Me.StartSession(Directives.Special)
                Case VSConstants.VSStd2KCmdID.RETURN, VSConstants.VSStd2KCmdID.TAB
                    Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(Char.MinValue, Char.MinValue))

                    Handled = Me.Complete()

                    If Not Handled Then Me._HandleFollowingAction = Nothing
                Case CType(103, VSConstants.VSStd2KCmdID) 'VSConstants.VSStd2KCmdID.CANCEL
                    Handled = Me.Cancel()
                Case Else
                    If ch <> Char.MinValue Then
                        If Not Me._CurrentSession Is Nothing AndAlso Not Me._CurrentSession.IsDismissed Then
                            If Array.IndexOf(TrackingChars, ch) > -1 Then
                                Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(ch, Char.MinValue))

                                Handled = Me.Complete()

                                If Not Handled Then Me._HandleFollowingAction = Nothing
                            ElseIf Char.IsWhiteSpace(ch) Then
                                Me._HandleFollowingAction = New Action(Sub() Me.HandleFollowingCompletion(Char.MinValue, Char.MinValue))

                                Handled = Me.Complete()

                                If Not Handled Then Me._HandleFollowingAction = Nothing
                            Else
                                If Array.IndexOf(OperatorChars, ch) > -1 Then
                                    Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                                    Handled = Me.Cancel()
                                End If
                            End If
                        Else
                            If Array.IndexOf(TrackingChars, ch) = -1 Then
                                If ch <> Char.MinValue Then
                                    Dim CursorIndex As Integer
                                    Dim StatementText As String = Me.ExtractXeoraStatement(Char.MinValue, CursorIndex)

                                    If Not String.IsNullOrEmpty(StatementText) Then
                                        Dim PageContent As String = Me._TextView.TextSnapshot.GetText()
                                        Dim RegEx As New System.Text.RegularExpressions.Regex("\$C\#\d*(\+)?")
                                        Dim LevelingMatch As System.Text.RegularExpressions.Match =
                                            RegEx.Match(PageContent, (Me._TextView.Caret.Position.BufferPosition - CursorIndex))

                                        If LevelingMatch.Success AndAlso
                                            LevelingMatch.Index = (Me._TextView.Caret.Position.BufferPosition.Position - CursorIndex) AndAlso
                                            LevelingMatch.Length >= CursorIndex Then

                                            If Char.IsDigit(ch) OrElse ch = "+"c OrElse ch = ":"c OrElse ch = "["c Then
                                                If Not Char.IsDigit(ch) AndAlso LevelingMatch.Length > 3 AndAlso Me._TextView.Selection.IsEmpty Then
                                                    If ch = "+" AndAlso CursorIndex = LevelingMatch.Length Then
                                                        If LevelingMatch.Value.IndexOf("+"c) = -1 Then _
                                                            Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                                    End If

                                                    If ch = ":"c OrElse ch = "["c Then
                                                        If LevelingMatch.Value.Chars(LevelingMatch.Length - 1) = "+"c Then
                                                            If LevelingMatch.Length > 4 Then Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                                        Else
                                                            If CursorIndex = LevelingMatch.Length Then _
                                                                Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                                        End If
                                                    End If
                                                Else
                                                    If Char.IsDigit(ch) Then _
                                                        Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                                End If
                                            End If

                                            Handled = True
                                        Else
                                            Handled = True

                                            rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                        End If
                                    Else
                                        Handled = True

                                        rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                    End If
                                Else
                                    Handled = True

                                    rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                End If
                            Else
                                If Not Me._TextView.Selection.IsEmpty Then
                                    Handled = IsNumeric(Me._TextView.Selection.SelectedSpans.Item(0).GetText())
                                End If
                            End If
                        End If
                    End If
            End Select

            If Not Handled Then
                Select Case CType(nCmdID, VSConstants.VSStd2KCmdID)
                    Case VSConstants.VSStd2KCmdID.TYPECHAR
                        If ch <> Char.MinValue Then
                            rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                            Select Case ch
                                Case "$"c
                                    Me.StartSession(Directives.Special)
                                Case "#"c, "["c, ":"c, "?"c, "."c
                                    Me.HandleFollowingCompletion(ch, ch)
                                Case Else
                                    Me.Filter()
                            End Select
                        End If
                    Case VSConstants.VSStd2KCmdID.BACKSPACE
QuickJumpForDelete:
                        Dim CursorIndex As Integer
                        Dim StatementText As String = Me.ExtractXeoraStatement(Char.MinValue, CursorIndex)

                        If Not String.IsNullOrEmpty(StatementText) Then
                            Dim PageContent As String = Me._TextView.TextSnapshot.GetText()
                            Dim RegEx As New System.Text.RegularExpressions.Regex("\$C\#\d*(\+)?")
                            Dim LevelingMatch As System.Text.RegularExpressions.Match =
                                RegEx.Match(PageContent, (Me._TextView.Caret.Position.BufferPosition - CursorIndex))

                            If LevelingMatch.Success AndAlso LevelingMatch.Index = (Me._TextView.Caret.Position.BufferPosition.Position - CursorIndex) Then
                                If pguidCmdGroup = Guid.Parse("5efc7975-14bc-11cf-9b2b-00aa00573819") Then
                                    ' It cames from DELETE key
                                    If CursorIndex = 2 OrElse (CursorIndex > 2 AndAlso CursorIndex + 1 >= LevelingMatch.Length) Then
                                        ' Delete Whole
                                        Dim Difference As Integer, Length As Integer

                                        If CursorIndex = 2 Then
                                            Difference = 0
                                        Else
                                            Difference = CursorIndex - 2
                                        End If
                                        Length = LevelingMatch.Length - 2

                                        Dim edit As ITextEdit =
                                            Me._TextView.TextBuffer.CreateEdit()

                                        edit.Replace(Me._TextView.Caret.Position.BufferPosition - Difference, Length, String.Empty)
                                        edit.Apply()
                                    Else
                                        ' Delete Char
                                        rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                    End If
                                Else
                                    ' BackSpace
                                    If CursorIndex = 3 OrElse CursorIndex = 4 Then
                                        ' Delete Whole
                                        Dim Difference As Integer, Length As Integer

                                        If CursorIndex = 2 Then
                                            Difference = 1
                                        Else
                                            Difference = CursorIndex - 2
                                        End If
                                        Length = LevelingMatch.Length - 2

                                        Dim edit As ITextEdit =
                                            Me._TextView.TextBuffer.CreateEdit()

                                        edit.Replace(Me._TextView.Caret.Position.BufferPosition - Difference, Length, String.Empty)
                                        edit.Apply()
                                    Else
                                        ' Delete Char
                                        rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                                    End If
                                End If
                            Else
                                rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
                            End If
                        Else
                            rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)

                            Me.Filter()
                        End If
                    Case Else
                        rExecResult = Me.NextCommandHandler.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut)
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

        Private Function ExtractXeoraStatement(ByRef SearchChar As Char, ByRef CursorIndex As Integer) As String
            Dim PageContent As String =
                Me._TextView.TextSnapshot.GetText()

            Dim caret As ITextCaret =
                Me._TextView.Caret
            Dim CurrentPosition As SnapshotPoint =
                caret.Position.BufferPosition

            Dim StatementText As String = String.Empty
            Dim TagIndex As Integer = PageContent.LastIndexOf("$", CurrentPosition)

            If TagIndex > -1 Then
                StatementText = PageContent.Substring(TagIndex, CurrentPosition - TagIndex)

                If StatementText.IndexOf("$"c) <> 0 OrElse
                    (StatementText.IndexOf("$"c) = 0 AndAlso StatementText.Length = 1) Then StatementText = String.Empty

                If Not String.IsNullOrEmpty(StatementText) Then
                    ' Control if Statement has whitespace!
                    For cC As Integer = 0 To StatementText.Length - 1
                        If Char.IsWhiteSpace(StatementText.Chars(cC)) Then
                            Return String.Empty
                        End If
                    Next

                    CursorIndex = StatementText.Length
                    If SearchChar = Char.MinValue Then _
                        SearchChar = StatementText.Chars(StatementText.Length - 1)

                    Dim MainPattern As New Xeora.Web.RegularExpressions.MainCapturePattern()

                    Dim StatementMatch As System.Text.RegularExpressions.Match =
                        MainPattern.Match(PageContent, TagIndex)

                    If StatementMatch.Success AndAlso StatementMatch.Index = 0 Then StatementText = StatementMatch.Value
                End If
            End If

            Return StatementText
        End Function

        Private Function GetDirective(ByVal SearchChar As Char) As Directives
            Dim rDirective As Directives = Directives.Special

            Dim StatementText As String = Me.ExtractXeoraStatement(SearchChar, 0)

            If SearchChar = "?"c OrElse SearchChar = "."c Then SearchChar = ":"c

            If Not String.IsNullOrEmpty(StatementText) Then
                Dim ColonIndex As Integer =
                    StatementText.IndexOf(SearchChar)

                If ColonIndex > 1 Then
                    Select Case StatementText.Substring(1, ColonIndex - 1)
                        Case "C"
                            rDirective = Directives.Control
                        Case "T"
                            rDirective = Directives.Template
                        Case "L"
                            rDirective = Directives.Translation
                        Case "P"
                            rDirective = Directives.TemplateWithVariablePool
                        Case "F"
                            rDirective = Directives.ServerExecutable
                        Case "XF"
                            rDirective = Directives.ClientExecutable
                        Case "S"
                            rDirective = Directives.InLineStatement
                        Case "H"
                            rDirective = Directives.UpdateBlock
                        Case "MB"
                            rDirective = Directives.MessageBlock
                    End Select
                End If

                If rDirective = Directives.Special AndAlso StatementText.Length > 2 Then
                    ' Check for Leveled and/or Parented Control
                    If StatementText.IndexOf("$C[") = 0 Then
                        ' This is Parented Control
                        rDirective = Directives.ControlWithParent
                    Else
                        If StatementText.Length > 4 Then
                            ' Control Leveled and Parented Control
                            Dim ItemMatch As System.Text.RegularExpressions.Match =
                                System.Text.RegularExpressions.Regex.Match(StatementText, "\$C\#\d+(\+)?(\[)?")

                            If ItemMatch.Success Then
                                If ItemMatch.Value.IndexOf("[") > -1 Then
                                    ' Control With Leveling and Parent
                                    rDirective = Directives.ControlWithLevelingAndParent
                                Else
                                    ' Control With Leveling
                                    rDirective = Directives.ControlWithLeveling
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Return rDirective
        End Function

        Private Function GetControlID() As String
            Dim StatementText As String = Me.ExtractXeoraStatement(Char.MinValue, 0)

            If Not String.IsNullOrEmpty(StatementText) Then
                Dim ColonIndex As Integer =
                    StatementText.IndexOf(":"c)

                If ColonIndex > -1 Then
                    StatementText = StatementText.Remove(0, ColonIndex + 1)

                    ColonIndex = StatementText.IndexOf(":"c)

                    If ColonIndex > -1 Then
                        StatementText = StatementText.Substring(0, ColonIndex)
                    Else
                        If String.IsNullOrWhiteSpace(StatementText) Then StatementText = String.Empty
                    End If
                Else
                    StatementText = String.Empty
                End If
            End If

            Return StatementText
        End Function

        Private Sub HandleFollowingCompletion(ByVal KeyChar As Char, ByVal SearchChar As Char)
            Me._HandleFollowingAction = Nothing

            Dim Directive As Directives =
                Me.GetDirective(SearchChar)

            Select Case Directive
                Case Directives.MessageBlock
                    Dim edit As ITextEdit =
                        Me._TextView.TextBuffer.CreateEdit()

                    edit.Insert(Me._TextView.Caret.Position.BufferPosition, "{}:MB$")
                    edit.Apply()

                    Me._TextView.Caret.MoveTo(Me._TextView.Caret.Position.BufferPosition - 5)
                Case Directives.UpdateBlock, Directives.InLineStatement
                    Dim ControlID As String =
                        Me.GetControlID()

                    If Not String.IsNullOrEmpty(ControlID) Then
                        Dim edit As ITextEdit =
                            Me._TextView.TextBuffer.CreateEdit()

                        edit.Insert(Me._TextView.Caret.Position.BufferPosition, String.Format("{{}}:{0}$", ControlID))
                        edit.Apply()

                        Me._TextView.Caret.MoveTo(Me._TextView.Caret.Position.BufferPosition - (ControlID.Length + 3))
                    End If
                Case Directives.Control, Directives.ControlWithLeveling, Directives.ControlWithParent, Directives.ControlWithLevelingAndParent
                    If KeyChar = "[" Then
                        Dim ControlIDTest As String = Me.GetControlID()
                        TemplateCommandHandler.CurrentDirectiveID = ControlIDTest

                        Dim edit As ITextEdit =
                            Me._TextView.TextBuffer.CreateEdit()

                        edit.Replace(Me._TextView.Caret.Position.BufferPosition - 1, 1, "[")
                        edit.Apply()

                        Me.StartSession(Directives.ControlWithParent)
                    ElseIf KeyChar = "#" Then
                        Dim edit As ITextEdit =
                            Me._TextView.TextBuffer.CreateEdit()

                        edit.Replace(Me._TextView.Caret.Position.BufferPosition - 1, 1, "#0")
                        edit.Apply()

                        Dim SelectionSpan As SnapshotSpan =
                            New SnapshotSpan(Me._TextView.Caret.Position.BufferPosition - 1, 1)
                        Me._TextView.Selection.Select(SelectionSpan, False)
                    Else
                        ' SearchChar is ":"

                        Dim ControlIDTest As String = Me.GetControlID()

                        If Not String.IsNullOrEmpty(ControlIDTest) Then
                            ' Control with Content
                            Dim edit As ITextEdit =
                                Me._TextView.TextBuffer.CreateEdit()

                            edit.Insert(Me._TextView.Caret.Position.BufferPosition, String.Format("{{}}:{0}$", ControlIDTest))
                            edit.Apply()

                            Me._TextView.Caret.MoveTo(Me._TextView.Caret.Position.BufferPosition - (ControlIDTest.Length + 3))
                        Else
                            Me.StartSession(Directives.Control)
                        End If
                    End If
                Case Directives.ServerExecutable
                    Dim StatementText As String =
                        Me.ExtractXeoraStatement(Char.MinValue, Nothing)

                    If Not String.IsNullOrEmpty(StatementText) Then _
                        StatementText = StatementText.Substring(3)

                    TemplateCommandHandler.CurrentDirectiveID = StatementText
                    Me.StartSession(Directive)
                Case Directives.ClientExecutable
                    Dim StatementText As String =
                        Me.ExtractXeoraStatement(Char.MinValue, Nothing)

                    If StatementText.Length = 4 Then
                        Dim edit As ITextEdit =
                            Me._TextView.TextBuffer.CreateEdit()

                        edit.Insert(Me._TextView.Caret.Position.BufferPosition, "{}:XF$")
                        edit.Apply()

                        Me._TextView.Caret.MoveTo(Me._TextView.Caret.Position.BufferPosition - 5)
                    Else
                        TemplateCommandHandler.CurrentDirectiveID = StatementText.Substring(5)
                    End If

                    Me.StartSession(Directives.ClientExecutable)
                Case Directives.Template, Directives.TemplateWithVariablePool
                    Dim TextDocument As ITextDocument = Nothing
                    If Me._TextView.TextBuffer.Properties.TryGetProperty(Of ITextDocument)(GetType(ITextDocument), TextDocument) Then
                        TemplateCommandHandler.CurrentDirectiveID = IO.Path.GetFileNameWithoutExtension(TextDocument.FilePath)
                    End If
                    Me.StartSession(Directive)
                Case Else
                    If Directive <> Directives.Special Then
                        Me.StartSession(Directive)
                    End If
            End Select
        End Sub

        Private Sub OnSessionDismissed(ByVal sender As Object, ByVal e As EventArgs)
            RemoveHandler Me._CurrentSession.Dismissed, AddressOf OnSessionDismissed

            Me._CurrentSession = Nothing

            If Not Me._HandleFollowingAction Is Nothing Then Me._HandleFollowingAction.Invoke()

            TemplateCommandHandler.CurrentDirective = Directives.Special
            TemplateCommandHandler.CurrentDirectiveID = String.Empty
        End Sub
    End Class
End Namespace