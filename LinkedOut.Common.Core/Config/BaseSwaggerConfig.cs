using System.Reflection;
using IGeekFan.AspNetCore.Knife4jUI;
using LinkedOut.Common.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LinkedOut.Common.Config;

public static class BaseSwaggerConfig
{
    private static OpenApiInfo ApiInfo(SwaggerProperties swaggerProperties)
    {
        return new OpenApiInfo
        {
            Title = swaggerProperties.Title,
            Version = swaggerProperties.Version,
            Description = swaggerProperties.Description,
            Contact = new OpenApiContact
            {
                Name = swaggerProperties.ContactName,
                Email = swaggerProperties.ContactEmail,
                Url = swaggerProperties.ContactUrl
            }
        };
    }

    private static string? ApiDescription(ApiDescription apiDesc)
    {
        apiDesc.TryGetMethodInfo(out var methodInfo);
        return methodInfo.GetCustomAttribute<HttpMethodAttribute>()?.Name ??
               (apiDesc.ActionDescriptor as ControllerActionDescriptor)?.ActionName;
    }

    public static IServiceCollection AddLinkedOutSwagger(
        this IServiceCollection services,
        SwaggerProperties swaggerProperties)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", ApiInfo(swaggerProperties));

            c.AddServer(new OpenApiServer
            {
                Url = "",
                Description = "vvv"
            });

            c.CustomOperationIds(ApiDescription);
        });
        return services;
    }
    
}
