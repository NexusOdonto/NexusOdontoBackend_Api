using Application.DTOs.Catalogs.ActionPermission;
using Application.DTOs.Catalogs.AntecedentType;
using Application.DTOs.Catalogs.AppointmentOrigin;
using Application.DTOs.Catalogs.AppointmentStatus;
using Application.DTOs.Catalogs.AuditEventType;
using Application.DTOs.Catalogs.ChatbotConversationStatus;
using Application.DTOs.Catalogs.ChatbotMessageRole;
using Application.DTOs.Catalogs.ChatChannel;
using Application.DTOs.Catalogs.DentalSurface;
using Application.DTOs.Catalogs.DentitionType;
using Application.DTOs.Catalogs.DocumentType;
using Application.DTOs.Catalogs.FindingStatus;
using Application.DTOs.Catalogs.FindingType;
using Application.DTOs.Catalogs.HubNotificationStatus;
using Application.DTOs.Catalogs.HubNotificationType;
using Application.DTOs.Catalogs.JobTitle;
using Application.DTOs.Catalogs.NotificationPriority;
using Application.DTOs.Catalogs.SessionStatus;
using Application.DTOs.Catalogs.Sex;
using Application.DTOs.Catalogs.Specialty;
using Application.DTOs.Catalogs.SupportTicketReason;
using Application.DTOs.Catalogs.SupportTicketStatus;
using Application.DTOs.Catalogs.ToothStatus;
using Application.DTOs.Catalogs.WhatsAppNotificationStatus;
using Application.DTOs.Catalogs.WhatsAppNotificationType;
using Domain.Common;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

internal static class CatalogMapping
{
    public static DocumentTypeDto ToDocumentTypeDto(DocumentTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder, CreatedAt = e.CreatedAt
    };

    public static SexDto ToSexDto(SexEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder, CreatedAt = e.CreatedAt
    };

    public static SpecialtyDto ToSpecialtyDto(SpecialtyEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static JobTitleDto ToJobTitleDto(JobTitleEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder, CreatedAt = e.CreatedAt
    };

    public static AppointmentStatusDto ToAppointmentStatusDto(AppointmentStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, SortOrder = e.SortOrder, IsFinal = e.IsFinal, IsActive = e.IsActive
    };

    public static AppointmentOriginDto ToAppointmentOriginDto(AppointmentOriginEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static SessionStatusDto ToSessionStatusDto(SessionStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static NotificationPriorityDto ToNotificationPriorityDto(NotificationPriorityEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, Level = e.Level, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static HubNotificationStatusDto ToHubNotificationStatusDto(HubNotificationStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static HubNotificationTypeDto ToHubNotificationTypeDto(HubNotificationTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        Icon = e.Icon, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static WhatsAppNotificationStatusDto ToWhatsAppNotificationStatusDto(WhatsAppNotificationStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static WhatsAppNotificationTypeDto ToWhatsAppNotificationTypeDto(WhatsAppNotificationTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        DefaultTemplate = e.DefaultTemplate, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ActionPermissionDto ToActionPermissionDto(ActionPermissionEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static AntecedentTypeDto ToAntecedentTypeDto(AntecedentTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static AuditEventTypeDto ToAuditEventTypeDto(AuditEventTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        Category = e.Category, RiskLevel = e.RiskLevel, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatbotConversationStatusDto ToChatbotConversationStatusDto(ChatbotConversationStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsFinal = e.IsFinal, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatbotMessageRoleDto ToChatbotMessageRoleDto(ChatbotMessageRoleEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatChannelDto ToChatChannelDto(ChatChannelEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static DentalSurfaceDto ToDentalSurfaceDto(DentalSurfaceEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        AppliesToAnterior = e.AppliesToAnterior, AppliesToPosterior = e.AppliesToPosterior,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static DentitionTypeDto ToDentitionTypeDto(DentitionTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static FindingStatusDto ToFindingStatusDto(FindingStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static FindingTypeDto ToFindingTypeDto(FindingTypeEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, VisualSymbol = e.VisualSymbol, RequiresSurface = e.RequiresSurface,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static SupportTicketReasonDto ToSupportTicketReasonDto(SupportTicketReasonEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static SupportTicketStatusDto ToSupportTicketStatusDto(SupportTicketStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsFinal = e.IsFinal, AllowsAssignment = e.AllowsAssignment,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ToothStatusDto ToToothStatusDto(ToothStatusEntity e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, VisualCode = e.VisualCode, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static void ApplyActive(CatalogEntity entity, bool isActive)
    {
        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
