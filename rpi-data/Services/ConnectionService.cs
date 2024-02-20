
using System.Text.Json;
using dotenv.net;
using MainService.Hardware;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;

namespace RPI.Connection
{
    public class Connection
    {
        private HubConnection HubConnection { get; set; }

        public Connection()
        {
            HubConnection = Connect();
            SetRpiAndConId();
        }

        HubConnection Connect()
        {
            try
            {
                DotEnv.Load();
                Console.WriteLine("Try Connect");
                HubConnection = new HubConnectionBuilder()
                .WithUrl($"{Environment.GetEnvironmentVariable("URL")!}/hub")
                .WithAutomaticReconnect()
                .Build();

                HubConnection.On("SendRebootRequest", () =>
                    {
                        Console.WriteLine("Reboot request recvied");

                        DeveiceStatus status = new()
                        {
                            RpiId = MainHardware._RpiId,
                            Message = "Manuel Reboot triggerd",
                            Status = "warning",
                            TriggerdBy = "hub"
                        };

                        ApiService api = new();
                        api.Post($"/devices/status", JsonSerializer.Serialize(status));

                        SystemService.Reboot();
                    });

                HubConnection.On("NewVersion", () =>
                    {
                        Console.WriteLine("New Version avaliable");

                        DeveiceStatus status = new()
                        {
                            RpiId = MainHardware._RpiId,
                            Message = "Update Hub",
                            Status = "warning",
                            TriggerdBy = "hub"
                        };

                        ApiService api = new();
                        api.Post($"/devices/status", JsonSerializer.Serialize(status));

                        //SystemService.Reboot();
                    });

                HubConnection.On<string>("SendMyEvent", (user) =>
                    {
                        GetMessage(user, "message");
                    });


                HubConnection.StartAsync().Wait();

                HubConnection.Closed += HubConnection_Closed;
                HubConnection.Reconnected += HubConnection_Reconnected;

                return HubConnection;
            }
            catch (Exception)
            {
                DeveiceStatus status = new()
                {
                    RpiId = MainHardware._RpiId,
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

        void SetRpiAndConId()
        {
            DotEnv.Load();
            string rpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
            HubConnection.SendAsync("SetRpiConnectionId", rpiId);
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
                RpiId = MainHardware._RpiId,
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
                RpiId = MainHardware._RpiId,
                Message = "Connection reconnected ",
                Status = "info",
                TriggerdBy = "hub"
            };

            ApiService api = new();
            api.Post($"/devices/status", JsonSerializer.Serialize(status));

            Console.WriteLine("Reconnected");
            SetRpiAndConId();
            return Task.CompletedTask;
        }
    }
}

