namespace LinkedOut.DB.Entity;

public class UserInfo
{
        
    /// <summary>
    /// userId
    /// </summary>
    public int Id { get; set; }

    public int UnifiedId { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; }
    /// <summary>
    /// 年龄
    /// </summary>
    public int? Age { get; set; }
    /// <summary>
    /// 身份证
    /// </summary>
    public string? IdCard { get; set; }
    /// <summary>
    /// 电话号码
    /// </summary>
    public int? PhoneNum { get; set; }
    /// <summary>
    /// 职位偏好
    /// </summary>
    public string? PrePosition { get; set; }
    /// <summary>
    /// 居住地(-分隔省市eg.河北-石家庄)
    /// </summary>
    public string? LivePlace { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}