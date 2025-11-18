using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly InsureContext _context;

        public PricingPlanController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.PricingPlans.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreatePricingPlan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePricingPlan(PricingPlan pricingPlan)
        {
            pricingPlan.IsFeature = false;

            await _context.PricingPlans.AddAsync(pricingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePricingPlan(int id)
        {
            var result = await _context.PricingPlans.FindAsync(id);
            _context.PricingPlans.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePricingPlan(int id)
        {
            var value = await _context.PricingPlans.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Update(pricingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
