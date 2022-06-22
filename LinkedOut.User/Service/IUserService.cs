using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IUserService
{

    Task<int> Register(DB.Entity.User user);

    Task<UserLoginDetailVo> Login(UserLoginVo user,HttpResponse response);

    Task<List<UserVo<string>>> SearchUser(string? keyword);

    Task<UserVo<string>> GetUserBasicInfo(int unifiedId);

    Task<string> SendEmail(string email);

    Task<UserDto> GetUserOrEnterpriseInfo(int? unifiedId);

    Task SubscribeUser(int unifiedId, int subscribeId);

    Task UnsubscribeUser(int unifiedId, int subscribeId);

    Task<List<SubscribeUserVo>> GetRecommendList(int unifiedId);

    Task<List<SubscribeUserVo>> GetSubscribeList(int unifiedId);

    Task<List<SubscribeUserVo>> GetFansList(int unifiedId);

    Task<List<UserDto>> GetSubscribeUserIds(int unifiedId);

    Task<string?> GetPrePosition(int unifiedId);
}