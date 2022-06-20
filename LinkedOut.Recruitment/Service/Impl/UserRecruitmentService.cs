using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service.Impl;

public class UserRecruitmentService : IUserRecruitmentService
{

    private readonly LinkedOutContext _context;

    private readonly IUserFeignClient _userFeignClient;

    private readonly PositionManager _positionManager;

    public UserRecruitmentService(LinkedOutContext context, IUserFeignClient userFeignClient,
        PositionManager positionManager)
    {
        _context = context;
        _userFeignClient = userFeignClient;
        _positionManager = positionManager;
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

        var positionVos = jobIds.AsParallel().Select(async o =>
        {
            var position = _context.Positions.SingleOrDefault(p => p.Id == o);

            return await _positionManager.GetCompanyPosition(position);
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
        var positions = _context.Positions.ToList();

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
                if (prePosition.Count == 0)
                {
                    break;
                }

                //岗位按照实践顺序排列
                positions = _context.Positions
                    .Where(o => o.PositionType != null && prePosition.Contains(o.PositionType))
                    .ToList();
                break;
        }

        //先筛选，后排序，再选前面10个
        return positions.AsParallel()
            .Where(o => o.Id < momentId)
            .OrderByDescending(o => o.Id)
            .Select(o => _positionManager.GetCompanyPosition(o).Result)
            .Take(10)
            .ToList();

    }
}