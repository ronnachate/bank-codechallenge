using CodeChallenge.DataObjects;

namespace CodeChallenge.Services.Transactions.Api.Models
{
    public class NewTransactionResponse : BaseResponse<NewTransactionResponse>
    {
        public string TransactionNumber { get; set; }
    }
}
