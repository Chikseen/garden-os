using System.Text.Json.Serialization;

public class MapJSON
{
    [JsonInclude]
    [JsonPropertyName("json")]
    public String Json = "";

    public MapJSON(List<Dictionary<String, String>> data)
    {
        var map = data.FirstOrDefault()!;
        this.Json = DeviceStatic.GetString(map, DeviceStatic.JSON);
    }
    public MapJSON()
    { }
}