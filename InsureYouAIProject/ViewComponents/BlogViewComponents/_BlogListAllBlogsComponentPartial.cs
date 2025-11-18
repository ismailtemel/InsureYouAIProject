using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace InsureYouAIProject.ViewComponents.BlogViewComponents
{
    public class _BlogListAllBlogsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListAllBlogsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page)
        {
            var values = await _context.Articles.Include(x => x.Category).Include(y => y.AppUser).ToListAsync();
            return View(values.ToPagedList(page, 3));
        }
    }
}
