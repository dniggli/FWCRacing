using System.Collections.Generic;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Web;
using System;
using System.Data;

namespace CodeBase2.Databases
{

    public static class GetDI
    {
        const string myConnectionString = "dsn=DataInovations;servername=172.18.140.209;port=1972;database=IM;authentication method=0;query timeout=1;";
        //const string myConnectionString = "DSN=DataInovations;";
        

        /// <summary>
        /// Creates a ODBCConnection associated with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcConnection GetConnection()
        {
            OdbcConnection conn = new OdbcConnection(myConnectionString);
            return conn;
        }
        /// <summary>
        /// Creates a GetODBCCommand and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcCommand GetCommand()
        {
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = GetConnection();
            return comm;
        }

        /// <summary>
        /// Uses a ODBCDataAdpter to create a filled table with the given CommandText and returns the first column of that table as an object array.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] FilledColumn(string commandText)
        {
            DataTable dt = FilledTable(commandText);
            var o = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                o.Add(dr[0].ToString());
            }

            return o.ToArray();
        }

        /// <summary>
        /// Uses a ODBCDataAdpter to create a filled array of ListItems with the given CommandText and returns the first column of that table as the listItem value and the second column as the ListItem text.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Web.UI.WebControls.ListItem[] FilledListItemsWeb(string commandText)
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
        /// Uses a ODBCDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string commandText)
        {
            var ddl = new System.Web.UI.WebControls.DropDownList();
            ddl.Items.AddRange(FilledListItemsWeb(commandText));
            return ddl;
        }

        /// <summary>
        /// Uses a ODBCDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string id, string commandText)
        {
            var ddl = FilledDropDownWeb(commandText);
            ddl.ID = id;
            return ddl;
        }

        /// <summary>
        /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcCommand GetCommand(string CommandText)
        {
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = GetConnection();
            comm.CommandText = CommandText;
            return comm;
        }
        /// <summary>
        /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ScalarValue(string CommandText)
        {
            string scalarvalue = null;
            OdbcCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            scalarvalue = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return scalarvalue;
        }

        /// <summary>
        /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ScalarValue(string CommandText, params OdbcParameter[] @params)
        {
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = GetConnection();
            comm.CommandText = CommandText;
            foreach (OdbcParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            string @out = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return @out;
        }

        /// <summary>
        /// Sets a single scalar value in the ODBC database with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <remarks></remarks>
        public static void SetValue(string CommandText)
        {
            OdbcCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Creates a ODBCDataAdpter and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcDataAdapter DataAdapter()
        {
            OdbcDataAdapter DA = new OdbcDataAdapter(GetCommand());
            return DA;
        }
        /// <summary>
        /// Creates a ODBCDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcDataAdapter DataAdapter(string CommandText)
        {
            OdbcDataAdapter DA = new OdbcDataAdapter(GetCommand(CommandText));
            return DA;
        }
        /// <summary>
        /// Uses a ODBCDataAdpter to create a filled table with the given CommandText.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable FilledTable(string CommandText)
        {
            OdbcDataAdapter DA = DataAdapter(CommandText);
            DataTable dt = new DataTable();
            DA.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Uses a ODBCDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the site's Live or TestDB as appropriate.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataRow FilledRow(string CommandText)
        {
            DataTable dt = FilledTable(CommandText);
            DataRow dr = dt.Rows[0];
            return dr;
        }

        /// <summary>
        /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcCommand ExecuteNonQuery(string CommandText)
        {
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = GetConnection();
            comm.CommandText = CommandText;
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            return comm;
        }

        /// <summary>
        /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OdbcCommand ExecuteNonQuery(string CommandText, params OdbcParameter[] @params)
        {
            OdbcCommand comm = new OdbcCommand();
            comm.Connection = GetConnection();
            comm.CommandText = CommandText;
            foreach (OdbcParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            return comm;
        }
    }



}