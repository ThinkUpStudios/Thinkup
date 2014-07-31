using Microsoft.AspNet.SignalR;
using System.Linq;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Setup;

namespace ThinkUp.SignalR
{
    public class ClientManager : IClientManager
    {
        private readonly ISetupManager setupManager;
        private readonly IUserConnectionMapper userConnectionMapper;
        private readonly ISerializer serializer;
        private readonly IHubContext<IClientHub> hubContext;

        private IPlugin plugin;

        public ClientManager(ISetupManager setupManager, IUserConnectionMapper userConnectionMapper, ISerializer serializer)
        {
            this.setupManager = setupManager;
            this.userConnectionMapper = userConnectionMapper;
            this.serializer = serializer;
            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<ThinkUpHub, IClientHub>();

            this.InitializePlugin();
        }

        public void Connect(string userName, string connectionId)
        {
            if (!this.userConnectionMapper.GetConnections(userName).Any())
            {
                var connectUserClientMessage = new ConnectUserClientMessage
                {
                    UserName = userName
                };
                var clientContract = new ClientContract
                {
                    Sender = userName,
                    Type = ClientMessageType.ConnectUser,
                    SerializedClientMessage = this.serializer.Serialize(connectUserClientMessage)
                };
                var serializedClientContract = this.serializer.Serialize(clientContract);

                this.plugin.HandleClientMessage(serializedClientContract);
            }

            this.userConnectionMapper.AddConnection(userName, connectionId);
        }

        public void Reconnect(string userName, string connectionId)
        {
            var connectionIds = this.userConnectionMapper.GetConnections(userName);

            if (!connectionIds.Contains(connectionId))
            {
                this.Connect(userName, connectionId);
            }
        }

        public void SendMessage(string message, string connectionId)
        {
            var clientContract = this.serializer.Deserialize<ClientContract>(message);

            if (clientContract.Type == ClientMessageType.ConnectUser)
            {
                var errorMessage = "Player Connect message is not supported. Connection parameters must be set on initial SignalR hub connection";

                this.SendError(connectionId, errorMessage);
            }
            else
            {
                this.plugin.HandleClientMessage(message);
            }
        }

        public void Disconnect(string userName, string connectionId)
        {
            this.userConnectionMapper.RemoveConnection(userName, connectionId);

            if (!this.userConnectionMapper.GetConnections(userName).Any())
            {
                var disconnectUserClientMessage = new DisconnectUserClientMessage
                {
                    UserName = userName
                };
                var clientContract = new ClientContract
                {
                    Sender = userName,
                    Type = ClientMessageType.DisconnectUser,
                    SerializedClientMessage = this.serializer.Serialize(disconnectUserClientMessage)
                };
                var serializedClientContract = this.serializer.Serialize(clientContract);

                this.plugin.HandleClientMessage(serializedClientContract);
            }
        }

        private void InitializePlugin()
        {
            this.plugin = this.setupManager.GetPlugin();

            this.plugin.ServerMessage += (sender, args) =>
            {
                this.PushMessage(args.Receiver, args.Contract);
            };
        }

        private void PushMessage(string receiver, ServerContract serverContract)
        {
            var serializedServerContract = this.serializer.Serialize(serverContract);
            var connectionIds = this.userConnectionMapper.GetConnections(receiver);

            foreach (var connectionId in connectionIds)
            {
                this.hubContext.Clients.Client(connectionId).PushMessage(serializedServerContract);
            }
        }

        private void SendError(string connectionId, string errorMessage, params object[] parameters)
        {
            this.SendError(connectionId, 0, errorMessage, parameters);
        }

        private void SendError(string connectionId, int errorCode, string errorMessage, params object[] parameters)
        {
            var errorServerMessage = new ErrorServerMessage
            {
                ErrorCode = errorCode,
                Message = string.Format(errorMessage, parameters)
            };
            var serverContract = new ServerContract
            {
                Type = ServerMessageType.Error,
                SerializedServerMessage = this.serializer.Serialize(errorServerMessage)

            };
            var serializedServerContract = this.serializer.Serialize(serverContract);

            this.hubContext.Clients.Client(connectionId).PushMessage(serializedServerContract);
        }
    }
}