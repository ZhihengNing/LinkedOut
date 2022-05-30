namespace LinkedOut.Common.Api;

public class MessageModel<T>
{
    public long Code { get; set; }

    public string Message { get; set; }

    public T? Data { get; set; }

    public MessageModel()
    {
        
    }
    protected MessageModel(long code, string message, T data)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    private static MessageModel<T> Success(long code, string message, T data)
    {
        if (code <= 0)
            throw new ArgumentOutOfRangeException(nameof(code));
        return new MessageModel<T>(code, message, data);
    }
    
    private static MessageModel<T?> Failed(long code, string message)
    {
        return new MessageModel<T?>(code, message, default);
    }

    public static MessageModel<T> Success()
    {
        return new MessageModel<T>(ResultCode.Success().Code, ResultCode.Success().Message, default);
    }

    public static MessageModel<T> Success(T data)
    {
        return Success(ResultCode.Success().Code, ResultCode.Success().Message, data);
    }

    public static MessageModel<T> Success(string message, T data)
    {
        return Success(code: ResultCode.Success().Code, message, data);
    }

    public static MessageModel<T?> Failed(ResultCode result)
    {
        return Failed(result.Code, result.Message);
    }

    public static MessageModel<T?> Failed(string message)
    {
        return Failed(ResultCode.Failed().Code, message);
    }

    public static MessageModel<T?> ValidateFailed()
    {
        return Failed(ResultCode.ValidateFailed());
    }

    public static MessageModel<T?> Forbidden()
    {
        return Failed(ResultCode.Forbbiden());
    }

}

public class MessageModel : MessageModel<object>
{

    public new object Data { get; set; }


    protected MessageModel(long code, string message, object data) : base(code, message, data)
    {
    }
}