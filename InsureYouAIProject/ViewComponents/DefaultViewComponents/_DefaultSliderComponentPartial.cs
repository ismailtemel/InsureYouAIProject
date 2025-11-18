using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSliderComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultSliderComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Sliders.ToListAsync();
            return View(values);
        }
    }
}
