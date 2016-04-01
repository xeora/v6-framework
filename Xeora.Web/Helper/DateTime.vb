Option Strict On

Namespace Xeora.Web.Helper
    Public Class [Date]
        Public Enum DateFormats
            OnlyDate
            DateWithTime
        End Enum

        Public Overloads Shared Function Format(Optional ByVal DTF As DateFormats = DateFormats.DateWithTime) As Long
            Return [Date].Format(Date.Now, DTF)
        End Function

        Public Overloads Shared Function Format(ByVal Value As Date, Optional ByVal DTF As DateFormats = DateFormats.DateWithTime) As Long
            Dim tDate As Date = Value
            Dim tDay As String, tMonth As String, tYear As String, tHour As String, tMinute As String, tSecond As String, rString As String

            tDay = tDate.Day.ToString()
            If CType(tDay, Integer) < 10 Then tDay = "0" & tDay

            tMonth = tDate.Month.ToString()
            If CType(tMonth, Integer) < 10 Then tMonth = "0" & tMonth

            tYear = tDate.Year.ToString()

            tHour = tDate.Hour.ToString()
            If CType(tHour, Integer) < 10 Then tHour = "0" & tHour

            tMinute = tDate.Minute.ToString()
            If CType(tMinute, Integer) < 10 Then tMinute = "0" & tMinute

            tSecond = tDate.Second.ToString()
            If CType(tSecond, Integer) < 10 Then tSecond = "0" & tSecond

            rString = tYear & tMonth & tDay

            Select Case DTF
                Case DateFormats.DateWithTime
                    rString &= tHour & tMinute & tSecond
            End Select

            Return CType(rString, Long)
        End Function

        Public Overloads Shared Function Format(ByVal Value As Long) As Date
            Dim dtString As String = CType(Value, String)

            If (dtString.Length >= 5 AndAlso dtString.Length <= 8) OrElse
                    (dtString.Length >= 11 AndAlso dtString.Length <= 14) Then

                If dtString.Length >= 5 AndAlso dtString.Length <= 8 Then
                    dtString = dtString.PadLeft(8, "0"c)
                ElseIf dtString.Length >= 11 AndAlso dtString.Length <= 14 Then
                    dtString = dtString.PadLeft(14, "0"c)
                End If
            Else
                Throw New ArgumentOutOfRangeException("Long value must have 8 or between 14 steps according to its type!")
            End If

            Dim rDate As Date

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

            Return rDate
        End Function
    End Class
End Namespace