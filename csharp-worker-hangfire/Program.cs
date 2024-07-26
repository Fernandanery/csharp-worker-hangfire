using csharp_worker_hangfire.Configuration;
using csharp_worker_hangfire.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("RedisConnection");
    var redis = ConnectionMultiplexer.Connect(connectionString);

    options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = $"HANG_FIRE" });
    options.UseConsole();
});
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<MonitorService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = HangFireDashBoard.AuthAuthorizationFilters()
});

app.Run();
