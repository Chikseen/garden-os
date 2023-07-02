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
            String token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJGemt0WXZVY2lSU1dDd25jdVNQU2hKZVNnYXB1QzFWQklEcHpSZ3BqVjNZIn0.eyJleHAiOjE2ODgyODk1NjMsImlhdCI6MTY4ODI4OTI2MywiYXV0aF90aW1lIjoxNjg4Mjg4MDYzLCJqdGkiOiI0ZmY2MzZmMi0wNGU3LTQ0OTQtOTE2Yi03Zjc5MTViMjlkMDUiLCJpc3MiOiJodHRwczovL2F1dGguZHJ1bmMubmV0L3JlYWxtcy9HYXJkZW5PUy1ERVYiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNjljMDBmYWYtY2QzYi00MGU1LWIzZDEtZDRiNWZmNmRjOTFkIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGV2LWNsaWVudCIsIm5vbmNlIjoiMGYwMGRhYzgtNzYyOC00MWNjLThmMmEtYjlmOWZlY2YyYWE0Iiwic2Vzc2lvbl9zdGF0ZSI6ImViMmIxZWRjLTZjZTktNDMzMy1iOGM2LTUwMjIxMWMzNzljMCIsImFjciI6IjAiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDo4MDgwIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJvZmZsaW5lX2FjY2VzcyIsImRlZmF1bHQtcm9sZXMtZ2FyZGVub3MtZGV2IiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6Im9wZW5pZCBlbWFpbCBwcm9maWxlIiwic2lkIjoiZWIyYjFlZGMtNmNlOS00MzMzLWI4YzYtNTAyMjExYzM3OWMwIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5hbWUiOiJ0dCBtbSIsInByZWZlcnJlZF91c2VybmFtZSI6InRAdC5kZSIsImdpdmVuX25hbWUiOiJ0dCIsImZhbWlseV9uYW1lIjoibW0iLCJlbWFpbCI6InRAdC5kZSJ9.UyNzlnGKuKwn62VXAkla9WTmr-ze0hzymHQkt47QSK6O447nsElOmp_EX2RwQrNsMRbS11ZYE0DUwHRlIft0wYgJBgiyXPUrPZPlJesY5iq4rmmXNfnUtAlZ_3zwErgCvxQ8w5Q6NYa6l9bSVmteYZ8SRdx_lMz2YknKvnfgubUZ_KnCAHxKb7sQ9V-Qi-9H4UJb8v01Wm6qAaZHdy3_rITZUF9t5XurkGGFNTHwmYNwMJg0f8yyyQeqnsSQgtHuVP8LeG5N7dn5stTSV4B32YXlcng79jvQiNjPoUih9F5XdMHh2ZoS7GMOhSBQWjZ1GOlqEo_1MDwBlH_IOQKH-A";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("https://auth.drunc.net/realms/GardenOS-DEV/protocol/openid-connect/token", new FormUrlEncodedContent(data));

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
