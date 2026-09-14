using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
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
                history = new ClinicalHistoryEntity(request.PatientId.Value, DateTime.UtcNow, "Historia clínica inicial generada automáticamente para odontograma.");
                await clinicalHistoryRepository.AddAsync(history);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            clinicalHistoryId = history.Id;
        }
        else
        {
            throw new ValidationException("Debe indicar PatientId o ClinicalHistoryId para registrar el odontograma.");
        }

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
                    request.ProfessionalId,
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

            var defaultToothStatus = new Guid("1a000000-0000-0000-0000-000000000001"); // SANO
            var defaultFindingStatus = new Guid("1d000000-0000-0000-0000-000000000001"); // ACTIVO

            var toothEntityByCode = new Dictionary<string, OdontogramToothEntity>(StringComparer.OrdinalIgnoreCase);

            foreach (var toothDto in request.Teeth)
            {
                var statusId = toothDto.ToothStatusId == Guid.Empty ? defaultToothStatus : toothDto.ToothStatusId;
                var tooth = existingTeeth.FirstOrDefault(t => string.Equals(t.ToothCode, toothDto.ToothCode, StringComparison.OrdinalIgnoreCase));
                if (tooth is null)
                {
                    tooth = new OdontogramToothEntity(odontogram.Id, toothDto.ToothCode.Trim(), statusId, toothDto.Note);
                    await odontogramToothRepository.AddAsync(tooth);
                }
                else
                {
                    tooth.Update(statusId, toothDto.Note);
                    odontogramToothRepository.Update(tooth);
                }
                toothEntityByCode[toothDto.ToothCode.Trim()] = tooth;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var existing in existingTeeth)
            {
                if (!toothEntityByCode.ContainsKey(existing.ToothCode))
                {
                    toothEntityByCode[existing.ToothCode] = existing;
                }
            }

            if (request.Findings.Count > 0)
            {
                foreach (var findingDto in request.Findings)
                {
                    if (string.IsNullOrWhiteSpace(findingDto.ToothCode)) continue;

                    if (!toothEntityByCode.TryGetValue(findingDto.ToothCode.Trim(), out var toothEntity))
                    {
                        toothEntity = new OdontogramToothEntity(odontogram.Id, findingDto.ToothCode.Trim(), defaultToothStatus);
                        await odontogramToothRepository.AddAsync(toothEntity);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        toothEntityByCode[findingDto.ToothCode.Trim()] = toothEntity;
                    }

                    var findingStatusId = findingDto.FindingStatusId == Guid.Empty ? defaultFindingStatus : findingDto.FindingStatusId;
                    var finding = new DentalFindingEntity(
                        toothEntity.Id,
                        findingDto.FindingTypeId,
                        findingStatusId,
                        findingDto.DentalSurfaceId == Guid.Empty ? null : findingDto.DentalSurfaceId,
                        findingDto.Note);
                    await dentalFindingRepository.AddAsync(finding);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return await BuildFullOdontogramResponseAsync(odontogram, cancellationToken);
        });
    }

    public async Task<FullOdontogramResponseDto?> GetLatestByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        var history = await clinicalHistoryRepository.GetByPatientIdAsync(patientId, cancellationToken);
        if (history is null)
        {
            return null;
        }

        var allOdontos = await repository.GetAllAsync();
        var latest = allOdontos
            .Where(o => o.ClinicalHistoryId == history.Id)
            .OrderByDescending(o => o.RecordedAt)
            .FirstOrDefault();

        if (latest is null)
        {
            return null;
        }

        return await BuildFullOdontogramResponseAsync(latest, cancellationToken);
    }

    public async Task<FullOdontogramResponseDto?> GetFullByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
        {
            return null;
        }

        return await BuildFullOdontogramResponseAsync(entity, cancellationToken);
    }

    private async Task<FullOdontogramResponseDto> BuildFullOdontogramResponseAsync(
        OdontogramEntity odontogram,
        CancellationToken cancellationToken)
    {
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
            var toothFindings = findings
                .Where(f => f.OdontogramToothId == tooth.Id)
                .Select(f =>
                {
                    typeById.TryGetValue(f.FindingTypeId, out var ft);
                    surfaceById.TryGetValue(f.DentalSurfaceId ?? Guid.Empty, out var ds);
                    findingStatusById.TryGetValue(f.FindingStatusId, out var fs);

                    return new FullDentalFindingResponseDto
                    {
                        Id = f.Id,
                        DentalSurfaceId = f.DentalSurfaceId,
                        DentalSurfaceCode = ds?.Code,
                        FindingTypeId = f.FindingTypeId,
                        FindingTypeCode = ft?.Code,
                        FindingTypeName = ft?.Name,
                        FindingTypeColorHex = ft?.ColorHex,
                        FindingStatusId = f.FindingStatusId,
                        FindingStatusCode = fs?.Code,
                        Note = f.Note,
                        RegisteredAt = f.RegisteredAt
                    };
                })
                .ToList();

            teethResponse.Add(new FullOdontogramToothResponseDto
            {
                Id = tooth.Id,
                ToothCode = tooth.ToothCode,
                ToothStatusId = tooth.ToothStatusId,
                ToothStatusCode = ts?.Code,
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
