namespace LinkedOut.DB.Entity;

public class JobExperience
{
    public int Id { get; set; }
    /// <summary>
    /// 简介
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 职位类型
    /// </summary>
    public string? PositionType { get; set; }
    /// <summary>
    /// 企业名字
    /// </summary>
    public string? EnterpriseName { get; set; }
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
    public int? UnifiedId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}