using LinkedOut.Common.Config;
using LinkedOut.Common.Helper;
using LinkedOut.Gateway.Config;
using LinkedOut.Gateway.MiddleWare;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using Ocelot.Provider.Nacos.NacosClient.V2;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddBasicModuleService()
    .AddModuleService()
    .AddNacosAspNet(builder.Configuration,"nacos")
    .AddSingleton(new AppSettingHelper(builder.Configuration));

builder.Services.AddOcelot(
    new ConfigurationBuilder()
        .AddJsonFile("ocelot.json", true, true)
        .Build()).AddNacosDiscovery();

var app = builder.Build();

app.UseMiddleware<JwtMiddleware>();
app.UseBasicModuleMiddleWare("Gateway");
app.UseOcelot().Wait();
app.Run();