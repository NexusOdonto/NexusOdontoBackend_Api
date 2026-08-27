using Application.DTOs.Catalogos;
using Domain.Common;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

internal static class SpanishCatalogMapping
{
    public static TipoDocumentoDto ToTipoDocumentoDto(DocumentType e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static SexoDto ToSexoDto(Sex e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static EspecialidadDto ToEspecialidadDto(Specialty e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static CargoDto ToCargoDto(JobTitle e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder, FechaCreacion = e.CreatedAt
    };

    public static EstadoCitaDto ToEstadoCitaDto(AppointmentStatus e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        ColorHex = e.ColorHex, Orden = e.SortOrder, EsFinal = e.IsFinal, Activo = e.IsActive
    };

    public static OrigenCitaDto ToOrigenCitaDto(AppointmentOrigin e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoSesionDto ToEstadoSesionDto(SessionStatus e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static PrioridadNotificacionDto ToPrioridadNotificacionDto(NotificationPriority e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        ColorHex = e.ColorHex, Nivel = e.Level, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoNotificacionHubDto ToEstadoNotificacionHubDto(HubNotificationStatus e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static TipoNotificacionHubDto ToTipoNotificacionHubDto(HubNotificationType e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Icono = e.Icon, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static EstadoNotificacionWhatsappDto ToEstadoNotificacionWhatsappDto(WhatsAppNotificationStatus e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        Activo = e.IsActive, Orden = e.SortOrder
    };

    public static TipoNotificacionWhatsappDto ToTipoNotificacionWhatsappDto(WhatsAppNotificationType e) => new()
    {
        Id = e.Id, Codigo = e.Code, Nombre = e.Name, Descripcion = e.Description,
        PlantillaPredeterminada = e.DefaultTemplate, Activo = e.IsActive, Orden = e.SortOrder
    };

    public static AccionPermisoDto ToAccionPermisoDto(ActionPermission e) => new()
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
