using LinkedOut.Common.Domain;
using LinkedOut.Common.Helper;
using NUnit.Framework;

namespace LinkedOut.Common.Test;

public class TokenTest
{
    [Test]
    public void TestToken()
    {
        var s=FileHelper
            .ReadJsonFile("../LinkedOut.Common.Core/config.json",false)
            .ToObj<TokenProperties>("token");

        Console.WriteLine(s);
    }
}