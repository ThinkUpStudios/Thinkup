using ThinkUp.Client.SignalR.Client.Configuration;

namespace ThinkUp.Client.SignalR.Client
{
	public class PluginClientFactory : IPluginClientFactory
	{
		private readonly IPluginClientConfiguration configuration;
		private readonly ISerializer serializer;

		public PluginClientFactory(IPluginClientConfiguration configuration, ISerializer serializer)
		{
			this.configuration = configuration;
			this.serializer = serializer;
		}

		public IPluginClient Create()
		{
			var pluginClientConnectable = new PluginClient(this.configuration, this.serializer);

			pluginClientConnectable.Connect();

			return pluginClientConnectable;
		}
	}
}
