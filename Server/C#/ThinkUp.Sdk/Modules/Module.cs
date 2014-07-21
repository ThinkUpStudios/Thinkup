using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Components;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Modules
{
    public class Module : IModuleSetup
    {
        private readonly ISerializer serializer;
        private readonly IList<IComponent> components;

        public event EventHandler<ServerMessageEventArgs> ServerMessage;

        public IEnumerable<IComponentInformation> Components { get { return this.components; } }

        public Module(ISerializer serializer)
            : this(serializer, new List<IComponent>())
        {
        }

        public Module(ISerializer serializer, IList<IComponent> components)
        {
            this.serializer = serializer;
            this.components = new List<IComponent>();

            foreach (var component in components)
            {
                this.RegisterComponent(component);
            }
        }

        public void RegisterComponent(IComponent component)
        {
            if (!this.components.Any(c => c.Name == component.Name))
            {
                component.ServerMessage += (sender, e) =>
                {
                    var serverMessageHandler = this.ServerMessage;

                    if (serverMessageHandler != null)
                    {
                        serverMessageHandler(this, e);
                    }
                };

                this.components.Add(component);
            }
        }

        public bool CanHandleClientMessage(string serializedClientMessage)
        {
            var clientContract = this.serializer.Deserialize<ClientContract>(serializedClientMessage);
            
            return this.components.Any(c => c.CanHandleClientMessage(clientContract));
        }

        public void HandleClientMessage(string serializedClientMessage)
        {
            var clientContract = this.serializer.Deserialize<ClientContract>(serializedClientMessage);
            var component = this.components.FirstOrDefault(c => c.CanHandleClientMessage(clientContract));

            if (component == null)
            {
                var errorMessage = string.Format("There is no component registered to handle client message of type {0}", clientContract.Type);

                this.SendErrorNotification(errorMessage, receiver: clientContract.Sender);
            }

            try
            {
                component.HandleClientMessage(clientContract);
            }
            catch (ServiceException serviceEx)
            {
                this.SendErrorNotification(serviceEx, receiver: clientContract.Sender);
            }
            catch (ComponentException componentEx)
            {
                this.SendErrorNotification(componentEx, receiver: clientContract.Sender);
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unhandled error occurred. Details: {0}", ex.Message);

                this.SendErrorNotification(errorMessage, receiver: clientContract.Sender);
            }
        }

        private void SendErrorNotification(Exception exception, string receiver)
        {
            this.SendErrorNotification(exception.Message, receiver);
        }

        private void SendErrorNotification(string exceptionMessage, string receiver)
        {
            var errorServerMessage = new ErrorServerMessage
            {
                Message = exceptionMessage
            };
            var contract = new ServerContract
            {
                Type = ServerMessageType.Error,
                SerializedServerMessage = this.serializer.Serialize(errorServerMessage)
            };
            var serverMessageHandler = this.ServerMessage;

            if (serverMessageHandler != null)
            {
                serverMessageHandler(this, new ServerMessageEventArgs(contract, receiver));
            }
        }
    }
}
