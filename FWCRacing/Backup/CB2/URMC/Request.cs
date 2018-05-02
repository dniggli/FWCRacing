using System.Web;
using System;
namespace CodeBase2.PathDirectory.URMC
{
    public static class Request
    {
        /// <summary>
        /// Retrieves PC information from remote user
        /// </summary>
        /// <remarks>Retrieves information through the HttpRequest object</remarks>
        public class PC
        {
            /// <summary>
            /// HostName of PC??
            /// </summary>
            private System.Net.IPHostEntry objREMOTE_HOST;
            /// <summary>
            /// IP Address of PC??
            /// </summary>
            private System.Net.IPHostEntry objREMOTE_ADDR;
            /// <summary>
            /// Name of PC
            /// </summary>
            /// Public REMOTE_IDENT As System.Net.IPHostEntry
            /// Public REMOTE_USER As System.Net.IPHostEntry
            private string strComputerName;
            //LIS-ROOMNAME-####
            public PC()
            {
                objREMOTE_ADDR = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                objREMOTE_HOST = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
                // REMOTE_IDENT = System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_IDENT"))
                //   REMOTE_USER = System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_USER"))
                //LIS-ROOMNAME-####
                strComputerName = objREMOTE_HOST.HostName.Split('.')[0];
            }

            public string ComputerName
            {
                get { return strComputerName; }
            }

            public string RemoteAddress
            {
                get { return objREMOTE_ADDR.AddressList.ToString(); }
            }

            public string RemoteHost
            {
                get { return objREMOTE_HOST.HostName; }
            }

        }


        /// <summary>
        /// Determine if requester is inside or outside the intranet
        /// </summary>
        public static bool IsInside
        {
            get
            {
                string MyIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                string[] MySeg = MyIP.Split('.');

                bool location;

                if ((MySeg[0] == "172"))
                {
                    if (((Convert.ToInt32(MySeg[1]) >= 16) && (Convert.ToInt32(MySeg[1]) <= 31)))
                    {
                        location = true;
                    }
                    else
                    {
                        location = false;
                    }
                }
                else if ((MySeg[0] == "10"))
                {
                    if (((Convert.ToInt32(MySeg[1]) == 132) || (Convert.ToInt32(MySeg[1]) == 134)))
                    {
                        location = false;
                    }
                    else
                    {
                        location = true;
                    }
                }
                else if ((MySeg[0] == "128"))
                {
                    location = true;
                }
                else
                {
                    location = false;
                }

                return location;
            }
        }


    }
}
