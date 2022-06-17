using LinkedOut.Common.Config;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Filter;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.Gateway.Config;

public static class ModelConfig
{
    public static IServiceCollection AddModuleService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddLinkedOutSwagger(new SwaggerProperties
        {
            Title = "网关子系统",
            Description = "gateway",
            Version = "1.0",
            ContactEmail = "1094554173@qq.com",
            ContactName = "nzh",
            ContactUrl = new Uri("https://www.baidu.com")
        });
        
        return services;
    }
}