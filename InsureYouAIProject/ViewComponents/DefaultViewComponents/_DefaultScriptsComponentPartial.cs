using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultScriptsComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
