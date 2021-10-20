using System;
namespace CodeChallenge.Services.Transactions.Api.Models
{
    public class NewTransactionRequest
    {
        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public string AccountNumber { get; set; }

        public string RecieverAccountNumber { get; set; }
    }
}
