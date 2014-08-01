namespace ThinkUp.Client.SignalR.Client.Configuration
{
	public class UserConfiguration : IUserConfiguration
	{
		public AuthenticationType AuthenticationType { get; set; }

		public string AuthenticationToken { get; set; }

		public string UserName { get; set; }
	}
}
