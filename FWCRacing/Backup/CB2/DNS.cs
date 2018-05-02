using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CodeBase2
{
   public static class DNS
    {
       /// <summary>
       /// retrieve the IP address using the DNS name
       /// </summary>
       /// <param name="DNS_Name"></param>
       /// <returns></returns>
       public static string NameToIPString(String DNS_Name)
       {
           IPAddress[] addresslist;
           try
           {
               addresslist = Dns.GetHostAddresses(DNS_Name);
           }
           catch (SocketException)
           {
               return "0.0.0.0";
           }

           addresslist = addresslist.Where(ip => !ip.IsIPv6LinkLocal).ToArray();
           if (addresslist.Length > 1) throw new Exception("Device has more than one IP address!");
           IPAddress ipOut = addresslist.First();
           
           return ipOut.ToString();
       }

       /// <summary>
       /// Converts a Dictionary of "Key:String, Value:IP Or DNS Name" to "Key:String, Value:IP Address"
       /// </summary>
       /// <param name="dict"></param>
       /// <returns></returns>
       public static Dictionary<string, string> NameDictionaryToIPDictionary(Dictionary<string, string> dict)
       {
            for (int x = 0; x < dict.Count;x++ )
            {
                string key = dict.Keys.ElementAt(x);
                if (!dict[key].Contains("."))
                    dict[key] = CodeBase2.DNS.NameToIPString(dict[key]);
            }
            return dict;
       }

       /// <summary>
       /// retrieve the IP address using the DNS name
       /// </summary>
       /// <param name="DNS_Name"></param>
       /// <returns></returns>
       public static string IPToName(String IP, bool giveIP)
       {
           try
           {
               IPHostEntry entry = Dns.GetHostEntry(IP);

               
               if (giveIP)
               {
                   //if it is a hostname chop off the rest of the name, but if it is an IP address send the whole IP address
                   return (entry.HostName.Contains(".edu")) ? entry.HostName.Split('.')[0] : entry.HostName;
               }
               else
               {
                   //if it is a hostname chop off the rest of the name, but if it is an IP address dont return the IP address
                   return (entry.HostName.Contains(".edu")) ? entry.HostName.Split('.')[0] : "";
               }
           }
           catch
           {
               return "";
           }
       }

       /// <summary>
       /// retrieve the IP address using the DNS name
       /// </summary>
       /// <param name="DNS_Name"></param>
       /// <returns></returns>
       public static string IPToFullName(String IP, Boolean giveIP)
       {
           try
           {
               IPHostEntry entry = Dns.GetHostEntry(IP);
               if (giveIP)
               {
                   return entry.HostName;
               }
               else
               {
                   //if it is a hostname return the hostname, but if it is an IP address dont return the IP address
                   return (entry.HostName.Contains(".edu")) ? entry.HostName : "";
               }
           }
           catch
           {
               return "";
           }
       }
    }
}
