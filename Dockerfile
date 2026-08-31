# --platform=$BUILDPLATFORM: el SDK corre en la arquitectura nativa de quien
# construye. La salida de "dotnet publish" es IL portable, asi que solo la
# imagen de runtime necesita coincidir con la arquitectura destino.
# Sin esto, compilar amd64 desde un Mac ARM emula el SDK con QEMU y falla.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.slnx ./
COPY Api/*.csproj ./Api/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
RUN dotnet restore

COPY . ./
WORKDIR /app/Api
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]