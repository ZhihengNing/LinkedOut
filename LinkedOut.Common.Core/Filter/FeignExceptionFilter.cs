using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Feign.Exception;

public class FeignExceptionFilter : IAsyncResultFilter
{

    private readonly ILogger<FeignExceptionFilter> _logger;

    public FeignExceptionFilter(ILogger<FeignExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

        var template = descriptor?
            .MethodInfo
            .GetCustomAttribute<HttpMethodAttribute>()?
            .Template;

        if (!template!.Contains("feign"))
        {
            await next();
        }

        var s = context.Result.ToString();
        _logger.LogInformation(s);
        await next();
    }
}