using LinkedOut.Common.Config;
using LinkedOut.Common.Helper;
using LinkedOut.User.Config;
using SummerBoot.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasicModuleService()
    .AddModuleService()
    .AddSingleton(new AppSettingHelper(builder.Configuration));


builder.Services.AddSummerBoot();
builder.Services.AddSummerBootFeign(item =>
{
    item.AddNacos(builder.Configuration);
});

var app = builder.Build();

app.UseBasicModuleMiddleWare("User");

app.Run();