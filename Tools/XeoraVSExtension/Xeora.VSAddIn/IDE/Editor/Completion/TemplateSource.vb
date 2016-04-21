Imports System.Collections.Generic
Imports Microsoft.VisualStudio.Language
Imports Microsoft.VisualStudio.Text

Namespace Xeora.VSAddIn.IDE.Editor.Completion
    Public Class TemplateSource
        Implements Intellisense.ICompletionSource

        Private _SourceProvider As TemplateSourceProvider
        Private _TextBuffer As ITextBuffer

        Public Sub New(ByVal SourceProvider As TemplateSourceProvider, ByVal textBuffer As ITextBuffer)
            Me._SourceProvider = SourceProvider
            Me._TextBuffer = textBuffer
        End Sub

        Public Sub AugmentCompletionSession(ByVal session As Intellisense.ICompletionSession, ByVal completionSets As IList(Of Intellisense.CompletionSet)) Implements Intellisense.ICompletionSource.AugmentCompletionSession
            Dim CompList As Intellisense.Completion() = Nothing
            Dim Builders As Intellisense.Completion() = Nothing

            Select Case TemplateCommandHandler.CurrentDirective
                Case TemplateCommandHandler.Directives.Special
                    Dim Special As New SourceBuilder.Special()

                    CompList = Special.Build()
                Case TemplateCommandHandler.Directives.Template, TemplateCommandHandler.Directives.TemplateWithVariablePool
                    Dim Template As New SourceBuilder.Template()

                    Template.WorkingTemplateID = TemplateCommandHandler.CurrentDirectiveID
                    CompList = Template.Build()
                    Builders = Template.Builders()
                Case TemplateCommandHandler.Directives.Control, TemplateCommandHandler.Directives.ControlWithLeveling
                    Dim Control As New SourceBuilder.Control()

                    CompList = Control.Build()
                    Builders = Control.Builders()
                Case TemplateCommandHandler.Directives.ControlWithParent, TemplateCommandHandler.Directives.ControlWithLevelingAndParent
                    Dim ControlWP As New SourceBuilder.Control(session, Me._TextBuffer)

                    ControlWP.WorkingControlID = TemplateCommandHandler.CurrentDirectiveID
                    CompList = ControlWP.Build()
                    Builders = Nothing

                    If CompList.Length = 0 Then
                        session.Dismiss()

                        Exit Sub
                    End If
                Case TemplateCommandHandler.Directives.Translation
                    Dim Translation As New SourceBuilder.Translation()

                    CompList = Translation.Build()
                    Builders = Translation.Builders()
                Case TemplateCommandHandler.Directives.ServerExecutable, TemplateCommandHandler.Directives.ClientExecutable
                    If String.IsNullOrEmpty(TemplateCommandHandler.CurrentDirectiveID) Then
                        Dim Executable As New SourceBuilder.Executable()

                        CompList = Executable.Build()
                        Builders = Executable.Builders()
                    Else
                        Dim [Class] As New SourceBuilder.Class()

                        [Class].WorkingExecutableInfo = TemplateCommandHandler.CurrentDirectiveID
                        [Class].IsClientExecutable = (TemplateCommandHandler.CurrentDirective = TemplateCommandHandler.Directives.ClientExecutable)

                        CompList = [Class].Build()
                        Builders = [Class].Builders()
                    End If
            End Select

            If Not CompList Is Nothing Then
                completionSets.Add(
                    New Intellisense.CompletionSet("XeoraTemplate", "Xeora³", Me.FindSpanAtPosition(session), CompList, Builders)
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