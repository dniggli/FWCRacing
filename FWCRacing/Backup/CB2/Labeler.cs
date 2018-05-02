using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using CodeBase2.URMC;
using System.Data;

namespace CodeBase2.Labeler
{
    /// <summary>
    /// sends data to any printer that has been configured within control panel (used for USB labelers, when network down)
    /// </summary>
    public static class Send_Printer
    {



        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DOCINFOW
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter(string src, ref IntPtr hPrinter, int pd);
        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, ref DOCINFOW pDI);
        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, ref Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array of  
        // bytes, the function sends those bytes to the print queue.
        // Returns True on success or False on failure.
        private static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            IntPtr hPrinter = default(IntPtr);
            // The printer handle.
            Int32 dwError = default(Int32);
            // Last error - in case there was trouble.
            DOCINFOW di = new DOCINFOW();
            // Describes your document (name, port, data type).
            Int32 dwWritten = default(Int32);
            // The number of bytes written by WritePrinter().
            bool bSuccess = false;
            // Your success code.

            // Set up the DOCINFO structure.
            {
                di.pDocName = "My Visual Basic .NET RAW Document";
                di.pDataType = "RAW";
            }
            // Assume failure unless you specifically succeed.
            bSuccess = false;
            if (OpenPrinter(szPrinterName, ref hPrinter, 0))
            {
                if (StartDocPrinter(hPrinter, 1, ref di))
                {
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your printer-specific bytes to the printer.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, ref dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }
        // SendBytesToPrinter()

        // SendFileToPrinter()
        // When the function is given a file name and a printer name, 
        // the function reads the contents of the file and sends the
        // contents to the printer.
        // Presumes that the file contains printer-ready data.
        // Shows how to use the SendBytesToPrinter function.
        // Returns True on success or False on failure.
        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes large enough to hold the file's contents.
            byte[] bytes = new byte[fs.Length + 1];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = default(IntPtr);

            // Read the contents of the file into the array.
            bytes = br.ReadBytes((int)fs.Length);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem((int)fs.Length);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, (int)fs.Length);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, (int)fs.Length);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }
        // SendFileToPrinter()

        // When the function is given a string and a printer name,
        // the function sends the string to the printer as raw bytes.
        public static void SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes = default(IntPtr);
            Int32 dwCount = default(Int32);
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
        }
    }
    /// <summary>
    /// sends data to a network printer that has not been configured within control panel (allows one to easily print on any networked labeler)
    /// </summary>
    public class Send_IP_Printer
    {
       public IPEndPoint PrinterIP {get; private set;}
       string _name = "";
       public string SoftName
       {
           get
           {
               if (_name == "")
               {
                   _name = GetSoftNameByAddress(PrinterIP.Address.ToString());
               }
               return _name;
           }
           private set
           {
               _name = value;
           }
       }
             
        public Send_IP_Printer(string ipAddress_OR_SoftName)
        {

            //Zebra Mobile printers use port 6101 (QL, RW, MZ, etc). 
            //Larger Zebra printers usually use port 9100.
            //If the connection is "actively refused" try the other port.

            if (!ipAddress_OR_SoftName.Contains('.'))
            { //if is SoftName
                SoftName = ipAddress_OR_SoftName;
                ipAddress_OR_SoftName = GetAddressBySoftName(SoftName);
            }

            //is IPAddress
            int portno = 9100;
            PrinterIP = new IPEndPoint(IPAddress.Parse(ipAddress_OR_SoftName), portno);
                 
           
        }

        public Send_IP_Printer(string SoftName, string ipAddress)
        {

            //Zebra Mobile printers use port 6101 (QL, RW, MZ, etc). 
            //Larger Zebra printers usually use port 9100.
            //If the connection is "actively refused" try the other port.

            this.SoftName = SoftName;

            //is IPAddress
            int portno = 9100;
            PrinterIP = new IPEndPoint(IPAddress.Parse(ipAddress), portno);


        }

   
        /// <summary>
        /// Queries a List of printers in the PathDirectory MySQL Schema for an IpAddress
        /// </summary>
        /// <param name="printerName">ex: C42</param>
        /// <returns></returns>
        public string GetAddressBySoftName(string printerName)
        {
           string ip = new GetMysql().ScalarValue(@"SELECT ip_addr FROM device WHERE `name`='" + printerName + "';");
           return ip;
        }

        /// <summary>
        /// Queries a List of printers in the PathDirectory MySQL Schema for a SoftName
        /// </summary>
        /// <param name="printerName">ex: C42</param>
        /// <returns></returns>
        public string GetSoftNameByAddress(string IPAddress)
        {
            string ip = new GetMysql().ScalarValue(@"SELECT name FROM device WHERE D.`ip_addr`='" + IPAddress + "';");
            return ip;
        }

        /// <summary>
        /// returns a Dictionary of labelers in the given group.  
        /// <para>The dictionary has a SoftID for its indexes and IP or DNS Name for its values</para>
        /// </summary>
        /// <param name="GroupName">ex: '/root/CLSS'</param>
        /// <returns></returns>
        [Obsolete("Replaced by GetLabelersListOfIPs_byGroup(), the new version will only return IPs instead of IPs or DNS names",true)]
        public static Dictionary<string,string> GetLabelersList_byGroup(string GroupPath)
        {
            return getLabelersList_byGroup(GroupPath);
        }

        private static Dictionary<string, string> getLabelersList_byGroup(string GroupPath)
        {
            DataTable dt = new GetMysql().FilledTable(@"SELECT D.name,D.ip_addr FROM device D, device_has_group Dh, devicemodel DM, `group` G
WHERE Dh.Device_ID=D.Device_ID AND D.devicemodel_iddevicemodel=DM.iddevicemodel AND DM.`Type`='Labeler' AND Dh.Group_id=G.id AND G.`id`=pathtoid('" + GroupPath + "');");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dict.Add(dr["name"].ToString(), dr["ip_addr"].ToString());
            }
            return dict;

        }

        /// <summary>
        /// Returns a Dictionary of labelers in the given group
        /// <para>The dictionary has a SoftID for its indexes and IP Address for its values (IP Address is automatically converted from any DNS names in the mysql tables)</para>
        /// </summary>
        /// <param name="GroupPath">The dictionary has a SoftID for its indexes and IP Address for its values</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLabelersListOfIPs_byGroup(string GroupPath)
        {
            return DNS.NameDictionaryToIPDictionary(Send_IP_Printer.getLabelersList_byGroup(GroupPath));
        }

        public static void PrintLabel(string ip,string label_zpl) {
            new Send_IP_Printer(ip).PrintLabel(label_zpl);
        }

        /// <summary>
        /// Print a single label from the given ZPL code
        /// </summary>
        /// <param name="label_zpl"></param>
        public void PrintLabel(string label_zpl)
        {
            PrintLabel(label_zpl, 1);
        }

        /// <summary>
        /// Modifies the ZPL code so it prints out multiples of the same label
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        private string multipleLabels(string label_zpl, int quantity)
        {
            string lbl = "";
            for (int x = 0; x < quantity; x++) lbl += label_zpl;

            return lbl;
        }

        /// <summary>
        /// Print multiple labels from the given ZPL code
        /// </summary>
        /// <param name="label_zpl"></param>
        /// <param name="quantity"></param>
        public void PrintLabel(string label_zpl,int quantity)
        {
            label_zpl = multipleLabels(label_zpl, quantity);

            byte[] sendBytes = Encoding.ASCII.GetBytes(label_zpl);
            int strlength = label_zpl.Length;
            NetworkStream ns = null;

            var socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

            try
            {
                socket.Connect(PrinterIP);
                ns = new NetworkStream(socket);

                 byte[] toSend = Encoding.ASCII.GetBytes(label_zpl);
                    ns.Write(toSend, 0, toSend.Length);

               // sock.Connect(ipep);
               // NetworkStream ns = new NetworkStream(sock);


               //// NetworkStream strm = printer.GetStream();
               // ns.Write(sendBytes, 0, strlength);
               // ns.Close();
               // sock.Close();
            }
            finally 
            {
                if (ns != null)
                    ns.Close();

                if (socket != null && socket.Connected)
                    socket.Close();
                

            }
        }

        /// <summary>
        /// Print a single label from the given file and parameters
        /// </summary>
        /// <param name="labelFileName"></param>
        /// <param name="parameters"></param>
        public void PrintLabelFromFile(string labelFileName, params string[] parameters)
        {
             var SR = new StreamReader(labelFileName);
             string fileContents = SR.ReadToEnd();

            string label = string.Format(fileContents, parameters);
            PrintLabel(label);
            SR.Close();
        }

        /// <summary>
        /// Print a multiple labels from the given file and parameters
        /// </summary>
        /// <param name="labelFileName"></param>
        /// <param name="quantity"></param>
        /// <param name="parameters"></param>
        public void PrintLabelFromFile(string labelFileName,int quantity, params string[] parameters)
        {
            var SR = new StreamReader(labelFileName);
            string fileContents = SR.ReadToEnd();

            string label = string.Format(fileContents, parameters);
            PrintLabel(label,quantity);
            SR.Close();
        }

        public static void PrintLabelfromFile(string ip, string labelFileName, params string[] parameters)
        {
            new Send_IP_Printer(ip).PrintLabelFromFile(labelFileName, parameters);
        }
    }
}
