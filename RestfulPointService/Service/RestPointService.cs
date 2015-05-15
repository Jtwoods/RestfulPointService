using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestfulPointService.Service.Contracts;

namespace RestfulPointService.Service
{
    class RestPointService: IRestPointService
    {

        public UserPoints GetUserPoint()
        {
            UserPoints toReturn = new UserPoints();
            toReturn.name = "Name";
            toReturn.points = 0;
            return toReturn;
        }
    }
}