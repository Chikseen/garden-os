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

    public DeveiceStatus(Dictionary<String, String> data)
    {
        this.RpiId = DeviceStatic.GetString(data, DeviceStatic.RPIID);
        this.TriggerdBy = DeviceStatic.GetString(data, DeviceStatic.TriggerdBy);
        this.Status = DeviceStatic.GetString(data, DeviceStatic.Status);
        this.Message = DeviceStatic.GetString(data, DeviceStatic.Message);
        this.Date = DeviceStatic.GetDateTime(data, DeviceStatic.Date);
    }

    [JsonConstructor]
    public DeveiceStatus() { }
}