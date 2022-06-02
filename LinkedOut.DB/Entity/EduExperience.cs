namespace LinkedOut.DB.Entity;

public class EduExperience
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 专业
    /// </summary>
    public string? Major { get; set; }

    /// <summary>
    /// 学位
    /// </summary>
    public string? Degree { get; set; }

    /// <summary>
    /// 学校名字
    /// </summary>
    public string? CollegeName { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public int UnifiedId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}