using LinkedOut.Common.Exception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkedOut.Common.Helper;

public static class FileHelper
{
    //webapi项目就是不需要这个
    private const string BasicPath = "../../../";

    public static async Task<string> ReadFile(string path, bool isWebApi = true)
    {
        var finalPath = isWebApi ? path : BasicPath + path;
        try
        {
            return await File.ReadAllTextAsync(finalPath);
        }
        catch (System.Exception e)
        {
            throw new ApiException($"不存在路径为{finalPath}的文件");
        }
    }

    public static JObject ReadJsonFile(string path, bool isWebApi = true)
    {
        var finalPath = isWebApi ? path : BasicPath + path;
        try
        {
            var streamReader = new StreamReader(finalPath);
            var jsonTextReader = new JsonTextReader(streamReader);
            var json = (JObject) JToken.ReadFrom(jsonTextReader);
            return json;
        }
        catch (System.Exception e)
        {
            throw new ApiException($"不存在路径为{finalPath}的文件");
        }
    }

}