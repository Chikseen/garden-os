using System.Text.Json.Serialization;

public class CreateNewuserRequest
{
    [JsonInclude]
    [JsonPropertyName("garden_id")]
    public String GardenId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("user_name")]
    public String UserName = String.Empty;
}

public class CreateNewUserResponse
{
    [JsonInclude]
    [JsonPropertyName("user_id")]
    public String UserId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("api_key")]
    public String ApiKey = String.Empty;

    public CreateNewUserResponse() { }

    public CreateNewUserResponse(List<Dictionary<String, String>> data)
    {
        var user = data.FirstOrDefault()!;
        this.UserId = DeviceStatic.GetString(user, DeviceStatic.UserId);
        this.ApiKey = DeviceStatic.GetString(user, DeviceStatic.ApiKey);
    }
}