using InsureYouAIProject.Context;
using InsureYouAIProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubCharts1ComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubCharts1ComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var policyData = await _context.Policies.GroupBy(p => p.PolicyType)
                .Select(g => new PolicyTypeCountViewModel
                {
                    PolicyType = g.Key,
                    Count = g.Count()
                }).ToListAsync();

            ViewBag.PolicyCounts = policyData.Sum(x => x.Count);
            ViewBag.PolicyData = JsonConvert.SerializeObject(policyData);

            return View();
        }
    }
}
