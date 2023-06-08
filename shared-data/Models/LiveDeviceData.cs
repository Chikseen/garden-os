using System.Text.Json.Serialization;

public class SaveDataRequest
{
    [JsonInclude]
    [JsonPropertyName("device_id")]
    public String Device_ID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public float Value = 0;
}

public class ResponseDevices
{
    [JsonInclude]
    [JsonPropertyName("devices")]
    public List<ReponseDevice> Devices = new();

    public ResponseDevices(List<Dictionary<String, String>> data)
    {
        List<ReponseDevice> devices = new();
        foreach (Dictionary<String, String> device in data)
        {
            devices.Add(new ReponseDevice(device));
        }
        this.Devices = devices;
    }

    [JsonConstructor]
    public ResponseDevices() { }
}

public class ReponseDevice
{
    [JsonInclude]
    [JsonPropertyName("device_id")]
    public String DeviceID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("entry_id")]
    public String EntryID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public float Value = 0;

    [JsonInclude]
    [JsonPropertyName("corrected_value")]
    public float CorrectedValue = 0;

    [JsonInclude]
    [JsonPropertyName("date")]
    public DateTime date = DateTime.Now;

    [JsonInclude]
    [JsonPropertyName("name")]
    public String Name = String.Empty;

    [JsonInclude]
    [JsonPropertyName("display_id")]
    public String DisplayID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("upper_limit")]
    public Int32 UpperLimit = 100;
    
    [JsonInclude]
    [JsonPropertyName("lower_limit")]
    public Int32 LowerLimit = 0;

    public ReponseDevice(Dictionary<String, String> data)
    {
        this.DeviceID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
        this.EntryID = DeviceStatic.GetString(data, DeviceStatic.ID);
        this.Value = DeviceStatic.GetFloat(data, DeviceStatic.Value, 0);
        this.date = DeviceStatic.GetDateTime(data, DeviceStatic.Date);
        this.Name = DeviceStatic.GetString(data, DeviceStatic.Name);
        this.DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
        this.UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
        this.LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
        this.CorrectedValue =  ((float)(Value - LowerLimit) * 100.0f) / (float)(UpperLimit - LowerLimit);
    }
}