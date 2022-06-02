using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Tweet.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Tweet.Controller;

[ApiController]
[Route("user")]
public class UserTweetController : ControllerBase
{
    private readonly ITweetService _tweetService;

    public UserTweetController(ITweetService tweetService)
    {
        _tweetService = tweetService;
    }
    
    [HttpGet("")]
    public async Task<MessageModel<DB.Entity.Tweet>> QueryTweet()
    {
        return null;
    }

    [HttpPost("", Name = "发布动态")]
    public async Task<MessageModel<object>> AddTweet([FromForm]AddTweetVo addTweetVo)
    {
        if (addTweetVo.UnifiedId == null)
        {
            throw new ValidateException("用户Id不能为空");
        }
        await _tweetService.AddTweet(addTweetVo);

        return MessageModel.Success();
    }

    [HttpDelete("",Name = "删除动态")]
    public async Task<MessageModel<object>> RemoveTweet([Required]int tweetId)
    {
        await _tweetService.DeleteTweet(tweetId);
        
        return MessageModel.Success();
    }
}