using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterLast2ArticleComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultFooterLast2ArticleComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Articles.OrderByDescending(x => x.ArticleId).Skip(3).Take(2).ToListAsync();
            return View(values);
        }
    }
}
