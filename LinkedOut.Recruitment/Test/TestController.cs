using LinkedOut.Common.Api;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Recruitment.Test;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{
    
    [HttpGet("test")]
    public Task<MessageModel<object>> Print()
    {
        return Task.FromResult(MessageModel.Success());
    }

}