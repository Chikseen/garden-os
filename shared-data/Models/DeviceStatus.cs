using System.Text.Json.Serialization;

public class DeveiceStatus
{
    [JsonInclude]
    [JsonPropertyName("rpi_id")]
    public String RpiId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("tiggerd_by")]
    public String TriggerdBy = String.Empty;

    [JsonInclude]
    [JsonPropertyName("stauts")]
    public String Status = String.Empty;
}