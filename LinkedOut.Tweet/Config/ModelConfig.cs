using LinkedOut.Common.Config;
using LinkedOut.Common.Domain;
using LinkedOut.DB;
using LinkedOut.DB.Helper;
using LinkedOut.Tweet.Manager;
using LinkedOut.Tweet.Service;
using LinkedOut.Tweet.Service.Impl;

namespace LinkedOut.Tweet.Config;

public static class ModelConfig
{
    public static IServiceCollection AddModuleService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddLinkedOutSwagger(new SwaggerProperties
        {
            Title = "动态子系统",
            Description = "tweet",
            Version = "1.0",
            ContactEmail = "1094554173@qq.com",
            ContactName = "nzh",
            ContactUrl = new Uri("https://www.baidu.com")
        });

        services.AddScoped<LinkedOutContext>();
        services.AddScoped<AppFileManager>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITweetService, TweetService>();

        services.AddScoped<LikeManager>();

        // services.AddScoped<IUserRpcClient>();
        return services;
    }
}