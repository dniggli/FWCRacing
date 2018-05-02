using System.Collections.Generic;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Web;
using System;
using System.Data;

namespace CodeBase2.Databases
{
    /// <summary>
    /// Enum of the Soft Modules
    /// </summary>
    public enum SoftModuleName
    {
        /// <summary>
        /// the Soft module: lab (SoftLab)
        /// </summary>
        lab,
        /// <summary>
        /// the Soft Module: mic (SoftMic)
        /// </summary>
        mic,
        /// <summary>
        /// the Soft Module: guipat (SoftPath)
        /// </summary>
        guipat,
        /// <summary>
        /// the Soft Module: sec (SoftSecurity)
        /// </summary>
        sec,
        /// <summary>
        /// the Soft Module: mmt (Multisite Table)
        /// </summary>
        mmt
    }

    /// <summary>
    /// contains extension to let one easily get an Instance of the Soft database with a SoftModuleName enum object
    /// </summary>
    public static class SoftModuleExtension
    {
        static Dictionary<SoftModuleName, GetSoft.Instance> instances;
        static SoftModuleExtension()
        {
            instances = new Dictionary<SoftModuleName, GetSoft.Instance>();
        }
        
        /// <summary>
        /// Retrieves an instance of the ODBC database connection for this Soft Module
        /// </summary>
        /// <param name="module">lab,guipat,sec,mmt,mic</param>
        /// <returns></returns>
        public static CodeBase2.Databases.GetSoft.Instance Instance(this SoftModuleName module)
        {
            const string MySQLPrimaryServer = "Server=LIS-S22104-DB1;Database=SoftODBC;uid=anon;Password=connection;";
            const string connection = "Driver={0};servername=shlab2.urmc.rochester.edu.25041;serverdsn={1};arrayfetchon=1;arraybuffersize=64;dbq=$DBPATH/PD/LD/rpa:$DBPATH/PD/LD/rpc:$DBPATH/PD/LD/rps:$DBPATH/PD/LD/rpw:$DBPATH/dat/hlsys:$DBPATH/dat/hosplab";
            try
            {
                MySqlConnection conn = new MySqlConnection(MySQLPrimaryServer);
                conn.Open();
                MySqlCommand c = new MySqlCommand("SELECT ConnectionString FROM SoftODBC.Connections C Where Name='" + module.ToString() + "';", conn);
                string s = c.ExecuteScalar().ToString();
                conn.Close();
                if (instances[module] == null) instances[module] = new CodeBase2.Databases.GetSoft.Instance(s);

                return instances[module];
            }
            catch (Exception)
            {
                return new CodeBase2.Databases.GetSoft.Instance(string.Format(connection,"{SimbaClient}", module.ToString()));
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public static string ToSoftPathFormattedDate(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

    }

    public class GetSoft
    {
        
        public static SoftModuleName GetModule(string modulename)
        {
            modulename = modulename.ToLower();
            return ((SoftModuleName)Enum.Parse(typeof(SoftModuleName), modulename));
        }

        /// <summary>
        /// Returns the instance of this class that is setup inside of the web application state variable
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Instance Lab()
        {
            return SoftModuleName.lab.Instance();
        }


        /// <summary>
        /// Returns the instance of this class that is setup inside of the web application state variable
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Instance Path()
        {
            return SoftModuleName.guipat.Instance();
        }


        /// <summary>
        /// Returns the instance of this class that is setup inside of the web application state variable
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Instance Mic()
        {
            return SoftModuleName.mic.Instance();
        }


        /// <summary>
        /// Returns the instance of this class that is setup inside of the web application state variable
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Instance Sec()
        {
            return SoftModuleName.sec.Instance();
        }

        /// <summary>
        /// Returns the instance of this class that is setup inside of the web application state variable
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Instance MMT()
        {
            return SoftModuleName.mmt.Instance();
        }

        /// <summary>
        /// An Instance of a connection to a specific Soft module
        /// </summary>
        public class Instance : IDatabase
        {
            string myConnectionString;
            /// <summary>
            /// Creates a new instance of this class using a ODBC connectionstring
            /// "Server=localhost;Database=myDB;uid=myUID;Password=myPASS;"
            /// </summary>
            /// <remarks></remarks>
            internal Instance(string connectionstring)
            {
                myConnectionString = connectionstring;
            }

            /// <summary>
            /// Creates a ODBCConnection associated with the site's Live or TestDB as appropriate.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public OdbcConnection GetConnection()
            {
                OdbcConnection conn = new OdbcConnection(myConnectionString);
                return conn;
            }
            /// <summary>
            /// Creates a GetODBCCommand and associates it with the site's Live or TestDB as appropriate.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public OdbcCommand GetCommand()
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
            public string[] FilledColumn(string commandText)
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
            /// Uses a ODBCDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
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
            /// Uses a ODBCDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.
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
            /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public OdbcCommand GetCommand(string CommandText)
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
            public string ScalarValue(string CommandText)
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
            public string ScalarValue(string CommandText, params OdbcParameter[] @params)
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
            public void SetValue(string CommandText)
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
            public OdbcDataAdapter DataAdapter()
            {
                OdbcDataAdapter DA = new OdbcDataAdapter(GetCommand());
                return DA;
            }
            /// <summary>
            /// Creates a ODBCDataAdpter with the given CommandText and associates it with the site's Live or TestDB as appropriate.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public OdbcDataAdapter DataAdapter(string CommandText)
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
            public DataTable FilledTable(string CommandText)
            {
                OdbcDataAdapter DA = DataAdapter(CommandText);
                DataTable dt = new DataTable();
                DA.Fill(dt);
                return dt;
            }

            /// <summary>
            /// Uses a OdbcDataAdapter to create a filled table with the given CommandText.  Uses the implemented connection.
            /// </summary>
            /// <param name="CommandText"></param>
            /// <returns></returns>
            public DataTable SchemaFilledTable(string CommandText)
            {
                OdbcDataAdapter DA = DataAdapter(CommandText);
                DataTable dt = new DataTable();
                DA.FillSchema(dt, SchemaType.Source);
                return dt;
            }

            /// <summary>
            /// Uses a ODBCDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the site's Live or TestDB as appropriate.
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

            public DataRow SchemaFilledRow(string CommandText)
            {
                DataTable dt = SchemaFilledTable(CommandText);
                DataRow dr = dt.Rows[0];
                return dr;
            }

            /// <summary>
            /// dumps the description of the table to the given CSV filename
            /// </summary>
            /// <param name="selectCommandOnTable">ex: Select * From tableName</param>
            /// <param name="filename">ex: tablename.csv</param>
            public void GetSchemaDescription(string selectCommandOnTable, string filename)
            {
                DataTable schemaTable = ExecuteReader(selectCommandOnTable).GetSchemaTable();
                schemaTable.ToCSV("schema_" + filename);
            }

            /// <summary>
            /// Creates a Command with the given CommandText and associates it with the implemented connection, and opens the connection.Sends the result back as a DBDataReader. DataReader should be closed when finished reading, this will close the connection too (using CommandBehavior.CloseConnection).
            /// </summary>
            /// <param name="CommandText"></param>
            /// <returns></returns>
            public DataReader ExecuteReader(string CommandText)
            {
                OdbcCommand comm = new OdbcCommand();
                comm.Connection = GetConnection();
                comm.CommandText = CommandText;
                comm.Connection.Open();
                return new DbDataReaderWrapper(comm.ExecuteReader(CommandBehavior.CloseConnection));
            }

            /// <summary>
            /// Creates a GetODBCCommand with the given CommandText and associates it with the site's Live or TestDB as appropriate.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public OdbcCommand ExecuteNonQuery(string CommandText)
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
            public OdbcCommand ExecuteNonQuery(string CommandText, params OdbcParameter[] @params)
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

            #region IDatabase Members

            System.Data.Common.DbConnection IDatabase.GetConnection()
            {
                return this.GetConnection();
            }

            System.Data.Common.DbCommand IDatabase.GetCommand()
            {
                return this.GetCommand();
            }

            System.Data.Common.DbCommand IDatabase.GetCommand(string CommandText)
            {
                return this.GetCommand(CommandText);
            }

            System.Data.Common.DbCommand IDatabase.GetCommand(string CommandText, params System.Data.Common.DbParameter[] myparams)
            {
                //return this.GetCommand(CommandText,(OdbcParameter[])myparams);
                throw new NotImplementedException();
            }

            ListItem[] IDatabase.FilledListItemsWeb(string commandText)
            {
                throw new NotImplementedException();
            }

            DropDownList IDatabase.FilledDropDownWeb(string commandText)
            {
                throw new NotImplementedException();
            }

            DropDownList IDatabase.FilledDropDownWeb(string id, string commandText)
            {
                throw new NotImplementedException();
            }

            string[] IDatabase.FilledColumn(string CommandText, params System.Data.Common.DbParameter[] myparams)
            {
                throw new NotImplementedException();
            }

            string IDatabase.ScalarValue(string CommandText, params System.Data.Common.DbParameter[] @params)
            {
                throw new NotImplementedException();
            }

            void IDatabase.SetValue(string CommandText)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter()
            {
                return this.DataAdapter();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(string CommandText, params System.Data.Common.DbParameter[] @params)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(string CommandText, bool UseCommandBuilder, params System.Data.Common.DbParameter[] @params)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(string CommandText)
            {
                return this.DataAdapter(CommandText);
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(string CommandText, bool UseCommandBuilder)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(System.Data.Common.DbCommand Command, bool UseCommandBuilder)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbDataAdapter IDatabase.DataAdapter(string SELECT, string INSERT, string UPDATE, string DELETE)
            {
                throw new NotImplementedException();
            }

            DataTable IDatabase.FilledTable(string CommandText, params System.Data.Common.DbParameter[] @params)
            {
                throw new NotImplementedException();
            }



            DataRow IDatabase.FilledRow(string CommandText, params System.Data.Common.DbParameter[] myparams)
            {
                throw new NotImplementedException();
            }

            System.Data.Common.DbCommand IDatabase.ExecuteNonQuery(string CommandText)
            {
                return this.ExecuteNonQuery(CommandText);
            }

            System.Data.Common.DbCommand IDatabase.ExecuteNonQuery(string CommandText, params System.Data.Common.DbParameter[] @params)
            {
                return this.ExecuteNonQuery(CommandText, (OdbcParameter[])@params);
            }


            /// <summary>
            /// Tests the connection with a select * FROM TABLE command
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsDbConnectionValid(string CommandText)
            {
                OdbcDataAdapter DA = DataAdapter(CommandText);
                DataTable dt = new DataTable();

                try
                {
                    DA.FillSchema(dt, SchemaType.Source);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            

            public DatabaseName DatabaseName
            {
                get { return Databases.DatabaseName.SoftSimba; }
            }



            #region IDatabase Members

            public void SendCSVFileToDatabase(string schema, string table, string CSVFileName)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IDatabase Members

            void IDatabase.SendCSVFileToDatabase(string schema, string table, string CSVFileName)
            {
                throw new NotImplementedException();
            }

            DatabaseName IDatabase.DatabaseName
            {
                get { throw new NotImplementedException(); }
            }

            string[] IDatabase.FilledColumn(string CommandText)
            {
                throw new NotImplementedException();
            }

            string IDatabase.ScalarValue(string CommandText)
            {
                throw new NotImplementedException();
            }

            DataTable IDatabase.FilledTable(string CommandText)
            {
                throw new NotImplementedException();
            }

            DataTable IDatabase.SchemaFilledTable(string CommandText)
            {
                throw new NotImplementedException();
            }

            DataRow IDatabase.FilledRow(string CommandText)
            {
                throw new NotImplementedException();
            }

            DataRow IDatabase.SchemaFilledRow(string CommandText)
            {
                throw new NotImplementedException();
            }

            DataReader IDatabase.ExecuteReader(string CommandText)
            {
                return this.ExecuteReader(CommandText);
            }

           

            bool IDatabase.IsDbConnectionValid(string CommandText)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

            #endregion



    }
}
            