using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.ServiceModel;
using RestfulPointService.Service;
using System.ServiceProcess;
using System.IO.Compression;
using System.IO;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace RestfulPointService
{
    class Program
    {
        /* Object that provides hosting capabilities for the REST service. */
        private static WebServiceHost host;

        static void Main(string[] args)
        {
            Uri httpUrl = new Uri("http://localhost:8080/");

            host = new WebServiceHost(typeof(RestPointService), httpUrl);
            var binding = new WebHttpBinding();
            host.AddServiceEndpoint(typeof(IRestPointService), binding, "/");

            foreach (ServiceEndpoint endPoint in host.Description.Endpoints)
            {
                endPoint.EndpointBehaviors.Add(new BehaviorAttribute());
            }

            host.Open();

            while (true) ;
        }
    }
}
