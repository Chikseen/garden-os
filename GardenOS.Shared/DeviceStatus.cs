using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class DeveiceStatus
    {
        [JsonInclude]
        [JsonPropertyName("rpi_id")]
        public string RpiId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("triggerd_by")]
        public string TriggerdBy = string.Empty;

        [JsonInclude]
        [JsonPropertyName("status")]
        public string Status = string.Empty;

        [JsonInclude]
        [JsonPropertyName("message")]
        public string Message = string.Empty;

        [JsonInclude]
        [JsonPropertyName("date")]
        public DateTime Date;

        [JsonInclude]
        [JsonPropertyName("current_build")]
        public string CurrentBuild = "-1";

        [JsonInclude]
        [JsonPropertyName("need_Update")]
        public bool NeedUpdate = false;

        public DeveiceStatus(Dictionary<string, string> data)
        {
            RpiId = DeviceStatic.GetString(data, DeviceStatic.RPIID);
            TriggerdBy = DeviceStatic.GetString(data, DeviceStatic.TriggerdBy);
            Status = DeviceStatic.GetString(data, DeviceStatic.Status);
            Message = DeviceStatic.GetString(data, DeviceStatic.Message);
            Date = DeviceStatic.GetLocalDateTime(data, DeviceStatic.Date);
            CurrentBuild = DeviceStatic.GetString(data, DeviceStatic.Build);
        }

        [JsonConstructor]
        public DeveiceStatus() { }
    }
}