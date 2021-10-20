using CodeChallenge.BuildingBlocks.EventBus.Events;

namespace CodeChallenge.DataObjects.Events
{
    public record CustomerBalanceUpdatedEvent : IntegrationEvent
    {
        public int CustomerId { get; set; }

        public int TypeId { get; set; }

        public decimal Amount { get; set; }


        public CustomerBalanceUpdatedEvent(
            int customerId,
            int typeId,
            decimal amount
        )
        {
            CustomerId = customerId;
            TypeId = typeId;
            Amount = amount;
        }
    }
}
