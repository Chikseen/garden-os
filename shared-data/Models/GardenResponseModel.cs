using System.Text.Json.Serialization;

public class GardenResponseModel
{
    [JsonInclude]
    [JsonPropertyName("user_name")]
    public String UserName = "";

    [JsonInclude]
    [JsonPropertyName("garden_id")]
    public String GardenID = "";

    [JsonInclude]
    [JsonPropertyName("garden_name")]
    public String GardenName = "";

    public GardenResponseModel(List<Dictionary<String, String>> data)
    {
        var garden = data.FirstOrDefault()!;
        this.UserName = DeviceStatic.GetString(garden, DeviceStatic.UserName);
        this.GardenID = DeviceStatic.GetString(garden, DeviceStatic.GardenID);
        this.GardenName = DeviceStatic.GetString(garden, DeviceStatic.GardenName);
    }

    [JsonConstructor]
    public GardenResponseModel() { }
}