using LinkedOut.Common.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Filter;

public class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var contextException = context.Exception;
        if (contextException is not ApiException apiException) return Task.CompletedTask;
        _logger.LogWarning("业务异常");
        var result = new
        {
            apiException.Code,
            apiException.Message
        };
        context.Result = new ObjectResult(result);
        //如果这样做了其他的异常拦截器就不会执行
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}