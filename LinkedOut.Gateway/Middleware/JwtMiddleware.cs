using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.Gateway.Helper;

namespace LinkedOut.Gateway.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<JwtMiddleware> _logger;

    private static readonly List<string> WhiteList =
        AppSettingHelper.App<string>("whiteList");

    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation("进入token拦截器");

        var paths = context.Request.Path.Value?.Split("/");
        var template = paths?[^1];

        var token = context.Request.Cookies["token"];

        _logger.LogInformation("token:" + token);

        if (template != null && (WhiteList.Count == 0 || WhiteList.Contains(template)))
        {
            await _next.Invoke(context);
        }

        try
        {
            VerifyHelper.VerifyToken(token);
        }
        catch (ApiException e)
        {
            _logger.LogInformation("token校验没通过qwq");
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new
            {
                e.Code,
                e.Message
            });
        }

        await _next.Invoke(context);
    }
}