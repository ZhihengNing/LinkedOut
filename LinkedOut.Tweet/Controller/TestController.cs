using LinkedOut.Common.Api;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Helper;
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
        Console.WriteLine("aa"+a);
        var messageModel = await _userFeignClient.Demo(a);
        Console.WriteLine(messageModel.Data+"<-data");
        var success = messageModel.Data+"success";
        return MessageModel<string>.Success(success);
    }

    [HttpGet("")]
    public async Task<int> TestInt()
    {
        var existsObject = OssHelper.ExistsObject("tweet/8/简历.pdf");

        Task.Run(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine(2333);
        });
        Console.WriteLine(existsObject);
        return 3;
    }
    
    
}