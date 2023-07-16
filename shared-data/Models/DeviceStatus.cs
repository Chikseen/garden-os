using System.Text.Json.Serialization;

public class DeveiceStatus
{
    [JsonInclude]
    [JsonPropertyName("rpi_id")]
    public String RpiId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("triggerd_by")]
    public String TriggerdBy = String.Empty;

    [JsonInclude]
    [JsonPropertyName("status")]
    public String Status = String.Empty;

    [JsonInclude]
    [JsonPropertyName("message")]
    public String Message = String.Empty;

    [JsonInclude]
    [JsonPropertyName("date")]
    public DateTime Date;

    public DeveiceStatus(List<Dictionary<String, String>> data)
    {
        var latest = data.FirstOrDefault()!;
        if (latest is not null)
        {
            this.RpiId = DeviceStatic.GetString(latest, DeviceStatic.RPIID);
            this.TriggerdBy = DeviceStatic.GetString(latest, DeviceStatic.TriggerdBy);
            this.Status = DeviceStatic.GetString(latest, DeviceStatic.Status);
            this.Message = DeviceStatic.GetString(latest, DeviceStatic.Message);
            this.Date = DeviceStatic.GetDateTime(latest, DeviceStatic.Date);
        }
    }

    [JsonConstructor]
    public DeveiceStatus() { }
}