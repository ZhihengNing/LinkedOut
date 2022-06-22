using LinkedOut.Common.Domain;
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

    public EnterpriseService(LinkedOutContext context, EnterpriseInfoManager enterpriseInfoManager,
        SubscribedManager subscribedManager, UserManager userManager)
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

        var fansNum = await _context.Subscribeds.CountAsync(o => o.SecondUserId == sid);

        var followNum = await _context.Subscribeds.CountAsync(o => o.FirstUserId == sid);
        
        var combineEnterAndEnterInfo = _enterpriseInfoManager
            .CombineEnterpriseAndInfo((int) subscribedState, fansNum, followNum, enterpriseById,
                enterpriseInfoById);

        return combineEnterAndEnterInfo;
    }


    public async Task UpdateEnterpriseInfo(EnterpriseInfoVo<IFormFile> enterpriseInfoVo)
    {
        var unifiedId = (int) enterpriseInfoVo.UnifiedId!;

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
        if (briefInfo != null)
        {
            userById.BriefInfo = briefInfo;
        }

        var description = enterpriseInfoVo.Description;
        if (description != null)
        {
            enterpriseInfoById.Description = description;
        }

        var contactWay = enterpriseInfoVo.ContactWay;
        if (contactWay != null)
        {
            enterpriseInfoById.ContactWay = contactWay;
        }

        var avatar = Task.Run(() =>
        {
            var avatar = enterpriseInfoVo.Avatar;
            if (avatar == null) return;
            var fileElement = new FileElement
            {
                File = avatar,
                BucketType = BucketType.Avatar,
                AssociateId = unifiedId
            };

            var url = OssHelper.UploadFile(fileElement);
            userById.Avatar = url;
        });


        var back = Task.Run(() =>
        {
            var background = enterpriseInfoVo.Back;
            if (background == null) return;
            var fileElement = new FileElement
            {
                File = background,
                BucketType = BucketType.Back,
                AssociateId = unifiedId
            };
            var url = OssHelper.UploadFile(fileElement);
            userById.Background = url;
        });

        await Task.WhenAll(avatar, back);
        await _context.SaveChangesAsync();
    }
}