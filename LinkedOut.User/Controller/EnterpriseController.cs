using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Controller;

[ApiController]
[Route("enterpriseInfo")]
public class EnterpriseController : ControllerBase
{
    private readonly IEnterpriseService _enterpriseService;

    public EnterpriseController(IEnterpriseService enterprise)
    {
        _enterpriseService = enterprise;
    }

    [NoTransaction]
    [HttpGet("",Name = "获取企业用户信息")]
    public async Task<MessageModel<EnterpriseInfoVo<string>>> QueryEnterpriseInfo([FromQuery]int uid,[FromQuery] int sid)
    {
        var enterpriseInfoVo = await _enterpriseService.GetEnterpriseInfo(uid,sid);
        
        return MessageModel<EnterpriseInfoVo<string>>.Success(enterpriseInfoVo);
    }
    
    [HttpPost("",Name="修改企业信息")]
    public async Task<MessageModel<object>> ModifyUserInfo([FromForm] EnterpriseInfoVo<IFormFile> userVo)
    {
        await _enterpriseService.UpdateEnterpriseInfo(userVo);

        return MessageModel.Success();
    }
    
}