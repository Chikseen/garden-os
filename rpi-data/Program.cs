using System.Text.Json;
using MainService.Hardware;
using RPI.Connection;

Console.WriteLine("--- Garden OS ---");
Connection connection = new Connection();

try
{
    MainHardware.Init();
    DeveiceStatus status = new()
    {
        RpiId = MainHardware.RpiId,
        Message = "debug 1",
        Status = "error",
        TriggerdBy = "hub"
    };

    ApiService api = new();
}
catch (System.Exception e)
{
    DeveiceStatus status = new()
    {
        RpiId = MainHardware.RpiId,
        Message = "RPI Exeption: Unexpected void was reached: " + e,
        Status = "error",
        TriggerdBy = "hub"
    };

    ApiService api = new();
    api.Post($"/devices/status", JsonSerializer.Serialize(status));
    SystemService.Reboot();
}