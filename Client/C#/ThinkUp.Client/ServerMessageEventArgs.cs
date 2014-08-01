using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR
{
	public class ServerMessageEventArgs<T> : EventArgs where T : IServerMessage
    {
		public T ServerMessage { get; private set; }

		public ServerMessageEventArgs(T serverMessage)
        {
			this.ServerMessage = serverMessage;
        }
    }
}
