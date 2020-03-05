Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.pdf



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

        Dim getf1 = New HtmlAgilityPack.HtmlDocument()


        Dim post = New PostSubmitter()

        '\\\\\\ run for testing setup////////
        Dim urls As String = Form1.TextBoxUrl.Text
        Dim url As String() = urls.Split(";")
        post.Url = url(0)





        '\\\\\\\run for Production//////////


        'post.Url = "http://www.race-database.com/results/results.php?year=" & years & "&race=" & race & "&series_id=2"
        'post.Url = "http://indycar.com/schedule/raceresults/55-izod-indycar-series/108/" & race & "*/"
        ' post.Url = "http://www.race-database.com/results/results.php?year=2012&race=1&series_id=2"

        post.Type = PostSubmitter.PostTypeEnum.Get
        Try
            Dim result As String = post.Post()
            'Console.WriteLine(result)
            getf1.LoadHtml(result)
            Dim document = getf1.DocumentNode()


            Dim circut As String = Form1.TextBoxnascarcircut.Text
            Dim racelist As New List(Of String)
            Dim races = document.QuerySelectorAll("a")

            For Each r As HtmlNode In races
                '  Console.WriteLine(r.InnerHtml)
                racelist.Add(r.InnerHtml)
                'Dim newrace = r.QuerySelectorAll("a")
                'For Each ra As HtmlNode In newrace
                'r.InnerHtml = r.InnerHtml.Replace("S&atilde;o", "Sao")
                'Dim therace As Match = Regex.Match(r.InnerHtml, "([\w .']+)")
                'Dim thisrace As String = therace.Groups(0).Value.ToString
                'thisrace = thisrace.Replace(" results", "")
                'thisrace = thisrace.Replace("2010 ", "")
                ' Console.WriteLine(r.InnerHtml)
                'Console.WriteLine(thisrace)
                'circut = thisrace
                'racelist.Add(therace.Groups(0).Value)
                'Next

            Next
            Dim table = document.QuerySelectorAll("table")

            ' Console.WriteLine(table)
            Dim rows As IEnumerable(Of HtmlNode)

            Dim n As Integer = 1
            Dim list0 As Integer = 1 - n
            Dim list1 As Integer = 2 - n
            Dim list2 As Integer = 3 - n
            Dim list3 As Integer = 4 - n
            Dim list4 As Integer = 5 - n
            Dim list5 As Integer = 6 - n
            Dim list6 As Integer = 7 - n
            Dim list7 As Integer = 8 - n
            Dim list8 As Integer = 9 - n
            Dim list9 As Integer = 10 - n
            Dim list10 As Integer = 11 - n
            Dim list11 As Integer = 12 - n
            Dim list12 As Integer = 13 - n
            Dim list13 As Integer = 14 - n
            Dim list14 As Integer = 15 - n





            Dim list As New List(Of String)
            For Each t As HtmlNode In table
                rows = t.QuerySelectorAll("td")
                'Console.WriteLine(t.InnerHtml)
                'For Each r As HtmlNode In rows
                Dim cells = t.QuerySelectorAll("td")

                For x As Integer = 0 To cells.Count - 1
                    Dim c = cells(x)
                    If c.QuerySelector("a") IsNot Nothing Then
                        'list add
                        list.Add(c.QuerySelector("a").InnerText)
                        ' Console.WriteLine(c.QuerySelector("a").InnerText)
                    Else
                        'list add
                        list.Add(c.InnerText)
                        'Console.WriteLine(c.InnerText)
                    End If
                    'For Each cd As HtmlNode In cells
                    'Console.WriteLine(cd.InnerText)
                    'Next
                Next
                'insert statement
                'read indexes from list to build row and do insert

            Next
            Try
                Do Until list14 > list.Count

                    'Dim lpper As String = list(list8).ToString
                    'Dim laplead As Match = Regex.Match(lpper, "([\w]+) ([0-9]+)")

                    'Dim pl As String = list(list5).ToString
                    'Dim poll As Match = Regex.Match(pl, "([\w]+) ([0-9]+)")

                    'Dim sts As String = list(list9).ToString
                    'Dim status As Match = Regex.Match(sts, "([\w]+) ([\w]+)")

                    'Console.WriteLine(status.Groups(2).Value)
                    'Console.WriteLine(status.Groups(1).Value)
                    ' Console.WriteLine(list(list2))

                    If list(list2) = "H&eacute;lio Castroneves" Then list(list2) = "Helio Castroneves"


                    Dim comm As New MySqlCommand("insert into fantasy_race.nascarresults(position,driver,laps,pollgrid,circut,running,stagewin) Values ('" & list(list0) & "','" & list(list1).Trim & "','" & list(list5) & "','" & list(list3) & "','" & circut & "','" & list(list6) & "','M')", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

                    comm.Connection.Open()
                    comm.ExecuteNonQuery()
                    comm.Connection.Close()

                    Dim n2 As Integer = 9
                    list0 = list0 + n2
                    list1 = list1 + n2
                    list2 = list2 + n2
                    list3 = list3 + n2
                    list4 = list4 + n2
                    list5 = list5 + n2
                    list6 = list6 + n2
                    list7 = list7 + n2
                    list8 = list8 + n2
                    list9 = list9 + n2
                    list10 = list10 + n2
                    list11 = list11 + n2
                    list12 = list12 + n2
                    list13 = list13 + n2
                    list14 = list14 + n2


                Loop

                'race = race + 1
                'Dim comm1 As New MySqlCommand("insert into fantasy_race.nascarracenumber(racenumber) Values ('" & race & "')", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

                'comm1.Connection.Open()
                'comm1.ExecuteNonQuery()
                'comm1.Connection.Close()
            Catch e As Exception
                Dim msg As String
                Dim title As String
                Dim style As MsgBoxStyle
                Dim response As MsgBoxResult

                msg = e.ToString   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End Try

            'Dim msg As String
            'Dim title As String
            'Dim style As MsgBoxStyle
            'Dim response As MsgBoxResult

            'msg = "Latest Nascar race has not been posted"   ' Define message.
            'style = MsgBoxStyle.OkOnly
            'title = "MsgBox"   ' Define title.
            ''Display message.
            'response = MsgBox(msg, style, title)
            'If response = MsgBoxResult.Ok Then
            '    Exit Sub
            'End If


        Catch ex As Exception
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = ex.ToString   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If

        End Try

        If url.Count > 1 Then
            GetNascarx.getnascarinfo(url(1))
        End If

    End Sub
End Module
