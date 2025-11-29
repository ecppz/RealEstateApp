using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace RealEstateApp.Controllers
{
    public class CustomerController : Controller
    {
        [Authorize(Roles = "Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
