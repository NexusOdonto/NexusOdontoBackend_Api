using Domain.Common;
using Infrastructure.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>();

        configurationBuilder
            .Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>();

        configurationBuilder
            .Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter>()
            .HaveMaxLength(5);

        configurationBuilder
            .Properties<TimeOnly?>()
            .HaveConversion<NullableTimeOnlyConverter>()
            .HaveMaxLength(5);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        DatabaseSeeder.Seed(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
                    .MakeGenericMethod(entityType.ClrType);
                method?.Invoke(null, [modelBuilder]);
            }
        }
    }

    private static void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.IsActive);
    }
}

internal sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d))
    {
    }
}

internal sealed class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    public NullableDateOnlyConverter() : base(
        d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null,
        d => d.HasValue ? DateOnly.FromDateTime(d.Value) : null)
    {
    }
}

internal sealed class TimeOnlyConverter : ValueConverter<TimeOnly, string>
{
    public TimeOnlyConverter() : base(
        time => time.ToString("HH:mm"),
        value => TimeOnly.ParseExact(value, "HH:mm"))
    {
    }
}

internal sealed class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, string?>
{
    public NullableTimeOnlyConverter() : base(
        time => time.HasValue ? time.Value.ToString("HH:mm") : null,
        value => value == null ? null : TimeOnly.ParseExact(value, "HH:mm"))
    {
    }
}