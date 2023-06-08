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
    
    [JsonInclude]
    [JsonPropertyName("weather_location_id")]
    public String WeatherLocationId = "";

    public GardenResponseModel(List<Dictionary<String, String>> data)
    {
        var garden = data.FirstOrDefault()!;
        this.UserName = DeviceStatic.GetString(garden, DeviceStatic.UserName);
        this.GardenID = DeviceStatic.GetString(garden, DeviceStatic.GardenID);
        this.GardenName = DeviceStatic.GetString(garden, DeviceStatic.GardenName);
        this.WeatherLocationId = DeviceStatic.GetString(garden, DeviceStatic.WeatherLocationId);
    }

    [JsonConstructor]
    public GardenResponseModel()
    { }
}