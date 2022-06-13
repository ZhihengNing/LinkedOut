using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Enum;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Helper;
using LinkedOut.User.Manager;
using static LinkedOut.User.Helper.EmailHelper;

namespace LinkedOut.User.Service.Impl;

public class UserService : IUserService
{

    private readonly LinkedOutContext _context;

    private readonly UserManager _userManager;

    private readonly SubscribedManager _subscribedManager;

    private readonly RecommendManager _recommendManager;

    public UserService(UserManager userManager, LinkedOutContext context, SubscribedManager subscribedManager, RecommendManager recommendManager)
    {
        _userManager = userManager;
        _context = context;
        _subscribedManager = subscribedManager;
        _recommendManager = recommendManager;
    }

    public async Task<int> Register(DB.Entity.User user)
    {
        if (_userManager.CheckUserSame(user.UserName))
        {
            throw new ApiException("名字已经存在");
        }

        user.SubscribeNum = 0;
        _context.Add(user);
        await _context.SaveChangesAsync();
        var unifiedId = user.UnifiedId;

        switch (user.UserType)
        {
            case "user":
                await _context.UserInfos.AddAsync(new UserInfo
                {
                    UnifiedId = unifiedId
                });
                break;
            case "company":
                await _context.EnterpriseInfos.AddAsync(new EnterpriseInfo
                {
                    UnifiedId = unifiedId,
                });
                break;
            default:
                throw new ApiException("用户类型不正确");
        }

        await _context.SaveChangesAsync();

        return unifiedId;
    }

    public Task Login(UserLoginVo user, HttpResponse response)
    {
        var userName = user.UserName;
        var password = user.Password;
        var userType = user.UserType;
        var needUser = _context.Users
            .Select(o => o)
            .SingleOrDefault(o => o.UserName == userName && o.Password == password);

        if (needUser == null)
        {
            throw new ApiException("用户名或密码错误");
        }

        var token = TokenHelper.GenerateToken(needUser.UnifiedId, userName, userType);
        response.Cookies.Append("token", token);
        return Task.CompletedTask;
    }

    public Task<string> SendEmail(string email)
    {
        if (_userManager.CheckEmailExist(email))
        {
            throw new ApiException("邮箱已被注册");
        }

        var result = GeneratorCode();
        EmailHelper.SendEmail(email, result);
        return Task.FromResult(result);
    }

    public Task<UserDto> GetUserOeEnterpriseInfo(int unifiedId)
    {
        var user = _context.Users.Select(o => o)
            .SingleOrDefault(o => o.UnifiedId == unifiedId);
        if (user == null)
        {
            throw new ApiException($"id为{unifiedId}的用户不存在");
        }
        
        var userDto = new UserDto
        {
            UnifiedId = user.UnifiedId,
            UserBriefInfo = user.BriefInfo,
            UserIconUrl = user.Avatar,
            UserName = user.UserName,
            UserType = user.UserType
        };

        return Task.FromResult(userDto);
    }

    public async Task SubscribeUser(int unifiedId, int subscribeId)
    {
        var (state, subscribed) = _subscribedManager.GetRelation(unifiedId, subscribeId);
        switch (state)
        {
            case SubscribedState.SubScribed:
                throw new ApiException("不能重复关注噢");
            case SubscribedState.Same:
                throw new ApiException("不能自己关注自己");
            case SubscribedState.NoSubscribed:
            default:
                await _context.Subscribeds.AddAsync(subscribed!);
                await _context.SaveChangesAsync();
                break;
        }
    }

    public async Task UnsubscribeUser(int unifiedId, int subscribeId)
    {
        var (state, relation) = _subscribedManager.GetRelation(unifiedId, subscribeId);
        switch (state)
        {
            case SubscribedState.NoSubscribed:
                throw new ApiException("不能重复取消关注噢");
            case SubscribedState.Same:
                throw new ApiException("不能自己取消关注自己");
            case SubscribedState.SubScribed:
            default:
                _context.Subscribeds.Remove(relation!);
                await _context.SaveChangesAsync();
                break;
        }
    }

    public async Task<List<RecommendUserVo>> GetRecommendList(int unifiedId)
    {
        var subscribed = _context.Subscribeds
            .Where(o => o.FirstUserId == unifiedId)
            .ToList();
        var recommendUserVo = subscribed.Select(o =>
        {
            var secondUserId = o.SecondUserId;
            var user = _context.Users.Single(u => u.UnifiedId == secondUserId);
            return new RecommendUserVo
            {
                UnifiedId = secondUserId,
                TrueName = user.TrueName,
                UserAvatar = user.Avatar,
                UserBriefInfo = user.BriefInfo,
                UserType = user.UserType
            };
        }).ToList();
        return _recommendManager.Transfer(recommendUserVo);
    }
}