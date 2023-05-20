using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainService.Hardware
{
    public static class Prep
    {

        private static readonly HttpClient client = new HttpClient();

        public static void SetDevices()
        {

            // var data = MainDB.query("SELECT * FROM devices");
            // DevicesData devicesData = new(data);
            //  _data = devicesData;
        }

        public static RPIdata SetRPI(String id)
        {
            var responseString = client.GetStringAsync($"https://gardenapi.drunc.net/devices/{id}").Result;
            Console.WriteLine(responseString);
            RPIdata data = JsonSerializer.Deserialize<RPIdata>(responseString)!;
            Console.WriteLine("1" + data.GardenId);
            Console.WriteLine("2" + data.GardenName);
            Console.WriteLine("3" + data.Id);
            return data;
        }
    }
}