namespace Shared.Models
{
    public static class DeviceStatic
    {
        public readonly static string DeviceId = "device_id";
        public readonly static string SensorId = "sensor_id";
        public readonly static string LogId = "logid";
        public readonly static string RawValue = "rawvalue";
        public readonly static string Value = "value";
        public readonly static string LogDate = "logdate";
        public readonly static string UploadDate = "upload_date";
        public readonly static string Date = "date";
        public readonly static string DeviceName = "device_name";
        public readonly static string UpperLimit = "upper_limit";
        public readonly static string LowerLimit = "lower_limit";
        public readonly static string DeviceTyp = "device_typ";
        public readonly static string Address = "address";
        public readonly static string SerialId = "serial_id";
        public readonly static string ADC = "ADC";
        public readonly static string DHT11 = "DHT11";
        public readonly static string DisplayId = "display_id";
        public readonly static string DataUpdateInterval = "data_update_interval";
        public readonly static string LastEntry = "logdate";
        public readonly static string RPIID = "rpi_id";
        public readonly static string GardenID = "garden_id";
        public readonly static string Name = "name";
        public readonly static string ID = "id";
        public readonly static string JSON = "json";
        public readonly static string UserName = "user_name";
        public readonly static string UserId = "user_id";
        public readonly static string GivenName = "given_name";
        public readonly static string FamilyName = "family_name";
        public readonly static string GardenName = "garden_name";
        public readonly static string WeatherLocationId = "weather_location_id";
        public readonly static string IsInverted = "is_inverted";
        public readonly static string ApiKey = "api_key";
        public readonly static string SortOrder = "sort_order";
        public readonly static string GroupId = "group_id";
        public readonly static string Unit = "unit";
        public readonly static string TriggerdBy = "triggerd_by";
        public readonly static string Status = "status";
        public readonly static string Message = "message";
        public readonly static string SpecialId = "special_id";
        public readonly static string Build = "build";
        public readonly static string UserRole = "userrole_id";

        public static string? GetString(Dictionary<string, string> dict, string key, bool allowNull = true)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return null;
        }

        public static string GetString(Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return "";
        }

        public static int? GetInt(Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return int.Parse(dict[key]);
            }
            return null;
        }
        public static int GetInt(Dictionary<string, string> dict, string key, int defaultValue)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return int.Parse(dict[key]);
            }
            return defaultValue;
        }

        public static float GetFloat(Dictionary<string, string> dict, string key, float defaultValue = 0.0f)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return float.Parse(dict[key]);
            }
            return defaultValue;
        }

        public static bool GetBool(Dictionary<string, string> dict, string key, bool defaultValue = false)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return bool.Parse(dict[key]);
            }
            return defaultValue;
        }

        public static byte GetByte(Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return Convert.ToByte(dict[key], 16);
            }
            return 0x00;
        }

        public static TimeSpan GetTimeSpan(Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return TimeSpan.Parse(dict[key]);
            }
            return new TimeSpan(0, 10, 0);
        }

        public static DateTime GetDateTime(Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != string.Empty)
            {
                return DateTime.Parse(dict[key]);
            }
            return DateTime.Now;
        }
    }
}