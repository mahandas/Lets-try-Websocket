using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.WebSockets;
using WebsocketCallback.DataContract;

namespace WebsocketCallback
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)] 
    public class WebsocketCallbackService : IWebsocketCallbackService
    {
        internal static ConcurrentDictionary<string, IWebsocketCallback> oSubscription;



        public WorkflowCallbackService()
        {

        }

         
        public void Subscribe(string Id)
        {
            
            try
            {
                
                if (oSubscription == null)
                    oSubscription = new ConcurrentDictionary<string, IWebsocketCallback>();

                if (!oSubscription.ContainsKey(Id))
                {
                    if (!oSubscription.TryAdd(Id, OperationContext.Current.GetCallbackChannel<IWebsocketCallback>()))
                    {
                        throw new Exception("Error while subscribing call back for RFQ Id " + RFQId + ".");
                    }
                 
                }
               
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex);
            }
        }
          

        public async void RequestIDPicked(string Id, string LoginGroup, string LoginId)
        {
            IWorkflowCallback callback = null;
            try
            {
                
                if (!oSubscription.TryGetValue(RFQId, out callback))
                {
                    throw new Exception("Subscription failed for ID:" + RFQId);
                }

                if (((IChannel)callback).State != CommunicationState.Opened)
                {
                    throw new Exception("Connection failed for ID.[" + Id + "]State:" + ((IChannel)callback).State);
                }

                string msgTextToClient = string.Format("ID_PICKED|{0}|{1}|{2}", RFQId, LoginId, LoginGroup);
                
                await SendMessageToServer(CreateMessage(msgTextToClient));
              
            }
            catch (Exception ex)
            {
             
                throw new FinIQServiceException(ex);
            }
        }

        public async void Response(string Id)
        {
            IWebsocketCallback callback = null;
            List<string> oKeys;
            try
            {
                

                oKeys = oSubscription.Keys.ToList<string>();


                foreach (string key in oKeys)
                {
                    if (!oSubscription.TryGetValue(key, out callback))
                    {
                        throw new Exception("");
                    }

         
                         
                    if (((IChannel)callback).State != CommunicationState.Opened)
                    {
                        throw new Exception("");
                    }

                    
                    string msgTextToClient = string.Format("RFQRESPONSE|{0}|{1}", callback, callback);
                    


                    await SendMessageToServer(CreateMessage(msgTextToClient));

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex);
            }
        }

        public void UnSubscribe(string Id)
        {
            IWebsocketCallback callback = null;
            try
            {
               
                if (!oSubscription.ContainsKey(Id))
                {
                    if (!oSubscription.TryRemove(Id, out callback))
                    {

                    }
                
                }
              
            }
            catch (Exception ex)
            {
        
                throw new Exception(ex);
            } 
        }

        private Message CreateMessage(string msgText)
        {
            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(msgText)));
            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty { MessageType = WebSocketMessageType.Text };
            return msg;
        }

        public async Task SendMessageToServer(Message msg)
        {
            string[] MessageData;
            IWorkflowCallback callback;
            List<string> oKeys;
            string msgTextToClient = string.Empty;
            try
            {
          
         
                if (msg.IsEmpty)
                {
                    return;
                }

                if (oSubscription == null)
                    oSubscription = new ConcurrentDictionary<string, IWorkflowCallback>();

                byte[] body = msg.GetBody<byte[]>();
                string msgTextFromClient = Encoding.UTF8.GetString(body);

                MessageData = msgTextFromClient.Split('|');

                if (MessageData != null && MessageData.Length > 0)
                {
                    switch (MessageData[0].Trim().ToUpper())
                    {

                        case "SUBSCRIBE":
                            if (!oSubscription.ContainsKey(MessageData[1]))
                            {
                                if (!oSubscription.TryAdd(MessageData[1], OperationContext.Current.GetCallbackChannel<IWebsocketCallback>()))
                                {
                                    throw new Exception("Error while subscribing call back for ID " + MessageData[1] + ".");
                                }
                                await OperationContext.Current.GetCallbackChannel<IWorkflowCallback>().SendMessageToClient(CreateMessage("Subscribed|Subscribed for Id : " + MessageData[1]));
                            }
                            else
                            {
                                // Update current ID with new channel
                            }
                            break;


                        case "UNSUBSCRIBE":
                            if (oSubscription.ContainsKey(MessageData[1]))
                            {
                                if (!oSubscription.TryRemove(MessageData[1], out callback))
                                {
                                    await callback.SendMessageToClient(CreateMessage("Unsubscribed|Error while unsubscribing call back for Id " + MessageData[1] + "."));
                                }
                                await callback.SendMessageToClient(CreateMessage("Unsubscribed|Unsubscribed for Id : " + MessageData[1]));
                            }
                            break;

                        case "RESPONSE":

                            oKeys = oSubscription.Keys.ToList<string>();

                            foreach (string key in oKeys)
                            { 
                                //print keys -- debug case
                            }


                            foreach (string key in oKeys)
                            {
                                try
                                { 
                                    

                                    if (oSubscription.ContainsKey(key))
                                    {

                                      

                                        if (!oSubscription.TryGetValue(key, out callback))
                                        {
                                            
                                            throw new Exception("Error while getting key value.....");

                                        }
                                 
                                     
                                        if (((IChannel)callback).State != CommunicationState.Opened)
                                        {
                                           
                                            throw new Exception("Connection clossed.....");
                                        }

                                        msgTextToClient = string.Format("RFQRESPONSE|{0}", callback);

                                        await callback.SendMessageToClient(CreateMessage(msgTextToClient));

                                    }

                                }
                                catch (Exception ex) 
                                {

                                    throw new Exception(ex);
                                } 
                            }


                             
                            break;
                    }                   
                }
                else
                { return; }

            }
            catch (Exception ex)
            {

                throw new Exception(ex);
            }
        }

      

        
         


    }
}
