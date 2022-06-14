using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Manager;
using Microsoft.EntityFrameworkCore;

namespace LinkedOut.User.Service.Impl;

public class EnterpriseService : IEnterpriseService
{

    private readonly EnterpriseInfoManager _enterpriseInfoManager;

    private readonly UserManager _userManager;

    private readonly SubscribedManager _subscribedManager;

    private readonly LinkedOutContext _context;

    public EnterpriseService(LinkedOutContext context, EnterpriseInfoManager enterpriseInfoManager, SubscribedManager subscribedManager, UserManager userManager)
    {
        _context = context;
        _enterpriseInfoManager = enterpriseInfoManager;
        _subscribedManager = subscribedManager;
        _userManager = userManager;
    }

    public async Task<EnterpriseInfoVo<string>> GetEnterpriseInfo(int uid, int sid)
    {
        var enterpriseInfoById = _enterpriseInfoManager.GetEnterpriseInfoById(sid);
        var enterpriseById = _userManager.GetUserById(sid);
        if (enterpriseInfoById == null || enterpriseById == null)
        {
            throw new ApiException($"不存在{sid}的企业");
        }

        var enterpriseInfoVo = new EnterpriseInfoVo<string>();
        var (subscribedState, _) = _subscribedManager.GetRelation(uid, sid);

        var fansNum = _context.Subscribeds.CountAsync(o => o.FirstUserId == sid);

        var followNum = _context.Subscribeds.CountAsync(o => o.SecondUserId == sid);

        var whenAll = await Task.WhenAll(fansNum, followNum);
        var combineEnterAndEnterInfo = _enterpriseInfoManager
            .CombineEnterpriseAndInfo((int) subscribedState, whenAll[0], whenAll[1], enterpriseById,
                enterpriseInfoById);

        return combineEnterAndEnterInfo;
    }


    public async Task UpdateEnterpriseInfo(EnterpriseInfoVo<IFormFile> enterpriseInfoVo)
    {
        var unifiedId = (int) enterpriseInfoVo.UnifiedId;

        var userById = _userManager.GetUserById(unifiedId);
        var enterpriseInfoById = _enterpriseInfoManager.GetEnterpriseInfoById(unifiedId);
        if (userById == null || enterpriseInfoById == null)
        {
            throw new ApiException($"id为{unifiedId}企业不存在");
        }

        var password = enterpriseInfoVo.Password;
        if (!string.IsNullOrEmpty(password))
        {
            userById.Password = password;
        }

        var email = enterpriseInfoVo.Email;
        if (!string.IsNullOrEmpty(email))
        {
            userById.Email = email;
        }

        var trueName = enterpriseInfoVo.TrueName;
        if (!string.IsNullOrEmpty(trueName))
        {
            userById.TrueName = trueName;
        }
        
        var briefInfo = enterpriseInfoVo.BriefInfo;
        userById.BriefInfo = briefInfo;

        var description = enterpriseInfoVo.Description;
        enterpriseInfoById.Description = description;

        var contactWay = enterpriseInfoVo.ContactWay;
        enterpriseInfoById.ContactWay = contactWay;

        var uploadAvatar = Task.Run(() =>
        {
            var avatar = enterpriseInfoVo.Avatar;
            if (avatar == null) return;
            var url = OssHelper.UploadFile(avatar, BucketType.Avatar, unifiedId!);
            userById.Avatar = url;
        });

        var uploadBack = Task.Run(() =>
        {
            var background = enterpriseInfoVo.Back;
            if (background == null) return;
            var url = OssHelper.UploadFile(background, BucketType.Back, unifiedId);
            userById.Background = url;
        });

        await Task.WhenAll(uploadAvatar, uploadBack);
        await _context.SaveChangesAsync();
    }
}