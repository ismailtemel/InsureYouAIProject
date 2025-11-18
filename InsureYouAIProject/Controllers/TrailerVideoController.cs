using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.Controllers
{
    public class TrailerVideoController : Controller
    {
        private readonly InsureContext _context;

        public TrailerVideoController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.TrailerVideos.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateTrailerVideo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrailerVideo(TrailerVideo trailerVideo)
        {

            await _context.TrailerVideos.AddAsync(trailerVideo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTrailerVideo(int id)
        {
            var result = await _context.TrailerVideos.FindAsync(id);
            _context.TrailerVideos.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTrailerVideo(int id)
        {
            var value = await _context.TrailerVideos.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrailerVideo(TrailerVideo trailerVideo)
        {
            _context.TrailerVideos.Update(trailerVideo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
