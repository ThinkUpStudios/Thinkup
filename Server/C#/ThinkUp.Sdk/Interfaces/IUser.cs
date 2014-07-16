namespace ThinkUp.Sdk.Interfaces
{
    public interface IUser
    {
        string Name { get; }

        string DisplayName { get; }

        bool IsConnected { get; }
    }
}
