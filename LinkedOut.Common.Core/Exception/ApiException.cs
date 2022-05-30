using LinkedOut.Common.Api;

namespace LinkedOut.Common.Exception;

public class ApiException : ApplicationException
{
    public long Code {get; set; }

    public override string Message { get;}

    public ApiException(long code, string message)
    {
        Code = code;
        Message = message;
    }

    public ApiException(ResultCode error)
    {
        Code = error.Code;
        Message=error.Message;
    }

    public ApiException(string message) : base(message)
    {
        Code = ResultCode.Failed().Code;
        Message = message;
    }
    
    
}