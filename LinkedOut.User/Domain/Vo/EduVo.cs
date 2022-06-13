namespace LinkedOut.User.Domain.Vo;

public class EduVo
{
    public int Id { get; set; }
    public string? Major { get; set; }

    public string? Degree { get; set; }

    public string? CollegeName { get; set; }

    public string? PictureUrl { get; set; }
}

public class EduExperienceVo
{
    public string? Major { get; set; }
    
    public string? Degree { get; set; }
    
    public string? CollegeName { get; set; }
    
    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? UnifiedId { get; set; }
}