using IGeekFan.AspNetCore.Knife4jUI;
using LinkedOut.Common.Filter;
using LinkedOut.Common.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LinkedOut.Common.Config;

public static class BaseModelConfig 
{
    public static IServiceCollection AddBasicModuleService(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            // options.Filters.Add<GlobalExceptionFilter>();
            options.Filters.Add<ApiExceptionFilter>();
            // options.Filters.Add<TransactionFilter>();
        });

        services.Configure<FormOptions>(x =>
        {
            x.MultipartBodyLengthLimit = int.MaxValue;
            x.ValueLengthLimit = int.MaxValue;
        });
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });
        services.AddCors(options => options
            .AddPolicy("cors", 
                p => p.SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            )
        );
        services.AddEndpointsApiExplorer();
        services.AddNewtonsoftJsonConfig();
        services.AddCors();
        services.AddHostedService<ConsumerService>();
        return services;
    }

    public static IApplicationBuilder UseBasicModuleMiddleWare(this WebApplication app, string serviceName)
    {
        app.UseCors("cors");
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseKnife4UI(c =>
        {
            c.RoutePrefix = "";
            c.SwaggerEndpoint("/v1/api-docs", serviceName);
        });
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger("{documentName}/api-docs");
        });
        app.UseHttpsRedirection();
        return app;
    }
}