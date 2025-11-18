using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultPricingPlanComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultPricingPlanComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var values = await _context.PricingPlans.Where(x => x.IsFEature == true).ToListAsync();
            var pricinPlan1 = await _context.PricingPlans.Where(x => x.IsFeature == true).FirstOrDefaultAsync();
            ViewBag.pricinPlan1Title = pricinPlan1.Title;
            ViewBag.pricinPlan1Price = pricinPlan1.Price;
            ViewBag.pricinPlan1Id = pricinPlan1.PricingPlanId;

            var pricingPlan2 = await _context.PricingPlans.Where(x => x.IsFeature == true).OrderByDescending(y => y.PricingPlanId).FirstOrDefaultAsync();
            ViewBag.pricinPlan2Title = pricingPlan2.Title;
            ViewBag.pricinPlan2Price = pricingPlan2.Price;
            ViewBag.pricinPlan2Id = pricingPlan2.PricingPlanId;

            var pricingPlanItems = await _context.PricingPlanItems.Where(x => x.PricingPlanId == pricinPlan1.PricingPlanId || x.PricingPlanId == pricingPlan2.PricingPlanId).ToListAsync();
            return View(pricingPlanItems);
        }
    }
}
