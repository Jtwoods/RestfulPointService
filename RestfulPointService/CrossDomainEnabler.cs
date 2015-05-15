using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace RestfulPointService
{
        public class MessageInspector : IDispatchMessageInspector
        {
            private ServiceEndpoint _serviceEndpoint;

            public MessageInspector(ServiceEndpoint serviceEndpoint)
            {
                _serviceEndpoint = serviceEndpoint;
            }

            public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request,
                                                  IClientChannel channel,
                                                  InstanceContext instanceContext)
            {
                StateMessage stateMsg = null;
                HttpRequestMessageProperty requestProperty = null;
                if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    requestProperty = request.Properties[HttpRequestMessageProperty.Name]
                                  as HttpRequestMessageProperty;
                }

                if (requestProperty != null)
                {
                    var origin = requestProperty.Headers["Origin"];
                    if (!string.IsNullOrEmpty(origin))
                    {
                        stateMsg = new StateMessage();
                        if (requestProperty.Method == "OPTIONS")
                        {
                            stateMsg.Message = System.ServiceModel.Channels.Message.CreateMessage(request.Version, null);
                        }
                        request.Properties.Add("CrossOriginResourceSharingState", stateMsg);
                    }
                }

                return stateMsg;
            }

            public void BeforeSendReply(ref  System.ServiceModel.Channels.Message reply, object correlationState)
            {
                var stateMsg = correlationState as StateMessage;

                if (stateMsg != null)
                {
                    if (stateMsg.Message != null)
                    {
                        reply = stateMsg.Message;
                    }

                    HttpResponseMessageProperty responseProperty = null;

                    if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                    {
                        responseProperty = reply.Properties[HttpResponseMessageProperty.Name]
                                        as HttpResponseMessageProperty;
                    }

                    if (responseProperty == null)
                    {
                        responseProperty = new HttpResponseMessageProperty();
                        reply.Properties.Add(HttpResponseMessageProperty.Name,
                                           responseProperty);
                    }

                    responseProperty.Headers.Set("Access-Control-Allow-Origin", "*");

                    if (stateMsg.Message != null)
                    {
                        responseProperty.Headers.Set("Access-Control-Allow-Methods",
                                                   "POST, OPTIONS, GET, DELETE, PUT");
                        responseProperty.Headers.Set("Access-Control-Allow-Headers",
                                "Content-Type, Accept, Authorization, x-requested-with");
                    }
                }
            }
        }

        class StateMessage
        {
            public System.ServiceModel.Channels.Message Message;
        }

        public class BehaviorAttribute : Attribute, IEndpointBehavior,
                                       IOperationBehavior
        {
            public void Validate(ServiceEndpoint endpoint) { }

            public void AddBindingParameters(ServiceEndpoint endpoint,
                                     BindingParameterCollection bindingParameters) { }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint,
                                              EndpointDispatcher endpointDispatcher)
            {
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                                                       new MessageInspector(endpoint));
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint,
                                            ClientRuntime clientRuntime) { }

            public void Validate(OperationDescription operationDescription) { }

            public void ApplyDispatchBehavior(OperationDescription operationDescription,
                                              DispatchOperation dispatchOperation) { }

            public void ApplyClientBehavior(OperationDescription operationDescription,
                                            ClientOperation clientOperation) { }

            public void AddBindingParameters(OperationDescription operationDescription,
                                      BindingParameterCollection bindingParameters) { }
        }
    
}
