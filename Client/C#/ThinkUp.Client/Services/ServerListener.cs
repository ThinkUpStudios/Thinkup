using System;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public class ServerListener<T> : IServerListener<T>
        where T : IServerMessage
    {
		private readonly int serverMessageType;
		private readonly IPluginClient pluginClient;
		private readonly ISerializer serializer;

        public event EventHandler<ServerMessageEventArgs<T>> NotificationReceived;

		public ServerListener(int serverMessageType, IPluginClient pluginClient, ISerializer serializer)
        {
            this.serverMessageType = serverMessageType;
            this.pluginClient = pluginClient;
			this.serializer = serializer;

			this.pluginClient.MessageReceived += (sender, args) =>
			{
				this.OnMessageReceived(args);
			};
        }

        private void OnMessageReceived(ServerContractEventArgs args)
        {
			if (this.CanParseServerContract(args.ServerContract))
            {
				var serverMessage = this.ParseServerContract(args.ServerContract);

                if (this.NotificationReceived != null)
                {
                    this.NotificationReceived(this, new ServerMessageEventArgs<T>(serverMessage));
                }
            }
        }

		private bool CanParseServerContract(ServerContract serverContract)
		{
			return serverContract.Type == this.serverMessageType;
		}

		private T ParseServerContract(ServerContract serverContract)
		{
			var serverMessage = this.serializer.Deserialize<T>(serverContract.SerializedServerMessage);

			return serverMessage;
		}
    }
}
