using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DocumentType;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class DocumentTypesController(IDocumentTypeService service)
    : CatalogCrudControllerBase<IDocumentTypeService, DocumentTypeDto>(service);
