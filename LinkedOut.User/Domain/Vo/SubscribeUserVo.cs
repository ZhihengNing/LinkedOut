namespace LinkedOut.User.Domain.Vo;

public class SubscribeUserVo
{
    public int? UnifiedId { get; set; }

    public string? TrueName { get; set; }

    public string? BriefInfo { get; set; }

    public string? UserType { get; set; }

    public string? UserAvatar { get; set; }

    public bool IsSubscribed { get; set; }
}