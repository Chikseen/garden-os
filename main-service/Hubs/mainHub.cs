using Microsoft.AspNetCore.SignalR;

namespace MainService.Hub
{
    public interface IMainHub
    {
        public Task SendMyEvent(ResponseDevices message);
        public Task SendCurrentDeviceData(ResponseDevices message);
        public Task HardwareRequestRPI();
    }

    // I have the assumbtion that clients are not disconnected in the correct manner
    public class MainHub : Hub<IMainHub>
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        static bool isInit = false;
        static private Dictionary<String, String> _groupList = new();

        public MainHub(IHubContext<MainHub, IMainHub> hubContext)
        {
            Console.WriteLine("hi");
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
            // MainHardware.ProcessCompleted += PrepareEventToSend; // register with an event
        }

        ~MainHub()
        {
            // MainHardware.ProcessCompleted -= PrepareEventToSend;
        }

        public void setUserToGarden(String gId)
        {
            String gardenId = gId.Replace("-", "");
            String userId = this.Context.ConnectionId;


            if (_groupList.ContainsKey(userId))
                this.Groups.RemoveFromGroupAsync(userId, _groupList[userId]);

            _groupList[userId] = gardenId;
            this.Groups.AddToGroupAsync(userId, gardenId);
        }
    }
}
