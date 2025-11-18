using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.BlogViewComponents
{
    public class _BlogListBreadCrumbComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
