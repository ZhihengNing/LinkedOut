namespace LinkedOut.DB.Entity;

public class Tweet
{
    /// <summary>
    /// 动态Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 动态内容
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 照片文件数组
    /// </summary>
    public string? PictureUrl { get; set; }
    /// <summary>
    /// 用户Id
    /// </summary>
    public int UnifiedId { get; set; }
    /// <summary>
    /// 喜欢数
    /// </summary>
    public int? LikeNum { get; set; }
    /// <summary>
    /// 评论数
    /// </summary>
    public int? CommentNum { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}