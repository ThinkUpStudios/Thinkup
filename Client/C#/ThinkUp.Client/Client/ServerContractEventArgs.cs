using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Client
{
	public class ServerContractEventArgs: EventArgs
    {
		public ServerContract ServerContract { get; private set; }

		public ServerContractEventArgs(ServerContract serverContract)
        {
            this.ServerContract = serverContract;
        }
    }
}
