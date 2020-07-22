Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.pdf
Imports System.Net



Module GetNascar


    ' Sub getnewnascar()

    '    Dim comm2 As New MySqlCommand("Truncate fantasy_race.nascarresults", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))
    '    comm2.Connection.Open()
    '    comm2.ExecuteNonQuery()
    '    comm2.Connection.Close()

    '    If Form1.TextBoxnascarcircut.Text.Length < 1 Then
    '        Dim msg As String
    '        Dim title As String
    '        Dim style As MsgBoxStyle
    '        Dim response As MsgBoxResult

    '        msg = "no Circut given"   ' Define message.
    '        style = MsgBoxStyle.OkOnly
    '        title = "MsgBox"   ' Define title.
    '        'Display message.
    '        response = MsgBox(msg, style, title)
    '        If response = MsgBoxResult.Ok Then
    '            Exit Sub
    '        End If

    '    End If
    '    Dim cnt As Integer = 0
    '    Dim lcnt As Integer = 0


    '    Dim position As String = String.Empty
    '    Dim driver As String = String.Empty
    '    Dim laps As String = 0
    '    Dim pollgrid As String = String.Empty
    '    Dim circut As String = Form1.TextBoxnascarcircut.Text
    '    Dim running As String = String.Empty
    '    Dim Stage1 As String = 0


    '    Dim stg1 As String = String.Empty
    '    Dim stg2 As String = String.Empty
    '    Dim stg3 As String = String.Empty
    '    Dim stg4 As String = String.Empty

    '    Dim count As Integer = 0
    '    Dim rdd As PdfReader
    '    Dim myDelims As String() = New String() {"Running"}


    '    Dim urls As String = Form1.TextBoxUrl.Text
    '    Dim url As String() = urls.Split(";")
    '    rdd = New PdfReader(url(0))
    '    Dim text As String = PdfTextExtractor.GetTextFromPage(rdd, 1)

    '    Dim DataLines() As String = Split(text, vbCr)
    '    Dim splt() As String = text.Split({Environment.NewLine, vbCrLf, vbLf}, StringSplitOptions.None)

    '    For Each s As String In splt



    '        If s.Contains("Running") Then

    '            running = "running"
    '        Else
    '            running = "no"

    '        End If


    '        If (s.Contains("Running") Or s.Contains("Accident") Or s.Contains("Vibration") Or s.Contains("Engine") Or s.Contains("Rear End")) = True Then
    '            Try


    '                For Each s1 As String In s.Split({"Running", "Accident", "Vibration"}, StringSplitOptions.None)
    '                    '*************************grab first part of split****************
    '                    If s1.Length > 0 Then
    '                        If s1.Length > 10 Then
    '                            cnt = 0
    '                            For Each sx As String In s1.Split(" ")
    '                                lcnt = lcnt + 1
    '                            Next
    '                            Dim star As Integer = 0
    '                            For Each s2 As String In s1.Split(" ")
    '                                If s2 = "*" Then

    '                                Else

    '                                    If cnt = 0 Then
    '                                        position = s2

    '                                    End If
    '                                    If cnt = 1 Then
    '                                        pollgrid = s2
    '                                    End If

    '                                    If cnt = 3 Then

    '                                        driver = s2
    '                                    End If

    '                                    If cnt = 4 Then
    '                                        If s2.EndsWith(")") Then
    '                                            s2 = s2.TrimEnd(")", "(", "i")

    '                                        End If
    '                                        driver = driver + " " + s2

    '                                    End If

    '                                    If cnt = 5 Then
    '                                        If s2 Like "Jr" Then
    '                                            driver = driver + ", " + s2 + "."
    '                                            Console.WriteLine(position, pollgrid, driver)
    '                                        ElseIf s2 Like "Jr." Then

    '                                            driver = driver + ", " + s2

    '                                        End If
    '                                    End If

    '                                    cnt = cnt + 1
    '                                End If

    '                            Next
    '                            Dim cnt3 As Integer = 0
    '                            For Each s3 As String In s1.Split(" ")
    '                                If cnt3 = lcnt - 5 Then
    '                                    stg1 = s3
    '                                End If
    '                                If cnt3 = lcnt - 4 Then
    '                                    stg2 = s3
    '                                End If
    '                                If cnt3 = lcnt - 3 Then
    '                                    stg3 = s3
    '                                End If
    '                                If cnt3 = lcnt - 2 Then
    '                                    stg4 = s3
    '                                End If
    '                                cnt3 = cnt3 + 1

    '                            Next
    '                            Console.WriteLine(stg1 + " " + stg2 + " " + stg3 + " " + stg4)
    '                            If IsNumeric(stg1) Then
    '                                If IsNumeric(stg2) Then
    '                                    If IsNumeric(stg3) Then
    '                                        If IsNumeric(stg4) Then
    '                                            Stage1 = 2


    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                            If Not IsNumeric(stg1) Then
    '                                If IsNumeric(stg2) Then
    '                                    If IsNumeric(stg3) Then
    '                                        If IsNumeric(stg4) Then

    '                                            Stage1 = 1

    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                            lcnt = 0

    '                        Else
    '                            Dim cnt2 As Integer = 0
    '                            For Each s2 As String In s1.Split(" ")
    '                                If cnt2 = 2 Then
    '                                    laps = s2

    '                                End If
    '                                ' Console.WriteLine(s2 + " " + count.ToString)
    '                                cnt2 = cnt2 + 1


    '                            Next

    '                        End If


    '                    End If

    '                Next
    '                Console.WriteLine(position + " " + pollgrid + " " + driver + " " + running + " " + circut + " " + laps + " " + Stage1)

    '                Dim comm As New MySqlCommand("insert into fantasy_race.nascarresults(position,driver,laps,pollgrid,circut,running,stagewin) Values ('" & position & "','" & driver & "','" & laps & "','" & pollgrid & "','" & circut & "','" & running & "','" & Stage1 & "')", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))
    '                comm.Connection.Open()
    '                comm.ExecuteNonQuery()
    '                comm.Connection.Close()

    '                Stage1 = 0
    '                laps = 0

    '            Catch ex As Exception
    '                Dim msg As String
    '                Dim title As String
    '                Dim style As MsgBoxStyle
    '                Dim response As MsgBoxResult

    '                msg = "Issue with Writing to table NascarResults"   ' Define message.
    '                style = MsgBoxStyle.OkOnly
    '                title = "MsgBox"   ' Define title.
    '                'Display message.
    '                response = MsgBox(msg, style, title)
    '                If response = MsgBoxResult.Ok Then

    '                End If
    '            End Try

    '        End If









    '        '   Console.WriteLine(s)
    '    Next
    '    GetNascarx.getnewnascar(url(1))
    'End Sub


    Sub getnascarinfo()


        Dim years As Integer = Now.Year
        Dim testyears As Integer = 2015


        Dim comm2 As New MySqlCommand("Truncate fantasy_race.nascarresults", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))
        comm2.Connection.Open()
        comm2.ExecuteNonQuery()
        comm2.Connection.Close()


        'Dim dast As New MySqlDataAdapter("SELECT * FROM fantasy_race.nascarracenumber order by racenumber desc limit 1 ", "server=" & Form1.server & ";uid=dave;pwd=vvo084;")
        'Dim dt As New DataTable
        'dast.Fill(dt)

        'Dim dr As DataRow = dt.Rows(0)

        'Dim race As String = dr("racenumber").ToString






        '\\\\\\ run for testing setup////////
        Dim urls As String = Form1.TextBoxUrl.Text
        Dim url As String() = urls.Split(";")

        NewFunctions.GetNewNascar(url(0), "M")





        '\\\\\\\run for Production//////////


        'post.Url = "http://www.race-database.com/results/results.php?year=" & years & "&race=" & race & "&series_id=2"
        'post.Url = "http://indycar.com/schedule/raceresults/55-izod-indycar-series/108/" & race & "*/"
        ' post.Url = "http://www.race-database.com/results/results.php?year=2012&race=1&series_id=2"




        If url.Count > 1 Then
            GetNascarx.getnascarinfo(url(1))
        End If

    End Sub
End Module
Class MyWebClient
    Inherits Net.WebClient

    Protected Overrides Function GetWebRequest(ByVal address As Uri) As Net.WebRequest
        Dim request As Net.HttpWebRequest = TryCast(MyBase.GetWebRequest(address), Net.HttpWebRequest)
        request.AutomaticDecompression = Net.DecompressionMethods.Deflate Or Net.DecompressionMethods.GZip
        Return request
    End Function
End Class