﻿Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports System.Threading

Public Class Form1

    Public Shared server As String
    Dim driverupdates As Thread
    Dim updatestats As Thread

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TextBoxServer.Text = "MySqlServer"
        'FormloadData()
        TextBoxServer.Focus()
    End Sub
    Sub FormloadData()

        Try
            server = TextBoxServer.Text
            retrivef1()
            retriveINDY()
            retriveNascarA()
            retriveNascarB()
            Dim dar As New MySqlDataAdapter("SELECT * FROM fantasy_race.week ORDER BY key1 desc limit 1;", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim ts As New DataTable
            dar.Fill(ts)
            Try
                Dim q As DataRow = ts.Rows(0)

                Labelweek.Text = q("lastweek").ToString
                TextBoxweek.Text = q("lastweek").ToString + 1
            Catch
                Labelweek.Text = " This is the first week"

            End Try


        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult
            msg = "Can't Connect to Server.  Please Provide Valid Server."   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then

                Exit Sub
            End If
        End Try

    End Sub

    Sub pointsF1()



        Dim comm1 As New MySqlCommand("insert into fantasy_race.f1racenumber (racenumber)select racenumber+1 from fantasy_race.f1racenumber order by racenumber desc limit 1 ", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm1.Connection.Open()
        comm1.ExecuteNonQuery()
        comm1.Connection.Close()


        Dim week As String = TextBoxweek.Text
        Dim circut As String = TextBoxCircut.Text
        Dim f1 As Integer = "1"

        Dim da As New MySqlDataAdapter("select * from fantasy_race.f1, fantasy_race.teams where f1.TeamNumber = teams.Number", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)


        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("F1Driver").ToString
            Dim key As String = dr("key").ToString
            Dim clas As String = dr("Class").ToString
            Dim team As String = dr("Team").ToString
            Dim teamnumber As String = dr("Number").ToString

            For Each ctl As Control In GroupBox1.Controls
                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = key Then
                        Dim pos As String = ctl.Text
                        Dim dnf As String = ""
                        Dim poinstdnf As Integer = 0
                        Dim pos1 As String = ""
                        Dim posdnf As Integer = 0

                        Dim rs As Match = Regex.Match(pos, "([0-9]+)([A-Z])")

                        If (rs.Groups(2).Value) = "D" Then
                            pos1 = rs.Groups(1).Value
                            pos = rs.Groups(1).Value
                            dnf = rs.Groups(2).Value
                            posdnf = "100"




                            Dim dsdnf As New MySqlDataAdapter("select * from fantasy_race.f1points where f1points.key = '" & posdnf & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                            Dim dtdnf As New DataTable
                            dsdnf.Fill(dtdnf)
                            For Each drs As DataRow In dtdnf.Rows
                                Dim position As String = drs("key").ToString
                                Dim a As String = drs("A").ToString
                                Dim b As String = drs("B").ToString
                                Dim c As String = drs("C").ToString

                                If clas = "A" Then poinstdnf = a
                                If clas = "B" Then poinstdnf = b
                                If clas = "C" Then poinstdnf = c

                            Next

                        End If

                        pos1 = pos
                        Console.WriteLine(pos)
                        If pos = "DSQ" Then
                            pos = "23"
                            pos1 = "DSQ"
                        End If

                        If pos = "DNF" Then pos = "100"
                        If pos = "" Then
                            pos = "23"
                            pos1 = "DNS"
                        End If

                        Dim ds As New MySqlDataAdapter("select * from fantasy_race.f1points where f1points.key = '" & pos & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                        Dim dt As New DataTable
                        ds.Fill(dt)
                        For Each drs As DataRow In dt.Rows
                            Dim position As String = drs("key").ToString
                            Dim a As Integer = drs("A")
                            Dim b As Integer = drs("B")
                            Dim c As Integer = drs("C")
                            Dim points As Integer
                            If clas = "A" Then points = a
                            If clas = "B" Then points = b
                            If clas = "C" Then points = c
                            Dim pp As Integer
                            For Each ctl2 As Control In GroupBox1.Controls
                                If TypeOf ctl2 Is CheckBox Then
                                    Dim db As CheckBox = ctl2
                                    If db.Tag = key Then
                                        If db.CheckState = CheckState.Checked Then
                                            If clas = "A" Then pp = "2"
                                            If clas = "B" Then pp = "3"
                                            If clas = "C" Then pp = "5"
                                        End If
                                        Dim ll As Integer
                                        For Each ctl3 As Control In GroupBox1.Controls
                                            If TypeOf ctl3 Is RadioButton Then
                                                Dim rb As RadioButton = ctl3
                                                If rb.Tag = key Then
                                                    If rb.Checked = True Then
                                                        If clas = "A" Then ll = "2"
                                                        If clas = "B" Then ll = "3"
                                                        If clas = "C" Then ll = "5"
                                                    End If


                                                    Dim neopos As String = pos1 + dnf

                                                    Dim comm As New MySqlCommand("insert into fantasy_race.raceresultsf1indy(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead,F1orIndy,TeamNumber) values('" & week & "', '" & circut & "', '" & team & "', '" & driver & "', '" & neopos & "', '" & points + poinstdnf & "', '" & pp & "', '" & ll & "', '" & f1 & "', '" & teamnumber & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                                                    comm.Connection.Open()
                                                    comm.ExecuteNonQuery()
                                                    comm.Connection.Close()

                                                    pp = "0"
                                                    ll = "0"
                                                End If
                                            End If
                                        Next


                                    End If
                                End If

                            Next

                        Next

                    End If
                End If


            Next

        Next

    End Sub

    Sub pointsindy()



        Dim comm1 As New MySqlCommand("insert into fantasy_race.indyracenumber (racenumber)select racenumber+1 from fantasy_race.indyracenumber order by racenumber desc limit 1; ", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm1.Connection.Open()
        comm1.ExecuteNonQuery()
        comm1.Connection.Close()



        Dim week As String = TextBoxweek.Text
        Dim circut As String = TextBoxindycircut.Text

        Dim da As New MySqlDataAdapter("select * from fantasy_race.indy, fantasy_race.teams where indy.TeamNumber = teams.Number", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)


        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("IndyDriver").ToString
            Dim key As String = dr("key").ToString
            Dim clas As String = dr("Class").ToString
            Dim team As String = dr("Team").ToString
            Dim teamnumber As String = dr("Number").ToString


            For Each ctl As Control In GroupBoxINDY.Controls
                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = key Then
                        Dim pos As String = ctl.Text
                        Dim dnf As String = ""
                        Dim poinstdnf As Integer = 0
                        Dim pos1 As String = ""
                        Dim posdnf As Integer = 0

                        Dim rs As Match = Regex.Match(pos, "([0-9]+)([A-Z])")

                        If (rs.Groups(2).Value) = "D" Then
                            pos1 = rs.Groups(1).Value
                            pos = rs.Groups(1).Value
                            dnf = rs.Groups(2).Value
                            posdnf = "100"




                            Dim dsdnf As New MySqlDataAdapter("select * from fantasy_race.indypoints where indypoints.key = '" & posdnf & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                            Dim dtdnf As New DataTable
                            dsdnf.Fill(dtdnf)
                            For Each drs As DataRow In dtdnf.Rows
                                Dim position As String = drs("key").ToString
                                Dim a As String = drs("A").ToString
                                Dim b As String = drs("B").ToString
                                Dim c As String = drs("C").ToString

                                If clas = "A" Then poinstdnf = a
                                If clas = "B" Then poinstdnf = b
                                If clas = "C" Then poinstdnf = c

                            Next

                        End If

                        pos1 = pos

                        If pos = "DNF" Then pos = "100"
                        If pos = "" Then
                            pos = "23"
                            pos1 = "DNS"
                        End If

                        Dim ds As New MySqlDataAdapter("select * from fantasy_race.indypoints where indypoints.key = '" & pos & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                        Dim dt As New DataTable
                        ds.Fill(dt)
                        For Each drs As DataRow In dt.Rows
                            Dim position As String = drs("key").ToString
                            Dim a As String = drs("A").ToString
                            Dim b As String = drs("B").ToString
                            Dim c As String = drs("C").ToString
                            Dim points As Integer = 0
                            If clas = "A" Then points = a
                            If clas = "B" Then points = b
                            If clas = "C" Then points = c
                            Dim PP As String = ""
                            For Each ctl2 As Control In GroupBoxINDY.Controls
                                If TypeOf ctl2 Is CheckBox Then
                                    Dim db As CheckBox = ctl2
                                    If db.Tag = key Then
                                        If db.CheckState = CheckState.Checked Then
                                            If clas = "A" Then PP = "2"
                                            If clas = "B" Then PP = "3"
                                            If clas = "C" Then PP = "5"
                                        End If
                                        Dim ll As String = ""
                                        For Each ctl3 As Control In GroupBoxINDY.Controls
                                            If TypeOf ctl3 Is RadioButton Then
                                                Dim rb As RadioButton = ctl3
                                                If rb.Tag = key Then
                                                    If rb.Checked = True Then
                                                        If clas = "A" Then ll = "2"
                                                        If clas = "B" Then ll = "3"
                                                        If clas = "C" Then ll = "5"
                                                    End If


                                                    If ll = "" Then ll = "0"

                                                    If PP = "" Then PP = "0"
                                                    Console.WriteLine(points + poinstdnf)
                                                    Dim totalpoints As Integer = points + poinstdnf
                                                    Dim comm As New MySqlCommand("insert into fantasy_race.raceresultsf1indy(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead,TeamNumber) values('" & week & "', '" & circut & "', '" & team & "', '" & driver & "', '" & pos1 + dnf & "', '" & totalpoints & "', '" & PP & "', '" & ll & "', '" & teamnumber & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                                                    comm.Connection.Open()
                                                    comm.ExecuteNonQuery()
                                                    comm.Connection.Close()
                                                End If
                                            End If
                                        Next


                                    End If
                                End If

                            Next

                        Next

                    End If
                End If


            Next

        Next
    End Sub

    Sub pointsnascara()




        Dim comm1 As New MySqlCommand("insert into fantasy_race.nascarracenumber (racenumber)select racenumber+1 from fantasy_race.nascarracenumber order by racenumber desc limit 1; ", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        comm1.Connection.Open()
        comm1.ExecuteNonQuery()
        comm1.Connection.Close()



        Dim week As String = TextBoxweek.Text
        Dim circut As String = TextBoxnascarcircut.Text

        Dim da As New MySqlDataAdapter("select * from fantasy_race.nascara, fantasy_race.teams where nascara.TeamNumber = teams.Number", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)


        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("NasCarADriver").ToString

            Dim key As String = dr("key").ToString
            Dim clas As String = dr("Class").ToString
            Dim team As String = dr("Team").ToString
            Dim teamnumber As String = dr("Number").ToString

            For Each ctl As Control In GroupBoxnascara.Controls
                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = key Then
                        Dim points As Integer

                        Dim pos As String = ctl.Text
                        Dim dnf As String = ""
                        Dim pos1 As String = ""
                        pos1 = pos
                        Dim rs As Match = Regex.Match(pos, "([0-9]+)([A-Z])")

                        If (rs.Groups(2).Value) = "D" Then
                            pos1 = rs.Groups(1).Value
                            dnf = rs.Groups(2).Value
                            If clas = "A" Then points = -2
                            If clas = "B" Then points = -1
                            If pos1 < "21" Then
                                pos = "30"
                            Else
                                pos = pos1
                            End If
                        End If

                        If pos = "" Then
                            pos = "23"
                            pos1 = "DNS"
                        End If
                        If pos = "DNF" Then pos = "100"
                        Dim ds As New MySqlDataAdapter("select * from fantasy_race.nascarpoints where nascarpoints.key = '" & pos & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                        Dim dt As New DataTable
                        ds.Fill(dt)

                        For Each drs As DataRow In dt.Rows
                            Dim position As String = drs("key").ToString
                            Dim a As String = drs("A").ToString
                            Dim b As String = drs("B").ToString


                            If clas = "A" Then points = points + a
                            If clas = "B" Then points = points + b

                            Dim PP As Integer
                            For Each ctl2 As Control In GroupBoxnascara.Controls
                                If TypeOf ctl2 Is CheckBox Then
                                    Dim db As CheckBox = ctl2
                                    If db.Tag = key Then
                                        If db.CheckState = CheckState.Checked Then
                                            If clas = "A" Then PP = "2"
                                            If clas = "B" Then PP = "3"
                                        End If
                                        Dim ll As Integer
                                        For Each ctl3 As Control In GroupBoxnascara.Controls
                                            If TypeOf ctl3 Is RadioButton Then
                                                Dim rb As RadioButton = ctl3
                                                If rb.Tag = key Then
                                                    If rb.Checked = True Then
                                                        If clas = "A" Then ll = "2"
                                                        If clas = "B" Then ll = "3"

                                                    End If


                                                    Dim comm As New MySqlCommand("insert into fantasy_race.raceresultsnascara(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead,TeamNumber) values('" & week & "', '" & circut & "', '" & team & "', '" & driver & "', '" & pos1 + dnf & "', '" & points & "', '" & PP & "', '" & ll & "', '" & teamnumber & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                                                    comm.Connection.Open()
                                                    comm.ExecuteNonQuery()
                                                    comm.Connection.Close()
                                                    points = 0
                                                    PP = "0"
                                                    ll = "0"
                                                End If
                                            End If
                                        Next


                                    End If
                                End If

                            Next

                        Next

                    End If
                End If


            Next

        Next
    End Sub



    Sub pointsnascarb()

        Dim week As String = TextBoxweek.Text
        Dim circut As String = TextBoxnascarcircut.Text

        Dim da As New MySqlDataAdapter("select * from fantasy_race.nascarb, fantasy_race.teams where nascarb.TeamNumber = teams.Number", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)


        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("NasCarBDriver").ToString
            Console.WriteLine(driver)
            Dim key As String = dr("key").ToString
            Dim clas As String = dr("Class").ToString
            Dim team As String = dr("Team").ToString
            Dim teamnumber As String = dr("Number").ToString

            For Each ctl As Control In GroupBoxnascarb.Controls
                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = key Then
                        Dim pos As String = ctl.Text
                        Dim points As Integer


                        Dim dnf As String = ""
                        Dim pos1 As String = ""
                        pos1 = pos
                        Dim rs As Match = Regex.Match(pos, "([0-9]+)([A-Z])")

                        If (rs.Groups(2).Value) = "D" Then
                            pos1 = rs.Groups(1).Value
                            dnf = rs.Groups(2).Value
                            If clas = "A" Then points = -2
                            If clas = "B" Then points = -1
                            If pos1 < "21" Then
                                pos = "30"
                            Else
                                pos = pos1
                            End If
                        End If
                        If pos = "" Then
                            pos = "29"
                            pos1 = "DNS"
                        End If
                        If pos = "DNF" Then pos = "100"
                        Dim ds As New MySqlDataAdapter("select * from fantasy_race.nascarpoints where nascarpoints.key = '" & pos & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                        Dim dt As New DataTable
                        ds.Fill(dt)

                        For Each drs As DataRow In dt.Rows
                            Dim position As String = drs("key").ToString
                            Dim a As String = drs("A").ToString
                            Dim b As String = drs("B").ToString


                            If clas = "A" Then points = points + a
                            If clas = "B" Then points = points + b

                            Dim PP As Integer
                            For Each ctl2 As Control In GroupBoxnascarb.Controls
                                If TypeOf ctl2 Is CheckBox Then
                                    Dim db As CheckBox = ctl2
                                    If db.Tag = key Then
                                        If db.CheckState = CheckState.Checked Then
                                            If clas = "A" Then PP = "2"
                                            If clas = "B" Then PP = "3"
                                        End If
                                        Dim ll As Integer
                                        For Each ctl3 As Control In GroupBoxnascarb.Controls
                                            If TypeOf ctl3 Is RadioButton Then
                                                Dim rb As RadioButton = ctl3
                                                If rb.Tag = key Then
                                                    If rb.Checked = True Then
                                                        If clas = "A" Then ll = "2"
                                                        If clas = "B" Then ll = "3"

                                                    End If


                                                    Dim comm As New MySqlCommand("insert into fantasy_race.raceresultsnascarb(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead,TeamNumber) values('" & week & "', '" & circut & "', '" & team & "', '" & driver & "', '" & pos1 + dnf & "', '" & points & "', '" & PP & "', '" & ll & "', '" & teamnumber & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                                                    comm.Connection.Open()
                                                    comm.ExecuteNonQuery()
                                                    comm.Connection.Close()
                                                    points = 0
                                                    PP = 0
                                                    ll = 0

                                                End If
                                            End If
                                        Next


                                    End If
                                End If

                            Next

                        Next

                    End If
                End If


            Next

        Next
    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        StandingsCalc()
    End Sub

    'Sub dotextboxthing(ByVal t As TextBox, ByVal msg As String)
    '    If t.Text = "" Then

    '        Dim title As String
    '        Dim style As MsgBoxStyle
    '        Dim response As MsgBoxResult

    '        style = MsgBoxStyle.OkOnly

    '        title = "MsgBox"   ' Define title.
    '        'Display message.
    '        response = MsgBox(msg, style, title)
    '        If response = MsgBoxResult.Ok Then
    '            t.Focus()
    '            Exit Sub
    '        End If
    '    End If

    'End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult

        If TextBoxweek.Text = "" Then
            msg = "must enter week"   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If
        End If


        'check for data and Run F1 Points
        If GroupBox1.Enabled Then
            If TextBoxCircut.Text = "" Then
                msg = "must enter week"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            If TextBoxf1c.Text = "" Then
                msg = "Data Must Be Enterd into F1 Fields or F1 Must Be Disabled"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            pointsF1()


        End If




        'Check for data and run indy points
        If GroupBoxINDY.Enabled Then
            If TextBoxindycircut.Text = "" Then
                msg = "must enter week"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            If TextBoxindy1.Text = "" Then
                msg = "Data Must Be Enterd into IndyCar Fields or IndyCar Must Be Disabled"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            pointsindy()

        End If



        If GroupBoxnascara.Enabled Then
            If TextBoxnascarcircut.Text = "" Then
                msg = "must enter week"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            If TextBox28.Text = "" Then
                msg = "Data Must Be Enterd into NasCar Fields or NasCar Must Be Disabled"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            pointsnascara()
        End If

        If GroupBoxnascarb.Enabled Then
            If TextBoxnascarcircut.Text = "" Then
                msg = "must enter week"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If
            If TextBox28.Text = "" Then
                msg = "Data Must Be Enterd into NasCar Fields or NasCar Must Be Disabled"   ' Define message.
                style = MsgBoxStyle.OkOnly
                title = "MsgBox"   ' Define title.
                'Display message.
                response = MsgBox(msg, style, title)
                If response = MsgBoxResult.Ok Then
                    Exit Sub
                End If
            End If

            pointsnascarb()
        End If



        weekTrack()



        msg = "This Weeks Race Data Has Been Enterd"   ' Define message.
        style = MsgBoxStyle.OkOnly
        title = "MsgBox"   ' Define title.
        'Display message.
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Ok Then

            If RadioButtonDisableIndy.Checked = False Then RadioButtonDisableIndy.Checked = True
            If RadioButtonDisableF1.Checked = False Then RadioButtonDisableF1.Checked = True
            If RadioButtonDisableNascar.Checked = False Then RadioButtonDisableNascar.Checked = True

        End If


    End Sub


    Sub checkfordatabeforeentry()
        If GroupBox1.Enabled Then
            If TextBoxf1c.Text = "" Then

            End If
        End If
    End Sub










    Sub retriveINDY()

        Dim da As New MySqlDataAdapter("select * from fantasy_race.indy", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("IndyDriver").ToString
            Dim key As String = dr("key").ToString

            For Each ctl As Control In GroupBoxINDY.Controls
                If TypeOf ctl Is Label Then
                    If ctl.Tag = key Then
                        ctl.Text = driver
                    End If
                End If

            Next

        Next


    End Sub

    Sub retrivef1()
        Dim da As New MySqlDataAdapter("select * from fantasy_race.f1", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("F1Driver").ToString
            Dim key As String = dr("key").ToString

            For Each ctl As Control In GroupBox1.Controls
                If TypeOf ctl Is Label Then
                    If ctl.Tag = key Then
                        ctl.Text = driver
                    End If
                End If

            Next

        Next

    End Sub

    Sub populatenascarreults()
        Dim das As New MySqlDataAdapter("select nascarb.key,NasCarBDriver,position,pollgrid,circut,running,laps from fantasy_race.nascarb,fantasy_race.nascarresults where nascarb.NasCarBDriver = nascarresults.driver order by laps desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ts As New DataTable
        das.Fill(ts)


        Dim da As New MySqlDataAdapter("select nascara.key,NasCarADriver,position,pollgrid,circut,running,laps from fantasy_race.nascara,fantasy_race.nascarresults where nascara.NasCarADriver = nascarresults.driver order by laps desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        Dim dat As New MySqlDataAdapter("select * from fantasy_race.nascarresults order by laps desc limit 1", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ta As New DataTable
        dat.Fill(ta)


        Try
            Dim lastrace As DataRow = ts.Rows(0)

            For Each dr As DataRow In t.Rows

                Dim driver As String = dr("NasCarADriver").ToString
                Dim key As String = dr("key").ToString
                Dim pos As String = dr("position").ToString
                Dim poll As String = dr("pollgrid").ToString
                Dim circut As String = dr("circut").ToString
                Dim status As String = dr("running").ToString
                Dim lapslead As String = dr("laps").ToString


                For Each ctl As Control In GroupBoxnascara.Controls

                    If pos = "100" Then pos = "DNF"
                    If TypeOf ctl Is TextBox Then

                        If ctl.Tag = "113" Then ctl.Text = circut
                        If ctl.Tag = key Then
                            If Not status = "running" Then pos = pos + "D"
                            ctl.Text = pos
                        End If
                    End If

                    If TypeOf ctl Is CheckBox Then
                        Dim cbx As CheckBox = ctl
                        If cbx.Tag = key Then
                            If poll = "1" Then
                                cbx.CheckState = CheckState.Checked
                            End If
                        End If
                    End If
                Next

            Next

            For Each dr As DataRow In ts.Rows

                Dim driver As String = dr("NasCarBDriver").ToString
                Dim key As String = dr("key").ToString
                Dim pos As String = dr("position").ToString
                Dim poll As String = dr("pollgrid").ToString
                Dim circut As String = dr("circut").ToString
                Dim status As String = dr("running").ToString
                Dim lapslead As String = dr("laps").ToString


                For Each ctl As Control In GroupBoxnascarb.Controls

                    If pos = "100" Then pos = "DNF"
                    If TypeOf ctl Is TextBox Then

                        If ctl.Tag = "113" Then ctl.Text = circut
                        If ctl.Tag = key Then
                            If Not status = "running" Then pos = pos + "D"
                            ctl.Text = pos
                        End If
                    End If

                    If TypeOf ctl Is CheckBox Then
                        Dim cbx As CheckBox = ctl
                        If cbx.Tag = key Then
                            If poll = "1" Then
                                cbx.CheckState = CheckState.Checked
                            End If
                        End If
                    End If
                Next

            Next

            Try
                Dim mlaps As DataRow = ta.Rows(0)
                Dim mostlapsdriver = mlaps("driver").ToString

                Dim dra As New MySqlDataAdapter("select * from fantasy_race.nascara where NasCarADriver= '" & mostlapsdriver & "';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")

                Dim tr As New DataTable
                dra.Fill(tr)


                Dim drs As DataRow = tr.Rows(0)
                If RadioButtonEnableNascar.Checked = False Then RadioButtonEnableNascar.Checked = True

                Dim keys As String = drs("key").ToString

                For Each ctl1 As Control In GroupBoxnascara.Controls
                    If TypeOf ctl1 Is RadioButton Then
                        Dim cbxs As RadioButton = ctl1

                        If cbxs.Tag = keys Then
                            cbxs.Checked = True
                        End If
                    End If
                Next
            Catch

            End Try

            Try
                Dim mlaps As DataRow = ta.Rows(0)
                Dim mostlapsdriver = mlaps("driver").ToString


                Dim drb As New MySqlDataAdapter("select * from fantasy_race.nascarb where NasCarBDriver= '" & mostlapsdriver & "';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                Dim tr As New DataTable

                drb.Fill(tr)

                Dim drs As DataRow = tr.Rows(0)
                If RadioButtonEnableNascar.Checked = False Then RadioButtonEnableNascar.Checked = True

                Dim keys As String = drs("key").ToString

                For Each ctl1 As Control In GroupBoxnascarb.Controls
                    If TypeOf ctl1 Is RadioButton Then
                        Dim cbxs As RadioButton = ctl1

                        If cbxs.Tag = keys Then
                            cbxs.Checked = True
                        End If
                    End If
                Next
            Catch

            End Try

            If RadioButtonEnableNascar.Checked = False Then RadioButtonEnableNascar.Checked = True




        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "Latest Nascar race has not been posted"   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If
        End Try
    End Sub

    Sub populatef1results()

        Dim da As New MySqlDataAdapter("select f1.key,F1Driver,position,pollgrid,circut,running,laps from fantasy_race.f1,fantasy_race.f1results where f1.F1Driver = f1results.driver order by laps desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("F1Driver").ToString
            Dim key As String = dr("key").ToString
            Dim pos As String = dr("position").ToString
            Dim poll As String = dr("pollgrid").ToString
            Dim circut As String = dr("circut").ToString
            Dim status As String = dr("running").ToString
            Dim lapslead As String = dr("laps").ToString


            For Each ctl As Control In GroupBox1.Controls

                If pos = "100" Then pos = "DNF"
                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = "111" Then ctl.Text = circut
                    If ctl.Tag = key Then
                        Console.WriteLine(status)
                        If Not status = "running" Then pos = pos + "D"
                        If status = "disqualified" Then pos = "DSQ"
                        If status = "did not start" Then pos = "DNS"
                        ctl.Text = pos
                    End If
                End If

                If TypeOf ctl Is CheckBox Then
                    Dim cbx As CheckBox = ctl
                    If cbx.Tag = key Then
                        If poll = "1" Then
                            cbx.CheckState = CheckState.Checked
                        End If
                    End If
                End If
            Next

        Next
        Try
            Dim drs As DataRow = t.Rows(0)
            If RadioButtonEnableF1.Checked = False Then RadioButtonEnableF1.Checked = True

            Dim keys As String = drs("key").ToString
            Dim lapsleads As String = drs("laps").ToString
            For Each ctl1 As Control In GroupBox1.Controls
                If TypeOf ctl1 Is RadioButton Then
                    Dim cbxs As RadioButton = ctl1

                    If cbxs.Tag = keys Then
                        cbxs.Checked = True
                    End If
                End If
            Next



           


            'Dim dast As New MySqlDataAdapter("SELECT * FROM fantasy_race.f1racenumber order by f1dotcom desc limit 1 ", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            'Dim dt As New DataTable
            'dast.Fill(dt)

            'Dim drf1 As DataRow = dt.Rows(0)

            'Dim datarace As String = drf1("racenumber").ToString
            'Dim dotrace As String = drf1("f1dotcom").ToString

            'dotrace = dotrace + 1
            'datarace = datarace + 1

            'Dim comm1 As New MySqlCommand("insert into fantasy_race.f1racenumber(f1dotcom,racenumber) Values ('" & dotrace & "','" & datarace & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            'comm1.Connection.Open()
            'comm1.ExecuteNonQuery()
            'comm1.Connection.Close()


        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "Latest F1 race has not been posted"   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If
        End Try


    End Sub
    Sub populateindyresults()


        Dim da As New MySqlDataAdapter("select indy.key,IndyDriver,position,pollgrid,laps,circut,running from fantasy_race.indy,fantasy_race.indyresults where indy.IndyDriver = indyresults.driver order by laps desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("IndyDriver").ToString
            Dim key As String = dr("key").ToString
            Dim pos As String = dr("position").ToString
            Dim poll As String = dr("pollgrid").ToString
            Dim lapslead As String = dr("laps").ToString
            Dim circut As String = dr("circut").ToString
            Dim status As String = dr("running").ToString



            For Each ctl As Control In GroupBoxINDY.Controls
                If Not status = "running" Then pos = pos + "D"

                If TypeOf ctl Is TextBox Then
                    If ctl.Tag = "112" Then ctl.Text = circut
                    If ctl.Tag = key Then
                        ctl.Text = pos
                    End If
                End If

                If TypeOf ctl Is CheckBox Then
                    Dim cbx As CheckBox = ctl
                    If cbx.Tag = key Then
                        If poll = "1" Then
                            cbx.CheckState = CheckState.Checked
                        End If
                    End If
                End If
            Next

        Next
        Try
            Dim drs As DataRow = t.Rows(0)
            If RadioButtonEnableIndy.Checked = False Then RadioButtonEnableIndy.Checked = True
            Dim keys As String = drs("key").ToString
            Dim lapsleads As String = drs("laps").ToString
            For Each ctl1 As Control In GroupBoxINDY.Controls
                If TypeOf ctl1 Is RadioButton Then
                    Dim cbxs As RadioButton = ctl1

                    If cbxs.Tag = keys Then
                        cbxs.Checked = True
                    End If
                End If
            Next


           


        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "Latest indy race has not been posted"   ' Define message.
            style = MsgBoxStyle.OkOnly
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then
                Exit Sub
            End If
        End Try

    End Sub
    Sub retriveNascarA()
        Dim da As New MySqlDataAdapter("select * from fantasy_race.nascara", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("NasCarADriver").ToString
            Dim key As String = dr("key").ToString

            For Each ctl As Control In GroupBoxnascara.Controls
                If TypeOf ctl Is Label Then
                    If ctl.Tag = key Then
                        ctl.Text = driver
                    End If
                End If

            Next

        Next

    End Sub

    Sub retriveNascarB()
        Dim da As New MySqlDataAdapter("select * from fantasy_race.nascarb", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim driver As String = dr("NasCarBDriver").ToString
            Dim key As String = dr("key").ToString

            For Each ctl As Control In GroupBoxnascarb.Controls
                If TypeOf ctl Is Label Then
                    If ctl.Tag = key Then
                        ctl.Text = driver
                    End If
                End If

            Next

        Next

    End Sub

    Sub weekTrack()
        Dim week As Integer = TextBoxweek.Text
        Dim comm2 As New MySqlCommand("insert into fantasy_race.week(lastweek) values('" & week & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm2.Connection.Open()
        comm2.ExecuteNonQuery()
        comm2.Connection.Close()
    End Sub

    Sub StandingsCalc()

        Dim comm5 As New MySqlCommand("Truncate fantasy_race.raceresultsall", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm5.Connection.Open()
        comm5.ExecuteNonQuery()
        comm5.Connection.Close()

        Dim comm1 As New MySqlCommand("Truncate fantasy_race.leaderboard", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm1.Connection.Open()
        comm1.ExecuteNonQuery()
        comm1.Connection.Close()


        Dim dar As New MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dat As New MySqlDataAdapter("select * from fantasy_race.raceresultsnascara", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim das As New MySqlDataAdapter("select * from fantasy_race.raceresultsnascarb", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim tb As New DataTable
        dar.Fill(tb)
        das.Fill(tb)
        dat.Fill(tb)

        For Each drs As DataRow In tb.Rows
            Dim week As String = drs("Week").ToString
            Dim Circut As String = drs("Circut").ToString
            Dim Team As String = drs("Team").ToString
            Dim driver As String = drs("Driver").ToString
            Dim position As String = drs("Position").ToString
            Dim points As String = drs("Points").ToString
            Dim poll As String = drs("Poll").ToString
            Dim Lapslead As String = drs("LapsLead").ToString
            Dim teamnumber As String = drs("TeamNumber").ToString

            Dim comm As New MySqlCommand("insert into fantasy_race.raceresultsall(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead, TeamNumber) values('" & week & "', '" & Circut & "', '" & Team & "', '" & driver & "', '" & position & "', '" & points & "', '" & poll & "', '" & Lapslead & "', '" & teamnumber & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()
        Next

        Dim da As New MySqlDataAdapter("select * from fantasy_race.teams", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim t As New DataTable
        da.Fill(t)

        For Each dr As DataRow In t.Rows

            Dim teams As String = dr("Team").ToString
            Dim teamnumb As String = dr("Number").ToString

            Dim wins As String = Wincount(teams)
            Dim poles As String = pollcount(teams)
            Dim laps As String = LapsLeadcount(teams)
            Dim five As String = topfive(teams)
            Dim ten As String = topten(teams)
            Dim dnfs As String = DNFfunction(teams)
            Dim pointbehind As String = pointsbehind(teamnumb).ToString
            Dim totals As String = totalpoints(teamnumb).ToString


            Dim comm As New MySqlCommand("insert into fantasy_race.leaderboard(Team,Behind,Wins,Poles,TopTen,TopFive,LapsLead,DNF,Points) values('" & teams & "', '" & pointbehind & "', '" & wins & "', '" & poles & "', '" & ten & "', '" & five & "', '" & laps & "', '" & dnfs & "', '" & totals & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()


        Next
        Dim addposition As New MySqlDataAdapter("select * from fantasy_race.leaderboard order by Points desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim addp As New DataTable
        addposition.Fill(addp)

        Dim comm8 As New MySqlCommand("Truncate fantasy_race.leaderboard", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm8.Connection.Open()
        comm8.ExecuteNonQuery()
        comm8.Connection.Close()

        Dim place As Integer = "1"
        For Each addt As DataRow In addp.Rows
            Dim teams As String = addt("Team").ToString
            Dim wins As String = addt("Wins").ToString
            Dim poles As String = addt("Poles").ToString
            Dim laps As String = addt("LapsLead").ToString
            Dim five As String = addt("TopFive").ToString
            Dim ten As String = addt("TopTen").ToString
            Dim dnfs As String = addt("DNF").ToString
            Dim pointbehind As String = addt("Behind").ToString
            Dim totals As String = addt("Points").ToString

            Dim comm As New MySqlCommand("insert into fantasy_race.leaderboard(Team,Behind,Wins,Poles,TopTen,TopFive,LapsLead,DNF,Points,Place) values('" & teams & "', '" & pointbehind & "', '" & wins & "', '" & poles & "', '" & ten & "', '" & five & "', '" & laps & "', '" & dnfs & "', '" & totals & "', '" & place & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()



            place = place + "1"
        Next
        Dim myda As New MySqlDataAdapter("select Place as 'Position',Team,Points,Behind,Wins,Poles,TopTen,TopFive,LapsLead,DNF from fantasy_race.leaderboard order by Points desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim lbt As New DataTable
        Dim tday As String = Date.Now.ToShortDateString
        myda.Fill(lbt)



        Try
            Dim sw As New StreamWriter("c:\Standings.html", False)

            'Write line 1 for column names
            sw.WriteLine("<html><head>Last updated on " & tday & "</head><body><table border='1'cellspacing='0' cellpadding='3' bordercolor='#000000'><tr>")
            Dim columnnames As String = ""
            For Each dc As DataColumn In lbt.Columns

                columnnames &= "<td align= 'center' bgcolor = #4FBDDD><div><b>" & dc.ColumnName & "<br></div></td>"
            Next
            columnnames = columnnames.TrimEnd(","c)
            sw.WriteLine(columnnames)
            sw.WriteLine("</tr>")
            'Write out the rows
            Dim I As Integer = "1"
            For Each drs As DataRow In lbt.Rows


                Dim ROW2 As String = ""
                Dim row As String = ""
                sw.WriteLine("<tr>")
                For Each dc As DataColumn In lbt.Columns
                    'If TypeOf (drs(dc.ColumnName)) Is Byte() Then
                    '    row &= "<td>" & getstring(drs(dc.ColumnName)) & "</td>"
                    'Else
                    Dim r As Regex = New Regex("^\d+$")
                    If I = "6" Then
                        ROW2 &= "<td bgcolor = 'yellow'><font size='1' color ='red'><b>PAYOUT TO TOP 6 <br></font></td>"

                    End If
                    If I > "6" Then
                        If r.IsMatch(drs(dc.ColumnName).ToString) Then

                            row &= "<td align= 'center' bgcolor = '#C0C0C0'>" & drs(dc.ColumnName).ToString & "</td>"
                        Else

                            If drs(dc.ColumnName).ToString.Length > 4 Then

                                row &= "<td bgcolor = '#C0C0C0'><font size='4'>" & drs(dc.ColumnName).ToString & "</font></td>"
                            Else
                                row &= "<td align= 'center' bgcolor = '#C0C0C0'><font size='4' color ='red'>" & drs(dc.ColumnName).ToString & "</font></td>"

                            End If
                        End If
                    Else
                        If r.IsMatch(drs(dc.ColumnName).ToString) Then

                            row &= "<td align= 'center'>" & drs(dc.ColumnName).ToString & "</td>"
                        Else

                            If drs(dc.ColumnName).ToString.Length > 4 Then

                                row &= "<td><font size='4'>" & drs(dc.ColumnName).ToString & "</font></td>"
                            Else
                                row &= "<td align= 'center'><font size='4' color ='red'>" & drs(dc.ColumnName).ToString & "</font></td>"

                            End If
                        End If
                    End If

                Next
                sw.WriteLine("</tr>")
                '   row = row.TrimEnd(","c)
                sw.WriteLine(row)
                If I = "6" Then
                    sw.WriteLine("<tr>")
                    sw.WriteLine("</tr>")
                    sw.WriteLine(ROW2)
                End If
                I = I + "1"
            Next

            sw.WriteLine("</table></body></html>")
            sw.Close()
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "Your file is done. Would you like to open?"   ' Define message.
            style = MsgBoxStyle.YesNo
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Yes Then
                sw.Close()
                'Dim excel As Microsoft.Office.Interop.Excel.Application

                'Dim wb As Microsoft.Office.Interop.Excel.Workbook

                'Try

                '    excel = New Microsoft.Office.Interop
                '    wb = Open("c:\\Standings.txt")
                '    excel.Visible = True
                '    wb.Activate()

                'Catch ex As Exception
                '    MessageBox.Show("Error accessing Excel: " + ex.ToString())



                'End Try
            End If
            If response = MsgBoxResult.No Then
                sw.Close()
                Exit Sub
            End If

        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "file must be colsed"   ' Define message.
            style = MsgBoxStyle.DefaultButton1
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then

                Exit Sub
            End If
        End Try


    End Sub

    Class TeamX
        Public Number As String
        Public drivers As String
        Public driverpoints As String
        Public Name As String
    End Class



    Sub pointsbyrace()

        CreatePBRTaqble()

        Dim newteamlist As String = String.Empty

        Dim comm5 As New MySqlCommand("Truncate fantasy_race.pointsbyrace", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm5.Connection.Open()
        comm5.ExecuteNonQuery()
        comm5.Connection.Close()

        Dim daw As New MySqlDataAdapter("SELECT * FROM fantasy_race.week group by lastweek order by key1", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ts As New DataTable
        daw.Fill(ts)

        For Each dr As DataRow In ts.Rows

            Dim week As String = dr("lastweek").ToString


            Dim da As New MySqlDataAdapter("select raceresultsnascara.Week, raceresultsnascara.Circut, raceresultsnascara.Team, raceresultsnascara.TeamNumber, raceresultsnascara.Driver as 'Drivera', raceresultsnascarb.Driver as 'Driverb', raceresultsnascara.Position as 'Positiona', raceresultsnascarb.position as 'Positionb', raceresultsnascara.Points as 'Pointsa', raceresultsnascarb.Points as 'Pointsb', raceresultsnascara.Poll as 'Polla', raceresultsnascarb.Poll as 'Pollb', raceresultsnascara.LapsLead as 'LapsLeada', raceresultsnascarb.LapsLead as 'LapsLeadb'  from fantasy_race.raceresultsnascara, fantasy_race.raceresultsnascarb where raceresultsnascara.Week = '" & week & "' and raceresultsnascara.Team = raceresultsnascarb.Team and raceresultsnascara.Week = raceresultsnascarb.Week group by Circut, Team order by Team", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dt As New DataTable
            da.Fill(dt)

            Dim daf1 As New MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy where Week = '" & week & "' AND F1orIndy = '1'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dtf1 As New DataTable
            daf1.Fill(dtf1)


            Dim daindy As New MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy where Week = '" & week & "' AND F1orIndy = '0'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dtindy As New DataTable
            daindy.Fill(dtindy)

            Dim daTeam As New MySqlDataAdapter("select * from fantasy_race.teams", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dtTeam = New DataTable
            daTeam.Fill(dtTeam)


            Dim teamdic As New List(Of TeamX)
            For Each drteam As DataRow In dtTeam.Rows

                teamdic.Add(New TeamX With {.Number = drteam("Number").ToString, .Name = drteam("Team").ToString})

            Next


            Dim circuts As String = String.Empty
            For Each dr1 As DataRow In dt.Rows
                Dim Circut As String = dr1("Circut").ToString
                Dim Team As String = dr1("TeamNumber").ToString
                Dim drivera As String = dr1("Drivera").ToString
                Dim positiona As String = dr1("Positiona").ToString
                Dim pointsa As Integer = dr1("Pointsa")
                Dim polla As Integer = dr1("Polla")
                Dim Lapsleada As Integer = dr1("LapsLeada")
                Dim driverb As String = dr1("Driverb").ToString
                Dim positionb As String = dr1("Positionb").ToString
                Dim pointsb As Integer = dr1("Pointsb")
                Dim pollb As Integer = dr1("Pollb")
                Dim Lapsleadb As Integer = dr1("LapsLeadb")
                Dim Teamname As String = dr1("Team").ToString
                Dim pollav As String = ""
                Dim pollbv As String = ""
                Dim Lapsleadav As String = ""
                Dim Lapsleadbv As String = ""


                If positiona Like "100" Then positiona = "DNF"
                If positionb Like "100" Then positionb = "DNF"

                pollav = If(polla > "0", "+P", "")
                pollbv = If(pollb > "0", "+P", "")
                Lapsleadbv = If(Lapsleadb > 0, "+LL", "")
                Lapsleadav = If(Lapsleada > 0, "+LL", "")

                For Each T As TeamX In teamdic
                    If T.Number = Team Then

                        T.drivers = drivera + "-" + positiona + Lapsleadav + pollav + " " + driverb + "-" + positionb + Lapsleadbv + pollbv
                        T.driverpoints = pointsa + pointsb + polla + pollb + Lapsleada + Lapsleadb

                        Console.WriteLine(T.drivers)
                    End If
                Next


                circuts = "NASCAR" + " " + Circut

            Next
            If circuts = "" Then
            Else

                Dim TeamList As String = String.Empty
                Dim Values As String = String.Empty



                For Each t As TeamX In teamdic
                    TeamList &= t.Name.Replace(" ", "").Replace("-", "") & ", " & t.Name.Replace(" ", "").Replace("-", "") & "Points,"
                    Values &= "'" & t.drivers & "', '" & t.driverpoints & "',"
                Next
                TeamList = TeamList.TrimEnd(","c)
                Values = Values.TrimEnd(","c)
                newteamlist = TeamList
                ' Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(DukeRacing,DukeRacingPoints,TACJACRacing,TACJACRacingPoints,TheOutlaws,TheOutlawsPoints,RocketShot,RocketShotPoints,MurphyRacing,MurphyRacingPoints,AsphaltAHole,AsphaltAHolePoints,ScuderiaRacing,ScuderiaRacingPoints,GooseEggsRacing,GooseEggsRacingPoints,RowlandRacing,RowlandRacingPoints,GaleForceRacing,GaleForceRacingPoints,MelNickRacing,MelNickRacingPoints,RamRodRacing,RamRodRacingPoints,IntimidatorRacing,IntimidatorRacingPoints,SeawayRacing,SeawayRacingPoints,TeamXanax,TeamXanaxPoints,Circut,TeamGooley,TeamGooleyPoints,VortexRacing,VortexRacingPoints) Values ('" & duke & "', '" & dukep & "','" & tacjac & "','" & tacjacp & "','" & outlaws & "','" & outlawsp & "','" & rocket & "', '" & rocketp & "','" & murphy & "', '" & murphyp & "','" & asphalt & "','" & asphaltp & "','" & scuderia & "','" & scuderiap & "','" & goose & "','" & goosep & "','" & rowland & "','" & rowlandp & "', '" & gale & "','" & galep & "', '" & melnick & "','" & melnickp & "','" & ramrod & "','" & ramrodp & "','" & intimidator & "','" & intimidatorp & "','" & seaway & "','" & seawayp & "','" & xanax & "','" & xanaxp & "', '" & circuts & "', '" & gooley & "', '" & gooleyp & "', '" & vortex & "', '" & vortexp & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
                Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(" & TeamList & ",Circut) Values (" & Values & ", '" & circuts & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                comm.Connection.Open()
                comm.ExecuteNonQuery()
                comm.Connection.Close()
                circuts = ""
            End If


            For Each dr1 As DataRow In dtf1.Rows

                Dim Circut As String = dr1("Circut").ToString
                Dim Team As String = dr1("TeamNumber").ToString
                Dim drivera As String = dr1("Driver").ToString
                Dim positiona As String = dr1("Position").ToString
                Dim pointsa As Integer = dr1("Points")
                Dim polla As Integer = dr1("Poll")
                Dim Lapsleada As Integer = dr1("LapsLead")
                Dim Teamname As String = dr1("Team").ToString

                Dim pollav As String = ""

                Dim Lapsleadav As String = ""


                pollav = If(polla > "0", "+P", "")
                Lapsleadav = If(Lapsleada > "0", "+LL", "")



                If positiona Like "100" Then positiona = "DNF"

                circuts = "F1" + " " + Circut
                For Each T As TeamX In teamdic
                    If T.Number = Team Then

                        T.drivers = drivera + "-" + positiona + Lapsleadav + pollav
                        T.driverpoints = pointsa + polla + Lapsleada
                    End If
                Next

                circuts = "F1" + " " + Circut
            Next





            If circuts = "" Then
            Else

                Dim TeamList As String = String.Empty
                Dim Values As String = String.Empty



                For Each t As TeamX In teamdic
                    TeamList &= t.Name.Replace(" ", "").Replace("-", "") & ", " & t.Name.Replace(" ", "").Replace("-", "") & "Points,"
                    Values &= "'" & t.drivers & "', '" & t.driverpoints & "',"
                Next
                TeamList = TeamList.TrimEnd(","c)
                Values = Values.TrimEnd(","c)
                newteamlist = TeamList
                ' Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(DukeRacing,DukeRacingPoints,TACJACRacing,TACJACRacingPoints,TheOutlaws,TheOutlawsPoints,RocketShot,RocketShotPoints,MurphyRacing,MurphyRacingPoints,AsphaltAHole,AsphaltAHolePoints,ScuderiaRacing,ScuderiaRacingPoints,GooseEggsRacing,GooseEggsRacingPoints,RowlandRacing,RowlandRacingPoints,GaleForceRacing,GaleForceRacingPoints,MelNickRacing,MelNickRacingPoints,RamRodRacing,RamRodRacingPoints,IntimidatorRacing,IntimidatorRacingPoints,SeawayRacing,SeawayRacingPoints,TeamXanax,TeamXanaxPoints,Circut,TeamGooley,TeamGooleyPoints,VortexRacing,VortexRacingPoints) Values ('" & duke & "', '" & dukep & "','" & tacjac & "','" & tacjacp & "','" & outlaws & "','" & outlawsp & "','" & rocket & "', '" & rocketp & "','" & murphy & "', '" & murphyp & "','" & asphalt & "','" & asphaltp & "','" & scuderia & "','" & scuderiap & "','" & goose & "','" & goosep & "','" & rowland & "','" & rowlandp & "', '" & gale & "','" & galep & "', '" & melnick & "','" & melnickp & "','" & ramrod & "','" & ramrodp & "','" & intimidator & "','" & intimidatorp & "','" & seaway & "','" & seawayp & "','" & xanax & "','" & xanaxp & "', '" & circuts & "', '" & gooley & "', '" & gooleyp & "', '" & vortex & "', '" & vortexp & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
                Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(" & TeamList & ",Circut) Values (" & Values & ", '" & circuts & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                comm.Connection.Open()
                comm.ExecuteNonQuery()
                comm.Connection.Close()
                circuts = ""
            End If




            For Each dr1 As DataRow In dtindy.Rows

                Dim Circut As String = dr1("Circut").ToString
                Dim Team As String = dr1("TeamNumber").ToString
                Dim drivera As String = dr1("Driver").ToString
                Dim positiona As String = dr1("Position").ToString
                Dim pointsa As Integer = dr1("Points")
                Dim polla As Integer = dr1("Poll")
                Dim Lapsleada As Integer = dr1("LapsLead")
                Dim Teamname As String = dr1("Team").ToString

                Dim pollav As String = ""
                Dim Lapsleadav As String = ""

                If positiona Like "100" Then positiona = "DNF"

                pollav = If(polla > "0", "+P", "")
                Lapsleadav = If(Lapsleada > "0", "+LL", "")

                For Each T As TeamX In teamdic
                    If T.Number = Team Then

                        T.drivers = drivera + "-" + positiona + Lapsleadav + pollav
                        T.driverpoints = pointsa + polla + Lapsleada
                    End If
                Next


                circuts = "INDYCAR" + " " + Circut

            Next

            If circuts = "" Then
            Else
                Dim TeamList As String = String.Empty
                Dim Values As String = String.Empty



                For Each t As TeamX In teamdic
                    TeamList &= t.Name.Replace(" ", "").Replace("-", "") & ", " & t.Name.Replace(" ", "").Replace("-", "") & "Points,"
                    Values &= "'" & t.drivers & "', '" & t.driverpoints & "',"
                Next
                TeamList = TeamList.TrimEnd(","c)
                Values = Values.TrimEnd(","c)
                newteamlist = TeamList

                Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(" & TeamList & ",Circut) Values (" & Values & ", '" & circuts & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

                comm.Connection.Open()
                comm.ExecuteNonQuery()
                comm.Connection.Close()
                circuts = ""
            End If


        Next



        gettotalsforpbr()





        Dim PBR As New MySqlDataAdapter("SELECT Circut," & newteamlist & " FROM fantasy_race.pointsbyrace where Circut not like 'TOTAL'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim lbt As New DataTable

        PBR.Fill(lbt)
        Try
            Dim sw As New StreamWriter("c:\Pointsbyrace.html", False)


            sw.WriteLine("<html><head></head><body><table border='1'cellspacing='0' cellpadding='3' bordercolor='#980000'><tr>")
            Dim columnnames As String = ""
            For Each dc As DataColumn In lbt.Columns
                If dc.ColumnName Like "*Points" Then
                Else
                    If dc.ColumnName = "Circut" Then
                        columnnames &= "<td align= 'center' bgcolor = #4FBDDD><div><b>" & dc.ColumnName & "<br></div></td>"
                    Else

                        columnnames &= "<td colspan=2 align= 'center' bgcolor = #99FFCC><div><b>" & dc.ColumnName & "<br></div></td>"
                    End If
                End If
            Next
            columnnames = columnnames.TrimEnd(","c)
            sw.WriteLine(columnnames)
            sw.WriteLine("</tr>")

            For Each drs As DataRow In lbt.Rows
                Dim row As String = ""
                For Each dc As DataColumn In lbt.Columns
                    Dim dccol As String = drs(dc.ColumnName).ToString
                    If (drs(dc.ColumnName).ToString Like "F1*" Or drs(dc.ColumnName).ToString Like "NASCAR*" Or drs(dc.ColumnName).ToString Like "INDY*") Then

                        row &= "<td align= 'center' bgcolor = '#C0C0C0'>" & drs(dc.ColumnName).ToString & "</td>"
                    Else
                        If drs(dc.ColumnName).ToString.Length > "4" Then
                            row &= "<td>" & drs(dc.ColumnName).ToString & "</td>"
                        Else
                            row &= "<td><font size='4' color ='red'>" & drs(dc.ColumnName).ToString & "</font></td>"
                        End If
                    End If
                Next





                sw.WriteLine("</tr>")

                sw.WriteLine(row)




            Next



            ' GET TOTALS FOR  Points By Race

            Dim PBRst As New MySqlDataAdapter("SELECT Circut," & newteamlist & " FROM fantasy_race.pointsbyrace where Circut like 'TOTAL'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim lbtt As New DataTable

            PBRst.Fill(lbtt)

            For Each drstotal As DataRow In lbtt.Rows
                Dim rows As String = ""
                For Each dc As DataColumn In lbt.Columns
                    Dim dccol As String = drstotal(dc.ColumnName).ToString




                    If drstotal(dc.ColumnName).ToString.Length > "4" Then
                        rows &= "<td bgcolor = 'yellow'>" & drstotal(dc.ColumnName).ToString & "</td>"
                    Else
                        rows &= "<td bgcolor = 'yellow'><font size='4' color ='red'>" & drstotal(dc.ColumnName).ToString & "</font></td>"
                    End If

                Next
                sw.WriteLine("<tr>")
                sw.WriteLine("</tr>")
                sw.WriteLine(rows)
            Next



            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "Your file is done. Would you like to open?"   ' Define message.
            style = MsgBoxStyle.YesNo
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Yes Then
                sw.Close()

            End If
            If response = MsgBoxResult.No Then
                sw.Close()
            End If

        Catch
            Dim msg As String
            Dim title As String
            Dim style As MsgBoxStyle
            Dim response As MsgBoxResult

            msg = "file must be colsed"   ' Define message.
            style = MsgBoxStyle.DefaultButton1
            title = "MsgBox"   ' Define title.
            'Display message.
            response = MsgBox(msg, style, title)
            If response = MsgBoxResult.Ok Then

                Exit Sub
            End If
        End Try

    End Sub
    Sub gettotalsforpbr()


        Dim dast As New MySqlDataAdapter("select * from fantasy_race.teams", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ts As New DataTable
        dast.Fill(ts)

        Dim teamy As New List(Of TeamX)
        For Each drtrs As DataRow In ts.Rows

            teamy.Add(New TeamX With {.Name = drtrs("Team").ToString, .Number = drtrs("Number").ToString})
        Next


        For Each drt As DataRow In ts.Rows
            Dim teams As String = drt("Team").ToString
            Dim TeamNumber As String = drt("Number").ToString
            For Each tr As TeamX In teamy
                If tr.Name = teams Then
                    tr.drivers = teams
                    tr.driverpoints = totalpoints(TeamNumber).ToString
                End If
            Next
        Next
        Dim TeamList As String = String.Empty
        Dim Values As String = String.Empty



        For Each t As TeamX In teamy
            TeamList &= t.Name.Replace(" ", "").Replace("-", "") & ", " & t.Name.Replace(" ", "").Replace("-", "") & "Points,"
            Values &= "'" & t.drivers & "', '" & t.driverpoints & "',"
        Next
        TeamList = TeamList.TrimEnd(","c)
        Values = Values.TrimEnd(","c)
        Dim circuts As String = "TOTAL"
        ' Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(DukeRacing,DukeRacingPoints,TACJACRacing,TACJACRacingPoints,TheOutlaws,TheOutlawsPoints,RocketShot,RocketShotPoints,MurphyRacing,MurphyRacingPoints,AsphaltAHole,AsphaltAHolePoints,ScuderiaRacing,ScuderiaRacingPoints,GooseEggsRacing,GooseEggsRacingPoints,RowlandRacing,RowlandRacingPoints,GaleForceRacing,GaleForceRacingPoints,MelNickRacing,MelNickRacingPoints,RamRodRacing,RamRodRacingPoints,IntimidatorRacing,IntimidatorRacingPoints,SeawayRacing,SeawayRacingPoints,TeamXanax,TeamXanaxPoints,Circut,TeamGooley,TeamGooleyPoints,VortexRacing,VortexRacingPoints) Values ('" & duke & "', '" & dukep & "','" & tacjac & "','" & tacjacp & "','" & outlaws & "','" & outlawsp & "','" & rocket & "', '" & rocketp & "','" & murphy & "', '" & murphyp & "','" & asphalt & "','" & asphaltp & "','" & scuderia & "','" & scuderiap & "','" & goose & "','" & goosep & "','" & rowland & "','" & rowlandp & "', '" & gale & "','" & galep & "', '" & melnick & "','" & melnickp & "','" & ramrod & "','" & ramrodp & "','" & intimidator & "','" & intimidatorp & "','" & seaway & "','" & seawayp & "','" & xanax & "','" & xanaxp & "', '" & circuts & "', '" & gooley & "', '" & gooleyp & "', '" & vortex & "', '" & vortexp & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
        Dim comm As New MySqlCommand("insert into fantasy_race.pointsbyrace(" & TeamList & ",Circut) Values (" & Values & ", '" & circuts & "')", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm.Connection.Open()
        comm.ExecuteNonQuery()
        comm.Connection.Close()
        circuts = ""






    End Sub


    Sub CreatePBRTaqble()


        Dim dast As New MySqlDataAdapter("select * from fantasy_race.teams", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ts As New DataTable
        dast.Fill(ts)

        Dim teamy As New List(Of TeamX)
        For Each drtrs As DataRow In ts.Rows

            teamy.Add(New TeamX With {.Name = drtrs("Team").ToString})
        Next


        For Each drt As DataRow In ts.Rows
            Dim teams As String = drt("Team").ToString
            For Each tr As TeamX In teamy
                If tr.Name = teams Then
                    tr.drivers = teams
                    tr.driverpoints = teams & "Points"
                End If
            Next
        Next
        Dim TeamList As String = String.Empty
        Dim varchar As String = " varchar(100) NOT NULL DEFAULT '0',"
        Dim int As String = " int(11) NOT NULL DEFAULT '0',"

        For Each t As TeamX In teamy
            TeamList &= "`" & t.drivers.Replace(" ", "").Replace("-", "") & "`" & varchar & "`" & t.driverpoints.Replace(" ", "").Replace("-", "") & "`" & int
        Next

        'TeamList.TrimEnd(","c)

        Dim comm1 As New MySqlCommand("DROP TABLE IF EXISTS `fantasy_race`.`pointsbyrace`;", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm1.Connection.Open()
        comm1.ExecuteNonQuery()
        comm1.Connection.Close()


        Dim comm As New MySqlCommand("CREATE TABLE  `fantasy_race`.`pointsbyrace` ( `key` int(10) unsigned NOT NULL AUTO_INCREMENT,`Circut` varchar(45) NOT NULL," & TeamList & "  PRIMARY KEY (`key`)) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

        comm.Connection.Open()
        comm.ExecuteNonQuery()
        comm.Connection.Close()




    End Sub
    Function getstring(ByVal value As Byte()) As String
        Dim enc As System.Text.ASCIIEncoding = New System.Text.ASCIIEncoding
        Return enc.GetString(value)
    End Function





    Private Sub RadioButtonEnableF1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonEnableF1.CheckedChanged
        If GroupBox1.Enabled = False Then GroupBox1.Enabled = True
    End Sub

    Private Sub RadioButtonDisableF1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonDisableF1.CheckedChanged
        If GroupBox1.Enabled = True Then GroupBox1.Enabled = False
    End Sub


    Private Sub RadioButtonEnableIndy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonEnableIndy.CheckedChanged
        If GroupBoxINDY.Enabled = False Then GroupBoxINDY.Enabled = True
    End Sub

    Private Sub RadioButtonDisableIndy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonDisableIndy.CheckedChanged
        If GroupBoxINDY.Enabled = True Then GroupBoxINDY.Enabled = False
    End Sub

    Private Sub RadioButtonEnableNascar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonEnableNascar.CheckedChanged
        If GroupBoxnascara.Enabled = False Then GroupBoxnascara.Enabled = True
        If GroupBoxnascarb.Enabled = False Then GroupBoxnascarb.Enabled = True
    End Sub

    Private Sub RadioButtonDisableNascar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonDisableNascar.CheckedChanged
        If GroupBoxnascara.Enabled = True Then GroupBoxnascara.Enabled = False
        If GroupBoxnascarb.Enabled = True Then GroupBoxnascarb.Enabled = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult

        msg = "This will Erase all Data for the year.  Do you wish to continue?"   ' Define message.
        style = MsgBoxStyle.YesNo
        title = "MsgBox"   ' Define title.
        'Display message.
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then

            Dim comm1 As New MySqlCommand("Truncate fantasy_race.raceresultsf1indy", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm1.Connection.Open()
            comm1.ExecuteNonQuery()
            comm1.Connection.Close()

            Dim comm3 As New MySqlCommand("Truncate fantasy_race.raceresultsnascara", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm3.Connection.Open()
            comm3.ExecuteNonQuery()
            comm3.Connection.Close()

            Dim comm As New MySqlCommand("Truncate fantasy_race.week", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()

            Dim comm2 As New MySqlCommand("Truncate fantasy_race.raceresultsnascarb", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))
            comm2.Connection.Open()
            comm2.ExecuteNonQuery()
            comm2.Connection.Close()

        End If


        If response = MsgBoxResult.No Then
            Exit Sub
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        pointsbyrace()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CreatePBRTaqble()
    End Sub






    Private Sub Button5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click


        GetF1Info.getf1infodatabasedotcom()




        populatef1results()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        GetIndyInfo.getindyinfo()
        populateindyresults()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        GetNascar.getnascarinfo()
        populatenascarreults()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click



        clearforum(GroupBoxINDY)
        clearforum(GroupBox1)
        clearforum(GroupBoxnascara)
        clearforum(GroupBoxnascarb)

        TextBoxweek.Clear()

        If RadioButtonDisableIndy.Checked = False Then RadioButtonDisableIndy.Checked = True
        If RadioButtonDisableF1.Checked = False Then RadioButtonDisableF1.Checked = True
        If RadioButtonDisableNascar.Checked = False Then RadioButtonDisableNascar.Checked = True

        retriveINDY()
        retriveNascarA()
        retriveNascarB()
        Dim dar As New MySqlDataAdapter("SELECT * FROM fantasy_race.week ORDER BY key1 desc limit 1;", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim ts As New DataTable
        dar.Fill(ts)
        Try
            Dim q As DataRow = ts.Rows(0)

            Labelweek.Text = q("lastweek").ToString
            TextBoxweek.Text = q("lastweek").ToString + 1

        Catch
            Labelweek.Text = "This is the first week"

        End Try

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        If driverupdates Is Nothing Then
            driverupdates = New Thread(AddressOf DriverUpdateForm.driverupdates)
            driverupdates.Start()
        ElseIf Not driverupdates.IsAlive Then
            driverupdates = New Thread(AddressOf DriverUpdateForm.driverupdates)
            driverupdates.Start()
        End If
    End Sub

    Private Sub TextBoxweek_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxweek.KeyPress
        If Char.IsNumber(e.KeyChar) = False Then
            If e.KeyChar = CChar(ChrW(Keys.Back)) Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Buttonresetweeks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Buttonresetweeks.Click
        Dim serieslist As New List(Of String)
        serieslist.Add("f1racenumber")
        serieslist.Add("indyracenumber")
        serieslist.Add("nascarracenumber")


        For Each Str As String In serieslist


            Dim comm1 As New MySqlCommand("Truncate fantasy_race." & Str & ";", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm1.Connection.Open()
            comm1.ExecuteNonQuery()
            comm1.Connection.Close()



            Dim comm2 As New MySqlCommand("insert into fantasy_race." & Str & "(racenumber)values('1');", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm2.Connection.Open()
            comm2.ExecuteNonQuery()
            comm2.Connection.Close()


        Next
    End Sub


    Private Sub ButtonUpdateStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateStats.Click
        If updatestats Is Nothing Then
            updatestats = New Thread(AddressOf UpdateResultForm.resultupdates)
            updatestats.Start()
        ElseIf Not updatestats.IsAlive Then
            updatestats = New Thread(AddressOf UpdateResultForm.resultupdates)
            updatestats.Start()
        End If
    End Sub

    
    Private Sub CheckBox49_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox49.CheckedChanged

    End Sub

    Private Sub GroupBoxnascara_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBoxnascara.Enter

    End Sub

    Private Sub buttonServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonServer.Click
        FormloadData()
    End Sub

   
    Private Sub TextBoxServer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxServer.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            FormloadData()

        End If

    End Sub


End Class


