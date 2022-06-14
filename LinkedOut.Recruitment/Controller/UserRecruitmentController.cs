using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Recruitment.Domain;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Controller;

[Route("")]
[ApiController]
public class UserRecruitmentController : ControllerBase
{
    private readonly IUserRecruitmentService _userRecruitmentService;


    public UserRecruitmentController(IUserRecruitmentService userRecruitmentService)
    {
        _userRecruitmentService = userRecruitmentService;
    }

    [HttpPost("application", Name = "用户申请岗位")]
    public async Task<MessageModel<object>> Apply([FromBody] ApplicationVo applicationVo)
    {
        var userId = applicationVo.UserId;
        var jobId = applicationVo.JobId;
        var resumeId = applicationVo.ResumeId;
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
    
    
    
}