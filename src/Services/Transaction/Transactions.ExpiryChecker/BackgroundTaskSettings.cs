namespace WeDev.Payment.Services.Transactions.ExpiryChecker
{
    public class BackgroundTaskSettings
    {
        public string MongoConnectionString { get; set; }

        public int ExpiryDurationInMinute { get; set; }
    }
}
