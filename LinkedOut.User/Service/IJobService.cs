using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IJobService
{
    Task<List<JobVo>> GetJobExperience(int unifiedId);

    Task InsertJobExperience(JobExperienceVo jobExperienceVo);

    Task DeleteJobExperience(int jobExperienceId);
}