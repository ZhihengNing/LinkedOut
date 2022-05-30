namespace LinkedOut.User.Domain.Vo;

public class JobVo
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? PositionType { get; set; }

    public string? EnterpriseName { get; set; }
 
    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
    

    public string? PictureUrl { get; set; }
}