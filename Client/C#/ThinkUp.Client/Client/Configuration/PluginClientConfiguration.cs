namespace ThinkUp.Client.SignalR.Client.Configuration
{
	public class PluginClientConfiguration : IPluginClientConfiguration
	{
		public string ServerUri { get; set; }

		public string ServiceName { get; set; }

		public IUserConfiguration User { get; set; }
	}
}
