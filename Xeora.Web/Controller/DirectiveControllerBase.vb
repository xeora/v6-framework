Option Strict On

Namespace Xeora.Web.Controller
    Public MustInherit Class DirectiveControllerBase
        Inherits ControllerBase

        Private _DirectiveType As DirectiveTypes

        Public Enum DirectiveTypes
            Control
            Template
            Translation
            HashCodePointedTemplate
            Execution
            EncodedExecution
            InLineStatement
            UpdateBlock
            MessageBlock
            PartialCache
            Undefined
        End Enum

        Public Sub New(ByVal DraftStartIndex As Integer, ByVal DraftValue As String, ByVal DirectiveType As DirectiveTypes, ByVal ContentArguments As [Global].ArgumentInfoCollection)
            MyBase.New(DraftStartIndex, DraftValue, ControllerTypes.Directive, ContentArguments)

            Me._DirectiveType = DirectiveType
        End Sub

        Protected Function CaptureLeveling(ByRef LevelingExecutionOnly As Boolean) As Integer
            Dim rCapturedLeveling As Integer = 0

            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)

            Dim CLevelingMatch As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(controlValueSplitted(0), "\#\d+(\+)?")

            If CLevelingMatch.Success Then
                ' Trim # character from match result
                Dim CleanValue As String =
                    CLevelingMatch.Value.Substring(1, CLevelingMatch.Value.Length - 1)

                If CleanValue.IndexOf("+"c) > -1 Then
                    LevelingExecutionOnly = False

                    CleanValue = CleanValue.Substring(0, CleanValue.IndexOf("+"c))
                Else
                    LevelingExecutionOnly = True
                End If

                Integer.TryParse(CleanValue, rCapturedLeveling)
            End If

            Return rCapturedLeveling
        End Function

        Protected Function CaptureBoundControlID() As String
            Dim rCapturedParentID As String = String.Empty

            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)

            Dim CPIDMatch As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(controlValueSplitted(0), "\[[\.\w\-]+\]")

            If CPIDMatch.Success Then
                ' Trim [ and ] character from match result
                rCapturedParentID = CPIDMatch.Value.Substring(1, CPIDMatch.Value.Length - 2)
            End If

            Return rCapturedParentID
        End Function

        Protected Function CaptureControlID() As String
            Dim rCapturedID As String = String.Empty

            Dim controlValueSplitted As String() =
                Me.InsideValue.Split(":"c)

            Dim CPIDMatch As Text.RegularExpressions.Match =
                Text.RegularExpressions.Regex.Match(controlValueSplitted(1), "[\/\.\w\-]+")

            If CPIDMatch.Success Then
                ' Trim [ and ] character from match result
                rCapturedID = CPIDMatch.Value
            End If

            Return rCapturedID
        End Function

        Public Shared Function CaptureDirectiveType(ByVal DraftValue As String) As DirectiveTypes
            Dim rDirectiveType As DirectiveTypes = DirectiveTypes.Undefined

            If Not String.IsNullOrEmpty(DraftValue) Then
                Dim CPIDMatch As Text.RegularExpressions.Match =
                    Text.RegularExpressions.Regex.Match(DraftValue, "\$(((?<DirectiveType>\w)(\#\d+(\+)?)?(\[[\.\w\-]+\])?)|(?<DirectiveType>\w+))\:")

                If CPIDMatch.Success Then
                    Select Case CPIDMatch.Result("${DirectiveType}")
                        Case "C"
                            rDirectiveType = DirectiveTypes.Control
                        Case "T"
                            rDirectiveType = DirectiveTypes.Template
                        Case "L"
                            rDirectiveType = DirectiveTypes.Translation
                        Case "P"
                            rDirectiveType = DirectiveTypes.HashCodePointedTemplate
                        Case "F"
                            rDirectiveType = DirectiveTypes.Execution
                        Case "S"
                            rDirectiveType = DirectiveTypes.InLineStatement
                        Case "H"
                            rDirectiveType = DirectiveTypes.UpdateBlock
                        Case "XF"
                            rDirectiveType = DirectiveTypes.EncodedExecution
                        Case "MB"
                            rDirectiveType = DirectiveTypes.MessageBlock
                        Case "PC"
                            rDirectiveType = DirectiveTypes.PartialCache
                    End Select
                End If
            End If

            Return rDirectiveType
        End Function
    End Class
End Namespace