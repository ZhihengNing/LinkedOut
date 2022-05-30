using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IEduService
{

    Task<List<EduVo>> GetEduExperience(int unifiedId);

    Task AddEduExperience(EduExperience eduExperience);

    Task DeleteEduExperience(int eduExperienceId);
}