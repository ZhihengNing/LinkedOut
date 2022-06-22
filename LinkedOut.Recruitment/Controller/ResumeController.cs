using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Recruitment.Domain;
using LinkedOut.Recruitment.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Controller;

[ApiController]
[Route("user/resume")]
public class ResumeController : ControllerBase
{

    private readonly IResumeService _resumeService;
    
    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpGet("",Name = "获取所有的简历")]
    public async Task<MessageModel<List<ResumeVo>>> QueryResumes([Required] int unifiedId)
    {
        var resumeVos = await _resumeService.GetResumes(unifiedId);
        
        return MessageModel<List<ResumeVo>>.Success(resumeVos);
    }

    [HttpDelete("",Name = "删除指定简历")]
    public async Task<MessageModel<object>> RemoveResume([Required] int resumeId)
    {
        await _resumeService.DeleteResume(resumeId);
        
        return MessageModel.Success();
    }


    [HttpPost("",Name = "上传简历")]
    public async Task<MessageModel<object>> AddResume([Required] [FromForm] int unifiedId,
        [Required] [FromForm]IFormFile file)
    {
        await _resumeService.InsertResume(unifiedId, file);
        
        return MessageModel.Success();
    }
    
    
    
    

}