Imports Microsoft.VisualStudio.Language

Namespace Xeora.VSAddIn.IDE.Editor.Completion.SourceBuilder
    Public Class [Type]
        Inherits BuilderBase

        Public Overrides Function Build() As Intellisense.Completion()
            Dim CompList As New Generic.List(Of Intellisense.Completion)()

            Dim ControlTypeNames As String() =
                [Enum].GetNames(GetType(Globals.ControlTypes))

            For Each ControlTypeName As String In ControlTypeNames
                If String.Compare(ControlTypeName, Globals.ControlTypes.Unknown.ToString(), True) <> 0 Then
                    CompList.Add(New Intellisense.Completion(ControlTypeName, ControlTypeName, String.Empty, Me.ProvideImageSource(IconResource.controltype), Nothing))
                End If
            Next

            CompList.Sort(New CompletionComparer())

            Return CompList.ToArray()
        End Function

        Public Overrides Function Builders() As Intellisense.Completion()
            Return Nothing
        End Function
    End Class
End Namespace