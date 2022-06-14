using LinkedOut.Tweet.Domain.Vo;

namespace LinkedOut.Tweet.Service;

public interface ITweetService
{
    Task<List<UserTweetVo>> GetSelfTweetList(int visitorId,int intervieweeId,int momentId);
    
    Task<List<TweetVo>> GetSubscribeTweets(int unifiedId, int ?momentId, string? type);
    
    Task AddTweet(AddTweetVo addTweetVo);
    
    Task DeleteTweet(int tweetId);

    Task LikeTweet(int unifiedId, int tweetId);

    Task UnLikeTweet(int unifiedId, int tweetId);
}