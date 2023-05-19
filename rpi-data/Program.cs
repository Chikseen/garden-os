using MainService.Hardware;
using RPI.Connection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:8080", "http://localhost:8081", "http://192.168.1.100:8080", "http://93.201.163.148:8080")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

Connection connection = new Connection();
MainHardware.Init(connection);

app.Run();
