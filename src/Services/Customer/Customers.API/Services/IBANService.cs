using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;

namespace CodeChallenge.Services.Customers.Api.Services
{
    public class IBANService : IIBANService
    {
        private HttpClient _httpClient;
        private readonly string _requestUrl;

        public IBANService(HttpClient httpClient, IOptions<CustomerSettings> settings)
        {
            _httpClient = httpClient;

            _requestUrl = settings.Value.IBANReqestUrl;
        }

        public async Task<string> GetRandomIBANAsync()
        {
            //var responseString = await _httpClient.GetStringAsync(_requestUrl);


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_requestUrl);
            httpWebRequest.Method = "GET";

            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;

        }
    }
}
