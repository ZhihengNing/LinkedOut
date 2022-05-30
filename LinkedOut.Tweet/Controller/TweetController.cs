using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.DB.Entity;
using LinkedOut.Tweet.Domain.Vo;
using LinkedOut.Tweet.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Tweet.Controller;

[ApiController]
[Route("")]
public class TweetController : ControllerBase
{
    private readonly ICommentService _commentService;

    private readonly ITweetService _tweetService;

    public TweetController(ICommentService commentService, ITweetService tweetService)
    {
        _commentService = commentService;
        _tweetService = tweetService;
    }

    [HttpGet("")]
    public async Task<MessageModel<DB.Entity.Tweet>> QueryTweet()
    {
        return null;
    }


    [HttpPost("")]
    public async Task<MessageModel<object>> AddTweet([FromForm] AddTweetVo addTweetVo)
    {
        await _tweetService.AddTweet(addTweetVo);

        return MessageModel.Success();
    }


    [NoTransaction]
    [HttpGet("comment",Name = "获取动态下的评论")]
    public async Task<MessageModel<List<CommentVo>>> QueryComment([FromQuery] int tweetId)
    {
        var commentVos = await _commentService.GetComment(tweetId);
        
        return MessageModel<List<CommentVo>>.Success(commentVos);
    }

    
    [HttpPost("comment",Name = "添加评论")]
    public async Task<MessageModel<object>> AddComment([FromBody] Comment comment)
    {
        await _commentService.AddComment(comment);
        
        return MessageModel.Success();
    }

    [HttpDelete("comment",Name="删除评论")]
    public async Task<MessageModel<object>> RemoveComment([FromBody] int commentId)
    {
        await _commentService.DeleteComment(commentId);
        
        return MessageModel.Success();
    }


}