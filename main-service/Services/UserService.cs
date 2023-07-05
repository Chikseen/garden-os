using System.Net.Http.Headers;
using ExtensionMethods;
using MainService.DB;
using Microsoft.Extensions.Primitives;

namespace Services.User
{
    public class UserService
    {
        private static Random random = new Random();

        public String? GetApiKey(HttpRequest Request)
        {
            Dictionary<string, string> requestHeaders = new();
            StringValues token = String.Empty;
            Request.Headers.TryGetValue("Authorization", out token);

            if (token == String.Empty)
                return null;

            String apiKey = ((String)token!).Replace("Bearer", "").Trim();
            return apiKey;
        }

        public async Task<UserData> GetUserDataFromKeycloak(HttpRequest Request)
        {
            Dictionary<string, string> data = new()
            {
                {"grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"},
                {"audience", "dev-client-be"},
            };
            Request.Headers.TryGetValue("Authorization", out StringValues bearer);
            if (StringValues.IsNullOrEmpty(bearer))
                throw new UnauthorizedAccessException("Accesstoken not found");

            String token = ((String)bearer!).Replace("Bearer", "").Trim();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync("https://auth.drunc.net/realms/GardenOS-DEV/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));
            var contents = await response.Content.ReadAsStringAsync();

            return new UserData(contents);
        }

        public GardenResponseModel GetGardenData(UserData userData)
        {
            String query = @$"
                SELECT
                    users.given_name,
                    users.family_name,
                    users.id,
                    garden.weather_location_id,
                    garden.name AS garden_name,
                    garden.id AS garden_id
                FROM
                    gardenuser
                    JOIN garden ON gardenuser.garden_id = garden.id
                    JOIN users ON gardenuser.user_id = '{userData.Id}'
                    AND users.id = '{userData.Id}';".Clean();
            List<Dictionary<String, String>> results = MainDB.query(query);

            if (results.Count < 1)
                throw new Exception("Gardendata not found");

            GardenResponseModel reponse = new(results);

            return reponse;
        }

        public void SaveNewUser(UserData user)
        {
            String query = @$"
                INSERT INTO USERS (
                    ID,
                    FAMILY_NAME,
                    GIVEN_NAME
                ) VALUES (
                    '{user.Id}',
                    '{user.FamilyName}',
                    '{user.GivenName}'
                ) ON CONFLICT DO NOTHING;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);
        }
    }
}