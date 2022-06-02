using LinkedOut.Common.Exception;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service.Impl;

public class EduService: IEduService
{
    private readonly LinkedOutContext _context;

    public EduService(LinkedOutContext context)
    {
        _context = context;
    }

    public Task<List<EduVo>> GetEduExperience(int unifiedId)
    {
        return Task.FromResult(_context.EduExperiences.Select(o => o)
            .Where(o => o.UnifiedId == unifiedId)
            .Join(_context.Users,
                edu => edu.CollegeName,
                user => user.TrueName,
                (edu, user) => new EduVo
                {
                    Degree = edu.Degree,
                    Major = edu.Major,
                    CollegeName = edu.CollegeName,
                    PictureUrl = user.Avatar,
                    Id = edu.Id
                }
            ).ToList());
    }

    public async Task AddEduExperience(EduExperienceVo eduExperienceVo)
    {
        var eduExperience = new EduExperience
        {
            CollegeName = eduExperienceVo.CollegeName,
            StartTime = eduExperienceVo.StartTime,
            EndTime = eduExperienceVo.EndTime,
            Degree = eduExperienceVo.Degree,
            Major = eduExperienceVo.Major,
            UnifiedId =(int) eduExperienceVo.UnifiedId,
        };
        var entityEntry = await _context.EduExperiences.AddAsync(eduExperience);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEduExperience(int eduExperienceId)
    {
        var one = _context.EduExperiences.Select(o => o)
            .SingleOrDefault(o => o.Id == eduExperienceId);
        if (one == null) throw new ApiException($"查询id为{eduExperienceId}的教育经历不存在");
        _context.EduExperiences.Remove(one);
        await _context.SaveChangesAsync();
    }
}