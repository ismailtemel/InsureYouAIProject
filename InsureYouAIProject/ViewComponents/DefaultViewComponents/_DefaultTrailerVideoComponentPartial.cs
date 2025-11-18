using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultTrailerVideoComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultTrailerVideoComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.TrailerVideos.FirstOrDefaultAsync();
            return View(values);
        }
    }
}
