using System.Reflection;
using Newtonsoft.Json;

namespace MqttClient
{
	internal class PescClient
	{
		const string BaseUrl = "https://ikus.pesc.ru";

		internal async Task<bool> GetData()
		{
			//using HttpClient client = new HttpClient();
			//client.BaseAddress = new Uri(BaseUrl);

			//var payload = new
			//{
			//	login = Config.Data.Pesc.Login,
			//	password = Config.Data.Pesc.Password,
			//	type = "PHONE"
			//};

			//HttpResponseMessage response = await client.PostAsJsonAsync("/api/v8/users/auth", payload);
			//if (!response.IsSuccessStatusCode)
			//	return false;

			//AuthResponse? authResponse = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());
			//if (authResponse == null)
			//	return false;

			//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.auth);

			//response = await client.GetAsync($"/api/v6/accounts/{Config.Data.Pesc.AccountId}/meters/info");
			//if (!response.IsSuccessStatusCode)
			//	return false;

			//var info = response.Content.ReadAsStringAsync();
			string json = File.ReadAllText("MetersInfo.json");

			var MetersInfo = JsonConvert.DeserializeObject<List<MeterInfoResponse>>(json);

			return true;
		}

		public class AuthResponse
		{
			public string access { get; set; }
			public string auth { get; set; }
		}

		public class MeterInfoResponse
		{
			public IdClass id { get; set; }
			public string serial { get; set; }
			public List<IndicationClass> indications { get; set; }

			public class IdClass
			{
				public string registration { get; set; }
			}

			public class IndicationClass
			{
				public DateTime previousReadingDate { get; set; }
				public int meterScaleId { get; set; }
				public string scaleName { get; set; }
				public decimal previousReading { get; set; }
			}
		}
	}
}
