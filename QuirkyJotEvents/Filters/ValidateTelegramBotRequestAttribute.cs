using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace QuirkyJotEvents.Filters;

/// <summary>
/// Check for "X-Telegram-Bot-Api-Secret-Token"
/// Read more: <see href="https://core.telegram.org/bots/api#setwebhook"/> "secret_token"
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ValidateTelegramBotAttribute : TypeFilterAttribute
{
    public ValidateTelegramBotAttribute()
        : base(typeof(ValidateTelegramBotFilter))
    {
    }

    private class ValidateTelegramBotFilter : IActionFilter
    {
        private readonly string _secretToken;

        public ValidateTelegramBotFilter(IOptions<BotConfiguration> options)
        {
            var botConfiguration = options.Value;
            _secretToken = botConfiguration.SecretToken;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsValidRequest(context.HttpContext.Request))
            {
                context.Result = new ObjectResult("\"X-Telegram-Bot-Api-Secret-Token\" is invalid")
                {
                    StatusCode = 403
                };
            }
        }

        private bool IsValidRequest(HttpRequest request)
        {
            var isSecretTokenProvided = request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var secretTokenHeader);
            
            Console.WriteLine(1241241224);
            if (!isSecretTokenProvided) return false;
            Console.WriteLine(1241241224);
            return string.Equals(secretTokenHeader, _secretToken, StringComparison.Ordinal);
        }
    }
}
