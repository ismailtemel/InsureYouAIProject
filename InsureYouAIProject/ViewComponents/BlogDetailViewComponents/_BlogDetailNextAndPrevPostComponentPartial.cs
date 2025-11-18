using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailNextAndPrevPostComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
