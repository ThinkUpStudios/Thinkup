using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using ThinkUp.Client.SignalR.Client.Configuration;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Client
{
	public class PluginClient : IPluginClientConnectable
	{
		private readonly IPluginClientConfiguration configuration;
        private readonly ISerializer serializer;

		private IDictionary<AuthenticationType, Func<Dictionary<string, string>>> queryStringBuilderList;
		private HubConnection hubConnection;
		private IHubProxy hubProxy;

        public event EventHandler<ServerContractEventArgs> MessageReceived;

		public PluginClient(IPluginClientConfiguration configuration, ISerializer serializer)
        {
			this.configuration = configuration;
			this.serializer = serializer;
			this.ConfigureQueryStringBuilderList();
        }

        public async void Connect()
        {
			Func<Dictionary<string, string>> queryStringBuilder = default(Func<Dictionary<string, string>>);

			this.queryStringBuilderList.TryGetValue(this.configuration.User.AuthenticationType, out queryStringBuilder);

			var queryString = queryStringBuilder.Invoke();

			this.hubConnection = new HubConnection(this.configuration.ServerUri, queryString);
			this.hubProxy = this.hubConnection.CreateHubProxy("ThinkUpHub");

			this.hubProxy.On<string>("PushMessage", serializedServerContract =>
			{
				this.ReceiveMessage(serializedServerContract);
			});

			await this.hubConnection.Start();
        }

		public async void Send(ClientContract clientContract)
        {
			var message = this.serializer.Serialize(clientContract);

			await this.hubProxy.Invoke("SendMessage", message);
        }

		private void ConfigureQueryStringBuilderList()
		{
			this.queryStringBuilderList = new Dictionary<AuthenticationType, Func<Dictionary<string, string>>>();

			this.queryStringBuilderList.Add(AuthenticationType.None, () =>
			{
				var queryString = new Dictionary<string, string>();
				var authenticationType = (int)AuthenticationType.None;
				var userName = this.configuration.User.UserName;

				queryString.Add("authenticationType", authenticationType.ToString());
				queryString.Add("userName", userName);

				return queryString;
			});

			this.queryStringBuilderList.Add(AuthenticationType.Facebook, () =>
			{
				var queryString = new Dictionary<string, string>();
				var authenticationType = (int)AuthenticationType.Facebook;
				var authenticationToken = this.configuration.User.AuthenticationToken;

				queryString.Add("authenticationType", authenticationType.ToString());
				queryString.Add("authenticationToken", authenticationToken);

				return queryString;
			});
		}

		private void ReceiveMessage(string serializedServerContract)
        {
			var serverContract = this.serializer.Deserialize<ServerContract>(serializedServerContract);

			if (this.MessageReceived != null)
			{
				this.MessageReceived(this, new ServerContractEventArgs(serverContract));
			}
        }
	}
}
