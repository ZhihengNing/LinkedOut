using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.Recruitment;
using LinkedOut.Common.Feign.Recruitment.Dto;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Client;

[Route("feign")]
[ApiController]
public class RecruitClient : IRecruitFeignClient
{
    private readonly IEnterprisePositionService _enterprisePositionService;
    
    public RecruitClient(IEnterprisePositionService enterprisePositionService)
    {
        _enterprisePositionService = enterprisePositionService;
    }

    [HttpGet("position")]
    public async Task<MessageModel<List<PositionDto>>> GetPosition(int unifiedId)
    {
        var positionDtos = await _enterprisePositionService.GetPosition(unifiedId);

        return MessageModel<List<PositionDto>>.Success(positionDtos);
    }
}