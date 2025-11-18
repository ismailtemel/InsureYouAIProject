using InsureYouAIProject.Context;
using InsureYouAIProject.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAIProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly InsureContext _context;

        public BlogController(InsureContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.page = page;
            return View();
        }

        [HttpGet]
        public IActionResult GetBlog(string keyword)
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BlogDetail(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.AppUserId = "e1cfffe2-6c82-49fd-9218-bcc06003b42d"; //userId değiştirilecek

            using (var client = new HttpClient())
            {
                //anahtar ezildi
                var apiKey = "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";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                try
                {
                    var translateRequestBody = new
                    {
                        inputs = comment.CommentDetail
                    };

                    var translateJson = JsonSerializer.Serialize(translateRequestBody);
                    var tranlateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

                    var translateResponse = await client.PostAsync("https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-tr-en", tranlateContent);
                    var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

                    string englishText = comment.CommentDetail;
                    if (translateResponseString.TrimStart().StartsWith("["))
                    {
                        var translateDoc = JsonDocument.Parse(translateResponseString);
                        englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString()!;
                    }
                    //ViewBag.englishText = englishText;

                    var toxicRequestBody = new
                    {
                        inputs = englishText
                    };

                    var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
                    var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
                    var toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
                    var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

                    if (toxicResponseString.TrimStart().StartsWith("["))
                    {
                        var toxicDoc = JsonDocument.Parse(toxicResponseString);
                        foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                        {
                            string label = item.GetProperty("label").GetString()!;
                            double score = item.GetProperty("score").GetDouble();
                            if (score >= 0.5)
                            {
                                comment.CommentStatus = "Toksik Yorum";
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(comment.CommentStatus))
                    {
                        comment.CommentStatus = "Yorum Onaylandı";
                    }
                }
                catch (Exception ex)
                {
                    comment.CommentStatus = "Onay Bekliyor";
                    throw;
                }
            }


            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("BlogDetail", "Blog", new { id = comment.ArticleId });
        }
    }
}
