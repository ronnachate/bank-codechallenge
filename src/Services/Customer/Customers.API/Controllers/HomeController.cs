using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Services.Customers.Api.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
