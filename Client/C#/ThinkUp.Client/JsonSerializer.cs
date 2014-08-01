using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace ThinkUp.Client.SignalR
{
	public class JsonSerializer : ISerializer
	{
		public TObject Deserialize<TObject>(string serializedObj)
		{
			return JsonConvert.DeserializeObject<TObject>(serializedObj, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects
			});
		}

		public string Serialize<TObject>(TObject obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
			});
		}
	}
}
