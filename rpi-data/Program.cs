using MainService.Hardware;
using RPI.Connection;

Console.WriteLine("--- Garden OS ---");
Connection connection = new Connection();
MainHardware.Init();
DeveiceStatus status = new()
{
    RpiId = MainHardware.RpiId,
    Message = "debug 1",
    Status = "error",
    TriggerdBy = "hub"
};

ApiService api = new();
SystemService.Reboot();