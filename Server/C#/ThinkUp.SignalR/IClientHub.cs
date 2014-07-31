namespace ThinkUp.SignalR
{
    public interface IClientHub
    {
        void PushMessage(string serializedServerContract);
    }
}
