using LinkedOut.Common.Api;
using LinkedOut.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Feign.Middleware;

public class FeignMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<FeignMiddleware> _logger;
    
    public FeignMiddleware(RequestDelegate next, ILogger<FeignMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation("进入feign拦截器");

        var path = context.Request.Path.Value!;

        _logger.LogInformation(path);
        if (!path.Contains("feign"))
        {
            await _next.Invoke(context);
        }

        // var bodyLength = context.Response.Body.Length;

        // _logger.LogInformation(bodyLength.ToString());

        await _next.Invoke(context);
        // await context.Response.WriteAsJsonAsync(new
        // {
        //     ResultCode.RemoteFailed().Code,
        //     ResultCode.RemoteFailed().Message
        // });
    }
}