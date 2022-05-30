namespace LinkedOut.User.Domain.Vo;

public class UserVo<T>
{
    public int unifiedId { get; set; }
    
    public string? password { get; set; }
    
    public string? trueName { get; set; }

    public int? isSubscribed { get; set; }

    public T? avatar { get; set; }

    public T? back { get; set; }

    public string? briefInfo { get; set; }

    public string? email { get; set; }
}