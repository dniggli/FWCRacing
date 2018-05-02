using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

 public partial class Form1 : Form
{
        public Form1()
        {
            InitializeComponent();
        }
    private void Form1_Load(object sender, System.EventArgs e)
    {
        retrivef1();
        retriveINDY();
        retriveNascarA();
        retriveNascarB();
        MySqlDataAdapter dar = new MySqlDataAdapter("SELECT * FROM fantasy_race.week ORDER BY key1 desc limit 1;", "server=localhost;uid=root;pwd=vvo084;");
        DataTable ts = new DataTable();
        dar.Fill(ts);
        try
        {
            DataRow q = ts.Rows[0];
            
            labelweek.Text = q["lastweek"].ToString();


        }
        catch
        {
            Labelweek.Text = "This is the first week";

        }
    }


    public void pointsF1()
    {


        string week = "Week" + " " + TextBoxweek.Text;
        string circut = TextBoxCircut.Text;

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.f1, fantasy_race.teams where f1.TeamNumber = teams.Number", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);



        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["F1Driver"].ToString();
            string key = dr["key"].ToString();
            string clas = dr["Class"].ToString();
            string team = dr["Team"].ToString();

            foreach (Control ctl in GroupBox1.Controls)
            {
                if (ctl is TextBox)
                {
                    if (ctl.Tag == key)
                    {
                        string pos = ctl.Text;
                        if (pos == "DNF")
                            pos = "100";
                        MySqlDataAdapter ds = new MySqlDataAdapter("select * from fantasy_race.f1points where f1points.key = '" + pos + "'", "server=localhost;uid=root;pwd=vvo084;");
                        DataTable dt = new DataTable();
                        ds.Fill(dt);
                        foreach (DataRow drs in dt.Rows)
                        {
                            string position = drs["key"].ToString();
                            int a = (int)drs["A"];
                            int b = (int)drs["B"];
                            int c = (int)drs["C"];
                            int points = 0;
                            if (clas == "A")
                                points = a;
                            if (clas == "B")
                                points = b;
                            if (clas == "C")
                                points = c;
                            int ll = 0;
                            foreach (Control ctl2 in GroupBox1.Controls)
                            {
                                if (ctl2 is CheckBox)
                                {
                                    CheckBox db = ctl2;
                                    if (db.Tag == key)
                                    {
                                        if (db.CheckState == CheckState.Checked)
                                        {
                                            if (clas == "A")
                                                ll = "2";
                                            if (clas == "B")
                                                ll = "3";
                                            if (clas == "C")
                                                ll = "4";
                                        }
                                        int PP = 0;
                                        foreach (Control ctl3 in GroupBox1.Controls)
                                        {
                                            if (ctl3 is RadioButton)
                                            {
                                                RadioButton rb = ctl3;
                                                if (rb.Tag == key)
                                                {
                                                    if (rb.Checked == true)
                                                    {
                                                        if (clas == "A")
                                                            PP = "2";
                                                        if (clas == "B")
                                                            PP = "3";
                                                        if (clas == "C")
                                                            PP = "4";
                                                    }




                                                    points = points + ll + PP;

                                                    MySqlCommand comm = new MySqlCommand("insert into fantasy_race.raceresultsf1indy(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead) values('" + week + "', '" + circut + "', '" + team + "', '" + driver + "', '" + pos + "', '" + points + "', '" + PP + "', '" + ll + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

                                                    comm.Connection.Open();
                                                    comm.ExecuteNonQuery();
                                                    comm.Connection.Close();

                                                    PP = "0";
                                                    ll = "0";
                                                }
                                            }
                                        }


                                    }
                                }

                            }

                        }

                    }
                }


            }

        }

    }


    public void pointsindy()
    {

        string week = "Week" + " " + TextBoxweek.Text;
        string circut = TextBoxindycircut.Text;

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.indy, fantasy_race.teams where indy.TeamNumber = teams.Number", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);



        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["IndyDriver"].ToString();
            string key = dr["key"].ToString();
            string clas = dr["Class"].ToString();
            string team = dr["Team"].ToString();

            foreach (Control ctl in GroupBoxINDY.Controls)
            {
                if (ctl is TextBox)
                {
                    if (ctl.Tag == key)
                    {
                        string pos = ctl.Text;
                        if (pos == "DNF")
                            pos = "100";
                        MySqlDataAdapter ds = new MySqlDataAdapter("select * from fantasy_race.f1points where f1points.key = '" + pos + "'", "server=localhost;uid=root;pwd=vvo084;");
                        DataTable dt = new DataTable();
                        ds.Fill(dt);
                        foreach (DataRow drs in dt.Rows)
                        {
                            string position = drs["key"].ToString();
                            string a = drs["A"].ToString();
                            string b = drs["B"].ToString();
                            string c = drs["C"].ToString();
                            string points = string.Empty;
                            if (clas == "A")
                                points = a;
                            if (clas == "B")
                                points = b;
                            if (clas == "C")
                                points = c;
                            string ll = "";
                            foreach (Control ctl2 in GroupBoxINDY.Controls)
                            {
                                if (ctl2 is CheckBox)
                                {
                                    CheckBox db = ctl2;
                                    if (db.Tag == key)
                                    {
                                        if (db.CheckState == CheckState.Checked)
                                        {
                                            if (clas == "A")
                                                ll = "2";
                                            if (clas == "B")
                                                ll = "3";
                                            if (clas == "C")
                                                ll = "4";
                                        }
                                        string PP = "";
                                        foreach (Control ctl3 in GroupBoxINDY.Controls)
                                        {
                                            if (ctl3 is RadioButton)
                                            {
                                                RadioButton rb = ctl3;
                                                if (rb.Tag == key)
                                                {
                                                    if (rb.Checked == true)
                                                    {
                                                        if (clas == "A")
                                                            PP = "2";
                                                        if (clas == "B")
                                                            PP = "3";
                                                        if (clas == "C")
                                                            PP = "4";
                                                    }


                                                    if (string.IsNullOrEmpty(ll))
                                                        ll = "0";

                                                    if (string.IsNullOrEmpty(PP))
                                                        PP = "0";
                                                    MySqlCommand comm = new MySqlCommand("insert into fantasy_race.raceresultsf1indy(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead) values('" + week + "', '" + circut + "', '" + team + "', '" + driver + "', '" + pos + "', '" + points + "', '" + PP + "', '" + ll + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

                                                    comm.Connection.Open();
                                                    comm.ExecuteNonQuery();
                                                    comm.Connection.Close();
                                                }
                                            }
                                        }


                                    }
                                }

                            }

                        }

                    }
                }


            }

        }
    }


    public void pointsnascara()
    {
        string week = "Week" + " " + TextBoxweek.Text;
        string circut = TextBoxnascarcircut.Text;

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.nascara, fantasy_race.teams where nascara.TeamNumber = teams.Number", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);



        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["NasCarADriver"].ToString();
            string key = dr["key"].ToString();
            string clas = dr["Class"].ToString();
            string team = dr["Team"].ToString();

            foreach (Control ctl in GroupBoxnascara.Controls)
            {
                if (ctl is TextBox)
                {
                    if (ctl.Tag == key)
                    {
                        string pos = ctl.Text;
                        if (pos == "DNF")
                            pos = "100";
                        MySqlDataAdapter ds = new MySqlDataAdapter("select * from fantasy_race.nascarpoints where nascarpoints.key = '" + pos + "'", "server=localhost;uid=root;pwd=vvo084;");
                        DataTable dt = new DataTable();
                        ds.Fill(dt);
                        foreach (DataRow drs in dt.Rows)
                        {
                            string position = drs["key"].ToString();
                            string a = drs["A"].ToString();
                            string b = drs["B"].ToString();

                            int points = 0;
                            if (clas == "A")
                                points = a;
                            if (clas == "B")
                                points = b;

                            int ll = 0;
                            foreach (Control ctl2 in GroupBoxnascara.Controls)
                            {
                                if (ctl2 is CheckBox)
                                {
                                    CheckBox db = ctl2;
                                    if (db.Tag == key)
                                    {
                                        if (db.CheckState == CheckState.Checked)
                                        {
                                            if (clas == "A")
                                                ll = "2";
                                            if (clas == "B")
                                                ll = "3";
                                        }
                                        int PP = 0;
                                        foreach (Control ctl3 in GroupBoxnascara.Controls)
                                        {
                                            if (ctl3 is RadioButton)
                                            {
                                                RadioButton rb = ctl3;
                                                if (rb.Tag == key)
                                                {
                                                    if (rb.Checked == true)
                                                    {
                                                        if (clas == "A")
                                                            PP = "2";
                                                        if (clas == "B")
                                                            PP = "3";

                                                    }

                                                    points = points + ll + PP;
                                                    MySqlCommand comm = new MySqlCommand("insert into fantasy_race.raceresultsnascara(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead) values('" + week + "', '" + circut + "', '" + team + "', '" + driver + "', '" + pos + "', '" + points + "', '" + PP + "', '" + ll + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

                                                    comm.Connection.Open();
                                                    comm.ExecuteNonQuery();
                                                    comm.Connection.Close();

                                                    PP = "0";
                                                    ll = "0";
                                                }
                                            }
                                        }


                                    }
                                }

                            }

                        }

                    }
                }


            }

        }
    }




    public void pointsnascarb()
    {
        string week = "Week" + " " + TextBoxweek.Text;
        string circut = TextBoxnascarcircut.Text;

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.nascarb, fantasy_race.teams where nascarb.TeamNumber = teams.Number", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);



        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["NasCarBDriver"].ToString();
            string key = dr["key"].ToString();
            string clas = dr["Class"].ToString();
            string team = dr["Team"].ToString();

            foreach (Control ctl in GroupBoxnascara.Controls)
            {
                if (ctl is TextBox)
                {
                    if (ctl.Tag == key)
                    {
                        string pos = ctl.Text;
                        if (pos == "DNF")
                            pos = "100";
                        MySqlDataAdapter ds = new MySqlDataAdapter("select * from fantasy_race.nascarpoints where nascarpoints.key = '" + pos + "'", "server=localhost;uid=root;pwd=vvo084;");
                        DataTable dt = new DataTable();
                        ds.Fill(dt);
                        foreach (DataRow drs in dt.Rows)
                        {
                            string position = drs["key"].ToString();
                            string a = drs["A"].ToString();
                            string b = drs["B"].ToString();

                            int points = 0;
                            if (clas == "A")
                                points = a;
                            if (clas == "B")
                                points = b;

                            int ll = 0;
                            foreach (Control ctl2 in GroupBoxnascarb.Controls)
                            {
                                if (ctl2 is CheckBox)
                                {
                                    CheckBox db = ctl2;
                                    if (db.Tag == key)
                                    {
                                        if (db.CheckState == CheckState.Checked)
                                        {
                                            if (clas == "A")
                                                ll = "2";
                                            if (clas == "B")
                                                ll = "3";
                                        }
                                        int PP = 0;
                                        foreach (Control ctl3 in GroupBoxnascarb.Controls)
                                        {
                                            if (ctl3 is RadioButton)
                                            {
                                                RadioButton rb = ctl3;
                                                if (rb.Tag == key)
                                                {
                                                    if (rb.Checked == true)
                                                    {
                                                        if (clas == "A")
                                                            PP = "2";
                                                        if (clas == "B")
                                                            PP = "3";

                                                    }

                                                    points = points + ll + PP;

                                                    MySqlCommand comm = new MySqlCommand("insert into fantasy_race.raceresultsnascarb(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead) values('" + week + "', '" + circut + "', '" + team + "', '" + driver + "', '" + pos + "', '" + points + "', '" + PP + "', '" + ll + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

                                                    comm.Connection.Open();
                                                    comm.ExecuteNonQuery();
                                                    comm.Connection.Close();

                                                    PP = "0";
                                                    ll = "0";

                                                }
                                            }
                                        }


                                    }
                                }

                            }

                        }

                    }
                }


            }

        }
    }




    private void Button1_Click(System.Object sender, System.EventArgs e)
    {

    }




    private void Button2_Click(System.Object sender, System.EventArgs e)
    {

    }


    public void checkfordatabeforeentry()
    {
        if (GroupBox1.Enabled)
        {

            if (string.IsNullOrEmpty(TextBoxf1c.Text))
            {
            }
        }
    }











    public void retriveINDY()
    {
        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.indy", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);


        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["IndyDriver"].ToString();
            string key = dr["key"].ToString();

            foreach (Control ctl in GroupBoxINDY.Controls)
            {
                if (ctl is Label)
                {
                    if (ctl.Tag == key)
                    {
                        ctl.Text = driver;
                    }
                }

            }

        }


    }


    public void retrivef1()
    {

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.f1", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);


        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["F1Driver"].ToString();
            string key = dr["key"].ToString();

            foreach (Control ctl in GroupBox1.Controls)
            {
                if (ctl is Label)
                {
                    if (ctl.Tag == key)
                    {
                        ctl.Text = driver;
                    }
                }

            }

        }



    }
    public void retriveNascarA()
    {
        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.nascara", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);


        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["NasCarADriver"].ToString();
            string key = dr["key"].ToString();

            foreach (Control ctl in GroupBoxnascara.Controls)
            {
                if (ctl is Label)
                {
                    if (ctl.Tag == key)
                    {
                        ctl.Text = driver;
                    }
                }

            }

        }

    }

    public void retriveNascarB()
    {
        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.nascarb", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);


        foreach (DataRow dr in t.Rows)
        {
            string driver = dr["NasCarBDriver"].ToString();
            string key = dr["key"].ToString();

            foreach (Control ctl in GroupBoxnascarb.Controls)
            {
                if (ctl is Label)
                {
                    if (ctl.Tag == key)
                    {
                        ctl.Text = driver;
                    }
                }

            }

        }

    }

    public void weekTrack()
    {
        string week = TextBoxweek.Text;
        MySqlCommand comm2 = new MySqlCommand("insert into fantasy_race.week(lastweek) values('" + week + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

        comm2.Connection.Open();
        comm2.ExecuteNonQuery();
        comm2.Connection.Close();
    }


    public void StandingsCalc()
    {
        MySqlCommand comm5 = new MySqlCommand("Truncate fantasy_race.raceresultsall", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

        comm5.Connection.Open();
        comm5.ExecuteNonQuery();
        comm5.Connection.Close();

        MySqlCommand comm1 = new MySqlCommand("Truncate fantasy_race.leaderboard", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

        comm1.Connection.Open();
        comm1.ExecuteNonQuery();
        comm1.Connection.Close();


        MySqlDataAdapter dar = new MySqlDataAdapter("select * from fantasy_race.raceresultsf1indy", "server=localhost;uid=root;pwd=vvo084;");
        MySqlDataAdapter dat = new MySqlDataAdapter("select * from fantasy_race.raceresultsnascara", "server=localhost;uid=root;pwd=vvo084;");
        MySqlDataAdapter das = new MySqlDataAdapter("select * from fantasy_race.raceresultsnascarb", "server=localhost;uid=root;pwd=vvo084;");
        DataTable tb = new DataTable();
        dar.Fill(tb);
        das.Fill(tb);
        dat.Fill(tb);

        foreach (DataRow drs in tb.Rows)
        {
            string week = drs["Week"].ToString();
            string Circut = drs["Circut"].ToString();
            string Team = drs["Team"].ToString();
            string driver = drs["Driver"].ToString();
            string position = drs["Position"].ToString();
            string points = drs["Points"].ToString();
            string poll = drs["Poll"].ToString();
            string Lapslead = drs["LapsLead"].ToString();

            MySqlCommand comm = new MySqlCommand("insert into fantasy_race.raceresultsall(Week,Circut,Team,Driver,Position,Points,Poll,LapsLead) values('" + week + "', '" + Circut + "', '" + Team + "', '" + driver + "', '" + position + "', '" + points + "', '" + poll + "', '" + Lapslead + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }

        MySqlDataAdapter da = new MySqlDataAdapter("select * from fantasy_race.teams", "server=localhost;uid=root;pwd=vvo084;");
        DataTable t = new DataTable();
        da.Fill(t);


        foreach (DataRow dr in t.Rows)
        {
            string teams = dr["Team"].ToString();

            string wins = Wincount(teams);
            string poles = pollcount(teams);
            string laps = LapsLeadcount(teams);
            string five = topfive(teams);
            string ten = topten(teams);
            string dnfs = DNFfunction(teams);
            string pointbehind = pointsbehind(teams).ToString;
            string totals = totalpoints(teams).ToString;


            MySqlCommand comm = new MySqlCommand("insert into fantasy_race.leaderboard(Team,Behind,Wins,Poles,TopTen,TopFive,LapsLead,DNF,Points) values('" + teams + "', '" + pointbehind + "', '" + wins + "', '" + poles + "', '" + ten + "', '" + five + "', '" + laps + "', '" + dnfs + "', '" + totals + "')", new MySqlConnection("server=localhost;uid=root;pwd=vvo084;"));

            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();


        }


        MySqlDataAdapter myda = new MySqlDataAdapter("select Team,Points,Behind,Wins,Poles,TopTen,TopFive,LapsLead,DNF from fantasy_race.leaderboard order by Points desc", "server=localhost;uid=root;pwd=vvo084;");
        DataTable lbt = new DataTable();

        myda.Fill(lbt);
        try
        {
            StreamWriter sw = new StreamWriter("c:\\Standings.csv", false);

            //Write line 1 for column names
            string columnnames = "";
            foreach (DataColumn dc in lbt.Columns)
            {
                columnnames += "\"" + dc.ColumnName + "\",";
            }
            columnnames = columnnames.TrimEnd(',');
            sw.WriteLine(columnnames);
            //Write out the rows
            foreach (DataRow drs in lbt.Rows)
            {
                string row = "";
                foreach (DataColumn dc in lbt.Columns)
                {

                    if ((drs[dc.ColumnName]) is byte[])
                    {
                        row += "\"" + getstring(drs[dc.ColumnName]) + "\",";
                    }
                    else
                    {
                        row += "\"" + drs[dc.ColumnName].ToString() + "\",";
                    }

                }
                row = row.TrimEnd(',');
                sw.WriteLine(row);
            }
            sw.Close();
            string msg = null;
            string title = null;
            MsgBoxStyle style = default(MsgBoxStyle);
            MsgBoxResult response = default(MsgBoxResult);

            msg = "Your file is done. Would you like to open?";
            // Define message.
            style = MsgBoxStyle.YesNo;
            title = "MsgBox";
            // Define title.
            //Display message.
            response = Interaction.MsgBox(msg, style, title);
            if (response == MsgBoxResult.Yes)
            {
                Microsoft.Office.Interop.Excel.Application excel = default(Microsoft.Office.Interop.Excel.Application);

                Microsoft.Office.Interop.Excel.Workbook wb = default(Microsoft.Office.Interop.Excel.Workbook);


                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                    wb = excel.Workbooks.Open("c:\\\\Standings.csv");
                    excel.Visible = true;
                    wb.Activate();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error accessing Excel: " + ex.ToString());



                }
            }
            if (response == MsgBoxResult.Yes)
            {
                return;
            }

        }
        catch
        {
            string msg = null;
            string title = null;
            MsgBoxStyle style = default(MsgBoxStyle);
            MsgBoxResult response = default(MsgBoxResult);

            msg = "file must be colsed";
            // Define message.
            style = MsgBoxStyle.DefaultButton1;
            title = "MsgBox";
            // Define title.
            //Display message.
            response = Interaction.MsgBox(msg, style, title);

            if (response == MsgBoxResult.Ok)
            {
                return;
            }
        }


    }


    public void pointsbyrace()
    {
        MySqlDataAdapter da = new MySqlDataAdapter("select raceresultsnascara.Week, raceresultsnascara.Circut, raceresultsnascara.Team, raceresultsnascara.Driver as 'Drivera', raceresultsnascarb.Driver as 'Driverb', raceresultsnascara.Position as 'Positiona', raceresultsnascarb.position as 'Positionb', raceresultsnascara.Points as 'Pointsa', raceresultsnascarb.Points as 'Pointsb', raceresultsnascara.Poll as 'Polla', raceresultsnascarb.Poll as 'Pollb', raceresultsnascara.LapsLead as 'LapsLeada', raceresultsnascarb.LapsLead as 'LapsLeadb'  from fantasy_race.raceresultsnascara, fantasy_race.raceresultsnascarb where raceresultsnascara.Week = raceresultsnascarb.Week and raceresultsnascara.Circut = raceresultsnascarb.Circut", "server=localhost;uid=root;pwd=vvo084;");


    }

    public string getstring(byte[] value)
    {
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        return enc.GetString(value);
    }





    private void RadioButtonEnableF1_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }

    private void RadioButtonDisableF1_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }


    private void RadioButtonEnableIndy_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }

    private void RadioButtonDisableIndy_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }

    private void RadioButtonEnableNascar_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }

    private void RadioButtonDisableNascar_CheckedChanged(System.Object sender, System.EventArgs e)
    {

    }


    private void Button3_Click(System.Object sender, System.EventArgs e)
    {

    }
    public Form1()
    {
        Load += Form1_Load;
    }
}


