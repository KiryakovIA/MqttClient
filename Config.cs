using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttClient
{
	internal class Config
	{
		public string Broker { get; set; }
		public int Port { get; set; }
		public string Topic { get; set; }
		public string TopicName { get; set; }
		public string TopicId { get; set; }

		string username = "igor";
		string password = "kiapass12";

	}
}
