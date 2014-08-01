using ThinkUp.Client.SignalR.Client;
using ThinkUp.Sdk.Contracts.ClientMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public class ServerSender<T> : IServerSender<T> where T : IClientMessage
	{
		private readonly int clientMessageType;
		private readonly IPluginClient pluginClient;
		private readonly ISerializer serializer;

		public ServerSender(int clientMessageType, IPluginClient pluginClient, ISerializer serializer)
        {
			this.clientMessageType = clientMessageType;
			this.pluginClient = pluginClient;
			this.serializer = serializer;
        }

		public void Send(T clientMessage)
		{
			var serializedClientMessage = this.serializer.Serialize(clientMessage);
			var clientContract = new ClientContract
			{
				Type = this.clientMessageType,
				Sender = clientMessage.UserName,
				SerializedClientMessage = serializedClientMessage
			};

			this.pluginClient.Send(clientContract);
		}
	}
}
