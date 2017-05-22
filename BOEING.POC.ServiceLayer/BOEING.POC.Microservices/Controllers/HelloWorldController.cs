using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
namespace BOEING.POC.Microservices.Controllers
{
    public class HelloWorldController : ApiController
    {
        [Route("api/v1/BOEING/Services/HelloWorld")]
        public string GetHelloWorld()
        {
            return "Hello World!";
        }

        [Route("api/v1/BOEING/Services/HelloWorld/{UserName}")]
        public string GetHelloWorld(string UserName)
        {
            return string.Format("Hello {0} World!", UserName);
        }
    }
}
