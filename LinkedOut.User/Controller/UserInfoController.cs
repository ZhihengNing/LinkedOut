using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.Common.Exception;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Controller;

[ApiController]
[Route("userInfo")]
public class UserInfoController : ControllerBase
{

    private readonly IUserInfoService _userInfoService;


    public UserInfoController(IUserInfoService userInfoService)
    {
        _userInfoService = userInfoService;
    }

    [NoTransaction]
    [HttpGet("", Name = "获取用户信息")]
    public async Task<MessageModel<UserInfoVo<string>>> QueryUserInfo([Required] int uid, [Required] int sid)
    {
        var userInfo = await _userInfoService.GetUserInfo(uid, sid);

        return MessageModel<UserInfoVo<string>>.Success(userInfo);
    }

    [HttpPost("", Name = "修改用户信息")]
    public async Task<MessageModel<object>> ModifyUserInfo([FromForm] UserInfoVo<IFormFile> userVo)
    {
        if (userVo.UnifiedId == null)
        {
            throw new ValidateException("用户Id不能为空");
        }
        await _userInfoService.UpdateUserInfo(userVo);

        return MessageModel.Success();
    }

    [NoTransaction]
    [HttpGet("", Name = "查询用户信息")]
    public async Task<MessageModel<DB.Entity.User>> QueryUserById([Required] int unifiedId)
    {
        
        return MessageModel<DB.Entity.User>.Success();
    }
}