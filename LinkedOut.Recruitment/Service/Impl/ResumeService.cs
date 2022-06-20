using LinkedOut.Common.Domain.Enum;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Helper;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service.Impl;

public class ResumeService : IResumeService
{
    private readonly AppFileManager _appFileManager;
    
    public ResumeService(AppFileManager appFileManager)
    {
        _appFileManager = appFileManager;
    }

    public async Task<List<ResumeVo>> GetResumes(int unifiedId)
    {
        return _appFileManager
            .GetResumes(unifiedId)
            .Select(o => new ResumeVo
            {
                DocumentUrl = o.Url,
                ResumeName = o.Name
            }).ToList();
    }

    public async Task DeleteResume(int resumeId)
    {
        _appFileManager.DeleteAppFile(resumeId,AppFileType.Resume);
        
    }

    public async Task InsertResume(int unifiedId, IFormFile file)
    {
        _appFileManager.AddToAppFile(file, BucketType.Resume, unifiedId);
    }
}