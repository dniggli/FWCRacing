
Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack


Module GetIndyInfo


    Sub getindyinfo()

        Dim years As Integer = Now.Year

        Dim comm2 As New MySqlCommand("Truncate fantasy_race.indyresults", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm2.Connection.Open()
        comm2.ExecuteNonQuery()
        comm2.Connection.Close()


        Dim dast As New MySqlDataAdapter("SELECT * FROM fantasy_race.indyracenumber order by racenumber desc limit 1 ", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        dast.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Dim race As String = dr("racenumber").ToString

        Dim getf1 = New HtmlAgilityPack.HtmlDocument()


        Dim post = New PostSubmitter()
        post.Url = "http://www.race-database.com/results/results.php?year=" & years & "&race=" & race & "&series_id=3"
        'post.Url = "http://indycar.com/schedule/raceresults/55-izod-indycar-series/108/" & race & "*/"
        ' post.Url = "http://www.race-database.com/results/results.php?year=2012&race=1&series_id=3"

        post.Type = PostSubmitter.PostTypeEnum.Get
        Try
            Dim result As String = post.Post()
            'Console.WriteLine(result)
            getf1.LoadHtml(result)
            Dim document = getf1.DocumentNode()


            Dim circut As String = ""
            Dim racelist As New List(Of String)
            Dim races = document.QuerySelectorAll("a")

            For Each r As HtmlNode In races
                'Console.WriteLine(r.InnerHtml)
                racelist.Add(r.InnerHtml)
                'Dim newrace = r.QuerySelectorAll("a")
                'For Each ra As HtmlNode In newrace
                'r.InnerHtml = r.InnerHtml.Replace("S&atilde;o", "Sao")
                'Dim therace As Match = Regex.Match(r.InnerHtml, "([\w .]+)")
                'Dim thisrace As String = therace.Groups(0).Value.ToString
                'thisrace = thisrace.Replace(" results", "")
                'thisrace = thisrace.Replace("2010 ", "")
                'Console.WriteLine(therace.Groups(0).Value)
                'Console.WriteLine(thisrace)
                'circut = thisrace
                ''racelist.Add(therace.Groups(0).Value)
                ''Next

            Next
            Dim table = document.QuerySelectorAll("table")

            'Console.WriteLine(table)
            Dim rows As IEnumerable(Of HtmlNode)

            Dim list0 As Integer = 16
            Dim list1 As Integer = 17
            Dim list2 As Integer = 18
            Dim list3 As Integer = 19
            Dim list4 As Integer = 20
            Dim list5 As Integer = 21
            Dim list6 As Integer = 22
            Dim list7 As Integer = 23
            Dim list8 As Integer = 24
            Dim list9 As Integer = 25
            Dim list13 As Integer = 29
            Dim list14 As Integer = 30
            Dim list10 As Integer = 26
            Dim list12 As Integer = 28



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
                        'Console.WriteLine(c.QuerySelector("a").InnerText)
                    Else
                        'list add
                        list.Add(c.InnerText)
                        'Console.WriteLine(c.InnerText)
                    End If
                    'For Each cd As HtmlNode In cells
                    '   Console.WriteLine(cd.InnerText)
                    'Next
                Next
                'insert statement
                'read indexes from list to build row and do insert

            Next
            Try
                Do Until list10 = 1000

                    'Dim lpper As String = list(list8).ToString
                    'Dim laplead As Match = Regex.Match(lpper, "([\w]+) ([0-9]+)")

                    'Dim pl As String = list(list5).ToString
                    'Dim poll As Match = Regex.Match(pl, "([\w]+) ([0-9]+)")

                    'Dim sts As String = list(list9).ToString
                    'Dim status As Match = Regex.Match(sts, "([\w]+) ([\w]+)")

                    'Console.WriteLine(status.Groups(2).Value)
                    'Console.WriteLine(status.Groups(1).Value)

                    If list(list2) = "H&eacute;lio Castroneves" Then list(list2) = "Helio Castroneves"
                    If list(list2) = "S&eacute;bastien Buemi" Then list(list2) = "Sebastien Buemi"
                    If list(list2) = "S&eacute;bastien Bourdais" Then list(list2) = "Sebastien Bourdais"
                    If list(list2) = "Oriol Servi&agrave;" Then list(list2) = "Oriol Servia"


                    Dim comm As New MySqlCommand("insert into fantasy_race.indyresults(position,driver,laps,pollgrid,circut,running) Values ('" & list(list0) & "','" & list(list2) & "','" & list(list12) & "','" & list(list1) & "','" & racelist(23) & "','" & list(list13) & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                    comm.Connection.Open()
                    comm.ExecuteNonQuery()
                    comm.Connection.Close()
                    list0 = list0 + 16
                    list1 = list1 + 16
                    list2 = list2 + 16
                    list3 = list3 + 16
                    list4 = list4 + 16
                    list5 = list5 + 16
                    list6 = list6 + 16
                    list7 = list7 + 16
                    list8 = list8 + 16
                    list9 = list9 + 16
                    list10 = list10 + 16
                    list12 = list12 + 16
                    list13 = list13 + 16
                    list14 = list14 + 16


                Loop


            Catch
            End Try



        Catch
            'Dim msg As String
            'Dim title As String
            'Dim style As MsgBoxStyle
            'Dim response As MsgBoxResult

            'msg = "Latest indy race has not been posted"   ' Define message.
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
