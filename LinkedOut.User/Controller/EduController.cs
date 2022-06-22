using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.Common.Exception;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Controller;

[Route("edu")]
[ApiController]
public class EduController : ControllerBase
{

    private readonly IEduService _eduService;

    public EduController(IEduService eduService)
    {
        _eduService = eduService;
    }

    [NoTransaction]
    [HttpGet("", Name = "查询教育经历")]
    public async Task<MessageModel<List<EduVo>>> QueryEduExperience([Required]int unifiedId)
    {
        var eduExperience = await _eduService.GetEduExperience(unifiedId);
        return MessageModel<List<EduVo>>.Success(eduExperience);
    }

    [HttpPost("", Name = "增加教育经历")]
    public async Task<MessageModel<object>> AddEduExperience([FromBody] EduExperienceVo eduExperience)
    {
        if (eduExperience.UnifiedId == null)
        {
            throw new ValidateException("用户Id不能为空");
        }

        if (eduExperience.CollegeName == null)
        {
            throw new ValidateException("学校名称不能为空");
        }
        await _eduService.AddEduExperience(eduExperience);

        return MessageModel.Success();
    }

    [HttpDelete("", Name = "删除教育经历")]
    public async Task<MessageModel<object>> RemoveEduExperience([Required] int eduExperienceId)
    {
        await _eduService.DeleteEduExperience(eduExperienceId);

        return MessageModel.Success();
    }

}