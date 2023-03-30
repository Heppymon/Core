using Microsoft.AspNetCore.Mvc;
using MyBotCore.Services;
using System.Net;
using Telegram.Bot.Types;

namespace MyBotCore.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("[controller]")]
    public class TgBotController : ControllerBase
    {
        private readonly TgBotService handleUpdateService;

        public TgBotController(TgBotService handleUpdateService)
        {
            this.handleUpdateService = handleUpdateService;
        }


        [HttpPost("HookPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            //https://core.tlgr.org/bots/api#setwebhook
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
