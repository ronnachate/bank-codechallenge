namespace CodeChallenge.DataObjects.Transactions
{
    public class Transaction : EntityWithTime
    {
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }

        public TransactionType TransactionType { get; set; }

        public string TransactionNumber { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public string RecieverAccountNumber { get; set; }

        public string RecieverAccountName { get; set; }

        public decimal GetNetAmout()
        {
            return Amount - Fee;
        }
    }
}
