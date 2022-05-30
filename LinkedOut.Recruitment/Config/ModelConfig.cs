using LinkedOut.Common.Config;
using LinkedOut.Common.Domain;
using LinkedOut.DB;

namespace LinkedOut.Recruitment.Config;

public static class ModelConfig
{
    public static IServiceCollection AddModuleService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddLinkedOutSwagger(new SwaggerProperties
        {
            Title = "招聘子系统",
            Description = "recruitment",
            Version = "1.0",
            ContactEmail = "1094554173@qq.com",
            ContactName = "nzh",
            ContactUrl = new Uri("https://www.baidu.com")
        });
        services.AddScoped<LinkedOutContext>();

        return services;
    }
}