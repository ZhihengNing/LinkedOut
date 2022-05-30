using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Manager;

public class EnterpriseInfoManager
{

    private readonly LinkedOutContext _context;

    public EnterpriseInfoManager(LinkedOutContext context)
    {
        _context = context;
    }

    public EnterpriseInfo? GetEnterpriseInfoById(int unifiedId)
    {
        return _context.EnterpriseInfos.Select(o=>o)
            .SingleOrDefault(o=>o.UnifiedId==unifiedId);
    }

    public EnterpriseInfoVo<string> CombineEnterAndEnterInfo(int isSubscribed, 
        int fansNum,
        int followNum,
        DB.Entity.User userById, 
        EnterpriseInfo enterpriseInfo)
    {
        return new EnterpriseInfoVo<string>
        {
            isSubscribed = isSubscribed,
            unifiedId = userById.UnifiedId,
            avatar = userById.Avatar,
            back = userById.Background,
            briefInfo = userById.BriefInfo,
            email = userById.Email,
            trueName = userById.TrueName,
            contactWay = enterpriseInfo.ContactWay,
            description = enterpriseInfo.Description,
            fansNum = fansNum,
            followNum = followNum
        };
    }

}