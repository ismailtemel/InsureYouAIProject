using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAIProject.Controllers
{
    public class ImageAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageAIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult CreateImageWithOpenAI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateImageWithOpenAI(string prompt)
        {
            //anahtar ezildi
            var apiKey = "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";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestData = new
            {
                prompt = prompt,
                n = 1,
                size = "512x512"
            };

            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.error = "OpenAI Hatası: " + await response.Content.ReadAsStringAsync();
                return View();
            }
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(json);
            var imageUrl = result.RootElement.GetProperty("data")[0].GetProperty("url").GetString();

            return View(model: imageUrl);
        }
    }
}
