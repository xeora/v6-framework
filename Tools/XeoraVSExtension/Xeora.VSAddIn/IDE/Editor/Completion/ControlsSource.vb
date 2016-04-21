Imports System.Collections.Generic
Imports Microsoft.VisualStudio.Language
Imports Microsoft.VisualStudio.Text

Namespace Xeora.VSAddIn.IDE.Editor.Completion
    Public Class ControlsSource
        Implements Intellisense.ICompletionSource

        Private _SourceProvider As ControlsSourceProvider
        Private _TextBuffer As ITextBuffer

        Public Sub New(ByVal SourceProvider As ControlsSourceProvider, ByVal textBuffer As ITextBuffer)
            Me._SourceProvider = SourceProvider
            Me._TextBuffer = textBuffer
        End Sub

        Public Sub AugmentCompletionSession(ByVal session As Intellisense.ICompletionSession, ByVal completionSets As IList(Of Intellisense.CompletionSet)) Implements Intellisense.ICompletionSource.AugmentCompletionSession
            Dim CompList As Intellisense.Completion() = Nothing
            Dim Builders As Intellisense.Completion() = Nothing

            Select Case ControlsCommandHandler.CurrentDirective
                Case ControlsCommandHandler.Directives.Tag
                    Dim ControlTag As New SourceBuilder.ControlTag(session, Me._TextBuffer)

                    CompList = ControlTag.Build()
                    Builders = ControlTag.Builders()
                Case ControlsCommandHandler.Directives.Bind
                    If String.IsNullOrEmpty(ControlsCommandHandler.CurrentDirectiveID) Then
                        Dim Executable As New SourceBuilder.Executable()

                        CompList = Executable.Build()
                        Builders = Executable.Builders()
                    Else
                        Dim [Class] As New SourceBuilder.Class()

                        [Class].WorkingExecutableInfo = ControlsCommandHandler.CurrentDirectiveID
                        [Class].IsClientExecutable = True

                        CompList = [Class].Build()
                        Builders = [Class].Builders()
                    End If
                Case ControlsCommandHandler.Directives.Control
                    Dim Control As New SourceBuilder.Control()

                    Control.WorkingControlID = SourceBuilder.ControlTag.CurrentControlID(session, Me._TextBuffer)
                    Control.OnlyButtons = True

                    CompList = Control.Build()
                    Builders = Control.Builders()
                Case ControlsCommandHandler.Directives.Type
                    Dim [Type] As New SourceBuilder.Type()

                    CompList = [Type].Build()
                    Builders = [Type].Builders()

                    ' Xeora Tag Requests
                Case ControlsCommandHandler.Directives.Special
                    Dim Special As New SourceBuilder.Special()

                    Special.RequestFromControl = True

                    CompList = Special.Build()
                Case ControlsCommandHandler.Directives.TemplateWithVariablePool
                    Dim Template As New SourceBuilder.Template()

                    CompList = Template.Build()
                    Builders = Template.Builders()
                Case ControlsCommandHandler.Directives.Translation
                    Dim Translation As New SourceBuilder.Translation()

                    CompList = Translation.Build()
                    Builders = Translation.Builders()
            End Select

            If Not CompList Is Nothing Then
                completionSets.Add(
                    New Intellisense.CompletionSet("XeoraControls", "Xeora³", Me.FindSpanAtPosition(session), CompList, Builders)
                )
            End If
        End Sub

        Private Function FindSpanAtPosition(ByVal session As Intellisense.ICompletionSession) As ITrackingSpan
            Dim currentPoint As SnapshotPoint =
                session.TextView.Caret.Position.BufferPosition

            'Dim navigator As ITextStructureNavigator =
            '    Me._SourceProvider.NavigatorService.GetTextStructureNavigator(Me._TextBuffer)
            'Dim extent As TextExtent =
            '    navigator.GetExtentOfWord(currentPoint)
            'extent.Span.Start, extent.Span.Length - 1
            Return currentPoint.Snapshot.CreateTrackingSpan(currentPoint, 0, SpanTrackingMode.EdgeInclusive)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    GC.SuppressFinalize(Me)
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace