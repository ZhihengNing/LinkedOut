using LinkedOut.Common.Api;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Helper;
using LinkedOut.Tweet.Domain.Enum;
using LinkedOut.Tweet.Domain.Vo;
using LinkedOut.Tweet.Manager;

namespace LinkedOut.Tweet.Service.Impl;

public class TweetService : ITweetService
{
    private readonly ILogger<TweetService> _logger;

    private readonly LinkedOutContext _context;

    private readonly LikeManager _likeManager;

    private readonly AppFileManager _appFileManager;

    private readonly IUserFeignClient _userFeignClient;

    public TweetService(LinkedOutContext context, LikeManager likeManager, ILogger<TweetService> logger,
        AppFileManager appFileManager, IUserFeignClient userFeignClient)
    {
        _context = context;
        _likeManager = likeManager;
        _logger = logger;
        _appFileManager = appFileManager;
        _userFeignClient = userFeignClient;
    }

    public async Task<List<UserTweetVo>> GetSelfTweetList(int visitorId, int intervieweeId, int momentId)
    {
        //先获取我们要访问的人的基本信息
        var userInfo = await _userFeignClient.GetUserInfo(intervieweeId);
        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException($"找不到id为{intervieweeId}人的动态");
        }

        var userInfoData = userInfo.Data;

        //momentId不传的话默认是0,刚好满足业务需求
        return _context.Tweets
            .Where(o => o.UnifiedId == intervieweeId && o.Id < momentId)
            .ToList()
            .Select(o => new UserTweetVo
                {
                    TweetId = o.Id,
                    SimpleUserInfo = userInfoData!,
                    CommentNum = o.CommentNum,
                    Content = o.Content,
                    LikeState = (int) _likeManager.GetRelation(visitorId, o.Id).Item1,
                    PictureList = _appFileManager
                        .GetTweetPictures(o.Id)
                        .Select(o=>o.Url).ToList(),
                    PraiseNum = o.LikeNum,
                    RecordTime = o.CreateTime
                }
            )
            .OrderByDescending(o => o.TweetId)
            .Take(9)
            .ToList();
    }

    private async Task<List<TweetVo>> GetAllTweetVos(int unifiedId)
    {
        //获取所有关注的人的基本信息(包括自己)
        var subscribeUsers = await _userFeignClient.GetSubscribeUserId(unifiedId);
        if (!subscribeUsers.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException($"不存在id为{unifiedId}的用户");
        }
        
        var data = subscribeUsers.Data!;

        _logger.LogInformation("成功走到了这里");

        //包括关注所有用户的动态
        var userTweet = data.SelectMany(o =>
        {
            var id = o.UnifiedId;
            return _context.Tweets
                .Where(t => t.UnifiedId == id)
                .ToList()
                .Select(t => (TweetVo) new UserTweetVo
                {
                    Type = "tweet",
                    TweetId = t.Id,
                    SimpleUserInfo = o,
                    CommentNum = t.CommentNum,
                    Content = t.Content,
                    LikeState = (int) _likeManager.GetRelation(unifiedId, t.Id).Item1,
                    PictureList = _appFileManager.GetTweetPictures(t.Id).Select(appFile=>appFile.Url).ToList(),
                    PraiseNum = t.LikeNum,
                    RecordTime = t.CreateTime
                });
        }).ToList();

        //包括关注所有公司的招聘启事
        var enterpriseTweet = data.Where(o => "company".Equals(o.UserType))
            .SelectMany(o =>
            {
                return _context.Positions
                    .Where(p => p.EnterpriseId == o.UnifiedId)
                    .ToList()
                    .Select(t => (TweetVo) new EnterpriseTweetVo
                    {
                        Type = "position",
                        TweetId = t.Id,
                        JobName = t.JobName,
                        PositionType = t.PositionType,
                        RecordTime = t.CreateTime,
                        Reward = t.Reward,
                        SimpleUserInfo = o
                    });
            })
            .ToList();

        //需要把两个数组，按照时间顺序排好

        return userTweet.Union(enterpriseTweet)
            .OrderByDescending(o => o.RecordTime)
            .ToList();
    }

    public async Task<List<TweetVo>> GetSubscribeTweets(int unifiedId, int? momentId, string? type)
    {
        if (momentId != null)
        {
            if (type == null)
            {
                throw new ApiException("momentId不为空时,type不能为空");
            }

            if (!"position".Equals(type) && !"tweet".Equals(type))
            {
                throw new ApiException("不符合要求的动态类型传参");
            }
        }
        
        //得到所有我关注人的动态，包括用户动态，公司动态，自己动态，公司招聘启示
        var allTweetVos = await GetAllTweetVos(unifiedId);
        
        //如果没传momentId
        if (momentId == null)
        {
            return allTweetVos.Take(9).ToList();
        }

        //我们只需要已经排好序后再筛选的九条
        return allTweetVos
            .Where(o => (o.Type.Equals(type) && o.TweetId < momentId) || !o.Type.Equals(type))
            .Take(9).ToList();
    }

    public async Task AddTweet(AddTweetVo addTweetVo)
    {
        var unifiedId = addTweetVo.UnifiedId;
        var tweet = new DB.Entity.Tweet
        {
            Content = addTweetVo.Content,
            UnifiedId = (int) unifiedId,
            CommentNum = 0,
            LikeNum = 0
        };
        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();

        var files = addTweetVo.Files;
        _appFileManager.AddToAppFile(files, BucketType.Tweet, tweet.Id);

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

        _appFileManager.DeleteAppFile(tweetId, AppFileType.Tweet);
        await _context.SaveChangesAsync();
    }

    public async Task LikeTweet(int unifiedId, int tweetId)
    {
        var (likeState, liked) = _likeManager.GetRelation(unifiedId, tweetId);
        if (likeState == LikeState.Liked)
        {
            throw new ApiException("不能重复点赞噢");
        }
        await _context.Likeds.AddAsync(liked!);

        var tweet = _context.Tweets.SingleOrDefault(o=>o.Id==tweetId);
        if (tweet == null)
        {
            throw new ApiException($"不存在Id为{tweetId}的动态");
        }
        tweet.LikeNum += 1;
        
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

        var tweet = _context.Tweets.SingleOrDefault(o=>o.Id==tweetId);
        if (tweet == null)
        {
            throw new ApiException($"不存在Id为{tweetId}的动态");
        }
        tweet.LikeNum -= 1;
        
        await _context.SaveChangesAsync();
    }
}