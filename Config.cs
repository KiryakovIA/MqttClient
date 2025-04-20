using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MqttClient
{
	internal class Config
	{
		public static Config Data { get; private set; }

		public MqttConfig Mqtt { get; set; }

		public static async Task<bool> LoadDataAsync()
		{
			try
			{
				string json = await File.ReadAllTextAsync("Config.json");
				Data = JsonConvert.DeserializeObject<Config>(json);
			}
			catch
			{
			}
			return Data != null;
		}

		public class MqttConfig
		{
			public string Server { get; set; }
			public int Port { get; set; }
			public string BaseTopic { get; set; }
			public string User { get; set; }
			public string Password { get; set; }
		}

	}
}
