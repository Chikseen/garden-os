using System.Text.Json;
using MainService.Hardware;
using RPI.Connection;
using Shared.Models;

Console.WriteLine("--- Garden OS ---");
new Connection();

try
{
    MainHardware.Init();
}
catch (Exception e)
{
    DeveiceStatus status = new()
    {
        RpiId = MainHardware._RpiId,
        Message = "RPI Exeption: Unexpected void was reached: " + e,
        Status = "error",
        TriggerdBy = "hub"
    };

    ApiService api = new();
    api.Post($"/devices/status", JsonSerializer.Serialize(status));
    SystemService.Reboot();
}

SystemService.Reboot();