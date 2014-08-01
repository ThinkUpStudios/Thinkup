namespace ThinkUp.Client.SignalR
{
	public interface ISerializer
	{
		TObject Deserialize<TObject>(string serializedObj);

		string Serialize<TObject>(TObject obj);
	}
}
