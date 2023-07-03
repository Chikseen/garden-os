using System.Text.Json;
using System.Text.Json.Serialization;

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

    public UserData(String jsonString)
    {
        UserData? userData = JsonSerializer.Deserialize<UserData>(jsonString);

        if (userData is not null)
        {
            this.EmailVerified = userData.EmailVerified;
            this.GivenName = userData.GivenName;
            this.FamilyName = userData.FamilyName;
            this.Id = userData.Id;
        }
    }

    [JsonConstructor]
    public UserData() { }
}