# Ejecutar NexusOdonto con Docker

Levanta la API y la base de datos Oracle en dos contenedores, con las tablas
creadas y los catálogos poblados automáticamente. No necesitas instalar .NET,
el SDK ni Oracle en tu máquina.

---

## Requisitos

| | |
|---|---|
| Docker Desktop | Windows, macOS o Linux |
| Memoria asignada a Docker | **mínimo 4 GB** |
| Puertos libres | `8080` (API) y `1521` (Oracle) |

> **La memoria no es opcional.** Oracle necesita cerca de 2 GB para arrancar. Con
> menos, el contenedor muere sin un mensaje claro.
> En Docker Desktop: *Settings → Resources → Memory*.

---

## Antes de empezar: el archivo `.env`

El repositorio **no incluye credenciales**. Antes del primer arranque tienes que
crear un `.env` en la raíz del proyecto con estas variables:

| Variable | Qué es |
|---|---|
| `ORACLE_PASSWORD` | Contraseña de `SYS` / `SYSTEM` |
| `ORACLE_APP_USER` | Usuario de la aplicación |
| `ORACLE_APP_PASSWORD` | Su contraseña |
| `ORACLE_PDB` | Nombre del PDB (`XEPDB1` con la imagen actual) |
| `ORACLE_PORT` | Puerto de Oracle en tu máquina |
| `API_PORT` | Puerto de la API en tu máquina |
| `ASPNETCORE_ENVIRONMENT` | `Development` o `Production` |
| `JWT_ISSUER` | Emisor del token |
| `JWT_AUDIENCE` | Audiencia del token |
| `JWT_SIGNING_KEY` | Clave de firma, **mínimo 32 bytes** |
| `JWT_EXPIRATION_MINUTES` | Vigencia del token |
| `ConnectionStrings__OracleConnection` | Solo para `dotnet run` fuera de Docker |

Los valores concretos los pide al equipo; no están en el repositorio a
propósito. El `.gitignore` excluye `*.env`, así que no hay riesgo de subirlo.

Si falta alguna, `docker compose up` avisa con `variable is not set`.

---

## Opción A — Desde el repositorio (recomendada)

Construye la imagen en tu propia máquina, así que funciona igual en Intel, AMD
y Apple Silicon.

```bash
git clone https://github.com/NexusOdonto/NexusOdontoBackend_Api.git
cd NexusOdontoBackend_Api

docker compose up -d --build
```

## Opción B — Desde la imagen `.tar`

Para quien recibe el archivo ya construido. **La imagen es `linux/amd64`**
(Intel/AMD); en Apple Silicon usa la Opción A.

```bash
docker load -i nexusodonto-backend-api-amd64.tar

docker compose -f docker-compose.image.yml up -d
```

La diferencia entre los dos compose es solo que `docker-compose.image.yml` usa
la imagen ya cargada en vez de construirla.

---

## El primer arranque tarda ~3 minutos

Oracle inicializa la base desde cero la primera vez. La API **espera** a que
termine: no se cae, no hay que reintentar.

Para seguir el progreso:

```bash
docker compose logs -f
```

Las señales de que terminó, en orden:

```
odontologia_oracle  | DATABASE IS READY TO USE!
odontologia_api     | Aplicando migraciones pendientes...
odontologia_api     | Migraciones aplicadas.
odontologia_api     | Now listening on: http://[::]:8080
```

Los arranques siguientes tardan segundos, porque los datos quedan en un volumen.

---

## Verificar que funciona

**Swagger:** abre <http://localhost:8080/swagger>

**Desde la terminal:**

```bash
curl -i -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"documentNumber":"1","password":"x"}'
```

Debe responder **401** con `{"message":"Invalid email or password."}`.

Un 401 es la señal de éxito: significa que la API consultó Oracle de verdad y
no encontró ese usuario. Un **500** indicaría que no llegó a la base.

**Los catálogos cargados:**

```bash
source .env   # carga ORACLE_APP_USER y ORACLE_APP_PASSWORD

docker compose exec oracle-db sqlplus -s \
  "$ORACLE_APP_USER/$ORACLE_APP_PASSWORD@localhost:1521/$ORACLE_PDB" <<'SQL'
select (select count(*) from "teeth")          as dientes,
       (select count(*) from "roles")          as roles,
       (select count(*) from "permissions")    as permisos
from dual;
SQL
```

Esperado: **52 dientes**, **5 roles**, **32 permisos**.

---

## Conectarte a la base con un cliente

Desde DBeaver, SQL Developer o similar, en tu máquina:

| Campo | Valor |
|---|---|
| Host | `localhost` |
| Puerto | valor de `ORACLE_PORT` |
| Service name | valor de `ORACLE_PDB` |
| Usuario | valor de `ORACLE_APP_USER` |
| Contraseña | valor de `ORACLE_APP_PASSWORD` |

Para tareas de administración el usuario es `system`, con la contraseña de
`ORACLE_PASSWORD`.

> Todos estos valores están en tu `.env`. Consúltalos ahí; no se documentan en
> el repositorio a propósito.

---

## Comandos habituales

```bash
docker compose ps                      # que esta corriendo
docker compose logs -f backend-api     # logs solo de la API
docker compose logs -f oracle-db       # logs solo de Oracle
docker compose restart backend-api     # reiniciar la API
docker compose down                    # apagar (conserva los datos)
docker compose down -v                 # apagar y BORRAR la base
docker compose up -d --build           # reconstruir tras cambiar codigo
```

> ⚠️ `down -v` borra el volumen de Oracle. El siguiente arranque vuelve a tardar
> 3 minutos y se pierde todo lo que hayas cargado a mano.

---

## Problemas frecuentes

### `variable is not set` al hacer `up`

Falta el `.env` o le falta alguna variable. Revisa la tabla de la sección
*Antes de empezar*.

### `port is already allocated`

Algo ya usa el 8080 o el 1521. Cambia los puertos en tu `.env`:

```
API_PORT=8081
ORACLE_PORT=1522
```

Solo cambia el puerto de tu máquina; dentro de la red de Docker siguen siendo
los mismos.

### Oracle arranca y se muere sin explicación

Casi siempre es memoria. Sube Docker Desktop a 4 GB o más y:

```bash
docker compose down -v
docker compose up -d
```

### `exec format error` al cargar el `.tar`

Estás en ARM (Apple Silicon) y la imagen es amd64. Usa la **Opción A**.

### La API responde 500 en vez de 401

No logró conectar con Oracle. Revisa el error concreto:

```bash
docker compose logs backend-api | grep -E "ORA-|Exception"
```

- `ORA-00942` → faltan las migraciones (no debería pasar; son automáticas)
- `ORA-01017` → usuario o contraseña mal en el `.env`
- Timeout → Oracle todavía no terminó de arrancar

### Cambié el usuario de Oracle en el `.env` y no funciona

La imagen crea el usuario **solo en la primera inicialización**. Hay que
recrear el volumen:

```bash
docker compose down -v
docker compose up -d
```

---

## Cómo está armado

```
┌─────────────────────┐        ┌─────────────────────┐
│   backend-api       │        │     oracle-db       │
│   :8080             │───────▶│     :1521           │
│   nexusodonto/      │        │  gvenzl/oracle-free │
│   backend-api       │        │                     │
└─────────────────────┘        └─────────────────────┘
         odontologia_network (bridge)
```

**Los contenedores se hablan por nombre de servicio.** La cadena de conexión
usa `oracle-db:1521`, no `localhost`: dentro de un contenedor, `localhost` es
ese mismo contenedor. Docker resuelve `oracle-db` por su DNS interno.

El mapeo `1521:1521` existe solo para que **tú** puedas conectarte con un
cliente desde fuera.

**El arranque está ordenado por un healthcheck.** La API no arranca hasta que
Oracle acepta conexiones, no solo hasta que el contenedor existe.

**Las migraciones se aplican solas**, únicamente cuando la app detecta que corre
en contenedor (variable `DOTNET_RUNNING_IN_CONTAINER`). Fuera de Docker el
comportamiento no cambia.

**El `.env` cumple dos funciones a la vez:** Docker Compose lo lee para
sustituir las `${VARIABLES}` del YAML, y la API lo lee con DotNetEnv cuando
corres `dotnet run` fuera de Docker. Las claves no se pisan entre sí.

---

## Correr sin Docker

Necesitas .NET 10 SDK y una instancia de Oracle accesible.

Ajusta `ConnectionStrings__OracleConnection` en tu `.env` para que apunte a tu
Oracle local y:

```bash
dotnet run --project Api
```

Fuera de Docker el `.env` **sí es obligatorio**: la app falla al arrancar si no
lo encuentra.
