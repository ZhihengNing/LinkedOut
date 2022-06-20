
using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;

public class PositionManager
{
    private readonly IUserFeignClient _userFeignClient;

    public PositionManager(IUserFeignClient userFeignClient)
    {
        _userFeignClient = userFeignClient;
    }
    
    public async Task<PositionVo> GetCompanyPosition(Position? position)
    {
        if (position == null)
        {
            throw new ApiException("不存在这样的岗位");
        }

        var enterpriseId = position.EnterpriseId;

        var userInfo = await _userFeignClient.GetUserInfo(enterpriseId);

        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException("不存在");
        }

        return new PositionVo
        {
            UserDto = userInfo.Data!,
            Position = position
        };
    }
    
}