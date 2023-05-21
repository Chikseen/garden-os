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

            String apiKey = ((String)token!).Replace("Bearer ", "");
            return apiKey;
        }
    }
}