using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkedOut.Common.Helper;

public static class FileHelper
{
    //webapi项目就是不需要这个
    private const string BasicPath = "../../../";

    public static async Task<string> ReadFile(string path,bool isWebApi=true)
    {
        if (isWebApi)
        {
            return await File.ReadAllTextAsync(path);
        }

        return await File.ReadAllTextAsync(BasicPath + path);
    }
    
    public static JObject ReadJsonFile(string path)
    {
        var streamReader = new StreamReader(path);
        var jsonTextReader = new JsonTextReader(streamReader);
        var jsonObject = (JObject) JToken.ReadFrom(jsonTextReader);
        return jsonObject;
    }
    
}