
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace RPI.Connection
{
    public class Connection
    {
        private Microsoft.AspNetCore.SignalR.Client.HubConnection _hubConnection { get; set; }

        public Connection()
        {
            _hubConnection = Connect();
        }

        private Microsoft.AspNetCore.SignalR.Client.HubConnection Connect()
        {
            Console.WriteLine("Try Connect");
            _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://gardenapi.drunc.net/hub")
            .WithAutomaticReconnect()
            .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
              {
                  GetMessage(user, message);
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
            Console.WriteLine("hi");
            Console.WriteLine(user);
            Console.WriteLine(message);
        }
    }
}

