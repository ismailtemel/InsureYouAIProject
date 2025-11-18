using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeaderContactComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultHeaderContactComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.email = await _context.Contacts.Select(x => x.Email).FirstOrDefaultAsync();
            ViewBag.phone = await _context.Contacts.Select(x => x.PhoneNumber).FirstOrDefaultAsync();
            return View();
        }
    }
}
