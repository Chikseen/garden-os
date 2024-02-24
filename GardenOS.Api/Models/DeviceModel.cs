using System.Text.Json.Serialization;
using Shared.Models;

public class DeviceModel
{
    [JsonInclude]
    public string Name = string.Empty;

    [JsonInclude]
    public string Id = string.Empty;

    [JsonInclude]
    public string DisplayId = string.Empty;

    [JsonInclude]
    public int Value = 0;

    [JsonInclude]
    public int? UpperLimit = null;

    [JsonInclude]
    public int? LowerLimit = null;

    public int? SerialId = 0;
    public string DeviceTyp = string.Empty;
    public byte Address = 0x00;
    public TimeSpan DataUpdateInterval = new(24, 24, 24);
    public DateTime LastEntry = DateTime.Now.AddYears(-1);

    public DeviceModel() { }

    public DeviceModel(Dictionary<string, string> deviceDictionary)
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

public class DeviceCache
{
    public DateTime Date = DateTime.Now;
    public float Value = 0;

    public DeviceCache(float value)
    {
        Value = value;
    }
    public DeviceCache()
    { }
}