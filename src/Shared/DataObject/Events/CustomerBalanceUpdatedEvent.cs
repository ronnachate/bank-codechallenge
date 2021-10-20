using CodeChallenge.BuildingBlocks.EventBus.Events;

namespace CodeChallenge.DataObjects.Events
{
    public record CustomerBalanceUpdatedEvent : IntegrationEvent
    {
        public string AccountNumber { get; set; }

        public int TypeId { get; set; }

        public decimal Amount { get; set; }


        public CustomerBalanceUpdatedEvent(
            string accountNumber,
            int typeId,
            decimal amount
        )
        {
            AccountNumber = accountNumber;
            TypeId = typeId;
            Amount = amount;
        }
    }
}
