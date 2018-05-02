Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports System.Threading

Public Class UpdateResultForm


    Dim teamdict As New Dictionary(Of String, Integer)
    Dim racelist As New List(Of String)

    Public Shared Sub resultupdates()

        Dim results As Form = New UpdateResultForm
        Application.Run(results)


    End Sub





    Private Sub resultupdates_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        loadseries()


    End Sub



    Sub loadseries()

        ComboBoxSeries.Items.Add("F1")
        ComboBoxSeries.Items.Add("Nascar")
        ComboBoxSeries.Items.Add("Indy")


    End Sub



    Private Sub ComboBoxSeries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxSeries.SelectedIndexChanged

        racelist.Clear()
        ListBoxRace.Items.Clear()
        ListBoxTeams.Items.Clear()
        TextBoxDriver.Clear()
        TextBoxCPosition.Clear()
        TextBoxPosition2.Clear()
        TextBoxDriver2.Clear()
        TextBoxTeam.Clear()


        Dim driver As String = ""
        Dim series As String = ""
        Dim nasA As String = "nascara"
        Dim nasB As String = "nascarb"

        If Not ComboBoxSeries.Text = "Nascar" Then

            If Panel1.Enabled Then Panel1.Enabled = False
            TextBoxPosition2.Clear()
            TextBoxDriver2.Clear()


            If ComboBoxSeries.Text = "F1" Then
                series = "1"

                LabelDriver1.Text = "F1 Driver"
            End If
            If ComboBoxSeries.Text = "Indy" Then
                series = "0"

                LabelDriver1.Text = "Indy Driver"
            End If


            Dim drivers As New MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy where F1orIndy = '" & series & "' group by Circut order by 'key'", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dtf1 As New DataTable

            drivers.Fill(dtf1)

            For Each dr As DataRow In dtf1.Rows

                If dr("Week").ToString.Length = 1 Then

                    ListBoxRace.Items.Add("   " + dr("Week").ToString + "                  " + dr("Circut").ToString)
                Else
                    ListBoxRace.Items.Add("   " + dr("Week").ToString + "                " + dr("Circut").ToString)
                End If
                'TextBoxDriver.Text = dr("" & driver & "").ToString
                'TextBoxClass1.Text = dr("Class").ToString

                'TextBoxTeam.Text = ListBoxTeams.Text


            Next

        Else


            Dim nascar As New MySqlDataAdapter("select * from fantasy_race.raceresultsnascara group by Circut order by 'key';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim nas As New DataTable
            nascar.Fill(nas)

            For Each dr As DataRow In nas.Rows

                If dr("Week").ToString.Length = 1 Then

                    ListBoxRace.Items.Add("   " + dr("Week").ToString + "                  " + dr("Circut").ToString)
                Else
                    ListBoxRace.Items.Add("   " + dr("Week").ToString + "                " + dr("Circut").ToString)
                End If
            Next

        End If


    End Sub






    Private Sub ListBoxRace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxRace.SelectedIndexChanged
        If ListBoxRace.Text = "" Then
        Else


            teamdict.Clear()
            ListBoxTeams.Items.Clear()
            TextBoxDriver.Clear()
            TextBoxCPosition.Clear()
            TextBoxPosition2.Clear()
            TextBoxDriver2.Clear()
            TextBoxTeam.Clear()
            TextBoxpoints.Clear()
            TextBoxpoints2.Clear()
            CheckBoxlapslead1.CheckState = CheckState.Unchecked
            CheckBoxlapslead2.CheckState = CheckState.Unchecked
            CheckBoxpoll1.CheckState = CheckState.Unchecked
            CheckBoxpoll2.CheckState = CheckState.Unchecked

            If Panel1.Enabled Then Panel1.Enabled = False

            Dim ch As New MySqlDataAdapter("select * from fantasy_race.teams;", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
            Dim dt As New DataTable
            ch.Fill(dt)

            For Each dr As DataRow In dt.Rows
                ListBoxTeams.Items.Add(dr("Team").ToString)
                Dim teams As String = dr("Team").ToString
                Dim teamsid As Integer = dr("Number").ToString




            Next
        End If
    End Sub


    Private Sub ListBoxTeams_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxTeams.SelectedIndexChanged

        If ListBoxTeams.Text = "" Then

        Else


            TextBoxDriver.Clear()
            TextBoxCPosition.Clear()
            TextBoxPosition2.Clear()
            TextBoxDriver2.Clear()
            TextBoxTeam.Clear()
            TextBoxpoints.Clear()
            TextBoxpoints2.Clear()
            CheckBoxlapslead1.CheckState = CheckState.Unchecked
            CheckBoxlapslead2.CheckState = CheckState.Unchecked
            CheckBoxpoll1.CheckState = CheckState.Unchecked
            CheckBoxpoll2.CheckState = CheckState.Unchecked


            Dim Circut As Match = Regex.Match(ListBoxRace.Text, " ([ 0-9 ]+)([A-Z a-z 0-9]+)")
            Dim week As Match = Regex.Match(Circut.Groups(1).Value, "([0-9]+)")



            Console.WriteLine(Circut.Groups(0).Value)
            Console.WriteLine(Circut.Groups(1).Value)
            Console.WriteLine(Circut.Groups(2).Value)



            Dim driver As String = ""
            Dim series As String = ""
            Dim nasA As String = "nascara"
            Dim nasB As String = "nascarb"

            If Not ComboBoxSeries.Text = "Nascar" Then

                If Panel1.Enabled Then Panel1.Enabled = False
                TextBoxPosition2.Clear()
                TextBoxDriver2.Clear()


                If ComboBoxSeries.Text = "F1" Then
                    series = "1"

                    LabelDriver1.Text = "F1 Driver"
                End If
                If ComboBoxSeries.Text = "Indy" Then
                    series = "0"

                    LabelDriver1.Text = "Indy Driver"
                End If


                Dim drivers As New MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy where Team = '" & ListBoxTeams.Text & "' and F1orIndy = '" & series & "' and Circut = '" & Circut.Groups(2).Value & "';", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                Dim dtf1 As New DataTable

                drivers.Fill(dtf1)

                Dim dr As DataRow = dtf1.Rows(0)

                TextBoxDriver.Text = dr("Driver").ToString
                TextBoxCPosition.Text = dr("Position").ToString
                TextBoxpoints.Text = dr("Points").ToString
                Dim poll As Integer = dr("Poll")
                Dim laps As Integer = dr("LapsLead")
                If poll > 1 Then CheckBoxpoll1.Checked = True
                If laps > 1 Then CheckBoxlapslead1.Checked = True

                TextBoxTeam.Text = ListBoxTeams.Text

            Else
                Panel1.Enabled = True

                Dim nascar As New MySqlDataAdapter("select raceresultsnascara.Driver as 'Drivera',raceresultsnascarb.Driver as 'Driverb',raceresultsnascara.Position as 'Positiona',raceresultsnascarb.Position as 'Positionb',raceresultsnascara.Points as 'Pointsa',raceresultsnascarb.Points as 'Pointsb',raceresultsnascara.Poll as 'Polla',raceresultsnascarb.Poll as 'Pollb',raceresultsnascara.LapsLead as 'lapsa',raceresultsnascarb.LapsLead as 'lapsb' from fantasy_race.raceresultsnascara, fantasy_race.raceresultsnascarb where raceresultsnascara.Team = '" & ListBoxTeams.Text & "' and raceresultsnascara.Team = raceresultsnascarb.Team and raceresultsnascara.Circut = '" & Circut.Groups(2).Value & "' and raceresultsnascara.Week = '" & week.Groups(0).Value & "' and raceresultsnascara.Week = raceresultsnascarb.Week ;", "server=" & Form1.server & ";uid=root;pwd=vvo084;")
                Dim nas As New DataTable
                nascar.Fill(nas)

                Dim dr As DataRow = nas.Rows(0)

                TextBoxDriver.Text = dr("Drivera").ToString
                TextBoxCPosition.Text = dr("Positiona").ToString
                TextBoxDriver2.Text = dr("Driverb").ToString
                TextBoxPosition2.Text = dr("Positionb").ToString
                TextBoxpoints.Text = dr("Pointsa").ToString
                TextBoxpoints2.Text = dr("Pointsb").ToString
                Dim poll As Integer = dr("Polla")
                Dim laps As Integer = dr("lapsa")
                If poll > 1 Then CheckBoxpoll1.Checked = True
                If laps > 1 Then CheckBoxlapslead1.Checked = True

                Dim pollb As Integer = dr("Pollb")
                Dim lapsb As Integer = dr("lapsb")
                If pollb > 1 Then CheckBoxpoll2.Checked = True
                If lapsb > 1 Then CheckBoxlapslead2.Checked = True

                TextBoxTeam.Text = ListBoxTeams.Text


            End If
        End If
    End Sub



    Sub driverinfo()



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult

        msg = "Are you Sure you wnat to Save Info?"   ' Define message.
        style = MsgBoxStyle.YesNo
        title = "MsgBox"   ' Define title.
        'Display message.
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then

            saveupdate()
        End If
        If response = MsgBoxResult.No Then
            Exit Sub
        End If


    End Sub


    Sub saveupdate()

        Dim Circut As Match = Regex.Match(ListBoxRace.Text, " ([ 0-9 ]+)([A-Z a-z]+)")

        Dim week As Match = Regex.Match(Circut.Groups(1).Value, "([0-9]+)")

        Dim lapslead As String = ""
        Dim poll As String = ""
        Dim lapslead2 As String = ""
        Dim poll2 As String = ""

        If CheckBoxpoll1.Checked Then
            poll = "2"
        Else
            poll = "0"

        End If

        If CheckBoxlapslead1.Checked Then
            lapslead = "2"
        Else
            lapslead = "0"
        End If

        If CheckBoxpoll2.Checked Then
            poll2 = "2"
        Else
            poll2 = "0"

        End If

        If CheckBoxlapslead2.Checked Then
            lapslead2 = "2"
        Else
            lapslead2 = "0"
        End If


        If ComboBoxSeries.Text = "F1" Then

            Dim comm As New MySqlCommand("update fantasy_race.raceresultsf1indy set Driver = '" & TextBoxDriver.Text & "',Points = '" & TextBoxpoints.Text & "',Position = '" & TextBoxCPosition.Text & "',Poll = '" & poll & "',LapsLead = '" & lapslead & "' where Team = '" & TextBoxTeam.Text & "' and Circut = '" & Circut.Groups(2).Value & "' and Week = '" & week.Groups(0).Value & "' and F1orIndy = '1'", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()


        End If



        If ComboBoxSeries.Text = "Indy" Then

            Dim comm As New MySqlCommand("update fantasy_race.raceresultsf1indy set Points = '" & TextBoxpoints.Text & "',Position = '" & TextBoxCPosition.Text & "',Poll = '" & poll & "',LapsLead = '" & lapslead & "' where Team = '" & TextBoxTeam.Text & "' and Circut = '" & Circut.Groups(2).Value & "' and Week = '" & week.Groups(0).Value & "' and F1orIndy = '0'", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()


        End If



        If ComboBoxSeries.Text = "Nascar" Then


            Dim comm As New MySqlCommand("update fantasy_race.raceresultsnascara set Points = '" & TextBoxpoints.Text & "',Position = '" & TextBoxCPosition.Text & "',Poll = '" & poll & "',LapsLead = '" & lapslead & "' where Team = '" & TextBoxTeam.Text & "' and Circut = '" & Circut.Groups(2).Value & "' and Week = '" & week.Groups(0).Value & "'", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()

            Dim comm1 As New MySqlCommand("update fantasy_race.raceresultsnascarb set Points = '" & TextBoxpoints2.Text & "',Position = '" & TextBoxPosition2.Text & "',Poll = '" & poll2 & "',LapsLead = '" & lapslead2 & "' where Team = '" & TextBoxTeam.Text & "' and Circut = '" & Circut.Groups(2).Value & "' and Week = '" & week.Groups(0).Value & "'", New MySqlConnection("server=" & Form1.server & ";uid=root;pwd=vvo084;"))

            comm1.Connection.Open()
            comm1.ExecuteNonQuery()
            comm1.Connection.Close()


        End If

    End Sub

   
End Class