using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.Tweet.Domain.Enum;

namespace LinkedOut.Tweet.Service.Impl;

public class TweetService: ITweetService
{

    private readonly LinkedOutContext _context;

    private readonly LikeManager _likeManager;

    public TweetService(LinkedOutContext context, LikeManager likeManager)
    {
        _context = context;
        _likeManager = likeManager;
    }

    public Task<List<TweetVo>> GetSelfTweetList(int unifiedId)
    {
        var tweets = _context.Tweets.Select(o=>o)
            .Where(o=>o.UnifiedId==unifiedId).ToList();
        throw new NotImplementedException();
    }

    public async Task AddTweet(AddTweetVo addTweetVo)
    {
        var unifiedId = addTweetVo.UnifiedId;
        var tweet = new DB.Entity.Tweet
        {
            Content = addTweetVo.Content,
            UnifiedId = (int)unifiedId,
            CommentNum = 0,
            LikeNum = 0
        };
        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();
        var tweetId = tweet.Id;
        var files = addTweetVo.Files;
        
        if (files != null && files.Count != 0)
        {
            var results = files.AsParallel()
                .Select(item => OssHelper.UploadFile(item, BucketType.Tweet, tweetId));
            var join = string.Join(",", results);
            tweet.PictureUrl = join;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTweet(int tweetId)
    {
        var tweet = _context.Tweets.SingleOrDefault(o => o.Id == tweetId);
        if (tweet == null)
        {
            throw new ApiException($"不存在id为{tweetId}的动态");
        }
        _context.Tweets.Remove(tweet);
        await _context.SaveChangesAsync();

        var tweetPictureUrl = tweet.PictureUrl;
        if (tweetPictureUrl == null) return;
        //todo 这个地方是什么情况
        var strings = tweetPictureUrl.Split(",");
        strings.AsParallel().ForAll(OssHelper.DeleteObject);
    }

    public async Task LikeTweet(int unifiedId, int tweetId)
    {
        var (likeState, liked) = _likeManager.GetRelation(unifiedId, tweetId);
        if (likeState == LikeState.Liked)
        {
            throw new ApiException("不能重复点赞噢");
        }
        
        await _context.Likeds.AddAsync(liked!);
        await _context.SaveChangesAsync();
    }

    public async Task UnLikeTweet(int unifiedId, int tweetId)
    {
        var (likeState, liked) = _likeManager.GetRelation(unifiedId, tweetId);
        if (likeState == LikeState.Unliked)
        {
            throw new ApiException("不能重复取消点赞噢");
        }

        _context.Likeds.Remove(liked!);

        await _context.SaveChangesAsync();
    }
}