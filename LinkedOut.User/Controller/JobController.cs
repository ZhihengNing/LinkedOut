 using System.ComponentModel.DataAnnotations;
 using LinkedOut.Common.Api;
 using LinkedOut.Common.Exception;
 using LinkedOut.DB.Entity;
 using LinkedOut.User.Domain.Vo;
 using LinkedOut.User.Service;
 using Microsoft.AspNetCore.Mvc;

 namespace LinkedOut.User.Controller; 

 [Route("job")]
 [ApiController]
 public class JobController : ControllerBase
 {

     private readonly IJobService _jobService;

     public JobController(IJobService jobService)
     {
         _jobService = jobService;
     }

     [HttpGet("",Name = "查询工作经历")]
     public async Task<MessageModel<List<JobVo>>> QueryJobExperience([Required]int unifiedId)
     {
         var jobExperience = await _jobService.GetJobExperience(unifiedId);
         
         return MessageModel<List<JobVo>>.Success(jobExperience);
     }

     [HttpPost("",Name = "添加工作经历")]
     public async Task<MessageModel<object>> AddJobExperience([FromBody] JobExperienceVo jobExperienceVo)
     {
         if (jobExperienceVo.UnifiedId == null)
         {
             throw new ValidateException("用户Id不能为空");
         }
         await _jobService.InsertJobExperience(jobExperienceVo);
         
         return MessageModel.Success();
     }

     [HttpDelete("",Name="删除工作经历")]
     public async Task<MessageModel<object>> DeleteJobExperience([Required]int jobExperienceId)
     {
         await _jobService.DeleteJobExperience(jobExperienceId);

         return MessageModel.Success();
     }

 }