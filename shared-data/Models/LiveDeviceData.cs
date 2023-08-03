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

    [JsonInclude]
    [JsonPropertyName("sort_order")]
    public int SortOrder = -1;

    [JsonInclude]
    [JsonPropertyName("group_id")]
    public String GroupId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("unit")]
    public String Unit = String.Empty;

    [JsonInclude]
    [JsonPropertyName("special_id")]
    public String SpecialId = String.Empty;

    public Boolean IsInverted = false;

    public ReponseDevice(Dictionary<String, String> data)
    {
        this.DeviceID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
        this.EntryID = DeviceStatic.GetString(data, DeviceStatic.ID);
        this.Value = DeviceStatic.GetFloat(data, DeviceStatic.Value, 0);
        this.date = DeviceStatic.GetDateTime(data, DeviceStatic.UploadDate);
        this.Name = DeviceStatic.GetString(data, DeviceStatic.Name);
        this.DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
        this.UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
        this.LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
        this.SortOrder = DeviceStatic.GetInt(data, DeviceStatic.SortOrder, -1);
        this.GroupId = DeviceStatic.GetString(data, DeviceStatic.GroupId);
        this.Unit = DeviceStatic.GetString(data, DeviceStatic.Unit);
        this.IsInverted = DeviceStatic.GetBool(data, DeviceStatic.IsInverted, false);
        this.SpecialId = DeviceStatic.GetString(data, DeviceStatic.SpecialId);

        Invert();
        AdjustSpecial();
    }

    public void SetNewValue(float value)
    {
        this.Value = value;
        this.date = DateTime.Now;
        Invert();
        AdjustSpecial();
    }

    private void Invert()
    {
        if (this.IsInverted)
            this.CorrectedValue = 100 - (float)(Value - LowerLimit) * 100.0f / (float)(UpperLimit - LowerLimit);
        else
            this.CorrectedValue = (float)(Value - LowerLimit) * 100.0f / (float)(UpperLimit - LowerLimit);
    }

    private void AdjustSpecial()
    {
        if (!String.IsNullOrEmpty(this.SpecialId))
        {
            switch (this.SpecialId)
            {
                case "brightness":
                    {
                        this.CorrectedValue /= 10;
                    }
                    break;
            }
        }
    }
}