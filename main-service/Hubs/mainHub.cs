using Microsoft.AspNetCore.SignalR;

namespace MainService.Hub
{
    public interface IMainHub
    {
        public Task SendMyEvent(ResponseDevices message);
        public Task SendCurrentDeviceData(ResponseDevices message);
        public Task NewDeviceStatus(DeveiceStatus message);
        public Task HardwareRequestRPI();
        public Task SendRebootRequest();
    }

    // I have the assumbtion that clients are not disconnected in the correct manner
    public class MainHub : Hub<IMainHub>
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        static bool isInit = false;
        private static readonly Dictionary<string, string> _groupList = new();
        static public Dictionary<string, string> _rpiList = new();

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

        public void SetUserToGarden(string gId)
        {
            string gardenId = gId.Replace("-", "");
            string userId = Context.ConnectionId;


            if (_groupList.ContainsKey(userId))
                Groups.RemoveFromGroupAsync(userId, _groupList[userId]);

            _groupList[userId] = gardenId;
            Groups.AddToGroupAsync(userId, gardenId);
        }

        public void SetRpiConnectionId(string rpiId)
        {
            string conId = Context.ConnectionId;

            _rpiList[rpiId] = conId;

            Console.WriteLine(rpiId);
            Console.WriteLine(conId);
        }
    }
}
