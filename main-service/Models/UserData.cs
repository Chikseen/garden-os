using System.Text.Json;
using System.Text.Json.Serialization;
using ExtensionMethods;
using MainService.DB;

public class UserData
{
    [JsonInclude]
    [JsonPropertyName("email_verified")]
    public bool EmailVerified = false;

    [JsonInclude]
    [JsonPropertyName("given_name")]
    public string GivenName = string.Empty;

    [JsonInclude]
    [JsonPropertyName("family_name")]
    public string FamilyName = string.Empty;

    [JsonInclude]
    [JsonPropertyName("preferred_username")]
    public string Id = string.Empty;

    [JsonInclude]
    [JsonPropertyName("gardenData")]
    public UserGardenData GardenData = null!;

    public List<string> GardenAccessList = new();

    public bool IsAuthorized = false;

    public UserData(string jsonString)
    {
        UserData? userData = JsonSerializer.Deserialize<UserData>(jsonString);

        if (userData is not null)
        {
            EmailVerified = userData.EmailVerified;
            GivenName = userData.GivenName;
            FamilyName = userData.FamilyName;
            Id = userData.Id;
            IsAuthorized = true;
        }
    }

    [JsonConstructor]
    public UserData() { }

    public void CheckGardenAccess(string gardenId)
    {
        if (GardenAccessList.Count < 1)
            SetGardenAccessList();

        if (!GardenAccessList.Contains(gardenId))
            throw new Exception("No Access");
    }

    public void AddGardenData(string gardenId)
    {
        string query = @$"
                SELECT
                    gardenuser.garden_id,
                    gardenuser.userrole_id
                FROM
                    users
                    JOIN gardenuser ON gardenuser.user_id = '{Id}'
                    AND users.id = '{Id}'
                    AND gardenuser.garden_id = '{gardenId}'".Clean();
        List<Dictionary<string, string>> results = MainDB.Query(query);
        Dictionary<string, string> user = results.FirstOrDefault()!;

        GardenData = new(
            DeviceStatic.GetString(user, DeviceStatic.GardenID),
            DeviceStatic.GetInt(user, DeviceStatic.UserRole, 0));
    }

    private void SetGardenAccessList()
    {
        string query = @$"
            SELECT
                garden_id
            FROM
                gardenUser
            WHERE
                user_id = '{Id}'";
        List<Dictionary<string, string>> results = MainDB.Query(query);

        foreach (var item in results)
        {
            GardenAccessList.Add(DeviceStatic.GetString(item, DeviceStatic.GardenID));
        }
    }
}

public record UserGardenData(string GardenId, int UserRole);
