using LinkedOut.Common.Domain;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
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

    private readonly UserInfoManager _userInfoManager;
    
    public UserService(UserManager userManager, LinkedOutContext context, SubscribedManager subscribedManager, UserInfoManager userInfoManager){
        _userManager = userManager;
        _context = context;
        _subscribedManager = subscribedManager;
        _userInfoManager = userInfoManager;
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
                    UnifiedId = unifiedId
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
        Console.WriteLine(token);
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

    public async Task<UserInfoVo<string>> GetUserInfo(int firstUserId, int secondUserId)
    {
        var userVo = new UserInfoVo<string>();
        
        var userInfoById = _userInfoManager.GetUserInfoById(secondUserId);
        var userById = _userManager.GetUserById(secondUserId);
        
        if (userInfoById == null|| userById==null)
        {
            throw new ApiException($"{firstUserId}或{secondUserId}为ID的某个用户不存在");
        }

        var relation = _subscribedManager.GetRelation(firstUserId, secondUserId);
        return _userInfoManager.CombineUserAndUserInfo((int)relation,userById, userInfoById);

    }

    public async Task UpdateUserInfo(UserInfoVo<IFormFile> userVo)
    {
        var unifiedId = userVo.unifiedId;

        var userById = _userManager.GetUserById(unifiedId);
        var userInfoById = _userInfoManager.GetUserInfoById(unifiedId);
        if (userById == null||userInfoById==null)
        {
            throw new ApiException($"id为{unifiedId}用户不存在");
        }
        
        var password = userVo.password;
        if (!string.IsNullOrEmpty(password))
        {
            userById.Password = password;
        }

        var idCard = userVo.idCard;
        if (string.IsNullOrEmpty(idCard))
        {
            userInfoById.IdCard = idCard;
        }

        var age = userVo.age;
        if (age != null)
        {
            userInfoById.Age = age;
        }

        var email = userVo.email;
        if (!string.IsNullOrEmpty(email))
        {
            userById.Email = email;
        }

        var gender = userVo.gender;
        if (!string.IsNullOrEmpty(gender))
        {
            userInfoById.Gender = gender;
        }

        var briefInfo = userVo.briefInfo;

        if (!string.IsNullOrEmpty(briefInfo))
        {
            userById.BriefInfo = briefInfo;
        }

        var livePlace = userVo.livePlace;
        if (!string.IsNullOrEmpty(livePlace))
        {
            userInfoById.LivePlace = livePlace;
        }

        var phoneNum = userVo.phoneNum;
        if (phoneNum!=null)
        {
            userInfoById.PhoneNum = phoneNum;
        }

        var prePosition = userVo.prePosition;
        if (!string.IsNullOrEmpty(prePosition))
        {
            userInfoById.PrePosition = prePosition;
        }

        var trueName = userVo.trueName;
        if (!string.IsNullOrEmpty(trueName))
        {
            userById.TrueName = trueName;
        }

        var uploadAvatar = Task.Run(() =>
        {
            var avatar = userVo.avatar;
            if (avatar == null) return;
            var url = OssHelper.UploadFile(avatar, BucketType.Avatar, unifiedId);
            userById.Avatar = url;
        });

        var uploadBack = Task.Run(() =>
        {
            var background = userVo.back;
            if (background == null) return;
            var url = OssHelper.UploadFile(background, BucketType.Back, unifiedId);
            userById.Background = url;
        });
        
        await Task.WhenAll(uploadAvatar, uploadBack);
        await _context.SaveChangesAsync();
        
    }

    public Task<DB.Entity.User> GetUserOeEnterpriseInfo(int unifiedId)
    {
        var user = _context.Users.Select(o =>o)
            .SingleOrDefault(o=>o.UnifiedId==unifiedId);
        if (user == null)
        {
            throw new ApiException($"id为{unifiedId}的用户不存在");
        }

        return Task.FromResult(user);
    }
}