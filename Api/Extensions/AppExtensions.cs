using Api.Middleware;
using Microsoft.Extensions.Http.Resilience;

namespace Api.Extensions;

public static class AppExtensions
{
    public static IServiceCollection AddResilientHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("WhatsAppWebhook")
            .AddStandardResilienceHandler();

        services.AddHttpClient("PythonAgent")
            .AddStandardResilienceHandler();

        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
