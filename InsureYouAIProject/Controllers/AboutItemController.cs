using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace InsureYouAIProject.Controllers
{
    public class AboutItemController : Controller
    {
        private readonly InsureContext _context;

        public AboutItemController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.AboutItems.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAboutItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAboutItem(AboutItem aboutItem)
        {
            await _context.AboutItems.AddAsync(aboutItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAboutItem(int id)
        {
            var result = await _context.AboutItems.FindAsync(id);
            _context.AboutItems.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAboutItem(int id)
        {
            var value = await _context.AboutItems.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Update(aboutItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAboutItemWithGoogleGemini()
        {
            //anahtar ezildi
            var apiKey = "AIzaSyCWnN59rffSSTsv_fH68Y8xazAudMGKyUE";
            var model = "gemini-1.5-pro";
            var url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkım alanları (about item)' yazısı oluştur. Örenğin : 'Geleceğnizi güvence altına alan kapsamlı sigorta çözümleri sunuyoruz' şeklinde veya bunun gibi ve buna benzer daha zengin içerikler gelsin, en az 10 tane item istiyorum."}
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseJson);
            var aboutText = jsonDoc
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            // Satır bazlı ayır

            var items = aboutText.Split(new[] { '\n', '-', '•' }, StringSplitOptions.RemoveEmptyEntries)

                                 .Select(x => x.Trim())

                                 .Where(x => !string.IsNullOrWhiteSpace(x))

                                 .ToList();

            ViewBag.value = items;

            return View();
        }
    }
}
