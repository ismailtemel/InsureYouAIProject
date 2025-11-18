using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.BlogViewComponents
{
    public class _BlogListCategoriesComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListCategoriesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Categories.ToListAsync();
            return View(values);
        }
    }
}
