using System;
using ThinkUp.Sdk.Contracts.ClientMessages;

namespace ThinkUp.Client.SignalR.Client
{
	public interface IPluginClientConnectable : IPluginClient
	{
		void Connect();
	}

	public interface IPluginClient
	{
		event EventHandler<ServerContractEventArgs> MessageReceived;

		void Send(ClientContract clientContract);
	}
}
