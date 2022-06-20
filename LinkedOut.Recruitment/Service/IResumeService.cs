using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service;

public interface IResumeService
{
    Task<List<ResumeVo>> GetResumes(int unifiedId);

    Task DeleteResume(int resumeId);

    Task InsertResume(int unifiedId, IFormFile file);
}