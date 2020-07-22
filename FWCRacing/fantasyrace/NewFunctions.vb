Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient

Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.pdf
Imports System.Net

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







    Public Sub GetNewNascar(ByVal url As String, ByVal mw As String)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls Or SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11
        ' Try
        Dim getf1 = New HtmlAgilityPack.HtmlDocument()
        Dim post = New PostSubmitter()
        post.Url = url
        post.Type = PostSubmitter.PostTypeEnum.Get
        Dim result As String = post.Post()
        'Console.WriteLine(result)
        getf1.LoadHtml(result)
        Dim document = getf1.DocumentNode()




        Dim circut As String = Form1.TextBoxnascarcircut.Text
        Dim racelist As New List(Of String)
        Dim races = document.QuerySelectorAll("a")
        Console.WriteLine(document.InnerText)

        Dim rows As IEnumerable(Of HtmlNode)

        Dim list As New List(Of String)
        '  foreach(HtmlNode row In doc.DocumentNode.SelectNodes("//table[@id='table2']//tr"))
        'foreach(HtmlNode col In row.SelectNodes("td"))
        Dim cnt As Integer = 0

        For Each t As HtmlNode In document.SelectNodes("//div[contains(@class, 'race-center-leaderboard-table-row')]")
            If cnt > 0 Then
                rows = t.QuerySelectorAll("tr")




                For Each td As HtmlNode In t.QuerySelectorAll("div")
                    Console.WriteLine(td.InnerText.Trim)

                    If td.QuerySelector("p") IsNot Nothing Then
                        'list add
                        list.Add(td.QuerySelector("p").InnerText)
                        Console.WriteLine(td.QuerySelector("p").InnerText.Trim)
                    ElseIf td.QuerySelector("h2") IsNot Nothing Then
                        'list add
                        list.Add(td.QuerySelector("h2").InnerText)
                        Console.WriteLine(td.QuerySelector("h2").InnerText.Trim)
                        'list add
                        list.Add(td.InnerText)
                        Console.WriteLine(td.InnerText.Trim)
                    End If

                Next
                If list.Count = 9 Then
                    Dim comm As New MySqlCommand("insert into fantasy_race.nascarresults(position,driver,laps,pollgrid,circut,running,stagewin) Values ('" & list(0) & "','" & list(1).Trim & "','" & list(6).Trim & "','" & list(3).Trim & "','" & circut & "','" & list(4).Trim & "','" & mw & "')", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

                    comm.Connection.Open()
                    comm.ExecuteNonQuery()
                    comm.Connection.Close()
                    list.Clear()
                End If
            End If
            cnt = cnt + 1
        Next



        ' Catch ex As Exception
        'Dim msg As String
        '    Dim title As String
        '    Dim style As MsgBoxStyle
        '    Dim response As MsgBoxResult

        '    msg = ex.ToString   ' Define message.
        '    style = MsgBoxStyle.OkOnly
        '    title = "MsgBox"   ' Define title.
        '    'Display message.
        '    response = MsgBox(msg, style, title)
        '    If response = MsgBoxResult.Ok Then
        '        Exit Sub
        '    End If

        ' End Try

    End Sub




End Module
