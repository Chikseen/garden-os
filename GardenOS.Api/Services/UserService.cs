using System.Net.Http.Headers;
using dotenv.net;
using ExtensionMethods;
using MainService.DB;
using Microsoft.Extensions.Primitives;
using Shared.Models;

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
                    AND gardenuser.userrole_id >= 0;".Clean();
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
                    gardenuser.userrole_id,
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
                   userrole_id
                ) VALUES (
                    '{gardenId}',
                    '{user.Id}',
                    -10
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
                    AND userrole_id < 0;".Clean();
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<string> list = new();
            foreach (var item in result)
            {
                list.Add(item[DeviceStatic.GardenID]);
            }

            return list;
        }

        public UserList Changestatus(ChangeUserStatusModel model)
        {
            string query = @$"
                UPDATE 
                    gardenuser
                SET 
                    userrole_id = '{model.Role}' 
                WHERE 
                    garden_id = '{model.GardenId}'
                AND 
                    user_id = '{model.UserId}'".Clean();
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