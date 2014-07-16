namespace ThinkUp.Sdk.Interfaces
{
    public interface IChatMessage
    {
        string From { get; }

        string To { get; }

        string Message { get; }
    }
}
