using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Controller;

[Route("edu")]
[ApiController]
public  class EduController : ControllerBase
{

    private readonly IEduService _eduService;

    public EduController(IEduService eduService)
    {
        _eduService = eduService;
    }

    [NoTransaction]
    [HttpGet("",Name="查询教育经历")]
    public async Task<MessageModel<List<EduVo>>> QueryEduExperience([FromQuery] int unifiedId)
    {
        var eduExperience = await _eduService.GetEduExperience(unifiedId);
        return MessageModel<List<EduVo>>.Success(eduExperience);
    }

    [HttpPost("", Name = "增加教育经历")]
    public async Task<MessageModel<object>> AddEduExperience([FromBody] EduExperience eduExperience)
    {
        await _eduService.AddEduExperience(eduExperience);

        return MessageModel.Success();
    }

    [HttpDelete("", Name = "查出教育经历")]
    public async Task<MessageModel<object>> RemoveEduExperience([FromQuery] int eduExperienceId)
    {
        await _eduService.DeleteEduExperience(eduExperienceId);

        return MessageModel.Success();
    }

}