Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports CodeBase2.Web
Imports HtmlAgilityPack
Imports Fizzler
Imports Fizzler.Systems.HtmlAgilityPack
Imports System.Threading

Public Class DriverUpdateForm

    Dim teamnum As String
    Dim teamdict As New Dictionary(Of String, Integer)

    Public Shared Sub driverupdates()

        Dim updates As Form = New DriverUpdateForm
        Application.Run(updates)


    End Sub





    Private Sub DriverUpdateForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        loadseries()


    End Sub



    Sub loadseries()

        ComboBox1.Items.Add("F1")
        ComboBox1.Items.Add("Nascar")
        ComboBox1.Items.Add("Indy")


    End Sub

    

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        teamdict.Clear()
        ListBoxTeams.Items.Clear()
        TextBoxDriver.Clear()
        TextBoxClass1.Clear()
        TextBoxClass2.Clear()
        TextBoxDriver2.Clear()
        TextBoxTeam.Clear()

        If Panel1.Enabled Then Panel1.Enabled = False

        Dim ch As New MySqlDataAdapter("select * from fantasy_race.teams;", "server=" & Form1.server & ";uid=dave;pwd=vvo084;")
        Dim dt As New DataTable
        ch.Fill(dt)

        For Each dr As DataRow In dt.Rows
            ListBoxTeams.Items.Add(dr("Team").ToString)
            Dim teams As String = dr("Team").ToString
            Dim teamsid As Integer = dr("Number").ToString

            teamdict.Add(teams, teamsid)

        Next


    End Sub



    Private Sub ListBoxTeams_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxTeams.SelectedIndexChanged

        Dim teams As String = teamdict(ListBoxTeams.Text)
        Dim driver As String = ""
        Dim series As String = ""
        Dim nasA As String = "nascara"
        Dim nasB As String = "nascarb"

        If Not ComboBox1.Text = "Nascar" Then

            If Panel1.Enabled Then Panel1.Enabled = False
            TextBoxClass2.Clear()
            TextBoxDriver2.Clear()


            If ComboBox1.Text = "F1" Then
                series = "f1"
                driver = "F1Driver"
                LabelDriver1.Text = "F1 Driver"
            End If
            If ComboBox1.Text = "Indy" Then
                series = "indy"
                driver = "IndyDriver"
                LabelDriver1.Text = "Indy Driver"
            End If


            Dim drivers As New MySqlDataAdapter("select * from fantasy_race." & series & " where TeamNumber = '" & teams & "'", "server=" & Form1.server & ";uid=dave;pwd=vvo084;")
            Dim dtf1 As New DataTable

            drivers.Fill(dtf1)

            Dim dr As DataRow = dtf1.Rows(0)

            TextBoxDriver.Text = dr("" & driver & "").ToString
            TextBoxClass1.Text = dr("Class").ToString

            TextBoxTeam.Text = ListBoxTeams.Text
            teamnum = teamdict(TextBoxTeam.Text)
        Else
            Panel1.Enabled = True

            Dim nascar As New MySqlDataAdapter("select nascara.Class as 'Classa',nascara.NasCarADriver,nascarb.NasCarBDriver,nascarb.Class as 'Classb' from fantasy_race.nascara, fantasy_race.nascarb where nascara.TeamNumber = '" & teams & "' and nascara.TeamNumber = nascarb.TeamNumber;", "server=" & Form1.server & ";uid=dave;pwd=vvo084;")
            Dim nas As New DataTable
            nascar.Fill(nas)

            Dim dr As DataRow = nas.Rows(0)

            TextBoxDriver.Text = dr("NasCarADriver").ToString
            TextBoxClass1.Text = dr("Classa").ToString
            TextBoxDriver2.Text = dr("NasCarBDriver").ToString
            TextBoxClass2.Text = dr("Classb").ToString
            TextBoxTeam.Text = ListBoxTeams.Text
            teamnum = teamdict(TextBoxTeam.Text)

        End If

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



        If ComboBox1.Text = "F1" Then

            Dim comm As New MySqlCommand("update fantasy_race.f1 set F1Driver = '" & TextBoxDriver.Text & "',Class = '" & TextBoxClass1.Text & "' where TeamNumber = '" & teamnum & "'", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()


        End If



        If ComboBox1.Text = "Indy" Then

            Dim comm As New MySqlCommand("update fantasy_race.indy set IndyDriver = '" & TextBoxDriver.Text & "',Class = '" & TextBoxClass1.Text & "' where TeamNumber = '" & teamnum & "'", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()


        End If



        If ComboBox1.Text = "Nascar" Then

            'Console.WriteLine(teamdict(TextBoxTeam.Text))


            Dim comm As New MySqlCommand("update fantasy_race.nascara set NasCarADriver = '" & TextBoxDriver.Text & "',Class = '" & TextBoxClass1.Text & "' where TeamNumber = '" & teamnum & "'", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

            comm.Connection.Open()
            comm.ExecuteNonQuery()
            comm.Connection.Close()

            Dim commz As New MySqlCommand("update fantasy_race.teams set Team ='" & TextBoxTeam.Text & "' where Number = '" & teamnum & "'", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))
            commz.Connection.Open()
            commz.ExecuteNonQuery()
            commz.Connection.Close()

            Dim comm1 As New MySqlCommand("update fantasy_race.nascarb set NasCarbDriver = '" & TextBoxDriver2.Text & "',Class = '" & TextBoxClass2.Text & "' where TeamNumber = '" & teamnum & "'", New MySqlConnection("server=" & Form1.server & ";uid=dave;pwd=vvo084;"))

            comm1.Connection.Open()
            comm1.ExecuteNonQuery()
            comm1.Connection.Close()


        End If

    End Sub
End Class