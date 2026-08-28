using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Odontogram;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services;
using Application.Contracts.Services.Chatbot;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Context;
using Infrastructure.Repositories.Catalogs;
using Infrastructure.Repositories.Chatbot;
using Infrastructure.Repositories.Clinical;
using Infrastructure.Repositories.Common;
using Infrastructure.Repositories.Odontogram;
using Infrastructure.Repositories.People;
using Infrastructure.Repositories.Schedule;
using Infrastructure.Repositories.Security;
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

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(ICatalogRepository<>), typeof(CatalogRepository<>));

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<IClinicalHistoryRepository, ClinicalHistoryRepository>();
        services.AddScoped<IClinicalAttentionRepository, ClinicalAttentionRepository>();

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();

        services.AddScoped<IChatbotConversationRepository, ChatbotConversationRepository>();
        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();

        services.AddScoped<IToothRepository, ToothRepository>();

        services.AddHttpClient<IChatbotClient, ChatbotHttpClient>();

        return services;
    }
}

