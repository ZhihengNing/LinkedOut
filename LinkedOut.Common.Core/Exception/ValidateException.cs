using LinkedOut.Common.Api;

namespace LinkedOut.Common.Exception;

public class ValidateException : ApiException
{
    public ValidateException(string? message=null) : base(ResultCode.ValidateFailed(message))
    {
        
    }
}