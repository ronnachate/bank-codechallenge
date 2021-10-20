using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Services.Transactions.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
