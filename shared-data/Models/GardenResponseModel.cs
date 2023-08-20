using System.Text.Json.Serialization;

namespace shared_data.Models
{
    public class GardenResponseModel
    {
        [JsonInclude]
        [JsonPropertyName("garden_data")]
        public List<GardenData> GardenData = new();

        public GardenResponseModel(List<Dictionary<string, string>> data)
        {
            foreach (var item in data)
            {
                GardenData.Add(new GardenData(item));
            }
        }

        [JsonConstructor]
        public GardenResponseModel()
        { }
    }

    public class GardenData
    {
        [JsonInclude]
        [JsonPropertyName("garden_id")]
        public string GardenID = "";

        [JsonInclude]
        [JsonPropertyName("garden_name")]
        public string GardenName = "";

        [JsonInclude]
        [JsonPropertyName("weather_location_id")]
        public string WeatherLocationId = "";

        [JsonInclude]
        [JsonPropertyName("hubs")]
        public List<string> Hubs = new();

        public GardenData(Dictionary<string, string> data)
        {
            GardenID = DeviceStatic.GetString(data, DeviceStatic.GardenID);
            GardenName = DeviceStatic.GetString(data, DeviceStatic.GardenName);
            WeatherLocationId = DeviceStatic.GetString(data, DeviceStatic.WeatherLocationId);
        }
    }
}