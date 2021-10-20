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
        Deposit = 1,
        [Description("Withdraw")]
        Withdraw = 2,
        [Description("Transfer")]
        Transfer = 3,
    }
}
