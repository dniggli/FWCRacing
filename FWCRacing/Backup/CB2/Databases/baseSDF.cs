using System.Collections.Generic;
using System.Web;
using System.Data;
//using System.Data.SqlServerCe;
using System;

namespace CodeBase2.Databases
{

    //public abstract class baseSDF
    //{

    //    /// <summary>
    //    /// Creates a SqlCeConnection associated with the site's Live or TestDB as appropriate.
    //    /// <para>get { return new System.Data.SqlServerCe.SqlCeConnection("Data Source = codabar.mdf"); }</para>
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public abstract SqlCeConnection GetConnection
    //    {
    //        //Example:
    //        //get { return new System.Data.SqlServerCe.SqlCeConnection("Data Source = codabar.mdf"); }
    //        get;
    //    }

    //    /// <summary>
    //    /// Creates a GetSqlCeCommand and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeCommand GetCommand()
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        return comm;
    //    }


    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeCommand GetCommand(string CommandText)
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        comm.CommandText = CommandText;
    //        return comm;
    //    }

    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeCommand GetCommand(string CommandText, params SqlCeParameter[] @params)
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        comm.CommandText = CommandText;
    //        comm.Parameters.AddRange(@params);
    //        return comm;
    //    }

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled array of ListItems with the given CommandText and returns the first column of that table as the listItem value and the second column as the ListItem text.   Uses the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <param name="commandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public System.Web.UI.WebControls.ListItem[] FilledListItemsWeb(string commandText)
    //    {
    //        DataTable dt = FilledTable(commandText);
    //        var o = new List<System.Web.UI.WebControls.ListItem>();

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            o.Add(new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString()));
    //        }

    //        return o.ToArray();
    //    }
    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
    //    /// </summary>
    //    /// <param name="commandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string commandText)
    //    {
    //        var ddl = new System.Web.UI.WebControls.DropDownList();
    //        ddl.Items.AddRange(FilledListItemsWeb(commandText));
    //        return ddl;
    //    }

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="commandText"></param>
    //    /// <returns></returns>
    //    public System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string id, string commandText)
    //    {
    //        var ddl = FilledDropDownWeb(commandText);
    //        ddl.ID = id;
    //        return ddl;
    //    }

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled table with the given CommandText and returns the first column of that table as an object array.  Uses the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <param name="CommandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public string[] FilledColumn(string CommandText)
    //    {
    //        DataTable dt = FilledTable(CommandText);
    //        List<string> o = new List<string>();

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            o.Add(dr[0].ToString());
    //        }

    //        return o.ToArray();
    //    }


    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public string ScalarValue(string CommandText)
    //    {
    //        string scalarvalue = null;
    //        SqlCeCommand comm = GetCommand(CommandText);
    //        comm.Connection.Open();
    //        scalarvalue = comm.ExecuteScalar().ToString();
    //        comm.Connection.Close();
    //        return scalarvalue;
    //    }

    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public string ScalarValue(string CommandText, params SqlCeParameter[] @params)
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        comm.CommandText = CommandText;
    //        foreach (SqlCeParameter param in @params)
    //        {
    //            comm.Parameters.Add(param);
    //        }
    //        comm.Connection.Open();
    //        string @out = comm.ExecuteScalar().ToString();
    //        comm.Connection.Close();
    //        return @out;
    //    }

    //    /// <summary>
    //    /// Sets a single scalar value in the SqlCe database with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <remarks></remarks>
    //    public void SetValue(string CommandText)
    //    {
    //        SqlCeCommand comm = GetCommand(CommandText);
    //        comm.Connection.Open();
    //        comm.ExecuteNonQuery();
    //        comm.Connection.Close();
    //    }
    //    /// <summary>
    //    /// Creates a SqlCeDataAdpter and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeDataAdapter DataAdapter()
    //    {
    //        SqlCeDataAdapter DA = new SqlCeDataAdapter(GetCommand());
    //        return DA;
    //    }
    //    /// <summary>
    //    /// Creates a SqlCeDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeDataAdapter DataAdapter(string CommandText, params SqlCeParameter[] @params)
    //    {
    //        SqlCeDataAdapter DA = new SqlCeDataAdapter(GetCommand(CommandText, @params));
    //        return DA;
    //    }

    //    /// <summary>
    //    /// Creates a SqlCeDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeDataAdapter DataAdapter(string CommandText)
    //    {
    //        SqlCeDataAdapter DA = new SqlCeDataAdapter(GetCommand(CommandText));
    //        return DA;
    //    }

    //    /// <summary>
    //    /// Creates a SqlCeDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeDataAdapter DataAdapter(string CommandText, bool UseCommandBuilder)
    //    {
    //        SqlCeDataAdapter DA = new SqlCeDataAdapter(GetCommand(CommandText));
    //        SqlCeCommandBuilder cb = default(SqlCeCommandBuilder);
    //        if (UseCommandBuilder)
    //        {
    //            cb = new SqlCeCommandBuilder(DA);
    //            cb.SetAllValues = false;
    //            DA.DeleteCommand = cb.GetDeleteCommand();
    //            DA.UpdateCommand = cb.GetUpdateCommand();
    //            DA.InsertCommand = cb.GetInsertCommand();

    //            DA.InsertCommand.Connection = DA.SelectCommand.Connection;
    //            DA.UpdateCommand.Connection = DA.SelectCommand.Connection;
    //            DA.DeleteCommand.Connection = DA.SelectCommand.Connection;
    //        }


    //        return DA;
    //    }

    //    /// <summary>
    //    /// Creates a SqlCeDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeDataAdapter DataAdapter(string SELECT, string INSERT, string UPDATE, string DELETE)
    //    {
    //        SqlCeDataAdapter DA = new SqlCeDataAdapter(GetCommand(SELECT));
    //        SqlCeCommand Ins = new SqlCeCommand(INSERT, DA.SelectCommand.Connection);
    //        SqlCeCommand Upd = new SqlCeCommand(UPDATE, DA.SelectCommand.Connection);
    //        SqlCeCommand Del = new SqlCeCommand(DELETE, DA.SelectCommand.Connection);
    //        DA.InsertCommand = Ins;
    //        DA.UpdateCommand = Upd;
    //        DA.DeleteCommand = Del;
    //        return DA;
    //    }

    //    ///' <summary>
    //    ///' Creates a SqlCeDataAdpter with the given CommandText and CommandBuilder and associates it with the site's Live or TestDB as appropriate.
    //    ///' </summary>
    //    ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
    //    ///' <param name="PKEY_SELECTforinsert">Used to retrieve row data after insert occurs: SELECT * FROM `Table` WHERE id=LAST_INSERT_ID();</param>
    //    ///' <returns></returns>
    //    ///' <remarks></remarks>
    //    //Public Overloads Function GetSqlCeDataAdapter(ByVal SELECTCommandText As String, ByVal PKEY_SELECTforinsert As String) As SqlCeDataAdapter
    //    //    Dim da As SqlCeDataAdapter = GetSqlCeDataAdapter(SELECTCommandText, True)
    //    //    'If UseCommandBuilder Then
    //    //    'Get the PKEY of the new row
    //    //    da.InsertCommand.CommandText = da.UpdateCommand.CommandText & PKEY_SELECTforinsert
    //    //    da.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
    //    //    da.MissingMappingAction = MissingMappingAction.Passthrough
    //    //    'End If

    //    //    Return da
    //    //End Function

    //    ///' <summary>
    //    ///' Creates a SqlCeDataAdpter with the given CommandText and CommandBuilder and associates it with the site's Live or TestDB as appropriate.
    //    ///' </summary>
    //    ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
    //    ///' <param name="Tablename">Used to build command to retrieve rowdata after insert occurs</param>
    //    ///' <param name="PKEYColumnName">Used to build command to retrieve rowdata after insert occurs</param>
    //    ///' <returns></returns>
    //    ///' <remarks></remarks>
    //    //Public Overloads Function GetSqlCeDataAdapter(ByVal SELECTCommandText As String, ByVal Tablename As String, ByVal PKEYColumnName As String) As SqlCeDataAdapter
    //    //    Return GetSqlCeDataAdapter(SELECTCommandText, "SELECT * FROM " & Tablename & " WHERE " & PKEYColumnName & "=LAST_INSERT_ID();")
    //    //End Function

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled table with the given CommandText.  Uses the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <param name="CommandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public DataTable FilledTable(string CommandText, params SqlCeParameter[] @params)
    //    {
    //        SqlCeDataAdapter DA = DataAdapter(CommandText, @params);
    //        DataTable dt = new DataTable();
    //        DA.Fill(dt);
    //        return dt;
    //    }

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled table with the given CommandText.  Uses the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <param name="CommandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public DataTable FilledTable(string CommandText)
    //    {
    //        SqlCeDataAdapter DA = DataAdapter(CommandText);
    //        DataTable dt = new DataTable();
    //        DA.Fill(dt);
    //        return dt;
    //    }

    //    /// <summary>
    //    /// Uses a SqlCeDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <param name="CommandText"></param>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public DataRow FilledRow(string CommandText)
    //    {
    //        DataTable dt = FilledTable(CommandText);
    //        DataRow dr = dt.Rows[0];
    //        return dr;
    //    }

    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeCommand ExecuteNonQuery(string CommandText)
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        comm.CommandText = CommandText;
    //        comm.Connection.Open();
    //        comm.ExecuteNonQuery();
    //        comm.Connection.Close();
    //        return comm;
    //    }

    //    /// <summary>
    //    /// Creates a GetSqlCeCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public SqlCeCommand ExecuteNonQuery(string CommandText, params SqlCeParameter[] @params)
    //    {
    //        SqlCeCommand comm = new SqlCeCommand();
    //        comm.Connection = GetConnection;
    //        comm.CommandText = CommandText;
    //        foreach (SqlCeParameter param in @params)
    //        {
    //            comm.Parameters.Add(param);
    //        }
    //        comm.Connection.Open();
    //        comm.ExecuteNonQuery();
    //        comm.Connection.Close();
    //        return comm;
    //    }

    //    /// <summary>
    //    /// Tests the SqlCeconnection with a select * FROM TABLE command
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <remarks></remarks>
    //    public bool IsSqlCeConnectionValid(string CommandText)
    //    {
    //        SqlCeDataAdapter DA = DataAdapter(CommandText);
    //        DataTable dt = new DataTable();

    //        try
    //        {
    //            DA.Fill(dt);
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }
    //}
}