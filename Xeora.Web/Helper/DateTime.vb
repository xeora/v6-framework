Option Strict On

Namespace Xeora.Web.Helper
    Public Class DateTime
        Public Overloads Shared Function Format(Optional ByVal FormatJustDate As Boolean = False) As Long
            Return DateTime.Format(System.DateTime.Now, FormatJustDate)
        End Function

        Public Overloads Shared Function Format(ByVal vDateTime As System.DateTime, Optional ByVal FormatJustDate As Boolean = False) As Long
            Dim tString As String =
                String.Format("{0:0000}{1:00}{2:00}", vDateTime.Year, vDateTime.Month, vDateTime.Day)

            If Not FormatJustDate Then _
                tString = String.Format("{0}{1:00}{2:00}{3:00}", tString, vDateTime.Hour, vDateTime.Minute, vDateTime.Second)

            Return Long.Parse(tString)
        End Function

        Public Overloads Shared Function Format(ByVal vDateTime As Long) As System.DateTime
            Dim dtString As String = CType(vDateTime, String)

            If (dtString.Length >= 5 AndAlso dtString.Length <= 8) OrElse
                (dtString.Length >= 11 AndAlso dtString.Length <= 14) Then

                If dtString.Length >= 5 AndAlso dtString.Length <= 8 Then
                    dtString = dtString.PadLeft(8, "0"c)
                ElseIf dtString.Length >= 11 AndAlso dtString.Length <= 14 Then
                    dtString = dtString.PadLeft(14, "0"c)
                End If
            Else
                If vDateTime > 0 Then _
                    Throw New System.Exception("Long value must have 8 or between 14 steps according to its type!")
            End If

            Dim rDate As Date = Date.MinValue

            If vDateTime > 0 Then
                If dtString.Length = 14 Then
                    rDate = New Date(
                                Integer.Parse(dtString.Substring(0, 4)),
                                Integer.Parse(dtString.Substring(4, 2)),
                                Integer.Parse(dtString.Substring(6, 2)),
                                Integer.Parse(dtString.Substring(8, 2)),
                                Integer.Parse(dtString.Substring(10, 2)),
                                Integer.Parse(dtString.Substring(12, 2))
                            )
                Else
                    rDate = New Date(
                               Integer.Parse(dtString.Substring(0, 4)),
                               Integer.Parse(dtString.Substring(4, 2)),
                               Integer.Parse(dtString.Substring(6, 2))
                           )
                End If
            End If

            Return rDate
        End Function
    End Class
End Namespace