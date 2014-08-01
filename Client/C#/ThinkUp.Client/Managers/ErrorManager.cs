using System;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public class ErrorManager : IErrorManager
	{
		private readonly IServerListener<ErrorServerMessage> errorListener;

		public event EventHandler<ServerMessageEventArgs<ErrorServerMessage>> ErrorNotificationReceived;

		public ErrorManager(IPluginClient pluginClient, ISerializer serializer)
		{
			this.errorListener = new ServerListener<ErrorServerMessage>(ServerMessageType.Error, pluginClient, serializer);

			this.errorListener.NotificationReceived += (sender, args) =>
			{
				if (this.ErrorNotificationReceived != null)
				{
					this.ErrorNotificationReceived(this, args);
				}
			};
		}
	}
}
