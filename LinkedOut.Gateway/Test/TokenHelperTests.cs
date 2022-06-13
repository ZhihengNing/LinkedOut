using LinkedOut.Common.Domain;
using LinkedOut.Common.Helper;
using LinkedOut.Gateway.Helper;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace LinkedOut.Gateway.Test;

public class TokenHelperTests
{

    [Test]
    public void ReadJsonFile()
    {
        var jsonFile = FileHelper.ReadJsonFile("../LinkedOut.Common.Core/config.json",false);

        Assert.NotNull(jsonFile.ToObj<TokenProperties>("token"));
    }

    [Test]
    public void VerifyToken()
    {
        VerifyHelper.VerifyToken("cscsdcsdcdss");
    }

    [Test]
    public void WhiteList()
    {
        var WhiteList =
            AppSettingHelper.App<string>("whiteList");
        Assert.NotNull(WhiteList);
        var white = WhiteList[0];
        Console.WriteLine(white);
    }
}