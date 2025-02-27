#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG NET_IMAGE=8.0
FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

# Create this "restore-solution" section by running ./Create-DockerfileSolutionRestore.ps1, to optimize build cache reuse
COPY ["src/ApiGateways/Mobile.Api.Gateway/Mobile.Api.Gateway.csproj", "src/ApiGateways/Mobile.Api.Gateway/"]
COPY ["src/ApiGateways/Web.Api.Gateway/Web.Api.Gateway.csproj", "src/ApiGateways/Web.Api.Gateway/"]
COPY ["src/Web/WebUI/WebUI.csproj", "src/Web/WebUI/"]
COPY ["src/BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj", "src/BuildingBlocks/Devspaces.Support/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Tests/EventBus.Tests.csproj", "src/BuildingBlocks/EventBus/EventBus.Tests/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "src/BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/Observability/Observability.csproj", "src/BuildingBlocks/Observability/"]
COPY ["src/BuildingBlocks/Shared.Kernel/Shared.Kernel.csproj", "src/BuildingBlocks/Shared.Kernel/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
COPY ["src/Services/Company/Company.API/Company.API.csproj", "src/Services/Company/Company.API/"]
COPY ["src/Services/Company/Company.FunctionalTests/Company.FunctionalTests.csproj", "src/Services/Company/Company.FunctionalTests/"]
COPY ["src/Services/Product/Product.API/Product.API.csproj", "src/Services/Product/Product.API/"]
COPY ["src/Services/Product/Product.FunctionalTests/Product.FunctionalTests.csproj", "src/Services/Product/Product.FunctionalTests/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["Awc.Containerized.sln", "./"]

RUN dotnet restore "Awc.Containerized.sln"

COPY . .
WORKDIR "/src/src/Services/Company/Company.API"

FROM build AS publish
RUN dotnet publish --no-restore "Company.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Company.API.dll"]