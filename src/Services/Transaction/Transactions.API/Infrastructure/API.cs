using System;
namespace CodeChallenge.Services.Transactions.Api.Infrastructure
{
    public static class API
    {
        public static class Customer
        {

            public static string GetAccountByNumber(string baseUri, string accountNumber)
            {
                return $"{baseUri}account/{accountNumber}";
            }
        }
    }
}
