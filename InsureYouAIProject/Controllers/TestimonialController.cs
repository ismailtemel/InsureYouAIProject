using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InsureYouAIProject.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly InsureContext _context;

        public TestimonialController(InsureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _context.Testimonials.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(Testimonial testimonial)
        {

            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var result = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var value = await _context.Testimonials.FindAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateTestimonialWithClaudeAI()
        {
            //anahtar ezildi
            string apiKey = "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";
            string prompt = "Bir sigorta şirketi için müşteri deneyimlerine dair yorum oluşturmak istiyorum yani İngilizce karşılığı ile Testimonial. Bu alanda 6 adet yorum, 6 adet müşteri adı ve soyadı, bu müşterilerin unvanı olsun. Buna göre içeiği hazırla. ";

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.anthropic.com/");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-3-5-sonnet-20241022",
                max_tokens = 512,
                temperature = 0.5,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody));
            var response = await client.PostAsync("v1/messages", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.testimonials = new List<string>
                {
                    $"Claued Api'den cevap alınamadı. Hata: {response.StatusCode}"
                };
                return View();
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);

            var fullText = doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString();

            var services = fullText.Split('\n').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart('1', '2', '3', '4', '5', '.')).ToList();
            ViewBag.testimonials = services;

            return View();
        }
    }
}
