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
            isSubscribed = isSubscribed,
            unifiedId = userById.UnifiedId,
            avatar = userById.Avatar,
            age = userInfoById.Age,
            gender = userInfoById.Gender,
            idCard = userInfoById.IdCard,
            livePlace = userInfoById.LivePlace,
            prePosition = userInfoById.PrePosition,
            back = userById.Background,
            email = userById.Email,
            briefInfo = userById.BriefInfo,
            phoneNum = userInfoById.PhoneNum,
            trueName = userById.TrueName
        };
    }

}