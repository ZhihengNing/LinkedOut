using Microsoft.Extensions.Configuration;

namespace LinkedOut.Common.Helper;

public class AppSettingHelper
{
    private static IConfiguration _configuration = null!;

    public AppSettingHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    /// <summary>
    /// 封装要操作的字符
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static string? App(params string[] sections)
    {
        try
        {
            if (sections.Any())
            {
                return _configuration[string.Join(":", sections)];
            }
        }
        catch (System.Exception)
        {
            // ignored
        }

        return null;
    }


    public static List<T> App<T>(params string[] sections)
    {
        var list = new List<T>();
        _configuration.Bind(string.Join(":", sections), list);
        return list;
    }


    /// <summary>
    /// 根据路径  configuration["App:Name"];
    /// </summary>
    /// <param name="sectionsPath"></param>
    /// <returns></returns>
    public static string GetValue(string sectionsPath)
    {
        try
        {
            return _configuration[sectionsPath];
        }
        catch (System.Exception e)
        {
            // ignored
        }

        return "";
    }
}
