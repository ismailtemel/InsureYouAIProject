using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultAboutComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultAboutComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.aboutTitle = _context.Abouts.Select(x => x.Title).FirstOrDefault();
            ViewBag.aboutDescription = _context.Abouts.Select(x => x.Description).FirstOrDefault();
            ViewBag.aboutImageUrl = _context.Abouts.Select(x => x.ImageUrl).FirstOrDefault();

            var values = await _context.AboutItems.ToListAsync();

            return View(values);
        }
    }
}
