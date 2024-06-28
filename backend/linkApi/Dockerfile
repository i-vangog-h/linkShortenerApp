FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5010
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["linkApi/linkApi.csproj", "linkApi/"]
COPY ["DataContextPostgres/DataContextPostgres.csproj", "DataContextPostgres/"]
RUN dotnet restore "./linkApi/linkApi.csproj"
COPY . .
WORKDIR "/src/linkApi"
RUN dotnet build "./linkApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./linkApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "linkApi.dll"]