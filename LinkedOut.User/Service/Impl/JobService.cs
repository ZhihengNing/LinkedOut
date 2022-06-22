using LinkedOut.Common.Exception;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service.Impl;

public class JobService : IJobService
{

    private readonly LinkedOutContext _context;

    public JobService(LinkedOutContext context)
    {
        _context = context;
    }

    public Task<List<JobVo>> GetJobExperience(int unifiedId)
    {
        var jobVos = _context.JobExperiences.Select(o => o)
            .Where(o => o.UnifiedId == unifiedId)
            .Join(_context.Users, job => job.EnterpriseName, user => user.TrueName,
                (job, user) => new JobVo
                {
                    Description = job.Description,
                    StartTime = job.StartTime,
                    EndTime = job.EndTime,
                    EnterpriseName = job.EnterpriseName,
                    PositionType = job.PositionType,
                    Id = job.Id,
                    PictureUrl = user.Avatar
                }
            ).ToList();
        return Task.FromResult(jobVos);
    }

    public async Task InsertJobExperience(JobExperienceVo jobExperienceVo)
    {
        var jobExperience = new JobExperience
        {
            Description = jobExperienceVo.Description,
            PositionType = jobExperienceVo.PositionType,
            EnterpriseName = jobExperienceVo.EnterpriseName,
            StartTime = jobExperienceVo.StartTime,
            EndTime = jobExperienceVo.EndTime,
            UnifiedId = (int) jobExperienceVo.UnifiedId!
        };
        await _context.JobExperiences.AddAsync(jobExperience);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobExperience(int jobExperienceId)
    {
        var jobExperience = _context.JobExperiences
            .SingleOrDefault(o => o.Id == jobExperienceId);
        if (jobExperience == null) throw new ApiException($"不存在{jobExperienceId}的工作经历");

        _context.JobExperiences.Remove(jobExperience);

        await _context.SaveChangesAsync();
    }
}

