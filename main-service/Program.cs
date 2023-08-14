using MainService.Hub;
using MainService.DB;
using System.Net;
using System.Net.Sockets;
using Keycloak.AuthServices.Authentication;
using dotenv.net;
using Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotEnv.Load();
var authenticationOptions = new KeycloakAuthenticationOptions
{
    AuthServerUrl = "https://auth.drunc.net/",
    Realm = Environment.GetEnvironmentVariable("AUTH_REALM")!,
    Resource = Environment.GetEnvironmentVariable("client")!,
};


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddKeycloakAuthentication(authenticationOptions);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(builder =>
        builder
            .WithOrigins("http://localhost:8080", $"http://{GetLocalIPAddress()}:8080", $"https://{GetLocalIPAddress()}:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
    //app.Urls.Add($"https://{GetLocalIPAddress()}:5082");
    app.Urls.Add($"https://localhost:5082");
    app.Urls.Add($"http://{GetLocalIPAddress()}:5082");
}
else
{
    app.UseCors(builder =>
        builder
            .WithOrigins("https://gardenos.drunc.net/", "https://gardenos.drunc.net")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<MainHub>("/hub");
app.UseRequestCulture();

MainDB.Init();

app.Run();

static string GetLocalIPAddress()
{
    return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault()?.ToString()!;
}