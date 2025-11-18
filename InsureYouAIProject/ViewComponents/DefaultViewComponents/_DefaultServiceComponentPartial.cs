using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultServiceComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultServiceComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Services.ToListAsync();
            return View(values);
        }
    }
}
