using CodeChallenge.BuildingBlocks.EventBus.Events;

namespace CodeChallenge.Services.Transactions.Api.IntegrationEvents
{
    public interface ITransactionIntegrationEventService
    {
        void PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
