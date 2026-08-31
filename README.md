<div align="center">

# NexusOdonto API

**Backend del sistema de gestión odontológica NexusOdonto**

API REST para administrar pacientes, agenda, historias clínicas, odontograma,
notificaciones y soporte con chatbot.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4)
![Oracle](https://img.shields.io/badge/Oracle-23ai-F80000)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED)
![License](https://img.shields.io/badge/license-MIT-green)

</div>

---

## Funcionalidades

- **Pacientes e historia clínica** — registro, antecedentes médicos, atenciones,
  diagnósticos y procedimientos realizados
- **Agenda** — citas con disponibilidad configurable por profesional
- **Odontograma** — notación FDI (52 piezas), estados por diente y hallazgos por
  superficie dental
- **Chatbot y soporte** — conversaciones con escalamiento a tickets
- **Notificaciones** — internas y por WhatsApp
- **Auditoría** — registro de eventos con categoría y nivel de riesgo
- **Control de acceso** — permisos granulares por módulo y acción, con JWT y
  refresh tokens

**109 rutas · 254 operaciones · 52 recursos**

---

## Arranque rápido

```bash
git clone https://github.com/NexusOdonto/NexusOdontoBackend_Api.git
cd NexusOdontoBackend_Api

# crea el .env con las variables del equipo (ver DOCKER.md)

docker compose up -d --build
```

Listo. Levanta la API y Oracle, aplica las migraciones y carga los catálogos.

> El primer arranque tarda unos 3 minutos mientras Oracle inicializa.

**Swagger:** <http://localhost:8080/swagger>

Detalles completos —variables de entorno, requisitos de memoria, verificación y
solución de problemas— en **[DOCKER.md](DOCKER.md)**.

---

## Ejemplo de uso

**Autenticarse:**

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"login":"usuario","password":"contrasena"}'
```

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAtUtc": "2026-08-31T15:30:00Z",
  "refreshToken": "9f8c2e14-...",
  "refreshExpiresAtUtc": "2026-09-07T14:30:00Z",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "personId": "1b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed",
    "name": "Ana Ramirez",
    "email": "ana@ejemplo.com",
    "roleIds": ["13000000-0000-0000-0000-000000000002"]
  },
  "permissions": ["PATIENTS_VIEW", "APPOINTMENTS_CREATE", "CLINICAL_EDIT"]
}
```

**Consumir un recurso protegido:**

```bash
curl http://localhost:8080/api/v1/DocumentTypes \
  -H "Authorization: Bearer $TOKEN"
```

```json
[
  {
    "id": "e0000000-0000-0000-0000-000000000001",
    "code": "CC",
    "name": "Citizenship Card",
    "description": "National identity document for Colombian citizens.",
    "isActive": true,
    "sortOrder": 1,
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

## Índice

- [Stack](#stack)
- [Arquitectura](#arquitectura)
- [Estructura del repositorio](#estructura-del-repositorio)
- [Autenticación y autorización](#autenticación-y-autorización)
- [Endpoints](#endpoints)
- [Swagger](#swagger)
- [Modelo de dominio](#modelo-de-dominio)
- [Base de datos](#base-de-datos)
- [Convenciones de código](#convenciones-de-código)
- [Contribuir](#contribuir)
- [Roadmap](#roadmap)

---

## Stack

| Componente | Tecnología |
|---|---|
| Runtime | .NET 10 |
| Base de datos | Oracle (`Oracle.EntityFrameworkCore` 10.23) |
| ORM | Entity Framework Core 10 |
| Autenticación | JWT Bearer + refresh tokens |
| Hashing | BCrypt.Net-Next |
| Validación | FluentValidation 12 |
| Mapeo | Mapster 10 |
| Logging | Serilog (consola y archivo) |
| Documentación | Swashbuckle / OpenAPI |
| Resiliencia HTTP | Microsoft.Extensions.Http.Resilience |
| Contenedores | Docker Compose |

### Correr sin Docker

Requiere el SDK de .NET 10 y una instancia de Oracle accesible.

```bash
dotnet restore
dotnet ef database update --project Infrastructure --startup-project Api
dotnet run --project Api
```

Fuera de contenedor el `.env` es obligatorio: la aplicación falla al arrancar si
no lo encuentra.

---

## Arquitectura

Clean Architecture en cuatro proyectos. Las dependencias apuntan **hacia
adentro**: cada capa conoce solo a las de su interior.

```
┌──────────────────────────────────────────────┐
│  Api             Controladores, middleware,  │
│                  filtros, composición        │
├──────────────────────────────────────────────┤
│  Infrastructure  EF Core, repositorios,      │
│                  UnitOfWork, seeders         │
├──────────────────────────────────────────────┤
│  Application     Casos de uso, DTOs,         │
│                  validadores, contratos      │
├──────────────────────────────────────────────┤
│  Domain          Entidades, Value Objects    │
│                  Sin dependencias externas   │
└──────────────────────────────────────────────┘
```

```
Api → Infrastructure → Application → Domain
```

**Consecuencia práctica:** `Application` **no puede** usar `AppDbContext`. Todo
acceso a datos pasa por interfaces de repositorio declaradas en
`Application/Contracts/` e implementadas en `Infrastructure/Repositories/`.

`Domain` no referencia ningún paquete NuGet: son POCOs y Value Objects puros.

---

## Estructura del repositorio

```
Domain/
├── Common/          Entity, BaseEntity, CatalogEntity
├── Entities/        52 entidades en 9 módulos
└── ValueObjects/    Objetos de valor con validación

Application/
├── Contracts/       Interfaces de repositorios y servicios
├── DTOs/            Request y Response por módulo
├── Services/        Casos de uso
├── Validators/      Reglas de FluentValidation
├── Exceptions/      NotFoundException, ValidationException
└── DependencyInjection.cs

Infrastructure/
├── Configurations/  IEntityTypeConfiguration por entidad
├── Data/
│   ├── Migrations/  Migraciones de EF Core
│   └── Seeder/      Datos semilla de catálogos
├── Persistence/     AppDbContext
├── Repositories/    Implementaciones
└── UnitOfWorks/     Transacciones

Api/
├── Controllers/     55 controladores en 10 módulos
├── Extensions/      Autenticación, Swagger, HTTP resiliente
├── Filters/         Filtros de acción
├── Middleware/      Manejo global de excepciones
└── Program.cs
```

---

## Autenticación y autorización

### Flujo

```
POST /api/auth/login
      │
      │  { "login": "...", "password": "..." }
      ▼
  Verifica con BCrypt, crea una sesión y devuelve:
      │
      ├─ token               JWT de acceso (corta duración)
      ├─ refreshToken        para renovar sin volver a autenticarse
      ├─ user                perfil del usuario
      └─ permissions         permisos efectivos
```

Las peticiones siguientes llevan el token en la cabecera:

```
Authorization: Bearer <token>
```

Cuando el token de acceso caduca, `POST /api/auth/refresh` entrega uno nuevo.
`POST /api/auth/logout` revoca la sesión.

### Endpoints de autenticación

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| `POST` | `/api/auth/login` | anónimo | Inicia sesión |
| `POST` | `/api/auth/refresh` | anónimo | Renueva el token de acceso |
| `POST` | `/api/auth/logout` | anónimo | Revoca el refresh token |
| `GET` | `/api/auth/me` | Bearer | Perfil y permisos del usuario actual |

### Permisos

La autorización no se basa en roles sino en **permisos granulares** de la forma
`MODULO` × `ACCION`.

**Módulos:** `APPOINTMENTS` · `CATALOGS` · `CHATBOT` · `CLINICAL` ·
`ODONTOGRAM` · `PATIENTS` · `PEOPLE` · `PERMISSIONS` · `ROLES` · `USERS`

**Acciones:** `VIEW` · `CREATE` · `EDIT` · `DELETE`

Cada controlador declara su módulo, y la clase base verifica el permiso según el
verbo HTTP:

| Verbo | Acción requerida |
|---|---|
| `GET` | `VIEW` |
| `POST` | `CREATE` |
| `PUT` | `EDIT` |
| `DELETE` | `DELETE` |

Los permisos se asignan a roles, y los roles a usuarios.

### Códigos de respuesta

| Código | Significado |
|---|---|
| `400` | El cuerpo no pasó las validaciones |
| `401` | Sin token, o token inválido o expirado |
| `403` | Autenticado pero sin el permiso necesario |
| `404` | El recurso no existe |
| `409` | Conflicto (por ejemplo, código duplicado) |

---

## Endpoints

Todos los recursos cuelgan de `/api/v1/`, salvo autenticación en `/api/auth/`.

### Contrato CRUD estándar

La mayoría de recursos expone las mismas cinco operaciones:

| Método | Ruta | Respuesta |
|---|---|---|
| `GET` | `/api/v1/{recurso}` | `200` lista completa |
| `GET` | `/api/v1/{recurso}/{id}` | `200` o `404` |
| `POST` | `/api/v1/{recurso}` | `201` + cabecera `Location` |
| `PUT` | `/api/v1/{recurso}/{id}` | `200` o `404` |
| `DELETE` | `/api/v1/{recurso}/{id}` | `204` o `404` |

Los identificadores son `Guid`.

Algunos recursos omiten alguna operación —por ejemplo `AuditEvents`, que no
permite creación manual porque los eventos los registra el sistema.

### Recursos por módulo

| Módulo | Recursos |
|---|---|
| **Personas** | `Persons` · `Patients` · `Employees` · `Professionals` · `Users` · `Sessions` |
| **Seguridad** | `Roles` · `Permissions` · `UserRoles` |
| **Agenda** | `Appointments` · `Availabilities` |
| **Clínico** | `ClinicalHistories` · `ClinicalAttentions` · `Diagnoses` · `ProceduresPerformed` · `PatientAntecedents` |
| **Odontograma** | `Odontograms` · `OdontogramTeeth` · `DentalFindings` · `Teeth` |
| **Notificaciones** | `HubNotifications` · `WhatsAppNotifications` |
| **Chatbot** | `ChatbotConversations` · `ChatbotMessages` · `SupportTickets` |
| **Auditoría** | `AuditEvents` |

**Catálogos** (26 recursos) — `ActionPermissions` · `AntecedentTypes` ·
`AppointmentOrigins` · `AppointmentStatuses` · `AuditEventTypes` ·
`ChatChannels` · `ChatbotConversationStatuses` · `ChatbotMessageRoles` ·
`DentalSurfaces` · `DentitionTypes` · `DocumentTypes` · `FindingStatuses` ·
`FindingTypes` · `HubNotificationStatuses` · `HubNotificationTypes` ·
`JobTitles` · `NotificationPriorities` · `Services` · `SessionStatuses` ·
`Sexes` · `Specialties` · `SupportTicketReasons` · `SupportTicketStatuses` ·
`ToothStatuses` · `WhatsAppNotificationStatuses` · `WhatsAppNotificationTypes`

Todos los catálogos comparten la misma forma: `code`, `name`, `description`,
`sortOrder`, `isActive`. Algunos añaden campos propios —`ToothStatuses` y
`FindingTypes` llevan color e ícono para renderizar el odontograma.

---

## Swagger

Con la aplicación en ejecución:

- **Interfaz:** <http://localhost:8080/swagger>
- **Especificación:** <http://localhost:8080/swagger/v1/swagger.json>

Habilitado solo en el entorno `Development`.

### Probar endpoints protegidos

1. Ejecuta `POST /api/auth/login` con tus credenciales
2. Copia el valor de `token` de la respuesta
3. Pulsa **Authorize** (arriba a la derecha) y pega el token
4. Los endpoints con candado ya envían la cabecera `Authorization`

La especificación documenta 123 esquemas de DTOs con los códigos de respuesta de
cada operación.

---

## Modelo de dominio

**52 entidades** repartidas en nueve módulos:

| Módulo | Entidades | Contenido |
|---|---|---|
| `Catalogs` | 26 | Tablas de referencia |
| `People` | 6 | Person, Patient, Employee, Professional, User, Session |
| `Clinical` | 5 | ClinicalHistory, ClinicalAttention, Diagnosis, ProcedurePerformed, PatientAntecedent |
| `Odontogram` | 4 | Odontogram, OdontogramTooth, DentalFinding, Tooth |
| `Chatbot` | 3 | ChatbotConversation, ChatbotMessage, SupportTicket |
| `Security` | 3 | Role, Permission, UserRole |
| `Notifications` | 2 | HubNotification, WhatsAppNotification |
| `Schedule` | 2 | Appointment, Availability |
| `Audit` | 1 | AuditEvent |

### Jerarquía de personas

`Person` es la raíz. Paciente, empleado y usuario no existen por sí solos:

```
Person
├── Patient        1:1  ── ClinicalHistory  1:1
├── Employee       1:1  ── Professional     1:1
└── User           1:1  ── UserRole  N:N  Role  N:N  Permission
```

Crear un paciente inserta en `persons`, `patients` e historia clínica dentro de
una sola transacción.

### Clases base

| Clase | Aporta |
|---|---|
| `Entity` | `Id` (`Guid`) |
| `BaseEntity : Entity` | `CreatedAt`, `UpdatedAt`, `IsActive` |
| `CatalogEntity : BaseEntity` | `Code`, `Name`, `Description`, `SortOrder` |

### Value Objects

Encapsulan reglas de negocio en el tipo. Se declaran como `sealed record` para
obtener comparación por valor, con constructor privado y una fábrica `Create`
que valida:

```csharp
var licencia = ProfessionalLicense.Create(entrada);
// lanza ArgumentException si viene vacia o supera 50 caracteres
```

Ejemplos: `CatalogCode`, `Price`, `HireDate`, `ProfessionalLicense`,
`PhoneNumber`, `RagConfidence`, `TimeOfDay`.

---

## Base de datos

**Oracle**, con nombres de tabla y columna en `snake_case` y minúsculas.

> ⚠️ Oracle trata los identificadores entrecomillados como **sensibles a
> mayúsculas**. Al consultar a mano hay que citarlos:
> `select * from "patients"` — sin comillas busca `PATIENTS` y no la encuentra.

### Migraciones

```bash
# crear una migración
dotnet ef migrations add NombreDeLaMigracion \
  --project Infrastructure --startup-project Api --output-dir Data/Migrations

# aplicarla
dotnet ef database update --project Infrastructure --startup-project Api
```

En contenedor **se aplican solas al arrancar**, detectando la variable
`DOTNET_RUNNING_IN_CONTAINER`. Fuera de Docker el comportamiento no cambia.

### Datos semilla

Los catálogos se cargan con `HasData()`, de modo que viajan **dentro de la
migración**: al aplicarla, las tablas quedan pobladas.

Incluye entre otros:

- 52 piezas dentales en notación FDI (32 permanentes, 20 temporales)
- Roles, permisos y acciones del sistema
- Tipos de documento, sexos, cargos, especialidades
- Estados y tipos del odontograma con sus colores

Los identificadores del seed son `Guid` **fijos**, escritos como literales. Si
se generaran en tiempo de ejecución, cada migración detectaría datos distintos y
produciría un `DeleteData` + `InsertData` completo.

---

## Convenciones de código

**Nomenclatura en inglés** en todo el proyecto: clases, propiedades, tablas y
columnas.

**Un tipo público por archivo**, con el mismo nombre que el archivo.

**El namespace refleja la ruta de carpetas:**
`Domain/Entities/People/` → `Domain.Entities.People`

**DTOs** siguen el patrón `CreateXRequestDto`, `UpdateXRequestDto`,
`XResponseDto`, agrupados por módulo.

**Nulabilidad alineada al esquema:** toda propiedad cuya columna admite `NULL`
se declara anulable. Las obligatorias van no anulables con `= string.Empty` o
`= null!`.

**camelCase en JSON** sin atributos: `AddControllers()` usa
`JsonSerializerDefaults.Web`, que aplica `JsonNamingPolicy.CamelCase` en ambos
sentidos.

**Las validaciones viven en `Application/Validators/`** con FluentValidation, no
en los controladores.

**Nunca se commitean credenciales.** El `.gitignore` excluye `*.env`; los
valores se comparten fuera del repositorio.

---

## Contribuir

Rama base: **`develop`**

```bash
git checkout develop
git pull
git checkout -b feat/nombre-descriptivo
```

Prefijos: `feat/` · `fix/` · `refactor/` · `docs/`

Antes de abrir un Pull Request contra `develop`:

```bash
dotnet build     # sin errores ni advertencias nuevas
```

Si la rama toca entidades o configuraciones de EF, genera la migración sobre
ella antes de mergear: los errores de `HasData` y de relaciones no aparecen al
compilar, solo al construir el modelo.

---

## Roadmap

Pendientes conocidos del proyecto:

- [ ] **Pruebas automatizadas** — no existe proyecto de tests
- [ ] **Integración continua** — sin GitHub Actions configurado
- [ ] **Paginación** en los endpoints de listado, que hoy devuelven la
      colección completa
- [ ] **Mover la clave JWT de `appsettings.json` al `.env`**
- [ ] **Fijar la versión de la imagen de Oracle** en lugar de `:latest`

---

## Licencia

MIT — ver [LICENSE](LICENSE).
