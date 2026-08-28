namespace Api.Extensions;

public static class CorsExtensions
{
    public const string DevelopmentPolicyName = "Development";

    public static IServiceCollection AddDevelopmentCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DevelopmentPolicyName, policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5170",
                        "https://localhost:7013")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseDevelopmentCors(this IApplicationBuilder app)
    {
        if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseCors(DevelopmentPolicyName);
        }

        return app;
    }
}
