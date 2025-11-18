using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultMessageComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
