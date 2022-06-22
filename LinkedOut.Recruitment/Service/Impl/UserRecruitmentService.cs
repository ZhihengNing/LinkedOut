using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service.Impl;

class PositionScore
{
    public Position Position { get; set; }

    public double Score { get; set; }
}

public class UserRecruitmentService : IUserRecruitmentService
{
    private readonly LinkedOutContext _context;

    private readonly IUserFeignClient _userFeignClient;

    public UserRecruitmentService(LinkedOutContext context, IUserFeignClient userFeignClient)
    {
        _context = context;
        _userFeignClient = userFeignClient;
    }

    private async Task<PositionVo> GetCompanyPosition(Position? position)
    {
        if (position == null)
        {
            throw new ApiException("不存在这样的岗位");
        }

        var enterpriseId = position.UnifiedId;

        var userInfo = await _userFeignClient.GetUserInfo(enterpriseId);

        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException("不存在");
        }

        return new PositionVo
        {
            UserDto = userInfo.Data!,
            Position = position
        };
    }

    public async Task ApplyPosition(int userId, int resumeId, int jobId)
    {
        var application = new Application()
        {
            UserId = userId,
            ResumeId = resumeId,
            JobId = jobId
        };
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
    }

    public async Task CancelApplication(int unifiedId, int jobId)
    {
        var application = _context.Applications
            .SingleOrDefault(o => o.UserId == unifiedId && o.JobId == jobId);

        if (application == null)
        {
            throw new ApiException("取消岗位申请失败");
        }

        _context.Applications.Remove(application);

        await _context.SaveChangesAsync();
    }

    public async Task<List<PositionVo>> GetPostedPosition(int unifiedId)
    {
        var jobIds = _context.Applications
            .Where(o => o.UserId == unifiedId)
            .Select(o => o.JobId).ToList();

        var positionVos = jobIds.Select(async o =>
        {
            var position = _context.Positions.SingleOrDefault(p => p.Id == o);

            return await GetCompanyPosition(position);
        });

        return new List<PositionVo>(await Task.WhenAll(positionVos));
    }

    public async Task<List<PositionVo>> GetRecommendPosition(int unifiedId, int? momentId)
    {
        //先获取用户的基本信息
        var userInfo = await _userFeignClient.GetUserInfo(unifiedId);

        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException(userInfo.Message);
        }

        var userDto = userInfo.Data!;
        var userType = userDto.UserType;

        //找出所有的岗位
        var positions = _context
            .Positions
            .Select(o => new PositionScore
            {
                Position = o,
                Score = 0.0
            })
            .ToList();

        switch (userType)
        {
            case "company":
                break;
            case "school":
                break;
            case "user":
                var userPrePosition = await _userFeignClient.GetUserPrePosition(unifiedId);
                if (!userPrePosition.Code.Equals(ResultCode.Success().Code))
                {
                    throw new ApiException("不存在对应的userInfo");
                }

                var prePosition = userPrePosition.Data!;
                //若没有职位偏好，返回一个随机的结果

                positions.ForEach(o =>
                {
                    o.Score = SimilarityHelper.SimilarScoreCos(prePosition, o.Position.PositionType);
                });
                break;
        }
        
        Func<PositionScore, bool> condition = momentId == null ? o => true : o => o.Position.Id < momentId;
        //先筛选，后排序，再选前面10个
        return positions
            .Where(condition)
            .OrderByDescending(o => o.Score)
            .ThenByDescending(o => o.Position.Id)
            .Select(o => GetCompanyPosition(o.Position).Result)
            .Take(10)
            .ToList();
    }
}