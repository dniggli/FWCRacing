using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace CodeBase2
{
    public static class Services
    {
        /// <summary>
        /// Restarts a service
        /// Ex: Print Spooler
        /// RestartService("Spooler")
        /// </summary>
        /// <param name="ServiceShortname">Listed in service properties as "Service Name"</param>
        /// <remarks></remarks>
        public static void RestartService(string ServiceShortname)
        {
            ServiceController controller = new ServiceController(ServiceShortname);
            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped);
            controller.Start();
            // System.Threading.Thread.Sleep()

            controller.WaitForStatus(ServiceControllerStatus.Running);
        }
    }

}
