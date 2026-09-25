using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwtBearer(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddServer(new OpenApiServer { Url = "/" });

            // Keep only application/json in the OpenAPI doc to avoid tripling size
            // (text/plain + application/json + text/json on every response).
            options.DocumentFilter<JsonOnlyMediaTypesDocumentFilter>();

            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Format = "time",
                Pattern = "^([01]\\d|2[0-3]):[0-5]\\d$",
                Example = "09:00"
            });

            // OpenAPI http scheme must be lowercase "bearer" (not "Bearer").
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
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
        // Enabled in all environments so HTTPS Swagger is reachable behind nginx in prod.
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Relative to /swagger/index.html so reverse-proxy path bases stay correct.
            options.SwaggerEndpoint("v1/swagger.json", "NexusOdonto API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}

/// <summary>
/// Drops non-JSON media types from the OpenAPI document to shrink swagger.json.
/// </summary>
file sealed class JsonOnlyMediaTypesDocumentFilter : IDocumentFilter
{
    private static readonly HashSet<string> Keep =
        new(StringComparer.OrdinalIgnoreCase) { "application/json" };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Paths is null)
        {
            return;
        }

        foreach (var pathItem in swaggerDoc.Paths.Values)
        {
            if (pathItem?.Operations is null)
            {
                continue;
            }

            foreach (var operation in pathItem.Operations.Values)
            {
                TrimContent(operation.RequestBody?.Content);

                if (operation.Responses is null)
                {
                    continue;
                }

                foreach (var response in operation.Responses.Values)
                {
                    TrimContent(response.Content);
                }
            }
        }
    }

    private static void TrimContent(IDictionary<string, OpenApiMediaType>? content)
    {
        if (content is null || content.Count == 0)
        {
            return;
        }

        var remove = content.Keys.Where(k => !Keep.Contains(k)).ToList();
        foreach (var key in remove)
        {
            content.Remove(key);
        }
    }
}
