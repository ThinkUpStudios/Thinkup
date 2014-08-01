namespace ThinkUp.Client.SignalR.Client.Configuration
{
	public interface IPluginClientConfiguration
	{
		string ServerUri { get; }

		string ServiceName { get; }

		IUserConfiguration User { get; }
	}
}
