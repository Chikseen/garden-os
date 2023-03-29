using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public class MainHub : Hub
    {
        public async Task SendMyEvent()
        {
            await Clients.All.SendAsync("MyEvent");
        }
    }
}