using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult SendChatWithAI()
        {
            return View();
        }
    }
}
