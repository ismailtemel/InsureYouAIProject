using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using MimeKit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace InsureYouAIProject.Controllers
{
    public class DefaultController : Controller
    {
        private readonly InsureContext _context;

        public DefaultController(InsureContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            message.PhoneNumber = "string";
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            #region Claude_AI_Analiz

            //anahtar ezildi
            string apiKey = "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";
            string prompt = $"Sen bir sigorta firmasının müşteri iletişim asistanısın. Kurumsal ama samimi, net ve anlaşılır bir dille yaz. Yanıtların 2-3 paragrafla sınırla. Eksik bilgi (poliçe numarası, kimlik, vb.) varsa kibarca talep et. Fiyat, ödeme, teminat gibi kritik konularsa kesin rakam verme, müşteri temsilcisine yönlendir. Hasar ve sağlık gibi hassas durumlarda empati kur. Cevaplarını teşekkür ve iyi dilekle bitir. Ayrıca cevabın için 1 cümlelik özet bir e-posta konu başlığı üret. Cevabı JSON formatında şu şekilde ver:  {{\r\n  \"\"subject\"\": \"\"...\"\",\r\n  \"\"body\"\": \"\"...\"\"\r\n}}. Kullanıcının sana gönderdiği mesaj şu şekilde: '{message.MessageDetail}'.";

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.anthropic.com/");
            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-4-opus-20250514",
                max_tokens = 1000,
                temperature = 0.5,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("v1/messages", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            var json = JsonNode.Parse(responseString);
            string? textContent = json?["content"]?[0]?["text"]?.ToString();
            //ViewBag.textcontent = textContent;



            #endregion

            #region E-Posta_Gönderme
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("InsureYouAI Admin", "denememardin0147@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", message.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            if (!string.IsNullOrEmpty(textContent))
            {
                string clean = textContent.Trim()
                         .Replace("\r", "")
                         .Replace("\n", " ");
                var subjectObj = JsonNode.Parse(clean);
                string? subjectLine = subjectObj?["subject"]?.ToString();
                string? bodyText = subjectObj?["body"]?.ToString();

                mimeMessage.Subject = subjectLine ?? "AI'dan gelen yanıt";
                bodyBuilder.TextBody = bodyText ?? textContent;
            }
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("denememardin0147@gmail.com", "qgbphhcmkkchurjq");
            client.Send(mimeMessage);
            client.Disconnect(true);
            #endregion

            #region Claude_AI_Mesaj_Kaydetme
            if (!string.IsNullOrEmpty(textContent))
            {
                string clean = textContent.Trim()
                         .Replace("\r", "")
                         .Replace("\n", " ");
                var subjectObj = JsonNode.Parse(clean);
                string? subjectLine = subjectObj?["subject"]?.ToString();
                string? bodyText = subjectObj?["body"]?.ToString();
                ClaudeAIMessage claudeAIMessage = new ClaudeAIMessage()
                {
                    MessageDetail = bodyText,
                    SendDate = DateTime.Now,
                    ReceiveEmail = message.Email,
                    ReceiveNameSurname = message.NameSurname
                };
                await _context.ClaudeAIMessages.AddAsync(claudeAIMessage);
                await _context.SaveChangesAsync();
            }
            #endregion

            await Task.Delay(5000);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeEmail(string email)
        {
            return RedirectToAction("Index");
        }
    }
}
