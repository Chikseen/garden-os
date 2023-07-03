using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;

namespace MainService.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;

        public MainController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
        }

        [HttpGet()]
        public async void Get()
        {
            var data = new Dictionary<string, string>();
            data.Add("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket");
            data.Add("audience", "dev-client-be");

            var client = new HttpClient();
            //var token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
            String token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJGemt0WXZVY2lSU1dDd25jdVNQU2hKZVNnYXB1QzFWQklEcHpSZ3BqVjNZIn0.eyJleHAiOjE2ODgzNzI3NTgsImlhdCI6MTY4ODM3MjQ1OCwiYXV0aF90aW1lIjoxNjg4MzcyNDU3LCJqdGkiOiI4NWU4NzY4Yy0wYWU2LTQwODUtYTQ2Zi02Nzk5NzkwZThmZDAiLCJpc3MiOiJodHRwczovL2F1dGguZHJ1bmMubmV0L3JlYWxtcy9HYXJkZW5PUy1ERVYiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNjljMDBmYWYtY2QzYi00MGU1LWIzZDEtZDRiNWZmNmRjOTFkIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGV2LWNsaWVudCIsIm5vbmNlIjoiODQ0NzhhYTctZjc5Zi00ODE3LTk4YmEtNGVhMmVjMTRjMGU2Iiwic2Vzc2lvbl9zdGF0ZSI6IjBhMjIzNWNhLTM4YTAtNDdlMi1hMzM4LTMxM2MxYmY4MmQyOCIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDo4MDgwIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJvZmZsaW5lX2FjY2VzcyIsImRlZmF1bHQtcm9sZXMtZ2FyZGVub3MtZGV2IiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6Im9wZW5pZCBlbWFpbCBwcm9maWxlIiwic2lkIjoiMGEyMjM1Y2EtMzhhMC00N2UyLWEzMzgtMzEzYzFiZjgyZDI4IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5hbWUiOiJ0dCBtbSIsInByZWZlcnJlZF91c2VybmFtZSI6InRAdC5kZSIsImdpdmVuX25hbWUiOiJ0dCIsImZhbWlseV9uYW1lIjoibW0iLCJlbWFpbCI6InRAdC5kZSJ9.Y8DeB3RseYPNKPAY1lsG5BkqeT3Ek9nRkvKrO8-V8SWwtoL5OiXXPbV9wlUSyrAP11nbLff9mmGAhAu4c_qusnjl88Nlzditru3aZUbE_CxkhgNFfy2tv4BDuwnD2V4gacwpNDpW7o3EI47jLaxlyFluQI1eSvcQGia8525tz-kA1gcZACDfcrj_Gds610u3mFAIVRRAfh3DvMEyS4BxL-Zr_jlllJKigO_3RrMXSLm-opYe5zJGGwYDCw2XJxqXjSGWazxfA0_MGoHPcrX3Rl0XE7sVWYg8NPQ2pLLK5fvYQ2mez9dIgq_zmqctOiSkDu_DlMubxdj0ukYbSvkdMA";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("https://auth.drunc.net/realms/GardenOS-DEV/protocol/openid-connect/userinfo", new FormUrlEncodedContent(data));
            var contents = await response.Content.ReadAsStringAsync();
            Console.WriteLine("dumy");
        }

        [HttpGet("ledToggle")]
        public OkResult ledToggle()
        {
            //WebInput.toggleLed();

            // I did not forgot to remove the apikey its an dummy key so dont be hyped
            String rpiid = "1a667139-d648-4745-8529-a296c6de6f05";
            String apiKey = "OExfKUsFUh8bpVaR8soHNGhvFcwMXAcsLLQazmzdDumn0nSKMne2lsMJCgkPoEF2rZuUkWRMlQ7lK4WH3TNnTe16adkHeVCVwqhmZXASrcBaZzQ5j2qVQoubRDMiVbOW";

            ResponseDevices devices = _deviceService.GetDataLog(rpiid, apiKey, true)!;
            _hubContext.Clients.All.SendCurrentDeviceData(devices);
            Console.WriteLine("pts");
            return Ok();
        }
    }
}
