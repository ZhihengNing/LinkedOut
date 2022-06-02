using LinkedOut.Common.Exception;

namespace LinkedOut.Common.Helper;

public static class AssertHelper
{
    public static void ValidateNull(object obj)
    {
        if (obj == null)
        {
            throw new ApiException($"{nameof(obj)}不能为空");
        }
    }
}