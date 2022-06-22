using LinkedOut.Common.Config;
using LinkedOut.Common.Domain;
using LinkedOut.DB;
using LinkedOut.DB.Helper;
using LinkedOut.Recruitment.Service;
using LinkedOut.Recruitment.Service.Impl;

namespace LinkedOut.Recruitment.Config;

public static class ModuleConfig
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
        services.AddScoped<AppFileManager>();

        services.AddScoped<IEnterprisePositionService, EnterprisePositionService>();
        services.AddScoped<IUserRecruitmentService, UserRecruitmentService>();
        services.AddScoped<IResumeService, ResumeService>();
        
        return services;
    }
}