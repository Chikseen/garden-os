using System.Text.Json;
using MainService.Hardware;
using RPI.Connection;

Console.WriteLine("--- Garden OS ---");
Connection connection = new Connection();

try
{
    MainHardware.Init();
    DeveiceStatus statusE = new()
    {
        RpiId = MainHardware.RpiId,
        Message = "debug 1",
        Status = "error",
        TriggerdBy = "hub"
    };

    ApiService apiE = new();
}
catch (System.Exception e)
{
    DeveiceStatus statusE = new()
    {
        RpiId = MainHardware.RpiId,
        Message = "RPI Exeption: Unexpected void was reached: " + e,
        Status = "error",
        TriggerdBy = "hub"
    };

    ApiService apiE = new();
    apiE.Post($"/devices/status", JsonSerializer.Serialize(statusE));
    SystemService.Reboot();
}

DeveiceStatus status = new()
{
    RpiId = MainHardware.RpiId,
    Message = "RPI Exeption: Unexpected void was reached 222: ",
    Status = "error",
    TriggerdBy = "hub"
};

ApiService api = new();
api.Post($"/devices/status", JsonSerializer.Serialize(status));
SystemService.Reboot();