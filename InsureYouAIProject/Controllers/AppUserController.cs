using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAIProject.Controllers
{
    public class AppUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly InsureContext _context;

        public AppUserController(UserManager<AppUser> userManager, InsureContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return View(values);
        }

        public async Task<IActionResult> UserProfileWithAI(string id)
        {
            var values = await _userManager.FindByIdAsync(id);
            ViewBag.name = values.Name;
            ViewBag.surname = values.Surame;
            ViewBag.imageUrl = values.ImageUrl;
            ViewBag.description = values.Description;
            ViewBag.userTitle = values.Title;
            ViewBag.city = values.City;
            ViewBag.education = values.Education;

            //kullanıcı bilgileri çeklidi
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //kullanıcıya ait makele listesi
            var articles = await _context.Articles.Where(a => a.AppUserId == id).Select(y => y.Content).ToListAsync();
            if (articles.Count == 0)
            {
                ViewBag.AIResult = "Bu kullanıcıya ait analiz yapılacak makale bulunamadı!";
                return View(user);
            }

            //makaleleri tek bir metinde topla
            var allarticles = string.Join("\n\n", articles);

            //anahtar ezildi
            var apiKey = "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";

            //prompt'un yazılması
            var prompt = $@"Siz bir sigorta sektöründe uzman bi içerik analistisin. Elinizde bir sigorta şirketinin çalışnının yazdığı tüm makeleler var. Sonra bu makaleler üzerinden çalışanın içerik üretim tarzını analiz et. Analiz başlıkları: 1. Konu çeşitliliği ve odak alanları(Sağlık, hayat, kasko, tamamlayıcı, BES vb.), 2. Hedef kitle tahmini (bireysel/kurumsal, segment, persona), 3. Dil ve anlatım tarzı (tekniklik seviyesi, okunabilirlik, ve ikna gücü), 4. Sigorta terimlerini kullanma ve doğruluk düzeyi, 5. Müşteri ihtiyaçlarına ve risk yönetimine odaklanma, 6. Pazarlama/satış vurgusu, CTA netliği, 7. geliştirilmesi gereken alanlar ve net aksiyon maddeleri. Makeleler: {allarticles} Lüfen çıktıyı profesyonel rapor formatında ve en sonda 5 maddelik aksiyon listesi ile ver.";

            //open ai chat completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Sen sigorta sektöründe içerik analizi yapan bir uzmansın." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //json dönüşümleri
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var respText = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                ViewBag.AIResult = "OpenAI API Hatası: " + respText;
                return View(user);
            }

            //json yapı içinden veriyi okuma
            try
            {
                using var doc = JsonDocument.Parse(respText);
                var aiText = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                ViewBag.AIResult = aiText ?? "Boş yanıt döndü!";
            }
            catch
            {
                ViewBag.AIResult = "OpenAI yanıtı beklenen formatta değil.";
                throw;
            }

            return View(user);
        }

        public async Task<IActionResult> UserCommentsProfileWithAI(string id)
        {
            var values = await _userManager.FindByIdAsync(id);
            ViewBag.name = values.Name;
            ViewBag.surname = values.Surame;
            ViewBag.imageUrl = values.ImageUrl;
            ViewBag.description = values.Description;
            ViewBag.userTitle = values.Title;
            ViewBag.city = values.City;
            ViewBag.education = values.Education;

            //kullanıcı bilgileri çeklidi
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //kullanıcıya ait makele listesi
            var comments = await _context.Comments.Where(a => a.AppUserId == id).Select(y => y.CommentDetail).ToListAsync();
            if (comments.Count == 0)
            {
                ViewBag.AIResult = "Bu kullanıcıya ait analiz yapılacak yorum bulunamadı!";
                return View(user);
            }

            //makaleleri tek bir metinde topla
            var allComments = string.Join("\n\n", comments);

            //anahtar ezildi
            var apiKey = "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";

            //prompt'un yazılması
            var prompt = $@"Sen kullanıcı davranış analizi yapan bir yapay zeka uzmanısın. Kullanıcı yorumlarına göre kullanıcıyı değerlendir. Analiz Başlıkları: 1. Genel Duygu Durumu (Pozitif/negatif/nötr), 2. Toksik içerik var mı? (örnekleriyle), 3. İlgi alanları/konu başlıkları, 4. İletişim tarzı(samimi, resmi, negatif vb.), 5. Geliştirmesi gereken iletişim alanları, 6. 5 maddelik kısa özet. Yorumlar: {allComments}";

            //open ai chat completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Sen kullanıcı yorum analizi yapan bir uzmansın." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //json dönüşümleri
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var respText = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                ViewBag.AIResult = "OpenAI API Hatası: " + respText;
                return View(user);
            }

            //json yapı içinden veriyi okuma
            try
            {
                using var doc = JsonDocument.Parse(respText);
                var aiText = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                ViewBag.AIResult = aiText ?? "Boş yanıt döndü!";
            }
            catch
            {
                ViewBag.AIResult = "OpenAI yanıtı beklenen formatta değil.";
                throw;
            }

            return View(user);
        }
    }
}
