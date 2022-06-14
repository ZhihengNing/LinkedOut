using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.Tweet.Domain.Vo;

namespace LinkedOut.Tweet.Service.Impl;

public class CommentService : ICommentService
{
    private readonly LinkedOutContext _context;

    private readonly IUserFeignClient _userFeignClient;

    public CommentService(LinkedOutContext context, IUserFeignClient userFeignClient)
    {
        _context = context;
        _userFeignClient = userFeignClient;
    }

    public async Task<List<CommentVo>> GetComment(int tweetId)
    {
        var comments = _context.Comments
            .Select(o => o)
            .Where(o => o.TweetId == tweetId);

        if (comments == null)
        {
            throw new ApiException($"不存在动态Id为{tweetId}的评论");
        }

        var commentVos = comments.AsParallel()
            .Select(o =>
            {
                var data = _userFeignClient.GetUserInfo(o.UnifiedId).Result;
                if (data.Code == ResultCode.Success().Code)
                {
                    return new CommentVo
                    {
                        Content = o.Content,
                        CommentId = o.Id,
                        CreateTime = o.CreateTime,
                        UserDto = data.Data!
                    };
                }

                throw new ApiException("找不到对应的User信息");
            }).ToList();
        return commentVos;
    }

    public async Task AddComment(AddCommentVo commentVo)
    {
        var comment = new Comment()
        {
            UnifiedId = (int)commentVo.UnifiedId,
            Content = commentVo.Content,
            TweetId = commentVo.TweetId
        };
        await _context.Comments.AddAsync(comment);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(int commentId)
    {
        var comment = _context.Comments
            .SingleOrDefault(o => o.Id == commentId);

        if (comment == null)
        {
            throw new ApiException($"不存在id为{commentId}的评论");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}