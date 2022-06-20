using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service;

public interface IUserRecruitmentService
{

    Task ApplyPosition(int userId, int resumeId, int jobId);

    Task CancelApplication(int unifiedId, int jobId);

    Task<List<PositionVo>> GetPostedPosition(int unifiedId);

    Task<List<PositionVo>> GetRecommendPosition(int unifiedId, int? momentId);
}