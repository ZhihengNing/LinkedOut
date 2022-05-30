namespace LinkedOut.DB.Entity;

public class User
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    public int UnifiedId { get; set; }

    /// <summary>
    /// 姓名（不可以改变）
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 用户类型
    /// </summary>
    public string UserType { get; set; } = null!;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }
    /// <summary>
    /// 简要介绍
    /// </summary>
    public string? BriefInfo { get; set; }
    /// <summary>
    /// 真实名字
    /// </summary>
    public string? TrueName { get; set; }
    /// <summary>
    /// 关注的数量
    /// </summary>
    public int? SubscribeNum { get; set; }
    /// <summary>
    /// 背景图
    /// </summary>
    public string? Background { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}