using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public interface IServerListener<T> where T : IServerMessage
	{
		event EventHandler<ServerMessageEventArgs<T>> NotificationReceived;
	}
}
