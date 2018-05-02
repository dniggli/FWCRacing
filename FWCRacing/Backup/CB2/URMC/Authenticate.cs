//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.DirectoryServices;
//using System.Diagnostics;
//using System.Data;
//using NUnit.Framework;

//namespace CodeBase2.PathDirectory.URMC
//{
//    public class Authenticate
//    {
//        private static void auth(string UserName, string Password, string LDAPpath)
//        {
//            //DirectoryEntry Class can be used to authenticate a username and password against active directory. You can force authentication to occur by retrieving the nativeObject property.
//            string user = "URMC-SH" + "\\" + UserName;
//            DirectoryEntry entry = new DirectoryEntry(LDAPpath, user, Password);

//            try
//            {
//                // Bind to the native object to force authentication to happen
//                object objnative = entry.NativeObject;
//                // "User authenticated" Move to the next page.
//                Console.WriteLine("Authenticated");

//            }
//            catch (Exception ex)
//            {
//                throw new UnauthorizedAccessException("User not authenticated: " + UserName,ex);
//            }
//        }

//        const string LDAPpath = "LDAP://URMC-SH.ROCHESTER.EDU";

//        /*
//        [WebMethod]
//        public static bool Authenticate(string username, string password)
//        {
//            if (username == "" || password == "") return false;

//            return CodeBase2.URMC.Authenticate.Authentication(username, password);
//        }
//        */

//        public static bool Authentication(string UserName, string Password)
//        {
//            //If authentication fails it assumes it cannot locate the domain, and tries again after specifying a domain controller


//            bool r;

//            try
//            {
//                auth(UserName, Password, LDAPpath);
//                r = true;
//            }
//            catch (UnauthorizedAccessException)
//            {
//                try
//                {
//                    string LDpath = "LDAP://urmcshdc01.urmc-sh.rochester.edu/DC=URMC-SH,DC=ROCHESTER,DC=EDU";
//                    auth(UserName, Password, LDpath);
//                    r = true;
//                }
//                catch (UnauthorizedAccessException)
//                {
//                    r = false;
//                }

//            }


//            return r;


//        }
       

//    }
//    [TestFixture]
//    public class TestAuth
//    {
//        [Test]
//        public void TestAuthsasd()
//        {
//            Authenticate.Authentication("cvanvranken", "");
//        }
//    }
//}
