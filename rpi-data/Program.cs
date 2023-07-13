using MainService.Hardware;
using RPI.Connection;

Console.WriteLine("--- Garden OS ---");
Connection connection = new Connection();
MainHardware.Init(connection);