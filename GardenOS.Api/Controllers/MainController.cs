using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        [HttpGet()]
        public string Get()
        {
            return "If your reading this your cool";
        }
    }
}
