Option Strict On

Imports Xeora.Web.Global
Imports Xeora.Web.Shared

Namespace Xeora.Web.Controller.Directive
    Public Class MessageBlock
        Inherits DirectiveControllerBase
        Implements IParsingRequires

        Public Event ParseRequested(ByVal DraftValue As String, ByRef ContainerController As ControllerBase) Implements IParsingRequires.ParseRequested

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal ContentArguments As ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, DirectiveTypes.MessageBlock, ContentArguments)
        End Sub

        Public Overrides Sub Render(ByRef SenderController As ControllerBase)
            If Me.IsUpdateBlockRequest AndAlso Not Me.InRequestedUpdateBlock Then
                Me.DefineRenderedValue(String.Empty)

                Exit Sub
            End If

            Dim matchMI As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(Me.InsideValue, "MB~\d+\:\{")

            If matchMI.Success Then
                If String.Compare(matchMI.Value.Split("~"c)(0), "MB") = 0 Then
                    If Me.MessageResult Is Nothing Then
                        Me.DefineRenderedValue(String.Empty)
                    Else
                        Dim controlValueSplitted As String() =
                            Me.InsideValue.Split(":"c)
                        Dim BlockContent As String =
                            String.Join(":", controlValueSplitted, 1, controlValueSplitted.Length - 2)

                        If Not BlockContent Is Nothing AndAlso
                            BlockContent.Trim().Length >= 2 Then

                            BlockContent = BlockContent.Substring(1, BlockContent.Length - 2)

                            If Not BlockContent Is Nothing AndAlso
                                BlockContent.Trim().Length > 0 Then

                                Me.ContentArguments.AppendKeyWithValue("MessageType", Me.MessageResult.Type)
                                Me.ContentArguments.AppendKeyWithValue("Message", Me.MessageResult.Message)

                                RaiseEvent ParseRequested(BlockContent, Me)

                                Me.DefineRenderedValue(Me.Create())
                            End If
                        End If
                    End If
                Else ' Standart Value
                    If String.Compare(matchMI.Value.Split("~"c)(0), "MB", True) = 0 Then
                        Throw New Exception.DirectivePointerException()
                    Else
                        Me.DefineRenderedValue(Me.InsideValue)
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
