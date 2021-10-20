namespace CodeChallenge.DataObjects.Customers
{
    public class CustomerAccount : EntityWithTime
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public decimal CurrentBalance { get; set; }

    }
}
