using System.Net;
using System.Net.Http.Headers;
using dotenv.net;
using ExtensionMethods;
using MainService.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Services.User
{
    public class UserService
    {
        private static Random random = new Random();

        public String GetApiKey(HttpRequest Request)
        {
            Dictionary<string, string> requestHeaders = new();
            StringValues token = String.Empty;
            Request.Headers.TryGetValue("Authorization", out token);

            if (token == String.Empty)
                throw new Exception("RPI not registerd");

            String apiKey = ((String)token!).Replace("Bearer", "").Trim();
            return apiKey;
        }

        public async Task<UserData> GetUserDataFromKeycloak(HttpRequest Request)
        {
            DotEnv.Load();
            String realm = Environment.GetEnvironmentVariable("AUTH_REALM")!;
            String clientId = Environment.GetEnvironmentVariable("AUTH_CLIENT_ID")!;

            Dictionary<string, string> data = new()
            {
                {"grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"},
                {"audience", clientId},
            };
            Request.Headers.TryGetValue("Authorization", out StringValues bearer);
            if (StringValues.IsNullOrEmpty(bearer))
                throw new UnauthorizedAccessException("Accesstoken not found");

            String token = ((String)bearer!).Replace("Bearer", "").Trim();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"https://auth.drunc.net/realms/{realm}/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));
            var contents = await response.Content.ReadAsStringAsync();

            if (String.IsNullOrEmpty(contents))
                return new();
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
                    AND users.id = '{userData.Id}'
                    AND gardenuser.is_approved = true;".Clean();
            List<Dictionary<String, String>> results = MainDB.query(query);

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

        public UserList GetUserList(UserData user, String gardenId)
        {
            String query = @$"
                SELECT
                    garden_id,
                    user_id,
                    is_approved
                FROM
                    gardenuser
                WHERE
                    garden_id = '{gardenId}'".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            return new(result);
        }

        public UserList AccessRequest(UserData user, String gardenId)
        {
            String query = @$"
                INSERT INTO GARDENUSER (
                   garden_id,
                   user_id,
                   is_approved
                ) VALUES (
                    '{gardenId}',
                    '{user.Id}',
                    false
                ) ON CONFLICT DO NOTHING;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            return new(result);
        }

        public List<String> GetRequestedGardenList(UserData user)
        {
            String query = @$"
                SELECT
                    garden_id
                FROM
                    gardenUser
                WHERE
                    user_id = '{user.Id}'
                    AND is_approved = false;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            List<String> list = new();
            foreach (var item in result)
            {
                list.Add(item[DeviceStatic.GardenID]);
            }

            return list;
        }

        public UserList Changestatus(String gardenId, String userId)
        {
            String query = @$"
                UPDATE  gardenuser
                SET is_approved = NOT is_approved 
                WHERE garden_id = '{gardenId}'
                AND user_id = '{userId}'".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            return new(result);
        }
    }
}