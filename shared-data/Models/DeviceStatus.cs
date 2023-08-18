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

    [JsonInclude]
    [JsonPropertyName("current_build")]
    public string CurrentBuild = "-1";

    [JsonInclude]
    [JsonPropertyName("need_Update")]
    public bool NeedUpdate = false;

    public DeveiceStatus(Dictionary<String, String> data)
    {
        RpiId = DeviceStatic.GetString(data, DeviceStatic.RPIID);
        TriggerdBy = DeviceStatic.GetString(data, DeviceStatic.TriggerdBy);
        Status = DeviceStatic.GetString(data, DeviceStatic.Status);
        Message = DeviceStatic.GetString(data, DeviceStatic.Message);
        Date = DeviceStatic.GetDateTime(data, DeviceStatic.Date);
        CurrentBuild = DeviceStatic.GetString(data, DeviceStatic.Build);
    }

    [JsonConstructor]
    public DeveiceStatus() { }
}