using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Tweet.Domain.Vo;
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

    [HttpGet("one", Name = "获取某个人的动态")]
    public async Task<MessageModel<List<UserTweetVo>>> QueryOnesTweet([Required] int visitorId,
        [Required] int intervieweeId, int? momentId)
    {
        var selfTweetList = await _tweetService.GetOnesTweetList(visitorId, intervieweeId, momentId);
        return MessageModel<List<UserTweetVo>>.Success(selfTweetList);
    }

    
    [HttpGet("subscribe", Name = "获取我关注人的动态")]
    public async Task<MessageModel<List<TweetVo>>> QuerySubscribeTweet([Required] int unifiedId,
        int? momentId, string? type)
    {
        var subscribeTweets = await _tweetService.GetSubscribeTweets(unifiedId, momentId, type);

        Console.WriteLine(subscribeTweets.Count);
        subscribeTweets.ForEach(item=>Console.Write(item.Type));
        return MessageModel<List<TweetVo>>.Success(subscribeTweets);
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