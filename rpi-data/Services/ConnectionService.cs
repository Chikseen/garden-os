
using System;
using System.Diagnostics;
using dotenv.net;
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
            Console.WriteLine("Try Connect");
            _hubConnection = new HubConnectionBuilder()
             .WithUrl("https://gardenapi.drunc.net/hub")
            .WithAutomaticReconnect()
            .Build();

            _hubConnection.On("SendRebootRequest", () =>
                {
                    Console.WriteLine("Reboot request recvied");
                    try
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "reboot" });
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            _hubConnection.On<string>("SendMyEvent", (user) =>
                {
                    GetMessage(user, "message");
                });

            _hubConnection.Closed += (exception) =>
            {
                if (exception == null)
                {
                    Console.WriteLine("Connection closed without error.");
                }
                else
                {
                    Console.WriteLine($"Connection closed due to an error: {exception}");
                }
                Connect();
                return null!;
            };

            _hubConnection.StartAsync().Wait();
            return _hubConnection;
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
    }
}

