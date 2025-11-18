using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
