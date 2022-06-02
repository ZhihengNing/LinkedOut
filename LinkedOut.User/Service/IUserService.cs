using LinkedOut.Common.Domain;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Service;

public interface IUserService
{

    Task<int> Register(DB.Entity.User user);

    Task Login(UserLoginVo user,HttpResponse response);

    Task<string> SendEmail(string email);

    Task<UserDto> GetUserOeEnterpriseInfo(int unifiedId);

    Task SubscribeUser(int unifiedId, int subscribeId);

    Task UnsubscribeUser(int unifiedId, int subscribeId);

    Task<List<RecommendUserVo>> GetRecommendList(int unifiedId);

}