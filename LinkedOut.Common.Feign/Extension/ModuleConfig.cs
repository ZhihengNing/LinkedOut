using LinkedOut.Common.Feign.Exception;
using Microsoft.Extensions.DependencyInjection;

namespace LinkedOut.Common.Feign.Extension;

public static class ModuleConfig
{
    public static IServiceCollection AddFeignModuleService(this IServiceCollection services)
    {
        services.AddMvc((opt) =>
        {
            opt.Filters.Add<FeignExceptionFilter>();
        });

        return services;
    }
}