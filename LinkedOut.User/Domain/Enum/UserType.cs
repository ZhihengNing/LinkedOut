using LinkedOut.Common.Exception;

namespace LinkedOut.User.Domain.Enum;

public enum UserType
{
    User,
    School,
    Company,
}

public static class UserTypeHelper
{
    private const string User = "user";

    private const string School = "school";

    private const string Company = "company";

    public static string GetUserType(UserType userType)
    {
        return userType switch
        {
            UserType.User => User,
            UserType.School => School,
            UserType.Company => Company,
            _ => throw new ApiException("不存在其他类型的用户")
        };
    }
}