using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public interface IPluginService<T, U> : IServerSender<T>, IServerListener<U>
		where T : IClientMessage
		where U : IServerMessage
	{
	}
}
