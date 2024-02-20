using System.Net.Http.Headers;
using System.Text.Json;
using dotenv.net;
using Shared.Models;

namespace MainService.Hardware
{
    public class Preperation
    {
        private readonly HttpClient client;
        private readonly string _url = string.Empty;

        public Preperation(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            DotEnv.Load();
            _url = Environment.GetEnvironmentVariable("URL")!;
        }

        public RPIDevices SetDevices(string id)
        {
            string responseString = client.GetStringAsync($"{_url}/devices/{id}").Result;
            RPIDevices data = JsonSerializer.Deserialize<RPIDevices>(responseString)!;
            return data;
        }

        public RPIData SetRPI(string id)
        {
            string responseString = client.GetStringAsync($"{_url}/devices/{id}/metadata").Result;
            RPIData data = JsonSerializer.Deserialize<RPIData>(responseString)!;
            return data;
        }
    }
}