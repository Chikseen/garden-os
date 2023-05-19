
using Microsoft.AspNetCore.SignalR.Client;

namespace RPI.Connection
{
    public class Connection
    {
        Microsoft.AspNetCore.SignalR.Client.HubConnection hubConnection;

        public Connection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://157.90.170.184:9992/hub")
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    GetMessage(user, message);
                });
            hubConnection.On<string>("SendMyEvent", (user) =>
                {
                    GetMessage(user, "message");
                });

            hubConnection.StartAsync().Wait();

            while (hubConnection.State == HubConnectionState.Connected)
            { }

            // Start Reconnect
        }

        void GetMessage(string user, string message)
        {
            Console.WriteLine("hi");
            Console.WriteLine(user);
            Console.WriteLine(message);
        }
    }
}

