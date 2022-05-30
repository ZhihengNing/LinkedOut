using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IEnterpriseService
{
    Task<EnterpriseInfoVo<string>> GetEnterpriseInfo(int uid,int sid);

    Task UpdateEnterpriseInfo(EnterpriseInfoVo<IFormFile> enterpriseInfoVo);
    
}