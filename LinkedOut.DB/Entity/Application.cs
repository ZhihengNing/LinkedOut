namespace LinkedOut.DB.Entity;

public class Application
{
    /// <summary>
    /// 申请Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 岗位Id
    /// </summary>
    public int JobId { get; set; }
    /// <summary>
    /// 简历Id
    /// </summary>
    public int ResumeId { get; set; }
    /// <summary>
    /// 用户Id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}