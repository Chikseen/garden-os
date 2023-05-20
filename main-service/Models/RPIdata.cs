using System.Text.Json.Serialization;

public class RPIdata
{
    [JsonInclude]
    [JsonPropertyName("rpi_id")]
    public String Id = "";

    [JsonInclude]
    [JsonPropertyName("name")]
    public String GardenName = "";

    [JsonInclude]
    [JsonPropertyName("garden_id")]
    public String GardenId = "";

    public RPIdata(List<Dictionary<String, String>> data)
    {
        var rpi = data.FirstOrDefault()!;
        this.Id = DeviceStatic.GetString(rpi, DeviceStatic.RPIID);
        this.GardenName = DeviceStatic.GetString(rpi, DeviceStatic.Name);
        this.GardenId = DeviceStatic.GetString(rpi, DeviceStatic.GardenID);
    }
}