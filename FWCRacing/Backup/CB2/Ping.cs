using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace CodeBase2
{
    public static class Ping
    {

        public const string URMCDomain = ".urmc-sh.rochester.edu";

        /// <summary>
        /// Used to Ping a device on the network
        /// </summary>
        /// <returns>A boolean value, True if the device was successfully pinged</returns>
        public static bool PingIt(string NameOrAddress)
        {
            if (NameOrAddress.Contains('.')) return PingIt(NameOrAddress, "");
            return PingIt(NameOrAddress, URMCDomain);
        }

        /// <summary>
        /// Used to Ping a device on the network
        /// </summary>
        /// <returns>A boolean value, True if the device was successfully pinged</returns>
        public static bool PingIt(string Name, string Domain)
        {
            bool siteresponds = false;
            var p = new System.Net.NetworkInformation.Ping();

            try
            {
                siteresponds = (p.Send(Name + Domain, 2700).Status == IPStatus.Success);
                Console.WriteLine(Name);
            }
            catch (System.Net.NetworkInformation.PingException ex)
            {
                var inner = ex.InnerException;
                Console.WriteLine(Name + ": " + inner.Message);
                return siteresponds;
            }
            return siteresponds;
        }

        /// <summary>
        /// Used to Ping a device on the network
        /// </summary>
        /// <returns>A boolean value, True if the device was successfully pinged</returns>
        public static IPStatus PingStatus(string NameOrAddress)
        {
            if (NameOrAddress.Contains('.')) return PingStatus(NameOrAddress, "");
            return PingStatus(NameOrAddress, URMCDomain);
        }

        /// <summary>
        /// Used to Ping a device on the network
        /// </summary>
        /// <returns>A IPStatus value, True if the device was successfully pinged</returns>
        public static IPStatus PingStatus(string Name, string Domain)
        {
            
            var p = new System.Net.NetworkInformation.Ping();      
                return p.Send(Name + Domain, 2700).Status;
        }

    }
}
