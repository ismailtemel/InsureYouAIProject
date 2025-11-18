using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.BlogDetailViewComponents
{
    public class _BlogDetailCommentListComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailCommentListComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var values = await _context.Comments.Where(x => x.ArticleId == id && x.CommentStatus == "Yorum Onaylandı").Include(y => y.AppUser).ToListAsync();
            return View(values);
        }
    }
}
