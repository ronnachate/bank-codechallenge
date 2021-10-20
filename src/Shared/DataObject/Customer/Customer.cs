namespace CodeChallenge.DataObjects.Customers
{
    public class Customer : EntityWithTime
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public decimal CurrentBalance { get; set; }

    }
}
