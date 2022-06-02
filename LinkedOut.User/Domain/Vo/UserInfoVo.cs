namespace LinkedOut.User.Domain.Vo;

public class UserInfoVo<T> : UserVo<T>
{
    public string? IdCard { get; set; }
    
    public int? PhoneNum { get; set; }

    /**
     * 职位偏好(只记录leve3级，即json文件最内层职位名字)
     */
    public string? PrePosition { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    /**
     * 居住地(-分隔省市eg.河北-石家庄)
     */
    public string? LivePlace;
}