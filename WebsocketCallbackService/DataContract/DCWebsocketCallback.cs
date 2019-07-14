using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketCallback.DataContract
{
    [DataContract()]
    public class ResponseContract
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public string ResponseToServer { get; set; }
    }

    [DataContract]
    public enum Status
    {
        [EnumMember]
        Replied,

        [EnumMember]
        Rejected
    }
}
