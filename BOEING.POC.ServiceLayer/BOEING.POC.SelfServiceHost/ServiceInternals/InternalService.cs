using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceProcess;

namespace BOEING.POC.SelfServiceHost.ServiceInternals
{
    public class InternalService : ServiceBase
    {
        internal static event Action OsStarted;
        internal static event Action OsStopped;

        protected override void OnStart(string[] args)
        {
            OsStarted.Invoke();
        }

        protected override void OnStop()
        {
            OsStopped.Invoke();
        }
    }
}
