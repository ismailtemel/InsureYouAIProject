using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.BlogViewComponents
{
    public class _BlogListLast3RecentPostComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListLast3RecentPostComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Articles.OrderByDescending(x => x.ArticleId).Take(3).ToListAsync();
            return View(values);
        }
    }
}
