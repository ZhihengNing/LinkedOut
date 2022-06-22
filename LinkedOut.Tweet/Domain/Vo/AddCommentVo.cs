namespace LinkedOut.Tweet.Domain.Vo;

public class AddCommentVo
{
    public int? UnifiedId { get; set; }

    public string? Contents { get; set; }

    public int? TweetId { get; set; }
}