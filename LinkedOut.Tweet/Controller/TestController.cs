using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.User;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Tweet.Controller;

[ApiController]
[Route("demo")]
public class TestController : ControllerBase
{
    private readonly IUserFeignClient _userFeignClient;

    public TestController(IUserFeignClient userFeignClient)
    {
        _userFeignClient = userFeignClient;
    }

    [HttpGet("test")]
    public async Task<MessageModel<string>> TestRpc([FromQuery] string a)
    {
        var messageModel = await _userFeignClient.Demo(a);
        Console.WriteLine(messageModel.Data+"<-data");
        var success = messageModel.Data+"success";
        return MessageModel<string>.Success(success);
    }
}