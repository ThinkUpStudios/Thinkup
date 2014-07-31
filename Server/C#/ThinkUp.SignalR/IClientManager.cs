namespace ThinkUp.SignalR
{
    public interface IClientManager
    {
        void Connect(string userName, string connectionId);

        void Reconnect(string userName, string connectionId);

        void SendMessage(string message, string connectionId);

        void Disconnect(string userName, string connectionId);
    }
}
