using CodeChallenge.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Serilog.Context;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.DataObjects.Events;
using CodeChallenge.Services.Customers.Api.Infrastructure;

namespace CodeChallenge.Services.Customers.Api.IntegrationEvents.EventHandling
{
    public class TransactionCreatedEventHandler : IIntegrationEventHandler<TransactionCreatedEvent>
    {
        private readonly CustomerContext _customerContext;
        private readonly ILogger<TransactionCreatedEventHandler> _logger;

        public TransactionCreatedEventHandler(
            CustomerContext customerContext,
            ILogger<TransactionCreatedEventHandler> logger)
        {
            _customerContext = customerContext;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(TransactionCreatedEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                try
                {
                    var account = await _customerContext.CustomerAccounts
                        .SingleOrDefaultAsync(c => c.AccountNumber == @event.AccountNumber);
                    if(account != null)
                    {
                        
                    }
                    else
                    {
                        _logger.LogError($"[{Program.AppName}] ERROR update customer balance no customer found: {@event.AccountNumber}");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[{AppName}] ERROR update customer balance: {Exception}", Program.AppName, ex);
                }
            }
        }
    }
}