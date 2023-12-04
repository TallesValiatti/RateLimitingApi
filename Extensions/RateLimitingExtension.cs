using System.Net;
using System.Threading.RateLimiting;

namespace RateLimitingApi.Extensions;

public static class RateLimitingExtension 
{
    public static IServiceCollection AddFixedRate(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 0,
                        Window = TimeSpan.FromSeconds(10)
                    }));

            options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
        });
            
        return services;
    }
}