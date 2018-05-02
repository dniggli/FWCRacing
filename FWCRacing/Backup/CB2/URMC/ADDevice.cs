using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using CodeBase2.URMC;

namespace CodeBase2.PathDirectory.URMC
{
    public class ADDevice : ADObject
    {
        public ADDevice(DirectoryEntry ent) : base(ent) { }

        /// <summary>
        /// search for PCs supports * as wildcard
        /// </summary>
        /// <param name="PCName"></param>
        /// <param name="Limit"></param>
        /// <param name="OnlyPathology"></param>
        /// <returns></returns>
        private static SearchResultCollection LookupComputerEntries(string PCName, int Limit, bool OnlyPathology)
        {
            Console.WriteLine(PCName);
            string[] properties = new string[] {
            "cn","memberOf"};        //PC Name

            AD.Query dsSearch = new AD.Query("(&(cn=" + PCName + ")(objectClass=computer)(!objectClass=nTFRSMember))", properties, "cn", OnlyPathology);
            dsSearch.SizeLimit = Limit;

            return dsSearch.FindAll();
        }

        


        /// <summary>
        /// search for PCs supports * as wildcard
        /// </summary>
        /// <param name="PCName"></param>
        /// <param name="Limit"></param>
        /// <param name="OnlyPathology"></param>
        /// <returns></returns>
        public static string[] LookupComputers(string PCName, int Limit, bool OnlyPathology)
        {

            SearchResultCollection result = LookupComputerEntries(PCName, Limit, OnlyPathology);          

            string[] pcs = (from SearchResult a in result
                              select a.GetDirectoryEntry().Properties["cn"].Value.ToString()).ToArray();

            return pcs;
        }

        /// <summary>
        /// search for PCs supports * as wildcard
        /// </summary>
        /// <param name="PCName">ex: LIS-S22104*</param>
        /// <param name="Limit">number of devices to return</param>
        /// <returns></returns>
        public static ADDevice[] LookupComputerObjects(string PCName, int Limit)
        {

            SearchResultCollection result = LookupComputerEntries(PCName, Limit, false);

            ADDevice[] pcs = (from SearchResult a in result
                              select new ADDevice(a.GetDirectoryEntry())).ToArray();

            return pcs;
        }

        /// <summary>
        /// search for PCs supports * as wildcard, returns the first device found
        /// </summary>
        /// <param name="PCName">ex: LIS-S22104*</param>
        /// <returns></returns>
        public static ADDevice LookupComputerObject(string PCName)
        {

            SearchResultCollection result = LookupComputerEntries(PCName, 1, false);

            ADDevice pc = (from SearchResult a in result
                              select new ADDevice(a.GetDirectoryEntry())).First();

            return pc;
        }

        //public static ADUser[] LookupUsers(string LastName, string FirstName, int Limit, bool onlyPathology)
        //{

        //    SearchResultCollection result = LookupUserEntries(LastName, FirstName, Limit, onlyPathology);

        //    ADUser[] users = (from SearchResult a in result
        //                      select new ADDevice(a.GetDirectoryEntry())).ToArray<ADUser>();

        //    return users;
        //}

        //private static SearchResultCollection LookupUserEntries(string LastName, string FirstName, int Limit, bool OnlyPathology)
        //{
        //    string[] properties = new string[] {
        //    "sAMAccountName",   //User's login name
        //    "cn",               //User's common name  (LastName, FirstName)
        //    "department",       //User's Department
        //    "initials",         //User's middle initial
        //    "memberOf"};        //User's AD Groups

        //    AD.Query dsSearch = new AD.Query("(&(sn=" + LastName + "*)(givenName=" + FirstName + "*)(objectClass=user)(!objectClass=nTFRSMember))", properties, "sn", OnlyPathology);
        //    dsSearch.SizeLimit = Limit;

        //    return dsSearch.FindAll();
        //}
    }
}
