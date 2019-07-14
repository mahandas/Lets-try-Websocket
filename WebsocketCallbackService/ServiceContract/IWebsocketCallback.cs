using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketCallback
{
    [ServiceContract]
    interface IWebsocketCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        Task SendMessageToClient(Message msg);
    }
}
