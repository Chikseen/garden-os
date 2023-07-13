
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

                        FileStream fs = File.Create("../111.txt");
                        System.Diagnostics.Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "reboot" });

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
            catch (System.Exception e)
            {
                // create a file at pathName
                FileStream fs = File.Create("../222.txt");
                Thread.Sleep(10000);
                System.Diagnostics.Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "reboot" });
                Console.WriteLine(e);
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

            return Task.CompletedTask;
        }

        Task HubConnection_Reconnected(string? arg)
        {
            Console.WriteLine("Reconnected");
            setRpiAndConId();
            return Task.CompletedTask;
        }
    }
}

