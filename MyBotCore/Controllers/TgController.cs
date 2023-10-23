using Microsoft.AspNetCore.Mvc;
using MyBotCore.Shared.Interfaces.Services;
using System.Net;
using Telegram.Bot.Types;

namespace MyBotCore.Controllers
{
    /// <summary>
    /// This controller exists only for the telegram webhook
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Attention: It`s hidden from Swagger by this row
    [ApiController]
    [Route("api/[controller]")]
    public class TgController : ControllerBase
    {
        private readonly ITgBotService handleUpdateService;

        public TgController(ITgBotService handleUpdateService)
        {
            this.handleUpdateService = handleUpdateService;
        }

        [HttpPost("Hook")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> HookPost([FromBody] Update update)
        {
            //https://core.tlgr.org/bots/api#setwebhook
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
