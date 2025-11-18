using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutNavbarMenuComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
