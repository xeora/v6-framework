Imports System.Collections.Generic
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Classification

Namespace Xeora.VSAddIn.IDE.Editor.Highlighter
    Public NotInheritable Class Syntax
        Implements IClassifier

        Private ReadOnly _ClassificationTypeForTagAndDirective As IClassificationType
        Private ReadOnly _ClassificationTypeForDirectiveID As IClassificationType
        Private ReadOnly _ClassificationTypeForInternalDirective As IClassificationType
        Private ReadOnly _ClassificationTypeForLeveling As IClassificationType
        Private ReadOnly _ClassificationTypeForBlackBracket As IClassificationType

        Public Event ClassificationChanged As EventHandler(Of ClassificationChangedEventArgs) Implements IClassifier.ClassificationChanged

        Public Sub New(ByVal registry As IClassificationTypeRegistryService)
            Me._ClassificationTypeForTagAndDirective =
                registry.GetClassificationType(SyntaxClassificationDefinition.TagAndDirective)
            Me._ClassificationTypeForDirectiveID =
                registry.GetClassificationType(SyntaxClassificationDefinition.DirectiveID)
            Me._ClassificationTypeForInternalDirective =
                registry.GetClassificationType(SyntaxClassificationDefinition.InternalDirective)
            Me._ClassificationTypeForLeveling =
                registry.GetClassificationType(SyntaxClassificationDefinition.Leveling)
            Me._ClassificationTypeForBlackBracket =
                registry.GetClassificationType(SyntaxClassificationDefinition.BlackBracket)
        End Sub

        Public Function GetClassificationSpans(ByVal span As SnapshotSpan) As IList(Of ClassificationSpan) Implements IClassifier.GetClassificationSpans
            Dim result As New List(Of ClassificationSpan)

            If Not PackageControl.IDEControl.CheckIsXeoraCubeProject() Then Return result
            If Not PackageControl.IDEControl.CheckIsXeoraTemplateFile() Then Return result

            Dim DraftValue As String = span.GetText()
            Dim PatternMatches As Text.RegularExpressions.MatchCollection
            Dim REMEnum As IEnumerator, MatchItem As Text.RegularExpressions.Match

            Dim MainPattern As New Xeora.Web.RegularExpressions.MainCapturePattern()
            Dim BracketedOpenPattern As New Xeora.Web.RegularExpressions.BracketedControllerOpenPattern()
            Dim BracketedClosePattern As New Xeora.Web.RegularExpressions.BracketedControllerClosePattern()
            Dim BracketedSeparatorPattern As New Xeora.Web.RegularExpressions.BracketedControllerSeparatorPattern()

            PatternMatches = MainPattern.Matches(DraftValue)

            If PatternMatches.Count > 0 Then
                REMEnum = PatternMatches.GetEnumerator()

                Dim IsHandled As Boolean
                Do While REMEnum.MoveNext()
                    IsHandled = False

                    MatchItem = CType(REMEnum.Current, Text.RegularExpressions.Match)

                    Dim OpenPatternMatch As Text.RegularExpressions.Match =
                        BracketedOpenPattern.Match(MatchItem.Value)

                    If OpenPatternMatch.Success Then
                        Dim ColonIndex As Integer =
                            MatchItem.Value.IndexOf(":"c)

                        ' Format Directive...
                        Dim LevelingSearchMatch As System.Text.RegularExpressions.Match =
                            System.Text.RegularExpressions.Regex.Match(MatchItem.Value.Substring(0, ColonIndex), "\$C(\#\d+(\+)?)?(\[)?")

                        If LevelingSearchMatch.Success Then
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index), 2)), Me._ClassificationTypeForTagAndDirective))

                            ' Leveling
                            If LevelingSearchMatch.Value.IndexOf("#"c) > -1 Then
                                Dim CloseTagIndex As Integer = LevelingSearchMatch.Value.IndexOf("["c)
                                If CloseTagIndex = -1 Then CloseTagIndex = LevelingSearchMatch.Length

                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 2), 1)), Me._ClassificationTypeForBlackBracket))
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 3), (CloseTagIndex - 3))), Me._ClassificationTypeForLeveling))
                            End If

                            If LevelingSearchMatch.Value.Chars(LevelingSearchMatch.Length - 1) = "["c Then
                                ' Has Parent
                                Dim CloseParentTagIndex As Integer = MatchItem.Value.IndexOf("]"c)

                                If CloseParentTagIndex > -1 Then
                                    result.Add(
                                        New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + LevelingSearchMatch.Length - 1), 1)), Me._ClassificationTypeForBlackBracket))
                                    result.Add(
                                        New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + LevelingSearchMatch.Length), (CloseParentTagIndex - LevelingSearchMatch.Length))), Me._ClassificationTypeForDirectiveID))
                                    result.Add(
                                        New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + CloseParentTagIndex), 1)), Me._ClassificationTypeForBlackBracket))
                                End If
                            End If
                        Else
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index), ColonIndex)), Me._ClassificationTypeForTagAndDirective))
                        End If

                        Dim EndColonIndex = MatchItem.Value.IndexOf(":{", ColonIndex + 1)
                        If EndColonIndex > -1 Then
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + ColonIndex + 1), (EndColonIndex - ColonIndex - 1))), Me._ClassificationTypeForDirectiveID))
                        End If

                        Dim CheckInternalDirective As String =
                            DraftValue.Substring(MatchItem.Index + MatchItem.Length)

                        If Not String.IsNullOrWhiteSpace(CheckInternalDirective) AndAlso
                            CheckInternalDirective.Length > 1 AndAlso CheckInternalDirective.Chars(0) = "!" Then

                            Dim InternalDirectiveMatch As Text.RegularExpressions.Match =
                                Text.RegularExpressions.Regex.Match(CheckInternalDirective, "\![A-Z]+")

                            If InternalDirectiveMatch.Success Then
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length), 1)), Me._ClassificationTypeForTagAndDirective))
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length + 1), (InternalDirectiveMatch.Length - 1))), Me._ClassificationTypeForInternalDirective))
                            End If
                        End If

                        IsHandled = True
                    End If

                    Dim ClosePatternMatch As Text.RegularExpressions.Match =
                        BracketedClosePattern.Match(MatchItem.Value)

                    If ClosePatternMatch.Success Then
                        Dim ColonIndex As Integer =
                            MatchItem.Value.IndexOf(":"c)
                        Dim ClosingID As String = MatchItem.Value.Substring(ColonIndex + 1, MatchItem.Length - ColonIndex - 2)

                        If String.Compare(ClosingID, "MB") = 0 OrElse
                            String.Compare(ClosingID, "XF") = 0 Then

                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + ColonIndex + 1), (MatchItem.Length - ColonIndex - 1))), Me._ClassificationTypeForTagAndDirective))
                        Else
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + ColonIndex + 1), (MatchItem.Length - ColonIndex - 2))), Me._ClassificationTypeForDirectiveID))
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length - 1), 1)), Me._ClassificationTypeForTagAndDirective))
                        End If

                        IsHandled = True
                    End If

                    Dim SeparatorPatternMatch As Text.RegularExpressions.Match =
                        BracketedSeparatorPattern.Match(MatchItem.Value)

                    If SeparatorPatternMatch.Success Then
                        result.Add(
                            New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 2), MatchItem.Length - 4)), Me._ClassificationTypeForDirectiveID))

                        Dim CheckInternalDirective As String =
                            DraftValue.Substring(MatchItem.Index + MatchItem.Length)

                        If Not String.IsNullOrWhiteSpace(CheckInternalDirective) AndAlso
                            CheckInternalDirective.Length > 1 AndAlso CheckInternalDirective.Chars(0) = "!" Then

                            Dim InternalDirectiveMatch As Text.RegularExpressions.Match =
                                Text.RegularExpressions.Regex.Match(CheckInternalDirective, "\![A-Z]+")

                            If InternalDirectiveMatch.Success Then
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length), 1)), Me._ClassificationTypeForTagAndDirective))
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length + 1), (InternalDirectiveMatch.Length - 1))), Me._ClassificationTypeForInternalDirective))
                            End If
                        End If

                        IsHandled = True
                    End If

                    If Not IsHandled Then
                        Dim ColonIndex As Integer =
                            MatchItem.Value.IndexOf(":"c)

                        If ColonIndex > -1 Then
                            ' Format Directive...
                            Dim LevelingSearchMatch As System.Text.RegularExpressions.Match =
                                System.Text.RegularExpressions.Regex.Match(MatchItem.Value.Substring(0, ColonIndex), "\$C(\#\d+(\+)?)?(\[)?")

                            If LevelingSearchMatch.Success Then
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index), 2)), Me._ClassificationTypeForTagAndDirective))

                                ' Leveling
                                If LevelingSearchMatch.Value.IndexOf("#"c) > -1 Then
                                    Dim CloseTagIndex As Integer = LevelingSearchMatch.Value.IndexOf("["c)
                                    If CloseTagIndex = -1 Then CloseTagIndex = LevelingSearchMatch.Length

                                    result.Add(
                                        New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 2), 1)), Me._ClassificationTypeForBlackBracket))
                                    result.Add(
                                        New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 3), (CloseTagIndex - 3))), Me._ClassificationTypeForLeveling))
                                End If

                                If LevelingSearchMatch.Value.Chars(LevelingSearchMatch.Length - 1) = "["c Then
                                    ' Has Parent
                                    Dim CloseParentTagIndex As Integer = MatchItem.Value.IndexOf("]"c)

                                    If CloseParentTagIndex > -1 Then
                                        result.Add(
                                            New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + LevelingSearchMatch.Length - 1), 1)), Me._ClassificationTypeForBlackBracket))
                                        result.Add(
                                            New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + LevelingSearchMatch.Length), (CloseParentTagIndex - LevelingSearchMatch.Length))), Me._ClassificationTypeForDirectiveID))
                                        result.Add(
                                            New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + CloseParentTagIndex), 1)), Me._ClassificationTypeForBlackBracket))
                                    End If
                                End If
                            Else
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index), ColonIndex)), Me._ClassificationTypeForTagAndDirective))
                            End If

                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + ColonIndex + 1), (MatchItem.Length - ColonIndex - 1))), Me._ClassificationTypeForDirectiveID))
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length - 1), 1)), Me._ClassificationTypeForTagAndDirective))
                        Else
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index), 1)), Me._ClassificationTypeForTagAndDirective))

                            Dim OperatorChars As Char() = New Char() {"^"c, "~"c, "-"c, "+"c, "="c, "#"c, "*"c}

                            Dim cC As Integer
                            For cC = 1 To MatchItem.Value.Length - 3
                                If Array.IndexOf(OperatorChars, MatchItem.Value.Chars(cC)) = -1 Then
                                    Exit For
                                End If
                            Next

                            If cC > 1 Then
                                result.Add(
                                    New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + 1), (cC - 1))), Me._ClassificationTypeForInternalDirective))
                            End If

                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + cC), (MatchItem.Length - (cC + 1)))), Me._ClassificationTypeForDirectiveID))
                            result.Add(
                                New ClassificationSpan(New SnapshotSpan(span.Snapshot, New Span((span.Start + MatchItem.Index + MatchItem.Length - 1), 1)), Me._ClassificationTypeForTagAndDirective))
                        End If
                    End If
                Loop
            End If

            Return result
        End Function
    End Class
End Namespace

