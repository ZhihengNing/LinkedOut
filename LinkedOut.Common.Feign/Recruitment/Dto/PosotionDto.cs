namespace LinkedOut.Common.Feign.Recruitment.Dto;


public class PositionDto
{
    public int Id { get; set; }
    
    public string? JobName { get; set; }
    
    public string? PositionType { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public string? Reward { get; set; }
}