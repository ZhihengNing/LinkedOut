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


}