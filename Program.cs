using System.Text;
using MqttClient;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal class Program
{
	private static async Task Main(string[] args)
	{
		if (!await Config.LoadDataAsync())
		{
			Console.WriteLine("Не удалось загрузить конфигурацию");
			return;
		}


		PescClient pescClient = new PescClient();
		if (!await pescClient.GetData())
		{
			Console.WriteLine("Не удалось выполнить авторизацию");
			return;
		}

		// Create a MQTT client factory
		var factory = new MqttFactory();

		// Create a MQTT client instance
		var mqttClient = factory.CreateMqttClient();

		// Create MQTT client options
		var options = new MqttClientOptionsBuilder()
			.WithTcpServer(Config.Data.Mqtt.Server, Config.Data.Mqtt.Port) // MQTT broker address and port
			.WithCredentials(Config.Data.Mqtt.User, Config.Data.Mqtt.Password) // Set username and password
			.WithClientId("MqttClient")
			.WithCleanSession()
			.Build();

		// Connect to MQTT broker
		var connectResult = await mqttClient.ConnectAsync(options);

		if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
		{
			Console.WriteLine("Connected to MQTT broker successfully.");

			// Subscribe to a topic
			await mqttClient.SubscribeAsync(Config.Data.Mqtt.BaseTopic + "/kwh");

			// Callback function when a message is received
			mqttClient.ApplicationMessageReceivedAsync += e =>
			{
				Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
				return Task.CompletedTask;
			};

			decimal value = 10;
			// Publish a message 10 times
			for (int i = 0; i < 100; i++)
			{
				string payload = JsonConvert.SerializeObject(new JObject(new JProperty("value", value)));

				var message = new MqttApplicationMessageBuilder()
					.WithTopic(Config.Data.Mqtt.BaseTopic + "/kwh")
					.WithPayload(payload)
					.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
					.WithRetainFlag()
					.Build();

				await mqttClient.PublishAsync(message);
				await Task.Delay(5000); // Wait for 1 second
				value++;
			}

			// Unsubscribe and disconnect
			await mqttClient.UnsubscribeAsync(Config.Data.Mqtt.BaseTopic + "/kwh");
			await mqttClient.DisconnectAsync();
		}
		else
		{
			Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
		}
	}
}