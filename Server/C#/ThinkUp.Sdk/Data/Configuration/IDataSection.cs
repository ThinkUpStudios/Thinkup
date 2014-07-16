namespace ThinkUp.Sdk.Data.Configuration
{
    public interface IDataSection
    {
        string ConnectionString { get; }

        string DatabaseName { get; }
    }
}
