namespace LinkedOut.DB.Entity;

public class Subscribed
{
    /// <summary>
    /// 关注Id
    /// </summary>
    public int Id { get; set; }
    public int? FirstUserId { get; set; }
    public int? SecondUserId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}