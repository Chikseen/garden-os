using System.Text.Json.Serialization;

namespace shared_data.Models
{
    public class ChangeUserStatusModel
    {
        [JsonInclude]
        [JsonPropertyName("user_id")]
        public string UserId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("userrole_id")]
        public string Role = string.Empty;

        [JsonInclude]
        [JsonPropertyName("garden_id")]
        public string GardenId = string.Empty;
    }
}