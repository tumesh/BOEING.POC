using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOEING.POC.Microservices
{
    class Program
    {
        static void Main(string[] args)
        {
            var microService = new BOEING.POC.SelfServiceHost.MicroService();
            microService.Run(args);
        }
    }
}
