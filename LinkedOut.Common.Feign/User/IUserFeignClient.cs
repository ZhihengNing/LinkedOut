using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.Exception;
using LinkedOut.Common.Feign.User.Dto;
using SummerBoot.Feign.Attributes;

namespace LinkedOut.Common.Feign.User;
//这里如果无法调用可能是命名空间出了问题

[FeignClient( ServiceName = "LinkedOut.User", 
    MicroServiceMode = true,
    NacosGroupName = "DEFAULT_GROUP",
    NacosNamespaceId = "dev")
]

public interface IUserFeignClient
{
    [GetMapping("/feign/userInfo")]
    Task<MessageModel<UserDto>> GetUserInfo([Query] int? unifiedId);

    [GetMapping("/feign/prePosition")]
    Task<MessageModel<List<string>>> GetUserPrePosition([Query] int unifiedId);

    [GetMapping("/feign/subscribe")]
    Task<MessageModel<List<UserDto>>> GetSubscribeUserId([Query] int unifiedId);
    
    [GetMapping("/feign/demo")]
    Task<MessageModel<string>> Demo([Query] string a);
}
