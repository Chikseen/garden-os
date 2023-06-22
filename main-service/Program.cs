using MainService.Hub;
using MainService.DB;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();

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

app.UseAuthorization();
app.MapControllers();
app.MapHub<MainHub>("/hub");

MainDB.Init();

app.Run();

static string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    throw new Exception("No network adapters with an IPv4 address in the system!");
}