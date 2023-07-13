using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using dotenv.net;

namespace MainService.Hardware
{
    public class Preperation
    {
        private readonly HttpClient client;
        private String _url = String.Empty;

        public Preperation(String apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            DotEnv.Load();
            _url = Environment.GetEnvironmentVariable("URL")!;
        }

        public RPIDevices SetDevices(String id)
        {
            String responseString = client.GetStringAsync($"{_url}/devices/{id}").Result;
            RPIDevices data = JsonSerializer.Deserialize<RPIDevices>(responseString)!;
            return data;
        }

        public RPIData SetRPI(String id)
        {
            String responseString = client.GetStringAsync($"{_url}/devices/{id}/metadata").Result;
            RPIData data = JsonSerializer.Deserialize<RPIData>(responseString)!;
            return data;
        }
    }
}