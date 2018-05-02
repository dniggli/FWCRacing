Imports System.Text.RegularExpressions

Module NewFunctions




    Public Function RaceNumbers() As String

        Dim racenumber As String = Form1.TextBoxRaceListNum.Text
        Return racenumber
    End Function

    Public Function standings1thru5(ByVal row As String) As String

        Dim r As Regex = New Regex("^\d+$")
        Dim returnrow As String = ""

        If r.IsMatch(row) Then

            returnrow &= "<td align= 'center'>" & row & "</td>"
        Else

            If row.Length > 4 Then

                returnrow &= "<td><font size='4'>" & row & "</font></td>"
            Else
                returnrow &= "<td align= 'center'><font size='4' color ='red'>" & row & "</font></td>"

            End If
        End If
        Return returnrow
    End Function

    Public Function standings5truEnd(ByVal row As String) As String

        Dim r As Regex = New Regex("^\d+$")
        Dim returnrow As String = ""

        If r.IsMatch(row) Then

            returnrow &= "<td align= 'center' bgcolor = '#C0C0C0'>" & row & "</td>"
        Else

            If returnrow.Length > 4 Then

                returnrow &= "<td bgcolor = '#C0C0C0'><font size='4'>" & row & "</font></td>"
            Else
                returnrow &= "<td align= 'center' bgcolor = '#C0C0C0'><font size='4' color ='red'>" & row & "</font></td>"

            End If
        End If
        Return returnrow
    End Function

End Module
