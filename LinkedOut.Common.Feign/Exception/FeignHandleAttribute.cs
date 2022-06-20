using LinkedOut.Common.Api;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Feign.Exception;

public class FeignHandleAttribute : System.Attribute, IAsyncResultFilter
{
    private readonly ILogger<FeignHandleAttribute> _logger;

    public FeignHandleAttribute(ILogger<FeignHandleAttribute> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var contextResult = context.Result;

        _logger.LogInformation(contextResult.ToString());
        await next();
    }
}