using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class DeviceVersion
    {
        [JsonInclude]
        [JsonPropertyName("build")]
        public string Build = "-1";
    }
}