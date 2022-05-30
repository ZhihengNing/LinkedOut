using LinkedOut.Common.Domain;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IUserService
{

    Task<int> Register(DB.Entity.User user);

    Task Login(UserLoginVo user,HttpResponse response);

    Task<string> SendEmail(string email);

    Task<UserInfoVo<string>> GetUserInfo(int firstUserId, int secondUserId);

    Task UpdateUserInfo(UserInfoVo<IFormFile> userVo);

    Task<DB.Entity.User> GetUserOeEnterpriseInfo(int unifiedId);

}