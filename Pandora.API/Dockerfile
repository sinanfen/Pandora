FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pandora.API/Pandora.API.csproj", "Pandora.API/"]
RUN dotnet restore "./Pandora.API/Pandora.API.csproj"
COPY . .
WORKDIR "/src/Pandora.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://*:$PORT
ENV ConnectionStrings__PandoraBoxDatabase=${DATABASE_URL}

ENTRYPOINT ["dotnet", "Pandora.API.dll"]
