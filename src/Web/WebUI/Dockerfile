FROM nginx:alpine AS base
EXPOSE 80

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
WORKDIR "/src/src/Web/WebUI"

FROM build AS publish
RUN dotnet publish --no-restore "WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY src/Web/WebUI/nginx.conf /etc/nginx/nginx.conf