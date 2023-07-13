using System.Text.Json.Serialization;

public class GardenResponseModel
{
    [JsonInclude]
    [JsonPropertyName("garden_data")]
    public List<GardenData> GardenData = new();

    public GardenResponseModel(List<Dictionary<String, String>> data)
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

    [JsonInclude]
    [JsonPropertyName("hubs")]
    public List<String> Hubs = new();
    
    public GardenData(Dictionary<String, String> data)
    {
        this.UserName = $"{DeviceStatic.GetString(data, DeviceStatic.GivenName)} {DeviceStatic.GetString(data, DeviceStatic.FamilyName)}";
        this.GardenID = DeviceStatic.GetString(data, DeviceStatic.GardenID);
        this.GardenName = DeviceStatic.GetString(data, DeviceStatic.GardenName);
        this.WeatherLocationId = DeviceStatic.GetString(data, DeviceStatic.WeatherLocationId);
    }
}