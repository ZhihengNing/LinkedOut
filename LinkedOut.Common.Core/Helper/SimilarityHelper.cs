namespace LinkedOut.Common.Helper;

public static class SimilarityHelper
{
    public static double SimilarScoreCos(string? text1, string? text2)
    {
        if (text1 == null || text2 == null)
        {
            //只要有一个文本为null，规定相似度分值为0，表示完全不相等
            return 0.0;
        }

        if ("".Equals(text1) && "".Equals(text2))
        {
            return 1.0;
        }

        var asii = new HashSet<int>();
        var text1Map = new Dictionary<int, int>();
        var text2Map = new Dictionary<int, int>();


        foreach (int temp in text1)
        {
            if (!text1Map.ContainsValue(temp))
            {
                text1Map.TryAdd(temp, 1);
            }
            else
            {
                text1Map.TryAdd(temp, text1Map[temp] + 1);
            }

            asii.Add(temp);
        }

        foreach (int temp in text2)
        {
            if (!text2Map.ContainsValue(temp))
            {
                text2Map.TryAdd(temp, 1);
            }
            else
            {
                text2Map.TryAdd(temp, text2Map[temp] + 1);
            }

            asii.Add(temp);
        }

        var xy = 0.0;
        var x = 0.0;
        var y = 0.0;
        //计算

        foreach (var it in asii)
        {

            var t1 = text1Map.ContainsValue(it) ? text1Map[it] : 0;
            var t2 = text1Map.ContainsValue(it) ? text2Map[it] : 0;
            xy += t1 * t2;
            x += Math.Pow(t1, 2);
            y += Math.Pow(t2, 2);
        }

        if (x == 0.0 || y == 0.0)
        {
            return 0.0;
        }
        
        return xy / Math.Sqrt(x * y);
    }
}