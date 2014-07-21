using System;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Sdk
{
    public class ServerMessageEventArgs : EventArgs
    {
        public string Receiver { get; set; }

        public ServerContract Contract { get; set; }

        public ServerMessageEventArgs(ServerContract contract, string receiver)
        {
            this.Contract = contract;
            this.Receiver = receiver;
        }

        public ServerMessageEventArgs()
        {
        }
    }
}
