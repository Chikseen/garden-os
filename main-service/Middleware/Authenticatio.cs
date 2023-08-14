using System.Net;
using System.Net.Http.Headers;
using dotenv.net;
using Microsoft.Extensions.Primitives;

namespace Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value!;

        if (path.Contains("/user/") || path.Contains("/garden/"))
        {
            DotEnv.Load();
            string realm = Environment.GetEnvironmentVariable("AUTH_REALM")!;
            string clientId = Environment.GetEnvironmentVariable("AUTH_CLIENT_ID")!;

            Dictionary<string, string> data = new()
            {
                {"grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"},
                {"audience", clientId},
            };
            context.Request.Headers.TryGetValue("Authorization", out StringValues bearer);
            if (StringValues.IsNullOrEmpty(bearer))
            {
                await ReturnErrorResponse(context);
                return;
            }

            string token = ((string)bearer!).Replace("Bearer", "").Trim();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"https://auth.drunc.net/realms/{realm}/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));
            var contents = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(contents))
            {
                await ReturnErrorResponse(context);
                return;
            }
            context.Features.Set(new UserData(contents));
        }
        //put auth here
        await _next(context);
    }

    private static async Task ReturnErrorResponse(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.StartAsync();
    }
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}
