namespace LinkedOut.Tweet.Service;

public interface ITweetService
{
    Task<List<TweetVo>> GetTweetList();
    
    Task AddTweet(AddTweetVo addTweetVo);
    
    Task DeleteTweet(int tweetId);
}