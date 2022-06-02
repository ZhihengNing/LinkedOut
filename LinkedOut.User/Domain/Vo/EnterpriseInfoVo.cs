namespace LinkedOut.User.Domain.Vo;

public class EnterpriseInfoVo<T> : UserVo<T>
{

    public string? ContactWay { get; set; }

    public string? Description { get; set; }
    
    public int? FansNum { get; set; }
    
    public int? FollowNum { get; set; }
}