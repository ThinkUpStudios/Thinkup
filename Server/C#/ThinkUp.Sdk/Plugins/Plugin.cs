using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Plugins
{
    public class Plugin : IPluginSetup
    {
        private readonly ISerializer serializer;
        private readonly IList<IPluginComponent> components;

        public event EventHandler<ServerMessageEventArgs> ServerMessage;

        public IEnumerable<IPluginComponentInformation> Components { get { return this.components; } }

        public Plugin(ISerializer serializer)
            : this(serializer, new List<IPluginComponent>())
        {
        }

        public Plugin(ISerializer serializer, IList<IPluginComponent> components)
        {
            this.serializer = serializer;
            this.components = new List<IPluginComponent>();

            foreach (var component in components)
            {
                this.RegisterComponent(component);
            }
        }

        public void RegisterComponent(IPluginComponent component)
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
            var components = this.components.Where(c => c.CanHandleClientMessage(clientContract));

            if (!components.Any())
            {
                var errorMessage = string.Format("There are no components registered to handle client message of type {0}", clientContract.Type);

                this.SendErrorNotification(errorMessage, receiver: clientContract.Sender);
            }

            try
            {
                foreach (var component in components)
                {
                    component.HandleClientMessage(clientContract);
                }
            }
            catch (ServiceException serviceEx)
            {
                this.SendErrorNotification(serviceEx, receiver: clientContract.Sender);
            }
            catch (PluginComponentException componentEx)
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
