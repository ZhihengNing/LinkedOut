namespace LinkedOut.Tweet.Service;

public interface ITweetService
{
    Task<List<TweetVo>> GetSelfTweetList(int unifiedId);
    
    Task AddTweet(AddTweetVo addTweetVo);
    
    Task DeleteTweet(int tweetId);

    Task LikeTweet(int unifiedId, int tweetId);

    Task UnLikeTweet(int unifiedId, int tweetId);
}