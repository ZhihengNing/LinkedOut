using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Enum;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Helper;
using LinkedOut.User.Manager;
using Microsoft.EntityFrameworkCore;
using static LinkedOut.User.Helper.EmailHelper;

namespace LinkedOut.User.Service.Impl;

public class UserService : IUserService
{

    private readonly LinkedOutContext _context;

    private readonly UserManager _userManager;

    private readonly SubscribedManager _subscribedManager;

    private readonly ILogger<UserService> _logger;

    public UserService(UserManager userManager, LinkedOutContext context, SubscribedManager subscribedManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _context = context;
        _subscribedManager = subscribedManager;
        _logger = logger;
    }

    public async Task<int> Register(DB.Entity.User user)
    {
        if (_userManager.CheckUserSame(user.UserName))
        {
            throw new ApiException("名字已经存在");
        }
        
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

    public async Task<List<UserVo<string>>> SearchUser(string keyword)
    {
        //现根据余弦相似度排序一波,然后排序的结果再取前五个
        var orderByDescending = _context.Users
            .Select(o => new
            {
                User = o,
                Score = SimilarityHelper.SimilarScoreCos(o.TrueName, keyword)
            })
            .OrderByDescending(o => o.Score)
            .Take(5)
            .ToList();

        //最后转换为我们需要的数据
        return orderByDescending.Select(o =>
        {
            var temp = o.User;
            return new UserVo<string>
            {
                UnifiedId = temp.UnifiedId,
                Avatar = temp.Avatar,
                TrueName = temp.TrueName,
                UserType = temp.UserType,
                BriefInfo = temp.BriefInfo
            };
        }).ToList();
    }

    public async Task<UserVo<string>> GetUserBasicInfo(int unifiedId)
    {
        return _context.Users.Where(o => o.UnifiedId == unifiedId)
            .Select(o => new UserVo<string>
            {
                UnifiedId = unifiedId,
                TrueName = o.TrueName,
                Avatar = o.Avatar,
                BriefInfo = o.BriefInfo,
                UserType = o.UserType,
                Back = o.Background
            }).SingleOrDefault()!;
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

    public async Task<UserDto> GetUserOrEnterpriseInfo(int? unifiedId)
    {
        var user = _context.Users.SingleOrDefault(o => o.UnifiedId == unifiedId);
        if (user == null)
        {
            throw new ApiException($"id为{unifiedId}的用户不存在");
        }

        var userDto = new UserDto
        {
            UnifiedId = user.UnifiedId,
            BriefInfo = user.BriefInfo,
            PictureUrl = user.Avatar,
            TrueName = user.TrueName,
            UserType = user.UserType
        };

        return userDto;
    }

    public async Task SubscribeUser(int unifiedId, int subscribeId)
    {
        var (state, _) = _subscribedManager.GetRelation(unifiedId, subscribeId);
        
        switch (state)
        {
            case SubscribedState.SubScribed:
                throw new ApiException("不能重复关注噢");
            case SubscribedState.Same:
                throw new ApiException("不能自己关注自己");
            case SubscribedState.NoSubscribed:
            default:
                var newRelation = new Subscribed
                {
                    FirstUserId = unifiedId,
                    SecondUserId = subscribeId
                };
                await _context.Subscribeds.AddAsync(newRelation);
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

    public async Task<List<SubscribeUserVo>> GetRecommendList(int unifiedId)
    {
        //根据自定义规则推荐用户，并给出推荐分值
        var userInfos = _context.Users
            .Select(o => new
            {
                User = o,
                Score = _subscribedManager.RecommendScore(unifiedId, o,o.UserType)
            })
            .ToList();

        //降序排列，去除掉已经关注过的,最后拿前五个出来
        return userInfos
            .OrderByDescending(o => o.Score)
            .Where(o =>
            {
                var user = o.User;
                var (subscribedState, _) =
                    _subscribedManager.GetRelation(unifiedId, user.UnifiedId);
                return subscribedState.Equals(SubscribedState.NoSubscribed);
            })
            .Select(o =>
            {
                var user = o.User;
                return new SubscribeUserVo
                {
                    UnifiedId = user.UnifiedId,
                    TrueName = user.TrueName,
                    UserAvatar = user.Avatar,
                    BriefInfo = user.BriefInfo,
                    UserType = user.UserType
                };
            })
            .Take(5)
            .ToList();
    }

    public async Task<List<SubscribeUserVo>> GetSubscribeList(int unifiedId)
    {
        //获取用户关注的列表的每个用户Id
        var subscribedUsers = _context.Subscribeds
            .Where(o => o.FirstUserId == unifiedId)
            .Select(o => o.SecondUserId)
            .ToList();

        //对于关注列表的每个人获取他们的基本信息
        return subscribedUsers.Select(secondUserId =>
        {
            var user = _context.Users.Single(u => u.UnifiedId == secondUserId);
            return new SubscribeUserVo
            {
                UnifiedId = secondUserId,
                TrueName = user.TrueName,
                UserAvatar = user.Avatar,
                BriefInfo = user.BriefInfo,
                UserType = user.UserType,
                IsSubscribed = true
            };
        }).ToList();

    }

    public async Task<List<SubscribeUserVo>> GetFansList(int unifiedId)
    {
        //获取用户粉丝列表每个用户Id
        var subscribedUsers = _context.Subscribeds
            .Where(o => o.SecondUserId == unifiedId)
            .Select(o => o.FirstUserId)
            .ToList();

        //对于关注列表的每个人获取他们的基本信息
        return subscribedUsers.Select(secondUserId =>
        {
            var user = _context.Users.Single(u => u.UnifiedId == secondUserId);
            return new SubscribeUserVo
            {
                UnifiedId = secondUserId,
                TrueName = user.TrueName,
                UserAvatar = user.Avatar,
                BriefInfo = user.BriefInfo,
                UserType = user.UserType,
                IsSubscribed = true
            };
        }).ToList();
    }


    public async Task<List<UserDto>> GetSubscribeUserIds(int unifiedId)
    {
        var list = _context.Subscribeds
            .Where(o => o.FirstUserId == unifiedId)
            .Join(_context.Users,
                s => s.SecondUserId,
                u => u.UnifiedId,
                (s, u) => new UserDto
                {
                    BriefInfo = u.BriefInfo,
                    PictureUrl = u.Avatar,
                    TrueName = u.TrueName,
                    UnifiedId = u.UnifiedId,
                    UserType = u.UserType
                }).ToList();
        //还要包括自己的信息
        var userDto = await GetUserOrEnterpriseInfo(unifiedId);
        list.Add(userDto);
        return list;
    }

    public async Task<List<string>> GetPrePosition(int unifiedId)
    {
        var userInfo = _context.UserInfos.SingleOrDefault(o => o.UnifiedId == unifiedId);

        if (userInfo == null)
        {
            throw new ApiException($"不存在对应{unifiedId}的用户");
        }

        var prePosition = userInfo.PrePosition;

        return string.IsNullOrWhiteSpace(prePosition) ? new List<string>() : prePosition.Split(",").ToList();
    }
}