using CodeChallenge.BuildingBlocks.EventBus.Events;

namespace CodeChallenge.Services.Customers.Api.IntegrationEvents
{
    public interface ICustomerIntegrationEventService
    {
        void PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
