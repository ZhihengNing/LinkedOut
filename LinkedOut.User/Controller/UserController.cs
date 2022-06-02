using System.ComponentModel.DataAnnotations;
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

    [HttpPost("register", Name = "注册")]
    public async Task<MessageModel<int>> Register([FromBody] DB.Entity.User user)
    {
        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidateException("密码不能为空");
        }

        if (string.IsNullOrEmpty(user.UserType))
        {
            throw new ValidateException("用户类型不能为空");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new ValidateException("邮箱不能为空");
        }
        
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ValidateException("用户名不能为空");
        }
        

        return MessageModel<int>.Success(await _userService.Register(user));
    }

    [NoTransaction]
    [HttpPost("login", Name = "登录")]
    public async Task<MessageModel<object>> Login([FromBody] UserLoginVo user)
    {
        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidateException("密码不能为空");
        }

        if (string.IsNullOrEmpty(user.UserType))
        {
            throw new ValidateException("用户类型不能为空");
        }
        
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ValidateException("用户名不能为空");
        }

        var response = HttpContext.Response;

        await _userService.Login(user, response);

        return MessageModel.Success();
    }

    [NoTransaction]
    [HttpPost("email", Name = "发送邮件")]
    public async Task<MessageModel<string>> SendEmail([Required] string email)
    {
        var result = await _userService.SendEmail(email);

        return MessageModel<string>.Success(result);
    }

    [HttpPut("subscription",Name="关注某人")]
    public async Task<MessageModel<object>> SubscribeUser([Required] int unifiedId,
        [Required] int subscribeId)
    {
        await _userService.SubscribeUser(unifiedId,subscribeId);
        
        return MessageModel.Success();
    }

    [HttpDelete("subscription",Name="取消关注")]
    public async Task<MessageModel<object>> UnsubscribeUser([Required] int unifiedId, [Required] int subscribeId)
    {
        await _userService.UnsubscribeUser(unifiedId, subscribeId);
        
        return MessageModel.Success();
    }

    [HttpGet("recommend")]
    public async Task<MessageModel<List<RecommendUserVo>>> QueryRecommendList([Required] int unifiedId)
    {
        var recommendUserVos = await _userService.GetRecommendList(unifiedId);
        
        return MessageModel<List<RecommendUserVo>>.Success(recommendUserVos);
    }


}