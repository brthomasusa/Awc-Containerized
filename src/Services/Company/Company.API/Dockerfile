#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG NET_IMAGE=8.0
FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

# Create this "restore-solution" section by running ./Create-DockerfileSolutionRestore.ps1, to optimize build cache reuse
COPY ["src/ApiGateways/Aggregators/Web.Shopping.HttpAggregator/Web.Shopping.HttpAggregator.csproj", "src/ApiGateways/Aggregators/Web.Shopping.HttpAggregator/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/Healthchecks/Healthchecks.csproj", "src/BuildingBlocks/Healthchecks/"]
COPY ["src/BuildingBlocks/Shared.Kernel/Shared.Kernel.csproj", "src/BuildingBlocks/Shared.Kernel/"]
COPY ["src/Services/Company/Company.API/Company.API.csproj", "src/Services/Company/Company.API/"]
COPY ["src/Services/Product/Product.API/Product.API.csproj", "src/Services/Product/Product.API/"]
COPY ["test/Company.API.IntegrationTests/Company.API.IntegrationTests.csproj", "test/Company.API.IntegrationTests/"]
COPY ["NuGet.config", "./"]
COPY ["AWC.Dapr.sln", "./"]

RUN dotnet restore "AWC.Dapr.sln"

COPY . .
WORKDIR "/src/src/Services/Company/Company.API"

FROM build AS publish
RUN dotnet publish --no-restore "Company.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.API.dll"]
