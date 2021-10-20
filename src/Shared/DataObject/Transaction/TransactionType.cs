using System.ComponentModel;

namespace CodeChallenge.DataObjects.Transactions
{
    public class TransactionType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public enum TransactionTypeEnum
    {
        [Description("Deposit")]
        NoAction = 1,
        [Description("Withdraw")]
        Pending = 2,
        [Description("Transfer")]
        Partial = 3,
    }
}
