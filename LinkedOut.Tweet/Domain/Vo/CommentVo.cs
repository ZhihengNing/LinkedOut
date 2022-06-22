using LinkedOut.Common.Feign.User.Dto;

namespace LinkedOut.Tweet.Domain.Vo;

public class CommentVo
{
    public UserDto? SimpleUserInfo { get; set; }

    public int CommentId { get; set; }

    public string? Contents { get; set; }

    public DateTime? RecordTime { get; set; }
}