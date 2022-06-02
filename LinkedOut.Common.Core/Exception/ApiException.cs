using LinkedOut.Common.Api;

namespace LinkedOut.Common.Exception;

/**
 * 最佳实践:和业务相关的校验放在service层,
 * controller层进行一些参数为空的校验什么的
 * 而且最好是保证传入service的参数是一个个的
 * 或者是一个Bo的对象
 */
public class ApiException : ApplicationException
{
    public long Code {get; }

    public override string Message { get;}

    public ApiException(long code, string message)
    {
        Code = code;
        Message = message;
    }

    public ApiException(ResultCode error, string? message = null)
    {
        Code = error.Code;
        Message = message ?? error.Message;
    }

    public ApiException(string message) : base(message)
    {
        Code = ResultCode.Failed().Code;
        Message = message;
    }
    
    
}