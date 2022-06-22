using LinkedOut.Common.Feign.User.Dto;

namespace LinkedOut.Tweet.Domain.Vo;

public class TweetVo
{
    public int TweetId { get; set; }
    public string Type { get; set; } = null!;
    public DateTime RecordTime { get; set; }
    public UserDto SimpleUserInfo { get; set; } = null!;
}


public class UserTweetVo : TweetVo
{
    public int PraiseNum { get; set; }

    public int LikeState { get; set; }

    public int CommentNum { get; set; }

    public string? Contents { get; set; }

    public List<string> PictureList { get; set; } = null!;
}

public class EnterpriseTweetVo : TweetVo
{
    public string? JobName { get; set; }

    public string? PositionType { get; set; }

    public string? Reward { get; set; }
}
