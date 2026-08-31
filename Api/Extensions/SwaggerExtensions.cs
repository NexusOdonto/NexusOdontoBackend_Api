using Microsoft.OpenApi;

namespace Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwtBearer(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddServer(new OpenApiServer { Url = "/" });

            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Format = "time",
                Pattern = "^([01]\\d|2[0-3]):[0-5]\\d$",
                Example = "09:00"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. " +
                    "Obtain a token from POST /api/auth/login, then enter: Bearer {your token}"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerWithJwtBearerUi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "NexusOdonto API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
