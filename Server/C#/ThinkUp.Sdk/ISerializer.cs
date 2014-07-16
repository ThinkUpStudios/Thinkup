namespace ThinkUp.Sdk
{
    public interface ISerializer
    {
        T Deserialize<T>(string serializedObj);

        string Serialize<T>(T obj);
    }
}
