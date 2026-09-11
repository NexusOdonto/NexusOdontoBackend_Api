namespace Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "NexusCorsPolicy";

    private static readonly string[] AllowedOrigins =
    [
        "http://localhost:3000",
        "http://localhost:5173",
        "http://localhost:5174",
        "http://localhost:5170",
        "https://localhost:7013",
        "http://nexusodonto.chatcampuslands.com",
        "https://nexusodonto.chatcampuslands.com"
    ];

    public static IServiceCollection AddNexusCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy.WithOrigins(AllowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseNexusCors(this IApplicationBuilder app)
    {
        app.UseCors(PolicyName);
        return app;
    }
}
