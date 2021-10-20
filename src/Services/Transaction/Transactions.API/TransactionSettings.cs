namespace CodeChallenge.Services.Transactions.Api
{
    public class TransactionSettings
    {
        public decimal PercentageOfFeeCharged { get; set; }

        public string ConnectionString { get; set; }

        public string GatewayUrl { get; set; }
    }
}
