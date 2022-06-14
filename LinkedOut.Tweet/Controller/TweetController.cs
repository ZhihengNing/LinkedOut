using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Attribute;
using LinkedOut.Common.Exception;
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

    [NoTransaction]
    [HttpGet("comment",Name = "获取动态下的评论")]
    public async Task<MessageModel<List<CommentVo>>> QueryComment([Required]int tweetId)
    {
        var commentVos = await _commentService.GetComment(tweetId);
        
        return MessageModel<List<CommentVo>>.Success(commentVos);
    }
    
    
    [HttpPost("comment", Name = "添加评论")]
    public async Task<MessageModel<object>> AddComment([FromBody]AddCommentVo comment)
    {
        if (comment.UnifiedId == null)
        {
            throw new ValidateException("用户Id不能为空");
        }

        if (string.IsNullOrEmpty(comment.Content))
        {
            throw new ValidateException("评论内容不能为空");
        }

        if (comment.TweetId == null)
        {
            throw new ValidateException("动态Id不能为空");
        }
        await _commentService.AddComment(comment);

        return MessageModel.Success();
    }

    [HttpDelete("comment", Name = "删除评论")]
    public async Task<MessageModel<object>> RemoveComment([Required]int commentId)
    {

        await _commentService.DeleteComment(commentId);

        return MessageModel.Success();
    }

    [HttpPost("likes", Name = "点赞动态")]
    public async Task<MessageModel<object>> LikeTweet([Required]int unifiedId,[Required]int tweetId)
    {
        Console.WriteLine(unifiedId + "   " + tweetId);
        Console.WriteLine("================================");
        await _tweetService.LikeTweet(unifiedId, tweetId);

        return MessageModel.Success();
    }

    [HttpDelete("likes", Name = "取消点赞动态")]
    public async Task<MessageModel<object>> UnLikeTweet([Required]int unifiedId, [Required]int tweetId)
    {
        Console.WriteLine(unifiedId + "   " + tweetId);
        await _tweetService.UnLikeTweet(unifiedId, tweetId);
    
        return MessageModel.Success();
    }



}