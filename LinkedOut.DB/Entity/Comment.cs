
namespace LinkedOut.DB.Entity;

public class Comment
{
    /// <summary>
    /// 评论Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public int UnifiedId { get; set; }

    /// <summary>
    /// 动态Id
    /// </summary>
    public int? TweetId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}