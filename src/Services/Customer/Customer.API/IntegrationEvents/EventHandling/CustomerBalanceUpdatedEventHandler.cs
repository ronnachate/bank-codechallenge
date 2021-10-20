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
    public class CustomerBalanceUpdatedEventHandler : IIntegrationEventHandler<CustomerBalanceUpdatedEvent>
    {
        private readonly CustomerContext _customerContext;
        private readonly ILogger<CustomerBalanceUpdatedEventHandler> _logger;

        public CustomerBalanceUpdatedEventHandler(
            CustomerContext customerContext,
            ILogger<CustomerBalanceUpdatedEventHandler> logger)
        {
            _customerContext = customerContext;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CustomerBalanceUpdatedEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                try
                {
                    var customer = await _customerContext.Customers
                        .SingleOrDefaultAsync(c => c.Id == @event.CustomerId);
                    if(customer != null)
                    {
                        
                    }
                    else
                    {
                        _logger.LogError($"[{Program.AppName}] ERROR update customer balance no customer found: {@event.CustomerId}");
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