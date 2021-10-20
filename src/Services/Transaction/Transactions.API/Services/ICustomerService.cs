using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Services.Transactions.Api.Services
{
    public interface ICustomerService
    {
        Task<CustomerAccount> GetAccountByNumberAsync(string accountNumber);
    }
}
