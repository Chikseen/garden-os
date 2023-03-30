using main_service.Hardware;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public interface IMainHub
    {
        Task SendMyEvent(string message);
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
            Hardware.ProcessCompleted -= PrepareEventToSend; // register with an event
        }

        public void Send(string message)
        {
            if (_hubContext != null)
            {
                _hubContext.Clients.All.SendMyEvent(message);
            }
        }

        public void PrepareEventToSend()
        {
            Send(Hardware._testValue.ToString());
        }
    }
}
