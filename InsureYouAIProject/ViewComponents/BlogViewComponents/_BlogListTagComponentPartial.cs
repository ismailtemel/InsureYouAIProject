using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.BlogViewComponents
{
    public class _BlogListTagComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
