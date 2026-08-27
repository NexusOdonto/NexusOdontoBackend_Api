using Application.Contracts.Services;
using Application.Contracts.Services.Audit;
using Application.Contracts.Services.Catalogs;
using Application.Contracts.Services.Chatbot;
using Application.Contracts.Services.Clinical;
using Application.Contracts.Services.Notifications;
using Application.Contracts.Services.Odontogram;
using Application.Contracts.Services.People;
using Application.Contracts.Services.Schedule;
using Application.Contracts.Services.Security;
using Application.Services;
using Application.Services.Audit;
using Application.Services.Catalogs;
using Application.Services.Chatbot;
using Application.Services.Clinical;
using Application.Services.Notifications;
using Application.Services.Odontogram;
using Application.Services.People;
using Application.Services.Schedule;
using Application.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwt = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? new JwtSettings();
        services.AddSingleton(jwt);

        services.AddScoped(typeof(CatalogServiceCore<>));

        // Auth
        services.AddScoped<IAuthService, AuthService>();

        // Catalogs
        services.AddScoped<IDocumentTypeService, DocumentTypeService>();
        services.AddScoped<ISexService, SexService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
        services.AddScoped<IJobTitleService, JobTitleService>();
        services.AddScoped<IAppointmentStatusService, AppointmentStatusService>();
        services.AddScoped<IAppointmentOriginService, AppointmentOriginService>();
        services.AddScoped<ISessionStatusService, SessionStatusService>();
        services.AddScoped<INotificationPriorityService, NotificationPriorityService>();
        services.AddScoped<IHubNotificationStatusService, HubNotificationStatusService>();
        services.AddScoped<IHubNotificationTypeService, HubNotificationTypeService>();
        services.AddScoped<IWhatsAppNotificationStatusService, WhatsAppNotificationStatusService>();
        services.AddScoped<IWhatsAppNotificationTypeService, WhatsAppNotificationTypeService>();
        services.AddScoped<IActionPermissionService, ActionPermissionService>();
        services.AddScoped<IAntecedentTypeService, AntecedentTypeService>();
        services.AddScoped<IAuditEventTypeService, AuditEventTypeService>();
        services.AddScoped<IChatbotConversationStatusService, ChatbotConversationStatusService>();
        services.AddScoped<IChatbotMessageRoleService, ChatbotMessageRoleService>();
        services.AddScoped<IChatChannelService, ChatChannelService>();
        services.AddScoped<IDentalSurfaceService, DentalSurfaceService>();
        services.AddScoped<IDentitionTypeService, DentitionTypeService>();
        services.AddScoped<IFindingStatusService, FindingStatusService>();
        services.AddScoped<IFindingTypeService, FindingTypeService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISupportTicketReasonService, SupportTicketReasonService>();
        services.AddScoped<ISupportTicketStatusService, SupportTicketStatusService>();
        services.AddScoped<IToothStatusService, ToothStatusService>();

        // People
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IProfessionalService, ProfessionalService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();

        // Security
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserRoleService, UserRoleService>();

        // Clinical
        services.AddScoped<IClinicalAttentionService, ClinicalAttentionService>();
        services.AddScoped<IClinicalHistoryService, ClinicalHistoryService>();
        services.AddScoped<IDiagnosisService, DiagnosisService>();
        services.AddScoped<IPatientAntecedentService, PatientAntecedentService>();
        services.AddScoped<IProcedurePerformedService, ProcedurePerformedService>();

        // Schedule
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();

        // Odontogram
        services.AddScoped<IOdontogramService, OdontogramService>();
        services.AddScoped<IOdontogramToothService, OdontogramToothService>();
        services.AddScoped<IDentalFindingService, DentalFindingService>();
        services.AddScoped<IToothService, ToothService>();

        // Notifications
        services.AddScoped<IHubNotificationService, HubNotificationService>();
        services.AddScoped<IWhatsAppNotificationService, WhatsAppNotificationService>();

        // Chatbot
        services.AddScoped<IChatbotConversationService, ChatbotConversationService>();
        services.AddScoped<IChatbotMessageService, ChatbotMessageService>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();

        // Audit
        services.AddScoped<IAuditEventService, AuditEventService>();

        return services;
    }
}
