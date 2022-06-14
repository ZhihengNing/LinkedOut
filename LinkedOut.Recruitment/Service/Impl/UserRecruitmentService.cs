using LinkedOut.DB;
using LinkedOut.DB.Entity;

namespace LinkedOut.Recruitment.Service.Impl;

public class UserRecruitmentService: IUserRecruitmentService
{

    private readonly LinkedOutContext _context;


    public UserRecruitmentService(LinkedOutContext context)
    {
        _context = context;
    }

    
    public async  Task ApplyPosition(int userId,int resumeId,int jobId)
    {
        var application = new Application()
        {
            UserId = userId,
            ResumeId = resumeId,
            JobId = jobId
        };
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
    }
}