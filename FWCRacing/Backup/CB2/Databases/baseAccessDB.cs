using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System;

// Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Documents and Settings\cvanvranken\My Documents\Visual Studio 2008\Projects\TRS_Rev48a edit 4-24-09\TRS_WebSite\App_Data\FormatsDB.mdb

namespace CodeBase2.Databases
{

    public abstract class baseAccessDB
    {

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="DBFileLocation"></param>
        ///// <remarks></remarks>
        //public baseAccessDB(string DBFileLocation)
        //{

        //}

        /// <summary>
        /// C:\Documents and Settings\cvanvranken\My Documents\Visual Studio 2008\Projects\TRS_Rev48a edit 4-24-09\TRS_WebSite\App_Data\FormatsDB.mdb
        /// </summary>
        protected abstract string AccessFileLocation
        {
            get;
        }

        /// <summary>
        /// Creates a OleDbConnection associated with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbConnection GetConnection
        {
            get
            {
                return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + AccessFileLocation);
            }
        }

        /// <summary>
        /// ConnectionTimeout for Commands
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Creates a GetOleDbCommand and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbCommand GetCommand()
        {
            OleDbCommand comm = new OleDbCommand();

            if (Timeout != null) comm.CommandTimeout = (int)Timeout;

            comm.Connection = GetConnection;
            return comm;
        }


        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbCommand GetCommand(string CommandText)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            return comm;
        }

        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbCommand GetCommand(string CommandText, params OleDbParameter[] @params)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            comm.Parameters.AddRange(@params);
            return comm;
        }

        /// <summary>
        /// Creates an OleDbDataReader, OleDbDataReader need to be closed when finished
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataReader GetReader(string CommandText)
        {
            OleDbCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            OleDbDataReader reader = comm.ExecuteReader();
           // comm.Connection.Close();
            return new DbDataReaderWrapper(reader);
        }

        /// <summary>
        /// Creates an OleDbDataReader, OleDbDataReader need to be closed when finished
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataReader GetReader(string CommandText, params OleDbParameter[] @params)
        {
            OleDbCommand comm = GetCommand(CommandText, @params);
            comm.Connection.Open();
            OleDbDataReader reader = comm.ExecuteReader();
          //  comm.Connection.Close();
            return new DbDataReaderWrapper(reader);
        }

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled array of ListItems with the given CommandText and returns the first column of that table as the listItem value and the second column as the ListItem text.   Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Web.UI.WebControls.ListItem[] FilledListItemsWeb(string commandText)
        {
            DataTable dt = FilledTable(commandText);
            var o = new List<System.Web.UI.WebControls.ListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                o.Add(new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString()));
            }

            return o.ToArray();
        }
        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string commandText)
        {
            var ddl = new System.Web.UI.WebControls.DropDownList();
            ddl.Items.AddRange(FilledListItemsWeb(commandText));
            return ddl;
        }

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string id, string commandText)
        {
            var ddl = FilledDropDownWeb(commandText);
            ddl.ID = id;
            return ddl;
        }

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled table with the given CommandText and returns the first column of that table as an object array.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] FilledColumn(string CommandText)
        {
            DataTable dt = FilledTable(CommandText);
            List<string> o = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                o.Add(dr[0].ToString());
            }

            return o.ToArray();
        }


        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ScalarValue(string CommandText)
        {
            string scalarvalue = null;
            OleDbCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            scalarvalue = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return scalarvalue;
        }

        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ScalarValue(string CommandText, params OleDbParameter[] @params)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            foreach (OleDbParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            string @out = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return @out;
        }

        /// <summary>
        /// Sets a single scalar value in the OleDb database with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <remarks></remarks>
        public void SetValue(string CommandText)
        {
            OleDbCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Creates a OleDbDataAdpter and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbDataAdapter DataAdapter()
        {
            OleDbDataAdapter DA = new OleDbDataAdapter(GetCommand());
            return DA;
        }
        /// <summary>
        /// Creates a OleDbDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbDataAdapter DataAdapter(string CommandText, params OleDbParameter[] @params)
        {
            OleDbDataAdapter DA = new OleDbDataAdapter(GetCommand(CommandText, @params));
            return DA;
        }

        /// <summary>
        /// Creates a OleDbDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbDataAdapter DataAdapter(string CommandText)
        {
            OleDbDataAdapter DA = new OleDbDataAdapter(GetCommand(CommandText));
            return DA;
        }

        /// <summary>
        /// Creates a OleDbDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbDataAdapter DataAdapter(string CommandText, bool UseCommandBuilder)
        {
            OleDbDataAdapter DA = new OleDbDataAdapter(GetCommand(CommandText));
            OleDbCommandBuilder cb = default(OleDbCommandBuilder);
            if (UseCommandBuilder)
            {
                cb = new OleDbCommandBuilder(DA);
                cb.SetAllValues = false;
                DA.DeleteCommand = cb.GetDeleteCommand();
                DA.UpdateCommand = cb.GetUpdateCommand();
                DA.InsertCommand = cb.GetInsertCommand();

                DA.InsertCommand.Connection = DA.SelectCommand.Connection;
                DA.UpdateCommand.Connection = DA.SelectCommand.Connection;
                DA.DeleteCommand.Connection = DA.SelectCommand.Connection;
            }


            return DA;
        }

        /// <summary>
        /// Creates a OleDbDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbDataAdapter DataAdapter(string SELECT, string INSERT, string UPDATE, string DELETE)
        {
            OleDbDataAdapter DA = new OleDbDataAdapter(GetCommand(SELECT));
            OleDbCommand Ins = new OleDbCommand(INSERT, DA.SelectCommand.Connection);
            OleDbCommand Upd = new OleDbCommand(UPDATE, DA.SelectCommand.Connection);
            OleDbCommand Del = new OleDbCommand(DELETE, DA.SelectCommand.Connection);
            DA.InsertCommand = Ins;
            DA.UpdateCommand = Upd;
            DA.DeleteCommand = Del;
            return DA;
        }

        ///' <summary>
        ///' Creates a OleDbDataAdpter with the given CommandText and CommandBuilder and associates it with the site's Live or TestDB as appropriate.
        ///' </summary>
        ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
        ///' <param name="PKEY_SELECTforinsert">Used to retrieve row data after insert occurs: SELECT * FROM `Table` WHERE id=LAST_INSERT_ID();</param>
        ///' <returns></returns>
        ///' <remarks></remarks>
        //Public Overloads Function GetOleDbDataAdapter(ByVal SELECTCommandText As String, ByVal PKEY_SELECTforinsert As String) As OleDbDataAdapter
        //    Dim da As OleDbDataAdapter = GetOleDbDataAdapter(SELECTCommandText, True)
        //    'If UseCommandBuilder Then
        //    'Get the PKEY of the new row
        //    da.InsertCommand.CommandText = da.UpdateCommand.CommandText & PKEY_SELECTforinsert
        //    da.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
        //    da.MissingMappingAction = MissingMappingAction.Passthrough
        //    'End If

        //    Return da
        //End Function

        ///' <summary>
        ///' Creates a OleDbDataAdpter with the given CommandText and CommandBuilder and associates it with the site's Live or TestDB as appropriate.
        ///' </summary>
        ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
        ///' <param name="Tablename">Used to build command to retrieve rowdata after insert occurs</param>
        ///' <param name="PKEYColumnName">Used to build command to retrieve rowdata after insert occurs</param>
        ///' <returns></returns>
        ///' <remarks></remarks>
        //Public Overloads Function GetOleDbDataAdapter(ByVal SELECTCommandText As String, ByVal Tablename As String, ByVal PKEYColumnName As String) As OleDbDataAdapter
        //    Return GetOleDbDataAdapter(SELECTCommandText, "SELECT * FROM " & Tablename & " WHERE " & PKEYColumnName & "=LAST_INSERT_ID();")
        //End Function

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled table with the given CommandText.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable FilledTable(string CommandText, params OleDbParameter[] @params)
        {
            OleDbDataAdapter DA = DataAdapter(CommandText, @params);
            DataTable dt = new DataTable();
            DA.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled table with the given CommandText.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable FilledTable(string CommandText)
        {
            OleDbDataAdapter DA = DataAdapter(CommandText);
            DataTable dt = new DataTable();
            DA.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Uses a OleDbDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataRow FilledRow(string CommandText)
        {
            DataTable dt = FilledTable(CommandText);
            DataRow dr = dt.Rows[0];
            return dr;
        }

        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbCommand ExecuteNonQuery(string CommandText)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            return comm;
        }

        /// <summary>
        /// Creates a GetOleDbCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbCommand ExecuteNonQuery(string CommandText, params OleDbParameter[] @params)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            foreach (OleDbParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            return comm;
        }

        /// <summary>
        /// Tests the OleDbconnection with a select * FROM TABLE command
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsOleDbConnectionValid(string CommandText)
        {
            OleDbDataAdapter DA = DataAdapter(CommandText);
            DataTable dt = new DataTable();

            try
            {
                DA.Fill(dt);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}