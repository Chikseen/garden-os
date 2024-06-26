using dotenv.net;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Net.Http.Headers;

namespace Middleware;

public class AuthMiddleware(
        RequestDelegate next)
{

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value!;

        if (path.Contains("/user") || path.Contains("/garden") || path.Contains("/manualData"))
            await GetUserData(context);

        else if (path.Contains("/devices"))
        {
            // Do auth check here later like for user but need to refine device controller
        }

        await _next(context);
    }

    private static async Task ReturnErrorResponse(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.StartAsync();
    }

    private static async Task GetUserData(HttpContext context)
    {
        if (context.Request.RouteValues.ContainsKey("gardenId")
            && string.IsNullOrWhiteSpace(context.Request.RouteValues["gardenId"]!.ToString())
            && context.Request.RouteValues["gardenId"]! == null)
        {
            await ReturnErrorResponse(context);
            return;
        }

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

        var response = await client.PostAsync($"https://auth.garden-os.com/realms/{realm}/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));

        var contents = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(contents))
        {
            await ReturnErrorResponse(context);
            return;
        }

        UserData userData = new(contents);
        if (userData == null)
        {
            await ReturnErrorResponse(context);
            return;
        }

        if (context.Request.RouteValues.ContainsKey("gardenId"))
            userData.CheckGardenAccess(context.Request.RouteValues["gardenId"]!.ToString()!);

        context.Features.Set(userData);
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
