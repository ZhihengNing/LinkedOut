using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Helper;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service.Impl;

public class ResumeService : IResumeService
{
    private readonly AppFileManager _appFileManager;

    private readonly LinkedOutContext _context;
    
    public ResumeService(AppFileManager appFileManager, LinkedOutContext context)
    {
        _appFileManager = appFileManager;
        _context = context;
    }

    public async Task<List<ResumeVo>> GetResumes(int unifiedId)
    {
        return _appFileManager
            .GetResumes(unifiedId)
            .Select(o => new ResumeVo
            {
                ResumeId = o.Id,
                DocumentUrl = o.Url,
                ResumeName = o.Name
            }).ToList();
    }

    public async Task DeleteResume(int resumeId)
    {
        var resume = _context.AppFiles.SingleOrDefault(o=>o.Id==resumeId);
        if (resume == null)
        {
            throw new ApiException($"不存在id为{resumeId}的简历");
        }
        
    }

    public async Task InsertResume(int unifiedId, IFormFile file)
    {
        _appFileManager.AddToAppFile(file, BucketType.Resume, unifiedId);
    }
}