using WebsocketCallback.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketCallback
{
    [ServiceContract(CallbackContract = typeof(IWebsocketCallback))]
    public interface IWebsocketCallbackService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(string Id);

        [OperationContract(IsOneWay = true)]
        void RequestIDPicked(string Id, string LoginGroup, string LoginId);

        [OperationContract(IsOneWay = true)]
        void Response(string oResponse);

        [OperationContract(IsOneWay = true)]
        void UnSubscribe(string Id);

        [OperationContract(IsOneWay = true, Action = "*")]
        Task SendMessageToServer(Message msg);
    }
}
