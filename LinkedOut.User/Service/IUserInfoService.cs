using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IUserInfoService
{
    Task<UserInfoVo<string>> GetUserInfo(int firstUserId, int secondUserId);

    Task UpdateUserInfo(UserInfoVo<IFormFile> userVo);
}