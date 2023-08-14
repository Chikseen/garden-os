using System.Net.Http.Headers;
using dotenv.net;
using ExtensionMethods;
using MainService.DB;
using Microsoft.Extensions.Primitives;

namespace Services.User
{
    public class UserService
    {
        public string GetApiKey(HttpRequest Request)
        {
            Request.Headers.TryGetValue("Authorization", out StringValues token);

            if (token == string.Empty)
                throw new Exception("RPI not registerd");

            string apiKey = ((string)token!).Replace("Bearer", "").Trim();
            return apiKey;
        }

        public async Task<UserData> GetUserDataFromKeycloak(HttpRequest Request)
        {
            DotEnv.Load();
            string realm = Environment.GetEnvironmentVariable("AUTH_REALM")!;
            string clientId = Environment.GetEnvironmentVariable("AUTH_CLIENT_ID")!;

            Dictionary<string, string> data = new()
            {
                {"grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"},
                {"audience", clientId},
            };
            Request.Headers.TryGetValue("Authorization", out StringValues bearer);
    

            string token = ((string)bearer!).Replace("Bearer", "").Trim();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"https://auth.drunc.net/realms/{realm}/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));
            var contents = await response.Content.ReadAsStringAsync();

            return new UserData(contents);
        }

        public GardenResponseModel GetGardenData(UserData userData)
        {
            string query = @$"
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
            List<Dictionary<string, string>> results = MainDB.Query(query);

            GardenResponseModel reponse = new(results);

            return reponse;
        }

        public void SaveNewUser(UserData user)
        {
            string query = @$"
                INSERT INTO USERS (
                    ID,
                    FAMILY_NAME,
                    GIVEN_NAME
                ) VALUES (
                    '{user.Id}',
                    '{user.FamilyName}',
                    '{user.GivenName}'
                ) ON CONFLICT DO NOTHING;".Clean();
            MainDB.Query(query);
        }

        public UserList GetUserList(string gardenId)
        {
            string query = @$"
                SELECT
                    gardenuser.garden_id,
                    gardenuser.user_id,
                    gardenuser.is_approved,
                    users.given_name,
                    users.family_name
                FROM
                    gardenuser
                    JOIN users ON gardenuser.user_id = users.id
                    AND garden_id = '{gardenId}';".Clean();
            List<Dictionary<string, string>> result = MainDB.Query(query);

            return new(result);
        }

        public UserList AccessRequest(UserData user, string gardenId)
        {
            string query = @$"
                INSERT INTO GARDENUSER (
                   garden_id,
                   user_id,
                   is_approved
                ) VALUES (
                    '{gardenId}',
                    '{user.Id}',
                    false
                ) ON CONFLICT DO NOTHING;".Clean();
            List<Dictionary<string, string>> result = MainDB.Query(query);

            return new(result);
        }

        public List<string> GetRequestedGardenList(UserData user)
        {
            string query = @$"
                SELECT
                    garden_id
                FROM
                    gardenUser
                WHERE
                    user_id = '{user.Id}'
                    AND is_approved = false;".Clean();
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<string> list = new();
            foreach (var item in result)
            {
                list.Add(item[DeviceStatic.GardenID]);
            }

            return list;
        }

        public UserList Changestatus(string gardenId, string userId)
        {
            string query = @$"
                UPDATE  gardenuser
                SET is_approved = NOT is_approved 
                WHERE garden_id = '{gardenId}'
                AND user_id = '{userId}'".Clean();
            List<Dictionary<string, string>> result = MainDB.Query(query);

            return new(result);
        }

        public List<string> GetBridges(string gardenId)
        {
            string query = @$"
                SELECT 
                    id 
                FROM 
                    rpis
                WHERE 
                    garden_id = '{gardenId}'".Clean();
            List<Dictionary<string, string>> results = MainDB.Query(query);

            return results.Select(b => b["id"]).ToList();
        }
    }
}