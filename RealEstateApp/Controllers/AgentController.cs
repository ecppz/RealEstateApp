using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
