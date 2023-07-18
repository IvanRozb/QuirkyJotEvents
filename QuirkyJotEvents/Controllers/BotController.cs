using Microsoft.AspNetCore.Mvc;
using QuirkyJotEvents.Filters;
using QuirkyJotEvents.Services;
using Telegram.Bot.Types;

namespace QuirkyJotEvents.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}
