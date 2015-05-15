using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestfulPointService.Service.Contracts
{
    [DataContract]
    public class UserPoints
    {
        [DataMember(Name = "userName")]
        public string name;
        [DataMember(Name = "userPoints")]
        public long points;
    }
}
