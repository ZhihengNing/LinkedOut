using LinkedOut.Common.Api;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.Recruitment;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Entity;
using LinkedOut.DB.Helper;
using LinkedOut.Tweet.Constant;
using LinkedOut.Tweet.Domain.Enum;
using LinkedOut.Tweet.Domain.Vo;
using LinkedOut.Tweet.Manager;
using Microsoft.EntityFrameworkCore;

namespace LinkedOut.Tweet.Service.Impl;

public class TweetService : ITweetService
{
    private readonly ILogger<TweetService> _logger;

    private readonly LinkedOutContext _context;

    private readonly LikeManager _likeManager;

    private readonly AppFileManager _appFileManager;

    private readonly IUserFeignClient _userFeignClient;

    private readonly IRecruitFeignClient _recruitFeignClient;

    public TweetService(LinkedOutContext context, LikeManager likeManager, ILogger<TweetService> logger,
        AppFileManager appFileManager, IUserFeignClient userFeignClient, IRecruitFeignClient recruitFeignClient)
    {
        _context = context;
        _likeManager = likeManager;
        _logger = logger;
        _appFileManager = appFileManager;
        _userFeignClient = userFeignClient;
        _recruitFeignClient = recruitFeignClient;
    }
    
    public async Task<List<UserTweetVo>> GetOnesTweetList(int visitorId, int intervieweeId, int? momentId)
    {
        //先获取我们要访问的人的基本信息
        var userInfo = await _userFeignClient.GetUserInfo(intervieweeId);
        if (!userInfo.Code.Equals(ResultCode.Success().Code))
        {
            throw new ApiException($"找不到id为{intervieweeId}人的动态");
        }

        var userInfoData = userInfo.Data;

        Func<DB.Entity.Tweet, bool> predicate = momentId != null
            ? o => o.UnifiedId == intervieweeId && o.Id < momentId
            : o => o.UnifiedId == intervieweeId;
        
        var likeds = _context.Likeds.ToList();

        var appFiles = _context.AppFiles.Where(o => o.FileType == (int) AppFileType.Tweet)
            .ToList();
        
        return _context.Tweets
            .Where(predicate)
            .ToList()
            .Select(o =>
            {
                var pictureList = appFiles
                    .Where(a => a.AssociatedId == o.Id)
                    .Select(a => a.Url)
                    .ToList();
                
                return new UserTweetVo
                {
                    TweetId = o.Id,
                    SimpleUserInfo = userInfoData!,
                    CommentNum = o.CommentNum,
                    Contents = o.Content,
                    LikeState = likeds.Any(liked => liked.TweetId == o.Id)?1:0,
                    PictureList = pictureList,
                    PraiseNum = o.LikeNum,
                    RecordTime = o.CreateTime
                };
            })
            .OrderByDescending(o => o.TweetId)
            .Take(ITweetNum.Num)
            .ToList();
    }

    private async Task<List<TweetVo>> GetUsersTweet(List<UserDto> data)
    {
        var list = data.Join(_context.Tweets,
            u => u.UnifiedId,
            t => t.UnifiedId,
            (u, t) => new
            {
                TweetId = t.Id,
                SimpleUserInfo = u,
                t.CommentNum,
                Contents = t.Content,
                PraiseNum = t.LikeNum,
                RecordTime = t.CreateTime
            }).ToList();

        var likeds = _context.Likeds.ToList();

        var appFiles = _context.AppFiles.Where(o => o.FileType == (int) AppFileType.Tweet)
            .ToList();

        var userTweet = list.Select(t =>
        {
            var tweetId = t.TweetId;

            var pictureList = appFiles
                .Where(a => a.AssociatedId == tweetId)
                .Select(o => o.Url)
                .ToList();
            
            return (TweetVo) new UserTweetVo
            {
                Type = "tweet",
                TweetId = t.TweetId,
                SimpleUserInfo = t.SimpleUserInfo,
                CommentNum = t.CommentNum,
                Contents = t.Contents,
                LikeState = likeds.Any(liked => liked.TweetId == tweetId)?1:0,
                PictureList = pictureList,
                PraiseNum = t.PraiseNum,
                RecordTime = t.RecordTime
            };
        }).ToList();

        return userTweet;
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

        foreach (var variable in data)
        {
            Console.WriteLine(variable.UnifiedId + variable.UserType);
        }

        _logger.LogInformation("成功走到了这里");

        // //包括关注所有用户的动态
        // var userTweet = data.SelectMany(o =>
        // {
        //     var id = o.UnifiedId;
        //     return _context.Tweets
        //         .Where(t => t.UnifiedId == id)
        //         .ToList()
        //         .Select(t => (TweetVo) new UserTweetVo
        //         {
        //             Type = "tweet",
        //             TweetId = t.Id,
        //             SimpleUserInfo = o,
        //             CommentNum = t.CommentNum,
        //             Contents = t.Content,
        // LikeState = (int) _likeManager.GetRelation(unifiedId, t.Id).Item1,
        //             // PictureList = _appFileManager.GetTweetPictures(t.Id).Select(appFile => appFile.Url).ToList(),
        //             PraiseNum = t.LikeNum,
        //             RecordTime = t.CreateTime
        //         });
        // }).ToList();

        var userTweet = await GetUsersTweet(data);

        _logger.LogInformation("查完了用户动态");

        //包括关注所有公司的招聘启事
        var enterpriseTweet = data
            .Where(o => "company".Equals(o.UserType))
            .ToList()
            .SelectMany(o =>
            {
                var positions =
                    _recruitFeignClient.GetPosition(o.UnifiedId).Result;

                if (!positions.Code.Equals(ResultCode.Success().Code))
                {
                    throw new ApiException("远程调用失败");
                }

                return positions.Data!
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

        _logger.LogInformation("查完了公司招聘信息");
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
            return allTweetVos.Take(ITweetNum.Num).ToList();
        }

        //我们只需要已经排好序后再筛选的九条
        return allTweetVos
            .Where(o => (o.Type.Equals(type) && o.TweetId < momentId) || !o.Type.Equals(type))
            .Take(ITweetNum.Num)
            .ToList();
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
        var (likeState, _) = _likeManager.GetRelation(unifiedId, tweetId);
        if (likeState == LikeState.Liked)
        {
            throw new ApiException("不能重复点赞噢");
        }
        await _context.Likeds.AddAsync(new Liked
        {
            TweetId = tweetId,
            UnifiedId = unifiedId
        });

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