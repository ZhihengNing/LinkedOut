using LinkedOut.Common.Feign.User.Dto;

namespace LinkedOut.Recruitment.Domain;

public class AddApplicationVo
{
    public int? ResumeId { get; set; }
    
    public int? UserId { get; set; }
    
    public int? JobId { get; set; }
}

public class ApplicantVo
{
    public UserDto UserDto { get; set; }
    
    public string? ResumeUrl { get; set; }
}