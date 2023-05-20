public static class DeviceStatic
{
    public readonly static String DeviceId = "device_id";
    public readonly static String LogId = "logid";
    public readonly static String RawValue = "rawvalue";
    public readonly static String Date = "logdate";
    public readonly static String DeviceName = "device_name";
    public readonly static String UpperLimit = "upper_limit";
    public readonly static String LowerLimit = "lower_limit";
    public readonly static String DeviceTyp = "device_typ";
    public readonly static String Address = "address";
    public readonly static String SerialId = "serial_id";
    public readonly static String ADC7080 = "i2c_adc_7080";
    public readonly static String DisplayId = "display_id";
    public readonly static String DataUpdateInterval = "data_update_interval";
    public readonly static String LastEntry = "logdate";
    public readonly static String RPIID = "rpi_id";
    public readonly static String GardenID = "garden_id";
    public readonly static String Name = "name";

    public static String? GetString(Dictionary<String, String> dict, String key, bool allowNull = true)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        return null;
    }

    public static String GetString(Dictionary<String, String> dict, String key)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        return "";
    }

    public static int? GetInt(Dictionary<String, String> dict, String key)
    {
        if (dict.ContainsKey(key) && dict[key] != String.Empty)
        {
            return null;
        }
        return null;
    }

    public static Byte GetByte(Dictionary<String, String> dict, String key)
    {
        if (dict.ContainsKey(key) && dict[key] != String.Empty)
        {
            return Convert.ToByte(dict[key], 16);
        }
        return 0x00;
    }

    public static TimeSpan GetTimeSpan(Dictionary<String, String> dict, String key)
    {
        if (dict.ContainsKey(key) && dict[key] != String.Empty)
        {
            return TimeSpan.Parse(dict[key]);
        }
        return new TimeSpan(0, 10, 0);
    }
}