using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
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
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ValidateException("密码不能为空");
        }

        if (string.IsNullOrWhiteSpace(user.UserType))
        {
            throw new ValidateException("用户类型不能为空");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ValidateException("邮箱不能为空");
        }
        
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            throw new ValidateException("用户名不能为空");
        }
        

        return MessageModel<int>.Success(await _userService.Register(user));
    }

    [NoTransaction]
    [HttpPost("login", Name = "登录")]
    public async Task<MessageModel<object>> Login([FromBody] UserLoginVo user)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ValidateException("密码不能为空");
        }

        if (string.IsNullOrWhiteSpace(user.UserType))
        {
            throw new ValidateException("用户类型不能为空");
        }
        
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            throw new ValidateException("用户名不能为空");
        }

        var response = HttpContext.Response;

        await _userService.Login(user, response);

        return MessageModel.Success();
    }

    [HttpGet("search",Name ="搜索用户")]
    public async Task<MessageModel<List<UserVo<string>>>> SearchUser([Required] string keyword)
    {
        var searchUsers = await _userService.SearchUser(keyword);

        return MessageModel<List<UserVo<string>>>.Success(searchUsers);
    }

    [NoTransaction]
    [HttpGet("get", Name = "获取用户基本信息")]
    public async Task<MessageModel<UserVo<string>>> QueryUserById([Required] int unifiedId)
    {
        var userBasicInfo = await _userService.GetUserBasicInfo(unifiedId);
        return MessageModel<UserVo<string>>.Success(userBasicInfo);
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

    [NoTransaction]
    [HttpGet("recommend",Name="获取推荐关注列表")]
    public async Task<MessageModel<List<SubscribeUserVo>>> QueryRecommendList([Required] int unifiedId)
    {
        var recommendUserVos = await _userService.GetRecommendList(unifiedId);
        
        return MessageModel<List<SubscribeUserVo>>.Success(recommendUserVos);
    }

    [NoTransaction]
    [HttpGet("follow",Name = "获取关注列表")]
    public async Task<MessageModel<List<SubscribeUserVo>>> QuerySubscribeList([Required] int unifiedId)
    {
        var subscribeUserVos = await _userService.GetSubscribeList(unifiedId);
        
        return MessageModel<List<SubscribeUserVo>>.Success(subscribeUserVos);
    }


    [HttpGet("fans", Name = "获取粉丝列表")]
    public async Task<MessageModel<List<SubscribeUserVo>>> QueryFansList([Required] int unifiedId)
    {
        var subscribeUserVos = await _userService.GetFansList(unifiedId);

        return MessageModel<List<SubscribeUserVo>>.Success(subscribeUserVos);
    }


}