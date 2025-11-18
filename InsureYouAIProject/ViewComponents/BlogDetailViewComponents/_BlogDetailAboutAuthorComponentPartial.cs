using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailAboutAuthorComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
