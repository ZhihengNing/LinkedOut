using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.Tweet.Domain.Enum;

namespace LinkedOut.Tweet.Manager;

public class LikeManager
{
    private readonly LinkedOutContext _context;

    public LikeManager(LinkedOutContext context)
    {
        _context = context;
    }

    public (LikeState, Liked?) GetRelation(int unifiedId, int tweetId)
    {
        var liked = _context.Likeds
            .SingleOrDefault(o => o.UnifiedId == unifiedId && o.TweetId == tweetId);
        return liked == null ? (LikeState.Unliked, null) : (LikeState.Liked, liked);

    }
}