using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Controller;

[Route("enterprise")]
[ApiController]
public class EnterprisePositionController : ControllerBase
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

    [HttpDelete("position", Name = "企业删除岗位")]
    public async Task<MessageModel<object>> DeletePosition([Required] int jobId)
    {
        await _enterprisePositionService.DeletePosition(jobId);

        return MessageModel.Success();
    }

    [HttpGet("applicants", Name = "企业获取某岗位所有申请者信息")]
    public async Task<MessageModel<List<ApplicantVo>>> QueryAllApplicants([Required] int jobId)
    {
        var allApplicants = await _enterprisePositionService.GetAllApplicants(jobId);

        return MessageModel<List<ApplicantVo>>.Success(allApplicants);
    }


    [HttpGet("position/all", Name = "用户获取企业所有岗位信息")]
    public async Task<MessageModel<List<PositionVo>>> QueryCompanyAllPosition([Required] int unifiedId, int? momentId)
    {
        var companyAllPosition = await _enterprisePositionService.GetCompanyAllPosition(unifiedId, momentId);

        return MessageModel<List<PositionVo>>.Success(companyAllPosition);
    }

}