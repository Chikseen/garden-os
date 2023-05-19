using System.Text.Json.Serialization;

public class Device
{
    [JsonInclude]
    public String Name = String.Empty;

    [JsonInclude]
    public String Id = String.Empty;

    [JsonInclude]
    public String DisplayId = String.Empty;

    [JsonInclude]
    public int Value = 0;

    [JsonInclude]
    public int? UpperLimit = null;

    [JsonInclude]
    public int? LowerLimit = null;
    
    public int? SerialId = 0;
    public String DeviceTyp = String.Empty;
    public Byte Address = 0x00;
    public TimeSpan DataUpdateInterval = new TimeSpan(24, 24, 24);
    public DateTime LastEntry = DateTime.Now.AddYears(-1);

    public Device(Dictionary<String, String> deviceDictionary)
    {
        Name = DeviceStatic.GetString(deviceDictionary, DeviceStatic.DeviceName);
        Id = DeviceStatic.GetString(deviceDictionary, DeviceStatic.DeviceId);
        DisplayId = DeviceStatic.GetString(deviceDictionary, DeviceStatic.DisplayId);
        UpperLimit = DeviceStatic.GetInt(deviceDictionary, DeviceStatic.UpperLimit);
        LowerLimit = DeviceStatic.GetInt(deviceDictionary, DeviceStatic.LowerLimit);
        DeviceTyp = DeviceStatic.GetString(deviceDictionary, DeviceStatic.DeviceTyp);
        Address = DeviceStatic.GetByte(deviceDictionary, DeviceStatic.Address);
        SerialId = DeviceStatic.GetInt(deviceDictionary, DeviceStatic.SerialId);
        DataUpdateInterval = DeviceStatic.GetTimeSpan(deviceDictionary, DeviceStatic.DataUpdateInterval);
    }
}