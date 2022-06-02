using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Manager;

public class UserInfoManager
{
    private readonly LinkedOutContext _context;

    public UserInfoManager(LinkedOutContext context)
    {
        _context = context;
    }
    
    public UserInfo? GetUserInfoById(int unifiedId)
    {
        return _context.UserInfos.Select(o => o)
            .SingleOrDefault(o => o.UnifiedId == unifiedId);
    }
    
    public UserInfoVo<string> CombineUserAndUserInfo(int isSubscribed,DB.Entity.User userById, UserInfo userInfoById)
    {
        return new UserInfoVo<string>
        {
            IsSubscribed = isSubscribed,
            UnifiedId = userById.UnifiedId,
            Avatar = userById.Avatar,
            Age = userInfoById.Age,
            Gender = userInfoById.Gender,
            IdCard = userInfoById.IdCard,
            LivePlace = userInfoById.LivePlace,
            PrePosition = userInfoById.PrePosition,
            Back = userById.Background,
            Email = userById.Email,
            BriefInfo = userById.BriefInfo,
            PhoneNum = userInfoById.PhoneNum,
            TrueName = userById.TrueName
        };
    }

}