#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pandora.API/Pandora.API.csproj", "Pandora.API/"]
COPY ["Pandora.Application/Pandora.Application.csproj", "Pandora.Application/"]
COPY ["Pandora.Core/Pandora.Core.csproj", "Pandora.Core/"]
COPY ["Pandora.CrossCuttingConcerns/Pandora.CrossCuttingConcerns.csproj", "Pandora.CrossCuttingConcerns/"]
COPY ["Pandora.Infrastructure/Pandora.Infrastructure.csproj", "Pandora.Infrastructure/"]
COPY ["Pandora.Shared/Pandora.Shared.csproj", "Pandora.Shared/"]
RUN dotnet restore "./Pandora.API/Pandora.API.csproj"
COPY . .
WORKDIR "/src/Pandora.API"
RUN dotnet build "./Pandora.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pandora.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pandora.API.dll"]