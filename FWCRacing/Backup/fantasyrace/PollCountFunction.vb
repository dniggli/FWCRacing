Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Module WinPollLapsCountFunction

    Public Function Wincount(ByVal winteam As String) As String
        Dim wins As Integer = 0
        Dim ds As New MySqlDataAdapter("SELECT Team, Position as 'Wins' FROM fantasy_race.raceresultsall where Team = '" & winteam & "'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Console.WriteLine(winteam)
        Dim dt As New DataTable
        ds.Fill(dt)
        For Each dr As DataRow In dt.Rows
            Dim winsint As Integer
            Dim winstr As String = dr("Wins").ToString
            winstr = winstr.Replace("DNS", "100")
            winstr = winstr.Replace("DSQ", "100")
            Dim topfvmtch As Match = Regex.Match(winstr, "([0-9]+)")

            ' Console.WriteLine(topfvmtch.Groups(0).Value)
            winsint = topfvmtch.Groups(0).Value

            If winsint < 2 Then wins = wins + 1

        Next

        Return wins
    End Function

    Public Function pollcount(ByVal pollteam As String) As String

        Dim ds As New MySqlDataAdapter("SELECT Team, count(Poll) as 'Poll' FROM fantasy_race.raceresultsall where Team = '" & pollteam & "' and Poll > '0'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Return dr("Poll").ToString

    End Function

    Public Function pollpoints(ByVal pollteam As String) As String

        Dim ds As New MySqlDataAdapter("SELECT Team, sum(Poll) as 'Poll' FROM fantasy_race.raceresultsall where Team = '" & pollteam & "' and Poll > '0'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Return dr("Poll").ToString

    End Function

    Public Function LapsLeadcount(ByVal lapteam As String) As String

        Dim ds As New MySqlDataAdapter("SELECT Team, count(LapsLead) as 'Lap' FROM fantasy_race.raceresultsall where Team = '" & lapteam & "' and LapsLead > '0'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Return dr("Lap").ToString

    End Function


    Public Function LapsLeadpoints(ByVal lapteam As String) As String

        Dim ds As New MySqlDataAdapter("SELECT Team, sum(LapsLead) as 'Lap' FROM fantasy_race.raceresultsall where Team = '" & lapteam & "' and LapsLead > '0'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Return dr("Lap").ToString

    End Function


    Public Function topfive(ByVal topfiveteam As String) As String
        Dim topfives As Integer = 0
        Dim ds As New MySqlDataAdapter("SELECT Team, Position as 'TopFive' FROM fantasy_race.raceresultsall where Team = '" & topfiveteam & "';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        For Each dr As DataRow In dt.Rows
            Dim tfiveint As Integer
            Dim tfivestr As String = dr("TopFive").ToString
            tfivestr = tfivestr.Replace("DNS", "100")
            tfivestr = tfivestr.Replace("DSQ", "100")
            Dim topfvmtch As Match = Regex.Match(tfivestr, "([0-9]+)")

            Console.WriteLine(topfvmtch.Groups(0).Value)
            tfiveint = topfvmtch.Groups(0).Value

            If tfiveint < 6 Then topfives = topfives + 1
        Next
        Return topfives.ToString
    End Function

    Public Function topten(ByVal toptenteam As String) As String
        Dim toptens As Integer = 0
        Dim ds As New MySqlDataAdapter("SELECT Team, Position as 'TopTen' FROM fantasy_race.raceresultsall where Team = '" & toptenteam & "';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        For Each dr As DataRow In dt.Rows
            Dim ttenint As Integer
            Dim ttenstr As String = dr("TopTen").ToString
            ttenstr = ttenstr.Replace("DNS", "100")
            ttenstr = ttenstr.Replace("DSQ", "100")
            Dim toptenmtch As Match = Regex.Match(ttenstr, "([0-9]+)")

            Console.WriteLine(toptenmtch.Groups(0).Value)
            ttenint = toptenmtch.Groups(0).Value

            If ttenint < 11 Then toptens = toptens + 1
        Next
        Return toptens.ToString
    End Function

    Public Function DNFfunction(ByVal dnfteam As String) As String

        Dim ds As New MySqlDataAdapter("SELECT Team, count(Position) as 'dnf' FROM fantasy_race.raceresultsall where Team = '" & dnfteam & "' and Position like '%D'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable
        ds.Fill(dt)

        Dim dr As DataRow = dt.Rows(0)

        Return dr("dnf").ToString
    End Function

    Public Function pointsbehind(ByVal behind As String) As String

        Dim dp As New MySqlDataAdapter("Select Team, sum(Points)+ sum(Poll)+ sum(LapsLead) as 'total' from fantasy_race.raceresultsall group by Team order by total desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dt As New DataTable

        dp.Fill(dt)
        Dim drs As DataRow = dt.Rows(0)


        Dim points As Integer = drs("total")




        Dim dpr As New MySqlDataAdapter("Select Team, sum(Points)+ sum(Poll)+ sum(LapsLead) as 'pointer' from fantasy_race.raceresultsall where TeamNumber = '" & behind & "' group by Team order by pointer desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dtr As New DataTable
        dpr.Fill(dtr)

        Dim dr As DataRow = dtr.Rows(0)

        Dim pointing As Integer = dr("pointer")

        Dim test As Integer = pointing - points
        Return test




    End Function

    Public Function totalpoints(ByVal totals As String) As String


        Dim dpr As New MySqlDataAdapter("Select Team, sum(Points)as 'pointer', sum(Poll)as 'poll', sum(LapsLead) as 'lap' from fantasy_race.raceresultsall where TeamNumber = '" & totals & "' group by Team order by pointer desc", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
        Dim dtr As New DataTable
        dpr.Fill(dtr)

        Dim dr As DataRow = dtr.Rows(0)
        Dim poll As Integer = dr("Poll")
        Dim laps As Integer = dr("Lap")
        Dim pointing As Integer = dr("pointer")

        Dim totalpoint As Integer = pointing + poll + laps

        Return totalpoint

    End Function

    Sub clearforum(ByVal groupbox As GroupBox)


        For Each ctl As Control In groupbox.Controls
            
            If TypeOf ctl Is TextBox Then ctl.Text = ""

            If TypeOf ctl Is CheckBox Then
                Dim cbx As CheckBox = ctl
                cbx.CheckState = CheckState.Unchecked
            End If

            If TypeOf ctl Is RadioButton Then
                Dim cbx As RadioButton = ctl
                cbx.Checked = CheckState.Unchecked
            End If
        Next


    End Sub


End Module
