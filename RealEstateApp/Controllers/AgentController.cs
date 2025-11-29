using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace RealEstateApp.Controllers
{
    public class AgentController : Controller
    {
        [Authorize(Roles = "Agent")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
