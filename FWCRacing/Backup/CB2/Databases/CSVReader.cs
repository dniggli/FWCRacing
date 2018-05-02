using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.Common;
using CodeBase2.Databases;

namespace CodeBase2
{
   public static class CSVReader
    {
        public static System.Data.DataTable GetDataTable(string strFileName)
        {
            return QueryCSV(strFileName,"","");
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="strFileName">filename.csv</param>
       /// <param name="column">idColumn</param>
       /// <param name="where">idColumn=22</param>
       /// <returns></returns>
        public static string Scalar(string strFileName, string column, string where)
        {
            return QueryCSV(strFileName, where, column).Rows[0][0].ToString();
        }
       private static System.Data.DataTable QueryCSV(string strFileName, string where, string column)
        {
            strFileName.Replace("\\","/");
            if (!strFileName.Contains("/")) strFileName = "./" + strFileName;


            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                //  System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
                conn.Open();

                string strQuery;
                if (column == "") column = "*";
                if (where == "") strQuery = "SELECT " + column + " FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
                else strQuery = "SELECT " + column + " FROM [" + System.IO.Path.GetFileName(strFileName) + "] WHERE " + where;

                System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
                
                adapter.Fill(dt);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

       /// <summary>
       /// creates a CSV file
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static void CSVFileBuilder(DataTable dt, string filename)
       {
           StreamWriter sw = new StreamWriter(filename);
           //label the columns
           string columns = "";
           foreach (DataColumn c in dt.Columns) {
               columns += "\"" + c.ColumnName + "\",";
           }
           sw.WriteLine(columns.TrimEnd(','));
           foreach (DataRow dr in dt.Rows)
           {
               sw.WriteLine(CSVLineBuilder(dr));
           }
           sw.Flush();
           sw.Close();
       }

       /// <summary>
       /// creates a line (row) of a csv file from a DataRow
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static string CSVLineBuilder(DataRow dr)
       {          
               string row = "";
               foreach (DataColumn c in dr.Table.Columns)
               {
                   row += "\"" + dr[c.ColumnName] + "\",";
               }
               row = row.TrimEnd(',');
               return row;

       }

       /// <summary>
       /// creates a CSV file
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static void ToCSV(this DataTable dt, string filename)
       {
           CSVFileBuilder(dt, filename);
       }

       /// <summary>
       /// creates a string appropriate for a row in a CSV file
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static string ToCSVLine(this DataRow dr)
       {
           return CSVLineBuilder(dr);
       }

       /// <summary>
       /// creates a string appropriate for a row in a CSV file from the current row being read
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static string ToCSVLine(this DbDataReader dataReader)
       {
           object[] values = new object[dataReader.FieldCount];
           dataReader.GetValues(values);

           return values.ToCSVLine();
       }

       /// <summary>
       /// creates a string appropriate for a row in a CSV file from the current row being read
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static string ToCSVLine(this DataReader dataReader)
       {
           object[] values =dataReader.GetValues();
           return values.ToCSVLine();
       }

       /// <summary>
       /// creates a string appropriate for a row in a CSV file from the current row being read
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="filename"></param>
       public static string ToCSVLine(this object[] values)
       {
          //TODO: ASSERT that this works (steal the test from SoftExport)
           string row = "";

           foreach (object val in values)
           {
               if (val == null)
               {
                   row += "\"\\N\",";
               }
               else
               {
                   string k;
                   k = val.ToString().Replace(@"""",@"""""")
                       .Replace("\\N", "!#!#N$%$%")
                       .Replace("\\", "\\\\")
                       .Replace("!#!#N$%$%", "\\N");

                   row += "\"" + k + "\",";
               }
           }
           row = row.TrimEnd(',');
           return row;
       }
    }
}
