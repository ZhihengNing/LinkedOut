namespace LinkedOut.DB.Entity;

public class Position
{
    /// <summary>
    /// 岗位Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 工作名称
    /// </summary>
    public string? JobName { get; set; }
    /// <summary>
    /// 岗位类型
    /// </summary>
    public string? PositionType { get; set; }
    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 薪资
    /// </summary>
    public string? Reward { get; set; }
    /// <summary>
    /// 工作地点(-分隔省市eg.河北-石家庄）
    /// </summary>
    public string? WorkPlace { get; set; }
    /// <summary>
    /// 企业Id
    /// </summary>
    public int? EnterpriseId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}