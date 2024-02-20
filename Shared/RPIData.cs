using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class RPIData
    {
        [JsonInclude]
        [JsonPropertyName("rpi_id")]
        public string Id = "";

        [JsonInclude]
        [JsonPropertyName("name")]
        public string GardenName = "";

        [JsonInclude]
        [JsonPropertyName("garden_id")]
        public string GardenId = "";

        public RPIData(List<Dictionary<string, string>> data)
        {
            var rpi = data.FirstOrDefault()!;
            Id = DeviceStatic.GetString(rpi, DeviceStatic.RPIID);
            GardenName = DeviceStatic.GetString(rpi, DeviceStatic.Name);
            GardenId = DeviceStatic.GetString(rpi, DeviceStatic.GardenID);
        }

        [JsonConstructor]
        public RPIData() { }
    }
}