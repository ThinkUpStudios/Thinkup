using ThinkUp.Sdk.Contracts.ClientMessages;

namespace ThinkUp.Client.SignalR.Services
{
	public interface IServerSender<T> where T : IClientMessage
	{
		void Send(T clientMessage);
	}
}
