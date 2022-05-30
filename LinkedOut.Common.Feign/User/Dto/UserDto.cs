namespace LinkedOut.Common.Feign.User.Dto;

public class UserDto
{
    public int UnifiedId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public string? UserBriefInfo { get; set; } = null!;

    public string? UserIconUrl { get; set; } = null!;
}