using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainService.Hardware
{
    public class Preperation
    {
        private readonly HttpClient client;

        public Preperation(String apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public RPIDevices SetDevices(String id)
        {
            String responseString = client.GetStringAsync($"https://gardenapi.drunc.net/devices/{id}").Result;
            RPIDevices data = JsonSerializer.Deserialize<RPIDevices>(responseString)!;
            return data;
        }

        public RPIData SetRPI(String id)
        {
            String responseString = client.GetStringAsync($"https://gardenapi.drunc.net/devices/{id}/metadata").Result;
            RPIData data = JsonSerializer.Deserialize<RPIData>(responseString)!;
            return data;
        }
    }
}