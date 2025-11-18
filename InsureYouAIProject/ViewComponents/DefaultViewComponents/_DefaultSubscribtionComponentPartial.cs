using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSubscribtionComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
