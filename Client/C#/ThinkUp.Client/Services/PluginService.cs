using System;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public class PluginService<T, U> : IPluginService<T, U>
        where T : IClientMessage
        where U : IServerMessage
    {
		private readonly IServerSender<T> serverSender;
		private readonly IServerListener<U> serverListener;

		public event EventHandler<ServerMessageEventArgs<U>> NotificationReceived;

		public PluginService(int clientMessageType, int serverMessageType, IPluginClient pluginClient, ISerializer serializer)
        {
			this.serverSender = new ServerSender<T>(clientMessageType, pluginClient, serializer);
			this.serverListener = new ServerListener<U>(serverMessageType, pluginClient, serializer);

			this.serverListener.NotificationReceived += (sender, args) =>
			{
				if (this.NotificationReceived != null)
				{
					this.NotificationReceived(this, args);
				}
			};
        }

		public void Send(T clientMessage)
		{
			this.serverSender.Send(clientMessage);
		}
	}
}
