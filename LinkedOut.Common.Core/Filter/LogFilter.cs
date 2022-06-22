using System.Threading.Channels;
using LinkedOut.Common.Domain;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Filter;

public class LogFilter : IAsyncActionFilter
{
    private readonly ILogger<LogFilter> _logger;

    public LogFilter(ILogger<LogFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var descriptor = (context.ActionDescriptor as ControllerActionDescriptor)!;

        var logProperties = new LogProperties();
        
        var name = descriptor.MethodInfo.GetParameters();
        await next();
    }
}