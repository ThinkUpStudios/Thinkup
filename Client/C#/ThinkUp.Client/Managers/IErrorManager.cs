using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Managers
{
	public interface IErrorManager
	{
		event EventHandler<ServerMessageEventArgs<ErrorServerMessage>> ErrorNotificationReceived;
	}
}
