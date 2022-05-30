using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.Common.Domain;
using Microsoft.AspNetCore.Mvc;
using LinkedOut.Common.Exception;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Service;

namespace LinkedOut.User.Controller;

[ApiController]
[Route("")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register",Name="注册")]
    public async Task<MessageModel<int>> Register([FromBody] DB.Entity.User user)
    {
        if (user.UserName == null)
        {
            throw new ApiException("用户名不能为空");
        }

        return MessageModel<int>.Success(await _userService.Register(user));
    }

    [NoTransaction]
    [HttpPost("login",Name="登录")]
    public async Task<MessageModel<object>> Login([FromBody] UserLoginVo user)
    {
        var response = HttpContext.Response;
        
        await _userService.Login(user,response);

        return MessageModel.Success();
    }

    [NoTransaction]
    [HttpPost("email",Name="发送邮件")]
    public async Task<MessageModel<string>> SendEmail([FromQuery] string email)
    {
        var result = await _userService.SendEmail(email);
        
        return MessageModel<string>.Success(result);
    }

    [NoTransaction]
    [HttpGet("userInfo",Name="获取用户信息")]
    public async Task<MessageModel<UserInfoVo<string>>> QueryUserInfo([FromQuery] int uid, [FromQuery] int sid)
    {
        var userInfo = await _userService.GetUserInfo(uid, sid);

        return MessageModel<UserInfoVo<string>>.Success(userInfo);
    }

    [HttpPost("userInfo",Name="修改用户信息")]
    public async Task<MessageModel<object>> ModifyUserInfo([FromForm] UserInfoVo<IFormFile> userVo)
    {
        await _userService.UpdateUserInfo(userVo);

        return MessageModel.Success();
    }

    [NoTransaction]
    [HttpGet("",Name = "查询用户信息")]
    public async Task<MessageModel<DB.Entity.User>> QueryUserById([FromQuery] int unifiedId)
    {
        
        
        return MessageModel<DB.Entity.User>.Success();
    }
    
    
}