using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UxCarrier.Controllers.Filter
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"REQ:{JsonConvert.SerializeObject(context.ActionArguments)}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context?.Result is ObjectResult objResult)
                _logger.LogInformation($"RESP:{JsonConvert.SerializeObject(objResult.Value)}");
        }

    }
}
