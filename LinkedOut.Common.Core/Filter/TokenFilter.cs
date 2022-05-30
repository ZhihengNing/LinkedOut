using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LinkedOut.Common.Filter;

public class TokenFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestCookie = context.HttpContext.Request.Cookies["token"];
        
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var template = descriptor?
            .MethodInfo
            .GetCustomAttribute<HttpMethodAttribute>()
            ?.Template;
        if ((bool) template?.Equals("login"))
        {
            await next();
        }
        //暂时注释
        //TokenHelper.VerifyToken(requestCookie);

        await next();
    }
}