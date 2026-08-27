using Application.DTOs.Catalogs.AntecedentType;
using Application.DTOs.Catalogs.AuditEventType;
using Application.DTOs.Catalogs.ChatbotConversationStatus;
using Application.DTOs.Catalogs.ChatbotMessageRole;
using Application.DTOs.Catalogs.ChatChannel;
using Application.DTOs.Catalogs.DentalSurface;
using Application.DTOs.Catalogs.DentitionType;
using Application.DTOs.Catalogs.FindingStatus;
using Application.DTOs.Catalogs.FindingType;
using Application.DTOs.Catalogs.SupportTicketReason;
using Application.DTOs.Catalogs.SupportTicketStatus;
using Application.DTOs.Catalogs.ToothStatus;
using Domain.Common;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

internal static class EnglishCatalogMapping
{
    public static AntecedentTypeDto ToAntecedentTypeDto(AntecedentType e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static AuditEventTypeDto ToAuditEventTypeDto(AuditEventType e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        Category = e.Category, RiskLevel = e.RiskLevel, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatbotConversationStatusDto ToChatbotConversationStatusDto(ChatbotConversationStatus e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsFinal = e.IsFinal, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatbotMessageRoleDto ToChatbotMessageRoleDto(ChatbotMessageRole e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ChatChannelDto ToChatChannelDto(ChatChannel e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static DentalSurfaceDto ToDentalSurfaceDto(DentalSurface e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        AppliesToAnterior = e.AppliesToAnterior, AppliesToPosterior = e.AppliesToPosterior,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static DentitionTypeDto ToDentitionTypeDto(DentitionType e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static FindingStatusDto ToFindingStatusDto(FindingStatus e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static FindingTypeDto ToFindingTypeDto(FindingType e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        ColorHex = e.ColorHex, VisualSymbol = e.VisualSymbol, RequiresSurface = e.RequiresSurface,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static SupportTicketReasonDto ToSupportTicketReasonDto(SupportTicketReason e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static SupportTicketStatusDto ToSupportTicketStatusDto(SupportTicketStatus e) => new()
    {
        Id = e.Id, Code = e.Code, Name = e.Name, Description = e.Description,
        IsFinal = e.IsFinal, AllowsAssignment = e.AllowsAssignment,
        IsActive = e.IsActive, SortOrder = e.SortOrder
    };

    public static ToothStatusDto ToToothStatusDto(ToothStatus e) => new()
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
