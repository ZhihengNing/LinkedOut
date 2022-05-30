namespace LinkedOut.DB.Entity;

public class EnterpriseInfo
{
    public int Id { get; set; }
    
    public int UnifiedId { get; set; }
    /// <summary>
    /// 联系方式
    /// </summary>
    public string? ContactWay { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}