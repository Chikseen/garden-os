using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class UserList
    {
        [JsonInclude]
        [JsonPropertyName("userList")]
        public List<User> User = new();

        public UserList(List<Dictionary<string, string>> data)
        {
            List<User> users = new();
            foreach (Dictionary<string, string> user in data)
            {
                users.Add(new User(user));
            }
            User = users;
        }

        [JsonConstructor]
        public UserList() { }
    }

    public class User
    {
        [JsonInclude]
        [JsonPropertyName("user_id")]
        public string UserId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("garden_id")]
        public string GardenId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("userrole_id")]
        public int UserRole = -10;

        [JsonInclude]
        [JsonPropertyName("given_name")]
        public string GivenName = string.Empty;

        [JsonInclude]
        [JsonPropertyName("family_name")]
        public string FamilyName = string.Empty;

        public User(Dictionary<string, string> data)
        {
            UserId = DeviceStatic.GetString(data, DeviceStatic.UserId);
            GardenId = DeviceStatic.GetString(data, DeviceStatic.GardenID);
            UserRole = DeviceStatic.GetInt(data, DeviceStatic.UserRole, 0);
            GivenName = DeviceStatic.GetString(data, DeviceStatic.GivenName);
            FamilyName = DeviceStatic.GetString(data, DeviceStatic.FamilyName);
        }

        [JsonConstructor]
        public User() { }
    }
}