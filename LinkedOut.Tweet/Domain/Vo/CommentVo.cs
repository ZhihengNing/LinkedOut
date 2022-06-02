using LinkedOut.Common.Feign.User.Dto;

namespace LinkedOut.Tweet.Domain.Vo;

public class CommentVo
{
    public UserDto? UserDto { get; set; }

    public int CommentId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreateTime { get; set; }
}