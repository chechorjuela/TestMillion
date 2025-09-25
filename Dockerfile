# =============================================================================
# Build stage
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/Presentation/TestMillion.Presentation.csproj", "src/Presentation/"]
COPY ["src/Application/TestMillion.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/TestMillion.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/TestMillion.Domain.csproj", "src/Domain/"]
COPY ["src/TestMillion.Shared/TestMillion.Shared.csproj", "src/TestMillion.Shared/"]
RUN dotnet restore "src/Presentation/TestMillion.Presentation.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Presentation"
RUN dotnet build "TestMillion.Presentation.csproj" -c Release -o /app/build

# =============================================================================
# Publish stage
# =============================================================================
FROM build AS publish
RUN dotnet publish "TestMillion.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =============================================================================
# Development stage
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS development
WORKDIR /app
COPY --from=build /app/build .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "TestMillion.Presentation.dll"]

# =============================================================================
# Production stage
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Create a non-root user
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "TestMillion.Presentation.dll"]
