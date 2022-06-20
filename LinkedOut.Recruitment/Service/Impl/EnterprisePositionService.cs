using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service.Impl;

public class EnterprisePositionService: IEnterprisePositionService
{

    private readonly LinkedOutContext _context;

    private readonly IUserFeignClient _feignClient;

    public EnterprisePositionService(LinkedOutContext context, IUserFeignClient feignClient)
    {
        _context = context;
        _feignClient = feignClient;
    }

    public async Task InsertPosition(Position position)
    {
        await _context.Positions.AddAsync(position);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeletePosition(int jobId)
    {
        var position = _context.Positions.SingleOrDefault(o => o.Id == jobId);

        if (position == null)
        {
            throw new ApiException($"id为{jobId}的工作岗位不存在");
        }

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ApplicantVo>> GetAllApplicants(int jobId)
    {
        var applications = _context.Applications
            .Where(o => o.JobId == jobId)
            .ToList();

        var tasks = applications
            .AsParallel()
            .Select(async o =>
            {
                var resumeUrl = _context.AppFiles
                    .SingleOrDefault(file => file.Id == o.ResumeId && file.AssociatedId == (int) AppFileType.Resume)?
                    .Url;
                var userId = o.UserId;
                var userInfo = await _feignClient.GetUserInfo(userId);
                if (!userInfo.Code.Equals(ResultCode.Success().Code))
                {
                    throw new ApiException($"不存在id为{userId}的用户");
                }

                return new ApplicantVo
                {
                    ResumeUrl = resumeUrl,
                    UserDto = userInfo.Data!
                };
            });

        return new List<ApplicantVo>(await Task.WhenAll(tasks));
    }

    public async Task<PositionDetailVo> GetPositionDetails(int unifiedId, int jobId)
    {
        var position = _context.Positions.SingleOrDefault(o => o.Id == jobId);
        if (position == null)
        {
            throw new ApiException($"不存在id为{jobId}的岗位");
        }

        var application = _context.Applications.SingleOrDefault(o => o.UserId == unifiedId);

        return new PositionDetailVo
        {
            IfApplied = application == null ? 0 : 1,
            Position = position
        };
    }

    public async Task<List<PositionVo>> GetCompanyAllPosition(int unifiedId, int? momentId)
    {
        Func<Position, bool> predicate;

        if (momentId != null)
        {
            predicate = o => o.EnterpriseId == unifiedId && o.Id <= momentId;
        }
        else
        {
            predicate = o => o.EnterpriseId == unifiedId;
        }

        //需要降序排列所有的岗位，并且只要前九个
        var positions = _context.Positions
            .Where(predicate)
            .OrderByDescending(o => o.Id)
            .Take(9)
            .ToList();

        var userInfo = await _feignClient.GetUserInfo(unifiedId);

        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException($"不存在id为{unifiedId}的用户");
        }

        //返回企业部分信息和全部岗位
        return positions.Select(o => new PositionVo
        {
            Position = o,
            UserDto = userInfo.Data!
        }).ToList();
    }
}