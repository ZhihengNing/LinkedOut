using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkedOut.Common.Helper;

public static class JsonHelper
{
    public static JObject? ConvertToJObject(object o)
    {
        var serializeObject = JsonConvert.SerializeObject(o);
        return JsonConvert.DeserializeObject<JObject>(serializeObject);
    }

    public static JObject ReadConfigJson(string path)
    {
        var streamReader = new StreamReader(path);
        var jsonTextReader = new JsonTextReader(streamReader);
        var jsonObject = (JObject) JToken.ReadFrom(jsonTextReader);
        return jsonObject;
    }
}