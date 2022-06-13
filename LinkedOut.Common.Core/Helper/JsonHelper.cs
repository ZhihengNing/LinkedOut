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

    public static T ToObj<T>(this JObject jObject, string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        return jObject.Value<JObject>(key)!.ToObject<T>()!;
    }


}