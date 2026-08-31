using Infrastructure.Data.Seeder.ActionPermission;
using Infrastructure.Data.Seeder.AdminUser;
using Infrastructure.Data.Seeder.AntecedentType;
using Infrastructure.Data.Seeder.AppointmentOrigin;
using Infrastructure.Data.Seeder.AppointmentStatus;
using Infrastructure.Data.Seeder.AuditEventType;
using Infrastructure.Data.Seeder.ChatbotConversationStatus;
using Infrastructure.Data.Seeder.ChatbotMessageRole;
using Infrastructure.Data.Seeder.ChatChannel;
using Infrastructure.Data.Seeder.DentalSurface;
using Infrastructure.Data.Seeder.DentitionType;
using Infrastructure.Data.Seeder.DocumentType;
using Infrastructure.Data.Seeder.FindingStatus;
using Infrastructure.Data.Seeder.FindingType;
using Infrastructure.Data.Seeder.HubNotificationStatus;
using Infrastructure.Data.Seeder.HubNotificationType;
using Infrastructure.Data.Seeder.JobTitle;
using Infrastructure.Data.Seeder.NotificationPriority;
using Infrastructure.Data.Seeder.Permission;
using Infrastructure.Data.Seeder.Role;
using Infrastructure.Data.Seeder.RolePermission;
using Infrastructure.Data.Seeder.Service;
using Infrastructure.Data.Seeder.SessionStatus;
using Infrastructure.Data.Seeder.Sex;
using Infrastructure.Data.Seeder.Specialty;
using Infrastructure.Data.Seeder.SupportTicketReason;
using Infrastructure.Data.Seeder.SupportTicketStatus;
using Infrastructure.Data.Seeder.Tooth;
using Infrastructure.Data.Seeder.ToothStatus;
using Infrastructure.Data.Seeder.User;
using Infrastructure.Data.Seeder.WhatsAppNotificationStatus;
using Infrastructure.Data.Seeder.WhatsAppNotificationType;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder;

public static class DatabaseSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // 1. Base catalogs (no FK dependencies)
        ActionPermissionSeeder.Seed(modelBuilder);
        DocumentTypeSeeder.Seed(modelBuilder);
        SexSeeder.Seed(modelBuilder);
        DentitionTypeSeeder.Seed(modelBuilder);
        AntecedentTypeSeeder.Seed(modelBuilder);
        AuditEventTypeSeeder.Seed(modelBuilder);
        DentalSurfaceSeeder.Seed(modelBuilder);
        FindingStatusSeeder.Seed(modelBuilder);
        FindingTypeSeeder.Seed(modelBuilder);
        JobTitleSeeder.Seed(modelBuilder);
        SessionStatusSeeder.Seed(modelBuilder);
        SpecialtySeeder.Seed(modelBuilder);
        ToothStatusSeeder.Seed(modelBuilder);
        AppointmentOriginSeeder.Seed(modelBuilder);
        AppointmentStatusSeeder.Seed(modelBuilder);
        ChatChannelSeeder.Seed(modelBuilder);
        ChatbotConversationStatusSeeder.Seed(modelBuilder);
        ChatbotMessageRoleSeeder.Seed(modelBuilder);
        HubNotificationStatusSeeder.Seed(modelBuilder);
        HubNotificationTypeSeeder.Seed(modelBuilder);
        NotificationPrioritySeeder.Seed(modelBuilder);
        ServiceSeeder.Seed(modelBuilder);
        SupportTicketReasonSeeder.Seed(modelBuilder);
        SupportTicketStatusSeeder.Seed(modelBuilder);
        WhatsAppNotificationStatusSeeder.Seed(modelBuilder);
        WhatsAppNotificationTypeSeeder.Seed(modelBuilder);

        // 2. Permissions (depends on ActionPermission)
        PermissionSeeder.Seed(modelBuilder);

        // 3. Roles
        RoleSeeder.Seed(modelBuilder);

        // 4. Role-permission assignments (admin gets all permissions, bot gets minimum)
        RolePermissionSeeder.Seed(modelBuilder);

        // 5. Development admin user (person + user + user_role)
        AdminUserSeeder.Seed(modelBuilder);

        // 6. Teeth (depends on DentitionType)
        ToothSeeder.Seed(modelBuilder);
    }
}
