using MainService.Hardware;
using Microsoft.AspNetCore.SignalR;

namespace MainService.Hub
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
            MainHardware.ProcessCompleted += PrepareEventToSend; // register with an event
        }

        ~MainHub()
        {
            MainHardware.ProcessCompleted -= PrepareEventToSend;
        }

        public void PrepareEventToSend()
        {
            Console.WriteLine($"Send!");
            _hubContext.Clients.All.SendMyEvent(MainHardware._data);
        }
    }
}
