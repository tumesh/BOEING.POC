using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace BOEING.POC.SelfServiceHost.ServiceInternals
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private static string _serviceDisplayName;
        private static string _serviceName;

        public static void InitInstaller(string serviceDisplayName, string serviceName)
        {
            _serviceDisplayName = serviceDisplayName;
            _serviceName = serviceName;
        }

        public ProjectInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };

            var serviceInstaller = new ServiceInstaller
            {
                DisplayName = _serviceDisplayName,
                ServiceName = _serviceName,
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
