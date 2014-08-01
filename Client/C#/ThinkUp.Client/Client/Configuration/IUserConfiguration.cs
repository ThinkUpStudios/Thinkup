namespace ThinkUp.Client.SignalR.Client.Configuration
{
	public interface IUserConfiguration
	{
		AuthenticationType AuthenticationType { get; }

		string AuthenticationToken { get; set; }

		string UserName { get; set; }
	}
}
