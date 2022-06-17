namespace LinkedOut.Common.Feign.User.Dto;

public class UserDto
{
    public int UnifiedId { get; set; }

    public string? TrueName { get; set; }

    public string UserType { get; set; } = null!;

    public string? BriefInfo { get; set; }

    public string? PictureUrl { get; set; }
}