using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultFooterComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.description = await _context.Contacts.Select(x => x.Description).FirstOrDefaultAsync();
            ViewBag.phone = await _context.Contacts.Select(x => x.PhoneNumber).FirstOrDefaultAsync();
            ViewBag.email = await _context.Contacts.Select(x => x.Email).FirstOrDefaultAsync();
            ViewBag.address = await _context.Contacts.Select(x => x.Address).FirstOrDefaultAsync();

            return View();
        }
    }
}
