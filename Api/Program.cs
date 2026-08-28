using Api.Extensions;
using Application.Contracts.Services;
using Serilog;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

LoadEnvFile(builder.Environment.ContentRootPath);
builder.Configuration.AddEnvironmentVariables();

// Configure Serilog structured logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtBearer();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSettings = ResolveJwtSettings(builder.Configuration);
builder.Services.AddJwtAuthentication(jwtSettings);

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

app.UseDevelopmentCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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
            "and Jwt__ExpirationMinutes in .env or environment variables.");
    }

    if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
        string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
        string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
    {
        throw new InvalidOperationException(
            "JWT configuration is incomplete. Required: Jwt:Issuer, Jwt:Audience, Jwt:SigningKey " +
            "(or JwtSettings:* equivalents). SigningKey must be at least 32 bytes.");
    }

    return jwtSettings;
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
