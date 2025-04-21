# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 5002


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bazario.Identity/src/Bazario.Identity.WebAPI/Bazario.Identity.WebAPI.csproj", "Bazario.Identity.WebAPI/"]
COPY ["Bazario.Identity/src/Bazario.Identity.Infrastructure/Bazario.Identity.Infrastructure.csproj", "Bazario.Identity.Infrastructure/"]
COPY ["Bazario.Identity/src/Bazario.Identity.Application/Bazario.Identity.Application.csproj", "Bazario.Identity.Application/"]
COPY ["Bazario.Identity/src/Bazario.Identity.Domain/Bazario.Identity.Domain.csproj", "Bazario.Identity.Domain/"]

COPY Bazario.AspNetCore.Shared/NuGetPackages /nuget-packages

RUN dotnet nuget add source /nuget-packages --name NuGetPackages

RUN dotnet restore "./Bazario.Identity.WebAPI/Bazario.Identity.WebAPI.csproj"
COPY Bazario.Identity/src/ .
WORKDIR "/src/Bazario.Identity.WebAPI"
RUN dotnet build "./Bazario.Identity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazario.Identity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazario.Identity.WebAPI.dll"]