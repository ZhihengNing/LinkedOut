
using LinkedOut.DB.Entity;
using LinkedOut.Tweet.Domain.Vo;

namespace LinkedOut.Tweet.Service;

public interface ICommentService
{
    Task<List<CommentVo>> GetComment(int tweetId);
    
    Task AddComment(Comment comment);

    Task DeleteComment(int commentId);
}