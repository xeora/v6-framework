Option Strict On

Namespace Xeora.Web.Controller
    Public Class RenderlessController
        Inherits ControllerBase

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControllerTypes.Renderless, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            ' Change ~/ values with the exact application root path
            Dim RootPathMatches As Text.RegularExpressions.MatchCollection =
                Text.RegularExpressions.Regex.Matches(Me.DraftValue, "[""']+(~|¨)/", Text.RegularExpressions.RegexOptions.Multiline)
            Dim ApplicationRoot As String = [Shared].Configurations.ApplicationRoot.BrowserImplementation
            Dim VirtualRoot As String = [Shared].Configurations.VirtualRoot

            Dim WorkingValue As New Text.StringBuilder()
            Dim LastIndex As Integer = 0

            Dim Enumerator As IEnumerator = RootPathMatches.GetEnumerator()
            Do While Enumerator.MoveNext()
                Dim MatchItem As Text.RegularExpressions.Match =
                    CType(Enumerator.Current, Text.RegularExpressions.Match)

                WorkingValue.Append(Me.DraftValue.Substring(LastIndex, MatchItem.Index - LastIndex))

                If MatchItem.Value.IndexOf("~") > -1 Then
                    ' ApplicationRoot Match
                    WorkingValue.AppendFormat("{0}{1}", MatchItem.Value.Substring(0, 1), ApplicationRoot)
                Else
                    ' VirtualRoot Match
                    WorkingValue.AppendFormat("{0}{1}", MatchItem.Value.Substring(0, 1), VirtualRoot)
                End If

                LastIndex = MatchItem.Index + MatchItem.Length
            Loop
            WorkingValue.Append(Me.DraftValue.Substring(LastIndex))

            If WorkingValue.Length > 0 Then
                ' This is renderless content
                Me.DefineRenderedValue(WorkingValue.ToString())
            Else
                ' This parented controlbase
                Me.DefineRenderedValue(Me.Create())
            End If
        End Sub
    End Class
End Namespace