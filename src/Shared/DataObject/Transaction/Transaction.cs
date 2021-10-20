namespace CodeChallenge.DataObjects.Transactions
{
    public class Transaction : EntityWithTime
    {
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }

        public TransactionType TransactionType { get; set; }

        public string TransactionNumber { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public int? RecieverId { get; set; }

        public string RecieverName { get; set; }


        public decimal GetNetAmout()
        {
            return Amount - Fee;
        }
    }
}
