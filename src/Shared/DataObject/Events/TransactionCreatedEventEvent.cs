using CodeChallenge.BuildingBlocks.EventBus.Events;

namespace CodeChallenge.DataObjects.Events
{
    public record TransactionCreatedEvent : IntegrationEvent
    {
        public string AccountNumber { get; set; }

        public int TypeId { get; set; }

        public decimal Amount { get; set; }

        public string RecieverAccountNumber { get; set; }


        public TransactionCreatedEvent(
            int typeId,
            decimal amount,
            string accountNumber,
            string recieverAccountNumber
        )
        {
            AccountNumber = accountNumber;
            TypeId = typeId;
            Amount = amount;
            RecieverAccountNumber = recieverAccountNumber;
        }
    }
}
