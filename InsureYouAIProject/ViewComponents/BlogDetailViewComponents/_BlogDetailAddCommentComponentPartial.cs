using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailAddCommentComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
