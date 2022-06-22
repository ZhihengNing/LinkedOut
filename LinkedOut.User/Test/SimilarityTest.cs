using LinkedOut.Common.Helper;
using LinkedOut.User.Helper;
using NUnit.Framework;

namespace LinkedOut.User.Test;

public class SimilarityTest
{
    [Test]
    public void Context()
    {
        SimilarityHelper.SimilarScoreCos("宁之恒", "宁予文");
    }
}