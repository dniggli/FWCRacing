
Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.pdf


Module GetF1Info

    Dim circ As String = Form1.TextBoxCircut.Text
    Dim testyears As Integer = 2015

    Sub getnewF1()

        '********* Uses site "https://www.foxsports.com/motor/results?circuit=1&season=2018&raceCode=18" for data scarping.************ 

        Dim comm2 As New MySqlCommand("Truncate fantasy_race.f1results", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm2.Connection.Open()
        comm2.ExecuteNonQuery()
        comm2.Connection.Close()

        If Form1.TextBoxCircut.Text.Length < 1 Then
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "no Circut given"   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If

        End If
        Dim cnt As Integer = 0

        Dim position As String = String.Empty
        Dim driver As String = String.Empty
        Dim laps As String = 0
        Dim pollgrid As String = String.Empty
        Dim circut As String = Form1.TextBoxnascarcircut.Text
        Dim running As String = String.Empty


        Dim count As Integer = 0
        Dim rdd As PdfReader
        Dim myDelims As String() = New String() {"Running"}

        rdd = New PdfReader(Form1.TextBoxUrl.Text)
        Dim text As String = PdfTextExtractor.GetTextFromPage(rdd, 1)

        Dim DataLines() As String = Split(text, vbCr)
        Dim splt() As String = text.Split({Environment.NewLine, vbCrLf, vbLf}, StringSplitOptions.None)

        For Each s As String In splt



            If s.Contains(":") Then

                running = "running"
            Else
                running = "no"

            End If


            If (s.Contains(":") Or s.Contains("Collisoin") Or s.Contains("Electrical") Or s.Contains("Tyre") Or s.Contains("Fire") Or s.Contains("Transmission")) = True Then
                Try

                    Console.WriteLine(s)
                    For Each s1 As String In s.Split({"Running", "Contact", "Electrical", "Tyre", "Fire", "Transmission"}, StringSplitOptions.None)
                        '*************************grab first part of split****************

                        Console.WriteLine(s1)
                        Console.WriteLine(s1.Length)
                        If s1.Length > 0 Then
                            If s1.Length > 10 Then
                                cnt = 0
                                Dim star As Integer = 0
                                For Each s2 As String In s1.Split(" ")
                                    If s2 = "*" Then

                                    Else

                                        If cnt = 0 Then
                                            position = s2

                                        End If
                                        If cnt = 1 Then
                                            pollgrid = s2
                                        End If

                                        If cnt = 3 Then

                                            driver = s2
                                        End If

                                        If cnt = 4 Then
                                            driver = driver + " " + s2

                                        End If

                                        If cnt = 5 Then
                                            If s2 Like "Jr*" Then
                                                driver = driver + ", " + s2
                                                Console.WriteLine(position, pollgrid, driver)
                                            End If
                                        End If

                                        cnt = cnt + 1
                                    End If

                                Next


                            Else
                                'Dim cnt2 As Integer = 0
                                'For Each s2 As String In s1.Split(" ")
                                '    If cnt2 = 2 Then
                                '        laps = s2

                                '    End If
                                '    ' Console.WriteLine(s2 + " " + count.ToString)
                                '    cnt2 = cnt2 + 1


                                'Next

                            End If


                        End If

                    Next
                    laps = 0
                    If driver Like "Juan*" Then
                        driver = driver + " Montoya"
                    End If
                    'Console.WriteLine(position + " " + pollgrid + " " + driver + " " + running + " " + circut + " " + laps)
                    Dim comm As New MySqlCommand("insert into fantasy_race.indyresults(position,driver,laps,pollgrid,circut,running) Values ('" & position & "','" & driver & "','" & laps & "','" & pollgrid & "','" & circut & "','" & running & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                    comm.Connection.Open()
                    comm.ExecuteNonQuery()
                    comm.Connection.Close()
                    laps = 0

                Catch ex As Exception
                    Dim msg As String
                    Dim title As String
                    Dim style As MsgBoxStyle
                    Dim response As MsgBoxResult

                    msg = "Issue with Writing to table NascarResults"   ' Define message.
                    style = MsgBoxStyle.OkOnly
                    title = "MsgBox"   ' Define title.
                    'Display message.
                    response = MsgBox(msg, style, title)
                    If response = MsgBoxResult.Ok Then

                    End If
                End Try

            End If

            '   Console.WriteLine(s)

        Next



    End Sub

    Sub getf1infodatabasedotcom()



        Dim years As Integer = Now.Year



        Dim comm2 As New MySqlCommand("Truncate fantasy_race.f1results", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm2.Connection.Open()
        comm2.ExecuteNonQuery()
        comm2.Connection.Close()


        Dim dast As New MySqlDataAdapter("SELECT * FROM fantasy_race.f1racenumber order by racenumber desc limit 1 ", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        dast.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)
        Dim dotrace As String = dr("f1dotcom").ToString
        Dim race As String = dr("racenumber").ToString

        Dim getf1 = New HtmlAgilityPack.HtmlDocument()


        Dim post = New PostSubmitter()
        ' post.Url = "http://www.race-database.com/results/results.php?year=" & years & "&race=" & race & "&series_id=1"
        ' post.Url = "http://www.race-database.com/results/results.php?year=" & testyears & "&race=" & race & "&series_id=1"
        post.Url = Form1.TextBoxF1url.Text
        'post.Url = "http://www.motorsport.com/f1/results/?r=2016000" & race & ""
        'post.Url = "http://www.race-database.com/results/results.php?year=2012&race=1&series_id=1"

        post.Type = PostSubmitter.PostTypeEnum.Get
        Try
            Dim result As String = post.Post()
            'Console.WriteLine(result)
            getf1.LoadHtml(result)
            Dim document = getf1.DocumentNode()


            Dim circut As String = ""
            Dim racelist As New List(Of String)
            Dim races = document.QuerySelectorAll("div")

            For Each r As HtmlNode In races
                'Console.WriteLine(r.InnerHtml)
                racelist.Add(r.InnerHtml)
                'Dim newrace = r.QuerySelectorAll("a")
                ''For Each ra As HtmlNode In newrace
                'r.InnerHtml = r.InnerHtml.Replace("S&atilde;o", "Sao")
                'r.InnerHtml = r.InnerHtml.Replace("Pr&ecirc;mio", "Premio")
                'r.InnerHtml = r.InnerHtml.Replace("Espa&ntilde;a", "Espana")
                'r.InnerHtml = r.InnerHtml.Replace("Gro&szlig;er", "Grosser")
                'Dim therace As Match = Regex.Match(r.InnerHtml, "([\w .]+)")
                'Dim thisrace As String = therace.Groups(0).Value.ToString
                'thisrace = thisrace.Replace(" results", "")
                'thisrace = thisrace.Replace("2010 ", "")
                ''Console.WriteLine(therace.Groups(0).Value)
                ''Console.WriteLine(thisrace)
                'circut = thisrace
                'racelist.Add(therace.Groups(0).Value)
                'Next

            Next
            Dim table = document.QuerySelectorAll("table")

            'Console.WriteLine(table)
            Dim rows As IEnumerable(Of HtmlNode)
            Dim n As Integer = 1
            Dim list0 As Integer = -1 + n
            Dim list1 As Integer = 0 + n
            Dim list2 As Integer = 1 + n
            Dim list3 As Integer = 2 + n
            Dim list4 As Integer = 3 + n
            Dim list5 As Integer = 4 + n
            Dim list6 As Integer = 5 + n
            Dim list7 As Integer = 23 + n
            Dim list8 As Integer = 24 + n
            Dim list9 As Integer = 25 + n
            Dim list13 As Integer = 29 + n
            Dim list14 As Integer = 30 + n
            Dim list10 As Integer = 26 + n
            Dim list12 As Integer = 28 + n
            Dim list15 As Integer = 31 + n



            Dim list As New List(Of String)
            For Each t As HtmlNode In table
                rows = t.QuerySelectorAll("td")
                Console.WriteLine(t.InnerHtml)
                'For Each r As HtmlNode In rows
                Dim cells = t.QuerySelectorAll("td")

                For x As Integer = 0 To cells.Count - 1
                    Dim c = cells(x)
                    If c.QuerySelector("a") IsNot Nothing Then
                        'list add
                        list.Add(c.QuerySelector("a").InnerText)
                        Console.WriteLine(c.QuerySelector("a").InnerText)
                    Else
                        'list add
                        list.Add(c.InnerText)
                        '  Console.WriteLine(c.InnerText)
                    End If
                    '  For Each cd As HtmlNode In cells
                    'Console.WriteLine(cd.InnerText)
                    'Next
                Next
                'insert statement
                'read indexes from list to build row and do insert

            Next
            Try
                Do Until list1 = 300

                    'Dim lpper As String = list(list8).ToString
                    'Dim laplead As Match = Regex.Match(lpper, "([\w]+) ([0-9]+)")

                    'Dim pl As String = list(list5).ToString
                    'Dim poll As Match = Regex.Match(pl, "([\w]+) ([0-9]+)")

                    'Dim sts As String = list(list9).ToString
                    'Dim status As Match = Regex.Match(sts, "([\w]+) ([\w]+)")

                    'Console.WriteLine(status.Groups(2).Value)
                    'Console.WriteLine(status.Groups(1).Value)

                    If list(list1) = "Nico H&uuml;lkenberg" Then list(list2) = "Nico Hulkenberg"
                    If list(list1) = "S&eacute;bastien Buemi" Then list(list2) = "Sebastien Buemi"
                    If list(list1) = "J&eacute;r&ocirc;me d'Ambrosio" Then list(list2) = "Jerome dAmbrosio"
                    If list(list1) = "Kimi R&auml;ikk&ouml;nen" Then list(list2) = "Kimi Raikkonen"
                    If list(list1) = "Sergio P&eacute;rez" Then list(list2) = "Sergio Perez"
                    If list(list1) = "Jean-&Eacute;ric Vergne" Then list(list2) = "Jean Eric Vergne"
                    If list(list1) = "Esteban Guti&eacute;rrez" Then list(list2) = "Esteban Gutierrez"



                    ' Console.WriteLine(list(list2).ToString)
                    'list(list14) = list(list14).Replace("+", "")
                    'Dim rx As Regex = New Regex("([0-9]+):([0-9]+):([0-9]+).([0-9]+)")
                    'Dim r As Regex = New Regex("([+0-9]+).([0-9]+)")
                    'Dim rex As Regex = New Regex("([+0-9]+)([\w]+)")

                    'If r.IsMatch(list(list15).ToString) Then list(list15) = "running"
                    'If rx.IsMatch(list(list15).ToString) Then list(list15) = "running"
                    'If rex.IsMatch(list(list15).ToString) Then list(list15) = "running"
                    If list(list6) = "" Then list(list6) = "running"
                    list(list1) = list(list1).Trim
                    Dim comm As New MySqlCommand("insert into fantasy_race.f1results(position,driver,laps,pollgrid,circut,running) Values ('" & list(list0) & "','" & list(list1) & "','" & list(list5) & "','" & list(list3) & "','" & circ & "','" & list(list6) & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

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
                    list12 = list12 + n2
                    list13 = list13 + n2
                    list14 = list14 + n2
                    list15 = list15 + n2


                Loop

                'dotrace = dotrace + 1
                'race = race + 1
                'Dim comm1 As New MySqlCommand("insert into fantasy_race.f1racenumber(f1dotcom,racenumber) Values ('" & dotrace & "','" & race & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                'comm1.Connection.Open()
                'comm1.ExecuteNonQuery()
                'comm1.Connection.Close()

            Catch
               
            End Try



        Catch
            'Dim msg As String
            'Dim title As String
            'Dim style As MsgBoxStyle
            'Dim response As MsgBoxResult

            'msg = "Latest F1 race has not been posted"   ' Define message.
            'style = MsgBoxStyle.OkOnly
            'title = "MsgBox"   ' Define title.
            ''Display message.
            'response = MsgBox(msg, style, title)
            'If response = MsgBoxResult.Ok Then
            '    Exit Sub
            'End If

        End Try

    End Sub






End Module
