using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutBreadCrumbComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
