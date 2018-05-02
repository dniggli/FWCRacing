using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace CodeBase2.PathDirectory.URMC
{
   public class ADObject
    {
       DirectoryEntry DirEnt;
       public ADObject(DirectoryEntry ent)
       {
           DirEnt = ent;
           //Set the groups field

       }

       public ADObject()
       {
       }

       public string[] MemberOf
       {
           get {
               return GetGroups(DirEnt.Properties["memberOf"]);
           }
       }

       public string MemberOfString
       {
           get
           {
               if (MemberOf.Length == 0) return "";
               if (MemberOf.Length == 1) return MemberOf[0];
                return MemberOf.Aggregate((o,group)=>group + " " + o);
           }
       }

       private static string[] GetGroups(PropertyValueCollection memberOf)
       {
           List<string> groupNames = new List<string>();


           int propertyCount = memberOf.Count;
           string dn;
           int equalsIndex, commaIndex;

           for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
           {
               dn = (string)memberOf[propertyCounter];
               equalsIndex = dn.IndexOf("=", 1);
               commaIndex = dn.IndexOf(",", 1);
               if (-1 == equalsIndex)
               {
                   return null;
               }
               groupNames.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));

           }

           return groupNames.ToArray();
       }


       public DirectoryEntry Entry { get { return DirEnt; } }
       
       /// <summary>
       /// retrieve property as string from user DirectoryEntry
       /// </summary>
       /// <param name="propname"></param>
       /// <returns></returns>
       private string prop(string propname)
       {
           string myprop = "";
           try
           {
               myprop = DirEnt.Properties[propname].Value.ToString();
           }
           catch (NullReferenceException)
           {
#if DEBUG
               Console.Error.WriteLine("Could not find AD user property: " + propname + " for " + this.sAMAccountName);
#endif
               myprop = "";
           }


           return myprop;
       }

       /// <summary>
       /// Account Name
       /// </summary>
       public string sAMAccountName
       {
           get
           {
               if (DirEnt == null) return "nondomain";
               return prop("sAMAccountName").Trim().TrimEnd(new char[] {'$'});
           }
       }

       public string OU
       {
           get
           {
               if (DirEnt == null) return "";
               string OU = DistinguishedName.Replace(",DC=rochester,DC=edu", "");
               string[] OUsplit = OU.Replace("\\,", "~").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

               List<string> OUs = new List<string>();
               foreach (string s in OUsplit)
               {
                   OUs.Add(s.Replace("~", ",").Replace("CN=", "").Replace("OU=", "").Replace("DC=", ""));
               }

               string output = "";
               OUs.Reverse();
               foreach (string s in OUs)
               {
                   output += "/" + s;
               }

               return output;
           }
       }

       public string DistinguishedName
       {
           get
           {
               if (DirEnt == null) return "nondomain";
               return prop("distinguishedName").Trim();
           }
       }

       public string Description
       {
           get
           {
               if (DirEnt == null) return "Generic NonActiveDirectory User For Vendors";
               return prop("description").Trim();
           }
       }

       public string PhysicalDeliveryOfficeName
       {
           get
           {
               if (DirEnt == null) return "";
               return prop("physicalDeliveryOfficeName").Trim();
           }
       }



    }
}
