using Api.Extensions;
using Application.Contracts.Services;
using Serilog;
using Application;
using Infrastructure;
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
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT configuration is missing.");
builder.Services.AddJwtAuthentication(jwtSettings);

builder.Services.AddResilientHttpClients();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

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

    throw new FileNotFoundException(
        "Could not find the .env file from the API root.");
}
