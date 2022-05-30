namespace LinkedOut.User.Domain.Vo;

public class EnterpriseInfoVo<T> : UserVo<T>
{

    public string? contactWay { get; set; }

    public string? description { get; set; }
    
    public int? fansNum { get; set; }
    
    public int? followNum { get; set; }
}