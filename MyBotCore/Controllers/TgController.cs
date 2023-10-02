using Microsoft.AspNetCore.Mvc;
using MyBotCore.Services;
using System.Net;
using Telegram.Bot.Types;

namespace MyBotCore.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class TgController : ControllerBase
    {
        private readonly TgBotService handleUpdateService;

        public TgController(TgBotService handleUpdateService)
        {
            this.handleUpdateService = handleUpdateService;
        }

        /// <summary>
        /// Telegram webhook endpoint
        /// </summary>
        [HttpPost("Hook")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> HookPost([FromBody]Update update)
        {
            //https://core.tlgr.org/bots/api#setwebhook
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
