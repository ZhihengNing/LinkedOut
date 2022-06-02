using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Manager;

namespace LinkedOut.User.Service.Impl;

public class UserInfoService : IUserInfoService
{
    private readonly UserInfoManager _userInfoManager;

    private readonly UserManager _userManager;

    private readonly SubscribedManager _subscribedManager;

    private readonly LinkedOutContext _context;

    public UserInfoService(UserInfoManager userInfoManager, UserManager userManager, LinkedOutContext context, SubscribedManager subscribedManager)
    {
        _userInfoManager = userInfoManager;
        _userManager = userManager;
        _context = context;
        _subscribedManager = subscribedManager;
    }

    public async Task<UserInfoVo<string>> GetUserInfo(int firstUserId, int secondUserId)
    {
        var userVo = new UserInfoVo<string>();

        var userInfoById = _userInfoManager.GetUserInfoById(secondUserId);
        var userById = _userManager.GetUserById(secondUserId);

        if (userInfoById == null || userById == null)
        {
            throw new ApiException($"{firstUserId}或{secondUserId}为ID的某个用户不存在");
        }

        var (state, _) = _subscribedManager.GetRelation(firstUserId, secondUserId);
        return _userInfoManager.CombineUserAndUserInfo((int) state, userById, userInfoById);

    }

    public async Task UpdateUserInfo(UserInfoVo<IFormFile> userVo)
    {
        var unifiedId = (int) userVo.UnifiedId;

        var userById = _userManager.GetUserById(unifiedId);
        var userInfoById = _userInfoManager.GetUserInfoById(unifiedId);
        if (userById == null || userInfoById == null)
        {
            throw new ApiException($"id为{unifiedId}用户不存在");
        }

        var password = userVo.Password;
        if (!string.IsNullOrEmpty(password))
        {
            userById.Password = password;
        }

        var idCard = userVo.IdCard;
        if (string.IsNullOrEmpty(idCard))
        {
            userInfoById.IdCard = idCard;
        }

        var age = userVo.Age;
        if (age != null)
        {
            userInfoById.Age = age;
        }

        var email = userVo.Email;
        if (!string.IsNullOrEmpty(email))
        {
            userById.Email = email;
        }

        var gender = userVo.Gender;
        if (!string.IsNullOrEmpty(gender))
        {
            userInfoById.Gender = gender;
        }

        var briefInfo = userVo.BriefInfo;

        if (!string.IsNullOrEmpty(briefInfo))
        {
            userById.BriefInfo = briefInfo;
        }

        var livePlace = userVo.LivePlace;
        if (!string.IsNullOrEmpty(livePlace))
        {
            userInfoById.LivePlace = livePlace;
        }

        var phoneNum = userVo.PhoneNum;
        if (phoneNum != null)
        {
            userInfoById.PhoneNum = phoneNum;
        }

        var prePosition = userVo.PrePosition;
        if (!string.IsNullOrEmpty(prePosition))
        {
            userInfoById.PrePosition = prePosition;
        }

        var trueName = userVo.TrueName;
        if (!string.IsNullOrEmpty(trueName))
        {
            userById.TrueName = trueName;
        }

        var uploadAvatar = Task.Run(() =>
        {
            var avatar = userVo.Avatar;
            if (avatar == null) return;
            var url = OssHelper.UploadFile(avatar, BucketType.Avatar, unifiedId);
            userById.Avatar = url;
        });

        var uploadBack = Task.Run(() =>
        {
            var background = userVo.Back;
            if (background == null) return;
            var url = OssHelper.UploadFile(background, BucketType.Back, unifiedId);
            userById.Background = url;
        });

        await Task.WhenAll(uploadAvatar, uploadBack);
        await _context.SaveChangesAsync();

    }
}