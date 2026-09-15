using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.FullOdontogram;
using Application.DTOs.Odontogram.Odontogram;
using Application.Exceptions;
using Domain.Entities.Catalogs;
using Domain.Entities.Clinical;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class OdontogramService(
    IRepository<OdontogramEntity> repository,
    IClinicalHistoryRepository clinicalHistoryRepository,
    IRepository<OdontogramToothEntity> odontogramToothRepository,
    IRepository<DentalFindingEntity> dentalFindingRepository,
    IRepository<ToothStatusEntity> toothStatusRepository,
    IRepository<FindingTypeEntity> findingTypeRepository,
    IRepository<DentalSurfaceEntity> dentalSurfaceRepository,
    IRepository<FindingStatusEntity> findingStatusRepository,
    IUnitOfWork unitOfWork) : IOdontogramService
{
    private static readonly Dictionary<string, string> SurfaceShortToCode = new(StringComparer.OrdinalIgnoreCase)
    {
        ["M"] = "MESIAL",
        ["D"] = "DISTAL",
        ["O"] = "OCLUSAL",
        ["I"] = "INCISAL",
        ["V"] = "VESTIBULAR",
        ["B"] = "VESTIBULAR",
        ["P"] = "PALATINO",
        ["L"] = "LINGUAL",
    };

    private static readonly Dictionary<string, string> SurfaceCodeToShort = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MESIAL"] = "M",
        ["DISTAL"] = "D",
        ["OCLUSAL"] = "O",
        ["INCISAL"] = "I",
        ["VESTIBULAR"] = "V",
        ["PALATINO"] = "P",
        ["LINGUAL"] = "L",
    };

    private static readonly Dictionary<string, string> FindingTypeAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["CARIES"] = "CARIES",
        ["CAR"] = "CARIES",
        ["RESTORATION"] = "RESTAURACION",
        ["RESTAURACION"] = "RESTAURACION",
        ["RES"] = "RESTAURACION",
        ["OBT"] = "RESTAURACION",
        ["FILLING"] = "RESTAURACION",
        ["FRACTURE"] = "FRACTURA",
        ["FRACTURA"] = "FRACTURA",
        ["FRA"] = "FRACTURA",
        ["CROWN"] = "PROTESIS",
        ["CORONA"] = "PROTESIS",
        ["PROTESIS"] = "PROTESIS",
        ["SEALANT"] = "SELLANTE",
        ["SELLANTE"] = "SELLANTE",
        ["SEL"] = "SELLANTE",
        ["SEA"] = "SELLANTE",
        ["ENDODONTIC"] = "RESTAURACION",
        ["END"] = "RESTAURACION",
        ["IMPLANT"] = "PROTESIS",
        ["IMP"] = "PROTESIS",
        ["EXTRACTION"] = "FRACTURA",
        ["EXT"] = "FRACTURA",
    };

    private static readonly Dictionary<string, string> ToothStateAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SANO"] = "SANO",
        ["HEALTHY"] = "SANO",
        ["AUSENTE"] = "AUSENTE",
        ["MISSING"] = "AUSENTE",
        ["EXTRAIDO"] = "EXTRAIDO",
        ["EXTRACTED"] = "EXTRAIDO",
        ["INCLUIDO"] = "SANO",
        ["UNERUPTED"] = "SANO",
        ["IMPLANTE"] = "IMPLANTE",
        ["IMPLANT"] = "IMPLANTE",
        ["CORONA"] = "CORONA",
        ["CROWN"] = "CORONA",
    };

    public async Task<IReadOnlyList<OdontogramResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<OdontogramResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<OdontogramResponseDto> CreateAsync(CreateOdontogramRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new OdontogramEntity(
                request.ClinicalHistoryId,
                request.ProfessionalId,
                request.ClinicalAttentionId,
                request.Notes);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<OdontogramResponseDto?> UpdateAsync(Guid id, UpdateOdontogramRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.Notes, entity.ClinicalAttentionId);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    public async Task<FullOdontogramResponseDto> SaveFullOdontogramAsync(
        SaveFullOdontogramRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.ProfessionalId.HasValue || request.ProfessionalId.Value == Guid.Empty)
        {
            throw new ValidationException("Se requiere ProfessionalId para registrar el odontograma.");
        }

        Guid clinicalHistoryId;
        if (request.ClinicalHistoryId.HasValue && request.ClinicalHistoryId.Value != Guid.Empty)
        {
            clinicalHistoryId = request.ClinicalHistoryId.Value;
        }
        else if (request.PatientId.HasValue && request.PatientId.Value != Guid.Empty)
        {
            var history = await clinicalHistoryRepository.GetByPatientIdAsync(request.PatientId.Value, cancellationToken);
            if (history is null)
            {
                history = new ClinicalHistoryEntity(
                    request.PatientId.Value,
                    DateTime.UtcNow,
                    "Historia clínica inicial generada automáticamente para odontograma.");
                await clinicalHistoryRepository.AddAsync(history);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            clinicalHistoryId = history.Id;
        }
        else
        {
            throw new ValidationException("Debe indicar PatientId o ClinicalHistoryId para registrar el odontograma.");
        }

        var toothStatuses = await toothStatusRepository.GetAllAsync();
        var statusByCode = toothStatuses.ToDictionary(s => s.Code, StringComparer.OrdinalIgnoreCase);
        var defaultToothStatus = statusByCode.GetValueOrDefault("SANO")
            ?? toothStatuses.FirstOrDefault()
            ?? throw new ValidationException("No hay estados de pieza dental configurados en el catálogo.");

        var findingTypes = await findingTypeRepository.GetAllAsync();
        var typeByCode = findingTypes.ToDictionary(t => t.Code, StringComparer.OrdinalIgnoreCase);

        var surfaces = await dentalSurfaceRepository.GetAllAsync();
        var surfaceByCode = surfaces.ToDictionary(s => s.Code, StringComparer.OrdinalIgnoreCase);

        var findingStatuses = await findingStatusRepository.GetAllAsync();
        var defaultFindingStatus = findingStatuses.FirstOrDefault(s =>
                string.Equals(s.Code, "ACTIVO", StringComparison.OrdinalIgnoreCase))
            ?? findingStatuses.FirstOrDefault()
            ?? throw new ValidationException("No hay estados de hallazgo configurados en el catálogo.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            OdontogramEntity? odontogram = null;
            if (request.ClinicalAttentionId.HasValue && request.ClinicalAttentionId.Value != Guid.Empty)
            {
                var allOdontos = await repository.GetAllAsync();
                odontogram = allOdontos.FirstOrDefault(o => o.ClinicalAttentionId == request.ClinicalAttentionId.Value);
            }

            if (odontogram is null)
            {
                odontogram = new OdontogramEntity(
                    clinicalHistoryId,
                    request.ProfessionalId.Value,
                    request.ClinicalAttentionId,
                    request.Notes);
                await repository.AddAsync(odontogram);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                odontogram.Update(request.Notes, request.ClinicalAttentionId);
                repository.Update(odontogram);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var allTeeth = await odontogramToothRepository.GetAllAsync();
            var existingTeeth = allTeeth.Where(t => t.OdontogramId == odontogram.Id).ToList();
            var toothEntityByCode = new Dictionary<string, OdontogramToothEntity>(StringComparer.OrdinalIgnoreCase);

            foreach (var toothDto in request.Teeth)
            {
                var toothCode = ResolveToothCode(toothDto);
                if (string.IsNullOrWhiteSpace(toothCode)) continue;

                var statusId = ResolveToothStatusId(toothDto, statusByCode, defaultToothStatus.Id);
                var tooth = existingTeeth.FirstOrDefault(t =>
                    string.Equals(t.ToothCode, toothCode, StringComparison.OrdinalIgnoreCase));

                if (tooth is null)
                {
                    tooth = new OdontogramToothEntity(odontogram.Id, toothCode, statusId, toothDto.Note);
                    await odontogramToothRepository.AddAsync(tooth);
                }
                else
                {
                    tooth.Update(statusId, toothDto.Note);
                    odontogramToothRepository.Update(tooth);
                }

                toothEntityByCode[toothCode] = tooth;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var allFindings = await dentalFindingRepository.GetAllAsync();
            var existingFindings = allFindings
                .Where(f => toothEntityByCode.Values.Any(t => t.Id == f.OdontogramToothId)
                            || existingTeeth.Any(t => t.Id == f.OdontogramToothId))
                .ToList();

            foreach (var finding in existingFindings)
            {
                dentalFindingRepository.Remove(finding);
            }

            if (existingFindings.Count > 0)
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            foreach (var toothDto in request.Teeth)
            {
                var toothCode = ResolveToothCode(toothDto);
                if (string.IsNullOrWhiteSpace(toothCode)) continue;
                if (!toothEntityByCode.TryGetValue(toothCode, out var toothEntity)) continue;

                foreach (var findingDto in toothDto.Findings ?? [])
                {
                    var findingTypeId = ResolveFindingTypeId(findingDto, typeByCode);
                    if (findingTypeId is null) continue;

                    var surfaceId = ResolveSurfaceId(findingDto, surfaceByCode);
                    var findingStatusId = findingDto.FindingStatusId is { } fs && fs != Guid.Empty
                        ? fs
                        : defaultFindingStatus.Id;

                    var note = findingDto.Description ?? findingDto.Note;
                    var finding = new DentalFindingEntity(
                        toothEntity.Id,
                        findingTypeId.Value,
                        findingStatusId,
                        surfaceId,
                        note);
                    await dentalFindingRepository.AddAsync(finding);
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return await BuildFullOdontogramResponseAsync(odontogram, cancellationToken);
        });
    }

    public async Task<FullOdontogramResponseDto?> GetLatestByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        var history = await clinicalHistoryRepository.GetByPatientIdAsync(patientId, cancellationToken);
        if (history is null) return null;

        var allOdontos = await repository.GetAllAsync();
        var latest = allOdontos
            .Where(o => o.ClinicalHistoryId == history.Id)
            .OrderByDescending(o => o.RecordedAt)
            .FirstOrDefault();

        if (latest is null) return null;

        return await BuildFullOdontogramResponseAsync(latest, cancellationToken);
    }

    public async Task<FullOdontogramResponseDto?> GetFullByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await BuildFullOdontogramResponseAsync(entity, cancellationToken);
    }

    private async Task<FullOdontogramResponseDto> BuildFullOdontogramResponseAsync(
        OdontogramEntity odontogram,
        CancellationToken cancellationToken)
    {
        _ = cancellationToken;

        var allTeeth = await odontogramToothRepository.GetAllAsync();
        var teeth = allTeeth.Where(t => t.OdontogramId == odontogram.Id).ToList();
        var toothIds = teeth.Select(t => t.Id).ToHashSet();

        var allFindings = await dentalFindingRepository.GetAllAsync();
        var findings = allFindings.Where(f => toothIds.Contains(f.OdontogramToothId)).ToList();

        var toothStatuses = await toothStatusRepository.GetAllAsync();
        var statusById = toothStatuses.ToDictionary(s => s.Id);

        var findingTypes = await findingTypeRepository.GetAllAsync();
        var typeById = findingTypes.ToDictionary(ft => ft.Id);

        var surfaces = await dentalSurfaceRepository.GetAllAsync();
        var surfaceById = surfaces.ToDictionary(s => s.Id);

        var findingStatuses = await findingStatusRepository.GetAllAsync();
        var findingStatusById = findingStatuses.ToDictionary(fs => fs.Id);

        var teethResponse = new List<FullOdontogramToothResponseDto>();

        foreach (var tooth in teeth)
        {
            statusById.TryGetValue(tooth.ToothStatusId, out var ts);
            _ = int.TryParse(tooth.ToothCode, out var fdiNumber);

            var toothFindings = findings
                .Where(f => f.OdontogramToothId == tooth.Id)
                .Select(f =>
                {
                    typeById.TryGetValue(f.FindingTypeId, out var ft);
                    surfaceById.TryGetValue(f.DentalSurfaceId ?? Guid.Empty, out var ds);
                    findingStatusById.TryGetValue(f.FindingStatusId, out var fs);

                    string? shortSurface = null;
                    if (ds is not null)
                    {
                        shortSurface = SurfaceCodeToShort.GetValueOrDefault(ds.Code) ?? ds.Code;
                    }

                    return new FullDentalFindingResponseDto
                    {
                        Id = f.Id,
                        FindingTypeCode = ft?.Code,
                        SurfaceCode = shortSurface,
                        Description = f.Note,
                        FindingTypeId = f.FindingTypeId,
                        DentalSurfaceId = f.DentalSurfaceId,
                        FindingTypeName = ft?.Name,
                        FindingTypeColorHex = ft?.ColorHex,
                        FindingStatusId = f.FindingStatusId,
                        FindingStatusCode = fs?.Code,
                        RegisteredAt = f.RegisteredAt
                    };
                })
                .ToList();

            teethResponse.Add(new FullOdontogramToothResponseDto
            {
                Id = tooth.Id,
                FdiNumber = fdiNumber,
                ToothCode = tooth.ToothCode,
                StateCode = ts?.Code ?? "SANO",
                ToothStatusId = tooth.ToothStatusId,
                ToothStatusName = ts?.Name,
                Note = tooth.Note,
                Findings = toothFindings
            });
        }

        return new FullOdontogramResponseDto
        {
            Id = odontogram.Id,
            ClinicalHistoryId = odontogram.ClinicalHistoryId,
            ClinicalAttentionId = odontogram.ClinicalAttentionId,
            ProfessionalId = odontogram.ProfessionalId,
            RecordedAt = odontogram.RecordedAt,
            Notes = odontogram.Notes,
            Teeth = teethResponse
        };
    }

    private static string? ResolveToothCode(OdontogramToothItemDto toothDto)
    {
        if (toothDto.FdiNumber is int fdi && fdi > 0)
            return fdi.ToString();

        if (!string.IsNullOrWhiteSpace(toothDto.ToothCode))
            return toothDto.ToothCode.Trim();

        return null;
    }

    private static Guid ResolveToothStatusId(
        OdontogramToothItemDto toothDto,
        IReadOnlyDictionary<string, ToothStatusEntity> statusByCode,
        Guid defaultStatusId)
    {
        if (toothDto.ToothStatusId is { } id && id != Guid.Empty)
            return id;

        if (!string.IsNullOrWhiteSpace(toothDto.StateCode))
        {
            var raw = toothDto.StateCode.Trim();
            if (ToothStateAliases.TryGetValue(raw, out var aliased)
                && statusByCode.TryGetValue(aliased, out var byAlias))
            {
                return byAlias.Id;
            }

            if (statusByCode.TryGetValue(raw, out var byCode))
                return byCode.Id;
        }

        return defaultStatusId;
    }

    private static Guid? ResolveFindingTypeId(
        DentalFindingItemDto findingDto,
        IReadOnlyDictionary<string, FindingTypeEntity> typeByCode)
    {
        if (findingDto.FindingTypeId is { } id && id != Guid.Empty)
            return id;

        if (string.IsNullOrWhiteSpace(findingDto.FindingTypeCode))
            return null;

        var raw = findingDto.FindingTypeCode.Trim();

        if (FindingTypeAliases.TryGetValue(raw, out var aliased)
            && typeByCode.TryGetValue(aliased, out var byAlias))
        {
            return byAlias.Id;
        }

        if (typeByCode.TryGetValue(raw, out var byCode))
            return byCode.Id;

        foreach (var (alias, catalogCode) in FindingTypeAliases)
        {
            if (raw.Contains(alias, StringComparison.OrdinalIgnoreCase)
                && typeByCode.TryGetValue(catalogCode, out var fuzzy))
            {
                return fuzzy.Id;
            }
        }

        return typeByCode.GetValueOrDefault("CARIES")?.Id;
    }

    private static Guid? ResolveSurfaceId(
        DentalFindingItemDto findingDto,
        IReadOnlyDictionary<string, DentalSurfaceEntity> surfaceByCode)
    {
        if (findingDto.DentalSurfaceId is { } id && id != Guid.Empty)
            return id;

        if (string.IsNullOrWhiteSpace(findingDto.SurfaceCode))
            return null;

        var raw = findingDto.SurfaceCode.Trim();

        if (SurfaceShortToCode.TryGetValue(raw, out var fullCode)
            && surfaceByCode.TryGetValue(fullCode, out var byShort))
        {
            return byShort.Id;
        }

        if (surfaceByCode.TryGetValue(raw, out var byCode))
            return byCode.Id;

        return null;
    }

    private static OdontogramResponseDto Map(OdontogramEntity e) => new()
    {
        Id = e.Id,
        ClinicalHistoryId = e.ClinicalHistoryId,
        ClinicalAttentionId = e.ClinicalAttentionId,
        ProfessionalId = e.ProfessionalId,
        RecordedAt = e.RecordedAt,
        Notes = e.Notes
    };
}
