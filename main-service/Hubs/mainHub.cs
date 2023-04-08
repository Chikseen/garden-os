using System.Text.Json;
using main_service.Hardware;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public interface IMainHub
    {
        public Task SendMyEvent(HardwareData message);
    }

    // I have the assumbtion that clients are not disconnected in the correct manner
    public class MainHub : Hub<IMainHub>
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        static bool isInit = false;

        public MainHub(IHubContext<MainHub, IMainHub> hubContext)
        {
            if (!isInit)
            {
                Init();
                isInit = true;
            }

            _hubContext = hubContext;
        }

        // One Time Init
        public void Init()
        {
            Console.WriteLine("Main Hub Init");
            Hardware.ProcessCompleted += PrepareEventToSend; // register with an event
        }

        ~MainHub()
        {
            Hardware.ProcessCompleted -= PrepareEventToSend;
        }

        public void PrepareEventToSend()
        {
            string jsonString = JsonSerializer.Serialize<HardwareData>(Hardware._data);
            _hubContext.Clients.All.SendMyEvent(Hardware._data);
        }
    }
}
