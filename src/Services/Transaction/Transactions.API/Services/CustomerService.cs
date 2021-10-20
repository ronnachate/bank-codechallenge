using CodeChallenge.DataObjects;
using CodeChallenge.DataObjects.Customers;
using CodeChallenge.Services.Transactions.Api.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge.Services.Transactions.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public CustomerService(HttpClient httpClient, IOptions<TransactionSettings> settings)
        {
            _httpClient = httpClient;

            _remoteServiceBaseUrl = $"{settings.Value.GatewayUrl}/m/api/v1/merchant/";
        }

        public async Task<CustomerAccount> GetAccountByNumberAsync(string accountNumber)
        {
            var uri = API.Customer.GetAccountByNumber(_remoteServiceBaseUrl, accountNumber);
            var responseString = await _httpClient.GetStringAsync(uri);
            var result = JsonConvert.DeserializeObject<CustomerAccount>(responseString);
            return result;
        }
    }
}
