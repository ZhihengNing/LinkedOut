namespace LinkedOut.Common.Api;

public class ResultCode
{
    public long Code { get; }

    public string Message { get; }

    public ResultCode(long code, string message)
    {
        Code = code;
        Message = message;
    }

    private static ResultCode Result(StatusCode code,string? message=null)
    {
        return code switch
        {
            StatusCode.Success => new ResultCode(200, message??"操作成功"),
            StatusCode.Failed => new ResultCode(500, message??"操作失败"),
            StatusCode.Forbidden => new ResultCode(403, message??"没有权限访问"),
            StatusCode.ValidateFailed => new ResultCode(404, message??"参数校验失败"),
            StatusCode.Unauthorized => new ResultCode(400, message??"暂未登录或token已经过期"),
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }

    public static ResultCode Success(string?message=null)
    {
        return Result(StatusCode.Success,message);
    }

    public static ResultCode Failed(string? message=null)
    {
        return Result(StatusCode.Failed,message);
    }

    public static ResultCode Forbidden(string?message=null)
    {
        return Result(StatusCode.Forbidden,message);
    }

    public static ResultCode ValidateFailed(string? message=null)
    {
        return Result(StatusCode.ValidateFailed,message);
    }


    public static ResultCode Unauthorized(string?message=null)
    {
        return Result(StatusCode.Unauthorized,message);
    }


    public enum StatusCode
    {
        Success,
        Failed,
        Forbidden,
        ValidateFailed,
        Unauthorized
    }
}