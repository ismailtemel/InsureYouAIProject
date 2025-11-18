using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.Controllers
{
    public class MessageController : Controller
    {
        private readonly InsureContext _context;

        public MessageController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.Messages.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            message.IsRead = false;
            message.SendDate = DateTime.UtcNow;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var result = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var value = await _context.Messages.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
