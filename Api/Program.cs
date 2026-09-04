using Api.Serialization;
using Api.Extensions;
using Api.Hubs;
using Api.Providers;
using Api.Services.Notifications;
using Application;
using Application.Contracts.Services;
using Application.Contracts.Services.Notifications;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.SignalR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoadEnvFile(builder.Environment.ContentRootPath);
builder.Configuration.AddEnvironmentVariables();

// Configure Serilog structured logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Api.Filters.ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new NullableTimeOnlyJsonConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtBearer();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSettings = ResolveJwtSettings(builder.Configuration);
var googleSettings = ResolveGoogleSettings(builder.Configuration);
builder.Services.AddJwtAuthentication(jwtSettings, googleSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRCorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // Configuración flexible para desarrollo/QA
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // REQUERIDO para SignalR WebSockets
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<INotificationPublisherService, NotificationPublisherService>();

builder.Services.AddResilientHttpClients();
builder.Services.AddDevelopmentCors();

var app = builder.Build();

// En contenedor la base arranca vacia: aplicar migraciones y seeders al iniciar
// deja el stack listo con un solo "docker compose up".
if (IsRunningInContainer())
{
    await ApplyMigrationsAsync(app.Services);
}

app.UseGlobalExceptionMiddleware();

app.UseSwaggerWithJwtBearerUi();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseCors("SignalRCorsPolicy");
app.UseDevelopmentCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NexusNotificationHub>("/hubs/notifications");

await DatabaseInitializer.SeedBotUserAsync(app.Services);

app.Run();

static JwtSettings ResolveJwtSettings(IConfiguration configuration)
{
    var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
        ?? configuration.GetSection("JwtSettings").Get<JwtSettings>();

    if (jwtSettings is null)
    {
        throw new InvalidOperationException(
            "JWT configuration is missing. Add a 'Jwt' section to appsettings.json " +
            "or set Jwt__Issuer, Jwt__Audience, Jwt__SigningKey (min 32 bytes), " +
            "Jwt__ExpirationMinutes, and Jwt__RefreshTokenExpirationDays in .env or environment variables.");
    }

    // Binder may leave ExpirationMinutes / RefreshTokenExpirationDays at 0 when the Jwt
    // section is present but those keys are omitted; Normalize applies defaults (15 / 7).
    jwtSettings = JwtSettings.Normalize(jwtSettings);

    if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
        string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
        string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
    {
        throw new InvalidOperationException(
            "JWT configuration is incomplete. Required: Jwt:Issuer, Jwt:Audience, Jwt:SigningKey " +
            "(or JwtSettings:* equivalents). SigningKey must be at least 32 bytes. " +
            "Also set Jwt__ExpirationMinutes and Jwt__RefreshTokenExpirationDays " +
            "(defaults applied when missing/<=0: 15 minutes and 7 days).");
    }

    return jwtSettings;
}

static GoogleSettings ResolveGoogleSettings(IConfiguration configuration)
{
    return configuration.GetSection("Google").Get<GoogleSettings>() ?? new GoogleSettings();
}

static void LoadEnvFile(string contentRoot)
{
    var dir = new DirectoryInfo(contentRoot);

    while (dir is not null)
    {
        var candidate = Path.Combine(dir.FullName, ".env");

        if (File.Exists(candidate))
        {
            Env.Load(candidate);
            return;
        }

        dir = dir.Parent;
    }

    // En contenedor la configuracion llega por variables de entorno del compose,
    // asi que el .env solo es obligatorio fuera de Docker.
    if (!IsRunningInContainer())
    {
        throw new FileNotFoundException(
            "Could not find the .env file from the API root.");
    }
}

static bool IsRunningInContainer() =>
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

static async Task ApplyMigrationsAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Aplicando migraciones pendientes...");
    await context.Database.MigrateAsync();
    logger.LogInformation("Migraciones aplicadas.");
}
