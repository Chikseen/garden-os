using API.Interfaces;
using dotenv.net;
using ESP_sensor.Models;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Middleware;

public class AuthMiddleware(
        RequestDelegate next,
        IDeviceService _standaloneService)
{

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value!;

        if (path.Contains("/user") || path.Contains("/garden") || path.Contains("/manualData"))
            await GetUserData(context);

        else if (path.Contains("/standalone"))
            await CheckStandaloneDevice(context);

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

        var response = await client.PostAsync($"https://auth.drunc.net/realms/{realm}/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));

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

    private async Task CheckStandaloneDevice(HttpContext context)
    {
        StandaloneDevice body = await GetBody<StandaloneDevice>(context.Request);
        if (body == null)
        {
            await ReturnErrorResponse(context);
            return;
        }

        if (!_standaloneService.IsCredentialsValid(body))
        {
            await ReturnErrorResponse(context);
            return;
        }

        context.Features.Set(body);
    }

    private static async Task<T?> GetBody<T>(HttpRequest request)
    {
        if (request.Method == HttpMethods.Post && request.ContentLength > 0)
        {
            request.EnableBuffering();
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestContent = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
            T body = JsonSerializer.Deserialize<T>(requestContent)!;
            return body;
        }
        return default;
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
