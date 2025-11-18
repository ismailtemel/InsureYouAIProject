using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardRadialChartComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardRadialChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.policyCount = await _context.Policies.CountAsync();
            ViewBag.policyCountIsActive = await _context.Policies.Where(x => x.Status == "Active").CountAsync();

            double percentage = 0;
            if (ViewBag.policyCount > 0)
                ViewBag.percentage = (double)ViewBag.policyCountIsActive / ViewBag.policyCount * 100;
            return View();
        }
    }
}
