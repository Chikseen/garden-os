using System.Text.Json;
using System.Text.Json.Serialization;
using MainService.DB;

public class UserData
{
    [JsonInclude]
    [JsonPropertyName("email_verified")]
    public Boolean EmailVerified = false;

    [JsonInclude]
    [JsonPropertyName("given_name")]
    public String GivenName = String.Empty;

    [JsonInclude]
    [JsonPropertyName("family_name")]
    public String FamilyName = String.Empty;

    [JsonInclude]
    [JsonPropertyName("preferred_username")]
    public String Id = String.Empty;

    public Boolean IsValid = false;

    public List<String> GardenAccessList = new();

    public UserData(String jsonString)
    {
        try
        {
            UserData? userData = JsonSerializer.Deserialize<UserData>(jsonString);

            if (userData is not null)
            {
                this.EmailVerified = userData.EmailVerified;
                this.GivenName = userData.GivenName;
                this.FamilyName = userData.FamilyName;
                this.Id = userData.Id;
                this.IsValid = true;
            }
            else
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
        }
        catch (System.Exception)
        {
            throw new UnauthorizedAccessException("Unauthorized");
            throw;
        }
    }

    [JsonConstructor]
    public UserData() { }

    public void CheckGardenAccess(String gardenId)
    {
        if (GardenAccessList.Count < 1)
            this.SetGardenAccessList();

        if (!GardenAccessList.Contains(gardenId))
            throw new Exception("No Access");
    }

    private void SetGardenAccessList()
    {
        String query = @$"
            SELECT
                garden_id
            FROM
                gardenUser
            WHERE
                user_id = 't@t.de'";
        List<Dictionary<String, String>> results = MainDB.query(query);

        foreach (var item in results)
        {
            GardenAccessList.Add(DeviceStatic.GetString(item, DeviceStatic.GardenID));
        }

    }
}