using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultLast3ArticleComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultLast3ArticleComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Articles.Include(x => x.AppUser).OrderByDescending(x => x.ArticleId).Include(y => y.Category).Take(3).ToListAsync();
            return View(values);
        }
    }
}
