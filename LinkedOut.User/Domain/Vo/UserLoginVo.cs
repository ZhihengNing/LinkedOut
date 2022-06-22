namespace LinkedOut.User.Domain.Vo;

public class UserLoginVo
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class UserLoginDetailVo
{
    public int UnifiedId { get; set; }
    
    public string UserType { get; set; }
}