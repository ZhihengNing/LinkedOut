using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.Exception;
using LinkedOut.Common.Feign.Filter;
using LinkedOut.Common.Feign.User.Dto;
using SummerBoot.Feign.Attributes;

namespace LinkedOut.Common.Feign.User;

[FeignClient( ServiceName = "LinkedOut.User", 
    MicroServiceMode = true,
    NacosGroupName = "DEFAULT_GROUP",
    NacosNamespaceId = "dev")
]

public interface IUserFeignClient
{
    [GetMapping("/feign/userInfo")]
    Task<MessageModel<UserDto>> GetUserInfo([Query] int unifiedId);

    [FeignHandle]
    [GetMapping("/feign/demo")]
    Task<MessageModel<string>> Demo([Query] string a);
}
