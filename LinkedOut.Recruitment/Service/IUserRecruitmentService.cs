namespace LinkedOut.Recruitment.Service;

public interface IUserRecruitmentService
{

    Task ApplyPosition(int userId,int resumeId,int jobId);
}