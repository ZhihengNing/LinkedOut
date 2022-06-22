using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.Recruitment.Dto;
using SummerBoot.Feign.Attributes;

namespace LinkedOut.Common.Feign.Recruitment;

[FeignClient( ServiceName = "LinkedOut.Recruitment", 
    MicroServiceMode = true,
    NacosGroupName = "DEFAULT_GROUP",
    NacosNamespaceId = "dev")
]
public interface IRecruitFeignClient
{
    [GetMapping("/feign/position")]
    Task<MessageModel<List<PositionDto>>> GetPosition([Query]int unifiedId);
}