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

        public Boolean ValidateUser(String id, String apiKey)
        {
            String query = @$"
                SELECT Count(*)
                FROM USERS
                    WHERE ID = '{id}'
                    AND API_KEY = '{apiKey}'".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count != 0 && result[0].ContainsKey("count") && result[0]["count"] == "1")
                return true;

            return false;
        }

        public GardenResponseModel? GetGardenData(String id, String apiKey)
        {
            String query = @$"
                SELECT 
                    USERS.NAME as USER_NAME, 
                    USERS.ID as GARDEN_ID, 
                    GARDEN.NAME as GARDEN_NAME,
                    GARDEN.weather_location_id
                FROM USERS
                JOIN GARDEN
                    ON USERS.ID = '{id}'
                    AND USERS.API_KEY = '{apiKey}'
                    AND GARDEN.ID = USERS.GARDEN_ID".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count != 1)
                return null;

            return new GardenResponseModel(result);
        }

        public CreateNewUserResponse CreateNewUser(CreateNewuserRequest garden)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            String apiKey = new string(Enumerable.Repeat(chars, 128)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            String query = @$"
                INSERT INTO USERS (
                    ID,
                    NAME,
                    API_KEY,
                    GARDEN_ID
                ) VALUES (
                    GEN_RANDOM_UUID(),
                    '{garden.UserName}',
                    '{apiKey}',
                    '{garden.GardenId}'
                )
                RETURNING ID AS USER_ID, API_KEY;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);
            return new CreateNewUserResponse(result);
        }
    }
}