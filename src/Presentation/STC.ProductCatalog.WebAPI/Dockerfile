FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/STC.ProductCatalog.WebAPI/STC.ProductCatalog.WebAPI.csproj", "src/Presentation/STC.ProductCatalog.WebAPI/"]
COPY ["src/Core/STC.ProductCatalog.Domain/STC.ProductCatalog.Domain.csproj", "src/Core/STC.ProductCatalog.Domain/"]
COPY ["src/Core/STC.ProductCatalog.Application/STC.ProductCatalog.Application.csproj", "src/Core/STC.ProductCatalog.Application/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Utilities.Response/STC.Shared.Utilities.Response.csproj", "STC.Shared/src/Utilities/STC.Shared.Utilities.Response/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Utilities.Pagination/STC.Shared.Utilities.Pagination.csproj", "STC.Shared/src/Utilities/STC.Shared.Utilities.Pagination/"]
COPY ["src/Infrastructure/STC.ProductCatalog.Infrastructure/STC.ProductCatalog.Infrastructure.csproj", "src/Infrastructure/STC.ProductCatalog.Infrastructure/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Logger/STC.Shared.Logger.csproj", "STC.Shared/src/Utilities/STC.Shared.Logger/"]
COPY ["src/Infrastructure/STC.ProductCatalog.Persistence.MongoDb/STC.ProductCatalog.Persistence.MongoDb.csproj", "src/Infrastructure/STC.ProductCatalog.Persistence.MongoDb/"]
COPY ["STC.Shared/src/Utilities/STC.Shared.Auth.Consumer/STC.Shared.Auth.Consumer.csproj", "STC.Shared/src/Utilities/STC.Shared.Auth.Consumer/"]
COPY ["STC.Shared/src/Contracts/STC.Shared.Contracts.Auth/STC.Shared.Contracts.Auth.csproj", "STC.Shared/src/Contracts/STC.Shared.Contracts.Auth/"]
RUN dotnet restore "src/Presentation/STC.ProductCatalog.WebAPI/STC.ProductCatalog.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/STC.ProductCatalog.WebAPI"
RUN dotnet build "STC.ProductCatalog.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "STC.ProductCatalog.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p wwwroot
ENTRYPOINT ["dotnet", "STC.ProductCatalog.WebAPI.dll"]
