using System.Text.Json.Serialization;

namespace shared_data.Models
{
    public class DeviceVersion
    {
        [JsonInclude]
        [JsonPropertyName("build")]
        public string Build = "-1";
    }
}