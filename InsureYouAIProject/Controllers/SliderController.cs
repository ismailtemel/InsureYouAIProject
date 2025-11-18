using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.Controllers
{
    public class SliderController : Controller
    {
        private readonly InsureContext _context;

        public SliderController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.Sliders.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlider(Slider slider)
        {

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSlider(int id)
        {
            var result = await _context.Sliders.FindAsync(id);
            _context.Sliders.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSlider(int id)
        {
            var value = await _context.Sliders.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSlider(Slider slider)
        {
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
