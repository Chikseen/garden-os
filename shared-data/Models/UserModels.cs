using System.Text.Json.Serialization;

public class UserList
{
    [JsonInclude]
    [JsonPropertyName("userList")]
    public List<User> User = new();

    public UserList(List<Dictionary<String, String>> data)
    {
        List<User> users = new();
        foreach (Dictionary<String, String> user in data)
        {
            users.Add(new User(user));
        }
        this.User = users;
    }

    [JsonConstructor]
    public UserList() { }
}

public class User
{
    [JsonInclude]
    [JsonPropertyName("user_id")]
    public String UserId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("garden_id")]
    public String GardenId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("isApproved")]
    public Boolean IsApproved = false;

    public User(Dictionary<String, String> data)
    {
        this.UserId = DeviceStatic.GetString(data, DeviceStatic.UserId);
        this.GardenId = DeviceStatic.GetString(data, DeviceStatic.GardenID);
        this.IsApproved = DeviceStatic.GetBool(data, DeviceStatic.IsApproved);
    }

    [JsonConstructor]
    public User() { }
}