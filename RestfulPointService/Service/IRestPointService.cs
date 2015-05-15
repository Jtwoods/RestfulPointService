using RestfulPointService.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestfulPointService.Service
{
    [ServiceContract]
    public interface IRestPointService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Points", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        UserPoints GetUserPoint();
    }
}
