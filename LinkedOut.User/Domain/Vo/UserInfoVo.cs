namespace LinkedOut.User.Domain.Vo;

public class UserInfoVo<T> : UserVo<T>
{
    public string? idCard { get; set; }
    
    public int? phoneNum { get; set; }

    /**
     * 职位偏好(只记录leve3级，即json文件最内层职位名字)
     */
    public string? prePosition { get; set; }

    public string? gender { get; set; }

    public int? age { get; set; }

    /**
     * 居住地(-分隔省市eg.河北-石家庄)
     */
    public string? livePlace;
}