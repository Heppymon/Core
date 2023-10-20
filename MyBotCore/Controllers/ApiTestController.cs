using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyBotCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        [HttpGet("Always500")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Always500()
        {
            throw new NotImplementedException();
        }


        [HttpGet("GetTest")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTest()
        {
            return Ok("Rdy");
        }

    }
}
