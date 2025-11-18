using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubChartsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubChartsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
