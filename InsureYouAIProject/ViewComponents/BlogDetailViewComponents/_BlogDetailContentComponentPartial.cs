using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailContentComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailContentComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var value = await _context.Articles.Include(x => x.AppUser).Include(y => y.Category).Where(x => x.ArticleId == id).FirstOrDefaultAsync();

            return View(value);
        }
    }
}

