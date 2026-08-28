using Application.DTOs.Catalogos;
using Domain.Common;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

internal static class SpanishCatalogMapping
{
    public static TipoDocumentoDto ToTipoDocumentoDto(DocumentTypeEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static SexoDto ToSexoDto(SexEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static EspecialidadDto ToEspecialidadDto(SpecialtyEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static CargoDto ToCargoDto(JobTitleEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static EstadoCitaDto ToEstadoCitaDto(AppointmentStatusEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        ColorHex = e.ColorHex, Orden = e.SortOrder, EsFinal = e.IsFinal, Activo = e.IsActive
    };

    public static OrigenCitaDto ToOrigenCitaDto(AppointmentOriginEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoSesionDto ToEstadoSesionDto(SessionStatusEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static PrioridadNotificacionDto ToPrioridadNotificacionDto(NotificationPriorityEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        ColorHex = e.ColorHex, Nivel = e.Level, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoNotificacionHubDto ToEstadoNotificacionHubDto(HubNotificationStatusEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static TipoNotificacionHubDto ToTipoNotificacionHubDto(HubNotificationTypeEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Icono = e.Icon, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoNotificacionWhatsappDto ToEstadoNotificacionWhatsappDto(WhatsAppNotificationStatusEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static TipoNotificacionWhatsappDto ToTipoNotificacionWhatsappDto(WhatsAppNotificationTypeEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        PlantillaPredeterminada = e.DefaultTemplate, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static AccionPermisoDto ToAccionPermisoDto(ActionPermissionEntity e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static void ApplyActive(CatalogEntity entity, bool activo)
    {
        entity.IsActive = activo;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
