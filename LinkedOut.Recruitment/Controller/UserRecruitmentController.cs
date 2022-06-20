using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Recruitment.Domain;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Controller;

[Route("user")]
[ApiController]
public class UserRecruitmentController : ControllerBase
{
    private readonly IUserRecruitmentService _userRecruitmentService;

    private readonly IEnterprisePositionService _enterprisePositionService;
    
    public UserRecruitmentController(IUserRecruitmentService userRecruitmentService, IEnterprisePositionService enterprisePositionService)
    {
        _userRecruitmentService = userRecruitmentService;
        _enterprisePositionService = enterprisePositionService;
    }

    [HttpPost("application", Name = "用户申请岗位")]
    public async Task<MessageModel<object>> Apply([FromBody] AddApplicationVo addApplicationVo)
    {
        var userId = addApplicationVo.UserId;
        var jobId = addApplicationVo.JobId;
        var resumeId = addApplicationVo.ResumeId;
        if (userId == null)
        {
            throw new ValidateException("userId不能为空");
        }

        if (jobId == null)
        {
            throw new ValidateException("jobId不能为空");
        }

        if (resumeId == null)
        {
            throw new ValidateException("简历Id不能为空");
        }

        await _userRecruitmentService.ApplyPosition((int) userId, (int) resumeId, (int) jobId);

        return MessageModel.Success();
    }

    [HttpDelete("application",Name = "用户取消岗位申请")]
    public async Task<MessageModel<object>> CancelApplication([Required] int unifiedId,[Required] int jobId)
    {
        await _userRecruitmentService.CancelApplication(unifiedId,jobId);
        return MessageModel.Success();
    }

    [HttpGet("application",Name = "用户获取已经投递的所有岗位信息")]
    public async Task<MessageModel<List<PositionVo>>> QueryPostedRecruitment([Required] int unifiedId)
    {
        var postedPosition = await _userRecruitmentService.GetPostedPosition(unifiedId);

        return MessageModel<List<PositionVo>>.Success(postedPosition);
    }

    [HttpGet("position/specified",Name = "用户获取岗位详情")]
    public async Task<MessageModel<PositionDetailVo>> QueryPositionDetail([Required] int unifiedId,[Required] int jobId)
    {
        var positionDetailVo = await _enterprisePositionService.GetPositionDetails(unifiedId,jobId);
        
        return MessageModel<PositionDetailVo>.Success(positionDetailVo);
    }

    [HttpGet("position/recommend",Name = "用户获取推荐岗位信息")]
    public async Task<MessageModel<List<PositionVo>>> QueryRecommendPosition([Required] int unifiedId, int? momentId)
    {
        var recommendPosition = await _userRecruitmentService.GetRecommendPosition(unifiedId,momentId);
        
        return MessageModel<List<PositionVo>>.Success(recommendPosition);
    }
    
}