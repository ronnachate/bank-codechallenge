using CodeChallenge.BuildingBlocks.EventBus.Abstractions;
using CodeChallenge.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Services.Transactions.Api.IntegrationEvents
{
    public class TransactionIntegrationEventService : ITransactionIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<TransactionIntegrationEventService> _logger;

        public TransactionIntegrationEventService(
            ILogger<TransactionIntegrationEventService> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                _eventBus.Publish(evt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
            }
        }
    }
}
