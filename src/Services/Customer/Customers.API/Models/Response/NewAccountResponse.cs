using CodeChallenge.DataObjects;

namespace CodeChallenge.Services.Customers.Api.Models
{
    public class NewAccountResponse : BaseResponse<NewAccountResponse>
    {
        public string AccountNumber { get; set; }
    }
}
