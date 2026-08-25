using Application.Contracts.Repositories;
using Infrastructure.Persistence.Context;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OracleConnection")
            ?? throw new InvalidOperationException("Connection string 'OracleConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}