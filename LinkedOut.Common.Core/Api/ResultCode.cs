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

    private static ResultCode Result(StatusCode code)
    {
        return code switch
        {
            StatusCode.Success => new ResultCode(200, "操作成功"),
            StatusCode.Failed => new ResultCode(500, "操作失败"),
            StatusCode.Forbidden => new ResultCode(403, "没有权限访问"),
            StatusCode.ValidateFailed => new ResultCode(404, "参数校验失败"),
            StatusCode.Unauthorized => new ResultCode(400, "暂未登录或token已经过期"),
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }

    public static ResultCode Success()
    {
        return Result(StatusCode.Success);
    }

    public static ResultCode Failed()
    {
        return Result(StatusCode.Failed);
    }

    public static ResultCode Forbbiden()
    {
        return Result(StatusCode.Forbidden);
    }

    public static ResultCode ValidateFailed()
    {
        return Result(StatusCode.ValidateFailed);
    }


    public static ResultCode Unauthorized()
    {
        return Result(StatusCode.Unauthorized);
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