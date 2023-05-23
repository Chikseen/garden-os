using ExtensionMethods;
using MainService.DB;
using Microsoft.Extensions.Primitives;

namespace Services.User
{
    public class UserService
    {
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

            if (result[0].ContainsKey("count") && result[0]["count"] == "1")
                return true;

            return false;
        }
    }
}