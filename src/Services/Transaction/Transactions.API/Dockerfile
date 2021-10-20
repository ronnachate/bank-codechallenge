FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "CodeChallenge.sln" "CodeChallenge.sln"

COPY "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj" "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj"
COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj" "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj"

COPY "Services/Customer/Customers.API/Customers.API.csproj" "Services/Customer/Customers.API/Customers.API.csproj"
COPY "Services/Transaction/Transactions.API/Transactions.API.csproj" "Services/Transaction/Transactions.API/Transactions.API.csproj"

COPY "Tests/Services/Application.FunctionalTests/Application.FunctionalTests.csproj" "Tests/Services/Application.FunctionalTests/Application.FunctionalTests.csproj"

COPY "Shared/DataObject/CodeChallenge.DataObject.csproj" "Shared/DataObject/CodeChallenge.DataObject.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "CodeChallenge.sln"

COPY . .
WORKDIR /src/Services/Transaction/Transactions.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Transactions.API.dll"]
