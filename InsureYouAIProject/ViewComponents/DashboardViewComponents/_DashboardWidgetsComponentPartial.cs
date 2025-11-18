using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardWidgetsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardWidgetsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int n1, n2, n3, n4;
            int r1, r2, r3, r4;
            Random random = new Random();
            r1 = random.Next(0, 10);
            n1 = random.Next(1, 30);
            r2 = random.Next(0, 10);
            n2 = random.Next(1, 30);
            r3 = random.Next(0, 10);
            n3 = random.Next(1, 30);
            r4 = random.Next(0, 10);
            n4 = random.Next(1, 30);


            ViewBag.articleCount = await _context.Articles.CountAsync();
            ViewBag.categoryCount = await _context.Categories.CountAsync();
            ViewBag.personCount = await _context.Users.CountAsync();
            ViewBag.commentCount = await _context.Comments.CountAsync();

            ViewBag.r1 = n1 + "." + r1;
            ViewBag.r2 = n2 + "." + r2;
            ViewBag.r3 = n3 + "." + r3;
            ViewBag.r4 = n4 + "." + r4;

            return View();
        }
    }
}
