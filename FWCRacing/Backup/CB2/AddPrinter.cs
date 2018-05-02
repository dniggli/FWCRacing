using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Drawing.Printing;
using Microsoft.VisualBasic;
using System.Diagnostics;
namespace CodeBase2.Labeler
{
   

    //make sure to pull the model name straight from the inf file or you may have problems
    public class AddPrinter
    {
        RegistryReadWrite registryReadWrite = new RegistryReadWrite();

        /// <summary>
        /// Add Printer IP Port
        /// </summary>
        /// <remarks></remarks>
        public void Add_IP_Port(string IP)
        {
            RegistryHive LM = RegistryHive.LocalMachine;
            string subkey = "SYSTEM\\CurrentControlSet\\Control\\Print\\Monitors\\Standard TCP/IP Port\\Ports\\IP_" + IP;
            registryReadWrite.WriteToRegistry(LM, subkey, "Protocol", 0x1);
            registryReadWrite.WriteToRegistry(LM, subkey, "Version", 0x1);
            registryReadWrite.WriteToRegistry(LM, subkey, "HostName", "");
            registryReadWrite.WriteToRegistry(LM, subkey, "IPAddress", IP);
            registryReadWrite.WriteToRegistry(LM, subkey, "HWAddress", "");
            registryReadWrite.WriteToRegistry(LM, subkey, "PortNumber", 0x238c);
            registryReadWrite.WriteToRegistry(LM, subkey, "SNMP Community", "public");
            registryReadWrite.WriteToRegistry(LM, subkey, "SNMP Enabled", 0x1);
            registryReadWrite.WriteToRegistry(LM, subkey, "SNMP Index", 0x1);

            //Now restart print spooler to use new ports

            Services.RestartService("Spooler");
        }
        
        /// <summary>
        /// Install printer and driver and attach to Printer port
        /// </summary>
        /// <param name="IP">172.16.60.252</param>
        /// <param name="Name">HP LaserJet 4000</param>
        /// <param name="INFDriverPath">D:\Install\Drivers\HP4000\WinXP\hp222ip6.inf</param>
        /// <param name="Model">HP LaserJet 4000 Series PCL 6</param>
        public void Add(string IP, string Name, string INFDriverPath, string Model)
        {
            //        Shell("rundll32 printui.dll,PrintUIEntry /y /if /b ""HP LaserJet 4000""" & _
            //"/f D:\Install\Drivers\HP4000\WinXP\hp222ip6.inf /r ""\\SSMD\""" & pq & _
            //""".Printers.Amsterdam.NL.SSMD"" /m ""HP LaserJet 4000 Series PCL 6""", AppWinStyle.NormalFocus, True)
            string shellcmd = null;
            // "/f """ & _INFDriverPath & """ " & _
            shellcmd = "rundll32 printui.dll,PrintUIEntry /if " + "/b \"" + Name + "\" " + "/f \"" + INFDriverPath + "\" " + "/r \"IP_" + IP + "\" " + "/m \"" + Model + "\" /u";
            Interaction.Shell(shellcmd, AppWinStyle.NormalFocus, true, 5000);
        }
        //private void RAW_print(string s)
        //{
        //    PrintDialog pd = new PrintDialog();

        //    // You need a string to send.
        //    // Open the printer dialog box, and then allow the user to select a printer.
        //    //  pd.PrinterSettings = new PrinterSettings();
        //    var result = pd.ShowDialog();
        //    if (result.HasValue && result.Value)
        //    {
        //        Send_Printer.SendStringToPrinter(pd.PrintQueue.FullName, s);
        //    }
        //}
    }
}
