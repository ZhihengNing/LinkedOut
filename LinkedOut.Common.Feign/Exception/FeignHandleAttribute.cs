using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Feign.Exception;

public class FeignHandleAttribute : System.Attribute, IResourceFilter
{

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine("进入了");
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine("退出了");
        var exception = context.Exception;
        
        if (exception != null)
        {
            throw exception;
        }
    }
}