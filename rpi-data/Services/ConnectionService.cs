
using System.Text.Json;
using dotenv.net;
using MainService.Hardware;
using Microsoft.AspNetCore.SignalR.Client;

namespace RPI.Connection
{
    public class Connection
    {
        private Microsoft.AspNetCore.SignalR.Client.HubConnection _hubConnection { get; set; }

        public Connection()
        {
            _hubConnection = Connect();
            setRpiAndConId();
        }

        private Microsoft.AspNetCore.SignalR.Client.HubConnection Connect()
        {
            try
            {
                DotEnv.Load();
                Console.WriteLine("Try Connect");
                _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Environment.GetEnvironmentVariable("URL")!}/hub")
                .WithAutomaticReconnect()
                .Build();

                _hubConnection.On("SendRebootRequest", () =>
                    {
                        Console.WriteLine("Reboot request recvied");

                        DeveiceStatus status = new()
                        {
                            RpiId = MainHardware.RpiId,
                            Message = "Manuel Reboot triggerd",
                            Status = "reboot",
                            TriggerdBy = "hub"
                        };

                        ApiService api = new();
                        api.Post($"/devices/status", JsonSerializer.Serialize(status));

                        SystemService.Reboot();
                    });
                _hubConnection.On<string>("SendMyEvent", (user) =>
                    {
                        GetMessage(user, "message");
                    });


                _hubConnection.StartAsync().Wait();

                _hubConnection.Closed += HubConnection_Closed;
                _hubConnection.Reconnected += HubConnection_Reconnected;

                return _hubConnection;
            }
            catch (System.Exception)
            {
                DeveiceStatus status = new()
                {
                    RpiId = MainHardware.RpiId,
                    Message = "Not able to create Hub Connection",
                    Status = "error",
                    TriggerdBy = "hub"
                };

                ApiService api = new();
                api.Post($"/devices/status", JsonSerializer.Serialize(status));

                SystemService.Reboot();
                return Connect();
            }
        }

        void GetMessage(string user, string message)
        {
            Console.WriteLine(user);
            Console.WriteLine(message);
        }

        void setRpiAndConId()
        {
            DotEnv.Load();
            String rpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
            _hubConnection.SendAsync("SetRpiConnectionId", rpiId);
        }

        static Task HubConnection_Closed(Exception? exception)
        {
            // Handle connection closed event
            Console.WriteLine("Connection closed.");

            if (exception != null)
            {
                // Handle connection error
                Console.WriteLine($"Connection error: {exception.Message}");
            }

            DeveiceStatus status = new()
            {
                RpiId = MainHardware.RpiId,
                Message = "Connection: Connection closed: " + exception,
                Status = "error",
                TriggerdBy = "hub"
            };

            ApiService api = new();
            api.Post($"/devices/status", JsonSerializer.Serialize(status));

            return Task.CompletedTask;
        }

        Task HubConnection_Reconnected(string? arg)
        {
            DeveiceStatus status = new()
            {
                RpiId = MainHardware.RpiId,
                Message = "Connection: Connection reconnected ",
                Status = "warning",
                TriggerdBy = "hub"
            };

            ApiService api = new();
            api.Post($"/devices/status", JsonSerializer.Serialize(status));

            Console.WriteLine("Reconnected");
            setRpiAndConId();
            return Task.CompletedTask;
        }
    }
}

