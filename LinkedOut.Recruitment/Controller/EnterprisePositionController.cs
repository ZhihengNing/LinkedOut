using LinkedOut.Common.Api;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Controller;

[Route("enterprise")]
[ApiController]
public class EnterprisePositionController: ControllerBase
{

    private readonly IEnterprisePositionService _enterprisePositionService;

    public EnterprisePositionController(IEnterprisePositionService enterprisePositionService)
    {
        _enterprisePositionService = enterprisePositionService;
    }

    [HttpPost("position", Name = "企业添加岗位")]
    public async Task<MessageModel<object>> AddPosition([FromBody] Position position)
    {
        await _enterprisePositionService.InsertPosition(position);

        return MessageModel.Success();
    }
}