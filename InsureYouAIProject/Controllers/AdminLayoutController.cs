using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
