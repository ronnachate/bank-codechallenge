FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "WeDev.Payment.sln" "WeDev.Payment.sln"

COPY "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj" "BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj"
COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj" "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj"
COPY "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj" "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "Services/Admin/Admin.API/Admin.API.csproj" "Services/Admin/Admin.API/Admin.API.csproj"
COPY "Services/Notification/SMS.API/SMS.API.csproj" "Services/Notification/SMS.API/SMS.API.csproj"
COPY "Services/Agent/Agents.API/Agents.API.csproj" "Services/Agent/Agents.API/Agents.API.csproj"
COPY "Services/Merchant/Merchants.API/Merchants.API.csproj" "Services/Merchant/Merchants.API/Merchants.API.csproj"
COPY "Services/Configuration/Configuration.API/Configuration.API.csproj" "Services/Configuration/Configuration.API/Configuration.API.csproj"
COPY "Services/Transaction/Transactions.API/Transactions.API.csproj" "Services/Transaction/Transactions.API/Transactions.API.csproj"
COPY "Services/Transaction/Transactions.ExpiryChecker/Transactions.ExpiryChecker.csproj" "Services/Transaction/Transactions.ExpiryChecker/Transactions.ExpiryChecker.csproj"
COPY "AgentIntegrations/Ksher/Ksher.InquiryChecker/Ksher.InquiryChecker.csproj" "AgentIntegrations/Ksher/Ksher.InquiryChecker/Ksher.InquiryChecker.csproj"
COPY "Services/Notification/Email.API/Email.API.csproj" "Services/Notification/Email.API/Email.API.csproj"
COPY "Tests/Services/Application.FunctionalTests/Application.FunctionalTests.csproj" "Tests/Services/Application.FunctionalTests/Application.FunctionalTests.csproj"

COPY "Web/WebStatus/WebStatus.csproj" "Web/WebStatus/WebStatus.csproj"
COPY "Web/WebAdmin/WebAdmin.csproj" "Web/WebAdmin/WebAdmin.csproj"
COPY "Web/WebMerchant/WebMerchant.csproj" "Web/WebMerchant/WebMerchant.csproj"

COPY "Public/Payment.Api/Payment.Api.csproj" "Public/Payment.Api/Payment.Api.csproj"

COPY "Shared/DataObject/WeDev.Payment.DataObject.csproj" "Shared/DataObject/WeDev.Payment.DataObject.csproj"
COPY "Shared/BusinessLogic/WeDev.Payment.BusinessLogic.csproj" "Shared/BusinessLogic/WeDev.Payment.BusinessLogic.csproj"
COPY "Shared/Util/WeDev.Payment.Util.csproj" "Shared/Util/WeDev.Payment.Util.csproj"

#Agent integration
COPY "AgentIntegrations/TwoCTwoP.Api/TwoCTwoP.Api.csproj" "AgentIntegrations/TwoCTwoP.Api/TwoCTwoP.Api.csproj"
COPY "AgentIntegrations/Ksher/Ksher.Api/Ksher.Api.csproj" "AgentIntegrations/Ksher/Ksher.Api/Ksher.Api.csproj"
COPY "AgentIntegrations/Ksher/Ksher.Payment/Ksher.Payment.csproj" "AgentIntegrations/Ksher/Ksher.Payment/Ksher.Payment.csproj"
COPY "AgentIntegrations/Ksher/Ksher.DataObject/Ksher.DataObject.csproj" "AgentIntegrations/Ksher/Ksher.DataObject/Ksher.DataObject.csproj"
COPY "AgentIntegrations/Ksher/Ksher.BusinessLogic/Ksher.BusinessLogic.csproj" "AgentIntegrations/Ksher/Ksher.BusinessLogic/Ksher.BusinessLogic.csproj"
COPY "AgentIntegrations/Ksher/Ksher.InquiryChecker/Ksher.InquiryChecker.csproj" "AgentIntegrations/Ksher/Ksher.InquiryChecker/Ksher.InquiryChecker.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "WeDev.Payment.sln"

COPY . .
WORKDIR /src/Services/Transaction/Transactions.ExpiryChecker
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Transactions.ExpiryChecker.dll"]
