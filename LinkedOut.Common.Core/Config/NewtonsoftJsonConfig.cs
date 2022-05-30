using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LinkedOut.Common.Config;

public static class NewtonsoftJsonConfig
{
    public static void AddNewtonsoftJsonConfig(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            
            options.SerializerSettings.Converters.Add(
                new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"}
            );
            
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    }
}