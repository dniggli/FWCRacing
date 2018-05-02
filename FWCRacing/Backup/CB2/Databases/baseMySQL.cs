using System.Collections.Generic;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace CodeBase2.Databases
{
    [Obsolete("Replaced with baseMySQL2")]
    public abstract class baseMySQL : baseMySQL2
    {
        public baseMySQL()
        {

        }
    }
    public abstract class baseMySQL2 : IDatabase
    {
        //Constructor should remain protected, and a static property or Field in the dervived class called Instance should return the instance of the class.  Only one Instance of the derived class should ever exist.
         protected baseMySQL2()
        {

        }

        /// <summary>
        /// Creates a MySqlConnection
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract MySqlConnection GetConnection
        {
            get;
        }

        /// <summary>
        /// Creates a GetMySQLCommand
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlCommand GetCommand()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = GetConnection;
            return comm;
        }


        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlCommand GetCommand(string CommandText)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            return comm;
        }

        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlCommand GetCommand(string CommandText, params MySqlParameter[] myparams)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            comm.Parameters.AddRange(myparams);
            return comm;
        }


        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection, sending the result back as a DBDataReader. DataReader should be closed when finished reading.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public DataReader ExecuteReader(string CommandText)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            comm.Connection.Open();
            return new DbDataReaderWrapper(comm.ExecuteReader(CommandBehavior.CloseConnection));
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled array of ListItems with the given CommandText and returns the first column of that table as the listItem value and the second column as the ListItem text.   Uses the implemented connection.
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
        /// Uses a MySQLDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.   Uses the implemented connection.
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
        /// Uses a MySQLDataAdpter to create a filled array of ListItems with the given CommandText and returns a DropDownList.   Uses the implemented connection.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public System.Web.UI.WebControls.DropDownList FilledDropDownWeb(string id ,string commandText)
        {
            var ddl = FilledDropDownWeb(commandText);
            ddl.ID = id;
            return ddl;
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText and returns the first column of that table as an object array.  Uses the implemented connection.
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
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText and returns the first column of that table as an object array.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] FilledColumn(string CommandText, params MySqlParameter[] myparams)
        {
            DataTable dt = FilledTable(CommandText, myparams);
            List<string> o = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                o.Add(dr[0].ToString());
            }

            return o.ToArray();

        }


        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ScalarValue(string CommandText)
        {
            string svalue = null;
            MySqlCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            svalue = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return svalue;
        }

        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ScalarValue(string CommandText, params MySqlParameter[] @params)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = GetConnection;
            comm.CommandText = CommandText;
            foreach (MySqlParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            string output = comm.ExecuteScalar().ToString();
            comm.Connection.Close();
            return output;
        }

        /// <summary>
        /// Sets a single scalar value in the mySQL database with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <remarks></remarks>
        public void SetValue(string CommandText)
        {
            MySqlCommand comm = GetCommand(CommandText);
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Creates a MySQLDataAdpter and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter()
        {
            return new MySqlDataAdapter(GetCommand());
        }
        /// <summary>
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(string CommandText, params MySqlParameter[] @params)
        {
            return new MySqlDataAdapter(GetCommand(CommandText, @params));
        }

        /// <summary>
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(string CommandText, bool UseCommandBuilder, params MySqlParameter[] @params)
        {
            return DataAdapter(GetCommand(CommandText, @params),UseCommandBuilder);
        }

        /// <summary>
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(string CommandText)
        {
            return new MySqlDataAdapter(GetCommand(CommandText));
        }

        /// <summary>
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(string CommandText, bool UseCommandBuilder)
        {
            return DataAdapter(GetCommand(CommandText), UseCommandBuilder);
        }

        /// <summary>
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(MySqlCommand Command, bool UseCommandBuilder)
        {
            MySqlDataAdapter DA = new MySqlDataAdapter(Command);
            MySqlCommandBuilder cb = default(MySqlCommandBuilder);
            if (UseCommandBuilder)
            {
                cb = new MySqlCommandBuilder(DA);
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
        /// Creates a MySQLDataAdpter with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlDataAdapter DataAdapter(string SELECT, string INSERT, string UPDATE, string DELETE)
        {
            MySqlDataAdapter DA = new MySqlDataAdapter(GetCommand(SELECT));
            MySqlCommand Ins = new MySqlCommand(INSERT, DA.SelectCommand.Connection);
            MySqlCommand Upd = new MySqlCommand(UPDATE, DA.SelectCommand.Connection);
            MySqlCommand Del = new MySqlCommand(DELETE, DA.SelectCommand.Connection);
            DA.InsertCommand = Ins;
            DA.UpdateCommand = Upd;
            DA.DeleteCommand = Del;
            return DA;
        }

        ///' <summary>
        ///' Creates a MySQLDataAdpter with the given CommandText and CommandBuilder and associates it with the implemented connection.
        ///' </summary>
        ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
        ///' <param name="PKEY_SELECTforinsert">Used to retrieve row data after insert occurs: SELECT * FROM `Table` WHERE id=LAST_INSERT_ID();</param>
        ///' <returns></returns>
        ///' <remarks></remarks>
        //Public Overloads Function DataAdapter(ByVal SELECTCommandText As String, ByVal PKEY_SELECTforinsert As String) As MySqlDataAdapter
        //    Dim da As MySqlDataAdapter = DataAdapter(SELECTCommandText, True)
        //    'If UseCommandBuilder Then
        //    'Get the PKEY of the new row
        //    da.InsertCommand.CommandText = da.UpdateCommand.CommandText & PKEY_SELECTforinsert
        //    da.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
        //    da.MissingMappingAction = MissingMappingAction.Passthrough
        //    'End If

        //    Return da
        //End Function

        ///' <summary>
        ///' Creates a MySQLDataAdpter with the given CommandText and CommandBuilder and associates it with the implemented connection.
        ///' </summary>
        ///' <param name="SELECTCommandText">SeLECT statement for use with FILL(): SELECT * FROM `Table` WHERE...</param>
        ///' <param name="Tablename">Used to build command to retrieve rowdata after insert occurs</param>
        ///' <param name="PKEYColumnName">Used to build command to retrieve rowdata after insert occurs</param>
        ///' <returns></returns>
        ///' <remarks></remarks>
        //Public Overloads Function DataAdapter(ByVal SELECTCommandText As String, ByVal Tablename As String, ByVal PKEYColumnName As String) As MySqlDataAdapter
        //    Return DataAdapter(SELECTCommandText, "SELECT * FROM " & Tablename & " WHERE " & PKEYColumnName & "=LAST_INSERT_ID();")
        //End Function

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable FilledTable(string CommandText, params MySqlParameter[] @params)
        {
            MySqlDataAdapter DA = DataAdapter(CommandText, @params);
            DataTable dt = new DataTable();
            DA.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable FilledTable(string CommandText)
        {
            MySqlDataAdapter DA = DataAdapter(CommandText);
            DataTable dt = new DataTable();
            try
            {
                DA.Fill(dt);
            }
            catch (MySqlException ex)
            {
                throw new Exception("CB2.MySQL.FilledTable() failed with command: " + CommandText,ex);
            }
            return dt;
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable SchemaFilledTable(string CommandText)
        {
            MySqlDataAdapter DA = DataAdapter(CommandText);
            DataTable dt = new DataTable();
            DA.FillSchema(dt, SchemaType.Source);
            return dt;
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the implemented connection.
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
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataRow FilledRow(string CommandText, params MySqlParameter[] myparams)
        {
            DataTable dt = FilledTable(CommandText, myparams);
            DataRow dr = dt.Rows[0];
            return dr;
        }

        /// <summary>
        /// Uses a MySQLDataAdpter to create a filled table with the given CommandText and returns the first row of that table.  Uses the implemented connection.
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataRow SchemaFilledRow(string CommandText)
        {
            DataTable dt = SchemaFilledTable(CommandText);
            DataRow dr = dt.Rows[0];
            return dr;
        }

        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlCommand ExecuteNonQuery(string CommandText)
        {

            MySqlCommand comm = GetCommand(CommandText);
            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("MySQLCommand failed: '" + CommandText + "' see innerexception for details", ex);
            }
            return comm;
        }
        
        /// <summary>
        /// Creates a Command with the given CommandText and associates it with the implemented connection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MySqlCommand ExecuteNonQuery(string CommandText, params MySqlParameter[] @params)
        {
            MySqlCommand comm = GetCommand(CommandText);
            foreach (MySqlParameter param in @params)
            {
                comm.Parameters.Add(param);
            }
            comm.Connection.Open();
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            return comm;
        }

        /// <summary>
        /// Tests the MySQLconnection with a select * FROM TABLE command
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDbConnectionValid(string CommandText)
        {
            MySqlDataAdapter DA = DataAdapter(CommandText);
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

        /// <summary>
        /// Uploads a CSV file to MySQL.  Skips the first line (assumes it is a header line).  And assumes fields are separated by commas and enclosed in "quotes"
        /// </summary>
        /// <param name="schema">ex: labmodule</param>
        /// <param name="table">ex: orderstable</param>
        /// <param name="CSVFileName">ex: myname.csv</param>
        public void SendCSVFileToDatabase(string schema, string table, string CSVFileName) {
            string FullTableName = schema + "."+ table;

             ExecuteNonQuery(" LOAD DATA LOCAL INFILE '" + CSVFileName +
               "' INTO TABLE " + FullTableName + " FIELDS TERMINATED BY ',' ENCLOSED BY '\"' LINES TERMINATED BY '\r\n' ignore 1 lines;");
        }

        #region IDatabase Members

        DbConnection IDatabase.GetConnection()
        {
            return this.GetConnection;
        }

        DbCommand IDatabase.GetCommand()
        {
            return this.GetCommand();
        }

        DbCommand IDatabase.GetCommand(string CommandText)
        {
            return this.GetCommand(CommandText);
        }

        DbCommand IDatabase.GetCommand(string CommandText, params DbParameter[] myparams)
        {
            return this.GetCommand(CommandText,(MySqlParameter[]) myparams);
        }

        string[] IDatabase.FilledColumn(string CommandText, params DbParameter[] myparams)
        {
            return this.FilledColumn(CommandText, (MySqlParameter[])myparams);
        }

        string IDatabase.ScalarValue(string CommandText, params DbParameter[] @params)
        {
            return this.ScalarValue(CommandText, (MySqlParameter[])@params);
        }

        DbDataAdapter IDatabase.DataAdapter()
        {
            return this.DataAdapter();
        }

        DbDataAdapter IDatabase.DataAdapter(string CommandText, params DbParameter[] @params)
        {
            return this.DataAdapter(CommandText, (MySqlParameter[])@params);
        }

        DbDataAdapter IDatabase.DataAdapter(string CommandText, bool UseCommandBuilder, params DbParameter[] @params)
        {
            return this.DataAdapter(CommandText, UseCommandBuilder,(MySqlParameter[])@params);
        }

        DbDataAdapter IDatabase.DataAdapter(string CommandText)
        {
            return this.DataAdapter(CommandText);
        }

        DbDataAdapter IDatabase.DataAdapter(string CommandText, bool UseCommandBuilder)
        {
            return this.DataAdapter(CommandText, UseCommandBuilder);
        }

        DbDataAdapter IDatabase.DataAdapter(DbCommand Command, bool UseCommandBuilder)
        {
            return this.DataAdapter((MySqlCommand)Command, UseCommandBuilder);
        }

        DbDataAdapter IDatabase.DataAdapter(string SELECT, string INSERT, string UPDATE, string DELETE)
        {
            return this.DataAdapter(SELECT,INSERT,UPDATE,DELETE);
        }

        DataTable IDatabase.FilledTable(string CommandText, params DbParameter[] @params)
        {
            return this.FilledTable(CommandText, (MySqlParameter[])@params);
        }

      

        DataRow IDatabase.FilledRow(string CommandText, params DbParameter[] myparams)
        {
            return this.FilledRow(CommandText, (MySqlParameter[])myparams);
        }

      

        DbCommand IDatabase.ExecuteNonQuery(string CommandText)
        {
            return this.ExecuteNonQuery(CommandText);
        }

        DbCommand IDatabase.ExecuteNonQuery(string CommandText, params DbParameter[] @params)
        {
            return this.ExecuteNonQuery(CommandText, (MySqlParameter[])@params);
        }

        #endregion

        #region IDatabase Members

        public DatabaseName DatabaseName
        {
            get { return Databases.DatabaseName.MySql; }
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

        System.Web.UI.WebControls.ListItem[] IDatabase.FilledListItemsWeb(string commandText)
        {
            throw new NotImplementedException();
        }

        System.Web.UI.WebControls.DropDownList IDatabase.FilledDropDownWeb(string commandText)
        {
            throw new NotImplementedException();
        }

        System.Web.UI.WebControls.DropDownList IDatabase.FilledDropDownWeb(string id, string commandText)
        {
            throw new NotImplementedException();
        }

        string[] IDatabase.FilledColumn(string CommandText)
        {
            throw new NotImplementedException();
        }

        string IDatabase.ScalarValue(string CommandText)
        {
            throw new NotImplementedException();
        }

        void IDatabase.SetValue(string CommandText)
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
}