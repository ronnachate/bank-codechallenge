using System;
namespace CodeChallenge.Services.Transactions.Api.Models
{
    public static class TransactionHelper
    {
        public static decimal GetTransactionFee(decimal amount, decimal percentageOfChanged)
        {
            return (amount * percentageOfChanged) / 100;
        }
    }
}
