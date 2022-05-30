using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Filter;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogError("系统异常");
        var contextException = context.Exception;
        var message = contextException.Message;
        var result = new
        {
            Code = 500,
            Message = message
        };
        context.Result = new ObjectResult(result);
        return Task.CompletedTask;
    }
}