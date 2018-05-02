using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Principal;
namespace CodeBase2
{
   public class EventLog_CB2
    {

       string Log = "Application";
       string source;
       public EventLog_CB2(string Source)
       {
           source = Source;
   }

       /// <summary>
       /// Writes a message to the eventlog.  If running vista or windows7 this only writes when running as an Admin and the program has elevated priveleges
       /// </summary>
       /// <param name="eventMessage"></param>
       public void Write(string eventMessage)
       {
           if ((!IsVistaOrHigher) || IsAdmin)
           {
               if (!System.Diagnostics.EventLog.SourceExists(source))
                   System.Diagnostics.EventLog.CreateEventSource(source, Log);

               System.Diagnostics.EventLog.WriteEntry(source, eventMessage);
           }
           Console.WriteLine(eventMessage);
       }

       public static void WriteStackTrace(string ProgramSource, Exception e)
       {
           if (e.InnerException != null)
           {
               //write to the eventlog if running on older OS than windows vista or program is elevated priveleges
               new EventLog_CB2(ProgramSource).Write(e.Message + "\n" + e.InnerException.Message + "\n\n " + e.StackTrace);
           }
           else
           {
               //write to the eventlog if running on older OS than windows vista or program is elevated priveleges
               new EventLog_CB2(ProgramSource).Write(e.Message + "\n\n" + e.StackTrace);
           }
           throw e;
       }

       static bool IsVistaOrHigher
       {
           get
           {
               return (Environment.OSVersion.Version.Major >= 6);
           }
       }

       /// <summary>
       /// Checks if the process is elevated
       /// </summary>
       /// <returns>If is elevated</returns>
       static public bool IsAdmin
       {
           get
           {
               WindowsIdentity id = WindowsIdentity.GetCurrent();
               WindowsPrincipal p = new WindowsPrincipal(id);
               return p.IsInRole(WindowsBuiltInRole.Administrator);
           }
       }


   }
}
