using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultInsureServiceComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
