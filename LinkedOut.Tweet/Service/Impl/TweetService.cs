using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Helper;
using LinkedOut.Tweet.Domain.Enum;
using LinkedOut.Tweet.Manager;

namespace LinkedOut.Tweet.Service.Impl;

public class TweetService: ITweetService
{

    private readonly ILogger<TweetService> _logger;

    private readonly LinkedOutContext _context;

    private readonly LikeManager _likeManager;

    private readonly AppFileManager _appFileManager;

    public TweetService(LinkedOutContext context, LikeManager likeManager, ILogger<TweetService> logger, AppFileManager appFileManager)
    {
        _context = context;
        _likeManager = likeManager;
        _logger = logger;
        _appFileManager = appFileManager;
    }

    public async Task<List<TweetVo>> GetSelfTweetList(int unifiedId)
    {
        var tweets = _context.Tweets.Select(o => o)
            .Where(o => o.UnifiedId == unifiedId)
            .Select(o => new TweetVo())
            .ToList();

        return tweets;

    }

    public async Task AddTweet(AddTweetVo addTweetVo)
    {
        var unifiedId = addTweetVo.UnifiedId;
        var tweet = new DB.Entity.Tweet
        {
            Content = addTweetVo.Content,
            UnifiedId =  (int) unifiedId,
            CommentNum = 0,
            LikeNum = 0
        };
        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();
        
        var files = addTweetVo.Files;
        _appFileManager.AddToAppFile(files,BucketType.Tweet,tweet.Id);
        
        _logger.LogInformation("保存完毕");
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


        var urls = _context.AppFiles
            .Where(o => o.AssociatedId == tweetId && o.FileType == (int) AppFileType.Tweet)
            .Select(o => o.Url)
            .ToList();
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