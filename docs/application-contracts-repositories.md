# Contratos de repositorio — Application/Contracts/Repositories

**Autor:** Alejandro Escobar  
**Fecha:** 2026-08-27

## Qué se hizo

Se definieron los contratos de acceso a datos en `Application/Contracts/Repositories`, organizados por módulo (alineado con `Application/Contracts/Services`): un CRUD genérico (`IRepository<T>`), unidad de trabajo (`IUnitOfWork`) y repositorios específicos por entidad solo cuando existe una consulta, include o clave de negocio que el genérico no cubre.

Las interfaces viven en Application; las implementaciones EF Core están en `Infrastructure/Repositories` y se registran en `Infrastructure/DependencyInjection.cs`.

### Estructura de carpetas y namespaces

```
Application/Contracts/Repositories/
  Common/       → Application.Contracts.Repositories.Common
    IRepository.cs
    IUnitOfWork.cs
  People/       → Application.Contracts.Repositories.People
    IUserRepository.cs
    IPersonRepository.cs
    IPatientRepository.cs
    IEmployeeRepository.cs
    IProfessionalRepository.cs
    ISessionRepository.cs
  Security/     → Application.Contracts.Repositories.Security
    IRoleRepository.cs
    IPermissionRepository.cs
  Clinical/     → Application.Contracts.Repositories.Clinical
    IClinicalHistoryRepository.cs
    IClinicalAttentionRepository.cs
  Schedule/     → Application.Contracts.Repositories.Schedule
    IAppointmentRepository.cs
    IAvailabilityRepository.cs
  Odontogram/   → Application.Contracts.Repositories.Odontogram
    IToothRepository.cs
  Catalogs/     → Application.Contracts.Repositories.Catalogs
    ICatalogRepository.cs
    IServiceRepository.cs
  Chatbot/      → Application.Contracts.Repositories.Chatbot
    IChatbotConversationRepository.cs
    ISupportTicketRepository.cs
```

Las interfaces que heredan `IRepository<T>` importan `Application.Contracts.Repositories.Common`. En consumidores (servicios, controllers), el patrón recomendado es:

```csharp
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People; // solo si usa IPatientRepository, etc.
```

## Por qué

Separar responsabilidades:

| Capa del contrato | Rol |
|-------------------|-----|
| `IRepository<T>` | CRUD estándar (`GetById`, `GetAll`, `Add`, `Update`, `Remove`) para cualquier `Entity`. |
| `IUnitOfWork` | Persistencia y transacciones (`SaveChanges`, begin/commit/rollback). |
| `I{Entity}Repository` | Consultas únicas: índices, joins, includes de navegación, lookups de autenticación/autorización. |

Así Application depende de abstracciones, no de EF; Infrastructure podrá implementarlas después sin cambiar los casos de uso.

## Convención

1. **`IRepository<TEntity> where TEntity : Entity`** — base genérica.
2. **`I{Entity}Repository : IRepository<T>`** — solo si hay al menos un método que el genérico no ofrece.
3. **No** crear una interfaz por cada entidad del Domain “por si acaso”.
4. Firmas async con `CancellationToken cancellationToken = default` cuando aplica.
5. **Excepción:** `IToothRepository` no hereda `IRepository<Tooth>` porque `Tooth` no es `Entity` (PK string FDI `Code`, no `Guid Id`).
6. **Catálogos:** `ICatalogRepository<T> where T : CatalogEntity` centraliza `GetByCodeAsync` para tablas de catálogo; `Service` tiene además `IServiceRepository` por ser catálogo con campos propios (duración, precio) y uso frecuente por código.

## Login: email o número de documento

El campo `Login` de `LoginRequestDto` acepta **email** o **número de documento**. No hay columna `Username` en `User`; el email vive en `Person`.

Flujo en `AuthService.LoginAsync`:

1. Si `Login` contiene `@` → se usa como email.
2. Si no → `GetEmailByDocumentNumberAsync(Login)` resuelve `Person.DocumentNumber` → `Person.Email`. Si no hay email, el login falla (`null`).
3. En ambos casos → `GetByEmailWithRolesAsync(email)` carga el usuario con roles; luego se verifica la contraseña y se emite el JWT.

```text
Login (email | DocumentNumber)
        │
        ├─ contiene "@" ──────────────────────► email
        │
        └─ documento ─► GetEmailByDocumentNumberAsync ─► email
                                                          │
                                                          ▼
                                              GetByEmailWithRolesAsync
                                                          │
                                                          ▼
                                              verificar password + token
```

Métodos de `IUserRepository` involucrados: `GetEmailByDocumentNumberAsync`, `GetByEmailWithRolesAsync` (más `Update` del genérico para `RegisterAccess`).

## Inventario de interfaces

### Common (`Application.Contracts.Repositories.Common`)

| Interfaz | Métodos | Justificación |
|----------|---------|---------------|
| `IRepository<TEntity>` | `GetByIdAsync`, `GetAllAsync`, `AddAsync`, `Update`, `Remove` | CRUD genérico para entidades con `Guid Id`. |
| `IUnitOfWork` | `SaveChangesAsync`, `BeginTransactionAsync`, `CommitTransactionAsync`, `RollbackTransactionAsync` | Persistencia atómica y transacciones explícitas. |

### People (`Application.Contracts.Repositories.People`) / Security (`Application.Contracts.Repositories.Security`)

| Interfaz | Métodos específicos | Justificación |
|----------|---------------------|---------------|
| `IUserRepository` | `GetByEmailWithRolesAsync` | Login / carga de usuario con roles (include). |
| | `GetEmailByDocumentNumberAsync` | Resolver email desde documento para login sin username. |
| | `GetByPersonIdAsync` | Relación 1:1 `User`–`Person` (lookup por persona). |
| | `GetAllWithRolesAsync` | Listado administrativo con roles. |
| | `GetByIdWithRolesAsync` | Detalle / post-registro con roles. |
| `IPersonRepository` | `GetByDocumentAsync` | Unicidad documento (`DocumentTypeId` + `DocumentNumber`). |
| | `GetByEmailAsync` | Lookup / unicidad de email. |
| `IPatientRepository` | `GetByPersonIdAsync` | `person_id` unique. |
| | `GetByIdWithPersonAsync` | Paciente con datos de `Person` (include). |
| `IEmployeeRepository` | `GetByPersonIdAsync` | `person_id` unique. |
| `IProfessionalRepository` | `GetByEmployeeIdAsync` | Relación 1:1 con empleado. |
| | `GetByLicenseAsync` | Unicidad de licencia profesional. |
| | `GetByIdWithSpecialtiesAsync` | Include especialidades (M:N). |
| `ISessionRepository` | `GetByRefreshTokenHashAsync` | Refresh token único (hash). |
| | `GetActiveByUserIdAsync` | Sesiones activas de un usuario. |
| `IRoleRepository` | `GetByCodeAsync` | Código de rol único. |
| | `GetByIdWithPermissionsAsync` | Rol con permisos (include). |
| `IPermissionRepository` | `GetByModuleAndActionAsync` | Lookup módulo + acción (autorización). |

### Clinical, Schedule, Catalogs, Chatbot, Odontogram

| Interfaz | Carpeta / namespace | Métodos específicos | Justificación |
|----------|---------------------|---------------------|---------------|
|----------|---------------------|---------------|
| `IClinicalHistoryRepository` | `Clinical/` | `GetByPatientIdAsync` | Historia clínica 1:1 por paciente. |
| `IClinicalAttentionRepository` | `Clinical/` | `GetByAppointmentIdAsync` | Atención clínica ligada a una cita. |
| `IAppointmentRepository` | `Schedule/` | `GetByProfessionalInRangeAsync` | Agenda / detección de solapes en rango. |
| | | `GetByPatientAsync` | Citas de un paciente. |
| `IAvailabilityRepository` | `Schedule/` | `GetByProfessionalAsync` | Horarios de disponibilidad del profesional. |
| `IServiceRepository` | `Catalogs/` | `GetByCodeAsync` | Servicio por código único. |
| `ICatalogRepository<T>` | `Catalogs/` | `GetByCodeAsync` | Catálogos (`CatalogEntity`) por `Code` único. |
| `IChatbotConversationRepository` | `Chatbot/` | `GetByChannelAndChatIdAsync` | Unicidad `(canal, chat_id)`. |
| `ISupportTicketRepository` | `Chatbot/` | `GetOpenByConversationIdAsync` | Ticket abierto de handoff por conversación. |
| `IToothRepository` | `Odontogram/` | `GetByCodeAsync`, `GetAllAsync`, `AddAsync`, `Update`, `Remove` | CRUD con PK string FDI; no hereda `IRepository<Tooth>`. |

## Qué no se creó

No hay `I{Entity}Repository` para entidades que solo necesitarían el CRUD genérico (o el catálogo genérico), por ejemplo:

- `Diagnosis`, `ProcedurePerformed`, `PatientAntecedent`
- `DentalFinding`, `Odontogram`, `OdontogramTooth`
- `HubNotification`, `WhatsAppNotification`
- `AuditEvent`, `ChatbotMessage`
- `UserRole` (tabla de unión)
- Catálogos concretos (`DocumentType`, `Specialty`, `AppointmentStatus`, `Sex`, etc.): se consumen vía `ICatalogRepository<T>`, no con una interfaz por tabla

Si en el futuro aparece una consulta distinta del genérico, se agrega entonces el contrato específico.

## Implementado (Infrastructure)

| Componente | Ubicación |
|------------|-----------|
| `AppDbContext` | `Infrastructure/Persistence/Context/AppDbContext.cs` — `ApplyConfigurationsFromAssembly` |
| `Repository<T>` / `CatalogRepository<T>` | `Infrastructure/Repositories/Common/`, `Infrastructure/Repositories/Catalogs/` |
| 17 repos específicos | `Infrastructure/Repositories/{People,Security,Clinical,Schedule,Catalogs,Chatbot,Odontogram}/` |
| `UnitOfWork` | `Infrastructure/UnitOfWorks/UnitOfWork.cs` |
| Registro DI | `Infrastructure/DependencyInjection.cs` — genéricos + 17 interfaces |

Consultas críticas implementadas: `GetEmailByDocumentNumberAsync` (join `Person` → email), `GetByEmailWithRolesAsync` / `GetByIdWithRolesAsync` (includes roles → permisos → acción), `GetOpenByConversationIdAsync` (`ClosedAt == null`).

---

*Documentado por Alejandro Escobar — 2026-08-27*
