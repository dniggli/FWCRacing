using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MyPara = MySql.Data.MySqlClient.MySqlParameter;
using MySql.Data.MySqlClient;



//replaced by the entityframework class
namespace CodeBase2.PathDirectory.URMC
{
//    public abstract class MySQLDevice
//    {

//        protected DataTable dt;
//        protected MySqlDataAdapter DA;

//        public abstract Device LookupDevice();
//        public abstract void SaveDevice();
//        public abstract string AccountName { get; set; }

//        public string DeviceType
//        {
//            get { return dt.Rows[0]["Type"].ToString().ToLower(); }
//            set { dt.Rows[0]["Type"] = value.ToLower(); }
//        }
//    }


//    public class MySQLPrinter : MySQLDevice
//    {
//        #region MySQLDevice Members


//        //Query mysql (either PC or printer\labeler), if the device exists get it. 
//        //If it dont and it is a PC then query active directory.
//        public Device LookupDevice(string SoftName)
//        {
//            using (var dbContext = new EntitiesPathDirectory())
//            {
//                var printer = (from a in dbContext.DeviceSet
//                              where a.name == SoftName
//                              select a).FirstOrDefault();
//                return printer;
//            }            
//        }

//        public Printer LookupPrinter(string SoftName)
//        {
//            using (var dbContext = new EntitiesPathDirectory())
//            {
//                var printer = (from a in dbContext.DeviceSet.OfType<Printer>()
//                               where a.name == SoftName
//                               select a).FirstOrDefault();
//                return printer;
//            }
//        }

//        public override void SaveDevice()
//        {
//            throw new NotImplementedException();
//        }

//        public override string AccountName
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        #endregion
//    }
//    public class MySQLComputer : MySQLDevice
//    {

//        public MySQLComputer(string AccountName, string Type)
//        {
//            LoadDevice(AccountName);
//        }

//        public override void SaveDevice()
//        {
//            MySqlConnection c = DA.SelectCommand.Connection;

//            c.Open();
//            try
//            {
//                DA.Update(dt);
//            }
//            catch (Exception ex)
//            {
//                throw ex;

//            }
//            finally
//            {
//                c.Close();
//            }
//            LoadDevice(AccountName);
//        }



//        private void LoadDevice(string AccountName)
//        {
//            dt = new DataTable();
//            DA = new GetMysql().DataAdapter(
//                "SELECT * FROM `device` D Where `AccountName`=?AccountName;", true, new MyPara("?AccountName", AccountName));

//            DA.Fill(dt);
//#if DEBUG
//            if (dt.Rows.Count == 0) Console.Error.WriteLine("Device not in CodeBase:" + dt.Rows.Count);
//#endif
//            if (dt.Rows.Count == 0) dt.Rows.Add(dt.NewRow());
//        }

//        public override string AccountName
//        {
//            get { return dt.Rows[0]["AccountName"].ToString().ToLower(); }
//            set { dt.Rows[0]["AccountName"] = value.ToLower(); }
//        }


      

//        //public override MySQLDevice LookupDevice()
//        //{
//        //    throw new NotImplementedException();
//        //}

//    }
}
