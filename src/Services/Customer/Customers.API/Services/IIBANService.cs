using System.Threading.Tasks;

namespace CodeChallenge.Services.Customers.Api.Services
{
    public interface IIBANService
    {
        string GetRandomIBANAsync();
    }
}
