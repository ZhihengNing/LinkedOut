using System.Reflection;
using System.Transactions;
using LinkedOut.Common.Attribute;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Filter;

public class TransactionFilter: IAsyncActionFilter
{
    private readonly ILogger<TransactionFilter> _logger;

    public TransactionFilter(ILogger<TransactionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var attribute = descriptor?.MethodInfo.GetCustomAttribute<NoTransactionAttribute>();
        if (attribute == null)
        {
            _logger.LogInformation("进入事务拦截器");
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var actionExecutedContext = await next();
            if (actionExecutedContext.Exception == null)
            {
                //如果没有出现异常就提交事务
                transaction.Complete();
            }
        }
        else
        {
            await next();
        }
    }
}