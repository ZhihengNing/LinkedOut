using Microsoft.AspNetCore.Mvc.ModelBinding;

public class AddTweetVo
{
    public int? UnifiedId { get; set; }
    
    public string? Content { get; set; }

    public List<IFormFile>? Files { get; set; }
}


public class TweetVo
{
    
}