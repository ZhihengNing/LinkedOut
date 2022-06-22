namespace LinkedOut.Tweet.Domain.Vo;

public class AddTweetVo
{
    public int? UnifiedId { get; set; }
    
    public string? Content { get; set; }

    public List<IFormFile>? Files { get; set; }
}

