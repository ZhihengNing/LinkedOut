namespace LinkedOut.User.Domain.Vo;

public class UserVo<T>
{
    public int? UnifiedId { get; set; }
    
    public string? Password { get; set; }
    
    public string? TrueName { get; set; }
    
    public string? UserType { get; set; }

    public int? IsSubscribed { get; set; }

    public T? Avatar { get; set; }

    public T? Back { get; set; }

    public string? BriefInfo { get; set; }

    public string? Email { get; set; }
}