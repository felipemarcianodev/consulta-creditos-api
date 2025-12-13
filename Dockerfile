FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/ConsultaCreditos.API/ConsultaCreditos.API.csproj", "src/ConsultaCreditos.API/"]
COPY ["src/ConsultaCreditos.Application/ConsultaCreditos.Application.csproj", "src/ConsultaCreditos.Application/"]
COPY ["src/ConsultaCreditos.Domain/ConsultaCreditos.Domain.csproj", "src/ConsultaCreditos.Domain/"]
COPY ["src/ConsultaCreditos.Infrastructure/ConsultaCreditos.Infrastructure.csproj", "src/ConsultaCreditos.Infrastructure/"]
COPY ["src/ConsultaCreditos.Shared/ConsultaCreditos.Shared.csproj", "src/ConsultaCreditos.Shared/"]

RUN dotnet restore "src/ConsultaCreditos.API/ConsultaCreditos.API.csproj"

COPY . .
WORKDIR "/src/src/ConsultaCreditos.API"
RUN dotnet build "ConsultaCreditos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsultaCreditos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsultaCreditos.API.dll"]
