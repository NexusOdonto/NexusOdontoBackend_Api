using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Oracle.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Persistence.Context;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        LoadEnvFile(Directory.GetCurrentDirectory());

        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__OracleConnection")
            ?? Environment.GetEnvironmentVariable("OracleConnection")
            ?? throw new InvalidOperationException(
                "Oracle connection string not found. Set ConnectionStrings__OracleConnection or OracleConnection in .env.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseOracle(connectionString, oracle =>
            oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));

        return new AppDbContext(optionsBuilder.Options);
    }

    private static void LoadEnvFile(string contentRoot)
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
            "Could not find the .env file from the current directory.");
    }
}
