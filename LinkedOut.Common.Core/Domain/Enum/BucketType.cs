using LinkedOut.Common.Exception;

namespace LinkedOut.Common.Domain.Enum;


public enum BucketType
{
    Resume,
    Tweet,
    Back,
    Avatar,
}

public static class BucketTypeHelper
{
    private const string Back = "back";

    private const string Avatar = "avatar";

    private const string Resume = "resume";

    private const string Tweet = "tweet";

    public static string GetBucketTypeStr(BucketType type)
    {
        return type switch
        {
            BucketType.Back => Back,
            BucketType.Avatar => Avatar,
            BucketType.Resume => Resume,
            BucketType.Tweet => Tweet,
            _ => throw new ApiException("文件类型错误")
        };
    }
}
