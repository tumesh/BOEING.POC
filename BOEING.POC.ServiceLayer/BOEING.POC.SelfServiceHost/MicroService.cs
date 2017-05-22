﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using System.ServiceProcess;
using System.Reflection;
 
using BOEING.POC.SelfServiceHost.ServiceInternals;
using BOEING.POC.SelfServiceHost.Network;

namespace BOEING.POC.SelfServiceHost
{
    public class MicroService
    {
        #region Events

        public event Action OnServiceStarted;
        public event Action OnServiceStopped;

        #endregion

        #region Fields

        private readonly string _serviceDisplayName;
        private readonly int _port;
        private readonly WindowsServiceManager _serviceManager;
        private readonly RegistryManipulator _registryManipulator;
        private SelfHostServer _selfHostServer;
        private readonly Action<HttpConfiguration> _configure;
        private readonly bool _useCors;

        #endregion

        #region C'tor

        public MicroService(int port = 8081, string serviceDisplayName = null, string serviceName = null,
            Action<HttpConfiguration> configure = null, bool useCors = true)
        {
            _port = port;
            _configure = configure;
            _useCors = useCors;

            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            _serviceDisplayName = serviceDisplayName ?? assemblyName;
            serviceName = serviceName ?? assemblyName;

            _serviceManager = new WindowsServiceManager(_serviceDisplayName);
            _registryManipulator = new RegistryManipulator(serviceName);

            InternalService.OsStarted += () => Start(_configure, _useCors);
            InternalService.OsStopped += Stop;
            ProjectInstaller.InitInstaller(_serviceDisplayName, serviceName);

        }

        #endregion

        #region Public

        public void Run(string[] args)
        {
            if (args.Length == 0)
            {
                RunConsole();
                return;
            }

            switch (args[0])
            {
                case "-service":
                    RunService();
                    break;
                case "-install":
                    InstallService();
                    break;
                case "-uninstall":
                    UnInstallService();
                    break;
                default:
                    throw new Exception(args[0] + " is not a valid command!");
            }
        }

        #endregion

        #region Private

        private void RunConsole()
        {
            Start(_configure, _useCors);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            Stop();
        }

        private static void RunService()
        {
            ServiceBase[] servicesToRun = { new InternalService() };
            ServiceBase.Run(servicesToRun);
        }

        private void InstallService()
        {
            _serviceManager.Install();
            _registryManipulator.AddMinusServiceToRegistry();
            _serviceManager.Start();
        }

        private void UnInstallService()
        {
            _serviceManager.Stop();
            _registryManipulator.RemoveMinusServiceFromRegistry();
            _serviceManager.UnInstall();
        }

        private void Stop()
        {
            _selfHostServer.Dispose();
            if (OnServiceStopped != null)
                OnServiceStopped.Invoke();
        }

        private void Start(Action<HttpConfiguration> configure, bool useCors)
        {
            _selfHostServer = new SelfHostServer(_port);

            _selfHostServer.Connect(configure, useCors);
            Console.WriteLine("Service {0} started on port {1}", _serviceDisplayName, _port);
            if (OnServiceStarted != null)
                OnServiceStarted.Invoke();
        }

        #endregion
    }
}
