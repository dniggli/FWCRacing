using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Win = Microsoft.Win32;
using Microsoft.Win32;

namespace CodeBase2
{
  

    
public class RegistryReadWrite
{
   

    public bool WriteToRegistry(Win.RegistryHive ParentKeyHive, string SubKeyName, string ValueName, object Value)
    {
        
        //DEMO USAGE
        //Dim bAns As Boolean
        //bAns = WriteToRegistry(RegistryHive.LocalMachine, "SOFTWARE\MyCompany\MyProgram\", "ProgramHasRunBefore", "Y")
        //Debug.WriteLine("Registry Write Successful: " & bAns)

        Win.RegistryKey objSubKey;

        Win.RegistryKey objParentKey;

        objParentKey = RegistryKey.OpenBaseKey(ParentKeyHive, RegistryView.Default);

            //Open 
            objSubKey = objParentKey.OpenSubKey(SubKeyName, true);
            //create if doesn't exist
            if (objSubKey == null) {
                objSubKey = objParentKey.CreateSubKey(SubKeyName);
            }
            
            objSubKey.SetValue(ValueName, Value);
            
        return true;
    }
    
    public string ReadRegistryValue(Win.RegistryHive Hive, string Key, string ValueName, string DefaultValue)
    {
        
        //DEMO USAGE

        //Dim sAns As String
        //Dim sErr As String = ""
        
        //sAns = RegValue(RegistryHive.LocalMachine, _
        //  "SOFTWARE\Microsoft\Windows\CurrentVersion", _
        //  "ProgramFilesDir", sErr)
        //If sAns <> "" Then
        //    Debug.WriteLine("Value = " & sAns)
        //Else
        //    Debug.WriteLine("This error occurred: " & sErr)
        
        //End If

        Win.RegistryKey objParent;
        Win.RegistryKey objSubkey;
        string sAns = DefaultValue;
        
        objParent = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
        
      
            objSubkey = objParent.OpenSubKey(Key);
            //if can't be found, object is not initialized
            if ((objSubkey != null))
            {

                sAns = (objSubkey.GetValue(ValueName)).ToString();

            }          
        return sAns;
    }

    public string ReadRegistryValue(Win.RegistryHive Hive, string Key, string ValueName)
    {
        return ReadRegistryValue(Hive, Key, ValueName, "");
    }
}


    public abstract class RegistryBase 
    {
        RegistryReadWrite registryReadWrite = new RegistryReadWrite();

        /// <summary>
        /// ex:  return "SOFTWARE\\MyCompany\\MyProgram\\";
        /// </summary>
        public abstract string SubKey
        {
            get;
        }
        /// <summary>
        /// ex: return Microsoft.Win32.RegistryHive.CurrentUser;
        /// </summary>
        public abstract Win.RegistryHive Hive
        {
            get;
        }

        protected void write(string keyname, string keyvalue)
        {
            registryReadWrite.WriteToRegistry(Hive, SubKey, keyname, keyvalue);
        }
        protected string read(string keyname,string DefaultValue)
        {
            return registryReadWrite.ReadRegistryValue(Hive, SubKey, keyname, DefaultValue);
        }

        protected int readInt(string keyname, int DefaultValue)
        {
            return Convert.ToInt32(registryReadWrite.ReadRegistryValue(Hive, SubKey, keyname, DefaultValue.ToString()));
        }

        protected string read(string keyname)
        {
            return registryReadWrite.ReadRegistryValue(Hive, SubKey, keyname);
        }

        protected int readInt(string keyname)
        {
            return Convert.ToInt32(registryReadWrite.ReadRegistryValue(Hive, SubKey, keyname));
        }
        
        //public static bool CaseNumber_SourceData
        //{
        //    get
        //    {
        //        return bool.Parse(reg_check("CaseNumber_SourceData", true.ToString()));
        //    }
        //    set
        //    {
        //        reg_modify("CaseNumber_SourceData", value.ToString());
        //    }
        //}

  
        //public static string CommonTemplatePath
        //{
        //    get
        //    {
        //        return reg_check("CommonTemplatePath", "");
        //    }
        //    set
        //    {
        //        reg_modify("CommonTemplatePath", value);
        //    }
        //}



        //static string caseDirectory;
        //public static string CaseDirectory
        //{
        //    get
        //    {
        //        if (caseDirectory == null)
        //            caseDirectory = reg_check("CaseDirectory", Env.GetFolderPath(Env.SpecialFolder.MyDocuments) + "\\PathFinder2 Data Storage\\Case\\");

        //        return caseDirectory;
        //    }
        //    set
        //    {
        //        reg_modify("CaseDirectory", value);
        //    }
        //}


        //public static class Database : RegistryBase
        //{
        //    const string db = root + "\\Database";

        //public static string Root {
    //    get {
        //return "rootstring";
    //}
    //    }

        //    public static string dsnPath
        //    {
        //        get
        //        {
        //            return reg_check("TempData_dsnPath", "");
        //        }
        //        set
        //        {
        //            reg_modify("TempData_dsnPath", value);
        //        }
        //    }

        //}
    }

    //public static class RegistryReadWrite
    //{
    //    /// <summary>
    //    /// Modifies the registry with a given string key. Creates the registry path if it does not exist.
    //    /// </summary>
    //    /// <param name="regpath">The location in the registry to save to.</param>
    //    /// <param name="keyname">The name of the registry key to set.</param>
    //    /// <param name="keyvalue">The value of the registry key to set. Strings only.</param>
    //    public static void registry_modify(string regrootpath, string keyname, string keyvalue)
    //    {
    //        Win.RegistryKey cu = Win.Registry.CurrentUser;

    //        Win.RegistryKey newReg = cu.OpenSubKey(regrootpath, true);
    //        if (newReg == null)
    //        {
    //            cu.CreateSubKey(regrootpath);
    //            newReg = cu.OpenSubKey(regrootpath, true);
    //        }
    //        newReg.SetValue(keyname, keyvalue, Win.RegistryValueKind.String);
    //        newReg.Close();
    //    }

    //    /// <summary>
    //    /// Returns the value of a registry key given its path and its name.
    //    /// </summary>
    //    /// <param name="regpath">The location in the registry that contains the key.</param>
    //    /// <param name="keyname">The name of the registry key to check.</param>
    //    public static string registry_check(string regrootpath, string keyname, string defaultvalue)
    //    {
    //        Win.RegistryKey cu = Win.Registry.CurrentUser;

    //        Win.RegistryKey newReg = cu.OpenSubKey(regrootpath, false);
    //        if (newReg == null)
    //        {
    //            return defaultvalue;
    //        }
    //        else if (newReg.GetValue(keyname) != null)
    //        {
    //            string value = newReg.GetValue(keyname).ToString();
    //            newReg.Close();
    //            return value;
    //        }
    //        return defaultvalue;
    //    }



    //}
}
           
