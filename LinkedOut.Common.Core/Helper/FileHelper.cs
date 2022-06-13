using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkedOut.Common.Helper;

public static class FileHelper
{
    //webapi项目就是不需要这个
    private const string BasicPath = "../../../";

    public static async Task<string> ReadFile(string path, bool isWebApi = true)
    {
        return await File.ReadAllTextAsync(isWebApi ? path : BasicPath + path);
    }

    public static JObject ReadJsonFile(string path, bool isWebApi = true)
    {
        var streamReader = new StreamReader(isWebApi ? path : BasicPath + path);
        var jsonTextReader = new JsonTextReader(streamReader);
        var json = (JObject) JToken.ReadFrom(jsonTextReader);
        return json;
    }

}