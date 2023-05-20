using System;
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

    public RPIdata()
    { }
}